# HANDOVER

## Latest session: 2026-10-29 (publishing local updates)

### Done
- Committed the guarded `RestoreMissingUsersTable` migration and related documentation as local commit `0ec0a7e`.
- Ran `dotnet test .\WorkForce360.API.Tests\WorkForce360.API.Tests.csproj --no-restore`; all 13 tests passed.
- Kept the modified `WorkForce360.API/appsettings.json`, generated `bin/obj` deletions, and staged `workfroce360` deletion out of the commit.

### In progress
- Publishing the local branch `feature/employee-validation` is pending; both command-line and GitKraken push attempts failed because GitHub credentials are unavailable.

### Broken or risky
- The branch contains local commits not confirmed on the remote.
- `WorkForce360.API/appsettings.json` has an uncommitted connection-string change and must not be published without review.
- Existing staged generated-file and duplicate-entry deletions remain untouched.
- The test run reported existing NU1902 and NU1903 advisories for `System.Security.Cryptography.Pkcs` 9.0.11.

### Next
- Configure GitHub authentication, then push `feature/employee-validation`.
- Keep the connection-string change and generated/deletion changes excluded unless separately reviewed and approved.

### Verification
- `dotnet test .\WorkForce360.API.Tests\WorkForce360.API.Tests.csproj --no-restore` passed 13 tests with 0 failures.
- `git push origin HEAD:refs/heads/feature/employee-validation` failed because Git could not obtain a password; the GitKraken push also failed because it could not obtain a GitHub username in a non-interactive environment.

## Latest session: 2026-10-27 (database startup issue)

### Done
- Investigated the reported `Invalid object name 'Users'` failure in `AuthController.InitializeAdmin`.
- Confirmed migration `20261021101528_AddAppUsers` exists and creates `Users`; `Program.cs` applies migrations automatically only in Development.
- Confirmed a `WorkForce360.API.exe` from the active project's `bin` directory is listening on port 5055. The running process environment and resolved database target could not be verified without exposing or inspecting runtime secrets.
- Added and applied migration `20261027063440_RestoreMissingUsersTable` to the confirmed local `WorkForce360` database. It conditionally restores `dbo.Users` and its unique email index.

### In progress
- No application work remains in progress.

### Broken or risky
- A password value was included in the submitted request example. If it is a real password, rotate it.
- The running API endpoint was not replayed; retry first-admin initialization with a new strong password if the submitted password was real.

### Next
- Retry `POST /api/auth/initialize-admin` against the local API. The user table is now present and the earlier failed request did not create an account.

### Verification
- SQL Server confirmed `dbo.Users`, unique index `UX_Users_Email`, and migration history entry `20261027063440_RestoreMissingUsersTable`.
- Release build succeeded with existing NU1902/NU1903 advisories. All 13 unit tests passed, and EF reported no pending model changes.
- The live authentication request was not replayed to avoid using the password included in the submitted example.

## Latest session: 2026-10-27

### Done
- Added `docs/OVERVIEW.md` to summarize the active API's capabilities, architecture, access rules, employee data, limitations, and related documentation.

### In progress
- No application work was requested in this documentation-only session.

### Broken or risky
- Existing staged and unstaged work remains; preserve it.

### Next
- No follow-up is required for the project overview.

### Verification
- Documentation-only change; no tests were run.

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
- GitHub push is pending. Commit `541e39c` is local on `feature/employee-validation`; command-line and GitKraken pushes both failed because GitHub credentials are unavailable in this environment.

### Broken or risky
- The repository had pre-existing staged, unstaged, and untracked changes when this documentation work began. They were not part of this task and must be preserved.
- The existing staged deletion of the root `workfroce360` entry was left unchanged during duplicate-folder cleanup.
- The local commit excludes the modified `WorkForce360.API/appsettings.json` because it contains a connection-string field, and excludes pre-existing staged build-output and submodule deletions.
- NuGet reports NU1902 and NU1903 advisories for transitive `System.Security.Cryptography.Pkcs` 9.0.11. The dependency path and compatible update need review before production use.
- JWT tokens have no refresh, revocation, or password-reset flow. All signed-in accounts currently read all employee data.
- Development startup applies migrations automatically. Only run it with a confirmed local/test database unless applying migrations elsewhere has been explicitly approved.
- Deployment setup and production status have not been verified. HTTPS redirection is still disabled in the current API startup file.

### Next
- Configure GitHub authentication for Git, then push commit `541e39c` to `origin/feature/employee-validation`. Decide separately whether to publish the excluded configuration or staged deletions; do not include connection strings.
- Start the API, initialize the first administrator in Swagger or `WorkForce360.API.http`, authorize Swagger with the returned token, then perform the documented role checks.
- Add controller and database integration tests after an isolated test-database strategy is approved.
- Keep this file current at the end of future work sessions.

### Verification
- `dotnet test .\WorkForce360.API.Tests\WorkForce360.API.Tests.csproj --no-restore` completed on 2026-10-26 with 13 passing tests and 0 failures; NU1902/NU1903 advisories remain for transitive `System.Security.Cryptography.Pkcs` 9.0.11.
- `git push --set-upstream origin HEAD:refs/heads/feature/employee-validation` and the GitKraken push both failed because no GitHub credentials were available.
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
