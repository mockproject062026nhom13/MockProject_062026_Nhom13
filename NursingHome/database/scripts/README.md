# database/scripts

Put your **schema** here. Files matching `*.sql` are applied (in filename order)
against the `NursingHome` database by the `db-init` container on first start.

Expected file: `database.sql`

You can also split it, e.g. `010_tables.sql`, `020_views.sql`, `030_procs.sql`.
Seed data goes in `../seeds/` instead.
