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
**Status:** DONE

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
- Ko-fi support link added to the README and synchronized to both Workshop presentation files. README: `ad16b572f9e8086a488aea19d85021e3c5d521bd`; Workshop presentation: `6aa9b6d8713c77ba85043967336e658bc00a40f7`; paste-ready BBCode: `4911dea2778eb569ee57f232efa30b6c5db20a9c`.
- Ko-fi support presentation changed from a plain text link to the requested Shields.io badge. README: `076347066f6c1c59020f5bf5f0c47368dae79da7`; Workshop BBCode equivalent: `84b48991ec19a7786b5b0bee8a7e577a8c495d92`; paste-ready BBCode: `c1842c3bf03b2a63916b1b3bf3ae9cd3c1c7cde9`.
- Steam Workshop save failure traced to the generated description exceeding Steam's 8,000-byte description limit; the prior paste-ready body was 8,655 characters before UTF-8 overhead and also used unsupported Workshop table tags. The Workshop presentation was compacted to 5,701 characters, all 33 crop before/after comparisons were retained in code blocks, and table/tr/td/th tags were removed. Presentation: `ed124d8144cce47f65a9a343e4f02b90d9d23300`; paste-ready BBCode: `4a1c56356e6b1b2ff2c070744f67ecb77acabedf`.
- After visual review on Steam, the long all-crop code-block list was replaced with eight representative before/after examples (Rice, Potato, Healroot, Hops, Cocoa, MO Cabbage, MO Apple, MO Lemon), using normal BBCode text for readability. The full 12 Vanilla + 21 MO comparison remains in README and is linked directly from the Workshop description. Presentation: `5b5a58d060e14cf65a0caf78cf8fed3f5f8da27e`; paste-ready BBCode: `b0865c6f3f0035a6260db6c91b5d8dc30a3c5658`; release checklist: `7ad86bc65133d785efd4f7cae309f48e2854cdf6`.
- Public balance rationale now states that crop-specific minimum-growth/death values are informed by real-world cold-tolerance, frost-damage, and lethal-temperature information where available, while treating those temperatures as reference points rather than exact universal constants because cultivar/growth stage/acclimation/exposure vary. README: `70371ef47a56d95247fd5ec35412e6d058eb16d2`; design source of truth: `7a9ead858966509c5627aba2953bcb384824c90a`; Workshop presentation: `c9051eb2bb95cb99da0358368dd3ea946565e691`; paste-ready BBCode: `78abf19fe9b386db9a7426de9d33b57ec21a4138`; release checklist: `3a9631a8356782430c4b1dbac8a9604321266d62`.
- Public introductions now explicitly mention CCTO's non-lethal cold-dormancy behavior: selected plants stop growing and become leafless in the cold instead of dying, then recover after warming. README: `ce5d3b9c4fee55894595c4660100fea2d6b22d97`; About.xml: `06998475999af96aa42d524266613ea2ae66c2fb`; Workshop presentation: `1eff522a551aa317ef701582c48d61e574e50a37`; paste-ready BBCode: `2de8343415e3ca97429fb39ee92b6f080f1b2b6f`.
- Supported languages (English and Japanese) are now listed in the README and Steam Workshop presentation. README: `e4abc8aa0c0eb3a9515bf9b1e9713228264e9d9b`; Workshop presentation: `7527e5e1645cce110fc75b7985a65125e5f24cc1`; paste-ready BBCode: `835d25959b6fdf7c64c141ba62b265425b4c7d5e`; release checklist: `5c5f17ad01b33cb82f2c079fc6a7b3048a7ebab4`.
- Japanese Steam Workshop localization added. Localized title: `作物耐寒性オーバーホール`; paste-ready title file: `Docs/SteamWorkshopTitle-ja.txt` (`4f6f5767fa3982b222a7f2992c2501ce5f4ecccb`); paste-ready Japanese BBCode description: `Docs/SteamWorkshopDescription-ja.txt` (`56aaf0d8cb9046e5e54a08dae2d813eea140240e`). Workshop presentation records the localization workflow: `59df0e8688f9a6811125b4c53f0938a3fdf3e8c3`; release checklist: `1f1fc23a7b36e23fd1cdc54dcacb4d56b7bb38aa`.
- User completed a subscription smoke check against the private Steam Workshop package and reported no apparent issues. This counts as a pre-publication package sanity check; the checklist still keeps the final post-publication clean-subscription smoke pending. Release checklist update: `9a73701332e2f9b6e38ad323915db36fcc29aaea`.
- `About/Preview.png` is now present on `main`. The binary was copied from the existing `balance-xml` branch blob `c3b690262b92ce06e753fb0ac6e6929ff96bcf73` into `main` in commit `30dae3c1c541e185be98fcb67a6fc1bc435b53b7`; release checklist marked complete in `91acc72b6418bc989b83f0ec2a0d5c538a494745`.
- Steam Workshop item is now public at `https://steamcommunity.com/sharedfiles/filedetails/?id=3812412548`. Public-release checklist marked published in `a977e71bcff03942d2199c62d99e5e8fde083497`. Post-publication subscribed-package check was then completed; representative CCTO balance values were confirmed correct with no apparent issue. Release checklist update: `b1ed31fd1d749c32192c8153100452ad31a5ad71`.
- GitHub 0.1 Beta release/tag is complete. Tag `v0.1.0-beta` resolves successfully on GitHub; user confirmed the release is published. Release checklist update: `ea42fbb329db3670d865cb844e023d04009de8d4`.

Public-description decisions:

- CCTO should state that it was split out while designing the planned Ancient & Medieval Japan (AMJ) mod because the cold-tolerance mechanic is independently useful;
- public descriptions should explicitly state that CCTO also functions as a lightweight framework for other mods to assign explicit cold-death temperatures and cold dormancy to their plants;
- the README and Steam Workshop long description should disclose AI-assisted development: AI was used primarily for code implementation, documentation, research, and test development, while design decisions, balance decisions, and final testing/review remain the author's responsibility;
- no separate display-only edition is currently planned. Vanilla uses a common derived cold-threshold rule rather than independently balanced fixed death temperatures per crop, so the display is intended to report CCTO's underlying species-specific behavior rather than exist as an isolated UI-only product.

Remaining public-release decisions/tasks:

- none for the 0.1 Beta publication flow.

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


### BAL-001 — Complete living plant coverage

**Requested by:** scope audit  
**Owner:** Balance/XML  
**Status:** IN PROGRESS

Durable scope decision: CCTO coverage is based on living PlantDef identity, not player sowability. Within explicitly supported plant sets, every living PlantDef receives an explicit CCTO cold-response model. This includes cultivated crops, medicinal/fiber plants, fruit trees, forestry trees, decorative plants, fungi, grasses, shrubs, wild trees, cave plants, and other wild-only vegetation. Dead stump/remnant Defs are excluded because they are not living vegetation. Unsupported third-party plant sets retain originating behavior unless compatibility is added.

Balance rule for wild-only plants: values must remain compatible with the climates/biomes where the plant naturally occurs, so normal seasonal cold does not cause implausible routine die-off or destabilize biome vegetation/grazing.

Current implemented target set:
- Core / Vanilla RimWorld 1.6: **49 living PlantDefs**. Three dead stump/remnant Defs are intentionally excluded.
- Medieval Overhaul 1.6: **29 MO-specific living PlantDefs** (21 cultivated + 8 wild-only).
- Total curated Core+MO target set: **78 living PlantDefs**.

New Core wild-only coverage added:
- `Agarilux`, `Plant_Agave`, `Plant_Alocasia`, `Plant_Ambrosia`, `Plant_Astragalus`, `Plant_Berry`, `Plant_Brambles`, `Bryolux`, `Plant_Bush`, `Plant_Chokevine`, `Plant_Clivia`, `Plant_Rafflesia`, `Glowstool`, `Plant_Grass`, `Plant_ShrubLow`, `Plant_Moss`, `Plant_PincushionCactus`, `Plant_TallGrass`, `Plant_HealrootWild`.

New MO wild-only coverage added:
- `DankPyon_Plant_MindwortWild`, `DankPyon_Plant_PoppyWild`, `DankPyon_Plant_FleawortWild`, `DankPyon_Plant_FlyAgaricWild`, `DankPyon_GreatOak`, `DankPyon_GreatIter`, `DankPyon_GreatFir`, `DankPyon_GreatWillow`.

Notable balance decisions:
- Japanese bamboo baseline remains minimum growth 10 C / fixed cold death -18 C.
- Wild counterparts of MO alchemy crops use the same species-level values as their cultivated counterparts.
- Great Oak / Great Iter / Great Willow use cold dormancy; Great Fir uses minimum growth 0 C / fixed cold death -35 C.
- Tundra/boreal/cold-bog Core vegetation generally uses dormancy or strong frost tolerance; explicitly tropical plants use warmer fixed death thresholds.

Implementation/status:
- Core balance XML now contains 49 minimum-growth patches + 49 CCTO extensions.
- MO balance XML now contains 29 minimum-growth patches + 29 CCTO extensions.
- Static validator expects 49 Core + 29 MO entries and checks all four relevant MO 1.6 source files (cultivated farm/alchemy + wild alchemy + wild Dark Forest).
- MO E2E fixture contains 29 target Defs.
- Loaded-value E2E assertions cover all 49 Core + 29 MO targets.
- Coverage regression expects exactly 78 living Core/MO PlantDefs in the isolated profile and excludes dead plant remnants.
- Full integration suite remains 14 scenarios; scenario names now refer to Core living plants, MO plants, and all supported living-plant coverage.
- README, Design, ImplementationTable, About.xml, Workshop English/Japanese descriptions, ReleaseChecklist, DevelopmentTools, and PatchPlan are synchronized to the all-living-PlantDef scope.
- Workshop maintained descriptions remain within Steam's 8,000-byte description limit (English 6,424 bytes; Japanese 5,588 bytes at current main).

Key commits for the all-living-plant scope:
- durable scope change: `73351ca2e7931d5a25d0883a69358a5bf9d4a83e`
- Core wild plant XML: `62406b18d0ef06b216a1776bf1d98c13837a485a`
- MO wild plant XML: `c7e3f8ebf1e382daec83a430f9f3d9d71f6e575f`
- 49/29 validator: `0c75b0885b4ab8550674666d71964d374cd5125e`
- MO 29-Def fixture: `719a17a0efc1dfc2669690cda8001d1b9f893c9d`
- E2E 49/29 + 78 coverage: `64ee4de1d0fdb0185ec815d0f83be5a33ad2e070`
- scenario renames: `c160d9d4cced1af1a724a261c4f6ff1b0bd75834`, `7320a0ab2bf2ec2305309094f33f3ca8dd91d7a5`
- design wild-value tables: `d97128ab26695a8f7db82644748b2048d7a25ea8`
- implementation mapping: `c4c7ae4aad7059cbc1ae11c5bd66761752caedbd`
- README: `245800a9cfb5aea2892adca1085a78291aa70b32`
- About / Workshop scope sync: `0564c9d23a770d585bb15c4ec29ce32931dbdb6a`, `0848f84437fa19c4b0a36e5cf2a523785edfa116`, `7c6c8886177e693823cff67b853774200c03b7b1`, `f0f8a9ff3bc92f92cc53fc20ea7a6961a49c2245`
- release/dev/patch docs: `9c9f4779077b87c8c1ea06125baf77109555f87c`, `0abd3e35b91079f38e46be206f167240c0ef0873`, `42cce60bdbbe0999f83e3563d3a46de1de278ffc`

Static consistency audit: PASS.
- Core XML 49 / validator 49 / loaded-value E2E 49.
- MO XML 29 / validator 29 / fixture 29 / loaded-value E2E 29.
- Coverage expected set: 78.
- Required 14-scenario summary names are synchronized.

Remaining release gate:
- full `run-tests.bat` rerun: **PASS, 14/14** (user-reported 2026-10-03), including the all-supported-living-plant coverage scenario;
- normal-profile wild-plant Info Card smoke: **PASS**. Verified Wild Healroot (min 0 C / death -9 C), Berry bush (min 0 C / dormancy 0 C), Bush (min 0 C / dormancy 0 C), and Oak (min 5 C / dormancy 5 C); displayed values matched the implemented CCTO balance.
- BAL-001 implementation/validation is complete.
- release/presentation remaining: rebuild/re-upload the Workshop package and update the live English/Japanese descriptions.

Release/presentation maintenance update (2026-10-03):
- Release notes are now centralized under `Docs/ReleaseNotes/`; root-level `Docs/ReleaseNotes-*.md` copies were removed and checklist references were updated.
- Japanese Workshop description wording was polished for natural Japanese, updated to 0.1.1 Beta verification status, and remains within Steam's 8,000-byte limit (5,951 bytes).
- Live Workshop page still requires the maintained Japanese description to be pasted/applied if not already updated after this commit.
- English Workshop presentation and paste-ready description were synchronized to **0.1.1 Beta / 14/14**; no stale 13/13 or 0.1 Beta wording remains in those two maintained English files. Current paste-ready English description size: 6,389 bytes.

UI consistency update (2026-10-03):
- Standard Info Card `Cold death temperature` and `Dormancy temperature` now use the same display priority, so the relevant cold threshold appears at the same row position when comparing plants.
- Nice Plants Menu was already inserting either threshold in the same position after growth temperature; no compatibility change was needed there.
- RimTest regression added to assert equal display priority for death vs dormancy entries.
- Code commits: `e24af7ea7e3ad9a50f6384b9978161def5b66751`, `cad86c5a55a8c4de2ca04999fb3b2b76f1ee3790`.
- Full local gate and normal-profile visual recheck are pending after this UI-only code change.
