# Docker setup — NursingHome

Runs the ASP.NET Core API together with SQL Server 2022 in one command.

## Layout

| Path | Purpose |
|------|---------|
| `docker/api/Dockerfile` | Multi-stage, non-root image for `NursingHome.Api` (.NET 8). |
| `docker/db/init.sh` | One-shot DB initialiser: creates the database, a least-privilege app login, then applies `database/scripts/*.sql` and `database/seeds/*.sql`. |
| `docker-compose.yml` | Orchestrates `db` + `db-init` + `api` (at the solution root). |
| `.env.example` | Template for secrets — copy to `.env`. |
| `.env` | **Local secrets, git-ignored.** Never commit. |

## First run

```bash
cd NursingHome
cp .env.example .env      # then edit .env and set strong passwords
docker compose up --build
```

The API comes up on <http://localhost:8088> (change with `API_PORT` in `.env`).
In `Development` the Swagger UI is at `/swagger` (JSON at `/swagger/v1/swagger.json`).

Startup order is enforced automatically:
`db` (healthy) → `db-init` (runs once, exits) → `api`.

## Commands

```bash
docker compose up --build       # build + start
docker compose down             # stop, keep the database volume
docker compose down -v          # stop and DELETE the database (fresh start)
docker compose logs -f api      # follow API logs
```

## Adding the schema / seed later

Drop your files in:

- `database/scripts/database.sql`  → schema (tables, views, procs…)
- `database/seeds/seed.sql`        → seed data

Then recreate so the initialiser re-runs against a clean database:

```bash
docker compose down -v && docker compose up --build
```

Multiple `*.sql` files per folder are supported and applied in filename order,
so you can prefix them (`010_tables.sql`, `020_indexes.sql`, …).

## Security notes

- **Secrets** live only in `.env` (git-ignored) and are injected as environment
  variables — nothing is baked into the image (`.dockerignore` blocks `.env`,
  `appsettings.Development.json`, certs, etc.).
- The **API connects with a least-privilege login** (`nursinghome_app`), never
  with `sa`. `sa` is used only by the throwaway `db-init` container.
- **SQL Server is not published to the host** by default (internal network
  only). Uncomment the `ports:` block in `docker-compose.yml` if you need SSMS
  access — it binds to `127.0.0.1` only.
- The API runs as a **non-root user** on an unprivileged port (8080).
- `TrustServerCertificate=True` is used for SQL Server's self-signed dev
  certificate. Replace it with a real certificate for non-local environments.
