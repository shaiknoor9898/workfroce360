# Verification checklist

## Automated checks

The active project is `WorkForce360.API/WorkForce360.API.csproj`. A build can be run from the repository root:

```powershell
dotnet build .\WorkForce360.API\WorkForce360.API.csproj
```

Expected result: command exits with code 0 and reports no build errors. This is a build check, not a substitute for behavioral tests.

Run the unit tests from the repository root:

```powershell
dotnet test .\WorkForce360.API.Tests\WorkForce360.API.Tests.csproj
```

The xUnit project does not connect to SQL Server. It verifies password hashing, JWT content, role normalization, and request validation. Controller and database behavior remain manual or future integration-test coverage.

## Manual API check

Use a local Development instance and the requests in `WorkForce360.API/WorkForce360.API.http` (review before sending). Do not use real employee data.

1. Confirm an unauthenticated employee request returns `401`.
2. While no accounts exist, call `POST /api/auth/initialize-admin` with an administrator email and a password that meets the documented password rules. Save the returned token.
3. Send the token as `Authorization: Bearer <token>` and confirm a list request returns `200`.
4. Use the Admin token to create an HR and an Employee account. Sign in as each.
5. Confirm the HR account can create or patch an employee; confirm the Employee account receives `403` for the same write request.
6. Confirm a second initialize-admin attempt returns `409`, invalid login returns `401`, and duplicate user email returns `409`.
7. Verify the existing employee CRUD, paging, search, duplicate, validation, and not-found cases using an Admin or HR token.

Do not run these requests against shared or production data without explicit authorization.

## Before a customer-facing release

- Add and run automated tests for validation, authorization, persistence, and error paths.
- Confirm authentication and authorization are implemented and exercised.
- Review the full diff and inspect configuration for credentials.
- Record the actual command output and any manual path exercised in the task handover.
