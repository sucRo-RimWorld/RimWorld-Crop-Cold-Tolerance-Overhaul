# Tests agent instructions — CCTO

Read root `AGENTS.md`, `main:Docs/Coordination.md`, and Project shared rules before testing.

- `Tests/validate_workshop_description.py` checks publication description source.
- `Tests/validate_workshop_payload.py` checks packaged payload contracts.
- `Tests/validate_add_changenote.py` checks changenote metadata contracts.

Passing these static checks does not prove actual Steam upload or Add Changenote hook execution. Keep release/readiness claims tied to their correct evidence, including `Docs/ReleaseChecklist.md`.
