# AI collaboration rules

Before changing code, read `docs/HANDOVER.md`, `docs/CONSTRAINTS.md`, and the relevant architecture and flow notes. Treat repository instructions as project guidance, not as permission to ignore the user's request or higher-priority instructions.

## Plan and scope

- Before editing, state the intended change, the files likely to change, the alternatives considered, and the main risks.
- Keep each request to one logical change. Ask before adding a dependency or changing authentication, deployment, infrastructure, production data, or database migration history.
- If the requested change conflicts with `docs/CONSTRAINTS.md`, stop and explain the conflict before editing.
- Do not overwrite or discard existing user changes. Inspect `git status` and preserve unrelated staged, unstaged, and untracked work.

## Implementation and verification

- Explain intent and assumptions in comments only where the logic is non-obvious.
- Update the relevant docs when behavior, data shape, request flow, risk, or decisions change. Record meaningful choices in `docs/DECISIONS.md` with alternatives and tradeoffs.
- Review the complete diff, including changed files, before reporting completion. Call out unexpected changes and unverified assumptions.
- Do not claim tests or commands passed unless they were actually run and their output supports the claim. Follow `docs/TESTING.md` when verification is requested or needed.
- Do not commit unless the user asks. Keep one logical change per commit when commits are requested.

## Session continuity

- Read `docs/HANDOVER.md` at the start of a work session and update it at the end with completed work, remaining work, risks, next steps, and files that must not be touched.
- Separate confirmed facts from assumptions. Never invent project policy, production status, data scale, or security controls.

## Permission habits

- Safe without extra confirmation: reading/listing project files, searching, and reviewing diffs.
- Pause and explain before: broad edits, dependency/config changes, migrations, deleting files, external network calls, or changes to authentication, authorization, secrets, deployment, or production data.
- Never expose secrets, commit credentials, weaken security checks, or run destructive commands without explicit user authorization.
