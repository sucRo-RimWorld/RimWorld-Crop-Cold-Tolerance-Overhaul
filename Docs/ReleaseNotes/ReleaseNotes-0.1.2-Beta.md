# Crop Cold Tolerance Overhaul — 0.1.2 Beta

Target Git tag: `v0.1.2-beta`

## Summary

This update fixes the standard Info Card ordering of CCTO's cold-threshold rows and a Medieval Overhaul wild-plant compatibility issue.

`Cold death temperature` and `Dormancy temperature` now appear in the same position, making plant-to-plant comparison easier.

## Changes

- Aligned the Info Card position of `Cold death temperature` and `Dormancy temperature`.
- Improved display consistency when comparing plants with different Vanilla stat rows.
- Fixed duplicate CCTO cold-tolerance extensions on Medieval Overhaul's four wild alchemy plants.

## Verification

- Standard Info Card visual recheck: **PASS**.
- Full automated integration gate: rerun required after the Medieval Overhaul compatibility fix.

## Compatibility

Required:

- Harmony

Optional:

- Medieval Overhaul 1.6
- Nice Plants Menu
- Dubs Mint Menus

## Notes

Plant balance values and intended cold-tolerance behavior are unchanged from 0.1.1 Beta.

## License

MIT License.

## AI-assisted development

AI was used primarily for code implementation, documentation, research, and test development. Design decisions, balance decisions, and final testing/review remain the author's responsibility.
