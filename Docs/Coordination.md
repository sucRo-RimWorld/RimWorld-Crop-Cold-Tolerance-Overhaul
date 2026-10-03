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
**Status:** DONE

Latest isolated run: 10/11 scenarios passed.

The sole failure was not a CCTO assertion. During the strict fixed-threshold boundary scenario, Vanilla sound update code called `MapTemperature.OutdoorTemp` and hit a `NullReferenceException` in `TileTemperaturesComp.GetOutdoorTemp`. The CCTO state dump at the failure showed the expected boundary state: ambient -10.00 C, plant alive, not leafless.

The E2E harness had still been mutating global outdoor temperature through a temporary `GameCondition_TemperatureOffset` and world tile-temperature cache clears. That global dependency is unnecessary for CCTO's checks, which consume the plant's actual `AmbientTemperature`.

Framework branch commit `a739d49dc556b5d68ff065a0ea91b80012d28cf6` removes that global temperature-condition path from CCTO E2E temperature setup. Tests now set the relevant RimWorld `Room.Temperature` directly and verify the spawned plant's `AmbientTemperature`. The indoor scenario uses a separate outdoor room temperature to preserve the warm-inside/cold-outside distinction without touching world temperature caches.

**Result:** final isolated Pickle run completed successfully with the full suite passing. The room-temperature based harness eliminated the remaining unrelated Vanilla temperature-cache failure. The automated E2E release gate is now green.

### CODE-005 — Integrated framework + balance XML regression

**Requested by:** Balance/XML  
**Owner:** Code/framework  
**Status:** DONE

The framework-only automated E2E gate is green through CODE-004.

Balance/XML has now rebased/merged the latest framework state into the balance branch:

- branch: `balance-xml`
- integration commit: `8e2b0d8843b54ea25bdb4a090540b5dd5889af5c`
- Draft PR: #2

This branch adds the actual Vanilla and Medieval Overhaul balance XML, including `minGrowthTemperature`, fixed `coldDeathTemperature`, and dormancy flags.

**Integration gate implemented on `balance-xml`:**

- `run-tests.bat` now verifies that Medieval Overhaul and its declared required dependencies are installed;
- it passes the installed MO root to `validate-balance.bat`, so all 21 targeted MO DefNames are checked against the installed 1.6 source XML;
- it launches the isolated Pickle suite in a `with-mo` mode that enables VEF, Processor Framework, and Medieval Overhaul in addition to the framework test set;
- new `balance.feature` scenarios assert the **final loaded `DefDatabase` values for all 12 Vanilla and all 21 Medieval Overhaul targets**, including `minGrowthTemperature`, exactly one CCTO extension, and the expected fixed death temperature/dormancy flag;
- `run-e2e.bat` remains framework-only by default, so the standalone framework gate does not unnecessarily require Medieval Overhaul.

Implementation commits on `balance-xml`:

- isolated MO test profile: `4780189eb21d7e8c319bd75dbd63dcbf47a51364`;
- loaded-Def scenarios: `e2d4492f595f9eaed14d78ccf69b2eaccb75b37b`, `67cb9e8685cdee9090c3287d6405f883eda0d870`;
- selectable Pickle filter / MO E2E mode: `beae177a3eedc7d61fba8c775c394df7afec0d95`, `a16ba426897dc0b777b8c39ad2c1d7e87d7b4bb4`;
- full integration runner: `9da133b1732cb867c4d39ee381f1cbf9fa058d3f`;
- generated test-mod verification: `5ed6caa02ba1da0a0107fa1e020b3dc7edd64dcb`.

First local integration attempt stopped in `Validate-BalanceXml.ps1` before any balance assertion because `validate-balance.bat` passed `%~dp0` (which ends in a backslash) as a quoted `-RepoRoot` argument. PowerShell then received an invalid path string.

This was a runner-only defect, not a balance-data failure. `Validate-BalanceXml.ps1` already derives the repository root safely from its own `Scripts` directory, so the redundant batch-level `-RepoRoot` argument has been removed.

Fix on `balance-xml`: `65237a894568cc753e2f03b09043dfe21d0769fd`.

The next local launch reached RimWorld, but the stripped profile failed during Medieval Overhaul's own mod-class startup before Pickle wrote a fresh report. The uploaded 11/11 report was therefore stale and did not contain the new balance scenarios.

The integration gate now keeps the installed real MO 1.6 XML as the source check, but uses a lightweight developer-only mod named `Medieval Overhaul` for runtime Def patch verification. This avoids unrelated MO runtime code while still exercising CCTO's real `PatchOperationFindMod` and XPath patches. The validator now also requires every real MO target to contain the local `<plant>` node that CCTO patches. Test save data and Pickle reports are cleared before each run so a failed startup cannot leave an old PASS behind.

Relevant `balance-xml` commits: `f5693133d5798d6458032db4361d348a13d0560b`, `0c81243ca5b0acf00ab10846b9e2c67331956790`, `45cfd4abe43519632ba71db59325f849f1cba992`, `25c0c738d4583f1daac2c5e8072cf34aff42882f`, `dae2b6e917a55f7d2b16e078412a82a9330cbb2f`, `847ea987f8e1c59c2e9b24d6acfb01f8087de9ab`, `6defb486478695266122722cce4d6cde4a78a48d`, `c2b1b2ef22e23bf371e101092f1733e53fe77d52`, `4a30636db4cf225492db930dfaa76358ff6c3297`.

The full integration mode now also validates the freshly generated `summary.json`: it must contain exactly 13 clean passes and must explicitly include both loaded-balance scenarios. This prevents a missing/undiscovered `balance.feature` from being mistaken for success. Summary-gate commits: `002f2b43ca74fba53cb8f379cd3bbb2de8e789b8`, `ce0d9fb088e8a67a9d5b8c4982a3254edb44579f`.

**Result:** the full local integration gate passed cleanly.

- static balance/XML validation passed;
- installed MO 1.6 source structure validation passed;
- Pickle executed `framework.feature,cold-tolerance.feature,balance.feature`;
- the fresh integration summary contained exactly **13/13 passing scenarios**;
- both loaded-balance scenarios were explicitly present;
- all 12 Vanilla and all 21 MO target Def values therefore passed the loaded-`DefDatabase` regression gate.

CODE-005 is complete. No balance numbers were changed during test-harness repair.

**Integration status update:** the latest `main` canonical docs/coordination state has been merged into `balance-xml` without changing tested code or balance XML. Integration merge commit: `efa6461c2302054f998e531b8f6e9fe554f5523a`.

PR #2 has been retargeted to `main` and renamed **Integrate CCTO framework and cold-tolerance balance**. It remains Draft until CODE-006 completes. PR #1 remains open for framework history but PR #2 is now the intended final integration path.

### CODE-006 — Normal-game pre-beta smoke and Info Card visual check

**Requested by:** Code/framework  
**Owner:** Code/framework  
**Status:** IN PROGRESS

The automated framework + balance gate is green at 13/13. The remaining pre-beta check is intentionally performed against the user's real normal mod profile rather than the isolated Pickle profile.

Preparation tooling added on `balance-xml`:

- normal-profile validator: `51c882decf241dc57493998251ed96d161cffaa5`;
- one-command cleanup/build/profile preparation: `b456d3d9099c563b0b91d14dc4da82b880326bf1`.

Run:

`prepare-smoke.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

Then launch RimWorld normally and verify representative Info Cards:

- Vanilla rice: min growth 10 C, cold death -1 C;
- Vanilla hops: min growth 5 C, dormancy;
- MO wheat: min growth 0 C, cold death -6 C;
- MO apple tree: min growth 5 C, dormancy.

Also require normal startup with no new CCTO-origin error, no duplicate CCTO rows, and acceptable visual grouping/wording of `枯死温度` / `低温反応: 休眠` next to native plant-temperature entries.

Durable checklist: `Docs/DevelopmentTools.md` on `balance-xml`.

## Completed handoffs

None yet.
