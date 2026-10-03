# CCTO Agent Instructions

Before starting CCTO work, read the authoritative cross-workstream handoff log:

`main:Docs/Coordination.md`

Use that file instead of asking the user to relay messages between separate chats or agents.

Rules:

1. Check open items owned by your workstream.
2. Perform the work directly when possible.
3. Update `main:Docs/Coordination.md` with status/results when an item changes.
4. Put durable design decisions in the proper source-of-truth document/code; Coordination is only the handoff/status surface.
5. Do not create branch-specific copies of `Docs/Coordination.md`; the copy on `main` is authoritative.

## Automated runtime-error gate

For any automated test that launches RimWorld, a passing scenario/test count is not sufficient by itself.

The test harness must capture an isolated runtime log and fail the overall test run if the repository-owned mod emits any ERROR-level entry. Do this even when all Pickle/RimTest scenarios otherwise pass. Warnings remain non-fatal unless a repository-specific test explicitly promotes them.

Any new RimWorld runtime-test harness added to this repository must include this mod-origin ERROR gate from the start. Static-only validation does not fabricate a runtime-log result; add the gate when runtime automation is introduced.
