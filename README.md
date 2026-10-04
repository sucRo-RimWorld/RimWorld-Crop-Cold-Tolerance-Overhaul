# Crop Cold Tolerance Overhaul

**RimWorld 1.6 — 0.1.3 Beta**

**A realism-focused cold-tolerance rebalance and lightweight XML framework for RimWorld plants.**

Crop Cold Tolerance Overhaul (CCTO) is built around real-world differences in plant cold tolerance. It uses documented frost injury, chilling sensitivity, overwintering behavior, and lethal-temperature information where available as reference points, then translates them into clear RimWorld thresholds rather than applying Vanilla's mostly generic cold response.

CCTO separates a plant's **minimum growth temperature** from its **cold-death behavior**, and adds a non-lethal **cold dormancy** response where appropriate: growth stops and the plant becomes leafless in the cold instead of dying, then recovers after temperatures rise.

Framework use is also a first-class feature: other mods can define explicit **cold-death temperatures** and cold dormancy for their own plants through XML without replacing the plant class.

Vanilla RimWorld derives cold death from the growth minimum with deterministic per-plant variation. CCTO instead gives every living PlantDef in its supported plant sets a clear species-level cold response:

- ordinary plants use one fixed cold-death temperature;
- selected perennial/overwintering plants enter cold dormancy below their minimum growth temperature;
- the Info Card displays the relevant `枯死温度 / Cold death temperature` or `休眠温度 / Dormancy temperature` explicitly.

## Scope

CCTO **rebalances cold-tolerance-related values only** for all living PlantDefs in supported plant sets: minimum growth temperature, cold-death temperature, and cold dormancy behavior where appropriate. The scope includes field crops, medicinal/fiber plants, fruit trees, forestry trees, decorative plants, fungi, grasses, shrubs, and wild-only vegetation. Dead plant remnants such as stump Defs are excluded because they have no meaningful living cold-response model.

The current balance is **generally more demanding than the original Vanilla / Medieval Overhaul settings**. Most supported plants stop growing at warmer temperatures, and many ordinary plants receive fixed death thresholds that are much warmer than the original generic cold-death range. In practice, this makes cold-season farming and forestry more demanding and makes plant choice, seasonal timing, and temperature control more important.

This is not a universal nerf to every plant: selected perennial or overwintering crops gain cold dormancy and can survive ordinary winter cold instead of dying.

### Realism-focused balance rationale

The balance is intentionally realism-focused rather than a generic difficulty rebalance. CCTO uses real-world plant cold-tolerance information, including frost-damage and lethal-temperature data where available, as reference points for each plant.

Real plants vary by cultivar, growth stage, acclimation, and exposure duration, so CCTO does not copy one real-world temperature literally. The real-world data is used as a guideline, then rounded and tuned into clear gameplay thresholds that preserve meaningful differences between plants.

It does **not** rebalance other plant properties such as:

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

- all **49 living Core PlantDefs** in RimWorld 1.6; the three Core stump/remnant Defs are intentionally excluded;
- all **29 Medieval Overhaul 1.6-specific PlantDefs**, including cultivated and wild-only plants.

Medieval Overhaul's Healroot continues to use the Core `Plant_Healroot` Def, so it is covered by the Core table rather than counted again.

Ancient & Medieval Japan (AMJ) uses CCTO's framework/API from its own PlantDefs rather than making CCTO depend on AMJ. AMJ-owned plants, including crops planned for future AMJ releases, are therefore **not part of CCTO's supported or planned plant sets** and are documented on the AMJ side instead. AMJC also owns those plants' temperature data and conditional compatibility XML; CCTO supplies the framework rather than maintaining an AMJC balance table.

### Planned compatibility

Support for **Vanilla Plants Expanded (VPE)** is planned. Because the wider VPE plant catalog exceeds 100 plants, the first compatibility pass will target the **20 plants in the basic set** rather than attempting full VPE coverage at once. Additional VPE plants can be considered later.

## Supported languages

- English
- Japanese


## Current rebalance values

The tables below compare the original RimWorld / Medieval Overhaul values and behavior with the values applied by CCTO.

Vanilla RimWorld does not store a fixed species-level cold-death temperature. Its normal cold-leafless threshold is calculated per individual plant as:

`minGrowthTemperature + deterministic random offset from -18°C to -10°C`

If `dieIfLeafless=true`, crossing that threshold kills the plant. If `dieIfLeafless=false`, the plant enters the leafless state but survives. CCTO replaces this generic behavior for supported living plants with explicit species-level fixed death temperatures or cold dormancy.

For CCTO dormancy plants, the CCTO minimum growth temperature is also the dormancy threshold.

### Core / Vanilla RimWorld

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
| Agarilux | 0°C | 0°C | generic wild cold behavior | cold dormancy below 0°C |
| Agave | 0°C | 5°C | generic wild cold behavior | fixed death at -7°C |
| Alocasia | 0°C | 10°C | generic wild cold behavior | fixed death at -1°C |
| Ambrosia bush | 0°C | 0°C | generic wild cold behavior | fixed death at -9°C |
| Astragalus | 0°C | 0°C | generic wild cold behavior | cold dormancy below 0°C |
| Berry bush | 0°C | 0°C | generic wild cold behavior | cold dormancy below 0°C |
| Brambles | 0°C | 0°C | generic wild cold behavior | cold dormancy below 0°C |
| Bryolux | 0°C | 0°C | generic cave-plant cold behavior | fixed death at -12°C |
| Bush | 0°C | 0°C | generic wild cold behavior | cold dormancy below 0°C |
| Chokevine | 0°C | 0°C | generic wild cold behavior | cold dormancy below 0°C |
| Clivia | 0°C | 8°C | generic wild cold behavior | fixed death at -2°C |
| Giant rafflesia | 0°C | 12°C | generic wild cold behavior | fixed death at 5°C |
| Glowstool | 0°C | 0°C | generic cave-plant cold behavior | fixed death at -8°C |
| Grass | 0°C | 0°C | generic wild cold behavior | cold dormancy below 0°C |
| Low shrubs | 0°C | 8°C | generic wild cold behavior | fixed death at -2°C |
| Moss | 0°C | 0°C | generic wild cold behavior | cold dormancy below 0°C |
| Pincushion cactus | 0°C | 5°C | generic wild cold behavior | fixed death at -8°C |
| Tall grass | 0°C | 0°C | generic wild cold behavior | cold dormancy below 0°C |
| Wild healroot | 0°C | 0°C | generic wild cold behavior | fixed death at -9°C |

### Medieval Overhaul 1.6

Medieval Overhaul's Healroot uses the Vanilla `Plant_Healroot` Def, so its before/after values are the Healroot row in the Vanilla table above.

The following are the 29 MO-specific cultivated and wild PlantDefs patched by CCTO:

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
| Wild mindwort | 0°C | 5°C | inherited wild leafless/cold behavior | fixed death at -3°C |
| Wild poppy | 0°C | 5°C | inherited wild leafless/cold behavior | fixed death at -5°C |
| Wild fleawort | 0°C | 3°C | inherited wild leafless/cold behavior | fixed death at -6°C |
| Wild fly agaric | 0°C | 0°C | inherited wild leafless/cold behavior | cold dormancy below 0°C |
| Great oak | 0°C | 5°C | leafless and survives | cold dormancy below 5°C |
| Great Iter | 0°C | 5°C | leafless and survives | cold dormancy below 5°C |
| Great fir | 0°C | 0°C | evergreen tree cold behavior | fixed death at -35°C |
| Great willow | 0°C | 5°C | leafless and survives | cold dormancy below 5°C |

## Dependencies

Required:

- Harmony.

Optional:

- Medieval Overhaul — CCTO applies its MO plant balance only when MO is present;
- Nice Plants Menu — CCTO adds its cold-death/dormancy rows to the custom plant summary when the mod is present;
- Dubs Mint Menus — no special patch is needed; its mouse-over standard Info Card already shows CCTO entries.

## Save compatibility

- Safe to add to or remove from an existing save.
- 既存のセーブに追加または削除しても安全です。

Temperature rules apply to existing plants after loading. Plants already killed by cold are not restored by removing CCTO. Mods that require CCTO must remain installed with their dependency.

CCTO patches existing plant definitions and behavior; it does not introduce save-owned plant classes or custom serialized components. This statement is based on the current implementation review, not a separate add/remove runtime test.

## Framework use

Framework use is a core part of CCTO, not merely an internal implementation detail. Other mods can use CCTO to assign explicit cold-death temperatures and cold dormancy behavior to their own plants, without replacing the plant class.

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

The **0.1.3 Beta** corrected build passed:

- the full automated framework + Core/MO integration gate: **14/14**;
- loaded-value validation for all **49 living Core + 29 MO-specific PlantDefs**, including complete supported-living-plant coverage;
- the isolated runtime-log gate, with **no CCTO-origin ERROR entries**;
- in-game verification of the Medieval Overhaul duplicate-extension error fix.

Earlier normal-game checks also verified wild-plant Info Cards, the standard Info Card ordering, Dubs Mint Menus display, and Nice Plants Menu compatibility.

The tested release source of truth is `main`.

## Status

Current version: **0.1.3 Beta**.

This is a Beta release: the core feature set is implemented and has passed the documented automated and normal-game smoke gates. The Beta phase is for wider real-play validation, compatibility reports, and feedback on the fixed-threshold balance model.

## Support

[![Ko-fi](https://img.shields.io/badge/Ko--fi-Support%20me-ff5e5b?logo=ko-fi&logoColor=white)](https://ko-fi.com/sucro0629)

