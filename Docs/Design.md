# RimWorld Crop Cold Tolerance Overhaul — Design

## Current development decision — fixed cold-death temperature values only (2026-10-26)

**Latest author decision:** Keep the fixed cold-death **mechanism** and release the bounded Pine/Great Fir values below as **0.1.4 Beta**. The change selects only two existing `coldDeathTemperature` numbers in Core / optional MO; the HP injury model remains deferred. No other coefficient, API or crop production behavior is changed.

**Change boundaries:** Keep C# hooks, fixed death semantics, `minGrowthTemperature`, optimum-growth temperature fields, dormancy assignments and behavior, supported 49 Core + 29 MO plant identities, integration/API, crop yield, growDays, and fertility unchanged. Do not copy V3 HP onset `A` or lethal degree-hours `D` into `coldDeathTemperature`; they are incompatible physiological/model endpoints. The V3 HP design table, code and existing Draft PR #60 are retained as **deferred historical/development work**, not the active cutover or an automatic future publication.

**Calibration rule:** All fixed thresholds are preserved by default. The two 0.1.4 changes below follow independent original-game cold-forest evidence and winter-acclimated conifer context; they are approximate species-role choices, not measured full-tree LT100. Further threshold changes require a concrete source or ordinary-biome contradiction, stage/exposure assumptions and expected normal/extreme-climate implications. Do not tune crop economics or increase difficulty for its own sake. Static checks and older-engine PASS do not establish 0.1.4 native acceptance.

**Release identity:** Repository `About/About.xml`, `About/Manifest.xml`, `About/Changelog.txt` and README are prepared for **0.1.4 Beta**, not yet uploaded/verified on Steam. The old `0.2` HP redesign stays deferred. Actual publication and subscribed-package acceptance require separate authorization. [ReleaseChecklist](ReleaseChecklist.md#014-fixed-threshold-adjustment--prepared-not-published).

### 0.1.4 fixed death threshold decision — Boreal pine and Great Fir (2026-10-26)

**Accepted narrow XML adjustment for the 0.1.4 source:** `Plant_TreePine` **−35 → −40°C** and conditional MO `DankPyon_GreatFir` **−35 → −40°C**. This is not yet a verified live Workshop update. Both plants retain `minGrowthTemperature=0°C`, ordinary fixed threshold semantics, no new dormant status, no HP mechanics or C# work. Vanilla and MO remain independent supported profiles. Other 46 fixed-death thresholds and 30 dormancy definitions are unchanged.

**Independent evidence and modeling limits:**

- Original RimWorld 1.6 **world/tile hourly calendar** sampling, [manually accepted original-game run #39776410000](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39776410000), 1,440 hour points per actual tile: pine-eligible cold BorealForest tile #44000 (mean −5.536°C) ranges **−37.835 to +26.670°C**; **11 hourly samples below the existing −35°C death threshold**. The same seed's actual generated-map BorealForest tile #40777 has 483 natural pines and minimum **−32.647°C**. The cold tile was not itself loaded as a map and its eligibility does not prove actual pine spawn there. Weather/normal Tick was **not** simulated for a season. These represent a concrete biome-extirpation **risk**, not a demonstrated live forest die-off.
- [Repo 1992, seasonal Scots pine/spruce frost hardiness](https://agris.fao.org/search/en/providers/122535/records/65dff6d10f3e94b9e5de11cb): sampled frost hardiness from −3.5°C to **below −40°C** across seasons, explicitly depending on winter acclimation and organ/tissue. [US Forest Service, Strimbeck et al. 2007](https://research.fs.usda.gov/treesearch/15616): midwinter temperate-mountain/continental conifer foliage LT50 approximately **−33 to −44°C**, boreal conifer species still more resistant in laboratory tests. These are **tissue cold-injury endpoints, not whole-tree instantaneous death thresholds**. `−40°C` is a conservative, game-readable stand-in for winter-hardened boreal evergreen survival, not a measured species-wide death point.
- MO Great Fir is paired with generic boreal Pine because the original CCTO design used the same `−35°C` threshold and conifer role. **Its actual MO natural-map distribution has not been validated**; do not claim a Great Fir game-world death was observed.

| Representative independent check | 0.1.3: −35°C | 0.1.4: −40°C | Intended outcome |
|---|---|---|---|
| Actual world cold forest sample −37.835°C | Temperature crosses instantaneous death threshold | Remains above death threshold | Avoid possible ordinary cold-forest pine wipeout |
| Median pine-bearing forest minimum −32.647°C | Not lethal | Not lethal | Leave ordinary/less-cold forest unchanged |
| Severe −45°C exposure | Below threshold | Below threshold | Preserve a threat under truly exceptional cold |
| Warm/normal growth or dormancy | Unchanged | Unchanged | No growth/dormancy/processing side effects |

**Approval/verification boundary:** The author selected this narrow fixed-death revision and **0.1.4** version. It is a game approximation rather than a measured biological all-tree lethal limit. Exact loaded-game / natural-season behavior, including actual MO Great Fir natural habitat, has not been established for this change. Targeted source checks and any required later native acceptance are separate; do not promote old 0.1.3 test results to new-number PASS. Public upload remains a distinct author-controlled step.

## Historical HP-model 0.2 plan — deferred (2026-10-26)

The earlier 0.2 release designation was contingent on converting fixed death to cumulative visible Plant HP injury. That redesign and its internal V2/V3 iteration names are **historical/deferred work**; Draft [PR #60](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/60) must not be merged into the **0.1.4 Beta** fixed-threshold release. The active source version is 0.1.4; its Steam distribution is not yet verified. No public 0.2 has shipped.

## Governing objective and verification boundaries — clarified 2026-10-26

**CCTO is a standalone, realism-oriented plant cold-tolerance overhaul, not a crop-production or inter-crop economic-balance mod.** The original V1 purpose continues into V3: represent species-specific growth-temperature limits, cold injury/death and dormancy within a deliberately simplified RimWorld model.

Priority:

1. **Biological evidence first:** distinguish growth limits, optimal growth, chilling/frost injury, exposure time, acclimation, life stage and overwintering. Record what the original source actually measured.
2. **Honest approximation:** a V3 HP injury-onset `A` and cumulative fatal dose `D` are game-model coefficients, **not measured laboratory LD100**. Rounding and simplification require a biological/modeling explanation, not a desire to rescue an economically weak crop.
3. **Ecology and technical sanity:** review ordinary weather, extreme cold and repeated exposures, including normal survival in natural biomes; exclude implausible routine die-offs caused by model error. Technical loaded-Def, HP, UI and save tests prove only their own behavior.
4. **No forced gameplay parity:** seasonal differences may emerge, but each crop is not required to occupy a competitive economic niche. Increased difficulty may result from realistic values, but is not itself a tuning objective.

**Independent profiles:** CCTO supports `Vanilla + CCTO` and optional `MO + CCTO`. Agriculture and other AMJ mods are **not dependencies or CCTO numerical reference profiles**. Observations from `MO + Agriculture + CCTO` remain useful only for the cold-temperature/HP behavior actually measured; altered yields, growDays, fertility, jobs, recipes or crop profitability are neither inputs nor release requirements for CCTO. Do not change CCTO temperature coefficients to compensate for those external crop economics.

**Numerical acceptance:** evidence independent of a derived coefficient plus representative normal, severe, repeated-exposure and biome conditions remain necessary; CSV completeness, formula self-consistency and technical PASS do not prove realism. Reopen a previously finalized value only for a concrete biological/model contradiction, implausible ordinary-climate/ecological outcome or implementation defect—not because Carrot, Onion or Cabbage loses a production ranking. The former seven crop-economy role comparisons are **optional player-context research, not seven mandatory V3 acceptance gates**. Previously accepted original 1.6 bounded technical evidence retains its limited scope; 1.7, natural-season/ecological realism, distributable build and publication remain distinct.

**Cabbage:** the MO 1.6 duplicate optimal-temperature tag is an upstream XML error, but merely correcting it to full-speed 0–25°C is not biologically accepted. A 15–20°C optimum is a research-backed *candidate*, not an approved/implemented value. Decide by plant biology and temperature response, not economic competition. The finalized V3 46 HP + 32 dormancy, `w=0` design values remain unchanged pending contrary relevant evidence.

## Current V3 numerical specification — finalized 2026-10-25

**DEFERRED by the 2026-10-26 fixed-threshold-only decision above.** This section is preserved as the approved *internal HP-model design record*, not the presently selected production model or the active fixed-temperature numeric authority. Existing fixed thresholds in `Patches/Vanilla_ColdTolerance.xml` and `Patches/MedievalOverhaul_ColdTolerance.xml` remain authoritative until evidence-backed changes are accepted.

The author delegated the remaining numerical decisions with “監査→修正→監査を行い、値を確定させて”. The resulting **game-design values are final**, not individually author-measured biological coefficients: [ColdDamageHpV3Candidates.csv](ColdDamageHpV3Candidates.csv) is the single current value table. It contains **46 HP profiles with w=0 and 32 dormancy profiles, zero pending values**. `FINALIZED_DESIGN_VALUES` denotes delegated final HP selection; `FINALIZED_DESIGN_DORMANCY` denotes the two newly selected dormant Healroots; `KEEP_EXISTING_DORMANCY` retains the original 30.

All seventeen previously selected crop A/D pairs are preserved, including the later Haygrass A-8/D220 decision from main 895a750 and owner draft 980ea65. Cultivated and wild Healroot both move from the trial A−10/D200 to existing dormancy without a fixed cold-death threshold. Saguaro changes from A−7/D700 to **A−3°C/D180°C·game-hour**. The other 28 previously unselected HP pairs retain their numbers as explicit gameplay choices; all former proposed `.20` dose widths are replaced by **0**. Drago retains **A+2/D240**; the erroneous RHS H3 interpretation is not used. No new organ, acclimation, injury-healing or variation subsystem is introduced.

For identical full-HP plants with identical histories, w=0 does not require mixed surviving/dead cohorts. The earlier S16/S24 mixed-survival preferences are retired explicitly, rather than reported as passed. Cold damage remains gradual visible HP loss, with severe accumulated frost potentially killing the full exposed cohort. Dormancy does not itself prohibit harvesting in the inspected 1.6 implementation. The fixed-death production XML and DLL have **not** been converted by this numerical decision.

Rationale, original-source distinctions, audit corrections and re-audit results: [final audit and selection record](Research/ColdDamageV3BatchReview.md). All dated candidate/approval-count/Healroot-HOLD passages below are historical where they conflict with this current specification. Numerical selection is complete; actual loaded-game acceptance and distribution remain separate.

作物の季節・気候・土地・用途の参考考察は[日本語資料](CropGameplayRoles-ja.md)を参照。農業経済上の優劣はCCTO温度値の採否・公開条件ではない。

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

Realistic, species-specific low-temperature response is the governing design objective. Plant-level evidence informs values, with clearly disclosed game approximations; game economy and difficulty are not independent numerical targets.

Compared with Vanilla / Medieval Overhaul, some plants may stop growth or suffer injury sooner, while appropriate overwintering species survive through dormancy. This difficulty change is a possible *consequence* of realism, never the goal to maximize. Natural-biome viability and credible temperature response take precedence over guaranteeing each cultivated crop a profitable niche.

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
| Great fir | 0°C | -40°C | evergreen snow-forest giant; aligned with Core pine-class severe-cold survival |
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
| Pine | 0°C | -40°C | evergreen boreal/tundra forestry tree; survives severe cold without leafless dormancy |
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
