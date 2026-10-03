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

The current balance is **generally more demanding than the original Vanilla / Medieval Overhaul settings**. Most supported crops stop growing at warmer temperatures, and many ordinary crops receive fixed death thresholds that are much warmer than the original generic cold-death range. In practice, this makes cold-season farming harder and makes crop choice, seasonal timing, and temperature control more important.

This is not a universal nerf to every plant: selected perennial or overwintering crops gain cold dormancy and can survive ordinary winter cold instead of dying.

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

The tables below compare the original RimWorld / Medieval Overhaul values and behavior with the values applied by CCTO.

Vanilla RimWorld does not store a fixed species-level cold-death temperature. Its normal cold-leafless threshold is calculated per individual plant as:

`minGrowthTemperature + deterministic random offset from -18°C to -10°C`

If `dieIfLeafless=true`, crossing that threshold kills the plant. If `dieIfLeafless=false`, the plant enters the leafless state but survives. CCTO replaces this generic behavior for supported plants with explicit species-level fixed death temperatures or cold dormancy.

For CCTO dormancy crops, the CCTO minimum growth temperature is also the dormancy threshold.

### Vanilla RimWorld

| Crop | Original min growth | CCTO min growth | Original cold behavior | CCTO cold behavior |
|---|---:|---:|---|---|
| Rice | 0°C | 10°C | death at -18 to -10°C, per plant | fixed death at -1°C |
| Potato | 0°C | 5°C | death at -18 to -10°C, per plant | fixed death at -2°C |
| Corn | 0°C | 8°C | death at -18 to -10°C, per plant | fixed death at -2°C |
| Strawberry | 0°C | 5°C | death at -18 to -10°C, per plant | fixed death at -9°C |
| Haygrass | 0°C | 0°C | death at -18 to -10°C, per plant | fixed death at -9°C |
| Cotton | 0°C | 10°C | death at -18 to -10°C, per plant | fixed death at -1°C |
| Devilstrand | 0°C | 8°C | death at -18 to -10°C, per plant | fixed death at -1°C |
| Healroot | 0°C | 0°C | leafless below -18 to -10°C, survives | fixed death at -9°C |
| Hops | 0°C | 5°C | death at -18 to -10°C, per plant | cold dormancy below 5°C |
| Smokeleaf | 0°C | 5°C | death at -18 to -10°C, per plant | fixed death at -4°C |
| Psychoid | 0°C | 8°C | death at -18 to -10°C, per plant | fixed death at -1°C |
| Cocoa | 0°C | 12°C | death at -18 to -10°C, per plant | fixed death at 0°C |

### Medieval Overhaul 1.6

Medieval Overhaul's Healroot uses the Vanilla `Plant_Healroot` Def, so its before/after values are the Healroot row in the Vanilla table above.

The following are the 21 MO-specific plant Defs patched by CCTO:

| Crop | Original min growth | CCTO min growth | Original cold behavior | CCTO cold behavior |
|---|---:|---:|---|---|
| Mindwort | 0°C | 5°C | leafless below -18 to -10°C, survives | fixed death at -3°C |
| Poppy | 0°C | 5°C | leafless below -18 to -10°C, survives | fixed death at -5°C |
| Fleawort | 0°C | 3°C | leafless below -18 to -10°C, survives | fixed death at -6°C |
| Fly agaric | 0°C | 0°C | leafless below -18 to -10°C, survives | cold dormancy below 0°C |
| Onion | 0°C | 5°C | death at -18 to -10°C, per plant | fixed death at -3°C |
| Lentil | 0°C | 5°C | death at -18 to -10°C, per plant | fixed death at -4°C |
| Cabbage | -18°C | 0°C | death at -36 to -28°C, per plant | fixed death at -6°C |
| Garlic | 0°C | 0°C | death at -18 to -10°C, per plant | cold dormancy below 0°C |
| Mushroom | 0°C | 5°C | death at -18 to -10°C, per plant | fixed death at -1°C |
| Wheat | 0°C | 0°C | death at -18 to -10°C, per plant | fixed death at -6°C |
| Flax | 0°C | 5°C | death at -18 to -10°C, per plant | fixed death at -5°C |
| Sugarcane | 0°C | 10°C | death at -18 to -10°C, per plant | fixed death at -5°C |
| Carrot | 0°C | 0°C | death at -18 to -10°C, per plant | fixed death at -4°C |
| Herb | 0°C | 5°C | death at -18 to -10°C, per plant | fixed death at -3°C |
| Tomato | 0°C | 10°C | death at -18 to -10°C, per plant | fixed death at -1°C |
| Pumpkin | 0°C | 10°C | death at -18 to -10°C, per plant | fixed death at -1°C |
| Grape | 0°C | 5°C | death at -18 to -10°C, per plant | cold dormancy below 5°C |
| Apple | 0°C | 5°C | leafless below -18 to -10°C, survives | cold dormancy below 5°C |
| Mulberry | 0°C | 5°C | leafless below -18 to -10°C, survives | cold dormancy below 5°C |
| Griffon berry | 0°C | 5°C | leafless below -18 to -10°C, survives | cold dormancy below 5°C |
| Lemon | 0°C | 10°C | leafless below -18 to -10°C, survives | fixed death at -4°C |

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

## Support

[Ko-fi](https://ko-fi.com/sucro0629)
