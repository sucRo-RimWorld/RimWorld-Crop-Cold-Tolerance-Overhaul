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

- crop-specific `minGrowthTemperature` values;
- crop-specific fixed `coldDeathTemperature` values;
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

## 4. CCTO validation sequence

### Automated release gate

The authoritative automated gate is `run-tests.bat`. On the integrated balance branch/final mod it performs both static balance validation and the Pickle + Quickstarts runtime suite, and returns a process exit code.

Build/run with:

`run-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

The full gate requires Medieval Overhaul plus its declared required dependencies to be installed in the same Steam library:

- Medieval Overhaul — Workshop 3219596926;
- Vanilla Expanded Framework — Workshop 2023507013;
- [SYR] Processor Framework — Workshop 3210544395.

`run-e2e.bat` remains usable by itself for framework-only testing without Medieval Overhaul. `run-tests.bat` invokes its `with-mo` mode for the complete integration gate.

The underlying E2E runner:

- builds the shipping CCTO DLL;
- creates a separate local developer mod at
  `D:\SteamLibrary\steamapps\common\RimWorld\Mods\CropColdToleranceOverhaul.E2E`;
- compiles CCTO-specific Quickstarts and Pickle step assemblies;
- copies all CCTO `.feature` files;
- prepares an isolated RimWorld save-data profile under `TestResults\SaveData`;
- copies the normal `Prefs.xml` into that isolated profile and forces only `devMode=True`, because Quickstarts does nothing when RimWorld dev mode is off;
- gives that profile a minimal test-only mod list; framework-only runs contain Harmony, Core, RimLogging, Pickle, Quickstarts, CCTO, and the CCTO E2E companion mod, while the full integration gate additionally enables Vanilla Expanded Framework, Processor Framework, and Medieval Overhaul;
- launches RimWorld with `-savedatafolder` pointing at that isolated profile;
- runs framework regression scenarios and live cold-behavior scenarios;
- writes reports to `TestResults\Pickle`;
- exits with Pickle's pass/fail/error exit code;
- is wrapped by an outer five-minute process watchdog, so a RimWorld/Pickle startup or runtime freeze cannot leave the batch file waiting indefinitely.

The user's normal RimWorld `ModsConfig.xml` is read only to reuse the current RimWorld version/known-expansion metadata. It is not rewritten. This prevents unrelated gameplay mods and their log errors from causing false Pickle failures.

The generated E2E mod is development-only and must not be included in the Workshop release. Remove it with:

`clean-e2e.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

The automated Pickle gate no longer depends on the user's normal active mod preset. `Scripts/Prepare-TestSaveData.ps1` writes a separate test-only `ModsConfig.xml` inside `TestResults\SaveData`, and `run-e2e.bat` launches RimWorld against that profile.

The normal gameplay preset is therefore left untouched, and unrelated mods do not need to be manually disabled before an E2E run.

`Scripts/Check-TestModList.ps1` is retained only for workflows that intentionally run development helpers against the normal active profile; it is not part of the authoritative Pickle release gate.

### Current Pickle gate coverage

Loaded balance-Def scenarios in the full integration gate check:

- all 12 targeted Vanilla plant Defs have the final expected `minGrowthTemperature`, exactly one CCTO extension, and the expected fixed death temperature or dormancy flag;
- all 21 targeted Medieval Overhaul plant Defs have the same final loaded-value checks;
- Medieval Overhaul's installed 1.6 source XML contains every targeted DefName before the runtime suite starts.

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

After the full automated gate passes:

1. optionally spot-check representative Defs with Things Explorer/XML Patch Helper if investigating UI/load-order concerns; exact Vanilla/MO balance values are already asserted from the loaded `DefDatabase` by the integration gate;
2. run a normal-game smoke test with development-only helpers disabled;
3. keep the relevant PR Draft until its required local automated gate has actually passed.

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

## 6. Current implementation branch

Framework code is developed on `framework-code`. Integrated Vanilla/Medieval Overhaul balance XML and its full integration gate are developed on `balance-xml`.

The framework and balance PRs remain separate until the integrated balance branch passes the full `run-tests.bat` gate.

## 7. Agent/model use for CCTO

Use the shared Library document for the general Sol/Astra rules.

CCTO-specific default:

- ordinary framework edits, compiler fixes, test authoring, XML API work: **Sol is sufficient**;
- escalate to **Astra** only if CCTO develops a difficult cross-mod Harmony/IL conflict, unclear multi-assembly runtime behavior, or another problem that meets the shared escalation criteria.

Do not spend Astra merely on routine CCTO file edits or compile-error loops.
