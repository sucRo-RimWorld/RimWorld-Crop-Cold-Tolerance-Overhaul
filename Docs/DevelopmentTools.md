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

Run checks in this order where applicable:

1. Build `CropColdToleranceOverhaul.dll`.
2. Build the separate developer test mod with:
   `build-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"`
3. Start RimWorld and confirm CCTO/Harmony loads without red errors.
4. Enable and run the RimTest Redux suite from `[DEV] Crop Cold Tolerance Overhaul Tests`.
5. Run Pickle E2E scenarios for actual temperature-dependent behavior once the E2E fixture/steps are available.
6. When balance XML is present, verify final resolved Def values with Things Explorer/XML Patch Helper.
7. Run a normal-game smoke test with development-only helper mods disabled.

The RimTest build creates a **separate sibling local mod** at:

`D:\SteamLibrary\steamapps\common\RimWorld\Mods\CropColdToleranceOverhaul.Tests`

This keeps `RimTestRedux.dll` out of the shipping CCTO assembly and prevents the Workshop mod from acquiring a development dependency. The generated test mod can be removed with:

`clean-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

### Current RimTest coverage

The developer suite currently checks:

- an ordinary extension without a fixed death temperature is rejected;
- dormancy-only configuration is valid;
- finite fixed death temperature is valid;
- infinite death temperature is rejected;
- configured plants return the exact same fixed leafless/death threshold across different plant IDs;
- dormancy uses native `minGrowthTemperature` as the leafless threshold;
- an unconfigured plant retains the vanilla random threshold range;
- Info Card stat construction yields the expected number of CCTO entries for fixed death, dormancy-only, and dormancy-plus-extreme-death configurations.

The fixed-threshold tests intentionally verify **non-random fixed behavior**, because that is the current beta candidate. The public beta may solicit feedback on whether a per-plant range would be preferable.

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
