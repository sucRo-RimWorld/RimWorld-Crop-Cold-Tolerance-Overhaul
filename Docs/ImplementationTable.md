# Implementation Def Mapping

This file is the implementation-facing mapping for the current fixed-threshold/dormancy balance.

## Rules

- `minGrowthTemperature` is the minimum temperature for active growth.
- Ordinary plants receive one fixed low-temperature death threshold.
- Dormancy plants do not receive a lethal cold threshold. They enter a survivable cold/leafless state below `minGrowthTemperature`.
- CCTO directly patches supported Vanilla and Medieval Overhaul Defs.
- AMJC-owned crop values and Def mappings belong to AMJC, together with its optional CCTO compatibility XML. This table contains only CCTO-owned support data.

## Core / Vanilla RimWorld 1.6

| DefName | Plant | minGrowthTemperature | Cold behavior | Fixed death threshold |
|---|---|---:|---|---:|
| `Plant_Rice` | Rice | 10°C | death | -1°C |
| `Plant_Potato` | Potato | 5°C | death | -2°C |
| `Plant_Corn` | Corn | 8°C | death | -2°C |
| `Plant_Strawberry` | Strawberry | 5°C | death | -9°C |
| `Plant_Haygrass` | Haygrass | 0°C | death | -9°C |
| `Plant_Cotton` | Cotton | 10°C | death | -1°C |
| `Plant_Devilstrand` | Devilstrand | 8°C | death | -1°C |
| `Plant_Healroot` | Healroot | 0°C | death | -9°C |
| `Plant_Hops` | Hops | 5°C | dormancy | — |
| `Plant_Smokeleaf` | Smokeleaf | 5°C | death | -4°C |
| `Plant_Psychoid` | Psychoid | 8°C | death | -1°C |
| `Plant_TreeCocoa` | Cocoa | 12°C | death | 0°C |
| `Plant_TreeBamboo` | Bamboo | 10°C | death | -18°C |
| `Plant_TreeBirch` | Birch | 5°C | dormancy | — |
| `Plant_TreeCecropia` | Cecropia | 10°C | death | 0°C |
| `Plant_TreeCypress` | Cypress | 5°C | dormancy | — |
| `Plant_Dandelion` | Dandelion | 0°C | dormancy | — |
| `Plant_Daylily` | Daylily | 0°C | dormancy | — |
| `Plant_TreeDrago` | Drago tree | 8°C | death | 0°C |
| `Plant_TreeMaple` | Maple | 5°C | dormancy | — |
| `Plant_TreeOak` | Oak | 5°C | dormancy | — |
| `Plant_TreePalm` | Palm | 10°C | death | 0°C |
| `Plant_TreePine` | Pine | 0°C | death | -35°C |
| `Plant_TreePoplar` | Poplar | 5°C | dormancy | — |
| `Plant_Rose` | Rose | 5°C | dormancy | — |
| `Plant_SaguaroCactus` | Saguaro cactus | 8°C | death | -6°C |
| `Plant_TreeTeak` | Teak | 12°C | death | 3°C |
| `Plant_Timbershroom` | Timbershroom | 0°C | dormancy | — |
| `Plant_Tinctoria` | Tinctoria | 5°C | death | -4°C |
| `Plant_TreeWillow` | Willow | 5°C | dormancy | — |
| `Agarilux` | Agarilux | 0°C | dormancy | — |
| `Plant_Agave` | Agave | 5°C | death | -7°C |
| `Plant_Alocasia` | Alocasia | 10°C | death | -1°C |
| `Plant_Ambrosia` | Ambrosia bush | 0°C | death | -9°C |
| `Plant_Astragalus` | Astragalus | 0°C | dormancy | — |
| `Plant_Berry` | Berry bush | 0°C | dormancy | — |
| `Plant_Brambles` | Brambles | 0°C | dormancy | — |
| `Bryolux` | Bryolux | 0°C | death | -12°C |
| `Plant_Bush` | Bush | 0°C | dormancy | — |
| `Plant_Chokevine` | Chokevine | 0°C | dormancy | — |
| `Plant_Clivia` | Clivia | 8°C | death | -2°C |
| `Plant_Rafflesia` | Giant rafflesia | 12°C | death | 5°C |
| `Glowstool` | Glowstool | 0°C | death | -8°C |
| `Plant_Grass` | Grass | 0°C | dormancy | — |
| `Plant_ShrubLow` | Low shrubs | 8°C | death | -2°C |
| `Plant_Moss` | Moss | 0°C | dormancy | — |
| `Plant_PincushionCactus` | Pincushion cactus | 5°C | death | -8°C |
| `Plant_TallGrass` | Tall grass | 0°C | dormancy | — |
| `Plant_HealrootWild` | Wild healroot | 0°C | death | -9°C |

## Medieval Overhaul 1.6 — cultivated farm crops

The DefNames below were verified against the uploaded Medieval Overhaul 1.6 data.

| DefName | Crop | minGrowthTemperature | Cold behavior | Fixed death threshold |
|---|---|---:|---|---:|
| `DankPyon_Plant_Onions` | Onion | 5°C | death | -3°C |
| `DankPyon_Plant_Lentils` | Lentil | 5°C | death | -4°C |
| `DankPyon_Plant_Cabbages` | Cabbage | 0°C | death | -6°C |
| `DankPyon_Plant_Garlic` | Garlic | 0°C | dormancy | — |
| `DankPyon_Plant_Mushrooms` | Mushroom | 5°C | death | -1°C |
| `DankPyon_Plant_Wheat` | Wheat | 0°C | death | -6°C |
| `DankPyon_Plant_Flax` | Flax | 5°C | death | -5°C |
| `DankPyon_Plant_Sugarcane` | Sugarcane | 10°C | death | -5°C |
| `DankPyon_Plant_Carrots` | Carrot | 0°C | death | -4°C |
| `DankPyon_Plant_Herb` | Herb | 5°C | death | -3°C |
| `DankPyon_Plant_Tomatoes` | Tomato | 10°C | death | -1°C |
| `DankPyon_Plant_Grape` | Grape | 5°C | dormancy | — |
| `DankPyon_Plant_Pumpkins` | Pumpkin | 10°C | death | -1°C |
| `DankPyon_Tree_Apple` | Apple | 5°C | dormancy | — |
| `DankPyon_Tree_Lemon` | Lemon | 10°C | death | -4°C |
| `DankPyon_Tree_Mulberry` | Mulberry | 5°C | dormancy | — |
| `DankPyon_Tree_GriffonBerry` | Griffon berry | 5°C | dormancy | — |

### MO 1.6 notes

- `DankPyon_Plant_Cabbages` currently explicitly defines `minGrowthTemperature=-18`; CCTO replaces it with 0°C.
- `DankPyon_Plant_Grape` currently has `dieIfLeafless=true`; CCTO changes its cold role to survivable dormancy.
- Apple, lemon, mulberry, and Griffon berry currently have `dieIfLeafless=False` in MO. Lemon nevertheless receives a lethal CCTO threshold because it is intentionally a warm-climate tree.
- Most other MO cultivated crops do not explicitly define `minGrowthTemperature` and therefore inherit the game's base value unless another patch changes it.

## Medieval Overhaul 1.6 — medicinal crops

These DefNames were verified against `Plants_Cultivated_Alchemy.xml` in the uploaded 1.6 data.

| DefName | Crop | minGrowthTemperature | Cold behavior | Fixed death threshold |
|---|---|---:|---|---:|
| `DankPyon_Plant_Mindwort` | Mindwort | 5°C | death | -3°C |
| `DankPyon_Plant_Poppy` | Poppy | 5°C | death | -5°C |
| `DankPyon_Plant_Fleawort` | Fleawort | 3°C | death | -6°C |
| `DankPyon_Plant_FlyAgaric` | Fly agaric | 0°C | dormancy | — |
| `DankPyon_Plant_MindwortWild` | Wild mindwort | 5°C | death | -3°C |
| `DankPyon_Plant_PoppyWild` | Wild poppy | 5°C | death | -5°C |
| `DankPyon_Plant_FleawortWild` | Wild fleawort | 3°C | death | -6°C |
| `DankPyon_Plant_FlyAgaricWild` | Wild fly agaric | 0°C | dormancy | — |
| `DankPyon_GreatOak` | Great oak | 5°C | dormancy | — |
| `DankPyon_GreatIter` | Great Iter | 5°C | dormancy | — |
| `DankPyon_GreatFir` | Great fir | 0°C | death | -35°C |
| `DankPyon_GreatWillow` | Great willow | 5°C | dormancy | — |

All four currently inherit `HealrootBase`, but CCTO deliberately assigns them distinct cold behavior.
