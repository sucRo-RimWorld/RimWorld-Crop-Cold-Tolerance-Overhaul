# CCTO Coordination

This file is the shared coordination surface between separate ChatGPT chats/workstreams working on CCTO.

Use this instead of asking the user to manually relay messages between chats.

## Working rule

At the start of CCTO work:

1. Read this file.
2. Check whether there are open items owned by the current workstream.
3. Perform the relevant work without asking the user to relay the request again.
4. Update this file with the result, status, and commit/reference when the item is resolved or materially changed.

For durable specifications and decided values, update the appropriate design/framework document as well. This file is for coordination and handoff, not the final source of truth for design.

## Workstreams

- **Balance/XML**
  - crop-specific `minGrowthTemperature`
  - fixed `coldDeathTemperature`
  - Vanilla / Medieval Overhaul balance XML
  - AMJ crop values and compatibility XML

- **Code/framework**
  - C# framework and Harmony patches
  - dormancy behavior
  - Info Card
  - validation
  - automated tests
  - build/test tooling

## Status vocabulary

- **OPEN** — needs work
- **IN PROGRESS** — currently being investigated/implemented
- **BLOCKED** — waiting on a specific prerequisite
- **DONE** — completed and reflected in the proper source-of-truth document/code

## Current coordination items

### CODE-001 — Fixed cold-death boundary

**Requested by:** Balance/XML  
**Owner:** Code/framework  
**Status:** DONE

Confirm that a configured fixed death threshold uses a strict below-threshold rule:

- exactly at the configured threshold: survive;
- below the configured threshold: die.

Current implementation uses:

`temperature < coldDeathTemperature`

An E2E boundary scenario exists. The first runtime attempt exposed a test-harness temperature synchronization issue rather than a CCTO logic failure. The harness was fixed so the plant's actual `AmbientTemperature` is synchronized before forcing the cold check.

**Result:** the rerun passed the full existing Pickle E2E suite, including the strict boundary scenario. Exact threshold survives; values below it die. Test harness synchronization fix is on framework branch commit `26c104677e189ff3450f2158ecb6e2882e857700`.

### CODE-002 — Recovery from cold dormancy

**Requested by:** Balance/XML  
**Owner:** Code/framework  
**Status:** IN PROGRESS

Balance/XML decision:

- preserve RimWorld's existing delayed leafless recovery;
- do **not** explicitly clear dormancy immediately when temperature rises;
- once temperature recovers, CCTO stops refreshing `madeLeaflessTick`, and the normal 60,000-tick leafless window is allowed to expire naturally.

This means visible/state recovery may lag warming by up to roughly one in-game day. That delay is intentional.

Durable specification: `Docs/PatchPlan.md`, commit `b868e278d04d772e406b92cde085ec7e4d36ca64`.

A delayed-recovery Pickle scenario has now been added on framework branch commit `28d7faac3760fb3d003f11d0878d0ab01de415d7`.

The first expanded-suite run was not a CCTO assertion failure: unrelated `Andromeda.PawnQuickInfo` errors were emitted during scenarios and Pickle correctly treated those `Log.Error` entries as failures. The E2E runner has therefore been moved to an isolated `-savedatafolder` profile containing only the required test mods, without rewriting the user's normal mod preset.

Isolation implementation: framework branch commits `149fefd4ca73d562f7900e6d90fbabe740c0b9e0` and `a2c8684299e884cc2feb964275caba8fdcc6a67e`.

**Next action for Code/framework:** rerun the automated suite in the isolated profile and require the delayed-recovery scenario to pass.

### CODE-003 — Indoor cold behavior / `CheckMakeLeafless` postfix

**Requested by:** Balance/XML  
**Owner:** Code/framework  
**Status:** IN PROGRESS

Balance/XML decision:

CCTO should intentionally evaluate configured plants using the plant's actual `AmbientTemperature`, even when Vanilla's original cold-leafless path would be gated by:

`room.UsesOutdoorTemperature`

Intended behavior:

- heated indoor greenhouse, cold outdoors -> survives if actual room/ambient temperature is safe;
- genuinely cold indoor room -> configured plant can enter dormancy or die;
- cold outdoor temperature alone must not kill a plant inside a warm room.

This is intentional CCTO behavior, not an accidental bypass.

Durable specification: `Docs/PatchPlan.md`, commit `b868e278d04d772e406b92cde085ec7e4d36ca64`.

Indoor-safe and indoor-cold Pickle coverage has now been added on framework branch commit `28d7faac3760fb3d003f11d0878d0ab01de415d7`.

The first expanded-suite run was contaminated by unrelated `Andromeda.PawnQuickInfo` `Log.Error` output rather than a CCTO assertion failure. The authoritative runner now uses the same isolated test profile described under CODE-002.

**Next action for Code/framework:** rerun the automated suite in the isolated profile and require the indoor room-temperature scenario to pass.

## Completed handoffs

None yet.
