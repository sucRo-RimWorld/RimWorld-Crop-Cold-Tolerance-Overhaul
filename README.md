# Crop Cold Tolerance Overhaul

**RimWorld 1.6 — Beta**

Crop Cold Tolerance Overhaul (CCTO) separates a plant's **minimum growth temperature** from its **cold-death behavior**, and adds a non-lethal **cold dormancy** response where appropriate: growth stops and the plant becomes leafless in the cold instead of dying, then recovers after temperatures rise.

CCTO also functions as a lightweight cold-tolerance framework: other mods can define explicit **cold-death temperatures** and cold dormancy for their own plants through XML without replacing the plant class.

Vanilla RimWorld derives cold death from the growth minimum with deterministic per-plant variation. CCTO instead gives supported player-sowable plants a clear species-level cold response:

- ordinary plants use one fixed cold-death temperature;
- selected perennial/overwintering plants enter cold dormancy below their minimum growth temperature;
- the Info Card displays the relevant `枯死温度 / Cold death temperature` or `休眠温度 / Dormancy temperature` explicitly.

## Scope

CCTO **rebalances cold-tolerance-related values only** for supported player-sowable plants: minimum growth temperature, cold-death temperature, and cold dormancy behavior where appropriate. The scope includes field crops, medicinal/fiber plants, fruit trees, forestry trees, decorative plants, fungi, and other plants exposed through sowing UI. Wild-only plants that the player cannot sow are not part of the default balance set.

The current balance is **generally more demanding than the original Vanilla / Medieval Overhaul settings**. Most supported plants stop growing at warmer temperatures, and many ordinary plants receive fixed death thresholds that are much warmer than the original generic cold-death range. In practice, this makes cold-season farming and forestry more demanding and makes plant choice, seasonal timing, and temperature control more important.

This is not a universal nerf to every plant: selected perennial or overwintering crops gain cold dormancy and can survive ordinary winter cold instead of dying.

### Balance rationale

The values are not arbitrary. CCTO uses real-world crop cold-tolerance information, including frost-damage and lethal-temperature data where available, as reference points for each crop.

Real plants vary by cultivar, growth stage, acclimation, and exposure duration, so CCTO does not copy one real-world temperature literally. The real-world data is used as a guideline, then rounded and tuned into clear gameplay thresholds that preserve meaningful differences between crops.

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

## Supported plant sets

The current balance set covers:

- all 30 player-sowable Core plants in RimWorld 1.6, including crops, decorative plants, forestry trees, fungi, and special sowable plants;
- all 21 Medieval Overhaul 1.6-specific cultivated plant Defs used by CCTO.

Medieval Overhaul's Healroot continues to use the Core `Plant_Healroot` Def, so it is covered by the Core table rather than counted again.

Ancient & Medieval Japan (AMJ) uses CCTO's framework/API from its own PlantDefs rather than making CCTO depend on AMJ.

## Supported languages

- English
- Japanese


## Current rebalance values

The tables below compare the original RimWorld / Medieval Overhaul values and behavior with the values applied by CCTO.

Vanilla RimWorld does not store a fixed species-level cold-death temperature. Its normal cold-leafless threshold is calculated per individual plant as:

`minGrowthTemperature + deterministic random offset from -18°C to -10°C`

If `dieIfLeafless=true`, crossing that threshold kills the plant. If `dieIfLeafless=false`, the plant enters the leafless state but survives. CCTO replaces this generic behavior for supported player-sowable plants with explicit species-level fixed death temperatures or cold dormancy.

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
| Bamboo | 0°C | 10°C | generic tree cold behavior | fixed death at -18°C |
| Birch | 0°C | 5°C | generic deciduous-tree cold behavior | cold dormancy below 5°C |
| Cecropia | 0°C | 10°C | generic tree cold behavior | fixed death at 0°C |
| Cypress | 0°C | 5°C | generic tree cold behavior | cold dormancy below 5°C |
| Dandelion | 0°C | 0°C | generic plant cold behavior | cold dormancy below 0°C |
| Daylily | 0°C | 0°C | generic plant cold behavior | cold dormancy below 0°C |
| Drago tree | 0°C | 8°C | generic tree cold behavior | fixed death at 0°C |
| Maple | 0°C | 5°C | generic deciduous-tree cold behavior | cold dormancy below 5°C |
| Oak | 0°C | 5°C | generic deciduous-tree cold behavior | cold dormancy below 5°C |
| Palm | 0°C | 10°C | generic tree cold behavior | fixed death at 0°C |
| Pine | 0°C | 0°C | generic tree cold behavior | fixed death at -35°C |
| Poplar | 0°C | 5°C | generic deciduous-tree cold behavior | cold dormancy below 5°C |
| Rose | 0°C | 5°C | generic plant cold behavior | cold dormancy below 5°C |
| Saguaro cactus | 0°C | 8°C | generic plant cold behavior | fixed death at -6°C |
| Teak | 0°C | 12°C | generic deciduous-tree cold behavior | fixed death at 3°C |
| Timbershroom | 0°C | 0°C | generic plant cold behavior | cold dormancy below 0°C |
| Tinctoria | 0°C | 5°C | generic plant cold behavior | fixed death at -4°C |
| Willow | 0°C | 5°C | generic deciduous-tree cold behavior | cold dormancy below 5°C |

### Medieval Overhaul 1.6

Medieval Overhaul's Healroot uses the Vanilla `Plant_Healroot` Def, so its before/after values are the Healroot row in the Vanilla table above.

The following are the 21 MO-specific cultivated plant Defs patched by CCTO:

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

- Medieval Overhaul — CCTO applies its MO plant balance only when MO is present;
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

The current balance model intentionally uses one exact species-level cold-death threshold instead of Vanilla-like per-plant variation.

Useful feedback includes:

- whether the fixed threshold makes seasonal planning clearer;
- whether whole-field synchronized cold death feels too abrupt;
- whether deterministic individual variation would add useful gameplay;
- whether the Info Card and plant-menu temperature information is sufficiently clear.

A possible future deterministic per-plant range mode is documented, but is **not** part of the current Beta.

## AI-assisted development

This mod was developed with AI assistance, primarily for code implementation, documentation, research, and test development. Design decisions, balance decisions, and final testing/review are performed by the author.

## Verification

The published 0.1 Beta baseline previously passed:

- the full automated framework + Vanilla/MO integration gate: **13/13**;
- normal-game Info Card smoke checks;
- Dubs Mint Menus display verification;
- Nice Plants Menu compatibility smoke checks.

The subsequent expansion from 12 to 30 Core player-sowable plants changes balance XML and loaded-Def expectations, so the full release gate must be rerun before that expanded balance is treated as verified for publication.

The tested release source of truth is `main`.

## Status

Current target: **0.1 Beta**.

This is a Beta release: the core feature set is implemented and has passed the documented automated and normal-game smoke gates. The Beta phase is for wider real-play validation, compatibility reports, and feedback on the fixed-threshold balance model.

## Support

[![Ko-fi](https://img.shields.io/badge/Ko--fi-Support%20me-ff5e5b?logo=ko-fi&logoColor=white)](https://ko-fi.com/sucro0629)
