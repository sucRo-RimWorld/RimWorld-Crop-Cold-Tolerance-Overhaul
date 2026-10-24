# RimWorld Crop Cold Tolerance Overhaul — Design

## 1. Scope

This mod rebalances cold tolerance for all plant Defs in supported plant sets.

Scope is determined by plant identity, not by player sowability. For supported sets such as Core and Medieval Overhaul, every gameplay PlantDef is in scope: cultivated crops, medicinal/fiber plants, fruit trees, forestry trees, decorative plants, fungi, grasses, shrubs, wild trees, and other wild-only plants. Sowability affects gameplay use but does not determine whether a plant receives an explicit CCTO cold-response model.

Wild plants are balanced differently from crops: their values must remain compatible with the climates/biomes in which they naturally occur so that CCTO does not cause implausible routine winter die-off or destabilize biome vegetation and grazing. The same temperature model still applies, but biome survival is a primary constraint for wild-only species.

It is responsible for:

- minimum growth temperature (`minGrowthTemperature`)
- plant-specific low-temperature death thresholds
- displaying low-temperature death behavior in the plant information card
- special low-temperature behavior such as dormancy where appropriate

It does **not** rebalance:

- harvest yield
- growth days
- fertility requirements or fertility sensitivity
- storage life
- processing
- research progression

The balance philosophy follows Ancient & Medieval Japan (AMJ): real-world cold tolerance is used as evidence, but final values are chosen so that plants have clear gameplay identities and meaningful climate/season tradeoffs. Where available, real-world frost-damage and lethal-temperature information is used as a reference for plant-specific death thresholds. Because actual plant response varies by cultivar, growth stage, acclimation, and exposure duration, CCTO treats those temperatures as guidelines rather than copying a single reported value literally; values are rounded/tuned into clear gameplay thresholds.

The intended difficulty direction is generally upward relative to the original Vanilla / Medieval Overhaul settings. Most supported plants should require warmer conditions for active growth, and ordinary plants should usually face lethal cold at substantially warmer temperatures than the original generic derived threshold. The gameplay purpose is to make plant selection, seasonal timing, and temperature management matter more. This is an overall balance direction rather than a universal nerf: selected perennial/overwintering plants intentionally gain cold dormancy and winter survival.

## 1.1 Standalone-mod rationale

CCTO originated during the design of **Ancient & Medieval Japan (AMJ)**. It was separated from AMJ because crop cold-tolerance behavior is broadly useful and does not need to depend on AMJ's larger historical content scope.

The standalone split also establishes a reusable dependency direction:

- CCTO owns the generic cold-tolerance mechanism;
- AMJ and other mods may consume that mechanism;
- CCTO does not require AMJ.

### Display-only variant policy

A separate display-only variant is **not currently planned**.

Vanilla RimWorld 1.6 does not store a species-specific fixed cold-death temperature for each crop. Its native cold leafless/death threshold is derived from `minGrowthTemperature` using the same deterministic offset rule for plants:

`minGrowthTemperature + Rand.RangeSeeded(-18f, -10f, thingIDNumber ^ 0x31F3A5C1)`

Therefore a display-only extraction would mainly expose Vanilla's generic derived rule, not the crop-specific cold-death data that CCTO introduces.

CCTO's temperature display is intentionally coupled to its behavior model: supported plants receive explicit species-level death thresholds or dormancy behavior, and the UI reports those actual CCTO semantics.

## 2. Temperature model

Growth stopping and plant death are separate concepts.

- **Minimum growth temperature**: below this temperature the plant stops growing.
- **Low-temperature death threshold**: each supported plant has one fixed species-level threshold. Below this temperature the crop may die.
- The initial release does **not** randomize the death threshold per plant. All plants of the same Def use the same configured threshold.
- Where appropriate, a plant enters **cold dormancy** instead of dying.
- Range-based values used during balancing are archived in §6. They remain candidate data for a possible future randomized mode.

## 2.1 Implementation ownership and dependency direction

CCTO owns the common cold-tolerance mechanism. It should not depend on AMJ.

- All gameplay PlantDefs in the supported Core and Medieval Overhaul sets are patched by CCTO, including wild-only plants.
- AMJC owns its crop-specific growth/death/dormancy values, design tables, candidate ranges, Def mapping, and optional compatibility XML in the AMJC repository. It consumes the CCTO extension/API from that side; CCTO contains no AMJC-owned crop data.
- AMJE follows the same owning-mod pattern for its own plants.
- CCTO therefore does not need to hard-code AMJ DefNames.
- This keeps the dependency direction one-way: **AMJ -> CCTO mechanism**, never CCTO -> AMJ.
- Other third-party crop mods can later add compatibility by attaching the same extension without CCTO needing to know their internal DefNames.

For ordinary plants, the extension stores one fixed low-temperature death threshold. For dormancy plants, the extension marks cold behavior as dormancy rather than supplying a lethal threshold.

A dormancy plant should enter its cold/leafless state when temperature falls below its configured minimum growth temperature and should survive that state. This avoids introducing a third temperature axis solely for dormancy.


## 3. Fixed implementation values — Medieval Overhaul 1.6

### 3.1 Medicinal crops

Medieval Overhaul medicinal crops must not all inherit the same cold tolerance merely because they share `HealrootBase`.

Cold tolerance is assigned per plant so that medicinal crop choice matters, especially once advanced medicine recipes explicitly require particular herbs.

| Crop | Minimum growth temperature | Fixed death threshold / behavior | Role |
|---|---:|---:|---|
| Healroot | 0°C | -9°C | baseline cold-hardy medicinal crop |
| Mindwort | 5°C | -3°C | delicate medicinal herb; relatively frost-sensitive |
| Poppy | 5°C | -5°C | cool-season medicinal crop |
| Fleawort | 3°C | -6°C | hardy medicinal crop |
| Fly agaric | 0°C | cold dormancy | fungal crop that survives ordinary winter cold |

Design notes:

- `Mindwort`, `Poppy`, `Fleawort`, and `Fly agaric` currently inherit from `HealrootBase` in Medieval Overhaul, but this inheritance is not treated as evidence that their cold tolerance should be identical.
- `Fleawort` is deliberately hardier than `Mindwort`.
- `Fly agaric` should not behave like a normal annual crop that dies permanently from ordinary winter cold.
- The information card should show the fixed death threshold for ordinary crops and explicit dormancy wording for special crops.

### 3.2 Food and fiber crops

| Crop | Minimum growth temperature | Fixed death threshold / behavior | Role |
|---|---:|---:|---|
| Onion | 5°C | -3°C | cool-season crop, but not strongly frost-hardy |
| Lentil | 5°C | -4°C | relatively cold-tolerant pulse |
| Cabbage | 0°C | -6°C | strongly frost-tolerant vegetable |
| Garlic | 0°C | cold dormancy | overwintering crop |
| Mushroom | 5°C | -1°C | above-ground fungal crop vulnerable to freezing |
| Wheat | 0°C | -6°C | cold-climate cereal |
| Flax | 5°C | -5°C | cool-season fiber crop with good frost tolerance |
| Sugarcane | 10°C | -5°C | requires warmth for growth but tolerates light freezing better than its growth requirement implies |
| Carrot | 0°C | -4°C | cool-season root crop |
| Herb | 5°C | -3°C | generic temperate herb |
| Tomato | 10°C | -1°C | warm-season, frost-sensitive |
| Pumpkin | 10°C | -1°C | warm-season, frost-sensitive |

The Medieval Overhaul 1.6 cabbage value `minGrowthTemperature=-18°C` is treated as a balance error for this overhaul and is replaced with 0°C. Cold survival and active growth are intentionally kept separate.

MO fungus coverage follows the same all-PlantDef rule as every other plant. `DankPyon_Plant_Mushrooms` and `DankPyon_Plant_FlyAgaric` are cultivated targets. The separate `DankPyon_Plant_FlyAgaricWild` Def explicitly clears its inherited `sowTags`, but it is still in scope because wild-only PlantDefs now receive explicit CCTO cold behavior as well.

### 3.3 Perennial crops

| Crop | Minimum growth temperature | Fixed death threshold / behavior |
|---|---:|---:|
| Grape | 5°C | cold dormancy |
| Apple | 5°C | cold dormancy |
| Mulberry | 5°C | cold dormancy |
| Griffon berry | 5°C | cold dormancy |
| Lemon | 10°C | -4°C |

Grape is changed from ordinary leafless death behavior to cold dormancy. Apple, mulberry, and Griffon berry already have leafless-survival behavior in Medieval Overhaul and retain the same conceptual role. Lemon remains a warm-climate perennial and receives a real low-temperature death threshold rather than unlimited dormancy survival.


### 3.4 Wild-only Medieval Overhaul plants

Wild counterparts of cultivated alchemy plants use the same cold model as the cultivated form because they represent the same species. Dark Forest giant trees are balanced to survive routine snowy winters in their native biome.

| Plant | Minimum growth temperature | Fixed death threshold / behavior | Role |
|---|---:|---:|---|
| Wild mindwort | 5°C | -3°C | same species model as cultivated mindwort |
| Wild poppy | 5°C | -5°C | same species model as cultivated poppy |
| Wild fleawort | 3°C | -6°C | same species model as cultivated fleawort |
| Wild fly agaric | 0°C | cold dormancy | same species model as cultivated fly agaric |
| Great oak | 5°C | cold dormancy | ancient deciduous Dark Forest tree |
| Great Iter | 5°C | cold dormancy | giant leaf-shedding Dark Forest tree |
| Great fir | 0°C | -35°C | evergreen snow-forest giant; aligned with Core pine-class severe-cold survival |
| Great willow | 5°C | cold dormancy | ancient deciduous willow |


## 4. Fixed implementation values — Core / Vanilla RimWorld 1.6

| Crop | Minimum growth temperature | Fixed death threshold / behavior | Role |
|---|---:|---:|---|
| Rice | 10°C | -1°C | warm-season grain; frost-sensitive |
| Potato | 5°C | -2°C | cool-climate crop but frost-sensitive |
| Corn | 8°C | -2°C | warm-season cereal |
| Strawberry | 5°C | -9°C | growth slows early, but the plant itself is strongly cold-hardy |
| Haygrass | 0°C | -9°C | cold-climate forage crop |
| Cotton | 10°C | -1°C | warm-climate fiber crop |
| Devilstrand | 8°C | -1°C | fictional warm-climate fungal crop |
| Healroot | 0°C | -9°C | cold-hardy medicinal crop |
| Hops | 5°C | cold dormancy | perennial rhizome; above-ground growth dies back in winter |
| Smokeleaf | 5°C | -4°C | relatively frost-tolerant compared with tropical crops |
| Psychoid | 8°C | -1°C | fictional warm-climate drug crop |
| Cocoa | 12°C | 0°C | tropical crop with very poor cold tolerance |
| Bamboo | 10°C | -18°C | Japanese temperate bamboo (moso/madake baseline); evergreen and winter-surviving, but active shoot development requires warmth |
| Birch | 5°C | cold dormancy | cold-hardy deciduous forestry tree |
| Cecropia | 10°C | 0°C | frost-sensitive tropical tree |
| Cypress | 5°C | cold dormancy | swamp cypress / bald-cypress style deciduous conifer |
| Dandelion | 0°C | cold dormancy | cold-hardy perennial herb |
| Daylily | 0°C | cold dormancy | herbaceous perennial that dies back over winter |
| Drago tree | 8°C | 0°C | evergreen warm/arid-climate tree; cold-sensitive but hardier than tropical rainforest trees |
| Maple | 5°C | cold dormancy | cold-hardy deciduous forestry tree |
| Oak | 5°C | cold dormancy | temperate deciduous forestry tree |
| Palm | 10°C | 0°C | generic warm-climate palm |
| Pine | 0°C | -35°C | evergreen boreal/tundra forestry tree; survives severe cold without leafless dormancy |
| Poplar | 5°C | cold dormancy | cold-hardy deciduous forestry tree |
| Rose | 5°C | cold dormancy | temperate perennial shrub |
| Saguaro cactus | 8°C | -6°C | desert plant; growth requires warmth but brief freezing is survivable |
| Teak | 12°C | 3°C | tropical hardwood with very poor frost tolerance |
| Timbershroom | 0°C | cold dormancy | fictional cave fungus; cold-stable non-lethal response chosen for gameplay |
| Tinctoria | 5°C | -4°C | fictional engineered dye crop; temperate annual-style gameplay baseline |
| Willow | 5°C | cold dormancy | cold-hardy deciduous forestry tree |


### 4.1 Core wild-only plants

Wild plants are not tuned as if they were farm crops. Their cold values must preserve the ecology of the biomes where they naturally spawn. Plants present in tundra, boreal forest, cold bog, or multiple climate bands generally use dormancy or strong frost tolerance; explicitly tropical plants use warmer lethal thresholds.

| Plant | Minimum growth temperature | Fixed death threshold / behavior | Role |
|---|---:|---:|---|
| Agarilux | 0°C | cold dormancy | cave fungus; survives ordinary cold rather than disappearing from cold-region caves |
| Agave | 5°C | -7°C | arid/desert succulent with moderate frost tolerance |
| Alocasia | 10°C | -1°C | tropical foliage plant; frost-sensitive |
| Ambrosia bush | 0°C | -9°C | rare wild harvest plant; retains strong generic wild cold survival |
| Astragalus | 0°C | cold dormancy | alpine/tundra perennial |
| Berry bush | 0°C | cold dormancy | occurs from tundra/boreal through temperate climates |
| Brambles | 0°C | cold dormancy | temperate/boreal woody perennial |
| Bryolux | 0°C | -12°C | cave moss/fungus with explicit lethal cold response |
| Bush | 0°C | cold dormancy | generic wild bush used across several climate bands |
| Chokevine | 0°C | cold dormancy | wetland vine present in cold bog as well as warmer swamps |
| Clivia | 8°C | -2°C | warm/tropical ornamental wild plant |
| Giant rafflesia | 12°C | 5°C | strongly tropical, highly cold-sensitive plant |
| Glowstool | 0°C | -8°C | cave fungus with explicit lethal cold response |
| Grass | 0°C | cold dormancy | ubiquitous grazing layer including tundra |
| Low shrubs | 8°C | -2°C | tropical low-shrub layer |
| Moss | 0°C | cold dormancy | tundra/boreal/cold-bog ground cover |
| Pincushion cactus | 5°C | -8°C | arid/desert cactus with meaningful frost tolerance |
| Tall grass | 0°C | cold dormancy | wild grass occurring in cold bog and warm wetlands |
| Wild healroot | 0°C | -9°C | same species-level cold model as cultivated healroot |

Core stump/remnant Defs are intentionally excluded. They are dead plant remnants rather than living vegetation and have no meaningful cold-tolerance gameplay model.

## 5. Compatibility principle

Unknown third-party plants are not automatically overwritten. The all-PlantDef rule applies within plant sets that CCTO explicitly supports; it is not a blanket patch over every third-party mod.

Explicit compatibility/balance data should be provided for supported plant sets such as Core and Medieval Overhaul. AMJC-owned plants are outside CCTO's balance-data scope and use owner-supplied data through the framework. Within a supported set, all living PlantDefs are expected to receive CCTO data. Unsupported third-party plant sets retain their originating behavior unless a compatibility patch is added.

## 6. Archived candidate ranges and future randomized mode

The range-based values used during balancing are intentionally preserved instead of being discarded.

### 6.1 Current release policy

The initial implementation uses **one fixed low-temperature death threshold per crop Def**. This is simpler to understand, easier to balance, and avoids unexpected differences between adjacent plants of the same crop.

### 6.2 Possible post-release alternative

If post-release player feedback strongly favors individual variation, the mod may later switch to or add an option for **per-plant deterministic random death thresholds within the archived crop range**.

In that model:

- each crop Def owns a configured minimum/maximum death-temperature range;
- each individual plant receives a stable threshold derived deterministically from its identity rather than rerolling over time;
- the information card shows the species range rather than the hidden per-plant exact threshold;
- the same saved range data can be reused without redesigning the crop balance from scratch.

The range tables below are therefore not deprecated data. They are both the historical balancing record and candidate configuration for a future randomized mode.

### 6.3 Archived Medieval Overhaul medicinal crop ranges

| Crop | Minimum growth temperature | Candidate death range / behavior |
|---|---:|---:|
| Healroot | 0°C | -10 to -8°C |
| Mindwort | 5°C | -4 to -2°C |
| Poppy | 3–5°C | -6 to -4°C |
| Fleawort | 3°C | -7 to -5°C |
| Fly agaric | 0°C | cold dormancy |

### 6.4 Archived Medieval Overhaul annual crop ranges

| Crop | Minimum growth temperature | Candidate death range / behavior |
|---|---:|---:|
| Onion | 5°C | -4 to -2°C |
| Lentil | 5°C | -5 to -3°C |
| Cabbage | 0°C | -7 to -5°C |
| Garlic | 0°C | cold dormancy |
| Mushroom | 5°C | -2 to 0°C |
| Wheat | 0°C | -7 to -5°C |
| Flax | 5°C | -6 to -4°C |
| Sugarcane | 10°C | -5 to -4°C |
| Carrot | 0°C | -5 to -3°C |
| Herb | 5°C | -4 to -2°C |
| Tomato | 10°C | -1 to 0°C |
| Pumpkin | 10°C | -1 to 0°C |

### 6.5 Archived Medieval Overhaul perennial crop ranges

| Crop | Minimum growth temperature | Candidate death range / behavior |
|---|---:|---:|
| Grape | 5°C | cold dormancy |
| Apple | 5°C | cold dormancy |
| Mulberry | 5°C | cold dormancy |
| Griffon berry | 5°C | cold dormancy |
| Lemon | 10°C | -5 to -3°C |

### 6.6 Archived Vanilla crop ranges

| Crop | Minimum growth temperature | Candidate death range / behavior |
|---|---:|---:|
| Rice | 10°C | -1 to 0°C |
| Potato | 5°C | -3 to -1°C |
| Corn | 8°C | -3 to -2°C |
| Strawberry | 5°C | -10 to -8°C |
| Haygrass | 0°C | -10 to -8°C |
| Cotton | 10°C | -2 to 0°C |
| Devilstrand | 8°C | -2 to 0°C |
| Healroot | 0°C | -10 to -8°C |
| Hops | 5°C | cold dormancy |
| Smokeleaf | 5°C | -5 to -3°C |
| Psychoid | 8°C | -2 to 0°C |
| Cocoa | 12°C | 0 to 2°C |

## 7. Integrated Vanilla/MO support and optional VPE addon (2026-10-13)

The confirmed boundary is **CCTO framework + Vanilla and Medieval Overhaul (MO) balance in the base Mod**, with a separately selectable VPE-only addon. CCTO retains the shared `ColdToleranceExtension` API and Harmony patches. The author explicitly cancelled MO separation after its proposal; this documentation update does **not** change the loaded balance.

### 7.1 CCTO for VPE core — 20 source-implemented plants, native testing waived

The separate [CCTO-for-VPE](https://github.com/sucRo-RimWorld/CCTO-for-VPE) repository now holds the **authoritative** VPE core **and related VPE plant-family addon** cold-compatibility specification and plant inventory: [Design](https://github.com/sucRo-RimWorld/CCTO-for-VPE/blob/main/Docs/Design.md), [PlantBalance.json](https://github.com/sucRo-RimWorld/CCTO-for-VPE/blob/main/Docs/PlantBalance.json) and [prior-art audit](https://github.com/sucRo-RimWorld/CCTO-for-VPE/blob/main/Docs/Research/PriorArt.md).

The original 14 ordinary VPE 1.6 PlantDefs were extended to **20 source-implemented core VPE 1.6 plants**, adding the six individually source-audited VPE custom classes through generated XML (no `thingClass` replacement). The addon **does have source-reviewed engineering numeric values and generated XML** (source implementation [PR #3](https://github.com/sucRo-RimWorld/CCTO-for-VPE/pull/3), merged `235663629fc77e689f2292292ff4bd327b8bb5fa`). They are gameplay model assumptions and have **not** passed a RimWorld loaded-game/native test.

Nine counterpart Vanilla/MO plants (Grass, TallGrass, Agave, Onion, Pumpkin, Tomato, Apple, Cabbage, and the Orange/Lemon citrus analogue) must use **exactly the same growth minimum, cold-death threshold and dormancy** as their base-CCTO counterparts. The separate addon [parity tests](https://github.com/sucRo-RimWorld/CCTO-for-VPE/blob/main/Tests/test_ccto_parity.py) now obtain **current main** CCTO Vanilla/MO XML via GitHub Actions sparse checkout and compare all three values for each of the original seven counterparts; integrated [PR #6](https://github.com/sucRo-RimWorld/CCTO-for-VPE/pull/6), main commit `26ca4bb1ed0ee4007a582ecd0161f8bbc1bdd7ab`, PR and main static CI PASS. When changing these base CCTO settings, ensure the separately maintained addon is reviewed, and request its existing `workflow_dispatch` test; CCTO's push alone does not automatically run the addon workflow. Temperature data authority **for the base's own Defs** remains in CCTO; the addon owns only VPE Defs.

**VPE family expansion:** The author assigned VPE add-on compatibility to **this same CCTO for VPE Mod**, rather than making one extra CCTO patch per optional provider. [Owner inventory](https://github.com/sucRo-RimWorld/CCTO-for-VPE/blob/main/Docs/AddonInventory.json) records More Plants, Mushrooms, Succulents and Flowers as **unsupported candidates**, with [issue #8](https://github.com/sucRo-RimWorld/CCTO-for-VPE/issues/8) tracking future conditional XML. Optional VPE child-pack dependencies must not become new hard dependencies in base CCTO or the VPE compatibility Mod. Current core VPE XML now covers 20 plants and nine CCTO-matched/citrus-analogue values ([CCTO for VPE PR #12](https://github.com/sucRo-RimWorld/CCTO-for-VPE/pull/12)); original 14 species' numbers remain unchanged. **No separate child-pack-specific values have been approved**. User reports, rather than mandatory author native testing, govern future post-distribution issues. No game runtime PASS or publication asserted.

CCTO itself supplies the existing `ColdToleranceExtension` framework without adding VPE-only Def mapping or balance values. VPE addon requires CCTO and VPE, and VPE 1.7 has not been validated.

### 7.2 MO stays in CCTO — confirmed decision

The author has cancelled MO separation. **Do not create `CCTO for MO`.** All 29 MO-specific living PlantDefs remain supported by CCTO's guarded `Patches/MedievalOverhaul_ColdTolerance.xml` in the base Mod, alongside 49 Vanilla living PlantDefs and their established framework behavior.

Keep existing temperature values, dormancy and cold-death semantics, optional MO detection, loading order, Info Card output, save compatibility and Workshop identity unchanged. Developing VPE support must not relocate or duplicate MO data. The earlier proposed MO migration/addon testing plan is superseded and not an active work item.

The startup cost of CCTO's conditional MO patch has not been measured. No performance benefit from a split is asserted.

**Status / gate:** MO support remains integrated and unchanged. VPE addon's separate runtime XML and initial engineering balance values are implemented; its seven shared plant settings match current CCTO XML and are covered by offline source-parity regression. **Native RimWorld/old-save validation and Workshop publication remain unverified** ([addon issue #4](https://github.com/sucRo-RimWorld/CCTO-for-VPE/issues/4)). VPE 1.7 compatibility remains independent of the inspected VPE 1.6 source.


## Opt-in cumulative cold injury implementation (2026-10-21)

The author accepted elapsed-exposure-based cold damage as the target model and
requires no unnecessary whole-map polling or per-tick plant update. CCTO
remains generic; it must not hard-code AMJ crop identities or move consumer
crop balances into CCTO.

A new `ColdToleranceExtension` model uses
`coldDamageStartTemperature` and `coldDamageDegreeHoursToDie` together.
When ambient temperature is below onset, accumulated degree-hours remove ordinary
Plant HP in proportion to maximum HP. There is no cold-specific recovery and
no separate hidden cold-injury gauge. At zero HP the plant dies. This is a
provisional game abstraction, not an empirically calibrated agronomic threshold.
Earlier 2026-10-21 hidden-injury/repair drafts in this document are historical.

The Info Card for a damage-model plant displays **損傷開始温度** /
**Cold damage starts**, instead of the legacy death-temperature stat. The
existing **休眠温度** stat is retained without renaming. Legacy
`coldDeathTemperature` and `coldDormancy` records retain their original
behavior. If both old death and valid new damage settings exist, new damage
mode takes precedence for that plant. Optional dormancy and damage can coexist.

The initial engine integrates damage from the existing
`Plant.CheckMakeLeafless` callback using the plant's actual ambient
temperature. Delta time is capped at 2,000 ticks so unobserved seasons are
not inferred from one instantaneous sample. Damage records are held only
for plants with positive damage in a map-owned dictionary and are saved
as plant references. Dead/despawned records are pruned. There is no
per-Tick map-wide scan or new weather loop.

**No stock crop conversion yet:** CCTO Vanilla/MO XML, CCTO-for-VPE and
AMJ consumer XML retain current values. Before replacing these values,
calibrate dose/onset/recovery for plant classes and evaluate extreme cold,
warm recovery, dormancy, sampled-temperature bias, save/load, and winter TPS
against the existing fixed model. Cold acclimation and snow insulation
remain separately scoped future decisions; neither is implemented here.


### Numerical rebalancing gate — 2026-10-21 (author priority correction)

**Recalibrate numbers before native tests or plant migration.** The old
`coldDeathTemperature` values cannot automatically be copied to a new
damage-onset field, and the proposed injury law must be assessed against
actual time-to-injury curves first. [ColdDamageCalibration](ColdDamageCalibration.md)
now records the 78-PlantDef inventory (48 finite death / 30
dormancy), a source-verified empirical measurement matrix, biological
endpoints and extrapolation limits. The previous 12 unverified numeric
profiles have been explicitly **withdrawn** as calibration values; none
of their thresholds, doses or recovery rates may enter production XML.

**Known design/source mismatch until recalibration finishes:** The draft
C# implementation in PR #24 currently uses **linear** °C·hour damage.
A **quadratic** (°C²·hour) injury curve had been suggested to mimic one
maize study's 50%-mortality exposure-duration ratio, but two points
cannot identify both the unknown onset and the exponent, and LT50 does
not equate to deterministic complete death. Neither curve is an
empirically accepted general-purpose cold mortality model yet.
The framework must be reviewed against independent biological datasets,
growth stages, acclimation, organ versus whole-plant outcomes, annual
calendar compression and sampling bias before choosing injury
equations, recovery rates or numeric values. Resolve these before
converting production XML, promoting PR #24 out of Draft or asserting
native gameplay/performance validation.


### Biological approximation target — author clarification (2026-10-21)

**Aim for defensible proximity to observed cold responses, not a single
physiological equation that perfectly describes every plant.** CCTO remains a
generic realism-oriented Mod; accurate *relative susceptibility, timescale
and overwinter survival* matter more than arbitrary precision or fitting
every reported laboratory observation exactly.

**Unify implementation, not biological coefficients:**

- Use one lightweight state/update path and a bounded set of response
  profiles **only where evidence warrants distinct response shapes**. Do not
  mandate the same onset, severity curve, acclimation response or recovery
  constant for all PlantDefs. Profile count and exact curves remain to be
  determined from calibration, not guessed now.
- Individual Def parameters override a suitable shared profile. Similar
  plants can share defaults **only after checking that their relevant
  observations do not materially contradict the grouping**. Distinguish
  nonfreezing chilling-sensitive plants, freeze-sensitive annuals, and
  cold-hardy/acclimating or dormant plants when their mechanisms differ.
  Nonlethal chilling/yield effects must never automatically be interpreted
  as whole-plant lethal damage.
- Calibrate against species- and stage-specific observations at **more
  than one temperature or exposure duration where available**. Preserve
  endpoint distinctions (plant survival vs leaf damage, LT50 vs universal
  death) and report uncertainty. No synthetic lethal hours can be passed
  off as measured values.
- For plants without direct survival data, use clearly labeled
  related-species or growth-habit proxies and conservative ecological
  bounds, rather than leave 78 plants indefinitely blocked or fabricate
  precision. Unsupported/fictional plants receive explicit game-ecology
  estimates. Retain meaningful crop decisions and ensure natural wild
  plant populations survive their established biomes.
- Cold acclimation is **selective**, not mandatory for all plants; avoid
  many new per-plant timers and per-tick scans. Phase in extra mechanics
  only when their absence demonstrably breaks a representative plant
  class (for example overwintering cereals), and retain the existing
  dormancy Info Card entry and the approved 損傷開始温度 label.

No specific response profile, species value or recovery speed is approved
by this policy; the linear draft and quadratic candidate remain
unvalidated. Reconcile empirical calibration with the equation and
parameters before migrating stock XML or accepting the Draft PR.


### Plant-by-plant empirical evidence register (2026-10-21)

The source collection work is recorded in
[ColdDamageEvidenceLedger](ColdDamageEvidenceLedger.md): **78** CCTO-owned
plant Def mappings, bibliography, measured tissue/stage outcome, relevance
and gaps. An evidence link is not itself a calibrated cold-death onset or
lethal dose; this register includes related-species proxies and nonlethal
organ damage observations. The first pass mapped **53** Defs to at least
one source/comparison and left **25** unmapped. The 12 abandoned synthetic
numerical proposals remain withdrawn. Retain current production XML and
legacy death/dormancy presentation until a documented plant-class model is
validated against source-derived outcome constraints.


### Stable per-plant cold-injury susceptibility (2026-10-21 author direction)

**Accepted goal:** plants of the same Def exposed to the same cold should
not be guaranteed to die on exactly the same long-tick. The model should
approximate **population-level whole-plant survival**, including the meaning
of whole-plant-mortality LT50 where that endpoint is actually measured.
No arbitrary variance percentage, exact distribution or numerical death
dose is approved until empirical calibration.

The low-cost first implementation candidate is a **plant-specific lethal
accumulated-injury multiplier**, while retaining the Def's calibrated common
damage-onset temperature and temperature-response function. Each spawned
plant receives a stable susceptibility rank, deterministically derived from
a persistent plant identifier and a fixed, versioned seed/hash rather than
drawing random numbers on every temperature check. This avoids a whole-map
per-tick scan and preserves outcomes through save/load without storing an
additional random state. Verify identifier stability across save/load,
copy/despawn/respawn, and seeded construction before selecting code. New
plantings can receive a different rank. Do not make weather-dependent
randomness masquerade as stable physiological susceptibility.

For example, for a calibrated reference cold dose **D50** in a specified
growth/acclimation state and treatment protocol, define each plant's
lethal dose as the inverse of a chosen positive-valued susceptibility
distribution evaluated at its stable rank. Calibrate the distribution
median to **D50** so that about half the population dies by this reference
dose. Use measured survival-at-severity curves (e.g. LT10/LT50/LT90 or
multiple mortality percentages) to estimate the **spread**. LT50 alone
determines a median point, **not** the width, and an LT50 from tissue
electrolyte leakage is **not** a whole-plant survival datum. With thin
evidence, keep dispersion conservative and label it a gameplay
approximation rather than assigning a pretend scientific standard
deviation. Age, cultivar, microclimate, cold acclimation and plant condition
may contribute to observed mortality spread; independent random genetic
variation is *not* a justified interpretation of the whole spread.

Do not add fixed per-plant mortality randomness to plants that are
**dormancy-only**, and do not introduce a new instant-death route. Native
legacy death-temperature users remain unchanged until explicitly migrated.
For HP-model plants the current draft uses `ColdDamageProgress.HitPointLoss`;
variation scales the whole-HP degree-hour dose per plant. The former hidden
`damage >= 1` model and `Advance` function no longer apply. Variation and
numerical balance remain experimental.

**Presentation:** the plant Info Card retains the **species-level**
`損傷開始温度` and existing `休眠温度`; do not display a false
individual-precise "death temperature". If a Workshop graph is made
after calibration, show a **population mortality/survival band** versus
temperature or exposure time with its stated acclimation/age assumptions,
alongside the species median, not one deterministic all-plants-dead curve.


### Calibrated-population evidence review, not a fitted model (2026-10-21)

[ColdDamageCalibration.md — empirical multi-point mortality curves](ColdDamageCalibration.md#empirical-multi-point-mortality-curves--targeted-review-2026-10-21)
compares whole-plant survivorship for ON03 onion, V03 *Arabidopsis*,
V04 tall fescue and V05 chickpea across multiple freezing treatment
temperatures. The records separate observed LT50, source-provided
LT10/LT90, all-survived/all-died finite-sample observations, and
reported survival percentages. They are not interchangeable
measurement types.

The results support an **individual-variation feature** but do
**not** establish one normally distributed, symmetric temperature
threshold or shared variance for all species. At least one
*Arabidopsis* line reports an asymmetric LT10/LT50/LT90 curve;
onion seedling age shifts cohort median independently of the
within-cohort spread; chickpea recovery fractions show a sharp
transition. Use a small selection of positively bounded
susceptibility profile families only after comparing their fit with
the actual study conditions. Until that analysis, no curve family
or variance percentage is accepted and CCTO remains on the
unchanged legacy production XML.

A practical first candidate is **a stable per-plant multiplier
on incremental injury**, with per-species median coefficients.
This preserves the existing injury clamp at 1.0 without per-Tick
randomness, but requires stable plant IDs and careful save/load
verification at the end of numeric implementation. It is
**design-only**, not currently active in code.


### Distribution-shape audit: symmetric baseline versus asymmetric exceptions (2026-10-21)

The research comparison in
[ColdDamageCalibration.md — Population-variation model shape audit](ColdDamageCalibration.md#population-variation-model-shape-audit-2026-10-21-exploratory-not-approved-parameters)
uses *whole-plant* LT10/LT50/LT90 and five-point survival percentages.

Observed *Arabidopsis* Ler under specified non-/briefly acclimated
conditions has LT10 around -1.5°C, LT50 around -7.5°C, LT90 around
-8.5°C: a **highly asymmetric** mortality curve. An ordinary
single-scale logistic or normal CDF cannot match all three quantiles.
By contrast, many genotypes show near-symmetric transitions.
In a separate five-point chickpea study, exploratory unweighted
two-parameter logistic and probit fits both miss a published 80%
survival observation at -10°C by approximately 19–20 percentage points.
These are **evidence about survival curve shape**, not a universal
recommended parameter set; chickpea treatment denominators and
sampling error are not sufficiently specified for statistical
uncertainty-based model choice.

**Current engineering recommendation (candidate, not final approval):**
prefer a minimal symmetric positive susceptibility distribution for
classes where reliable measurements show an adequately symmetric
response. Permit a bounded/asymmetric profile **only for a group
where repeatable direct observations require it**. Keep species-specific
spread, lethal cold doses, acclimation and response kinetics
unapproved until calibrated. No new XML or C# implementation was
activated by this research; the existing Draft accumulator still
lacks the individual-susceptibility feature.


### Winter-wheat time-dose evidence affects model-scope choice (2026-10-21)

Three direct winter-wheat studies are now cited in
[ColdDamageEvidenceLedger](ColdDamageEvidenceLedger.md) as W03–W05 and
analysed in [ColdDamageCalibration](ColdDamageCalibration.md#winter-wheat-duration--severity-studies-2026-10-21-new-direct-ccto-owned-analogue).

- A thresholded freezing-degree-day accumulation model, using **soil**
  minimum temperature below a reported study-specific -12°C reference,
  has empirical support for certain acclimated winter-wheat experiments
  (W05). It is **not** a verified general wheat damage-onset temperature
  or a coefficient for ambient-air temperature.
- Duration mortality sensitivity differs greatly between winter-wheat
  cultivars (W04). This can motivate per-Def/class parameters but does
  not set the stable *within-cohort* susceptibility distribution width.
- W03 reports non-monotonic whole-plant survival after longer continuous
  freezing exposure, which a fixed-susceptibility + purely nonnegative
  accumulated-damage model **cannot produce**. This is a documented
  evidence conflict, not authorization to implement continuous
  acclimation in every plant.

**Design gate:** keep implementation simple where it reproduces the
main study outcomes, and consider **winter cereal-specific
acclimation/state** only if needed for ordinary climate realism.
Do not silently treat study soil minima, outdoor air temperature
and RimWorld's current plant `AmbientTemperature` as identical.
Neither a global quadratic injury law nor a universal recovery
rate is approved by these experiments. New runtime code/XML remains
on hold until source-specific calibration reaches a defensible
level for a representative crop class.


### W04/W06 numerical reconstruction gate (2026-10-21)

The primary comparison is now reproducible in
[ColdDamageCalibration.md — W04 original regression](ColdDamageCalibration.md#w04-source-regression-reconstruction--exact-published-tables-245-2026-10-21)
and [W06 independent frost-dose cross-check](ColdDamageCalibration.md#w06-independent-multi-temperature-frost-dose-cross-check--tillers-only),
with machine-readable entries
[W04 whole-plant fit](Research/WinterWheatW04Regression.csv)
and [W06 tiller dose response](Research/WinterWheatW06TillerRegression.csv).

**W04:** four field-acclimated wheat cultivar cohorts, each frozen
over cyclic **soil** temperature ranges, fit source whole-plant
mortality `M(n)=1-(1-P)^(n-K)` at its own experiment cycle. The
source **LD10/20/50 are regression-derived**, not hold-out
observations. Stable per-plant threshold `N_i=K-ln(1-u_i)/h`
(`h=-ln(1-P)`) reproduces a fitted source cohort-mortality CDF
without per-tick random checks. This is a suitable *computational
equivalence example*, not a CCTO source-level tested feature.
K/P and their spread differ by cultivar even at identical
nominal soil minima; no common plant species tolerance width
has been approved.

**W06:** original 2024 source Table 4 relates **tiller**
mortality to thresholded accumulated **soil crown** cold:
linear-in-dose coefficients and tiller-death 50% exposures
are separately recorded for three cultivars. This supports
the *shape* of a low-cost accumulated cold metric over the
observed experimental range but **is not whole-plant
mortality calibration**. Crown soil temperatures deviated
from imposed air temperature by several degrees and the
gap varied with the number of freezing days. It cannot
be pasted into an ambient-air-based RimWorld Plant model.
One Yanzhan4110 2-day logistic table coefficient is
inconsistent with its own published LD50 and has been
quarantined in research.

**Implementation gate:** prioritize a compact dose-integration
path + stable plant-specific susceptibility derived from
approved source-specific exposure curves. Avoid a global
linear/quadratic law, unsupported temperature offsets or
universal recovery/variance. Do not migrate MO Wheat
or any other Def just because one winter cultivar's data
can be fitted. Raw whole-plant survival across multiple
temperature-duration combinations, including post-thaw
recovery details, remains the key missing evidence for
numeric acceptance. PR Draft implementation is still
uncalibrated; no production XML changed.


### Fixed-susceptibility feasibility and dose-calibration status (2026-10-21)

The evidence-driven time-response feasibility study now includes
[W07-via-W08 Kharkov cohort data and exploratory fit](ColdDamageCalibration.md#controlled-constant-temperature-survival-duration-kharkov-wheat-w07-via-w08)
plus its [machine-readable observations and comparison](Research/WinterWheatW07DurationSurvival.csv).

**Specific finding:** In the quoted -16°C Kharkov
winter-wheat series (1,3,6,12,24h; observed survivor
fractions 80/63/53/33/17%), a one-parameter constant
survival hazard gives 7.77 percentage-point in-sample
RMSE; a two-parameter Weibull survival-time curve
gives 1.59pp. The **original 1975 study
survival table has not been read directly**: a 2025
open-access scholarly review (W08) repeats the figures.
The same review quotes no Kharkov deaths after
5 days at -6°C, but only a single temperature/time
data cell for that condition. Hence Weibull-versus-
exponential fit does **not** identify a cold-injury
rate across temperatures or endorse a universal
positive-dose distribution.

**Runtime implication:** one stable susceptibility
draw/rank per plant (from a validated persistent ID)
can be transformed into a candidate survival-time
distribution for a given source regimen without
periodic RNG draws; **an initial uniform random rank
with an inverse-CDF threshold is a deterministic
representation of a population survival curve**
once the rank is fixed. More flexible survival
time curves are an **opt-in calibrated exception**,
not a mandatory replacement of the simplest default.
The Weibull shape <1 is a *population effect* and
does not by itself imply an individual plant recovers
while frozen.

**W05 missing-data boundary:** original whole-plant
survival **per temperature×duration treatment**
has not been accessible, despite a source-supported
two-factor 2018 winter-wheat study design. Aggregate
mortality-versus-soil-degree-days alone does not
authorize a generic game ambient-air cold-damage
equation. Therefore the first activation of the
new damage engine remains gated on **a defensible
temperature/time response model in at least one
owning plant class**, separate from individual
survival dispersion. No numeric plant XML,
new code, save schema, native acceptance or
published Workshop text was changed.


### 2024 whole-plant LD50/LT50 study extends calibration constraints

[Husiny et al. (2024) W10 original study and accessible
individual-data DOI](ColdDamageCalibration.md#w10-full-plant-cold-freezing-factorial-design-paired-ld50--lt50-2026-10-21)
adds one methodological advance. Intact cold-acclimated wheat
(Zadoks 21) was scored dead or alive by **3-week post-thaw
regrowth**, with two **separate** tests:

1. **Cold duration at -4°C**: original fitted population
   LD50 Norstar 33 days (extrapolates beyond last
   observed 32d), CM614 20d, Branson 18d.
   Spring control AC Carberry had complete observed
   mortality by day 4 (LD50 not estimable).
2. **Acute LT50 after 0 versus 7d at -4°C**:
   winter cultivars' fitted LT50 shifted warmer by
   only 0.1–0.4°C; spring wheat control shifted
   warmer by 3.9°C. Cooling used sequential
   one-hour holds at temperature steps, NOT a
   constant seven-day minimum.

[Machine-readable author-table records](Research/WinterWheatW10HusinyWholePlantLT50.csv)
preserve all exact reported paired LT50 medians and
observed/extrapolated LD50 classifications.
The paper explicitly links a raw individual-level
dataset [DOI 10.5683/SP3/YHOZUK](https://doi.org/10.5683/SP3/YHOZUK);
the archive currently returns HTTP 403 to our web
retrieval, so raw mortality counts and joint
temperature×duration response cells have NOT been
obtained or numerically fitted.

**Design implication:** the same species may show
very different duration tolerance and different
pre-freeze *state* dependence. Fixed individual
susceptibility remains a promising lightweight
abstraction within each physiological state, but
must not substitute for stage/acclimation in the
species median. A full general CCTO fatal-dose
equation still has no empirically validated
temperature×time surface for whole plants. No new
numeric XML or C# has been approved.


### No extra acclimation state for the first calibrated slice (2026-10-21 evaluation)

The complete original Table 2 comparison from
[Husiny et al. 2024, W10](ColdDamageCalibration.md#w10-raw-data-retrieval-attempt-and-significance-aware-state-model-decision-2026-10-21)
includes **Tukey HSD significance groups**. Following four
weeks' shared acclimation, the winter cultivars' day-0 vs
day-7 changes in LT50 (-13.6→-13.2°C Norstar;
-12.7→-12.6°C CM614; -11.9→-11.6°C Branson)
are **not significant at alpha 0.05**. The
spring cultivar AC Carberry has a significant
-6.6→-2.7°C shift, but it had 100% mortality
by freeze day 4 in the **different** duration
arm and its physiological history cannot be
transferred between those cohorts.

**Minimal-scope candidate:** the first numerically
calibrated plant response should use the existing
elapsed-dose accumulator and stable individual
susceptibility **without new per-plant acclimation
timers**. Do not derive winter-wheat dynamic hardiness
loss rates from those nonsignificant differences.
A shared map-wide ambient-temperature proxy is
also excluded as an evidence-free substitute for
root/crown and indoor microclimate conditions.
A per-plant acclimation state remains an **opt-in
future mechanism**, conditional on repeatable
physiology data and a demonstrated game-world
survival need. This avoids storing state for every
overwintering plant before it suffers cold damage.

**Limit:** the W10 day-0 plants were themselves
cold-acclimated; this does not make fully acquired
hardiness realistic for newly planted warm-grown
crops. Before activating a winter-hardiness
profile, specify which physiological state the
game cohort represents and compare ordinary
winter survival in representative biomes. Fixed
individual variability alone cannot create
seasonal acclimation. There is **no authorized
new XML value or changed runtime behavior** here.

The Borealis DOI
[10.5683/SP3/YHOZUK](https://doi.org/10.5683/SP3/YHOZUK)
continues to link the primary individual responses,
but public portal/API access was not obtainable in
this execution context. The reference source remains
in GitHub; no individual responses have been invented
or identified from the medians.


### Species-class cold response must include chilling where supported (2026-10-21)

The [new empirical maize audit](ColdDamageCalibration.md#first-identifiable-crop-class-response-audit-maize-2026-10-21)
now distinguishes M01 near-frost seedling 50%-death
time/temperature pairs, M03 **nominal +0.3°C near-zero
chilling that can end in irreversible whole-seedling death
after prolonged exposure**, and M04 30 inbred-line cohort
survival differences at the same **−1°C for 3 hours**.
These are different experiments and genotypes; they
must **not** be fit as if they were one coherent
three-temperature treatment grid.

**Scientific requirement:** do not hardcode all damage onset
to <0°C or force leaf/crown freezing to be synonymous with
plant death. Especially for susceptible juvenile crops,
long enough *above-freezing chilling* may be injurious,
and shorter durations may be reversible. Existing UI
label `損傷開始温度` still communicates a species-class
model onset, not a precise lethal point. Chilling
and stronger freezing can be distinct **response
regimes within the same inexpensive accumulated-dose
engine** if the selected plant class has evidence.
This does not approve a new global two-profile option,
warm/cold rate, recovery number, or another map scan.

**Mathematical identifiability gate:** M01's approximate
-2°C×48h and -6°C×2h 50%-death points are exactly
compatible with multiple different power-law
temperature functions. Their extrapolations to -4°C
differ materially (linear 3.84h vs quadratic 5.52h
under separately refitted conditional onset and median
dose). Neither model is uniquely validated, and
joining M03's +0.3°C result from another cohort does
not resolve that ambiguity. The [CSV with several
conditional functions](Research/MaizeM01DegreeHourIdentifiability.csv)
is **an algebraic sensitivity analysis, not new
observed death data**. Do not assign its fitted
onsets/lethal doses to production Corn XML.

**Historical V2 engineering constraint, superseded by HP draft:** its
`ColdDamageProgress.Advance` historically used linear degree-hours and
clamped hidden injury at 1.0. This is no longer the active draft code:
`ColdDamageProgress.HitPointLoss` now removes ordinary HP and the map
component saves only observation time and fractional HP remainder. This is
a minimal opt-in proof of concept, **not** a
validated biological injury profile or active
individual-variation implementation. Any future
shape/threshold choice must first reconcile
the clamp, plant identity stability, sparse
update history (at most 2000 ticks sampled)
and recovery semantics. Do not add per-plant
acclimation timers based on a nonsignificant
W10 winter-wheat state change.

**External evidence check:** FROSTOL W11 models
winter-wheat LT50 across a season from crown
soil temperature, snow, vernalization, hardening
and dehardening; its W12 geographic
out-of-sample comparison classifies winter
survival with 75% agreement in Sweden and
37% in Norway. This is evidence that
environmental state matters and a mature
model can still fail outside its calibration
conditions, **not** support for importing
FROSTOL coefficients or runtime dependencies
into the CCTO light-weight engine.

**Next user-visible acceptance:** release a
selected plant-class profile only after a
defensible *stage-labelled* time–temperature
mortality calibration and explanation of
experimental days versus compressed
RimWorld game days. Then activate opt-in
values for that class and run one consolidated
gameplay test. Fictional/general wild plants
may use explicitly marked ecological analogues
and biome-survival boundaries; no invented
claim of laboratory precision. Dormancy-only
plants and legacy death temperatures remain
unchanged until expressly migrated.


### New two-factor seedling-survival evidence and owned-lentil boundary (2026-10-21)

The [C03 2023 open original paper](ColdDamageCalibration.md#first-primary-5--5-temperature-and-duration-whole-plant-study--chickpea-2026-10-21)
establishes a **genuine 5 temperatures × 5 target-hold durations
factorial study of whole-plant survival** after a four-week
regrowth endpoint. It uses **six-leaf chickpea seedlings**
from a seed collection of incompletely specified
genetic identity, **not CCTO's MO Lentils**.
At −12°C target temperature, the source quotes
survival fractions **75/78/25/8/0% at 1/2/3/4/5h**.
The source's five time-specific fitted LT50 values
are **−12.1/−12.1/−10.5/−11.0/−9.0°C**.
Some neighboring points are nonmonotonic
(sample variation/source fitting). Target
holding times exclude cooling ramps that
vary by temperature.

In-sample source-percentage fitting demonstrates
that neither a constant-rate exponential nor
a two-parameter Weibull fully reproduces
the reported −12°C time trajectory
(**17.2 vs 10.1 percentage-point RMSE**;
Weibull itself is 18.8pp high at 1h).
This is a **model-check example**,
not authorization to add a global Weibull
random draw or per-species coefficients.
[Reproducible C03 observations/fit CSV](Research/LegumeC03ChickpeaFreezingSurvival.csv)
clearly separates published percentages and
reported LT50 from derived model predictions.

For **CCTO-owned MO Lentils**, *Lens culinaris*,
the [evidence ledger](ColdDamageEvidenceLedger.md)
now cites L01 (40 genotypes, −13/−15/−18°C)
and L02 (18 genotypes, 0/−15/−18/−20°C
at 1h) as **direct same-species whole-plant
survival** sources. L02 records >80%
survival for several *named* genotypes
at −15°C and no survivors among sampled
genotypes at −20°C; it does **not**
supply the multiple exposure durations
needed to fit an independently validated
lentil lethal-dose curve. L01's fractions
of genotypes meeting *non-significance*
versus their maximum survival must
not be mistaken for proportions
of individual seedlings living.

**Minimum-scope decision for the first
numerical activation:** keep
calibration restricted to stage- and
species-labelled cohort evidence.
Do not force the same damage law
from chickpea onto lentil, or
from long-hardened wheat onto
maize. Do not start 78 species XML
migration while even the first
candidate class's damage onset,
fatal time-temperature dose and
intra-cohort susceptibility
spread remain unquantified. The
current C# code is still an
**opt-in uncalibrated framework
prototype** and has not been
modified by this research pass.


### Lentil source-scope and monotonicity check (2026-10-21)

[ColdDamageCalibration.md](ColdDamageCalibration.md) now compares
the published C03 chickpea 1–5h survival fractions to an unweighted
nonincreasing isotonic fit. The observed 75/78/25/8/0% at -12°C
becomes a modelled 76.5/76.5/25/8/0% (RMSE 0.95 percentage
points). This is a descriptive within-sample fit, not a fitted
temperature-dose equation. Minor mortality inversions do not
justify an additional per-plant physiological-state mechanism.

The properly identified data file is
[LegumeC03ChickpeaFreezingSurvival.csv](Research/LegumeC03ChickpeaFreezingSurvival.csv).
The previous file incorrectly used ID C01, which belongs to a
cabbage study; the wrong file was removed and Git history retained.

L03 and L04 add direct lentil (*Lens culinaris*) whole-plant
recovery experiments, but not a complete same-genotype
temperature-by-duration survival matrix. L05 measures
seed germination rather than survival of growing plants and is
excluded from lethal-dose calibration. Do not migrate
winter-acclimated lentil experimental tolerances to
the unclassified MO Lentil Def without a stated physiological
state and time-scale conversion. Production code/XML remains unchanged.


### C03 full 25-condition plot limits the monotone-dose claim (2026-10-21 correction)

The earlier statement that C03's nonmonotonic survival can be
represented with ~0.95 percentage-point RMSE is restricted to
the five **verbatim** source outcomes at -12°C. Inspection of
[original Figure 2A](https://www.frontiersin.org/files/Articles/1239008/xml-images/fpls-14-1239008-g002.webp)
shows a much larger **apparent** inversion at -10°C:
3h survival ~60%, 4h survival ~93%. A [clearly
labelled approximate graph transcription and monotone
projection](Research/ChickpeaC03Figure2Approx25.csv)
places the 25-condition **minimum monotone** fitting
RMSE at ~5.29 percentage points. Other 20 points
were visually approximated, not source-published raw
counts; the paper's error bars do not establish
which inversion is significant. Refer to the
[calibration correction](ColdDamageCalibration.md#correction--audit-the-complete-c03-figure-not-only-the--12c-text-row-2026-10-21)
for provenance and meaning.

**Resulting design boundary:** a nonnegative dose plus
persistent per-plant susceptibility remains an affordable
biological abstraction, **not quantitatively verified
against the entire C03 temperature×duration study**.
Do not add dynamic acclimation, new per-tick sampling,
or asymmetrical probability families merely to
interpolate an uncertain small survival experiment.
Nor may C03 chickpea parameters be inserted into
the unrelated MO Lentil Def. The first live
plant-class migration still requires source-matched
temperature/time evidence and a stated game
time-scale. C# / plant XML remain unchanged.


### Individual lethal-dose variation: implementation staged, values uncalibrated (2026-10-21)

The Draft C# opt-in injury engine now includes a **zero-default**
`ColdToleranceExtension.coldDamageLethalDoseVariation` fractional
half-width `w`, validated as `0 <= w < 1`. Each plant's persistent
integer Thing ID deterministically maps to rank `u` and lethal-dose
multiplier `1 + w*(2*u-1)`. At `w=0`, the multiplier is exactly
1.0. With `w>0`, the current bounded uniform *simulation*
distribution approximates a cohort median at 1.0, but it is
**NOT an empirically fitted species population distribution**:
no LT10/LT90 or variance number is approved. This candidate
choice is reversible before any production Def opts in and must not
be reported as scientifically validated.

Implementation changes are confined to the Draft experimental
opt-in path: extension validation, stable hash function,
and using the per-plant multiplier on lethal degree-hours
**before the existing damage-clamp [0,1]**. No CCTO
Vanilla/MO, consumer, or dormancy-only plant XML
has been migrated. No map-wide plant scan, per-tick
random sampling, per-plant acclimation history, or
new save fields are introduced. Existing positive
damage records store normalized injury and lastTick;
XML coefficient changes partway through a save alter
their physical interpretation. Save/load persistence
of `Thing.thingIDNumber`, destruction/re-spawning,
and XML edits across saves remain runtime acceptance
items, not claims of completed verification.

This implementation supplies the previously approved
**engineering mechanism for stable individual differences**,
not the missing species-class biological coefficients.
Reference research remains in
[ColdDamageCalibration.md](ColdDamageCalibration.md)
and [ColdDamageEvidenceLedger.md](ColdDamageEvidenceLedger.md).
Framework consumers see only documented opt-in
`Docs/Framework.md` semantics; existing default
behavior and published Workshop copy stay unchanged.
The Draft prototype's linear degree-hour injury
rate and warm recovery coefficient remain
*uncalibrated*. Test source for deterministic
ranking and input bounds was added; tests **have
not** been executed in native RimWorld.


### First-cold observation must not charge unknown history; crop calibration gate (2026-10-21)

**Implementation correction (Draft C#):** the earliest version of
`ColdDamageMapComponent.UpdatePlant` created a cold-damage record with
`lastTick = now - 2000` on **the first sampled cold temperature**,
and immediately charged the prior 2000 ticks at that temperature.
This **fabricates up to 0.8 in-game hours of past freezing** and
can kill a plant immediately on observing an abrupt cold snap:
exactly the player-visible failure this cumulative system is meant
to avoid. Now the first cold sample **only starts** a
`damage = 0, lastTick = now` observation; the next sample charges
bounded elapsed time. The zero-damage record is **retained at
PostLoadInit** so saving/reloading mid-freeze does not repeatedly
reset the observation. Warm samples remove zero records, and
legacy death/dormancy plants remain unaffected. Existing positive
damage records keep their fields and normalized meaning.

This is a conservative sampling approach: a short freeze entirely
between scheduled plant callbacks may not be detected, and there
is no false claim of continuous sensor data or catch-up through
the unknown history. Existing 2000-tick integration cap stays.

**Direct rice evidence R02 (not a production coefficient):**
[Sagehashi (2023), DOI 10.1080/1343943X.2023.2236344](https://doi.org/10.1080/1343943X.2023.2236344)
studied living **10-day-old Hokkaido rice seedlings** at
`4°C for 10 days` followed by 10d greenhouse recovery.
All six tested rice cultivars had **0%** live seedlings
when transferred directly to 4°C, yet **88–100%**
survived if first held at **12°C for five days**.
Survival loss duration extended by **8–10 days**
for Oboroduki and Kitaaoba after acclimation.
**Fixed rank and fixed dose alone cannot reproduce
both physiological states**, even with perfect
within-cohort susceptibility fitting.
The source provides **neither** a generic onset
temperature `coldDamageStartTemperature`
**nor** genotype-independent lethal degree-hours
or susceptibility width. Do not convert “4°C
× 10 days” into a blanket CCTO rice fatal-dose
parameter, or assume every RimWorld rice seedling
was already acclimated.

**Activation shortlist (status, not a proposed XML migration):**

| CCTO plant group | Supported experiment | Numerically identifiable now | Source-specific blocker |
|---|---|---|---|
| Corn (`Plant_Corn`) | M01 approximate two freeze-temperature LT50 durations; M03 separate +0.3°C prolonged-chilling deaths | Two conditional freeze-law parameters **only if exponent chosen independently**; no unique class law | Growth stage/seed cohort not matched, chilling/icing mechanisms differ, neither within-cohort width nor recovery established |
| Rice (`Plant_Rice`) | R02 4°C×10d same-species whole-seedling mortality with/without 12°C×5d acclimation | **0% vs 88–100%** survival for specific state treatments, not a continuous dose surface | Acclimation/state is a dominant missing variable; no meaningful single default fatal dose |
| MO Lentil (`DankPyon_Plant_Lentils`) | L01–L04 genotype-specific cold-acclimated whole-seedling survival, C03 *other-species* 5×5 cross-check | Qualitative survival bounds for named genotypes | Generic MO variety identity and acclimation, full same-genotype time×temperature survival and spread missing |
| MO Wheat (`DankPyon_Plant_Wheat`) | W04 complete whole-plant mortality regression for specific field-acclimated winter cultivars, W10 separate LD50/LT50 arms | Four **regimen-specific** survival-time curves (W04), but **not** a common air-temperature severity law | Soil min/max cycles, overwinter physiological state and 60-day RimWorld calendar mapping |

**First playable class migration is therefore
NOT yet source-calibrated**. The engineering feature can be
built and checked without silently assigning untested numbers
to all Vanilla/MO plants. To unlock one species, pick one
measured physiological state and compare it with
the intended colonist planting/winter weather:
then determine a class severity curve, dose
median and **within-genotype** mortality spread
under that specific thermal signal, plus
the explicit mapping from laboratory hours/days
to game hours. The existing old threshold
will remain active for all production plants until
that requirement is met, rather than falsely
advertising laboratory accuracy.

**Testing stage:** no native gameplay performed
by this correction. Required eventual consolidated
acceptance includes first-cold sampling, continuous
frost across save/load, warmth clearing zero records,
identity-stable individual fatal doses, and legacy
dormancy/death regression; do not start an
additional unrelated CI/test matrix.


### Three-species numeric model feasibility gate (2026-10-21)

The [bounded quantitative comparison](ColdDamageCalibration.md#quantitative-feasibility-gate--wheat-maize-rice-2026-10-21)
evaluates the **existing experimental Draft API**, not newly selected
plant values:

- **Winter wheat** (W07 via W08, secondary-reported -16°C durations):
  a fixed-state **linear** degree-hour engine with one *uniform* stable
  individual lethal-dose spread cannot fit the 1h ~80% and 24h ~17%
  survival observations simultaneously at any valid `0<=w<1`.
  The best five-point boundary approximation has ~**13.86pp RMSE**,
  compared with an in-sample Weibull ~1.59pp. This challenges the
  uniform threshold **distribution**, not necessarily the reusable
  nonnegative accumulation mechanism. It does not approve Weibull
  for all plants.
- **Maize** (M01) has two approximate 50%-mortality
  temperature/duration pairs which permit multiple severity
  exponents with different unmeasured predictions. A conditional
  linear fit gives ~**-1.826°C onset**, **not** a measured biological
  onset or an XML candidate. M03 separately documents severe
  **above-zero prolonged chilling**, from different plant/stage
  conditions; do not merge those studies into one cohort.
- **Rice** (R02) has **0% versus 88-100%** survival after a matching
  4°C × 10d exposure with/without 12°C × 5d acclimation. Stable
  plant IDs alone cannot represent the physiological-state change.

**Design disposition:** retain sparse opt-in infrastructure and stable
per-plant identity; keep susceptibility distribution,
temperature-severity law and acclimation as **separate design axes**.
Do not turn six evidence-class categories into six mandatory runtime
formulas, or activate any Vanilla/MO/consumer plant until a
source-matched and game-timescale-labelled profile is justified.
Preserve all existing production thresholds and dormancy for now.
No code/XML update, build, native test or publication was performed
during this mathematical audit; PR #24 remains Draft.


### Follow-up decision: distribution is not severity (2026-10-21)

[Quantitative research source](ColdDamageCalibration.md#distribution-versus-temperature-severity-model-identifiability-audit-2026-10-21)
and the existing [W07 comparison CSV](Research/WinterWheatW07DurationSurvival.csv)
separate **(a)** temperature severity, **(b)** per-plant
lethal-exposure distribution, **(c)** prior acclimation/state,
and **(d)** a genuinely nonadditive time-course model.
The six previously proposed ecological/evidence groups are not
six mandatory runtime formulas.

**Decision for this Draft:**

- Preserve the current **additive exposure and sparse state**, stable
  plant-ID percentile and legacy dormancy/death compatibility.
  Do not add a temperature exponent just to improve a single
  constant-temperature wheat survival series.
- At -16°C the W07/W08 five duration points challenge the **uniform
  lethal-dose spread** (best limiting fit **13.86pp RMSE**).
  Keeping additive exposure and changing the *threshold distribution*
  improves in-sample fits (log-logistic **3.00pp**, Weibull **1.59pp**),
  without requiring an expensive per-plant exposure timer.
  A nonlinear **duration** transform with extra state could also fit
  (**1.45pp**, three coefficients); this alone does not justify it.
- These results **do not approve Weibull** or any plant-specific
  distribution, including the existing uniform `w`; unknown
  sampling uncertainty, long-survivor tail, breeding genotype and
  physiological-state limits prevent direct XML calibration.
  The present uniform implementation remains an opt-in prototype
  with no migrated production Def.
- Maize M01 supports temperature-versus-duration tradeoffs but
  cannot identify a unique onset and intensity exponent from
  only two approximate median-mortality points. Rice R02 requires
  acclimation differentiation (or an explicitly limited single-state
  approximation), **not merely another fixed individual CDF**.
- New CDF families or acclimation state **must not be implemented
  yet** solely from these in-sample comparisons. The smallest
  defensible future implementation keeps one reusable accumulator
  and selects a class-/species-scoped stable lethal-dose mapping
  **only after** matching whole-plant study conditions and
  game-time conversion.

No plant numeric settings, C# features, runtime checks or Steam
publication were authorized by this quantitative audit.
The coldDamageStartTemperature Info Card label remains
**損傷開始温度** and the existing dormancy label
**休眠温度** remains unchanged.


### Distribution transfer and tail audit (2026-10-21)

[W07/W04 comparison and extreme-rank calculations](ColdDamageCalibration.md#w07-weibull-transfer-and-tail-risk-gate-2026-10-21)
add a concrete **STOP** to the previous "prioritize a better
individual distribution" candidate:

- W07's -16°C 5-point fitted Weibull **shape 0.6566917**
  is **not** a plant-class parameter. W04 uses different
  cultivars, field pre-hardening and soil-temperature cycles,
  and an author-fitted **delayed exponential survival curve**;
  neither W07 shape nor its temperature-dose units can be
  transferred unchanged to W04, let alone generic MO Wheat.
  Rescaling that W07 shape against *W04 regression-derived
  predictions* yields discrepancies, **not** independently
  validated prediction errors.
- The W07 Weibull's inverse-CDF has a long unbounded tail:
  model median **6.01h**, 99th-percentile lethal duration
  **107.44h**, 99.9th **199.22h** at its one -16°C
  condition (time values **not mapped to game days**).
  With the present **24-bit** plant-ID rank, rare extreme
  extrapolations exist. No empirical tail cutoff or
  truncation percentile has been approved.
- Hence neither the current uniform distribution nor
  Weibull/log-logistic becomes the final biological
  individual-variation law solely by low five-point RMSE.
  Keep **accumulated exposure**, **individual lethal-dose
  mapping**, **temperature severity**, and **acclimation**
  as distinct responsibilities. Reuse the same sparse
  opt-in engine and deterministic rank while the correct
  class/state-calibrated mapping remains **not selected**.
- **No new CDF implementation, runtime timer, random
  death rolls, numerical plant XML, gameplay test or
  release** is authorized by this cross-study audit.
  The existing Draft opt-in C# is an *uncalibrated*
  prototype, and PR #24 stays Draft.

**Next implementable scientific gate, not another
unbounded bibliography sweep:** confirm one suitable
same-cultivar, same-stage full-plant mortality
temperature×duration protocol (with original counts
and treatment-temperature histories), or explicitly
accept a bounded **ecological gameplay approximation**
instead of claiming source-calibrated parameters.
Do not choose an arbitrary Weibull tail cap merely
to make a test or demonstration pass.


### First candidate decision: matched wheat factorial evidence (2026-10-21)

**CCTO owner-level outcome:** do not activate a first
opt-in crop simply because an individual Weibull/linear
curve fits one winter wheat experiment. [Current primary
experiment acquisition gate and complete W13 24h cohort
table](ColdDamageCalibration.md#w13-measured-24-hour-wheat-frost-bounds-and-2d-activation-gate-2026-10-21)
were reviewed against the owner's Vanilla/MO plant data.

- **First complete 2D calibration target:**
  `DankPyon_Plant_Wheat`, biological model **Jing411
  winter wheat / W05**. W05 directly studied one cultivar at
  soil minima −14/−17/−20°C over multiple freeze
  durations, but its *per-treatment whole-plant
  mortality cells and soil/crown temperature history*
  are **not currently verified in accessible data**.
  The published aggregate slope is NOT enough to infer
  an onset, distribution or fatal accumulated dose.
- **New matched-temperature validation axis:**
  primary W13 Deryabin et al. 2024 directly reports
  wheat seedling survival at 0/−3/−5/−7°C for
  24h with and without 4°C×7d acclimation.
  Same-genotype observed 24h LT50 brackets include
  M39 conditioned (−7,−5)°C versus untreated
  (−3,0)°C; this **is not** a 24h biological
  onset value or matched duration curve.
  W13 has exactly **one** freeze duration and uses
  different cultivars/stage from W05; cannot jointly
  fit W05+W13 as a single experiment.
- The W07/W08 constant-temperature time curve and
  W10 separated LD50/LT50 experimental arms are
  **independent non-mergeable constraints**.
  C03 chickpea's full-factorial study does not
  calibrate MO lentil because it is **another species**
  and the 25-cell original individual outcomes
  are unavailable.
- **No first source-validated plant injury XML
  can be enabled at this stage.** Missing W05
  factorial whole-plant observations, physical
  plant-temperature versus in-game ambient mapping,
  physiological state and lab-to-60-day-game-year
  exposure scaling are **relevant blockers**.
  Preserve the 48 current fixed-death profiles,
  30 dormant profiles, owner boundaries, current
  sparse opt-in prototype and the already accepted
  Japanese Info Card terminology.
- The bounded **W05 original figure/table acquisition** was attempted. The original publisher-indexed study gives the exact **20 freezing treatment combinations** (5 at −14°C, 8 at −17°C, 7 at −20°C), now indexed in [W05 treatment availability](Research/WinterWheatW05TreatmentAvailability.csv), but **0 of 20 observed whole-plant mortality values were retrievable**. This is source-access status, not a mortality percentage. [Detailed accession gate](ColdDamageCalibration.md#w05-exact-treatment-grid-now-indexed-observed-outcomes-still-unavailable-2026-10-21). Do not re-create the results from the paper's single aggregated FDD slope or imply these daily soil minima are continuous ambient readings. Seek the original outcome figure/table and denominators only if source-backed activation is still the chosen release scope.
  Do not invent individual trial counts or use
  source-derived 24h LT50 bounds as lethal game-hour
  settings. No code, production XML, native test,
  main merge or publication follows from this
  documentary decision; PR #24 remains Draft.


### Evidence-constrained approximation policy (2026-10-21)

**Development decision:** continue using **explicitly labelled,
empirically constrained gameplay approximations**, rather than
suspending all engineering until inaccessible W05 mortality
cells are retrieved. This **does not** approve a particular
plant-specific onset, lethal accumulated dose, recovery rate,
individual spread, Weibull law, time conversion, or new
production XML. Preserve the opportunity for full source
calibration if the original observations become available.

**Separate the deliverables:**

1. **Generic framework:** keep the existing sparse,
   opt-in additive cold-injury prototype and stable plant-ID
   rank; finish technical correctness and consolidated native
   acceptance when a coherent slice is ready. Do not
   require plant-specific balance values merely to verify
   the optional API. Framework correctness is not biological
   calibration or a crop-release claim.
2. **First playable plant:** require a *single*
   well-specified cultivar/physiological-state approximation,
   its source evidence and runtime mapping. Only after
   the explicit candidate's observable consequences,
   limitations and compatibility are reviewed may the
   owning plant Def be switched from the legacy model.
   The 48 fixed-lethal and 30 dormancy production profiles
   retain their current settings until individually justified.

**Minimum source-to-game contract before crop opt-in:**

| Axis | Required acceptance information |
|---|---|
| Plant and state | Owning Def, biological proxy/cultivar, growth stage, winter acclimation and how game code knows that state |
| Experimental endpoint | Whole-plant live/dead or recovery after thaw; sample counts, evidence source, temperature and duration; do not use tiller or leaf loss as whole-plant death |
| Provenance | Distinguish exact observed values, interpolation between observations, extrapolation outside tests and **gameplay-assumed** parameters |
| Exposure | Plant/crown/soil versus sampled map-air temperature, cooling ramp, 24h freeze versus multi-day frost and mapping experimental hours to RimWorld hours |
| Model | Nonnegative severity, dose distribution, upper-tail behavior, irreversible/reversible injury and which constants cannot be inferred |
| Player outcome | Short snaps versus prolonged freeze, warmer breaks, repeat freezes, season transitions, existing dormancy and save compatibility |
| Activation | Predicted survival at measured conditions contrasted with source data, bounded extrapolation and smallest appropriate final runtime check |

**Prohibited transfers:** W13's 24h temperature and
acclimation table cannot determine its duration curve;
W07's single-temperature Kharkov duration curve cannot
be mixed with W13 or W05 as the same genotype; W05's
20 treatment combinations with **0 recovered mortality
values** are not a 20-point survival calibration;
M01's two approximate maize mortality medians
do not identify onset *and* severity exponent together.
Neither a fitted RMSE nor an arbitrary upper-tail cutoff
constitutes scientific approval.

**Next bounded work:** retain the current C# API and
select one defensible **one-state** candidate profile
using source-matched whole-plant observations. Treat
missing dimensions as named, sensitivity-bounded
engineering assumptions rather than measurements.
Wheat W13 is a valid 24h constraint but not proof that
generic Medieval Overhaul wheat represents either
named winter cultivar; do not silently activate it.
No new six-class runtime system, universal acclimation
counter, or guessed production values. Consolidate
game tests only after a real implementation slice.

**Decision boundary:** authorization here is for the
*approximation method*, NOT a specific numerical
parameter or production migration. Keep PR #24 Draft;
no changes to C#, live XML, Steam or default gameplay
from this design-policy update alone.


### First single-state quantitative candidate gate: W13 M39 (2026-10-21)

[Exact mathematical comparison](ColdDamageCalibration.md#w13-single-cohort-approximate-fit-identifiability-2026-10-21)
and [reproducible candidate table](Research/WinterWheatW13Moskovskaya39ConditionalFits.csv)
is now available for *Moskovskaya 39 wheat seedlings
conditioned at 4°C for 7 days only*.
**It does not describe all generic MO Wheat.**

- At **24-hour** target conditions the source's
  whole-seedling survival at **0/−3/−5/−7°C**
  is **100/97/65/15%**. The current additive
  cold injury with *bounded uniform lethal-dose
  variation* can produce **100/100/65/15%**
  (in-sample **1.50 percentage-point RMSE**)
  without introducing another runtime engine or
  changing the individual CDF family.
- This does **not** identify an onset or fatal
  degree-hours. Mathematically different
  **unapproved example** sets
  (onset **−1.6°C**, baseline lethal
  **96°C·h**, variation **0.5**)
  and (onset **−3.1°C**, baseline lethal
  **60°C·h**, variation **0.8**)
  give the **same four 24-hour predictions**.
  At −5°C for 48h, their *unmeasured forecasts*
  diverge to **0%** versus **17.5% survival**.
  These examples are **neither empirical parameter
  estimates nor production XML values**.
- The observed **−3°C 97±3%** is
  **not** an observed 100%; both examples miss its
  central value by +3 points and its uncertainty
  cannot be treated as a verified confidence
  interval without the original sampling definition.
- Pre-acclimation is a **prerequisite state** of
  this treatment. The existing generic opt-in
  accumulator has **no biological hardening state**;
  applying either fitted profile to all MO Wheat
  would silently assume identical physiology.
  Experiments use freezer target temperatures and
  lab exposure hours, not the current RimWorld
  map-air sampled temperature and seasonal clock.
- **Disposition:** the current linear + uniform
  implementation is numerically feasible at the
  one measured 24h profile, so **do not introduce
  a Weibull CDF, nonadditive exposure or acclimation
  subsystem to solve this isolated fit**.
  **Neither example qualifies for plant activation**;
  first require a bounded game-time translation,
  physiologically applicable default-state policy
  and the intended short/multi-day frost outcomes
  (or matched empirical durations).
  Preserve current 48 finite-death, 30 dormancy
  profiles, Japanese UI labels, C# and saves.
  Draft PR #24 remains Draft; no game test
  or Workshop promotion from this fit alone.


### Time and acclimation implementation boundary (2026-10-21)

**Resolved process-level design:** retain direct **game-hour**
accumulation for *acute cold injury*. Treat **hardening
(acclimation)** as a separate biological process, **not**
a multiplier inferred from RimWorld's shorter calendar.
No biological rate or crop activation is approved here.

#### In-game versus laboratory time

- The implemented sparse injury engine divides ticks by
  **2,500 ticks/game-hour**. RimWorld has 24 hours/day,
  60,000 ticks/day and **60 days/year** (15 days/quadrum).
  An acute *24 lab-hour constant-temperature* treatment
  is compared initially with **24 game-hours of continuously
  observed matched temperature**. This is a declared
  **numerical comparison convention**, *not evidence of
  biological equivalence* or an approved game lethal dose.
- **Do not multiply acute injury by 365/60 or 60/365**
  merely to represent compressed game seasons. A
  one-day cold snap remains a one-day weather event.
  The degree-hour accumulator already measures exposure
  in game-hours; no mandatory time-scale XML field
  is needed.
- The W13 **4°C ×7 lab-day preconditioning** is a
  different physiological process. Seven game days
  would consume nearly half a 15-day quadrum;
  scaling by the calendar ratio would yield
  ~1.15 game days. Neither hardening duration is
  *measured* or *automatically approved*: W13
  measured the endpoints, **not** the progress
  during those seven days or loss of hardening
  after a warm spell. Rate choice must use
  explicit gameplay sensitivity and source
  identification, not a universal clock rule.
- A source **freezer target** is not automatically
  plant crown temperature; the current engine
  uses **AmbientTemperature**. W05 measures
  cycling soil minima. Seasonal and physical
  temperature mappings remain explicit limitations.
- The existing CheckMakeLeafless sample captures
  only the current ambient temperature. First
  below-onset observation has no retroactive injury;
  elapsed intervals are capped at 2,000 ticks
  (0.8 game-hour), so missing/staggered callbacks
  can undercount a long exposure. Do not claim that
  an ideal continuous 24h fit is **already**
  faithfully executed over 24 game-hours.
  Verify this only with the smallest consolidated
  end-stage real-game acceptance.

#### Physiological state: design choice and cost

| Option | UX and technical consequence | Choice |
|---|---|---|
| All wheat always fully cold hardened | Warm-grown/new wheat receives unearned frost resistance, contrary to W13 | **Reject** |
| Treat dormancy, minGrowthTemperature or winter quadrum as hardening | No prior-temperature memory; indoor or recently planted crops become falsely acclimated | **Reject** |
| Universal daily 7-day hardening rule | Invents cross-species temperature band, rate and loss kinetics; ignores 15-day quadrums | **Reject now** |
| Optional, species-scoped *separately saved* hardening state | Temperature-history based, but increases per-plant state even without injury; requires numerical calibration and UX validation | **Preferred future architecture** |
| Current opt-in injury without acclimation | Cheap sparse prototype and backwards-compatible; cannot claim to simulate W13 pre-hardened plant generally | **Retain for PR #24** |

For a later **source-supported** optional hardening
implementation, the expected contract is:

1. **Cold injury**, **hardening** and Vanilla
   **dormancy** are three distinct concepts.
   Dormancy still begins below the plant's
   existing minGrowthTemperature. Hardening
   cannot be inferred from dormant status or
   plant-ID random rank.
2. Newly spawned plants have no retroactive
   observed cool-temperature history.
   An eligible, moderately cool exposure may
   raise bounded hardening, warmer exposure may
   reduce it, and severe freezing must not
   grant **instant full protection**.
   The hardening response to a freeze depends
   on the state *before* that freeze.
   Temperature band, rates, decay, cultivar
   range and interpolation of hardening into
   dose/onset all remain **unapproved numeric
   hypotheses**, not values measured by W13.
3. A per-plant saved state is required for the
   history-dependent response and must also
   survive warm intervals. If using existing
   staggered plant callbacks, no *new per-tick
   scan of every plant* is warranted.
   Unlike sparse injury-only state, it will
   store records for some **non-injured
   cool-exposed plants**, increasing memory.
   Stale references must be pruned; state can
   be dropped only once injury and hardening
   have both genuinely cleared. Save/load
   behavior and planting/removal require
   focused eventual checks.
4. Keep temperature context physical:
   ambient air, protected indoor climate and
   soil/crown frost are **not interchangeable**.
   Do not make plant hardiness a latitude or
   seasonal calendar buff. W13's 22°C
   untreated versus 4°C ×7d pretreated
   cohorts do **not** provide a universal
   multi-step response trajectory.
5. Before activating any crop's acclimation
   profile, require at least a defensible
   source/state assignment, a stated game
   clock interpretation, short/multi-day
   frost outcomes, warm-spell dehardening
   assumptions and measured constraints.
   Existing studies of winter wheat, including
   FROSTOL, establish that hardening and
   dehardening matter but do not validate a
   one-rate-for-all-crops shortcut.

**Implementation sequence:** finish the existing
C# sparse opt-in injury slice and defer only
the *physiological hardening* subsystem.
The experimental W13 M39 A/B coefficient
sets remain research-only; no generic MO Wheat
XML activation. Avoid a placeholder
acclimation counter that cannot yet have
defensible gameplay behavior.
Preserve the **48 existing fixed death profiles**,
**30 dormancy profiles**, their Japanese Info
Card labels and old-save defaults.
No new biological numerical values, production
XML, native test, merge or Workshop release
are authorized by this decision. PR #24
remains Draft.

**External evidence:** RimWorld time
https://rimworldwiki.com/wiki/Time ;
W13 Deryabin et al. 2024
https://doi.org/10.3390/metabo14040199 ;
FROSTOL Bergjord et al. 2008
https://doi.org/10.1016/j.eja.2007.10.002
(soil temperature, hardening and dehardening;
not general crop coefficients).


### First plant-specific numbers that can be bounded without guessed XML (2026-10-22)

[The owner calibration calculation](ColdDamageCalibration.md#w13-four-cohort-numerical-constraints-observed-interpolated-identifiable-2026-10-22)
and [four-cohort derived CSV](Research/WinterWheatW13FourCohortMedianBounds.csv)
now distinguish actual **24-hour whole-wheat-seedling survival**
from **interpolated conditional LT50** and inferred
parameter products. The original assay counted undamaged
seedlings after 24h at 4°C in darkness, followed by
72h at 22°C in light. Its surviving fractions are
not field wheat mortality under constant map air
temperature.

| Wheat cultivar/state | Linear fraction-interpolated 24h T50 | What it does **not** mean |
|---|---:|---|
| Moskovskaya 39, non-acclimated | **−2.308°C** | Not a death threshold, injury onset, or cold-damage Def |
| Moskovskaya 39, 4°C ×7d acclimated | **−5.600°C** | Not generic MO Wheat frost tolerance or damage onset |
| Zlata, non-acclimated | **−1.765°C** | Not a second sample of the same genotype |
| Zlata, 4°C ×7d acclimated | **−3.377°C** | Not a directly measured LT50 or acclimation-rate equation |

In the existing uniform-individual-lethal-dose model,
**only the product `w*D` is conditionally identifiable**
from two reported interior survival points: **48.000°C·h**
for M39-acclimated and **45.283°C·h** for Zlata-acclimated,
where `w` is the allowed fractional lethal-dose
half-width and `D` the median baseline lethal
degree-hours. The respective approximate
published-± endpoint sensitivity intervals are
42.857–54.545 and 41.379–50.000°C·h.
These are **conditional math, not two separately
measured biological parameters**. A 100%-survival
boundary cannot be used as an unsaturated uniform
distribution point. Median T50 is likewise not
`coldDamageStartTemperature`.

**Immediate crop decision:** source-verified
**conditional medians and one combined shape/dose
product are recorded**. The first generic wheat
**onset, fatal dose and individual variation
remain UNDECIDED** because multiple setting
tuples match each cohort and acclimation/actual
tissue temperature are not represented by
the Draft engine. Between-genotype differences
(65% versus 7% survival at acclimated
−5°C×24h) are not within-genotype susceptibility.
Do not silently map one chosen W13 genotype
to `DankPyon_Plant_Wheat`, nor transfer
these wheat figures to other 77 Defs.
No production migration, engine extension,
game-test rerun or release is justified
by this numerical interpolation alone.


### CCTO numerical V1 proposal REJECTED after independent audit (2026-10-22)

**This corrective decision supersedes the previous 'locked
before XML' section in its entirety.** The numerical
selection of 48 cold-injury profiles was **not validly
reviewed**, and the original approval/lock statements
are **withdrawn**. The numerical rows remain available
as a failed historical candidate in
[ColdDamageBalanceV1.csv](ColdDamageBalanceV1.csv),
marked **REJECTED FOR XML** in the canonical
[review state](ColdDamageBalanceReview.json).

The independent [causal audit and reproducible
counterexamples](ColdDamageBalanceFailureAudit.md)
found the central mistake: the author of the values
selected arbitrary target cold durations, set
`onset=old fixed death + offset`, back-calculated
`D=(onset-old fixed death)*duration`, and then
declared success because `D/(onset-old death)`
reproduced the selected target. Checks of
78 unique DefNames and these tautological
identities establish only **schema/math consistency**.
Neither source science, within-year nocturnal
recovery, 15-day seasonal exposure nor natural
forest persistence had been independently
tested prior to approval.

Critical *unresolved* biological/gameplay issues
include W13 acclimated-vs-naive wheat cohorts,
R02 rice chilling after preconditioning,
a sharp −1.9°C/−2.0°C maize response,
wheat attrition across repeated cold nights,
and pine injury recovery at −30°C.
RimWorld's 1.6 engine-only Pickle and real
Scribe save/reload evidence remains genuine
but did **not** verify these profile values.
No general plant death XML has opted into
the draft cumulative model.

**Requirements before new selection:** separately
specify empirical physiological stage and target
temperature, choose independently acceptable
short/multi-night/long-cold/warm-recovery
outcomes for representative species and wild
ecology, test proposed numbers against those
expectations, disclose source vs gameplay
assumptions, and resolve any major mismatches.
An author's instruction to decide values first
does **not** permit the proposing agent to label
its own unverified coefficients "accepted".
No V1 XML migration, PR merge or publication
is authorized; the 48 legacy fixed-death and
30 existing dormancy settings remain current.


### V2 numerical recalibration — predeclared outcomes and restricted candidates (2026-10-22)

**V1 remains rejected for XML.** The user's instruction to
redo the numerical balance authorizes selecting and
checking *new candidates*, **not** approving internally
invented crop physiological coefficients by default.

The V2 process **first** recorded independent
[temperature-duration/biome and biological-state
targets](ColdDamageBalanceV2Targets.md)
(commit `07a695f741c1a36bc646d1f3149a11bbaebd939d`)
**before** computing or writing any candidate numbers.
Then it produced [the exact 78-Def, status-bearing V2
table](ColdDamageBalanceV2Candidates.csv) and a
[side-by-side independent outcome evaluation](ColdDamageBalanceV2Assessment.md).

**Current scope:** 30 dormancy unchanged, 41 other
legacy lethal profiles have no responsibly justifiable
V2 numbers yet, 2 example annual crops (MO tomato
and cabbage) have explicit **unapproved gameplay
candidates**, winter wheat has a *non-acclimated*
numeric proxy **blocked for generic MO Wheat**,
Pine/Great Fir have two paired ecological candidates
**blocked by below-freezing recovery semantics**;
rice and maize are **biologically/model blocked**.
Altogether **5 numeric candidate rows (0 XML-ready)**;
the other **43 fixed-death profiles have blank V2
parameter fields** (including rice/maize).

V2 annual candidate numbers (not live XML):

| Plant | Injury onset | Median baseline fatal dose | Individual dose half-width | Recovery/game hour |
|---|---:|---:|---:|---:|
| MO Tomato | 0°C | 18°C·h | 0.25 | 0.010 |
| MO Cabbage | −3°C | 72°C·h | 0.25 | 0.007 |

Both are tested only against the independent
predeclared *gameplay* frost/night constraints.
Their coefficients are **not lab-calibrated**.

Source-conditional comparisons and proposed
numbers for wheat and cold-evergreen trees,
plus the fundamental near-onset maize and
conditioning gaps for rice/wheat, remain
**explicitly not ready for XML**. The current
CCTO `ColdDamageProgress.Advance` historically recovered
at ambient T≥onset, including −30°C pine; the present HP draft does not heal HP;
none of the numerical retries may be approved
by pretending this already represents thaw-only
repair. A minimal potential no-extra-state
correction is a **proposed** thaw floor
`T≥max(0°C, onset)` for future review, not
implemented code.

For maize, simultaneous exact linear fits to
M01's approximately 50% endpoints at
−2°C×48h and −6°C×2h mathematically
force onset ≈−1.826°C and unacceptably
steep near-onset sensitivity; this is
a model-assumption problem, **not** permission
to publish arbitrary precision. For rice
and winter wheat, physiological preconditioning
changes measured survival at identical
freeze conditions and requires either explicit
state tracking or a clearly author-accepted
limited-scope proxy. All original main/Steam
fixed-death/dormancy XML and 1.6 native
engine test evidence remain unchanged.

**Next stage:** independently evaluate the
remaining lifeform groups against their own
climate/source targets, settle the extremely
small recovery semantics change before approving
wild trees, and resolve how to represent
unacclimated vs acclimated cereal cohorts.
Do not reset the V1 review file to
`accepted_for_xml`; no production XML
integration or game-test rerun has been authorized
by this numeric analysis alone.


### V2 recovery and physiological-state boundary (2026-10-22)

**Draft engine decision, not a biological numerical approval.**
Commit `9dfd6d7a501676a28ee4fa92f84181b7e364af34`
changes opt-in injury recovery to **T ≥ max(A, 0°C)**,
where T is RimWorld AmbientTemperature and A is the
plant's injury onset. Below A, accumulate injury;
between a negative A and 0°C, preserve existing injury
without falsely repairing frozen plants; above the
threshold, recover injury at the existing configured rate.
For above-zero chill onset, recovery remains at A.
This adds **no per-plant fields, state, XML parameters,
UI or new map scans**. The ambient 0°C floor is a
simple gameplay thaw proxy, not an experimentally
measured tissue repair threshold. Existing 1.6 native
test results precede this change: updated branch
**compilation and real-game behavior unverified**.

**Separate conditioning policy:** retain winter
wheat and rice's *existing fixed death* production
rules while lack of actual physiological history
makes numerical cumulative-damage coefficients
non-identifiable. W13 and R02 show same condition
following different prior temperature histories
produces radically different survival: there is
no defensible constant hardiness multiplier for
all generic wheat/rice. Do not equate dormancy,
random ThingID sensitivity, time since planting
or calendar winter with hardening.

Future acclimation, if separately justified by
source+gameplay evidence, must be an **optional
history-aware per-plant state** on just the
eligible species. It must survive before cold
injury, respond to **observed cool exposure**,
and decay separately in warm periods; it is
not an automatic part of the sparse injury record.
Avoid a universal hardening rule across all crops
and avoid inventing rates from one endpoint.
Only after actual state/time parameters and
climate outcomes are accepted can wheat or
rice opt into full injury simulation.

[Independent V2 acceptance outcomes and revised
winter carryover analysis](ColdDamageBalanceV2Assessment.md)
show that the pine/fir **3300°C·h candidate**
can still lose ≈18% of the simulated population
after 60 days of −35°C nights/−30°C days
**without** imaginary subzero healing.
That is an unresolved ecological consequence,
not proof of appropriate wild-tree balance.
All 5 V2 numeric rows remain *unapproved*,
V1 is still rejected and no production XML
changed. This policy resolves only the
code-level recovery flaw and the allowable
scope of later conditioning work.


### V2 numeric candidate extension using predeclared S19–S31 (2026-10-22)

**Candidate expansion; NOT author-approved values or
production XML migration.** The independent
[S19–S31 source/stage/weather targets](ColdDamageBalanceV2Targets.md)
were committed as `15d4d3acda732e42d13466649beca047b9cfd83d`
**before** any new rows. Their original literature
identity/organ limits are in
[the evidence ledger](ColdDamageEvidenceLedger.md).
The exact [V2 candidate data](ColdDamageBalanceV2Candidates.csv)
and [independent outcome comparison](ColdDamageBalanceV2Assessment.md)
now have **7 numeric candidate Defs** in total:
**4 annual gameplay candidates** (tomato, cabbage,
onion, pumpkin) and **3 still unapproved
biological/ecological conditional profiles**
(wheat, Pine and MO Great Fir).
Another **41** original death Defs have
**no V2 coefficients**, including rice,
maize, strawberry and potato; **30**
dormancy Defs remain unchanged.

New conditional candidates, for the sole purpose
of evaluating independently stated outcomes:

| Def | Onset A | Median fatal dose D | Uniform halfwidth w | Recovery/h | Status |
|---|---:|---:|---:|---:|---|
| MO Onion | −3°C | 21°C·h | 0.25 | 0.009 | Gameplay candidate only |
| MO Pumpkin | 0°C | 20°C·h | 0.25 | 0.008 | Gameplay candidate only |
| Vanilla Pine / MO Great Fir | −31°C | **3600**°C·h (was 3300 in earlier V2) | 0.20 | 0.004 | Long-winter safety trial, biological calibration lacking |

The two annual candidates fit predeclared
short severe/multi-night survival targets,
but no cited experiment directly measures
their full fatal-dose, repair-rate or
within-cultivar spread. The evergreen
candidate now predicts **100% survival
after 60 synthetic days** with -35°C
nights/-30°C days versus **81.8%** with
the old 3300 dose. At constant -40°C
for 15 days it predicts **75%** survival
versus previous **54.5%**. Both outcomes
are compared openly; actual weather
frequency, crown temperatures and
multi-year population ecology remain
unverified. This is **NOT** a
species-specific physiological fit.

**Important type-of-death boundary:**
secondary onion seedling survival after
short frost does not imply bulb scale
LT50, strawberry crown/living perennial
survival is not a flower-bud injury
measure, and frost-damaged potato
tops need not mean death of the whole
tuber-bearing plant. Consequently
**strawberry and potato retain no
V2 whole-plant fatal-dose settings**
rather than inheriting a superficially
plausible group coefficient.
The old fixed thresholds continue
in runtime until an explicitly
accepted migration is implemented.

**Review gate remains closed:** V1
`rejected_for_xml` still applies;
no V2 table checksum or scenario
evidence is author-accepted,
no runtime games were launched for
these candidate numbers. The
new rows are suitable for further
review, **not for a blanket 48-Def
migration or release**.


### V2 parameter sensitivity remains unapproved (2026-10-22)

The V2 onion, pumpkin and two-quadrum evergreen
proposals also received a separate **post-selection
temperature perturbation audit**, reported in
[the V2 assessment](ColdDamageBalanceV2Assessment.md#post-selection-temperature-sensitivity-check--not-a-predeclared-acceptance-pass).
The same three-night onion condition changes
from **72.5%** survival at −4°C nights to
**37.1%** at −4.2°C; pumpkin from **58.9%**
at −1°C to **20.7%** at −1.2°C. Sixty days
of pine/fir −35°C nights give **100%**
survival under ideal zero-history assumptions,
but −35.2°C nights yield **90%**. These
perturbations were **not** part of the original
predeclared S19–S31 acceptance set and
cannot retroactively be called successful
pre-selection testing.

Meeting the narrow original targets is
**not sufficient evidence of robust
climate gameplay balance**. Do not
automatically retune or approve the
same candidate values based on arbitrary
new threshold checks. A representative
temperature-frequency profile, plant
temperature offset and permissible
player-visible failure envelope must
be considered before any V2 coefficient
is labelled ready for XML. Owner
[review status](ColdDamageBalanceReview.json)
remains `rejected_for_xml` for V1 and
no V2 numerical profile is approved.


### V2 seasonal climate eligibility versus frozen-night arithmetic (2026-10-22)

V2's early 15-/60-day **fixed nightly
temperature** scenarios were **stress
fixtures**, not proven real wild-forest
or crop-growing-map climate frequencies.
The separate [60-day game-generated
one-tile forecast and synthetic colder
offset exploration](ColdDamageBalanceV2Assessment.md#seasonal-temperature-example-and-climate-envelope-risk-2026-10-22)
demonstrates why the earlier claim
"pine/fir survive 60 days at −35°C"
does **not** validate their balance
in a real RimWorld forest biome.
That publicly posted external 1.6
log is one heavily modded
**latitude 22.2169°, average 16.1°C**
tile, *not* a representative winter
forest tile and *not* hourly weather
actually played over 60 days.
Shifting its climate by uniform
−40 to −45°C is explicitly
**hypothetical**, not a valid
biome/latitude world generator.

For its measured daily extrema,
the candidate onion's **−3°C**
injury onset is never crossed
(min reported daily low −1.09°C).
For a hypothetical three-day
autumn −5°C anomaly on this
trace, ideal survival is ~67%
and a −5.2°C anomaly ~48%,
so crop loss remains highly
sensitive to nearby temperature
paths. Reusing the logged 60-day
curve with unrealistic −43°C and
−44°C *annual shifts* produces
~93% and ~55% hypothetical
pine/fir survival; these results
are **not** probabilities of
wild-tree mortality on a
biologically eligible tile.
The source
[`RimWorldPublicOneTile60DayLoHi2026.csv`](Research/RimWorldPublicOneTile60DayLoHi2026.csv)
contains full third-party
provenance and limits.

Also separately treat RimWorld's
**ColdSnap** incident (~−20°C
temporary weather anomaly for
1.5–3.5 game days with a gradual
onset) rather than calling
fixed nightly climate extremes
an ordinary cold snap. This
event's trigger eligibility
depends on the world's seasonal
temperatures, unlike an
arbitrary imposed shift.

**Formal V2 decision: keep
all seven proposals unapproved
without further coefficient
changes.** The necessary
independent validation input
is a small set of **actual
RimWorld-generated, tree-bearing
cold-map seasonal Lo/Hi
distributions with their
world-tile settings**, plus
one representative crop-growing
tile. Simulations for these
maps must respect real season/
incident frequency and
the difference between ambient
temperature and crown/root
temperature. There is no
evidence yet to select an
acceptable wildfire/forest
attrition rate or retune 3600
degree-hours based on a single
external subtropical trace.
No runtime, XML or prior
native test evidence changed.


### Native original RimWorld 1.6 climate input for V2 numerical review (2026-10-22)

**New, genuinely original-game climate evidence:** the
manual Project 1.6 climate collector
[Actions #39776410000](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39776410000)
built and loaded a source-pinned temporary
CCTO Quickstart against original **RimWorld Linux
1.6.4871 rev598**. The naturally generated
BorealForest sample map contained **483 original
wild `Plant_TreePine` plants**. The original
`TileTemperaturesComp` was sampled
**hourly for a whole 60-day game calendar**
on **two real pine-eligible BorealForest
tiles** and **one unforced marginal
TemperateForest crop tile**, producing
**4320 original world-temperature samples**,
game process exit 0, Unity ERROR 0,
Quickstart 3/3 PASS. The original
unrounded CSV is retained in Project
artifact 12424418617 (SHA256
`6eac0d50124fbd622d37737abd1a86223b3649b27133e3e9349a43797e37b8c5`);
a [stable 180 daily low/high
subset](Research/CCTOVanilla16ActualWorldClimateDailyLoHi.csv)
is stored in this owner repo.

**Reality boundary:** this was
the game's real seasonal *world-tile*
temperature component queried
at calendar times, **not**
60 days of map ticking,
weather incidents or observed
plant frost injury. The unforced
cold pine forest tile's annual
mean was **−5.536°C**
and its sampled minimum
**−37.835°C**; it spent
only **92 hours below the
Pine candidate onset −31°C**.
The median pine-bearing forest
tile (−1.370°C annual mean)
spent only **6 hours** below
−31°C. Ideal V2 Pine/fir
A=−31°C,D=3600°C·h survives
100% in both temperature
sequences; this establishes
**candidate resilience on these
two Vanilla calendar pathways
only**, not physiological
appropriateness or global forest
biome validity. In a similarly
unforced temperate farming tile
(+5.672°C mean, minimum −23.709°C),
four numerical annual trial crops
survive the first 20 calendar
days from game start, but die
before a hypothetical full
year if never harvested.
No actual harvest/maturity
was tested.

The [temperature-driven
candidate output and
full evidence distinctions](ColdDamageBalanceV2Assessment.md#original-rimworld-16-cold-forest-and-farming-tile-generated-calendar-2026-10-22)
replace earlier reliance on
unverified uniform -40°C
offsets applied to a subtropical
third-party daily log. **The
seven V2 coefficient rows remain
UNAPPROVED**; no XML, runtime
or Steam changes are implied.
Full Medieval Overhaul was not
loaded, so Great Fir ecology
is a proxy transfer only.
Actual incident weather,
existing planted/preserved crops,
sunlight, growth maturity and
1.7 remain unknown.


## V2 design direction — HP damage, no cold-only recovery (2026-10-22; experimental)

The previously drafted independent `damage=0..1` pool and warm-weather healing are superseded for V2 by proportional damage to the plant's existing HP. One degree-hour dose can lower HP without killing; unrelated damage shares the same HP and can hasten death. Temperature above the injury onset stops additional cold HP damage, **not** restores it. Round-off below one HP is retained until subsequent observed frost. Individual lethal-dose variation remains experimental. Do not add acclimation, organ-specific simulation, extra per-tick sampling, or special cold healing. Legacy 48 fixed-death and 30 dormancy profiles are unchanged. Prior numeric V2 survival percentages/calibration targets need reassessment under the HP model, not relabeling as approved XML. Detailed schema contract is in `Docs/Framework.md`.

### HP-only V3 numerical trial ownership (2026-10-22)

The complete 48 ordinary/30 dormant Def coverage and provisional two-parameter-onset/dose model is maintained in [ColdDamageHpV3Candidates.csv](ColdDamageHpV3Candidates.csv), with outcome and risk review in [ColdDamageHpV3Assessment.md](ColdDamageHpV3Assessment.md). These experimental values supersede the older reversible-injury **proposals for evaluation only**. They do not supersede approved production fixed-death XML or constitute author-approved per-plant settings. Wild winter survival requires multi-year safety under irreversible HP losses; no published plant data justify automatic promotion. The separate recovery rate is absent from V3 numeric candidates. See [Framework](Framework.md) for current experimental HP behavior.

## CCTO V3 HP implementation-method re-audit (2026-10-23)

**Scope:** source-level review of `ColdDamageMapComponent`, `ColdDamageProgress`, Harmony plant hooks, sparse Scribe record and existing draft Quickstart/Pickle tests; **not** new real-game proof. Original RimWorld 1.6 C# build previously passed (Project Actions #39806727803) but runtime HP/Scribe/patch execution has not been accepted. Numeric V3 candidate approvals are separate and remain withheld.

### Method retained

- Use **ordinary Plant HP**, not an auxiliary normalized injury gauge; no special cold HP healing. Existing `DamageDefOf.Rotting` is also used by CCTO's already-existing fixed-death path and by older Vanilla plant deterioration implementations. Merely seeing this DamageDef does **not** prove a bug; confirm actual 1.6 death/damage behavior only in the consolidated native check.
- Keep the opt-in pair `coldDamageStartTemperature` and `coldDamageDegreeHoursToDie`, deterministic optional dose variation (default disabled), `Plant.CheckMakeLeafless` patch and sparse MapComponent with last observation tick and sub-HP remainder. No new per-tick scanning, specialized acclimation/organ system or cold-specific regeneration.
- Temperature is sourced from the **plant ambient** (including indoor room), not the global outdoor tile weather. Existing fixed cold-death and 30 dormancy XML definitions are unaffected.

### Findings and acceptance relevance

1. **Observation approximation, material for short frost:** the first below-onset sample records only the clock, charging zero prior dose; later samples assign the **current sampled temperature to up to 2000 previous ticks**. If temperature changes between samples, a short frost can be missed, or a just-arrived frost can be charged to earlier warm time. Gaps longer than 2000 ticks are truncated. This is a conscious simplicity/performance tradeoff, **not** a measured continuous dose. Must be shown acceptable for ordinary frost events before numeric V3 acceptance; do not pretend constant-hour offline CSV integration is identical to the live sampler. Avoid adding a new temperature-history subsystem unless an actual gameplay discrepancy demands it.
   **2026-10-23 implementation correction:** Sparse HP records additionally persist `lastObservationWasCold`. After a warm observation retained only for sub-HP remainder, the first subsequent cold observation restarts cold timing without charging the preceding warm interval. A following cold observation resumes bounded exposure. This fixes warm-to-cold overbilling without a temperature-history system; boundary timing remains approximate and native confirmation is pending.
2. **Fatal cold-death UX notification — source fix applied (2026-10-23):** `ApplyHitPointLoss` now captures relevant crop identity/label/map only for a potentially fatal hit and calls the existing shared `MessagePlantDiedOfCold` notification **after `TakeDamage` confirms plant destruction**. Partial cold HP damage sends no death message; fixed-temperature CCTO death uses the same shared notification helper without changing its prior pre-kill signaling. The actual 1.6 game notification has **not yet been runtime accepted**.
3. **Native plant HP has an existing yield consequence:** RimWorld's plant harvest yield uses the plant's `HitPoints / MaxHitPoints` factor. A crop at half HP yields approximately 75% of the same crop at full HP before other harvest multipliers, not the same yield. Thus V3 already produces a partial-frost yield penalty **without a new production bonus/debuff subsystem**. Treat this as intended/observed design consequence for balancing; verify against 1.6 loaded behavior if needed rather than adding a duplicate yield penalty.
4. **Save state and rounding:** fraction <1 HP is preserved across warm observations and Scribe; post-load reconciliation drops stale/destroyed records. This is reasonable for fractional accumulating HP damage, but both first sample and actual .rws identity/HP restoration are **not proven by compile or forced-hook tests alone**. The existing test quickstart directly invokes `CheckMakeLeafless` and manipulates `lastTick`, so it cannot establish that ordinary in-game scheduling covers all temperatures.
5. **Limits, not release blockers by themselves:** the regular sample cap and lack of age/cultivar acclimation are intentional simplifications; deterministic variation width 0.20 and all 48 dose pairs remain unapproved. Real gameplay may diverge from idealized exposure predictions due to sparse sampling, integer HP, other HP injuries and plant growth/harvest timing.

**Disposition:** retain the simple HP architecture; do **not** add healing or biological simulation. The fatal cold-death notification has since been fixed in the V3 Draft; the exact V3 source built against 1.6 managed assemblies, but normal-tick exposure, notification and saved-fraction continuation remain pending consolidated original-game acceptance before production XML conversion. Numerical owner should calibrate against what the sampler actually does, not only continuous-temperature arithmetic.

## V3 accepted frost-role direction for MO cabbage and onion (2026-10-23)

**Author-approved design priority (not numeric approval):** For MO cabbage and MO onion, prefer **survival through representative hard frost with ordinary plant HP injury and reduced harvest yield**, rather than forcing ≥70% plant deaths in one cold episode. This explicitly supersedes the former gameplay-only S04 and S20 high-mortality gates. The original dated trial rows remain retained as historical evidence in [ColdDamageBalanceV2Targets.md](ColdDamageBalanceV2Targets.md); the **active V3 test descriptions** and limits are in its V3 supersession section. Both hardy crops may die from sufficient **longer/deeper/repeated** exposure because CCTO still uses ordinary HP with **no special cold recovery**.

**Scope/compatibility:** The direction uses current opt-in V3 HP damage and the existing `Plant.YieldNow()` HP factor. No additional crop-yield penalty, temperature model, growth-stage simulator, recovery mechanic, modification to legacy fixed-death XML, or change to the 30 dormancy plants is approved. Tomato/pumpkin strong-frost fatality goals are unchanged. **2026-10-23 later author numerical-candidate decision supersedes the prior illustrative values:** Cabbage **A−3.5°C / D240°C·h**, Onion **A−5°C / D120°C·h** are author-selected **V3 A/D candidates**. This does **not** authorize their 0.20 individual spread, their production XML opt-in, a release, or an in-game PASS. [V3 candidates](ColdDamageHpV3Candidates.csv) now explicitly distinguish **2 APPROVED_A_D_CANDIDATE_VARIATION_PENDING + 44 UNAPPROVED_HP_TRIAL + 2 FAILED_PREDECLARED_FROST_GOALS + 30 KEEP_EXISTING_DORMANCY**. The old D160 and Onion A−3.5 appear only in historical comparisons, not current selected values. [Harvest-yield comparison](ColdDamageHpV3YieldReview.md) is supporting analysis, not evidence of a native-game crop outcome.

### Approved V3 numeric *candidates* — not deployment authority (2026-10-23)

| Owned plant | DefName | Cold HP injury starts (A) | Full-HP cumulative lethal dose (D) | Numerical candidate status |
|---|---|---:|---:|---|
| MO Cabbage | `DankPyon_Plant_Cabbages` | **−3.5°C** | **240°C·h** | **A and D selected for candidate review** |
| MO Onion | `DankPyon_Plant_Onions` | **−5°C** | **120°C·h** | **A and D selected for candidate review** |

Both values embody the author's hardy-crop priority: short and moderate cold can reduce visible HP and harvest output without immediate mass death; extended/deeper cold can still kill. The values were selected using model-based exposure and expected harvest comparisons, **not** validated as experimentally measured physiological tolerances or observed in RimWorld 1.6. Both rows retain `lethal_dose_variation=0.20` only as a clearly **unapproved proposal**, not an active setting; the C# framework default remains zero variation. **Do not copy an entire CSV row into executable XML as an "approved profile."** Actual native ambient/tick/HP/save checks and the decision on variation remain pending; production retains the 48 old fixed-death profiles and 30 dormancy profiles. This decision changes no other plant A/D, Vanilla/MO balance, growth or recovery mechanics.

## V3 per-plant lethal-dose variability gate (2026-10-23)

**Approved numerical A/D candidate boundary is unchanged:** MO Cabbage A−3.5°C / D240°C·h and MO Onion A−5°C / D120°C·h remain author-selected *candidate numbers only*. **No individual variation width has yet been accepted** for these plants or any other HP profile; active production XML remains legacy fixed-death/dormancy. See [quantitative four-crop 0/10/20% comparison](ColdDamageHpV3YieldReview.md#independent-spread-width-comparison-for-v3--2026-10-23).

**Existing minimal mechanism:** A per-plant `ColdToleranceExtension.coldDamageLethalDoseVariation` (half-width `w`) modifies **D only**, not onset or damage calculation structure; it is deterministic per saved Thing ID and requires no added state or per-tick global plant scan. Current default **w=0**. Candidate CSV values **w=0.20** are *unapproved comparative placeholders* even in the two rows with accepted candidate A/D. Do not infer approval by reading/copying the entire row.

**Independent player-facing tradeoff:** For ideal constant exposure, cabbage A−3.5/D240 at −8°C×48h has field survival **100% at w=0 or .10, 75% at w=.20**. Onion A−5/D120 at −10°C×24h has **0% at w=0, about 50% at either .10 or .20**. Under not-yet-approved tender trials Tomato A0/D21.5 and Pumpkin A0/D22 at −3°C×8h, w=.10 leaves about **0% and 4.5%** alive, versus w=.20 about **20.9% and 27.3%**; this demonstrates that a universal width changes gameplay categories differently. These are hypothetical population expectations with constant plant ambient temperature and zero preexisting damage, **not native-game acceptance or physiological calibration**.

**Design recommendation, awaiting author decision:** Preserve w=0 default; keep per-Def variation opt-in rather than applying the unverified ±20% to all 48; compare a narrow w=.10 for hardy crops and wider w=.20 only for tender annuals if actual player-facing mixed survival warrants it. A fixed uniform dose multiplier is a gameplay approximation, not an experimentally fitted LT50/survival distribution. No extra age/acclimation or recovery system. **Do not change executable XML or mark a new width selected without express authorization.** A later consolidated live acceptance should evaluate actual ambient variations before concluding uniform plant populations need artificial variation.

## V3 tomato/pumpkin frost model selection boundary (2026-10-23)

**2026-10-23 author decision: A/D candidate values SELECTED.** Alongside previously selected hardy MO cabbage/onion, **MO Tomato A0/D21.5 and MO Pumpkin A0/D22** are now author-approved A/D *candidate* tuples. These replace the old D60/D68 preliminary failures without asserting actual gameplay PASS. Detailed independent numerical comparisons, nonzero population variation dependency, and verified original 1.6 world-tile hourly calendar are in [ColdDamageHpV3YieldReview.md](ColdDamageHpV3YieldReview.md#tender-annual-advariation-combination-review-2026-10-23).

**Selected A/D candidates (numeric scope only):** MO Tomato `DankPyon_Plant_Tomatoes` **A=0°C, D=21.5°C·h** and MO Pumpkin `DankPyon_Plant_Pumpkins` **A=0°C, D=22°C·h**. Historical −0.5°C onset alternatives (Tomato D18, Pumpkin D18.5) were reviewed but **not selected**. The original published XML/runtime remain unchanged. These candidates are gameplay compromises, not empirically calibrated whole-plant LT50/D results.

**Individual variation must be decided separately:** With A=0 and a controlled −3°C×8h trial (24°C·h), Tomato D21.5 gets any uninjured survivors only when uniform half-width w is over ≈0.1163, and Pumpkin D22 when w exceeds ≈0.0909. Therefore a purely model-fitted ±20% width would produce mixed populations (20.9% tomato and 27.3% pumpkin survivors), but ±10% leaves 0%/4.5%; **this is an algebraic constraint, not empirical evidence for w=.20**. Identically imposed S02/S16 and S23/S24 have equal cumulative lethal exposure because cold-specific HP recovery was intentionally eliminated, so do not double-count their pass status as independent validation.

**Actual player outcome:** Under idealized unmodified source world-hourly TemperateForest tile 2851 temperatures, A=0 trial profiles have only ~46% Tomato and ~50% Pumpkin **whole-field remaining harvest** by end of calendar day 25 if unharvested, with some crop deaths; under the alternate A−0.5 profiles neither crop has died at that point and harvest is ~61% of undamaged. These are not real growing map ambient temperatures, actual sow/harvest timing, Pickle or 2000-tick-sampler evidence. Source evidence remains Project original-game native Actions #39776410000 (original 4320-value SHA256 `6eac0d50124fbd622d37737abd1a86223b3649b27133e3e9349a43797e37b8c5`). Any final parameter selection must acknowledge the weak-frost survival vs severe-frost danger tradeoff; native acceptance is consolidated later, not launched for each value.

**Authorization boundary as of 2026-10-23:** The author has selected **4 A/D candidates** (Tomato A0/D21.5, Pumpkin A0/D22, Cabbage A−3.5/D240, Onion A−5/D120). Their individual `coldDamageLethalDoseVariation` w=.20 in the candidate CSV is **NOT authorized**; it remains provisional, with runtime extension default w=0. The remaining **44 HP A/D trials remain unapproved**, all 30 dormancy entries remain unchanged, and **no active failure flag remains for the four newly selected candidate A/D values**. S02/S23 severe-frost fit is still *model arithmetic* and requires actual-game confirmation. No conversion into production XML, release, extra recovery or growth-stage simulation is authorized.

## V3 empirical-first selection gate for remaining crop species (2026-10-24)

**Approved selection method:** Source measurements and **their actual species, biological stage, conditioning, measured tissue/whole-organ endpoint, exposure units, survival observation time and source uncertainty** must precede changes to the 44 remaining speculative HP A/D tuples. The author previously selected only MO Tomato A0/D21.5, Pumpkin A0/D22, Cabbage A−3.5/D240, Onion A−5/D120 as **numerical candidates**, not a runtime-ready HP profile or variation setting.

**First five source-first reviews:** See canonical [research audit](Research/ColdDamageV3SourceFirstAudit.md) and the original source CSVs in `Docs/Research`. Source studies identify material failure modes for:
- **Rice**: actual 10-day-old seedlings at **+4°C ×10 lab-days** after 12°C×5d conditioning show **88–100%** recovery/growth survival vs **0%** for unconditioned cohorts; current trial A=+3°C produces **zero** injury at +4°C no matter D or w.
- **Wheat**: original W13 −3/−5°C×24 lab-hours post-treatment whole-seedling death changes sharply after 4°C×7d conditioning; current A=−7°C yields **zero** cold HP injury at either treatment. Separate real prolonged −5°C acclimated-wheat survival studies also conflict with a *universal* zero-loss model; laboratory hours/crown temperatures cannot be equated silently to actual RimWorld crop ambient.
- **Maize**: two source secondary rounded ~50% seedling-death endpoints (−2°C×48h, −6°C×2h) are predicted by A0/D64 w=.20 as **all dead and all alive** respectively. Exact simple-degree-hour two-point median fit A≈−1.826°C, D≈8.348°C·h is **not adopted** because it badly amplifies 0.1°C onset sensitivity (S12).
- **Strawberry**: −5°C short freeze survivability applies to hardened dormant crowns, whereas −5°C can injure flowering/fruiting tissue. Current A−12°C ignores **all** −5/−10°C exposure, so an explicit crown-versus-fruiting gameplay endpoint decision is required.
- **Potato**: source −1 to −2°C frost *tops/foliage* damage can reduce tuber yield while preserving underground tubers; current A−3°C has **no effect** at −2°C. A whole-plant HP lethal dose cannot be derived from a frozen top or individual tuber measurement.

**Status separation (2026-10-24):** `Docs/ColdDamageHpV3Candidates.csv` retains all 78 rows and **no numeric A/D/w alteration** from before this audit. It now marks **4 `APPROVED_A_D_CANDIDATE_VARIATION_PENDING`**, **5 `HOLD_SOURCE_STAGE_ENDPOINT_UNRESOLVED`**, **39 `UNAPPROVED_HP_TRIAL`**, **30 `KEEP_EXISTING_DORMANCY`**. HOLD is a subset of the previously 44 unapproved species, **not author approval, evidence-based calibration or a new runtime model**. Never interpret it as five scientifically validated plants. Keeping the exact values visible is historical comparison only; they are blocked for executable XML adoption. Do not introduce a new cold-acclimation/state machine or organ-damage system before a separate author-approved scope/design change.

**Minimal next decision path:** For rice/wheat, select a representative cold-state / generic-crop purpose before comparing A and D to source survival; for maize, keep the independent short-versus-long severity and `S12` robustness constraint and seek additional same-cohort data; for potato/strawberry define whether Plant HP reflects alive crown/root, visible leaves, or harvest penalty. Then set a bounded A range from matching injury/death temperatures, D from multiple **matching** exposure durations where measured, otherwise explicitly label it a gameplay compromise; review normal climate and first/second frost cycles. Do **not** equate °C×lab-hours to °C×game-hours as if an observed physiological conversion had been established. No new native test or production XML conversion has been authorized.

### Second source-first set: MO lentil, MO carrot, Vanilla cotton, MO sugarcane (2026-10-24)

**Explicitly blocked from numeric adoption:** After checking actual original MO 1.6.2.2 Def identities/plant MaxHP and independent agricultural studies, add **MO Lentil `DankPyon_Plant_Lentils`, MO Carrot `DankPyon_Plant_Carrots`, Vanilla Cotton `Plant_Cotton`, MO Sugarcane `DankPyon_Plant_Sugarcane`** to `HOLD_SOURCE_STAGE_ENDPOINT_UNRESOLVED`. Original candidate A/D/w figures remain `Lentil −5/120/.20`, `Carrot −5/170/.20`, `Cotton +2/48/.20`, `Sugarcane +2/90/.20` **only as unapproved trials**.

**Why the direct plant-HP abstraction needs an explicit representative endpoint before numerical review:**
- **Lentil**: 1999 cold-screening study used 6–8-week **vegetative cold-acclimation**, −15°C×3h and found substantial genotype differentiation; a separate 2026 −4.8 to −2.2°C seed LT50 study measures **imbibed seed viability**, not grown-plant killing. Do not silently choose the hardiest conditioned genotype or seed LT50 as generic MO plant mortality.
- **Carrot**: freezing foliage can perish while **protected harvested taproots remain edible**; current A−5 yields zero field HP loss for −4°C×8h, but raising A to model top freeze can wrongly destroy the harvestable taproot through generic whole-Plant HP. Need a root-first gameplay objective without a new tissue model.
- **Cotton**: early seedlings suffer chilling/growth delays at temperatures far warmer than +2°C, but these are **not measured lethal whole-crop HP damage**; raising HP onset to 10–11°C to emulate slowed growth would instead accumulate permanent HP/yield loss without evidence. Current +4°C gives no injury for any duration.
- **Sugarcane**: LSU agricultural source separates freezing injury to **leaves/buds (0 to −3.9°C)**, **stalk tissue (~−3.9 to −5.6°C)** and **aboveground commercial plant tissues (colder)**, not the protected underground stubble. Current A+2/D90 deals ~53% MaxHP injury at −4°C×8h in the simple fixed-temperature model, a gameplay output **not supported as 53% whole-crop mortality or stalk necrosis by the source**. CCTO generic MO support retains this crop despite separate AMJ mainland cultivation exclusions.

Canonical evidence, source URLs, explicit temperature×duration HP/yield arithmetic and exact upstream MO Def harvest stats are in [ColdDamageV3SourceFirstAudit.md](Research/ColdDamageV3SourceFirstAudit.md#2026-10-24-second-source-first-cohort-lentil-carrot-cotton-sugarcane).

**Status after this four-crop cohort**: 78 coverage records = **4 selected A/D candidates** (individual width still pending), **9 source-/stage-/endpoint-HOLD**, **35 further unapproved HP candidates**, and **30 unchanged dormancy**. This supersedes the previous interim 4/5/39/30 count, not the old approved author A/D values. **No numerical A/D/w modification, newly enabled patch, C# implementation, age/acclimation system, or live-game test**. Next numerical step is to choose **one representative gameplay endpoint per plant** (green non-acclimated lentil; edible carrot root with protected underground part; cotton freezing mortality rather than warm chilling-growth stress; sugarcane productive stalk/aboveground fate rather than permanent stubble death), compare independent seasonal mild/severe cumulative exposures, and then calculate bounded source-informed candidate A/D **as gameplay approximations**, not numerical lab fit.

## V3 representative crop-role options and numeric shortlist (2026-10-24)

This entry clarifies the *minimal static HP/harvest interpretation* used when comparing the second source-first group; **it is a reviewer proposal, not author selection of A/D or biology**. Full source, growth-stage, organ, experimental endpoint and stress/yield matrix: [Research/ColdDamageV3SourceFirstAudit.md](Research/ColdDamageV3SourceFirstAudit.md#2026-10-24-four-crop-representative-endpoints-and-bounded-numeric-alternatives).

| Crop (current status HOLD) | Intended representative HP/harvest role | Existing A/D, unapproved | First alternative to scrutinize, UNAPPROVED |
|---|---|---|---|
| MO Lentil `DankPyon_Plant_Lentils` | Established green annual's living crown/basal meristem and ability to reach seed harvest; no flower-specific HP | **−5°C / 120°C·h** | **−4°C / 120°C·h** |
| MO Carrot `DankPyon_Plant_Carrots` | Storage-root survival and availability for harvest, not instantaneous leaf injury | **−5°C / 170°C·h** | **−7°C / 170°C·h** |
| Vanilla Cotton `Plant_Cotton` | Productive green aboveground stand/bolls; fatal frost injury not 0–11°C slowed growth | **+2°C / 48°C·h** | **0°C / 48°C·h** |
| MO Sugarcane `DankPyon_Plant_Sugarcane` | Productive cane stalk as the harvestable living Plant; no protected stubble/ratoon reproduction | **+2°C / 90°C·h** | **−3°C / 24°C·h** |

**Reasoning constraints:** Pulse seedlings can regrow from basal nodes after losing frost-injured tops (Saskatchewan Pulse Growers); protected carrot roots remain harvestable into winter (RHS/UMD); cotton chilling-growth injury is not a whole-plant frost fatality (Cotton Incorporated/University of Georgia); LSU AgCenter source differentiates sugarcane light-frost leaf/bud injury from stalk injury and loss of aboveground cane below ~−5.6°C. The CCTO `Plant.YieldNow()` ordinary-HP reduction approximates lost productive output only; neither yield factor nor D is a laboratory-measured mortality/rate for these specific crops.

**Concrete gameplay tradeoffs with proposed alternatives, w=0 and ideal constant map ambient temperature:** five −8°C×8h nights in Carrot move permanent HP attrition **70.6%→23.5%**; three +1°C×8h nights in Cotton move **50%→0%**; −6°C×8h Sugarcane move **71.1% injury with crop alive→full harvestable stalk death**; three −8°C×8h nights in Lentil move **60%→80%** damage and can be excessively punitive for regenerating younger plants. All predictions ignore actual source MaxHP integer quantization, intermittent game ticks, cultivar/stage and seed/fruit/root temperature gaps. This last Lentil effect is an explicit **counterargument** to choosing A−4 without further validation.

**Historical 2026-10-24 comparison boundary, superseded for two crops by the later author decision below:** The four alternatives were a narrowed **comparison set**, not scientific calibrations; the original status at the time was 4 A/D selected / 9 HOLD / 35 otherwise unapproved / 30 dormancy. Experimental survival endpoints cannot determine both A and D; prior author approval covers only Tomato, Pumpkin, Cabbage, Onion A/D. w=.20 is still an unapproved independent hypothesis. Retain generic MO Sugarcane support: AMJ Agriculture's regional sugarcane cultivation suppression does not amend the CCTO generic supported plant list. No production XML, C#, growth-state subsystem, cold-specific HP healing, release or native game job is approved. Choose numeric A/D only after considering root/meristem/stalk intent and bounded mild/frost-repetition consequences.

## V3 carrot/cotton A-D candidate selection and remaining two-crop hold (2026-10-24)

**Latest author approval superseding the four-way review-only table:** Approve only the **candidate numerical A/D** fields for **MO Carrot `DankPyon_Plant_Carrots`: A−7°C / D170°C·h**, and **Vanilla Cotton `Plant_Cotton`: A0°C / D48°C·h**. These are informed static-gameplay proxies, not measured tissue LT50/LD100 parameters or actual-game balance PASS. Carrot represents an edible root remaining harvestable despite top injury; cotton ordinary HP represents a productive growing stand under freezing stress, not all nonfreezing seedling chilling/growth slowdown.

This author selection changes the previous unapproved **onset A only** (Carrot −5→−7°C; Cotton +2→0°C); D=170/48 remains the same uncalibrated previous proposed dose. For Carrot, five ideal −8°C×8h nights now project about **23.5% MaxHP loss and 88.2% standing-harvest fraction** rather than 70.6% and 64.7%; for Cotton three +1°C×8h nights give **0% cold HP loss** rather than 50%, while −2°C×8h still predicts about 33.3% HP loss. This is the accepted *simplified role and A/D numerical-candidate compromise*, not a statement that leaf/root soil temperature was measured or that cotton at +1°C suffers no slow growth. Numeric variation w=.20 on both rows remains **UNAPPROVED**; C# default is w=0.

**Still held, no numerical decision:** MO Lentil `DankPyon_Plant_Lentils` retains original **A−5/D120**, comparison **A−4/D120** remains **not adopted**; practical pulse seedling frost survival/regrowth [Saskatchewan Pulse Growers](https://saskpulse.com/resources/seeding-tips-for-pulse-crops/) includes −4 to −6°C even when the main shoot dies, so A−4 can over-penalize ordinary frost in a permanent-HP, no-special-recovery system. MO Sugarcane `DankPyon_Plant_Sugarcane` retains original **A+2/D90**, comparison **A−3/D24** remains **not adopted**; [LSU AgCenter](https://www.lsuagcenter.com/portals/communications/publications/agmag/archive/2001/fall/resistance-of-louisiana-sugarcane-to-deterioration-from-freezing-temperatures) distinguishes terminal-leaf/bud brown-out, stalk injury and total aboveground commercial cane failure over multiple actual freeze events. See [source-first numeric review](Research/ColdDamageV3SourceFirstAudit.md#latest-2026-10-24-a-d-authority-and-lentilsugarcane-residual-uncertainty) for additional field-tradeoff comparisons. CCTO retains generic MO sugarcane support even though AMJ Agriculture separately excludes normal Japanese mainland cultivation; do not conflate Mod owner scopes.

**Updated 78-row candidate status:** **6 `APPROVED_A_D_CANDIDATE_VARIATION_PENDING`, 7 `HOLD_SOURCE_STAGE_ENDPOINT_UNRESOLVED` (including Lentil/Sugarcane), 35 `UNAPPROVED_HP_TRIAL`, 30 `KEEP_EXISTING_DORMANCY`**. These are status changes and two candidate onset values only; candidate D and individual w unchanged, C# implementation and production XML untouched. No native-game HP/tick/harvest/save-load verification or release authorization.

## V3 pumpkin D21.5 sampler correction — latest author-selected A/D candidate (2026-10-24)

**Supersedes the 2026-10-23 Pumpkin A0/D22 numerical candidate, not the framework, and not any production XML.** The author selected MO Pumpkin `DankPyon_Plant_Pumpkins` injury onset **A=0°C** and cumulative full-HP lethal dose **D=21.5°C·game-hour** as the current V3 **candidate numeric fields only**. The previous D22 references in dated experiments are historical comparisons; `Docs/ColdDamageHpV3Candidates.csv` is the current numeric authority. The other five author-selected crop candidate tuples stay Tomato A0/D21.5, Cabbage A−3.5/D240, Onion A−5/D120, Carrot A−7/D170, and Cotton A0/D48.

**Separate quantitative reason:** Under an ideal steady −3°C for eight game-hours, the current provisional `Plant.CheckMakeLeafless` cold sampler (every 2,000 ticks, first cold sample starts observation without back-charging) charges **7.2 game-hours**, i.e. **21.6°C·h** instead of the continuous ideal **24°C·h**. Against original MO 1.6.2.2 100-MaxHP Pumpkin, at the actual framework default `w=0` and under these theoretical nominal callbacks, **D22 leaves 2HP alive** (conditional remaining harvest multiplier ≈51% of otherwise ready crop) while **D21.5 depletes HP and kills**. A milder −1°C×8h case changes end HP from 68 to 67; repeated 2h nights may still be missed or vary by callback phase. This is a bounded gameplay/engineering choice, **not** a lab-derived species-wide mortality dose or guarantee of behavior on actual ambient-temperature curves. Do not fit any other species merely to this sampler artifact.

**Authorization and implementation boundary:** Six selected A/D candidates remain six; seven source/stage/endpoint HOLD, 35 unapproved HP trials and 30 current dormancy unchanged. The draft CSV's per-plant `lethal_dose_variation=0.20` remains **unapproved**, C# default **w=0**. No HP opt-in Vanilla/MO production XML, HP runtime C# behavior, old save handling, 1.6/1.7 game acceptance or publication is approved by this selection. Keep existing 2,000-tick sampling as the provisional simple model and compare actual normal-tick map frost, HP, harvest and save/load together only at consolidated acceptance. Current comparative evidence: [ColdDamageHpV3Assessment](ColdDamageHpV3Assessment.md#six-selected-a-d-candidates-discrete-sampler-viability-review-2026-10-24).

## V3 six-species lethal-dose variation comparison boundary (2026-10-24)

**Review classification only; no individual width is author-selected.** The experimental `lethal_dose_variation=0.20` fields in `Docs/ColdDamageHpV3Candidates.csv` remain **unapproved** for all 48 prospective HP species; the functional C# default remains `w=0`. Stable per-plant Thing-ID variation changes **D only**, not temperature onset A, stage or acclimation, and is not quantitatively derived from published genotype-level LT50 data. An already-changing map ambient temperature, protective building and ordinary plant HP can cause different outcomes even at w=0.

Following the 2026-10-24 author-selected **Pumpkin A0/D21.5** candidate correction, [the six-species discrete-sampler width audit](ColdDamageHpV3Assessment.md#six-species-individual-lethal-dose-variation-role-by-role-review-2026-10-24) narrows **comparisons, not selections**:

| Author-selected V3 A/D candidate | Restricted width sensitivity to compare | Present review outcome |
|---|---|---|
| MO Tomato 0 / 21.5 | w=0 baseline; nonzero width deferred | Nominal −3°C×8h: w=.05 alone creates ≈45% survivors, conflicting with older ≤30% severe-frost preference |
| MO Pumpkin 0 / 21.5 | w=0 baseline; nonzero width deferred | Same sampled near-lethal boundary; D21.5 was selected for w=0 and not jointly calibrated with w>0 |
| MO Cabbage −3.5 / 240 | w=0 versus **w=.10** for sensitivity only | −8°C×48h: both all survive; w=.20 kills ≈21% of hypothetical plants, contrary to survival-first priority |
| MO Onion −5 / 120 | w=0 versus **w=.05** for sensitivity only | −10°C×24h: w0 survives at 4HP, w=.05 kills ≈17% |
| MO Carrot −7 / 170 | w=0 pending meaningful individual endpoint | −12°C×24h: all modeled comparison widths still survive, so no convincing gameplay gain |
| Vanilla Cotton 0 / 48 | w=0 versus **w=.05** for sensitivity only | −2°C×24h: w0 survives, w=.05 kills ≈17%; original loaded Cotton HP remains unverified |

**Practical numerical gate:** The accepted six A/D candidate tuples can all be kept unchanged under the existing **w=0** code default. Optional w is already implemented, so no new C# mechanism is necessary to preserve future same-species variability. In the representative −3°C×8h sampled event, Tomato/Pumpkin A0/D21.5 give 0% projected survivors at w0 but ≈26.7% at w=.01 and ≈45.3% at w=.05; the seemingly conservative ±1% is smaller than the dose bias from omitting the first 0.8 game-hour. A wider spread **cannot be added as a transparent improvement while also treating previously selected A/D outcomes as unchanged**. Retuning D for each width from that single event would be overfitting, not laboratory calibration. Therefore **do not block current A/D review or build a new system to fit artificial cohort survival**: use w=0 as the V3 provisional review/implementation baseline, and leave nonzero species widths open until a player-visible benefit and independently checked short/multi-night/severe outcomes warrant an explicit author decision. This preserves the historical interest in within-species differences without fabricating a measured spread.

**Hold the scope:** No w values are selected for these six or the other 42 candidate HP species; none is ready for release XML from this comparison. Thirty dormancy species retain their existing model. No engine, XML, save format, optional addon parity, native test or publication change follows from this design review. Width may only be proposed after showing a distinct player-visible benefit without violating the selected frost survival/harvest intent; do not fit width to a single nominal temperature event and call that an empirical coefficient.

## V3 seven HOLD crops: precise sampler-review boundary (2026-10-24)

**Not an A/D or species-role selection.** The seven `HOLD_SOURCE_STAGE_ENDPOINT_UNRESOLVED` rows (Rice, Wheat, Corn, Potato, Strawberry, Lentil, Sugarcane) remain held. The original `A/D/w` CSV values are unchanged and **must not be copied into HP opt-in XML**. [The source-first audit](Research/ColdDamageV3SourceFirstAudit.md#seven-hold-species-exact-sparse-sampler-falsification-before-ad-selection-2026-10-24) now checks the unchanged 2,000-tick `CheckMakeLeafless` model against short/multi-night/extended frost; ideal 8h duration usually results in only **7.2h charged** due to first-cold observation. Most important: the unapproved MO Sugarcane candidate A−3/D24 is *not* fatal at −6°C×8h under nominal sampling (**90% HP loss**, not historical ideal **100% fatal**), while A−3/D36 predicts 60%. Do not select a new D (e.g. 21.5) merely to force the discrete fatality cutoff. Sugarcane A+2/D90 also wrongly injures productive stalk at 0°C under current representative-stalk interpretation. Existing sugarcane XML and AMJ Agriculture's separate crop exclusion are unaffected.

**Interpretation preference for further author evaluation, not adoption:** Rice = established field stand (not unconditioned laboratory seedling), Wheat = ordinary partially winter-hardy field crop (not permanently full-acclimated genotype), Corn = standing reproductive crop (not one emerged-seedling assay), Potato = tuber harvesting and viability (not frost-killed tops), Strawberry = perennial crown survival (not flower-set injury), Lentil = regrowing lower-node crop (not imbibed seed LT50), Sugarcane = productive harvestable stalk (not underground ratoon). Preserve stage/organ uncertainty, w=0 as framework comparison baseline and current A/D HOLD status until bounded numerical choices are authorized. No extra organ/stage/healing subsystem is justified by this comparison alone.

## V3 sugarcane daily frost minimum versus harvest semantics (2026-10-24)

**Review finding, not adoption:** MO Sugarcane `DankPyon_Plant_Sugarcane` represents **harvestable aboveground stalk**, not underground ratoon survival; original MO 1.6 source explicitly declares **100 MaxHP / 11 harvestYield / 5.8 growDays**. LSU's 2015 minimum-temperature damage bands assume an **8–12-hour freezing episode**, *not eight hours continuously at its absolute minimum*. Current 2,000-tick HP model has marked variability between constant minima and gradual overnight cooling; [bounded calculations and source limitations](Research/ColdDamageV3SourceFirstAudit.md#sugarcane-a3--d24-vs-d36-minimum-temperature-duration-and-harvest-semantics-2026-10-24) are authoritative for this comparison.

At proposed **A−3**, a single **−6°C constant for 8h** still leaves **10HP with D24** and **40HP with D36**, while three hypothetical gradual 0→−6→0°C frost nights leave **28–40HP with D24** and **52–60HP with D36**. Three nights each including **two hours sustained at −6°C** kill the D24 crop but leave **14–24HP** with D36. Compromised stalk tissues may still be harvested soon after freezing, before delayed biological quality decline; visible ordinary HP plus existing partial harvest better represents this than requiring every single −6°C minimum to instantly destroy all harvest.

**Reviewer preference: retain A−3/D24 as the *stronger next author-choice comparison***, D36 as a softer comparison/fallback, **NOT a selected A/D candidate**. Do not lower D just to reproduce one LSU categorical minimum temperature as a literal whole-Plant death. The authoritative candidate CSV still holds **A+2/D90, HOLD**; its w=.20 is unapproved, framework default w=0. Six previously author-selected A/D tuples, other six HOLD profiles, 35 unapproved HP trials and 30 dormancy rows unchanged. No C#/XML/CI/runtime/main adoption is authorized here.

## V3 MO sugarcane A/D candidate selection — latest 2026-10-24 authority

**LATEST USER A/D CANDIDATE SELECTION (supersedes the earlier source-stage HOLD, A+2/D90 trial, D24/D36 review-only text above):** For generic MO `DankPyon_Plant_Sugarcane` select low-temperature HP onset **A−3°C** and full-MaxHP cold dose **D24°C·game-hour**. In the candidate CSV its status is now `APPROVED_A_D_CANDIDATE_VARIATION_PENDING`; this author decision resolves the representative-endpoint review *for choosing A/D only*. The chosen single ordinary `Plant.HitPoints` represents the continued harvestability of its **aboveground commercial cane stalk**, *not* underground ratoon survival, leaf/bud discoloration alone, actual extracted sugar quality, bacterial decomposition or the post-frost storage/harvesting deadline. This is a bounded gameplay proxy, **not a laboratory-calibrated living whole-stalk LD100 coefficient**.

**Numerical reasons and known accepted approximation:** 2,000-tick cold samples do **not** charge the first observed 0.8 game-hour of an episode; at continuous −6°C for eight game-hours, current hypothetical sampled C# with w=0 gives **90% HP loss, 10HP alive** with A−3/D24 versus **60% loss, 40HP alive** for unselected D36. Three gradual overnight 0→−6→0°C cycles give ~**28–40HP alive** for selected D24 vs **52–60HP** under D36. Three nightly 0→−6°C (two-hour plateau)→0°C events kill D24 stalks while leaving D36 stalks at ~14–24HP. **Single minimum temperature is NOT identical to eight continuous hours at that minimum**; LSU source documents tissue injury and subsequent quality loss, not a universal in-game fatal dose. The selected D24 intentionally does **not** force every single night reaching −6°C to destroy all immediately harvestable stalks. Earlier ideal continuous-time `−6°C×8h = 24°C·h` crop-death wording is superseded by this actual sampled calculation. See [source-first detailed comparison](Research/ColdDamageV3SourceFirstAudit.md#sugarcane-a3--d24-vs-d36-minimum-temperature-duration-and-harvest-semantics-2026-10-24).

**Limits/status after this author choice:** There are now **7 selected A/D candidate rows**, **6 HOLD** (Rice, Potato, Corn, Strawberry, Wheat, Lentil), **35 unapproved HP trials** and **30 existing dormancy** out of 78. Other selected A/D tuples (Tomato A0/D21.5, Pumpkin A0/D21.5, Cabbage A−3.5/D240, Onion A−5/D120, Carrot A−7/D170, Cotton A0/D48) stay unchanged. The CSV `lethal_dose_variation=0.20` for sugarcane and all other species is **still unapproved**, C# defaults to w=0. No consent for C#/XML production activation, a per-plant width, additional temperature physiology, broad crop rebalancing, native game PASS or Steam release. CCTO continues supporting generic MO sugarcane; AMJ Agriculture's separate Japanese-mainland exclusion does not change this base CCTO authority.

## V3 potato / cultivated strawberry actual harvest-lifecycle correction (2026-10-24)

**Important mechanical correction:** Vanilla **`Plant_Strawberry` is a sowable *one-harvest crop*, destroyed on harvesting and resown** ([RimWorld Wiki strawberry plant](https://rimworldwiki.com/wiki/Strawberry_plant)). Earlier 2026-10-24 reviewer wording treating this specific crop as an in-game perpetually fruiting **overwintering perennial crown** must not be followed as a required V3 gameplay outcome. Real strawberry crowns *can* survive winter, but that is separate physiology; neither a new repeat-harvest system nor a flower-health subsystem is approved. In a simple ordinary-HP V3, assess strawberry **the living productive crop and reduced final berry yield under frost**, not a destroyed-upon-harvest crown. The existing **A−12/D500** HOLD trial ignores all fruit/flower frost at −2 to −10°C and is **not** numerically validated simply because hardier dormant biological crowns exist.

[The relevant comparison](Research/ColdDamageV3SourceFirstAudit.md#potato-and-vanilla-strawberry-tuberone-harvest-crop-endpoint-2026-10-24) shows at w=0 and nominal 2,000-tick sampling: Potato A−3/D100 retains 67/85HP and ~89.4% conditional harvest after three warm-separated −4°C×8h nights; Potato A−2/D100 instead retains 49/85HP and ~78.8%, a genuine tuber-yield-versus-top-injury tradeoff. Strawberry A−12/D500 suffers no loss even after −5°C×8h for three nights; hypothetical A−3/D240 instead retains 70/85HP (~91.2% harvest), and A−2/D120 retains 40/85HP (~73.5% harvest). In an entire −10°C×24h event, A−3/D240 survives 28/85HP while A−2/D120 kills the crop: the latter may over-penalize genuine sheltered crown viability, but it is *one-harvest sowable Plant* behavior, not an independently measured crown-death prediction.

**No coefficient selection:** Leave both CSV rows `HOLD_SOURCE_STAGE_ENDPOINT_UNRESOLVED`: Potato A−3/D100, Strawberry A−12/D500; variation 0.20 in CSV remains UNAPPROVED; C# default w=0. **Reviewer comparison options only:** Potato A−2/D100 vs old A−3/D100 (A−1/D100 a stronger leaf-sensitive bound); Strawberry A−3/D240 vs A−2/D120 (A−1/D120 a more frost-sensitive bound). Neither means lab-calibrated fatality or actual in-game harvest output; rely on loaded original 1.6/1.7 evidence at the final consolidated stage. Preserve all previously author-selected seven A/D species, other four HOLD, 35 unapproved trials and 30 dormancy Defs; no new C#/XML behavior, biological organ subsystem or Steam release.

## V3 potato and strawberry growing-window acceptance screen (2026-10-24)

**Latest review (not author A/D selection):** The existing original-game 1.6 **world-tile daily low/high** archive is reused in a transparent *hypothetical* 06:00–19:00 outdoor crop-growth plus exact 2,000-tick HP-sampler model; not a weather-event, real sowing window, incident, or loaded map-native gameplay verification. Original Plant `growDays` 5.8 potato/4.6 cultivated strawberry is **~10.71/8.49 actual days on ordinary soil in ideal temperatures**, not 5.8/4.6 calendar days. Details, provenance and assumptions: [source-first seasonal window gate](Research/ColdDamageV3SourceFirstAudit.md#potato-and-strawberry-actual-climate-window-review-before-author-ad-selection-2026-10-24).

- **Potato**: A−2/D100 remains a **recommended comparison for author numerical selection**, not automatically approved; all tested early-season (day 10) grow windows in sampled temperate/boreal weather permit harvest. In colder BorealForest 44000, a day-10 sowing is estimated to yield ~83–94% if harvested promptly. Very late day-14 Boreal 40777/44000 potato crop failure occurs with **both** original A−3/D100 and comparative A−2/D100, so do not attribute it uniquely to the revised onset. Top injury and protected tuber survival remain biologically different endpoints.
- **Cultivated Strawberry**: A−2/D120 is **not yet numerically safe to adopt** merely from its single-night frost behavior. In the coldest sampled BorealForest 44000, one day-14 planted crop at near maturity dies in **60/80 theoretical observation phases**, while A−2/D240 matures and remains harvestable under **80/80** phases at an estimated **58–77%** full-health harvest factor. A−3/D240 is another previously compared gentler onset. Higher D also protects against extended severe freeze, so it is **not** a source-measured LD100 or automatic preferable number. Capturing flowers/berry yield loss and keeping a crown alive cannot be both faithfully modeled using one ordinary HP number; the sowable Vanilla crop is removed when harvested.

**Current authority unchanged:** CSV `Plant_Potato` A−3/D100 and `Plant_Strawberry` A−12/D500 remain `HOLD_SOURCE_STAGE_ENDPOINT_UNRESOLVED`. Reviewer choices A−2/D100 (potato), A−2/D120 versus A−2/D240 (strawberry) are **not approved numbers**. The other 7 selected A/D rows, four remaining source HOLD rows, 35 unapproved HP trials, 30 retained dormancy, optional but unapproved individual variation and C# default w=0 remain. No existing production XML, C#, save behavior, main merge, runtime PASS, UI or Steam publication change.

## V3 potato author A/D selection and strawberry stronger-frost gate (2026-10-24)

**Current selection:** `Plant_Potato` **A−2°C/D100°C·game-hour**, selected *A/D candidate only*, supersedes its earlier held A−3/D100. Ordinary visible HP is a tuber-bearing crop viability/harvest proxy, not independent exposed-leaf and buried-tuber injury. In reviewed 2,000-tick w=0 scenarios on 85HP, three −4°C×8h nights leave **49HP and 78.8% conditional yield**, versus 67HP/89.4% under A−3. Original 1.6 world-tile climate approximation still permits early sowing (day10) and harvest in all three reviewed regions, with late day14 Boreal crop death under **both** onset options. D100 is an accepted gameplay candidate, not a laboratory LD100.

**Strawberry remains unselected:** The sowable one-harvest `Plant_Strawberry` is destroyed after harvest; spring flower/berry frost sensitivity cannot be replaced by indefinitely dormant perennial-crown survival. Compare reviewer-only **A−2/D120 vs A−2/D240**, with exact 2,000-tick sampling (constant hypothetical air, first cold sample not charged, default w=0, 85HP):

| Frost | D120: HP / relative harvest if living | D240: HP / relative harvest if living |
|---|---|---|
| −3°C×8h | 80 / 97.1% | 83 / 98.8% |
| −5°C×8h | 70 / 91.2% | 78 / 95.9% |
| −5°C×8h for three warm-separated nights | 40 / 73.5% | 63 / 87.1% |
| −10°C×12h | 22 / 62.9% | 54 / 81.8% |
| −10°C×24h | dead / 0% | 20 / 61.8% |
| −12°C×24h | dead / 0% | 3 / 51.8% |

D240 is the **less punitive comparison**, preserving maturity in 80/80 synthetic sampling phases for the marginal Boreal #44000 day14 sowing where D120 fails before maturity in 60/80. But D240 may **understate flowering frost loss**, and plant HP harvest scaling cannot separately kill blossoms; neither dose is calibrated or approved. The original `Plant_Strawberry` A−12/D500 remains HOLD until a distinct numerical choice. Day14 source data are *world-tile daily extrema with assumed sinusoidal daily profile*, not a real planted map, growing-season sow permission, or native runtime acceptance.

**Current 78 species:** 8 A/D candidates selected (previous seven plus Potato), 5 source/stage HOLD (Rice/Corn/Strawberry/Wheat/Lentil), 35 unapproved HP trial, 30 preexisting dormancy. All w=.20 CSV entries unapproved; C# default w=0. No new C#/XML opt-in, test job, main merge or publication. Historical older counts above are dated, not competing authority.
