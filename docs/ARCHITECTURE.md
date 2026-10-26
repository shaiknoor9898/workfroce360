# Architecture

## Current shape

The active application is a single ASP.NET Core Web API in `WorkForce360.API/`, targeting .NET 10. No separate frontend or worker is present in the active project tree inspected for this document.

| Component | Responsibility |
| --- | --- |
| `Program.cs` | Registers controllers, SQL Server EF Core context, JWT authentication, role authorization, and OpenAPI/Swagger; configures the HTTP pipeline. |
| `Controllers/AuthController.cs` | Initializes the first administrator, signs users in, and lets an administrator create accounts. |
| `Controllers/EmployeeController.cs` | Employee CRUD HTTP endpoints, request-to-model mapping, response DTO mapping, and role requirements. |
| `DTOs/` | Request, patch, and response API shapes. |
| `Models/` | `Employee`, application-user, and allowed-role entity definitions. |
| `Services/` | PBKDF2 password hashing and JWT token creation. |
| `Data/WorkForce360DbContext.cs` | EF Core `Employees` and `Users` sets and SQL Server persistence context. |
| `Migrations/` | EF Core schema history and model snapshot. |

## Data path and trust boundaries

External HTTP clients first obtain a JWT from the authentication endpoints. ASP.NET Core validates the token and role before employee actions. Model binding and DTO validation form the data boundary; the controller maps accepted values to EF Core entities; EF Core persists to SQL Server using `DefaultConnection`; response DTOs are returned to the caller.

The connection string is supplied through configuration. Keep credentials outside tracked files. Swagger and automatic migration execution are currently enabled only in Development according to `Program.cs`.

## Known limits

- The first administrator endpoint is intentionally usable only while the `Users` table is empty. Account recovery, password reset, token revocation, and refresh tokens are not implemented.
- No explicit service layer, rate limit, audit trail, or centralized exception handling is visible in the active project.
- Production hosting, database topology, traffic, and availability targets are unknown.

See `FLOW.md`, `DATA.md`, `THREAT.md`, and `FAILURE.md` for details and open questions.
