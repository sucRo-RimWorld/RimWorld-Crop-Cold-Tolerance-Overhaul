# Crop Cold Tolerance Overhaul

**RimWorld 1.6 — Beta**

Crop Cold Tolerance Overhaul (CCTO) separates a crop's **minimum growth temperature** from its **cold-death behavior**.

CCTO also functions as a lightweight cold-tolerance framework: other mods can define explicit **cold-death temperatures** and cold dormancy for their own plants through XML without replacing the plant class.

Vanilla RimWorld derives cold death from the growth minimum with deterministic per-plant variation. CCTO instead gives supported crops a clear species-level cold response:

- ordinary crops use one fixed cold-death temperature;
- selected perennial/overwintering crops enter cold dormancy below their minimum growth temperature;
- the Info Card displays the relevant `枯死温度 / Cold death temperature` or `休眠温度 / Dormancy temperature` explicitly.

## Scope

CCTO **rebalances cold-tolerance-related values only**: minimum growth temperature, cold-death temperature, and cold dormancy behavior where appropriate.

It does **not** rebalance other crop properties such as:

- harvest yield;
- growth time;
- fertility;
- storage life;
- processing;
- research.


## Origin

CCTO was originally designed as part of **Ancient & Medieval Japan (AMJ)**, a larger RimWorld mod I plan to build.

The cold-tolerance system was split into a standalone mod because the distinction between growth temperature, cold death, and dormancy is useful well beyond AMJ. Keeping it independent also lets other crop and overhaul mods use the same XML-facing cold-tolerance framework without depending on AMJ.

## Supported crop sets

The current Beta includes balance data for:

- Vanilla RimWorld crops;
- Medieval Overhaul 1.6 crops.

Ancient & Medieval Japan (AMJ) uses CCTO's framework/API from its own PlantDefs rather than making CCTO depend on AMJ.


## Current rebalance values

These are the final cold-tolerance values currently applied by CCTO. They are the values CCTO uses in game, not candidate ranges.

For dormancy crops, the listed minimum growth temperature is also the cold-dormancy threshold.

### Vanilla RimWorld

| Crop | Minimum growth temperature | Cold-death temperature / behavior |
|---|---:|---:|
| Rice | 10°C | -1°C |
| Potato | 5°C | -2°C |
| Corn | 8°C | -2°C |
| Strawberry | 5°C | -9°C |
| Haygrass | 0°C | -9°C |
| Cotton | 10°C | -1°C |
| Devilstrand | 8°C | -1°C |
| Healroot | 0°C | -9°C |
| Hops | 5°C | cold dormancy |
| Smokeleaf | 5°C | -4°C |
| Psychoid | 8°C | -1°C |
| Cocoa | 12°C | 0°C |

### Medieval Overhaul 1.6

| Crop | Minimum growth temperature | Cold-death temperature / behavior |
|---|---:|---:|
| Healroot | 0°C | -9°C |
| Mindwort | 5°C | -3°C |
| Poppy | 5°C | -5°C |
| Fleawort | 3°C | -6°C |
| Fly agaric | 0°C | cold dormancy |
| Onion | 5°C | -3°C |
| Lentil | 5°C | -4°C |
| Cabbage | 0°C | -6°C |
| Garlic | 0°C | cold dormancy |
| Mushroom | 5°C | -1°C |
| Wheat | 0°C | -6°C |
| Flax | 5°C | -5°C |
| Sugarcane | 10°C | -5°C |
| Carrot | 0°C | -4°C |
| Herb | 5°C | -3°C |
| Tomato | 10°C | -1°C |
| Pumpkin | 10°C | -1°C |
| Grape | 5°C | cold dormancy |
| Apple | 5°C | cold dormancy |
| Mulberry | 5°C | cold dormancy |
| Griffon berry | 5°C | cold dormancy |
| Lemon | 10°C | -4°C |

## Dependencies

Required:

- Harmony.

Optional:

- Medieval Overhaul — CCTO applies its MO crop balance only when MO is present;
- Nice Plants Menu — CCTO adds its cold-death/dormancy rows to the custom plant summary when the mod is present;
- Dubs Mint Menus — no special patch is needed; its mouse-over standard Info Card already shows CCTO entries.

## Framework use

CCTO is not only a balance mod. It can also be used as a framework for assigning explicit cold-death temperatures and cold dormancy behavior to plants added by other mods, without replacing the plant class.

Example fixed cold-death threshold:

```xml
<li Class="CropColdToleranceOverhaul.ColdToleranceExtension">
  <coldDeathTemperature>-10</coldDeathTemperature>
</li>
```

Example cold dormancy:

```xml
<li Class="CropColdToleranceOverhaul.ColdToleranceExtension">
  <coldDormancy>true</coldDormancy>
</li>
```

Dormancy begins below the plant's native `minGrowthTemperature`.

See [Docs/Framework.md](Docs/Framework.md) for the XML-facing API and [Docs/Design.md](Docs/Design.md) for the balance rationale.


## Display-only version?

There is currently **no plan for a separate display-only edition**.

Vanilla RimWorld does not provide an independently balanced cold-death temperature for each crop. Its cold threshold is derived from the plant's minimum growth temperature using the same underlying rule and deterministic offset range for all plants.

CCTO's displayed `Cold death temperature` becomes useful because CCTO also changes the underlying cold behavior to explicit crop-specific thresholds or dormancy. Splitting only the display out would therefore expose Vanilla's generic rule rather than the species-specific cold-tolerance model this mod is designed around.

## Beta feedback

The initial Beta intentionally uses one exact species-level cold-death threshold instead of Vanilla-like per-plant variation.

Useful feedback includes:

- whether the fixed threshold makes seasonal planning clearer;
- whether whole-field synchronized cold death feels too abrupt;
- whether deterministic individual variation would add useful gameplay;
- whether the Info Card and plant-menu temperature information is sufficiently clear.

A possible future deterministic per-plant range mode is documented, but is **not** part of the current Beta.

## AI-assisted development

This mod was developed with AI assistance, primarily for code implementation, documentation, research, and test development. Design decisions, balance decisions, and final testing/review are performed by the author.

## Verification

The current integrated build has passed:

- the full automated framework + Vanilla/MO integration gate: **13/13**;
- normal-game Info Card smoke checks;
- Dubs Mint Menus display verification;
- Nice Plants Menu compatibility smoke checks.

The tested release source of truth is `main`.

## Status

Current target: **0.1 Beta**.

This is a Beta release: the core feature set is implemented and has passed the documented automated and normal-game smoke gates. The Beta phase is for wider real-play validation, compatibility reports, and feedback on the fixed-threshold balance model.
