# CCTO Development Workflow

Before code/framework work, read the authoritative cross-workstream handoff log at `main:Docs/Coordination.md`. Update it directly instead of asking the user to relay messages between chats.

This file contains **CCTO-specific** development and test guidance only.

General RimWorld mod-development tools, enablement policy, Local Work/Codex usage, and Sol/Astra escalation rules are maintained separately in the ChatGPT Library:

`/Ancient & Medieval Japan/RimWorld_Mod_Development_Environment.md`

That shared document is the source of truth for cross-project tooling policy. Do not duplicate those general rules here.

## 1. CCTO responsibility split

CCTO development is intentionally split into two workstreams.

### Balance/XML workstream

Owns:

- plant-specific `minGrowthTemperature` values;
- plant-specific fixed `coldDeathTemperature` values;
- Vanilla / Medieval Overhaul balance XML;
- AMJ overrides and balance decisions.

The code workstream must not silently redefine decided balance values.

### Code/framework workstream

Owns:

- `ColdToleranceExtension`;
- Harmony integration with RimWorld plant cold behavior;
- fixed cold-death handling;
- low-temperature dormancy support;
- Info Card display;
- English/Japanese framework UI strings;
- validation and diagnostics;
- build tooling;
- automated framework tests.

## 2. Local repository layout

Recommended local checkout:

`D:\SteamLibrary\steamapps\common\RimWorld\Mods\CropColdToleranceOverhaul`

This allows the repository to be both the Git checkout and the active local RimWorld mod.

Build with:

`build.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

Expected output:

`Assemblies\CropColdToleranceOverhaul.dll`

## 3. CCTO-specific development helpers

Enable only what the current CCTO task needs.

| Task | Preferred helper |
|---|---|
| framework logic / Harmony regression | RimTest Redux |
| actual temperature, tick, death, dormancy, recovery scenarios | Pickle |
| verify final loaded plant Def values | Things Explorer |
| verify XPath / XML patch results | XML Patch Helper |
| startup/runtime exception investigation | RimDoctor + Better Stacktraces |
| rapid reproducible map entry | Quickstarts |

The shared Library document contains the general enable/disable policy for these tools.

### Steam Workshop upload via YADA

YADA (Yet Another Dev Assistant) may be used for CCTO's in-game Steam Workshop upload workflow.

CCTO keeps a repository-root `.rimignore` specifically for YADA. Its purpose is to exclude repository/development material from the Workshop package while preserving runtime content.

The intended uploaded runtime set includes:

- `About/`
- `Assemblies/`
- `Languages/`
- `Patches/`
- `LICENSE`

The filter excludes repository-only documentation, source, tests, scripts, batch tooling, test reports, local test patches, version-control metadata, and debug/build leftovers. Do not add `Assemblies/`, `Languages/`, or `Patches/` wholesale to `.rimignore`.

Workshop presentation sources are kept separately:

- `README.md` — public-content source of truth;
- `Docs/WorkshopDescription.md` — title, version, categories, and maintained Workshop presentation;
- `Docs/SteamWorkshopDescription.txt` — paste-ready BBCode body for the Steam Workshop description field.

When the README's public feature/balance description changes, update both Workshop description files before publication.

## 4. CCTO validation sequence

### Automated release gate

The authoritative automated gate is `run-tests.bat`. On the integrated balance branch/final mod it performs both static balance validation and the Pickle + Quickstarts runtime suite, and returns a process exit code.

Build/run with:

`run-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

The full gate requires the installed Medieval Overhaul source tree (Workshop 3219596926) so the validator can check the real 1.6 plant DefNames and XML structure.

It intentionally does **not** activate Medieval Overhaul's runtime assemblies in the isolated Pickle profile. MO's own Harmony startup is outside CCTO's responsibility and can fail under an artificially stripped mod set even when the Def XML CCTO patches is valid. Instead, the gate stages a developer-only mod named `Medieval Overhaul` containing lightweight versions of the 29 MO-specific target plant Defs. RimWorld's `PatchOperationFindMod` matches active mods by display name, so CCTO's real MO patch file executes against these fixture Defs.

This is paired with static checks against the installed real MO 1.6 source: every target DefName must exist and must contain the local `<plant>` node required by CCTO's XPath. Thus the runtime fixture tests CCTO patch application while the static validator protects against drift in the actual MO source structure.

`run-e2e.bat` remains usable by itself for framework-only testing. `run-tests.bat` invokes its `with-mo-fixture` mode for the complete balance integration gate.

The underlying E2E runner:

- builds the shipping CCTO DLL;
- creates a separate local developer test mod at
  `D:\SteamLibrary\steamapps\common\RimWorld\Mods\CropColdToleranceOverhaul.E2E`;
- creates a second developer-only MO Def fixture at
  `D:\SteamLibrary\steamapps\common\RimWorld\Mods\CropColdToleranceOverhaul.MOFixture`;
- compiles CCTO-specific Quickstarts and Pickle step assemblies;
- copies all CCTO `.feature` files;
- prepares an isolated RimWorld save-data profile under `TestResults\SaveData`;
- copies the normal `Prefs.xml` into that isolated profile and forces only `devMode=True`, because Quickstarts does nothing when RimWorld dev mode is off;
- gives that profile a minimal test-only mod list; framework-only runs contain Harmony, Core, RimLogging, Pickle, Quickstarts, CCTO, and the CCTO E2E companion mod, while the full balance gate additionally enables only the lightweight MO Def fixture before CCTO;
- launches RimWorld with `-savedatafolder` pointing at that isolated profile;
- runs framework regression scenarios and live cold-behavior scenarios;
- clears the previous isolated save-data and Pickle report directories before every run, preventing a startup failure from being mistaken for a stale earlier PASS;
- writes fresh reports to `TestResults\Pickle`;
- in full balance mode, parses the fresh `summary.json` and requires exactly 14 clean passes plus explicit presence of the Core loaded-balance scenario, the Medieval Overhaul loaded-balance scenario, and the all-loaded-supported-living-plant coverage scenario;
- exits with Pickle's pass/fail/error result, or an integration-summary error if the expected balance scenarios were not actually run;
- is wrapped by an outer five-minute process watchdog, so a RimWorld/Pickle startup or runtime freeze cannot leave the batch file waiting indefinitely.

The user's normal RimWorld `ModsConfig.xml` is read only to reuse the current RimWorld version/known-expansion metadata. It is not rewritten. This prevents unrelated gameplay mods and their log errors from causing false Pickle failures.

Both generated test mods are development-only and must not be included in the Workshop release. Remove them with:

`clean-e2e.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

The automated Pickle gate no longer depends on the user's normal active mod preset. `Scripts/Prepare-TestSaveData.ps1` writes a separate test-only `ModsConfig.xml` inside `TestResults\SaveData`, and `run-e2e.bat` launches RimWorld against that profile.

The normal gameplay preset is therefore left untouched, and unrelated mods do not need to be manually disabled before an E2E run.

`Scripts/Check-TestModList.ps1` is retained only for workflows that intentionally run development helpers against the normal active profile; it is not part of the authoritative Pickle release gate.

### Current Pickle gate coverage

Loaded balance-Def scenarios in the full integration gate check:

- all 49 living Core PlantDefs have the final expected `minGrowthTemperature`, exactly one CCTO extension, and the expected fixed death temperature or dormancy flag;
- all 29 targeted Medieval Overhaul fixture Defs have the same final loaded-value checks after CCTO's real `PatchOperationFindMod`/XPath patch runs;
- a coverage scenario scans all loaded living PlantDefs in the isolated Core/MO profile and fails if a living plant lacks CCTO balance or falls outside the curated 49-Core / 29-MO target set; dead stump/remnant Defs are excluded;
- Medieval Overhaul's installed 1.6 source XML contains every targeted DefName **and** a local `<plant>` node before the runtime suite starts.

Framework regression scenarios check:

- fixed death thresholds are identical across different plant identities;
- dormancy uses native `minGrowthTemperature` as its cold-response threshold;
- unconfigured plants retain the vanilla per-plant threshold range;
- extension validation accepts dormancy-only and finite fixed death values while rejecting incomplete/infinite configurations;
- CCTO Info Card stat construction matches fixed-death, dormancy-only, and combined configurations.

Live-map scenarios check:

- a configured plant survives safely above its fixed death threshold;
- the same plant dies when its actual `Plant.TickLong()` path runs below the threshold;
- at exactly the fixed threshold the plant survives, while below it the plant dies (strict `<` boundary);
- a `dieIfLeafless` plant can enter CCTO dormancy without dying;
- a dormant plant with an explicit extreme-cold death threshold still dies below that threshold;
- cold dormancy preserves RimWorld's delayed 60,000-tick leafless recovery window after warming; the test verifies the stored `madeLeaflessTick` and advances that private recovery timer directly rather than simulating an entire in-game day;
- a heated indoor plant survives lethal outdoor cold when its actual room temperature is safe;
- a configured plant in a genuinely cold indoor room responds to its actual `AmbientTemperature`.

Each live scenario starts from a fixed-seed Quickstarts world. CCTO test temperatures are controlled by setting the relevant RimWorld `Room.Temperature` directly and verifying the spawned plant's resulting `AmbientTemperature`. This avoids mutating global world temperature caches merely to exercise CCTO's ambient-temperature checks. The indoor regression uses separate indoor and outdoor rooms to model warm-inside/cold-outside behavior.

Full-world fast waits are avoided for CCTO cold-response assertions. Earlier `I wait 2100 ticks` steps advanced all Vanilla world/pawn systems and could surface unrelated WorldPawns/AI errors. CCTO now calls the spawned test plant's `TickLong()` directly, which exercises the real plant long-tick cold path without ticking unrelated simulation systems.

### RimTest Redux developer suite

A separate RimTest Redux suite remains available as a fast interactive/unit-style development tool.

Build it with:

`build-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

It creates:

`D:\SteamLibrary\steamapps\common\RimWorld\Mods\CropColdToleranceOverhaul.Tests`

This keeps `RimTestRedux.dll` out of the shipping CCTO assembly. Remove the generated test mod with:

`clean-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

Its current coverage overlaps the framework portion of the Pickle gate: extension validation, fixed-vs-vanilla thresholds, dormancy threshold behavior, and Info Card stat construction. RimTest Redux is useful for rapid development feedback; Pickle is the release gate because it can also exercise a live RimWorld session and produce an unattended process result.

### Final pre-beta checks

After the full automated gate passes, prepare the normal-game smoke environment with:

`prepare-smoke.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

The script:

- removes generated CCTO E2E/MO-fixture and RimTest companion mods;
- rebuilds the shipping CCTO DLL;
- reads, but does not rewrite, the normal RimWorld `ModsConfig.xml`;
- requires Harmony, Core, Medieval Overhaul, and CCTO to be active;
- requires CCTO E2E/MO-fixture/RimTest, Pickle, Quickstarts, and RimTest Redux to be inactive.

Then launch RimWorld normally, without `-savedatafolder`, and perform this representative smoke/visual check:

| Case | Expected Info Card |
|---|---|
| Vanilla rice | minimum growth temperature 10 C; `枯死温度` -1 C; no `低温反応: 休眠` |
| Vanilla hops | minimum growth temperature 5 C; `休眠温度` 5 C; no `枯死温度` |
| Vanilla bamboo | minimum growth temperature 10 C; `枯死温度` -18 C; no `休眠温度` |
| MO wheat | minimum growth temperature 0 C; `枯死温度` -6 C; no `休眠温度` |
| MO apple tree | minimum growth temperature 5 C; `休眠温度` 5 C; no `枯死温度` |

The exact spacing/unit formatting around Celsius is RimWorld-native and is not a CCTO wording requirement.

Pass criteria:

1. normal startup completes with the real gameplay mod profile and no new CCTO-origin error;
2. the five representative cards show the expected data above;
3. `枯死温度` and `休眠温度` are readable and visually grouped sensibly with RimWorld's native plant temperature entries;
4. no duplicate CCTO temperature rows appear;
5. opening/closing the Info Card and returning to play causes no visible error or UI breakage.

Things Explorer/XML Patch Helper are optional diagnostics only if a discrepancy is found; exact underlying Vanilla/MO values are already asserted by the automated loaded-`DefDatabase` gate.

The normal-game smoke/visual check and the Nice Plants Menu compatibility smoke passed for the published 0.1 Beta baseline. After the 49-Core / 29-MO all-living-plant expansion, rerun the full gate and normal-profile smoke before publishing the updated balance.

### Nice Plants Menu compatibility smoke

When **Nice Plants Menu** (Workshop 3685058533) is active, repeat the normal-game visual check in its compact plant-summary panel:

| Case | Expected Nice Plants Menu additions |
|---|---|
| Vanilla rice | `枯死温度` -1 C |
| Vanilla hops | `休眠温度` 5 C |
| MO wheat | `枯死温度` -6 C |
| MO apple tree | `休眠温度` 5 C |

Pass criteria:

1. CCTO rows appear immediately after Nice Plants Menu's growth-temperature row;
2. fixed-death crops show only `枯死温度`;
3. dormancy-only crops show only `休眠温度`;
4. there are no duplicate CCTO rows;
5. opening/closing the plant menu produces no new CCTO error;
6. Nice Plants Menu remains optional: the same CCTO build starts normally when that mod is absent.

Dubs Mint Menus needs no dedicated compatibility smoke beyond confirming that its mouse-over standard Info Card continues to show CCTO rows; that path has already been verified.

## 5. Framework behavior that tests must protect

Regression tests should cover at least:

- plants without `ColdToleranceExtension` retain vanilla behavior;
- configured plants use one fixed Def-level `coldDeathTemperature`, not per-plant randomness;
- ordinary configured plants die below the configured threshold;
- they survive above the configured threshold;
- dormancy begins below native `minGrowthTemperature`;
- dormant plants do not die merely because vanilla `dieIfLeafless` is true;
- a dormant plant with an explicit `coldDeathTemperature` can still die under more extreme cold;
- dormancy clears after temperature recovery according to the intended CCTO behavior;
- Info Card shows the configured fixed death temperature;
- Info Card shows dormancy for dormancy-type plants;
- duplicate CCTO extensions are diagnosed;
- invalid extension configuration is diagnosed.

Items involving actual spawned-map temperature, death, dormancy persistence/recovery, and save/runtime interaction remain E2E targets rather than unit-style RimTest targets.

## 6. Current integrated source

The tested release source of truth is now `main`.

Historical development branches `framework-code` and `balance-xml` produced the framework and integrated balance work, but new release validation should be run against the current `main` state unless a new work branch is intentionally created.

## 7. Agent/model use for CCTO

Use the shared Library document for the general Sol/Astra rules.

CCTO-specific default:

- ordinary framework edits, compiler fixes, test authoring, XML API work: **Sol is sufficient**;
- escalate to **Astra** only if CCTO develops a difficult cross-mod Harmony/IL conflict, unclear multi-assembly runtime behavior, or another problem that meets the shared escalation criteria.

Do not spend Astra merely on routine CCTO file edits or compile-error loops.
