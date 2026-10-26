# Assumptions and open questions

## Unverified assumptions

- `WorkForce360.API/` is the active codebase because it is the non-duplicate project at the repository root. Confirm with the maintainer.
- The API may be a prototype; no deployment or customer-use evidence was inspected. Do not infer it is safe to ship.
- The existing `WorkForce360.API copy/` and `workfroce360/` directories are local duplicates, and `.gitignore` excludes them intentionally.

## Open questions

- Is this API currently used by customers or connected to real employee data?
- What authentication, authorization, audit, retention, and privacy requirements apply?
- What is the production hosting and database migration process?
- What are the expected record counts, traffic, availability targets, and data recovery objectives?
- Which test database and manual API test path should the team use?

Mark each item verified, wrong, or still unverified as evidence arrives. Clear or consciously accept release-relevant unknowns before customer use.
