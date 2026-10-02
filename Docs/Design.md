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
- The death threshold is **not randomized per plant**. All plants of the same Def use the same configured threshold.
- Where appropriate, a crop may enter **cold dormancy** instead of dying.
- Ranges such as `-7 to -5°C` in balance work are candidate/validation bands only. They are not runtime random ranges. Before implementation, each crop is reduced to one fixed death threshold.

AMJ crop minimum growth temperatures that have already been decided remain unchanged.

## 3. Medicinal crops

Medieval Overhaul medicinal crops must not all inherit the same cold tolerance merely because they share `HealrootBase`.

Cold tolerance is assigned per plant so that medicinal crop choice matters, especially once advanced medicine recipes explicitly require particular herbs.

### Agreed roles and candidate death bands

| Crop | Minimum growth temperature | Candidate death band / behavior | Role |
|---|---:|---:|---|
| Healroot | 0°C | -10 to -8°C | baseline cold-hardy medicinal crop |
| Mindwort | 5°C | -4 to -2°C | delicate medicinal herb; relatively frost-sensitive |
| Poppy | 3–5°C | -6 to -4°C | cool-season medicinal crop |
| Fleawort | 3°C | -7 to -5°C | hardy medicinal crop |
| Fly agaric | 0°C | cold dormancy | visible growth stops/dies back while the underlying organism survives winter |

### Design notes

- `Mindwort`, `Poppy`, `Fleawort`, and `Fly agaric` currently inherit from `HealrootBase` in Medieval Overhaul, but this inheritance is not treated as evidence that their cold tolerance should be identical.
- `Fleawort` should be clearly hardier than `Mindwort`.
- `Fly agaric` is a special case: it should not behave like a normal annual crop that dies permanently from ordinary winter cold.
- The information card should show the fixed death threshold for ordinary crops, and dormancy-type behavior explicitly for special crops.

## 4. Medieval Overhaul food, fiber, and perennial crops

These values are balanced using the same AMJ principle: real-world cold behavior establishes the relative ordering, while final thresholds are simplified for clear gameplay roles.

### Annual / harvest-destroying crops

The numeric death ranges below are balance-validation bands. Final runtime values will be one fixed threshold per crop.

| Crop | Minimum growth temperature | Candidate death band / behavior | Role |
|---|---:|---:|---|
| Onion | 5°C | -4 to -2°C | cool-season crop, but not strongly frost-hardy |
| Lentil | 5°C | -5 to -3°C | relatively cold-tolerant pulse |
| Cabbage | 0°C | -7 to -5°C | strongly frost-tolerant vegetable |
| Garlic | 0°C | cold dormancy | overwintering crop |
| Mushroom | 5°C | -2 to 0°C | above-ground fungal crop vulnerable to freezing |
| Wheat | 0°C | -7 to -5°C | cold-climate cereal |
| Flax | 5°C | -6 to -4°C | cool-season fiber crop with good frost tolerance |
| Sugarcane | 10°C | -5 to -4°C | requires warmth for growth but survives light freezing better than tropical growth needs imply |
| Carrot | 0°C | -5 to -3°C | cool-season root crop |
| Herb | 5°C | -4 to -2°C | generic temperate herb |
| Tomato | 10°C | -1 to 0°C | warm-season, frost-sensitive |
| Pumpkin | 10°C | -1 to 0°C | warm-season, frost-sensitive |

The Medieval Overhaul 1.6 cabbage value `minGrowthTemperature=-18°C` is treated as a balance error for this overhaul and is replaced with 0°C. Cold survival and active growth are intentionally kept separate.

### Perennial crops

| Crop | Minimum growth temperature | Candidate death band / behavior |
|---|---:|---|
| Grape | 5°C | cold dormancy |
| Apple | 5°C | cold dormancy |
| Mulberry | 5°C | cold dormancy |
| Griffon berry | 5°C | cold dormancy |
| Lemon | 10°C | death at -5 to -3°C |

Grape is changed from ordinary leafless death behavior to cold dormancy. Apple, mulberry, and Griffon berry already have leafless-survival behavior in Medieval Overhaul and retain the same conceptual role. Lemon is deliberately different: it remains a warm-climate perennial and receives a real low-temperature death threshold rather than unlimited dormancy survival.

## 5. Compatibility principle

Unknown third-party crops are not automatically overwritten.

Explicit compatibility/balance data should be provided for supported crop sets such as Vanilla, Medieval Overhaul, and AMJ crops. Unsupported crops retain their originating behavior unless a compatibility patch is added.
