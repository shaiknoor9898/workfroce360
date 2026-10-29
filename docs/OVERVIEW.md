# WorkForce360 project overview

## What it is

WorkForce360 is an ASP.NET Core Web API for managing employee records and user accounts. The active application is in `WorkForce360.API/` and targets .NET 10. The current repository documentation does not identify a frontend or background worker in the active application.

## Main capabilities

- Create, retrieve, update, partially update, and delete employee records.
- List employees with optional department filtering, text search, and bounded pagination.
- Authenticate users with JWT bearer tokens.
- Initialize the first administrator while no user accounts exist.
- Allow administrators to create user accounts with supported roles: Admin, HR, Manager, Employee, and Recruiter.
- Validate request data and reject duplicate employee codes, employee emails, and user emails.

## Access rules

All employee endpoints require authentication. Any authenticated user can read employee records; Admin and HR users can create, update, or delete them. The API does not currently enforce record-level or department-level read access.

## Technical architecture

- **API and HTTP pipeline:** ASP.NET Core controllers, configured in `WorkForce360.API/Program.cs`.
- **Persistence:** Entity Framework Core with SQL Server and versioned migrations.
- **Authentication:** JWT token creation and PBKDF2 password hashing are implemented in `WorkForce360.API/Services/`.
- **Data contracts:** Request, patch, response, and paging DTOs are in `WorkForce360.API/DTOs/`.
- **Automated tests:** `WorkForce360.API.Tests/` contains xUnit unit tests for password hashing, JWT contents, roles, and request validation.

## Employee data

Employee records include an employee code, name, email, phone, joining date, department, designation, employment type, status, salary, and address. Salary is stored as a decimal with precision `(18,2)`. Employee code and email are protected by unique database indexes. These fields include sensitive personal information and should not be copied into public examples, logs, or test fixtures.

## Limitations and operational notes

- Password reset, account recovery, token refresh, and token revocation are not implemented.
- Every signed-in role can currently read all employee records.
- Development startup applies pending database migrations. Use only a confirmed local or test database unless applying migrations elsewhere has been explicitly approved.
- HTTPS redirection is currently disabled in the API startup code.
- The repository documents dependency advisories and other production-readiness questions; this overview does not constitute a security or deployment review.

## Further reading

- [Architecture](./ARCHITECTURE.md) — components and trust boundaries.
- [Request flow](./FLOW.md) — authentication, employee endpoints, and startup behavior.
- [Data](./DATA.md) — data structures and access patterns.
- [Testing](./TESTING.md) — automated checks and safe manual API verification.
- [Threat notes](./THREAT.md) — initial risk inventory and open security questions.
- [Constraints](./CONSTRAINTS.md) — repository-specific safety and change constraints.
