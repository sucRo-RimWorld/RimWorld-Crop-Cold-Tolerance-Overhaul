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
When ambient temperature is below onset, damage accumulates in proportion to
the degrees below onset times the elapsed in-game hours. At unit damage the
plant dies. Optional `coldDamageRecoveryPerHour` reduces prior cold damage
while temperature is above onset. This is a provisional game abstraction, not
an empirically calibrated agronomic threshold.

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
For new damage-model users, the existing draft hardcoded lethal
`damage >= 1` and clamping in `ColdDamageProgress.Advance` must be
reconciled with the chosen distribution at the numerical-model integration
step; no working per-plant variation is claimed yet.

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
