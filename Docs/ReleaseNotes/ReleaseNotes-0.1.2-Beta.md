# Crop Cold Tolerance Overhaul — 0.1.2 Beta

Target Git tag: `v0.1.2-beta`

## Summary

This update fixes the standard Info Card ordering of CCTO's cold-threshold rows.

`Cold death temperature` and `Dormancy temperature` now occupy the same display tier, making plant-to-plant comparison more consistent.

## Changes

- Aligned `Cold death temperature` and `Dormancy temperature` to display priority **4151**.
- Both CCTO cold-threshold rows now appear directly below the minimum growth temperature tier and above Vanilla lifespan/harvest-yield rows.
- Fixed the mismatch that was especially noticeable when comparing wild plants and cultivated plants, because Vanilla exposes different optional plant stats for different Defs.
- Confirmed the issue was caused by plant-specific Vanilla stat rows rather than English/Japanese localization.
- Nice Plants Menu required no change; its CCTO rows were already inserted in a consistent position after growth temperature.
- Added a regression test that locks both CCTO cold-threshold entries to the same priority.

## Verification

- Full automated integration gate after the UI ordering change: **14/14 PASS**.
- Standard Info Card visual recheck: **PASS**.
- Cold-death and dormancy thresholds now use the intended aligned position.

## Compatibility

Required:

- Harmony

Optional:

- Medieval Overhaul 1.6
- Nice Plants Menu
- Dubs Mint Menus

## Notes

This update changes only Info Card ordering. Plant balance values and cold-tolerance behavior are unchanged from 0.1.1 Beta.

## License

MIT License.

## AI-assisted development

AI was used primarily for code implementation, documentation, research, and test development. Design decisions, balance decisions, and final testing/review remain the author's responsibility.
