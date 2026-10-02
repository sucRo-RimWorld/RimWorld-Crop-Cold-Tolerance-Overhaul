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
- **Low-temperature death threshold**: below this temperature the crop may die.
- Where appropriate, a crop may enter **cold dormancy** instead of dying.

AMJ crop minimum growth temperatures that have already been decided remain unchanged.

## 3. Medicinal crops

Medieval Overhaul medicinal crops must not all inherit the same cold tolerance merely because they share `HealrootBase`.

Cold tolerance is assigned per plant so that medicinal crop choice matters, especially once advanced medicine recipes explicitly require particular herbs.

### Current agreed values

| Crop | Minimum growth temperature | Low-temperature death behavior | Role |
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
- The information card should show dormancy-type behavior explicitly rather than presenting a misleading ordinary death-temperature range.

## 4. Compatibility principle

Unknown third-party crops are not automatically overwritten.

Explicit compatibility/balance data should be provided for supported crop sets such as Vanilla, Medieval Overhaul, and AMJ crops. Unsupported crops retain their originating behavior unless a compatibility patch is added.
