# Failure and resilience notes

Known failure points from the current implementation:

- **SQL Server unavailable or slow:** employee reads and writes depend on SQL Server through EF Core. No explicit retry, timeout policy, health check, or graceful degraded response is visible in the active project. Confirm provider defaults and hosting behavior before setting expectations.
- **Invalid or conflicting employee data:** request DTOs validate required fields, lengths, email, phone, status, salary, and joining-date bounds. The API returns 409 when it finds an existing employee code or email. SQL Server also enforces code and email uniqueness through unique indexes.
- **Large employee table:** the list endpoint uses a maximum page size of 100, but search and department filters may need database indexes as record counts grow.
- **Startup migration failure:** in Development, startup calls `Database.Migrate()`. A migration or database connection failure can prevent startup. EF Core does not rerun a migration recorded in `__EFMigrationsHistory`; if that migration's objects were later removed, a guarded forward migration is needed to restore them. Production migration policy is not documented.
- **Unauthorized access:** JWT authentication and role-gated employee writes are configured. Expired or invalid tokens return 401 and a valid token without the required role returns 403. Account recovery, token revocation, and record-level access are still missing.

## Detection gaps

Logging, metrics, alerting, health checks, service-level objectives, and production hosting are not documented or verified. Establish these before relying on the API for customer operations.
