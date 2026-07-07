#!/usr/bin/env bash
# =============================================================================
# Database initialiser for the NursingHome SQL Server container.
#
# Runs once at `docker compose up` from a throwaway `db-init` container:
#   1. waits for SQL Server to accept connections,
#   2. creates the application database (if missing),
#   3. creates a least-privilege application login (the API never uses 'sa'),
#   4. applies schema files from database/scripts/*.sql, then database/seeds/*.sql,
#   5. writes a marker so the schema/seed step is skipped on later start-ups.
#
# The .sql files are optional: with none present the container still provisions
# the empty database + login and exits successfully. Drop database.sql into
# database/scripts/ and seed.sql into database/seeds/ later, then recreate the
# stack (`docker compose down -v && docker compose up`) to apply them.
# =============================================================================
set -euo pipefail

SQLCMD="/opt/mssql-tools/bin/sqlcmd"
SERVER="db"
SA_USER="sa"
SA_PASSWORD="${MSSQL_SA_PASSWORD:?MSSQL_SA_PASSWORD is required}"
DB_NAME="${DB_NAME:-NursingHome}"
DB_APP_USER="${DB_APP_USER:-nursinghome_app}"
DB_APP_PASSWORD="${DB_APP_PASSWORD:-}"

SCRIPTS_DIR="/database/scripts"
SEEDS_DIR="/database/seeds"

# -C trusts the server's self-signed certificate; -b makes sqlcmd return a
# non-zero exit code on any SQL error so a bad script fails the init container.
sa_sql() {
  local db="$1"; shift
  if [[ -n "$db" ]]; then
    "$SQLCMD" -S "$SERVER" -U "$SA_USER" -P "$SA_PASSWORD" -C -b -d "$db" "$@"
  else
    "$SQLCMD" -S "$SERVER" -U "$SA_USER" -P "$SA_PASSWORD" -C -b "$@"
  fi
}

echo "==> Waiting for SQL Server at '$SERVER' to accept connections..."
for i in $(seq 1 60); do
  if "$SQLCMD" -S "$SERVER" -U "$SA_USER" -P "$SA_PASSWORD" -C -b -Q "SELECT 1" >/dev/null 2>&1; then
    echo "==> SQL Server is ready."
    break
  fi
  if [[ "$i" -eq 60 ]]; then
    echo "!! SQL Server did not become ready within ~120s." >&2
    exit 1
  fi
  sleep 2
done

echo "==> Ensuring database [$DB_NAME] exists..."
sa_sql "" -Q "IF DB_ID(N'$DB_NAME') IS NULL CREATE DATABASE [$DB_NAME];"

# ---- Least-privilege application account (never expose 'sa' to the app) ------
if [[ -n "$DB_APP_PASSWORD" ]]; then
  echo "==> Ensuring application login [$DB_APP_USER] exists..."
  sa_sql "" -Q "IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = N'$DB_APP_USER')
                  CREATE LOGIN [$DB_APP_USER] WITH PASSWORD = N'$DB_APP_PASSWORD', CHECK_POLICY = ON;"
  sa_sql "$DB_NAME" -Q "IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'$DB_APP_USER')
                          CREATE USER [$DB_APP_USER] FOR LOGIN [$DB_APP_USER];
                        ALTER ROLE db_owner ADD MEMBER [$DB_APP_USER];"
else
  echo "!! DB_APP_PASSWORD not set; skipping application login (the API would have to use 'sa')." >&2
fi

# ---- Idempotency guard -------------------------------------------------------
# The data volume persists between runs, so re-running schema/seed would fail on
# duplicate objects. Skip once a marker table is present.
MARKER="$(sa_sql "$DB_NAME" -h -1 -W -Q "SET NOCOUNT ON; IF OBJECT_ID(N'dbo.__docker_init_marker', N'U') IS NULL SELECT 'missing' ELSE SELECT 'present';" | tr -d '[:space:]')"
if [[ "$MARKER" == "present" ]]; then
  echo "==> Init marker found; schema/seed already applied. Nothing to do."
  exit 0
fi

APPLIED=0
apply_dir() {
  local dir="$1"; local label="$2"
  if [[ ! -d "$dir" ]]; then
    echo "==> Directory $dir not found; skipping $label."
    return 0
  fi
  shopt -s nullglob
  local files=("$dir"/*.sql)
  shopt -u nullglob
  if [[ ${#files[@]} -eq 0 ]]; then
    echo "==> No $label files in $dir yet."
    return 0
  fi
  IFS=$'\n' files=($(sort <<<"${files[*]}")); unset IFS
  for f in "${files[@]}"; do
    echo "==> Applying $label: $(basename "$f")"
    sa_sql "$DB_NAME" -i "$f"
    APPLIED=$((APPLIED + 1))
  done
}

apply_dir "$SCRIPTS_DIR" "schema script"
apply_dir "$SEEDS_DIR" "seed script"

if [[ "$APPLIED" -gt 0 ]]; then
  echo "==> Writing init marker..."
  sa_sql "$DB_NAME" -Q "IF OBJECT_ID(N'dbo.__docker_init_marker', N'U') IS NULL
                          CREATE TABLE dbo.__docker_init_marker (applied_at DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME());"
  echo "==> Database initialization complete ($APPLIED file(s) applied)."
else
  echo "==> No .sql files applied. Add database/scripts/database.sql and database/seeds/seed.sql, then run: docker compose down -v && docker compose up"
fi
