# CCTO V3 fictional and real-analogue plant source/biome audit (2026-10-25)

## Scope and authorization

**This is a source-and-gameplay screening, NOT numerical A/D author approval, production XML adoption or native gameplay PASS.** Owner candidate source: [ColdDamageHpV3Candidates.csv](../ColdDamageHpV3Candidates.csv) on feature Draft PR #31. Exactly **35** remaining `UNAPPROVED_HP_TRIAL` rows after 13 prior crop A/D selections; 30 separately retained dormancy defs **unchanged**.

Use direct plant biology only for an identifiable real-world organism and matching organ/stage. For fictional species, prioritize the actual original-game/MO plant description and declared ecology, then a *clearly labeled* real growth-form analogue, and test ordinary season, repeated cold and extreme events with the existing 2,000-tick visible-HP rules. Do **not** infer scientific precision from two decimal places, give a made-up measured LD100, or import nonfreezing growth inhibition as irreversible whole-Plant HP injury. A natural wild plant should plausibly persist or reappear in its published spawn biomes. Growing forms or common names that map to multiple real species need a *generic* analogue, not false one-to-one taxonomy. CCTO remains generic and does not exclude Vanilla/MO fictional plants based on AMJ history.

Complete count: **11 fictional game species/Defs** (including paired cultivated/wild), **1 stylized oversized conifer** (MO Great fir), **4 generic real growth forms**, **19 actual taxon/common-name analogues** = 35. "Fictional" counts *Defs*, not biologically unique species. The remaining real/genus/common-name group includes the actual genus Rafflesia and the Drago/dragon tree; their stylized in-game attributes do not make these names wholly invented.

## Unapproved fictional and stylized crops — original A/D preserved

Calculated column is illustrative **HP percentage remaining** for a constant example temperature lasting 8 game-hours per episode, once and on three **warm-separated** nights, assuming nominal 7.2h billable per episode because the initial cold reading is not charged (2,000 ticks/sample, 2,500 ticks/game-hour, w=0). This ignores exact HP integer rounding, actual map temperature and sow/growth; **0% means modelled death, not experimental mortality**. It is deliberately an independent impact check of the **existing unapproved** A/D inputs, not self-validation of a formula-derived D.

| Def | Class | Trial A °C / D °C·h | Illustrative frost | HP after 1 ×8h | HP after 3 ×8h | Specific acceptance concern |
|---|---|---:|---:|---:|---:|---|
| `Plant_Devilstrand` | Fictional | 2/70 | -1°C | 69.1% | 7.4% | Very long ~41.5d normal-soil grow: repeated mild frost nearly wipes out valuable crop; seasonal viability first |
| `Plant_Healroot` | Fictional | -10/200 | -15°C | 82.0% | 46.0% | Long-lived harvestable medicine: avoid equating leaf cold damage with root death |
| `Plant_Smokeleaf` | Fictional | -4/100 | -6°C | 85.6% | 56.8% | Cannabis-like but invented drug plant: avoid claiming cannabis experimental thresholds |
| `Plant_Psychoid` | Fictional | 2/50 | -1°C | 56.8% | dead | Fictional psychite crop: dies after three marginal frost nights at this trial; evaluate economy vs warm-crop identity |
| `Plant_Tinctoria` | Fictional | -4/110 | -6°C | 86.9% | 60.7% | Engineered dye crop: fast growth and low light requirement, not tropical by name alone |
| `Plant_Ambrosia` | Fictional | -10/240 | -20°C | 70.0% | 10.0% | Rare event-only wild crop, naturally appears also in boreal forest; check sprout season and existing growth before winter loss |
| `Bryolux` | Fictional | -14/450 | -20°C | 90.4% | 71.2% | Cave moss: actual cell and roof temperatures, not unroofed world-tile annual low; preserve natural cave population |
| `Glowstool` | Fictional | -10/280 | -20°C | 74.3% | 22.9% | Cave fungus: natural cave viability and harvest; compare Bryolux without assumed same cold mechanism |
| `Plant_HealrootWild` | Fictional | -10/200 | -15°C | 82.0% | 46.0% | Wild/cultivated same-species A/D must remain consistent |
| `DankPyon_Plant_Mindwort` | Fictional | -3/110 | -6°C | 80.4% | 41.1% | MO 1.6 thin delicate alchemy herb; growDays 7, parent HealrootBase is not biological equivalence |
| `DankPyon_Plant_MindwortWild` | Fictional | -3/110 | -6°C | 80.4% | 41.1% | MO wild counterpart, growDays 10: same-species A/D and natural spawn survival |
| `DankPyon_GreatFir` | Stylized conifer | -40/5000 | -45°C | 99.3% | 97.8% | MO fictional ~300-foot evergreen fir near snowy forests; cold-conifer/pine analogue, not measured giant-tree LT50 |

**Important distinctions:**
- Devilstrand is genetically engineered for high-value fibers and has **22.5 growDays / ~41.5 ideal normal-soil calendar days**; a modelled frost-loss that is appropriate for an ordinary fast plant can be catastrophic late in this crop's season. The live game's Devilstrand temperature/growing behavior and the speculative V3 onset must not be conflated. [RimWorld Wiki Devilstrand](https://rimworldwiki.com/wiki/Devilstrand_mushroom)
- Vanilla Psychoid **9 growDays**, Smokeleaf **7.5**, Ambrosia bush **6** but not player-sowable; Ambrosia can arise in **boreal forest** via a special sprout. [Psychoid](https://rimworldwiki.com/wiki/Psychoid_plant), [Smokeleaf](https://rimworldwiki.com/wiki/Smokeleaf_plant), [Ambrosia bush](https://rimworldwiki.com/wiki/Ambrosia_bush).
- Bryolux and Glowstool are **cave fungi/moss** that die from *light*, not inferred from a tropical outdoor home biome. Assess cave **cell** temperatures, roof and cave-openings, and whether wild stocks are sustainable. Do not apply outside tile minimum as if it were actual cave temperature. [Bryolux](https://rimworldwiki.com/wiki/Bryolux), [Glowstool](https://rimworldwiki.com/wiki/Glowstool).
- MO Mindwort's **original 1.6 Def** describes a delicate herb, `growDays=7` in `3219596926/1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Alchemy.xml`, and wild Mindwort has `growDays=10` in `Plants_Wild_Alchemy.xml`. Both share A−3/D110 in the V3 proposal; this is a deliberate same-identity trial, **not** experimental approval. The parent `HealrootBase` is inheritance, not a biological guarantee.
- MO Great fir's original 1.6 `Plants_Wild_DarkForest.xml` describes an evergreen giant in snowy forest, 250 maxHP, 18 growDays. This is a fantasy-sized *real conifer analogue*. Its **A−40/D5000** matches the candidate pine analogue, and existing actual-world TileTemperaturesComp cold Boreal forest #44000 daily minimum (~−37.8°C) is **warmer** than A−40, so that source calendar cannot meaningfully measure its injury rate. Separate biome-survival adequacy from unrealistic plant invulnerability; no automatic new frost challenge is required.

## Real species/common names and generic forms — distinct evidence route

These remaining **23** rows are *not* to be assigned fictional-species freeform parameters merely because RimWorld uses altered artwork or simplified lore. Real plant class uses documented habitat/organ/season first; broad labels like Herb/Mushroom/Haygrass/Low shrubs use an explicit growth-form/habitat proxy with species ambiguity shown.

| Def | Classification | Unapproved A/D |
|---|---|---:|
| `Plant_Haygrass` | Real-world generic form, no single species | -12/220 |
| `Plant_TreeCocoa` | Real-world genus/species/common-name proxy (species may be ambiguous) | 5/180 |
| `Plant_TreeBamboo` | Real-world genus/species/common-name proxy (species may be ambiguous) | -20/1400 |
| `Plant_TreeCecropia` | Real-world genus/species/common-name proxy (species may be ambiguous) | 4/160 |
| `Plant_TreeDrago` | Real-world genus/species/common-name proxy (species may be ambiguous) | 2/240 |
| `Plant_TreePalm` | Real-world genus/species/common-name proxy (species may be ambiguous) | 4/200 |
| `Plant_TreePine` | Real-world genus/species/common-name proxy (species may be ambiguous) | -40/5000 |
| `Plant_SaguaroCactus` | Real-world genus/species/common-name proxy (species may be ambiguous) | -7/700 |
| `Plant_TreeTeak` | Real-world genus/species/common-name proxy (species may be ambiguous) | 6/220 |
| `Plant_Agave` | Real-world genus/species/common-name proxy (species may be ambiguous) | -8/550 |
| `Plant_Alocasia` | Real-world genus/species/common-name proxy (species may be ambiguous) | 3/120 |
| `Plant_Clivia` | Real-world genus/species/common-name proxy (species may be ambiguous) | -3/180 |
| `Plant_Rafflesia` | Real-world genus/species/common-name proxy (species may be ambiguous) | 7/100 |
| `Plant_ShrubLow` | Real-world generic form, no single species | -3/240 |
| `Plant_PincushionCactus` | Real-world genus/species/common-name proxy (species may be ambiguous) | -10/750 |
| `DankPyon_Plant_Mushrooms` | Real-world generic form, no single species | 0/90 |
| `DankPyon_Plant_Flax` | Real-world genus/species/common-name proxy (species may be ambiguous) | -5/140 |
| `DankPyon_Plant_Herb` | Real-world generic form, no single species | -4/110 |
| `DankPyon_Tree_Lemon` | Real-world genus/species/common-name proxy (species may be ambiguous) | -5/650 |
| `DankPyon_Plant_Poppy` | Real-world genus/species/common-name proxy (species may be ambiguous) | -6/150 |
| `DankPyon_Plant_Fleawort` | Real-world genus/species/common-name proxy (species may be ambiguous) | -7/170 |
| `DankPyon_Plant_PoppyWild` | Real-world genus/species/common-name proxy (species may be ambiguous) | -6/150 |
| `DankPyon_Plant_FleawortWild` | Real-world genus/species/common-name proxy (species may be ambiguous) | -7/170 |

## Screening outcome and next decisions

**No approval and no edit to any of the 35 A/D CSV rows.** Priority discrepancies worth further selection review are (1) Devilstrand long-season survival under repeated mild freeze at A+2/D70, (2) Psychoid survival at A+2/D50 under three −1°C nights, (3) rare cold-biome Ambrosia sprouts surviving until useful harvest, and (4) Bryolux/Glowstool cave-temperature ecology. Mindwort/wild Mindwort and Healroot/wild Healroot must retain paired same-species parameters unless the original Def justifies a stage distinction. Cold-hardy fantastic Great Fir should be benchmarked against existing real Pine *as a proxy* and natural biome, not assigned invented physiological LD100. Do not redesign the fixed 2,000-tick HP engine or migrate the 30 established dormancy species because of this screening.

**Current candidate state unchanged:** 13 selected A/D, 35 `UNAPPROVED_HP_TRIAL`, 30 `KEEP_EXISTING_DORMANCY`, 0 source/endpoint HOLD. `lethal_dose_variation=0.20` remains unapproved in every CSV row; runtime w=0; no new native game tests, active XML, C# edits or Steam action.
