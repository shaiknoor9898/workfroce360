# Dependency and secrets log

## Direct project packages

Source: `WorkForce360.API/WorkForce360.API.csproj`.

| Package | Version | Visible purpose |
| --- | --- | --- |
| `Microsoft.AspNetCore.OpenApi` | 10.0.12 | OpenAPI support. |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 10.0.12 | Official Microsoft JWT bearer authentication middleware. |
| `Microsoft.EntityFrameworkCore.Design` | 10.0.12 | EF Core design-time tooling. |
| `Microsoft.EntityFrameworkCore.SqlServer` | 10.0.12 | SQL Server provider for EF Core. |
| `Microsoft.EntityFrameworkCore.Tools` | 10.0.12 | EF Core tooling. |
| `Swashbuckle.AspNetCore` | 10.0.1 | Swagger generation/UI. |

NuGet reports `NU1902` and `NU1903` advisories for transitive `System.Security.Cryptography.Pkcs` 9.0.11. Review the dependency path and an available compatible update before production use.

## Secrets

No secret values belong in this file. The API reads `ConnectionStrings:DefaultConnection` and `Jwt:Key`; the local JWT key is stored with .NET User Secrets. Provide secrets through protected deployment configuration outside tracked files. Do not copy real connection strings or JWT keys into issues, logs, docs, or chat.

Before adding a package, record its purpose, version, maintainer/source, and reason existing dependencies are insufficient. Pin and review versions according to the repository's package management policy once established.
