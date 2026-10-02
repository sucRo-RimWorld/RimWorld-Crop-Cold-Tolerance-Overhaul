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
**Status:** DONE

Balance/XML decision:

- preserve RimWorld's existing delayed leafless recovery;
- do **not** explicitly clear dormancy immediately when temperature rises;
- once temperature recovers, CCTO stops refreshing `madeLeaflessTick`, and the normal 60,000-tick leafless window is allowed to expire naturally.

This means visible/state recovery may lag warming by up to roughly one in-game day. That delay is intentional.

Durable specification: `Docs/PatchPlan.md`, commit `b868e278d04d772e406b92cde085ec7e4d36ca64`.

A delayed-recovery Pickle scenario has now been added on framework branch commit `28d7faac3760fb3d003f11d0878d0ab01de415d7`.

The first expanded-suite run was not a CCTO assertion failure: unrelated `Andromeda.PawnQuickInfo` errors were emitted during scenarios and Pickle correctly treated those `Log.Error` entries as failures. The E2E runner has therefore been moved to an isolated `-savedatafolder` profile containing only the required test mods, without rewriting the user's normal mod preset.

Isolation implementation: framework branch commits `149fefd4ca73d562f7900e6d90fbabe740c0b9e0` and `a2c8684299e884cc2feb964275caba8fdcc6a67e`.

A later isolated-profile run appeared to hang before completing. The recovery scenario originally advanced 60,050 real simulation ticks in fast mode; that long-running approach has now been replaced with a deterministic test that records the private `madeLeaflessTick`, verifies warm recovery does not refresh it, then ages the timer directly to the 60,000-tick boundary. This preserves the same regression contract without simulating a full in-game day.

Fast recovery test commits: `0529e9f8fe79f1379be6dd92eadba0c78d25ddcf`, `d8e0382c234ed1578ff57f4e4d55301f81cc0616`.

The E2E launcher also now has an outer five-minute process watchdog so a startup/runtime freeze cannot leave `run-tests.bat` waiting indefinitely. Watchdog commits: `510d6c5b31345259070ff5cd0f3e74e3061969af`, `7c1d1dc98d4be60bde823fe506c8d4ca4f8c6e42`.

Latest isolated-profile run result: the delayed-recovery scenario itself passed. The suite overall reported 9/11 passed because two other scenarios that used Pickle's full-world fast `I wait 2100 ticks` step triggered unrelated Vanilla WorldPawns/AI `Log.Error` entries.

The full-world waits have now been replaced with a targeted `Plant.TickLong()` E2E step so CCTO's real plant long-tick path is exercised without advancing unrelated world/pawn systems.

Targeted long-tick commits: `06aa7613f6e1150a0ae418ac6bf7d77ac85d84bf`, `474a38e0400100fb9cde8e07a73021741b09f264`.

A subsequent isolated-profile launch reached the RimWorld title screen but did not start Pickle scenarios. Cause: the fresh isolated profile had no `Prefs.xml`, so RimWorld dev mode defaulted OFF; Quickstarts explicitly requires dev mode ON and otherwise does nothing.

The test-profile preparation now copies the user's existing `Prefs.xml` into the isolated profile, sets only `<devMode>True</devMode>`, and removes any isolated `DevModeDisabled` marker. The user's normal preferences are not modified.

Dev-mode profile fix: framework branch commits `e36ef0e7f5745f1c2a9bec330545f41bcd8a435f` and `c0045c9886f6bfc6d9c3c22c8b07c78876548d17`.

**Result:** the isolated Pickle regression scenario for delayed dormancy recovery passed. It confirms that warming does not refresh `madeLeaflessTick`, and that the plant leaves the leafless state after the preserved 60,000-tick recovery window expires.

### CODE-003 — Indoor cold behavior / `CheckMakeLeafless` postfix

**Requested by:** Balance/XML  
**Owner:** Code/framework  
**Status:** DONE

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

Latest isolated-profile run result: the indoor room-temperature scenario passed. Heated indoor survival and genuinely cold indoor death both behaved as designed.

The suite overall still had two unrelated Vanilla errors from full-world fast ticking in other scenarios; those waits have now been replaced by the targeted plant long-tick step described under CODE-002.

**Result:** the isolated Pickle indoor regression scenario passed. A configured plant survives in a warm indoor room while a separate outdoor-temperature room is cold, and dies when its own actual room/ambient temperature is lowered below the fixed death threshold.

### CODE-004 — E2E harness clean all-pass

**Requested by:** Code/framework  
**Owner:** Code/framework  
**Status:** IN PROGRESS

Latest isolated run: 10/11 scenarios passed.

The sole failure was not a CCTO assertion. During the strict fixed-threshold boundary scenario, Vanilla sound update code called `MapTemperature.OutdoorTemp` and hit a `NullReferenceException` in `TileTemperaturesComp.GetOutdoorTemp`. The CCTO state dump at the failure showed the expected boundary state: ambient -10.00 C, plant alive, not leafless.

The E2E harness had still been mutating global outdoor temperature through a temporary `GameCondition_TemperatureOffset` and world tile-temperature cache clears. That global dependency is unnecessary for CCTO's checks, which consume the plant's actual `AmbientTemperature`.

Framework branch commit `a739d49dc556b5d68ff065a0ea91b80012d28cf6` removes that global temperature-condition path from CCTO E2E temperature setup. Tests now set the relevant RimWorld `Room.Temperature` directly and verify the spawned plant's `AmbientTemperature`. The indoor scenario uses a separate outdoor room temperature to preserve the warm-inside/cold-outside distinction without touching world temperature caches.

**Next action:** rerun the isolated suite and require 11/11 clean pass.

## Completed handoffs

None yet.
