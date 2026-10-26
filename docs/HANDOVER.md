# HANDOVER

## Latest session: 2026-10-26

### Done
- Resolved the EF pending-model-changes startup failure by explicitly configuring salary precision as `(18,2)` and generating migration `AddAppUsers` for the existing `AppUser` model. The migration was not applied to a database.
- Removed the ignored duplicate project folders `WorkForce360.API copy/` and `workfroce360/`. The active `WorkForce360.API/` project was preserved.
- Fixed the API build error by changing `Program.cs` to use the `Microsoft.OpenApi` namespace required by the resolved OpenAPI package.
- Added the repository AI collaboration guidance and project documentation scaffolding.
- Inspected the active `WorkForce360.API` project structure and request/data flow.
- Completed the Employee API module's bounded list endpoint, search, duplicate checks, and request validation. The list response now contains `items` and pagination metadata.
- Added and applied migration `20261016101105_AddUniqueEmployeeIdentifiers` to the approved local `WorkForce360` database. SQL Server now enforces unique employee code and email values.
- Added JWT authentication with local PBKDF2 password hashes, a one-time first-admin initialization endpoint, login, Admin-only account creation, and role authorization. Employee reads require a signed-in account; writes require Admin or HR.
- Added the JWT configuration metadata and stored the local development signing key in .NET User Secrets, outside tracked configuration.
- Reviewed the `AddAppUsers` SQL and confirmed the approved local database already has that migration applied. Removed a duplicate migration created during this session.
- Added `WorkForce360.API.Tests`, an xUnit project that tests password hashing, JWT token contents, supported roles, and data-annotation request validation without connecting to SQL Server.

### In progress
- No application work is currently in progress.

### Broken or risky
- The repository had pre-existing staged, unstaged, and untracked changes when this documentation work began. They were not part of this task and must be preserved.
- The existing staged deletion of the root `workfroce360` entry was left unchanged during duplicate-folder cleanup.
- NuGet reports NU1902 and NU1903 advisories for transitive `System.Security.Cryptography.Pkcs` 9.0.11. The dependency path and compatible update need review before production use.
- JWT tokens have no refresh, revocation, or password-reset flow. All signed-in accounts currently read all employee data.
- Development startup applies migrations automatically. Only run it with a confirmed local/test database unless applying migrations elsewhere has been explicitly approved.
- Deployment setup and production status have not been verified. HTTPS redirection is still disabled in the current API startup file.

### Next
- Start the API, initialize the first administrator in Swagger or `WorkForce360.API.http`, authorize Swagger with the returned token, then perform the documented role checks.
- Add controller and database integration tests after an isolated test-database strategy is approved.
- Keep this file current at the end of future work sessions.

### Verification
- `dotnet test .\WorkForce360.API.Tests\WorkForce360.API.Tests.csproj --no-restore --no-build` completed on 2026-10-26 with 13 passing tests and 0 failures. The test runner required unrestricted local execution because it hung during discovery in the restricted sandbox.
- `dotnet build` completed on 2026-10-24 with 0 errors. It reported two NU1902/NU1903 advisory warnings for transitive `System.Security.Cryptography.Pkcs` 9.0.11.
- `dotnet ef migrations script 20261016101105_AddUniqueEmployeeIdentifiers 20261021101528_AddAppUsers` was reviewed on 2026-10-24; it creates `Users` and the unique email index.
- `dotnet ef database update` completed on 2026-10-24 and reported the approved local database was already up to date.
- `dotnet ef migrations has-pending-model-changes --project .\WorkForce360.API\WorkForce360.API.csproj --no-build` reported no changes after generating `AddAppUsers`.
- The API build succeeded after migration generation with 0 errors and the same 3 package warnings. No database migration was applied and `dotnet run` was not repeated.
- `dotnet run` was not repeated because Development startup applies pending database migrations; the target database has not been confirmed safe for this task.
- `dotnet build .\WorkForce360.API\WorkForce360.API.csproj` completed on 2026-10-16 with 0 warnings and 0 errors.
- No live API requests were sent because the available database has not been confirmed as a local/test database with non-real employee data.
- `dotnet ef database update` applied migration `20261016101105_AddUniqueEmployeeIdentifiers` successfully to the approved local database.

### Do not touch
- Do not reset, clean, stage, or overwrite the pre-existing user changes without an explicit request.
- Do not modify the ignored duplicate project folders as part of active-project work.
- Do not apply migrations to a live or shared database without explicit authorization and a reviewed rollback plan.

Read first: `AGENTS.md`, `docs/CONSTRAINTS.md`, `docs/ARCHITECTURE.md`.
