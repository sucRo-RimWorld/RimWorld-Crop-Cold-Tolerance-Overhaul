# Tests agent instructions — CCTO

Read root `AGENTS.md`, `main:Docs/Coordination.md`, and Project shared rules before testing.

- `Tests/Release/validate_workshop_description.py` checks publication description source.
- `Tests/Release/validate_workshop_payload.py` checks packaged payload contracts.
- `Tests/Release/validate_add_changenote.py` checks changenote metadata contracts.

Passing these static checks does not prove actual Steam upload or Add Changenote hook execution. Keep release/readiness claims tied to their correct evidence, including `Docs/ReleaseChecklist.md`.

Release tests are owned by `Tests/Release`; E2E belongs to Runtime. Research is opt-in only (`Tests/TestLifecycle.md`).
