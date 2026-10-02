# RimWorld Crop Cold Tolerance Overhaul — Design

## 1. Scope

This mod rebalances only cold tolerance for sowable crops.

It is responsible for:

- minimum growth temperature (`minGrowthTemperature`)
- crop-specific low-temperature death thresholds
- displaying low-temperature death behavior in the plant information card
- special low-temperature behavior such as dormancy where appropriate

It does **not** rebalance:

- harvest yield
- growth days
- fertility requirements or fertility sensitivity
- storage life
- processing
- research progression

The balance philosophy follows Ancient & Medieval Japan (AMJ): real-world cold tolerance is used as evidence, but final values are chosen so that crops have clear gameplay identities and meaningful climate/season tradeoffs. Values defined here are intended to be usable by AMJ without a second, conflicting balance layer.

## 2. Temperature model

Growth stopping and plant death are separate concepts.

- **Minimum growth temperature**: below this temperature the crop stops growing.
- **Low-temperature death threshold**: each supported crop has one fixed species-level threshold. Below this temperature the crop may die.
- The initial release does **not** randomize the death threshold per plant. All plants of the same Def use the same configured threshold.
- Where appropriate, a crop enters **cold dormancy** instead of dying.
- Range-based values used during balancing are archived in §7. They remain candidate data for a possible future randomized mode.

AMJ crop minimum growth temperatures that have already been decided remain unchanged.

## 2.1 Implementation ownership and dependency direction

CCTO owns the common cold-tolerance mechanism. It should not depend on AMJ.

- Vanilla and Medieval Overhaul crops are patched by CCTO because those are supported external Defs.
- AMJ crops consume the same CCTO extension/API from their own PlantDefs.
- CCTO therefore does not need to hard-code AMJ DefNames.
- This keeps the dependency direction one-way: **AMJ -> CCTO mechanism**, never CCTO -> AMJ.
- Other third-party crop mods can later add compatibility by attaching the same extension without CCTO needing to know their internal DefNames.

For ordinary crops, the extension stores one fixed low-temperature death threshold. For dormancy crops, the extension marks cold behavior as dormancy rather than supplying a lethal threshold.

A dormancy crop should enter its cold/leafless state when temperature falls below its configured minimum growth temperature and should survive that state. This avoids introducing a third temperature axis solely for dormancy.


## 3. Fixed implementation values — AMJ crops

These are the cold-tolerance values intended to be used directly by AMJ.

| Crop | Minimum growth temperature | Fixed death threshold / behavior |
|---|---:|---:|
| Barley | 0°C | -8°C |
| Daikon | 0°C | -5°C |
| Buckwheat | 5°C | -2°C |
| Barnyard millet | 5°C | -4°C |
| Hemp | 5°C | -6°C |
| Kudzu | 5°C | cold dormancy |
| Foxtail millet | 8°C | -4°C |
| Proso millet | 8°C | -3°C |
| Adzuki bean | 8°C | -1°C |
| Soybean | 8°C | -3°C |
| Perilla | 8°C | -1°C |
| Rice | 10°C | -1°C |
| Taro | 10°C | -1°C |

Kudzu is a special case: cold removes or suppresses above-ground growth, but the plant survives through its rootstock and can recover when conditions improve.

## 4. Fixed implementation values — Medieval Overhaul 1.6

### 4.1 Medicinal crops

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

### 4.2 Food and fiber crops

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

### 4.3 Perennial crops

| Crop | Minimum growth temperature | Fixed death threshold / behavior |
|---|---:|---:|
| Grape | 5°C | cold dormancy |
| Apple | 5°C | cold dormancy |
| Mulberry | 5°C | cold dormancy |
| Griffon berry | 5°C | cold dormancy |
| Lemon | 10°C | -4°C |

Grape is changed from ordinary leafless death behavior to cold dormancy. Apple, mulberry, and Griffon berry already have leafless-survival behavior in Medieval Overhaul and retain the same conceptual role. Lemon remains a warm-climate perennial and receives a real low-temperature death threshold rather than unlimited dormancy survival.

## 5. Fixed implementation values — Vanilla

| Crop | Minimum growth temperature | Fixed death threshold / behavior | Role |
|---|---:|---:|---|
| Rice | 10°C | -1°C | warm-season grain; aligned with AMJ rice |
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

## 6. Compatibility principle

Unknown third-party crops are not automatically overwritten.

Explicit compatibility/balance data should be provided for supported crop sets such as Vanilla, Medieval Overhaul, and AMJ crops. Unsupported crops retain their originating behavior unless a compatibility patch is added.

## 7. Archived candidate ranges and future randomized mode

The range-based values used during balancing are intentionally preserved instead of being discarded.

### 7.1 Current release policy

The initial implementation uses **one fixed low-temperature death threshold per crop Def**. This is simpler to understand, easier to balance, and avoids unexpected differences between adjacent plants of the same crop.

### 7.2 Possible post-release alternative

If post-release player feedback strongly favors individual variation, the mod may later switch to or add an option for **per-plant deterministic random death thresholds within the archived crop range**.

In that model:

- each crop Def owns a configured minimum/maximum death-temperature range;
- each individual plant receives a stable threshold derived deterministically from its identity rather than rerolling over time;
- the information card shows the species range rather than the hidden per-plant exact threshold;
- the same saved range data can be reused without redesigning the crop balance from scratch.

The range tables below are therefore not deprecated data. They are both the historical balancing record and candidate configuration for a future randomized mode.

### 7.3 Archived AMJ crop ranges

| Crop | Minimum growth temperature | Candidate death range / behavior |
|---|---:|---:|
| Barley | 0°C | -9 to -7°C |
| Daikon | 0°C | -6 to -4°C |
| Buckwheat | 5°C | -3 to -1°C |
| Barnyard millet | 5°C | -5 to -3°C |
| Hemp | 5°C | -7 to -5°C |
| Kudzu | 5°C | cold dormancy |
| Foxtail millet | 8°C | -4 to -3°C |
| Proso millet | 8°C | -3 to -2°C |
| Adzuki bean | 8°C | -2 to 0°C |
| Soybean | 8°C | -4 to -2°C |
| Perilla | 8°C | -2 to 0°C |
| Rice | 10°C | -1 to 0°C |
| Taro | 10°C | -1 to 0°C |

### 7.4 Archived Medieval Overhaul medicinal crop ranges

| Crop | Minimum growth temperature | Candidate death range / behavior |
|---|---:|---:|
| Healroot | 0°C | -10 to -8°C |
| Mindwort | 5°C | -4 to -2°C |
| Poppy | 3–5°C | -6 to -4°C |
| Fleawort | 3°C | -7 to -5°C |
| Fly agaric | 0°C | cold dormancy |

### 7.5 Archived Medieval Overhaul annual crop ranges

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

### 7.6 Archived Medieval Overhaul perennial crop ranges

| Crop | Minimum growth temperature | Candidate death range / behavior |
|---|---:|---:|
| Grape | 5°C | cold dormancy |
| Apple | 5°C | cold dormancy |
| Mulberry | 5°C | cold dormancy |
| Griffon berry | 5°C | cold dormancy |
| Lemon | 10°C | -5 to -3°C |

### 7.7 Archived Vanilla crop ranges

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
