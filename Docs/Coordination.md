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

PR #2 **Integrate CCTO framework and cold-tolerance balance** passed all automated/manual gates and was merged into `main`. Merge commit: `a1616054a7d081f3167db501a49dfe3630ce4402`. The integrated `main` state is now the release source of truth.

### CODE-006 — Normal-game pre-beta smoke and Info Card visual check

**Requested by:** Code/framework  
**Owner:** Code/framework  
**Status:** DONE

The automated framework + balance gate is green at 13/13. The remaining pre-beta check is intentionally performed against the user's real normal mod profile rather than the isolated Pickle profile.

Preparation tooling added on `balance-xml`:

- normal-profile validator: `51c882decf241dc57493998251ed96d161cffaa5`;
- one-command cleanup/build/profile preparation: `b456d3d9099c563b0b91d14dc4da82b880326bf1`.

Run:

`prepare-smoke.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

Then launch RimWorld normally and verify representative Info Cards:

- Vanilla rice: min growth 10 C, cold death -1 C;
- Vanilla hops: min growth 5 C, dormancy temperature 5 C;
- MO wheat: min growth 0 C, cold death -6 C;
- MO apple tree: min growth 5 C, dormancy temperature 5 C.

Also require normal startup with no new CCTO-origin error, no duplicate CCTO rows, and acceptable visual grouping/wording of `枯死温度` / `休眠温度` next to native plant-temperature entries.

**Initial visual check result:** the standard Info Card path itself passed, but the dormancy wording exposed a UX defect.

User-provided normal-game screenshots confirmed the numeric balance values for all four representative standard Info Cards:

- Vanilla rice: min growth 10 C, cold death -1 C;
- Vanilla hops: min growth 5 C, dormancy behavior present;
- MO wheat: min growth 0 C, cold death -6 C;
- MO apple tree: min growth 5 C, dormancy behavior present.

No duplicate CCTO rows were visible. However, `低温反応: 休眠` did not make the actual dormancy threshold obvious even to the developer. The UI specification has therefore changed: dormancy crops now display `休眠温度 <minGrowthTemperature>` directly. This reuses the native minimum-growth value; it does not add a third balance parameter.

Implementation on `balance-xml`:
- Info Card logic: `381d67873d88f2125f0bdb1806670e638dc91c51`;
- Japanese text: `087a5e078ed788878293b4ed74804198d733aef4`;
- English text: `b2ab2621fae41fb055ec9d86c5487ba2f19c5460`;
- E2E / RimTest coverage: `7c8ed7bd417bb4f0d15ea3985a2baad63242ac7c`, `ab53c9772392a95169150f9f3053d182f3789f4e`, `c688d4bbd4080c40a117a8411a260fcb2d5b3a28`;
- smoke checklist/output: `410b6c1b69615da3744e562130f13ecdb03b24ba`, `141cc40fe0b0b6281fc2ba009a8cbb0aa35f9c0e`.

The visual part of CODE-006 must be rerun after rebuilding because the displayed dormancy row has changed.

**Post-change automated rerun:** PASS. The user reran the full integrated gate after the explicit dormancy-temperature implementation and all **13/13** scenarios passed again. The automated blocker remains cleared for the current code.

A separate compact plant-information panel from another mod does not surface CCTO's `SpecialDisplayStats` rows and also showed unrelated garbled text. This is not a failure of the standard RimWorld Info Card path and is outside the current CCTO release gate.

**Final result:** PASS.

After rebuilding the current dormancy-temperature implementation, the normal-game smoke/visual recheck completed without issue. The user confirmed:

- `休眠温度` displays correctly for dormancy crops;
- the normal startup/runtime smoke showed no new CCTO-origin problem;
- the representative standard Info Cards remained correct and readable.

CODE-006 is complete.

Durable checklist: `Docs/DevelopmentTools.md` on `balance-xml`.


### CODE-007 — Nice Plants Menu compatibility

**Requested by:** normal-game smoke finding  
**Owner:** Code/framework  
**Status:** DONE

The priority alternate plant-selection UI set was checked against both target mods.

**Dubs Mint Menus** (Steam Workshop 1446523594, packageId `Dubwise.DubsMintMenus`) requires no dedicated CCTO patch. Its plant-selection UI shows RimWorld's standard Info Card on mouse-over, and the user confirmed CCTO's current `枯死温度` / `休眠温度` rows are already visible there. Native `SpecialDisplayStats` reuse is therefore the supported path.

**Nice Plants Menu** (Steam Workshop 3685058533, packageId `Andromeda.NicePlantsMenu`) still needs a dedicated soft/conditional compatibility path because its compact plant-summary panel is built independently and does not surface CCTO's `ThingDef.SpecialDisplayStats` rows.

Compatibility requirements:

- Nice Plants Menu must remain optional; no hard dependency;
- show CCTO `枯死温度` for fixed-death crops;
- show explicit `休眠温度` using the plant's native `minGrowthTemperature` for dormancy crops;
- if both dormancy and an extreme-cold death threshold are configured, show both;
- unsupported/unconfigured plants remain unchanged;
- Dubs Mint Menus gets no redundant compatibility code.

Implementation is now present on `balance-xml`.

DLL audit:

- uploaded Nice Plants Menu DLL SHA-256: `f82a8c52724bf24ca93e966d9b5881e3788655e64656bacdebaaffb9e1264c9a`;
- plant-info row method: `NicePlantsMenu.Dialog_PlantBrowser.DrawInfoBlock(ref float y, float x, float totalWidth, string label, Texture2D icon, string valueStr, Func<TaggedString> tooltip, Color? color)`;
- current displayed plant: `Dialog_PlantBrowser.drawInfoFor -> PlantRecord.plant`;
- Nice Plants Menu's growth-temperature row uses translation key `NPM_GrowthTemperature` and the same `DrawInfoBlock` method.

Implementation strategy:

- only initialize compatibility when packageId `andromeda.niceplantsmenu` is active;
- dynamically resolve Nice Plants Menu types/members so there is no compile-time or hard mod dependency;
- Harmony-postfix `DrawInfoBlock`;
- immediately after the growth-temperature row, call Nice Plants Menu's own `DrawInfoBlock` to add `休眠温度` / `枯死温度`;
- reuse the existing temperature icon/layout/scroll/tooltip path;
- guard recursive postfix entry;
- if a future Nice Plants Menu update removes the reflected members, fail soft with a warning.

Implementation commits on `balance-xml`:

- compatibility patch: `96283e78ae905dcd88db928f2d52d55159d70c83`;
- Bootstrap activation: `5bbcdc4be482cd151aca7e41d73ddff54c0cf838`;
- durable compatibility design: `d17854a1aab101717435bb389582d4c7e61504b3`;
- manual smoke checklist: `e1944034e78ecb13fb5ddcdffa428318f60dc578`.

**First post-implementation gate attempt:** build stopped before RimWorld launch. The Nice Plants Menu compatibility source directly references UnityEngine `Texture2D` / nullable `Color`, which made the legacy `csc.exe` shipping build require RimWorld's `netstandard.dll`. This was a build-script/reference omission, not a CCTO behavior assertion failure.

Fixes on `balance-xml`:

- `build.bat` now verifies and references `RimWorldWin64_Data\\Managed\\netstandard.dll`;
- the SDK project file also declares the same RimWorld `netstandard.dll` reference for consistency.

**Second post-implementation gate attempt:** the added `netstandard.dll` reference resolved the previous UnityEngine type-reference errors. Compilation then reached the compatibility source and failed only on `nameof(DrawInfoBlockPostfix)`, because the shipping build intentionally uses Windows' legacy .NET Framework `csc.exe`, which does not support the C# 6 `nameof` operator.

The compatibility source was made legacy-compiler-safe by replacing the `nameof` expression with the literal method name. Fix on `balance-xml`: `ede65a8b15334f6f5423af2d9da9a1ececee6dff`.

**Post-fix automated rerun:** PASS. The full integrated `run-tests.bat` gate completed successfully with **13/13** passing scenarios after the Nice Plants Menu compatibility implementation, `netstandard.dll` reference fix, and legacy-compiler `nameof` fix.

**Final manual visual result:** PASS.

The user confirmed the Nice Plants Menu smoke check completed without issue after the 13/13 automated rerun. The representative fixed-death and dormancy rows displayed correctly in the Nice Plants Menu panel, and no new CCTO-origin error was observed.

CODE-007 is complete. Dubs Mint Menus remains supported through the standard Info Card path with no dedicated compatibility code.

### RELEASE-001 — 0.1 Beta public release preparation

**Owner:** release/presentation  
**Status:** IN PROGRESS

The tested implementation is integrated on `main`.

Release classification decision: **0.1 Beta**. Core functionality and the documented automated/manual gates are complete; the public Beta phase is for broader real-play validation, compatibility reports, and balance feedback.

Completed release-preparation work:

- public-facing `README.md` added: `3e46342e13b2baae570be30dab76e5cc396d1681`;
- `About/About.xml` description was updated for the public feature set and optional UI compatibility; release classification is now **Beta**;
- `Docs/ReleaseChecklist.md` added: `a430ca6252a88d2ff7e9820a7cf276432237d22e`;
- README now explicitly presents CCTO as a framework for other mods to assign fixed cold-death temperatures / dormancy, and includes an AI-assisted development disclosure: `a69972f21d26900b0023e91957317b5baecc58dd`;
- README now also describes CCTO explicitly as a cold-tolerance rebalance and publishes before/after minimum-growth values plus original/CCTO cold behavior for all 12 Vanilla targets and 21 MO-specific targets; MO Healroot is documented as using the Vanilla `Plant_Healroot` value. The comparison reflects Vanilla's per-plant derived threshold (`minGrowthTemperature + -18..-10°C`) and MO's original `dieIfLeafless` behavior: `f4fff83ca546a9027064d546687cc26d902b4a01`;
- public positioning now states that the current balance is generally higher difficulty than the original Vanilla/MO cold settings because most supported crops require warmer active-growth conditions and ordinary crops often die at much warmer temperatures; dormancy crops are explicitly described as exceptions rather than universal nerfs. README: `af44a136568b8b3c8fca9cd2d07b24089ed00c6e`; design source of truth: `9388272a79dee24bb8178d67849f540ab819baad`; About description: `626aa4ad2af16fe14ba1548888c2450d50dd0e49`;
- `About/About.xml` public description now explicitly says CCTO rebalances cold tolerance: `96c4362fec8a49d55e658b901c0cf9e8935e5cd1`;
- `About/About.xml` now explicitly states the framework role: `3462030feac0779d9ee9e34e6b5c2efd30607f1f`;
- `Docs/Framework.md` now defines the framework purpose in terms of plant-specific cold-death temperatures and dormancy: `aa08d59ad4b62d546373e0410be49eb1ef60d01c`;
- release checklist now requires the Steam Workshop description to carry the framework role and the same AI-assisted development disclosure: `f51e322c658ced558336101fc6947b0dd021ce0f`;
- release checklist now requires the public Workshop presentation to expose the Vanilla/MO rebalance as a before/after comparison: `3397dcba575fb00785fda7a574bdffe531b7dee7`;
- release checklist now also requires the higher-difficulty direction and dormancy exception to be disclosed in the Workshop description: `7e6524cc07c9a54cfeb49e045b24799d6094005b`.
- Workshop presentation is now synchronized with the README's current before/after cold-behavior tables, and explicitly records README as the public-content source of truth: `4d0dbbd8721f45eca4aba8bbba4f2f66709727f6`;
- paste-ready Steam Workshop BBCode body added at `Docs/SteamWorkshopDescription.txt`: `837f0b086f230940ac202e1275abd269011f0264`;
- YADA Workshop upload filtering added through repository-root `.rimignore`, preserving runtime `About/`, `Assemblies/`, `Languages/`, `Patches/`, and `LICENSE` while excluding repository/development assets: `077e7bd563cc4ee2a079ab8df981dff2682cc93e`;
- CCTO development workflow documents the YADA upload path and the README -> Workshop presentation -> paste-ready BBCode maintenance chain: `c913cbd7dda7024e7d3f4fe6d9d942b77f2438a3`;
- release checklist records the paste-ready BBCode and YADA filter as complete and simplifies the remaining GitHub release task to the already-fixed tag `v0.1.0-beta`: `e87bf804e8c06c68102e5b6effe63d792090b2ae`.

Public-description decisions:

- CCTO should state that it was split out while designing the planned Ancient & Medieval Japan (AMJ) mod because the cold-tolerance mechanic is independently useful;
- public descriptions should explicitly state that CCTO also functions as a lightweight framework for other mods to assign explicit cold-death temperatures and cold dormancy to their plants;
- the README and Steam Workshop long description should disclose AI-assisted development: AI was used primarily for code implementation, documentation, research, and test development, while design decisions, balance decisions, and final testing/review remain the author's responsibility;
- no separate display-only edition is currently planned. Vanilla uses a common derived cold-threshold rule rather than independently balanced fixed death temperatures per crop, so the display is intended to report CCTO's underlying species-specific behavior rather than exist as an isolated UI-only product.

Remaining public-release decisions/tasks:

- create and add `About/Preview.png` using the approved preview brief;
- create GitHub release/tag `v0.1.0-beta`;
- publish to Steam Workshop using the finalized Workshop presentation;
- perform a clean published-package smoke test after publication.

Release-presentation decisions completed on `main`:

- existing repository license confirmed as MIT; checklist update: `a725a09fb24840291a6038ba757f4b607c5b7bc7`;
- public display version fixed to **0.1 Beta**, Git tag fixed to **v0.1.0-beta** in the same checklist update;
- Steam Workshop title/long description/value tables/framework disclosure/AI-assisted-development disclosure drafted in `Docs/WorkshopDescription.md`: `3f860d1b981dff33e202c9e6c0346a18bb9f53dd`;
- RimWorld Workshop categories fixed to `Mod` + `1.6`, no required DLC: `5936b2108d63a6a9d04aa6de2970ebff68376f24`;
- GitHub release notes drafted: `8fff8f7728f6e44b03239896f5e54ddf14fbb722`;
- Preview image specification fixed at 640×360 PNG / 16:9 / under 1 MB: `8c6fada61bf9fcd93f3fa6908a81c0f17797e3d6`;
- release checklist narrowed to remaining publication steps: `d80786082bb3dfe0aa35ac45cb1cce98272dcea3`.

### Completed handoffs

- Framework + Vanilla/MO balance + explicit dormancy-temperature UI + Nice Plants Menu compatibility integrated into `main` through PR #2. Merge commit: `a1616054a7d081f3167db501a49dfe3630ce4402`.
