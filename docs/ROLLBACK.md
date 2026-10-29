# Rollback

## Code changes

- Before a risky change, record the last known good commit or tag in the task handover. The repository had existing local modifications when this document was added, so do not use a broad reset or checkout to roll anything back.
- Prefer reverting only the specific change in question after reviewing its diff. Preserve unrelated staged, unstaged, and untracked work.

## Database changes

- Never apply a migration to a shared or production database without an explicit backup and reviewed rollback plan.
- EF Core migrations may have `Down` methods, but a down migration can still lose data. Inspect the exact migration and database state first.
- Prefer a tested forward-fix or restore plan when rollback could destroy employee data.
- `RestoreMissingUsersTable` intentionally has a no-op `Down`: the `Users` table belongs to the earlier `AddAppUsers` migration, so rolling back the repair must not delete user accounts.

## Recovery checks

After rollback, build the active API and verify the affected local/test request path. Record actual output and confirm the database schema is compatible before restarting a deployed service.
