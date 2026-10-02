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

## 2. CCTO extension

The intended shared API is a `DefModExtension` attached to a plant `ThingDef`.

Conceptual fields:

- cold behavior: ordinary death or cold dormancy
- fixed low-temperature death threshold for ordinary death crops

`minGrowthTemperature` remains RimWorld's native `PlantProperties` value rather than being duplicated inside the extension.

This keeps the responsibilities clear:

- native RimWorld field = growth stopping temperature
- CCTO extension = behavior below colder conditions

AMJ and compatibility mods can attach the same extension to their own PlantDefs. CCTO does not hard-code AMJ DefNames.

## 3. Harmony behavior

### 3.1 `Plant.LeaflessTemperatureThresh`

Vanilla RimWorld 1.6 calculates:

`minGrowthTemperature + deterministic random(-18, -10)`

for each plant instance.

CCTO replaces this result only when a CCTO extension exists:

- **death crop** -> fixed CCTO death threshold
- **dormancy crop** -> `minGrowthTemperature`

Plants without the CCTO extension keep the Vanilla formula unchanged.

This means the initial release has no per-plant random cold-death threshold for supported crops.

### 3.2 `Plant.MakeLeafless(LeaflessCause cause, ...)`

CCTO must change behavior only for `LeaflessCause.Cold`. Poison, pollution, no-pollution and other leafless causes must retain Vanilla behavior.

For a supported death crop:

- if the original Def already has `dieIfLeafless=true`, let Vanilla perform the cold death after CCTO supplies the fixed leafless threshold;
- if the original Def has `dieIfLeafless=false` but CCTO defines lethal cold behavior, CCTO forces the same cold-death result only for the Cold cause.

This is required for cases such as Healroot and Lemon, where CCTO wants a finite lethal cold threshold even though the originating Def can survive ordinary leaflessness.

For a supported dormancy crop:

- if the original Def already has `dieIfLeafless=false`, let Vanilla perform the nonlethal leafless state;
- if the original Def has `dieIfLeafless=true`, CCTO converts only Cold-triggered leaflessness into the normal nonlethal leafless state.

This is required for crops such as Garlic, Grape, and Hops.

Do **not** globally rewrite `dieIfLeafless` for these crops, because that would also change poison/pollution behavior outside CCTO's responsibility.

## 4. Dormancy semantics

Dormancy does not introduce a third configured temperature.

For a dormancy crop:

- active growth stops below `minGrowthTemperature`;
- below that same temperature, the plant enters the leafless/cold-dormant state;
- the plant survives;
- after temperatures recover, Vanilla's existing leafless recovery timing is reused.

This keeps the public model at two concepts: growth temperature and lethal cold behavior, while allowing perennial/surviving crops to express winter dormancy.

## 5. Information card

The plant information card already displays:

- minimum growth temperature
- maximum growth temperature

CCTO adds one entry in the same Basics section, adjacent to those values.

For ordinary death crops:

- label: localized equivalent of **Low-temperature death**
- value: the fixed configured threshold, e.g. `-6°C`
- description: temperatures below this threshold can kill the plant

For dormancy crops:

- label: localized equivalent of **Low-temperature behavior**
- value: localized equivalent of **Cold dormancy below 5°C**
- description: the plant becomes leafless/dormant below its minimum growth temperature but survives and can recover when temperatures rise

The exact display priority should be tested in-game so the new entry appears directly beside the existing growth-temperature entries.

## 6. Patch safety

- Unsupported crops keep Vanilla/originating behavior.
- Missing optional MO Defs must not generate patch errors.
- CCTO must not overwrite unrelated MO or Vanilla fields.
- Cold handling should be keyed by the extension, not by broad tests such as `Sowable`, so third-party crops are not silently changed.
- Fixed thresholds are species/Def-level values in the initial release.
- The archived range tables remain available for a later optional deterministic per-plant randomized mode.
