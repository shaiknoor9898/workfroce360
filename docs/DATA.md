# Data structures

## Employee entity

`WorkForce360.API/Models/Employee.cs` defines an `Employee` row with integer `Id`, employee code, names, email, phone, joining date, department, designation, employment type, status, decimal salary, and address. Salary is stored as `decimal(18,2)`.

`WorkForce360.API/Models/AppUser.cs` defines application users with an email, password hash, role, and creation timestamp. `WorkForce360DbContext` exposes the `Employees` and `Users` sets. EF Core migrations describe the SQL Server schema; migration `AddAppUsers` creates the `Users` table and enforces unique user email. `RestoreMissingUsersTable` restores that table and index if the database records the original migration as applied but the objects are missing.

## Access patterns

- List: optional exact department match, case-insensitive text search, ordered by `Id`, and returned as bounded pages of up to 100 results.
- Single record: lookup by integer ID.
- Writes: create, full replacement via PUT, field updates via PATCH, and delete.
- DTOs separate request and response payloads from the persistence entity.

## Tradeoffs and unknowns

- Integer identity keys are simple for local lookup; public identifier exposure and indexing policy are not documented.
- Department filtering, text search, and ordering are straightforward, but indexes and expected table size are unknown.
- Employee code and email are checked by the API and enforced by unique SQL Server indexes. This prevents duplicate records, including concurrent writes.
- Salary, contact, and address fields are sensitive. Retention, encryption, audit, and access policy need owner confirmation.
- Exact SQL types, lengths, nullability, and uniqueness constraints should be confirmed from the migrations and current database before schema changes.
