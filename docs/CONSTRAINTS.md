# Constraints

These rules are grounded in the current repository and should be confirmed by the maintainer before production use.

- Treat `WorkForce360.API/` as the active application. `WorkForce360.API copy/` and `workfroce360/` are ignored local duplicates; do not edit them unless asked.
- Never commit connection strings, passwords, tokens, certificates, or other secrets. Do not print secret values in summaries or logs.
- Do not run database migrations against a shared or production database without explicit authorization and a reviewed backup/rollback plan.
- Do not change authentication, authorization, deployment, infrastructure, or employee data retention behavior without explicit approval of the approach.
- Do not add packages or make outbound network calls without first explaining why and getting approval.
- Preserve all existing user changes. Do not reset, clean, or overwrite unrelated staged, unstaged, or untracked files.
- Do not remove tests, validation, or security controls to make a change pass.
- Treat employee records, including salary and contact details, as sensitive personal information. Avoid copying real records into fixtures, logs, examples, or documentation.
- JWT authentication and role-gated employee writes are configured. Do not describe the API as production-ready until HTTPS, account recovery, token revocation, dependency advisories, and access-scope requirements have been reviewed.

If a task appears to require breaking a constraint, stop and explain the conflict and the minimum decision needed.
