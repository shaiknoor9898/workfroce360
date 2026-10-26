# Decisions

Record meaningful project decisions here before implementation where practical. Each entry should include date, context, options, choice, reason, tradeoff, and a condition for revisiting it.

## D-001: Establish repository AI collaboration docs

- Date: 2026-10-16
- Context: The maintainer asked to implement the practices in the attached AI collaboration guide.
- Options: (1) add only a general instruction file; (2) add an instruction file plus project-specific handover, architecture, flow, safety, and verification notes; (3) create a full operational policy claiming details not verified in the repository.
- Chosen: (2).
- Reason: It makes the workflow persistent while grounding technical notes in the active API source and marking unknowns explicitly.
- Tradeoff: Documentation requires maintenance and some security sections remain initial inventories pending maintainer review.
- Revisit if: The active project moves, the API gains services or deployment infrastructure, or the team adopts another documentation structure.

## D-002: Bound Employee API list responses and validate duplicate writes in the application

- Date: 2026-10-16
- Context: The Employee API needed search, pagination, duplicate prevention, and stronger write validation without an approved database migration.
- Options: (1) return every employee as an array; (2) paginate through HTTP headers while keeping the array response; (3) return a JSON page object with result and count metadata.
- Chosen: (3), with a maximum page size of 100. The API checks duplicate employee code and email before create, PUT, and PATCH operations, and SQL Server enforces the same rule with unique indexes.
- Reason: A page object gives a frontend the information needed to render navigation without loading the entire employee table. The database indexes make the duplicate rule reliable during concurrent writes, while application checks provide a user-friendly 409 response.
- Tradeoff: Existing list clients must read `items` instead of a raw array. Employee code and email are now limited to 450 SQL characters so SQL Server can index them.
- Revisit if: Product requirements change the definition of a unique employee identifier or require scoped uniqueness by organization.

## D-003: Keep the EF model aligned with the employee schema and persist application users

- Date: 2026-10-21
- Context: Development startup failed because the EF model included `AppUser` while the latest migration snapshot did not. EF also warned that the employee salary's SQL precision was implicit.
- Options: (1) suppress EF's pending-model warning or remove the user entity from the context; (2) add a migration for the actual user model and explicitly configure salary precision to match the existing `decimal(18,2)` schema.
- Chosen: (2). Migration `AddAppUsers` creates `Users` with the configured email, password hash, role, and UTC creation timestamp fields and a unique email index. Employee salary precision is explicitly configured as `(18,2)`.
- Reason: The runtime context already exposes `Users`; a migration records that schema rather than hiding the mismatch. Explicit precision preserves the existing schema and removes the ambiguity noted by EF.
- Tradeoff: Applying the migration creates a new table and index in the target database. It is present in the approved local database.
- Revisit if: The user schema or salary storage requirements change, or the migration is reviewed against the target database.

## D-004: Use local JWT authentication with role-gated employee writes

- Date: 2026-10-24
- Context: The Employee API needed an authentication boundary before more modules are added.
- Options: (1) leave endpoints anonymous; (2) add public registration; (3) initialize one administrator once, then let only an Admin create accounts and issue local JWTs after login.
- Chosen: (3). Passwords use PBKDF2-SHA512 hashes. JWTs last 60 minutes, validate issuer, audience, signature, and expiry, and carry the account role. Any signed-in role can read employees; Admin and HR can create, replace, patch, and delete employees.
- Reason: It protects the API while avoiding open registration. The one-time setup lets the project bootstrap its first administrator without a hard-coded account.
- Tradeoff: There is no password reset, refresh token, token revocation, or record-level authorization yet. A trusted operator must initialize the first administrator.
- Revisit if: The project gains an identity provider, a web frontend, multi-organization access controls, or account-recovery requirements.

## D-005: Begin automated coverage with isolated xUnit tests

- Date: 2026-10-26
- Context: The API had no automated test project for employee and authentication logic.
- Options: (1) rely on Swagger requests; (2) add database-backed integration tests first; (3) add focused unit tests that run without SQL Server, then add integration coverage separately.
- Chosen: (3), using xUnit in `WorkForce360.API.Tests`.
- Reason: Isolated tests give quick feedback for password hashing, JWT creation, role rules, and validation without changing the local database or depending on a running API.
- Tradeoff: These tests do not prove routing, authorization middleware, or EF Core persistence behavior.
- Revisit if: A test database or in-memory integration test strategy is approved.
