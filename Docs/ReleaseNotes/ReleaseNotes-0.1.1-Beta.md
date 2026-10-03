# Crop Cold Tolerance Overhaul — 0.1.1 Beta

Git tag: `v0.1.1-beta`

## Summary

This Beta update expands CCTO from selected crops and player-sowable plants to **all living PlantDefs in the supported Core and Medieval Overhaul sets**.

Cold tolerance is now treated as a property of the plant itself rather than being limited by whether the player can sow it.

## Changes

- Expanded Core coverage to **49 living PlantDefs**.
- Expanded Medieval Overhaul coverage to **29 MO-specific PlantDefs**.
- Added explicit cold behavior for wild-only plants, grasses, shrubs, cave plants, fungi, and wild trees.
- Dead stump/remnant Defs are intentionally excluded because they are not living vegetation.
- Wild plants are balanced to remain compatible with the climates and biomes where they naturally occur, avoiding routine winter die-off in their native environments.
- Wild counterparts of MO alchemy plants now use the same species-level cold model as their cultivated counterparts.
- Added cold behavior for MO Dark Forest trees, including Great Oak, Great Iter, Great Fir, and Great Willow.
- Bamboo now uses a Japanese moso/madake-style baseline: minimum growth temperature 10°C and fixed cold-death temperature -18°C.
- Updated English/Japanese Workshop descriptions and public documentation for the expanded scope.
- Aligned the standard Info Card position of `Cold death temperature` and `Dormancy temperature` so the relevant cold threshold appears in the same place when comparing plants.

## Verification

The expanded balance has passed:

- full automated integration gate: **14/14**;
- loaded-value validation for all **49 Core + 29 MO** targets;
- complete supported-living-PlantDef coverage regression for all **78** curated targets;
- normal-profile wild-plant Info Card smoke checks.

Representative normal-profile checks included:

- Wild Healroot: minimum growth 0°C / fixed death -9°C;
- Berry bush: minimum growth 0°C / dormancy 0°C;
- Bush: minimum growth 0°C / dormancy 0°C;
- Oak: minimum growth 5°C / dormancy 5°C.

## Compatibility

Required:

- Harmony

Optional:

- Medieval Overhaul 1.6
- Nice Plants Menu
- Dubs Mint Menus

## Notes

CCTO still changes only cold-tolerance-related behavior: minimum growth temperature, fixed cold-death temperature, and cold dormancy where appropriate.

It does not rebalance harvest yield, growth time, fertility, storage life, processing, or research.

## License

MIT License.

## AI-assisted development

AI was used primarily for code implementation, documentation, research, and test development. Design decisions, balance decisions, and final testing/review remain the author's responsibility.
