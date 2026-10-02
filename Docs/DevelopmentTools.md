# CCTO Development Workflow

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

The authoritative automated gate is the Pickle + Quickstarts suite. It contains both framework-level regression scenarios and live-map/tick scenarios, and returns a process exit code.

Build/run with:

`run-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

The underlying E2E runner:

- builds the shipping CCTO DLL;
- creates a separate local developer mod at
  `D:\SteamLibrary\steamapps\common\RimWorld\Mods\CropColdToleranceOverhaul.E2E`;
- compiles CCTO-specific Quickstarts and Pickle step assemblies;
- copies all CCTO `.feature` files;
- launches RimWorld with Pickle autorun;
- runs framework regression scenarios and live cold-behavior scenarios;
- writes reports to `TestResults\Pickle`;
- exits with Pickle's pass/fail/error exit code.

The generated E2E mod is development-only and must not be included in the Workshop release. Remove it with:

`clean-e2e.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

For the automated run, the active development mod set must contain:

- Harmony;
- RimLogging;
- Pickle;
- Quickstarts;
- Crop Cold Tolerance Overhaul;
- `[DEV] Crop Cold Tolerance Overhaul E2E`.

The active-mod selection itself is intentionally not rewritten by the repository scripts, so normal gameplay presets are not modified behind the user's back.

### Current Pickle gate coverage

Framework regression scenarios check:

- fixed death thresholds are identical across different plant identities;
- dormancy uses native `minGrowthTemperature` as its cold-response threshold;
- unconfigured plants retain the vanilla per-plant threshold range;
- extension validation accepts dormancy-only and finite fixed death values while rejecting incomplete/infinite configurations;
- CCTO Info Card stat construction matches fixed-death, dormancy-only, and combined configurations.

Live-map scenarios check:

- a configured plant survives safely above its fixed death threshold;
- the same plant dies after the actual scheduled plant tick path runs below the threshold;
- at exactly the fixed threshold the plant survives, while below it the plant dies (strict `<` boundary);
- a `dieIfLeafless` plant can enter CCTO dormancy without dying;
- a dormant plant with an explicit extreme-cold death threshold still dies below that threshold.

Each live scenario starts from a fixed-seed Quickstarts world. The test runner creates a temporary temperature-offset game condition so test temperatures are deterministic without changing shipping balance XML.

Dormancy recovery timing is deliberately not locked by the automated suite yet. The current implementation uses vanilla `madeLeaflessTick` behavior, but the intended recovery timing should be explicitly settled before it becomes a regression contract.

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

After the automated gate passes:

1. when balance XML is present, inspect representative final Def values with Things Explorer/XML Patch Helper;
2. run a normal-game smoke test with development-only helpers disabled;
3. keep PR #1 draft until the local automated run has actually passed.

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

Framework code is currently developed on:

`framework-code`

The framework PR should remain separate from balance XML changes until both workstreams have been validated.

## 7. Agent/model use for CCTO

Use the shared Library document for the general Sol/Astra rules.

CCTO-specific default:

- ordinary framework edits, compiler fixes, test authoring, XML API work: **Sol is sufficient**;
- escalate to **Astra** only if CCTO develops a difficult cross-mod Harmony/IL conflict, unclear multi-assembly runtime behavior, or another problem that meets the shared escalation criteria.

Do not spend Astra merely on routine CCTO file edits or compile-error loops.
