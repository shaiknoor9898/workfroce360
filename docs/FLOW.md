# Request flow

## Employee create example

```text
HTTP client
  -> Bearer JWT validation and role authorization
  -> ASP.NET Core routing/model binding
  -> EmployeeController.CreateEmployee(EmployeeRequestDto)
  -> ApplyRequest(Employee, request)
  -> WorkForce360DbContext.Employees.Add
  -> SaveChangesAsync
  -> ToResponse(Employee)
  -> HTTP 201 with location from GetEmployeeById
```

## Other endpoint paths

- `POST /api/auth/initialize-admin`: anonymous only while no accounts exist; creates the first Admin and returns a JWT.
- `POST /api/auth/login`: anonymous; validates an account password and returns a JWT.
- `POST /api/auth/users`: Admin only; creates an account with one of the supported roles.
- `GET /api/employee`: any signed-in user; optional exact department filter plus case-insensitive text search across employee code, name, email, department, and designation. The response is paged with `items`, page metadata, and a total count; page size is limited to 100.
- `GET /api/employee/{id}`: no-tracking lookup; returns 404 when absent.
- `PUT`, `PATCH`, and `DELETE /api/employee/{id}`: Admin or HR only. They load tracked rows, validate and normalize input, reject duplicate code or email with 409, then save. Missing IDs return 404.

## Startup

`Program.cs` registers controllers and `WorkForce360DbContext` with SQL Server. It reads the JWT signing key from configuration, validates issuer, audience, signature, and expiry, then enables authentication and authorization. In Development, it applies pending EF Core migrations at startup and enables Swagger/OpenAPI.

Update this trace when endpoint routing, persistence flow, or startup behavior changes.
