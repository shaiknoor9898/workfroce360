# Threat notes

This is an initial inventory based on the active API source, not a completed security review.

| Actor / entry point | Plausible abuse | Visible control or gap |
| --- | --- | --- |
| Anonymous HTTP caller / employee endpoints | Read, create, alter, or delete employee records; submit oversized or malformed values. | Employee endpoints require a valid JWT. Reads are available to signed-in users; writes require Admin or HR. DTO validation bounds request fields. |
| Anonymous caller / first-admin endpoint | Attempt to take over a new deployment. | The endpoint works only while no user exists, with a serializable database transaction. Initialize it promptly on a trusted local or protected network. |
| Authenticated user | Access employees outside their role or business unit. | Role policy distinguishes employee writes, but all signed-in roles can read all employee records. Record-level or department-level access is not implemented. |
| Operator / configuration source | Expose SQL credentials or point the app at the wrong database. | Connection string comes from configuration; secret storage and deployment configuration are unknown. |
| Database/service dependency | Return errors, become unavailable, or expose employee data through overly broad permissions. | SQL Server is accessed through EF Core; deployment safeguards and database permissions are unknown. |

## Required review questions

- Do all signed-in roles need access to every employee record, or should access be scoped by department or organization?
- Are request sizes, string lengths, and query frequency bounded?
- Where are production secrets stored, rotated, and audited?
- What data is logged, and how are salary and contact fields kept out of logs?
- How will accounts be recovered, passwords reset, and compromised tokens revoked?

Do not represent this inventory as proof the API is secure.
