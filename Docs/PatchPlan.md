# Implementation and Patch Plan

## 1. Supported data application

### Vanilla

Vanilla crop cold-tolerance data is always applied.

CCTO changes only:

- `PlantProperties.minGrowthTemperature`
- CCTO's own cold-death / dormancy behavior

It does not touch harvest, growth time, fertility, research, products, or processing.

### Medieval Overhaul

Medieval Overhaul support is optional and applies only when MO is present.

The uploaded MO 1.6 data was audited. Its Vanilla crop changes do not conflict with CCTO's temperature responsibility:

- `Plant_Corn`: MO changes graphics and adds an agriculture research prerequisite.
- `Plant_Cotton`: MO's cloth-chain setting may change `harvestedThingDef`.
- MO does not need CCTO to replace those changes.

Therefore CCTO can apply the normal Vanilla temperature data regardless of MO, then add MO-specific cold-tolerance data only to MO's own crop Defs.

The balance/XML workstream uses an optional `loadAfter` entry for `DankPyon.Medieval.Overhaul` so MO Defs exist before CCTO's MO-specific XML patches are applied.

## 2. CCTO extension — implemented API

The code/framework workstream has already implemented `ColdToleranceExtension : DefModExtension`.

Current fields:

- `float coldDeathTemperature = float.NaN`
- `bool coldDormancy`

The two fields may coexist. This intentionally supports three configurations:

1. fixed cold death only;
2. cold dormancy only;
3. cold dormancy below `minGrowthTemperature` plus death under a more extreme fixed `coldDeathTemperature`.

For the initial CCTO balance tables, current dormancy crops use dormancy only. The third mode remains available to compatibility mods and future balance revisions.

`minGrowthTemperature` remains RimWorld's native `PlantProperties` value rather than being duplicated inside the extension.

AMJ and compatibility mods can attach the same extension to their own PlantDefs. CCTO does not hard-code AMJ DefNames.

## 3. Harmony behavior — current framework implementation

The framework implementation is integrated on `main`.

### 3.1 `Plant.LeaflessTemperatureThresh`

Vanilla RimWorld 1.6 calculates:

`minGrowthTemperature + deterministic random(-18, -10)`

for each plant instance.

CCTO applies a postfix only when `ColdToleranceExtension` exists:

- if `coldDormancy=true`, the leafless trigger becomes native `minGrowthTemperature`;
- otherwise, if `coldDeathTemperature` is configured, the trigger becomes that fixed value;
- plants without the extension keep the Vanilla formula unchanged.

When dormancy and a death threshold coexist, the leafless trigger remains `minGrowthTemperature`; the colder lethal threshold is evaluated separately by the Cold handling patches.

### 3.2 `Plant.MakeLeafless`

The implemented prefix changes behavior only for `LeaflessCause.Cold`.

Order:

1. if a fixed death threshold exists and ambient temperature is below it, CCTO performs the standard cold-death outcome;
2. otherwise, if dormancy is enabled and ambient temperature is below `minGrowthTemperature`, CCTO enters nonlethal cold dormancy;
3. otherwise Vanilla `MakeLeafless` continues normally.

Poison, pollution, no-pollution and other causes are not intercepted.

CCTO does **not** globally rewrite `dieIfLeafless`, so unrelated leafless causes retain the originating Def's behavior.

### 3.3 `Plant.CheckMakeLeafless`

The current framework also applies a postfix to `CheckMakeLeafless` that rechecks:

- fixed lethal cold;
- cold dormancy.

This supports the live cold-response path used by the automated integration tests.

**Decided indoor semantics:** CCTO intentionally evaluates configured plants from the plant's actual `AmbientTemperature` even when Vanilla's original cold-leafless path would be gated by `room.UsesOutdoorTemperature`.

Therefore:

- a heated indoor greenhouse remains safe when its actual room/ambient temperature is above the configured cold threshold;
- a genuinely cold indoor room can trigger CCTO dormancy or cold death;
- outdoor temperature by itself does not bypass a warm room.

This behavior is part of CCTO's temperature model and should be covered by indoor-safe and indoor-cold E2E regression scenarios.

## 4. Dormancy semantics

Dormancy does not require a third configured temperature.

For the current balance data:

- active growth stops below `minGrowthTemperature`;
- below that same temperature, the plant enters a leafless/cold-dormant state;
- the plant survives;
- while cold persists, the dormancy state is refreshed;
- after temperature recovery, CCTO stops refreshing the cold-dormancy state and **RimWorld's existing delayed leafless recovery timing is intentionally preserved**.

CCTO does not explicitly clear the leafless state the moment temperature rises. Visible/state recovery may therefore lag warming by up to roughly one in-game day, matching the existing 60,000-tick leafless window. This is intentional and should be protected by a regression test.

The framework additionally supports dormancy plus a separate extreme-cold lethal threshold, although the initial balance tables do not currently use that combination.

## 5. Information card — current implementation

The framework already patches `ThingDef.SpecialDisplayStats`.

RimWorld's native entries are:

- minimum growth temperature — priority 4152;
- maximum growth temperature — priority 4153.

CCTO currently adds:

- **dormancy temperature** for dormancy crops — priority 4151;
- **cold-death temperature** when configured — priority 4150.

The dormancy temperature is not an independent third balance value. It is displayed directly from the plant's native `minGrowthTemperature`, because CCTO enters dormancy below that same threshold.

A plant configured with both dormancy and extreme-cold death displays both temperatures.

Current Japanese labels are:

- `休眠温度`
- `枯死温度`

The previous `低温反応: 休眠` display was replaced because it did not tell the player at what temperature dormancy begins. The display is covered by framework tests and must be rechecked visually in-game before release.

## 5.1 Plant-selection UI compatibility

CCTO's canonical display path is RimWorld's standard `ThingDef.SpecialDisplayStats` / Info Card.

Priority compatibility set for alternate plant-selection displays:

1. **Nice Plants Menu** — Steam Workshop 3685058533, packageId `Andromeda.NicePlantsMenu`;
2. **Dubs Mint Menus** — Steam Workshop 1446523594, packageId `Dubwise.DubsMintMenus`.

Compatibility rules:

- neither UI mod is a hard dependency;
- compatibility must be soft/conditional;
- Nice Plants Menu uses its own compact plant-summary panel and therefore needs a dedicated compatibility path for `枯死温度` / `休眠温度`;
- the Nice Plants Menu compatibility implementation is soft/conditional and uses reflection/Harmony only when packageId `Andromeda.NicePlantsMenu` is active. It patches `NicePlantsMenu.Dialog_PlantBrowser.DrawInfoBlock` and inserts CCTO rows immediately after the `NPM_GrowthTemperature` row, reusing Nice Plants Menu's own row layout, temperature icon, scrolling, and tooltip behavior;
- the audited Nice Plants Menu DLL supplied during development has SHA-256 `f82a8c52724bf24ca93e966d9b5881e3788655e64656bacdebaaffb9e1264c9a`. The relevant 1.6 signature is `DrawInfoBlock(ref float y, float x, float totalWidth, string label, Texture2D icon, string valueStr, Func<TaggedString> tooltip, Color? color)`, and the displayed plant is resolved through `Dialog_PlantBrowser.drawInfoFor -> PlantRecord.plant`;
- if Nice Plants Menu changes these reflected members in a later update, CCTO must fail soft with a warning rather than make Nice Plants Menu a hard dependency;
- Dubs Mint Menus exposes RimWorld's standard Info Card on plant-menu mouse-over. Normal-game verification confirmed CCTO's `SpecialDisplayStats` rows already appear there, so **no Dubs-specific compatibility patch is required**;
- Dubs Mint Menus compatibility is satisfied through the canonical standard Info Card path;
- other plant-selection UI mods remain best-effort/on-request rather than release blockers.

## 6. Balance XML integration

The balance/XML workstream must consume the implemented extension rather than introducing another C# data model.

For each supported crop:

1. set or replace native `plant/minGrowthTemperature`;
2. add exactly one `CropColdToleranceOverhaul.ColdToleranceExtension`;
3. set either:
   - `coldDeathTemperature`, or
   - `coldDormancy=true`;
4. do not touch harvest, growth days, fertility, research, products, or processing.

Vanilla values are unconditional.

MO-specific values are gated on Medieval Overhaul and target the verified MO 1.6 DefNames.

## 7. Patch safety

- Unsupported crops keep Vanilla/originating behavior.
- CCTO must not overwrite unrelated MO or Vanilla fields.
- Cold handling is keyed by the extension, not by broad tests such as `Sowable`.
- Fixed thresholds are species/Def-level values in the initial release.
- The archived range tables remain available for a later optional deterministic per-plant randomized mode.
- Framework and balance may still be developed as separate workstreams, but the tested integrated state on `main` is the release source of truth.

## 8. Verification status

As of 2026-10-03:

- The C# framework, Vanilla/MO balance XML, Info Card changes, and Nice Plants Menu compatibility are integrated on `main`.
- Shipping CCTO DLL has compiled successfully against the user's RimWorld 1.6 installation.
- RimTest Redux and Pickle + Quickstarts test infrastructure is present.
- The live Pickle suite passed all existing scenarios after the boundary-test temperature synchronization fix. This confirmed fixed-threshold behavior, Vanilla fallback, extension validation, Info Card construction, live cold death, strict threshold boundary, dormancy survival, and extreme-cold death.
- Additional regression scenarios have since been added for the decided delayed dormancy recovery semantics and for indoor actual-room-temperature behavior.
- The first expanded-suite run was contaminated by unrelated `Andromeda.PawnQuickInfo` `Log.Error` output; Pickle treated those external errors as scenario failures even though they were not CCTO assertion failures.
- The authoritative E2E runner now launches RimWorld with an isolated `-savedatafolder` profile containing only the required test mods, while leaving the user's normal gameplay mod configuration untouched.
- A later isolated-profile run reached 10 of 11 scenarios passed. The delayed-recovery and indoor actual-room-temperature scenarios both passed.
- The sole remaining failure was a Vanilla sound-update `NullReferenceException` while reading `MapTemperature.OutdoorTemp` during the strict fixed-threshold boundary scenario; the CCTO plant state itself matched the expected boundary result.
- CCTO E2E temperature control no longer mutates global outdoor temperature through `GameCondition_TemperatureOffset` or world tile-temperature cache clears. It now sets the relevant RimWorld `Room.Temperature` directly and verifies the plant's actual `AmbientTemperature`. The indoor test uses a separate outdoor room to model cold outside versus warm inside.
- The final isolated Pickle run completed successfully with the full 11/11 suite passing.
- The automated release gate is now green. The framework implementation has passed fixed-threshold, Vanilla fallback, validation, Info Card, live cold-death, strict boundary, dormancy, delayed recovery, extreme-cold, and indoor actual-room-temperature regression coverage.
- PR #2 merged the tested framework + balance integration into `main` at `a1616054a7d081f3167db501a49dfe3630ce4402`.
- The integrated balance branch subsequently passed the complete `run-tests.bat` gate with a fresh **13/13 Pickle pass**, including the two loaded-balance scenarios.
- The integration gate verified the final loaded CCTO values for all 12 targeted Vanilla plants and all 21 targeted Medieval Overhaul plants, while also validating the target DefNames and required local `<plant>` nodes against the installed real MO 1.6 source.
- After the dormancy UI was changed from `低温反応: 休眠` to explicit `休眠温度 <minGrowthTemperature>`, the full integrated `run-tests.bat` gate was rerun locally and again passed **13/13**. The automated framework + Vanilla/MO balance verification is therefore green for the current dormancy-temperature implementation. The documented normal-game smoke/visual recheck also passed. Dubs Mint Menus was verified to show CCTO through the standard Info Card path without dedicated code. After the Nice Plants Menu compatibility implementation and its legacy-build fixes, the full integrated `run-tests.bat` gate passed **13/13** again. The manual Nice Plants Menu visual smoke check also passed without a new CCTO-origin error. All documented pre-merge automated and manual gates are green.
