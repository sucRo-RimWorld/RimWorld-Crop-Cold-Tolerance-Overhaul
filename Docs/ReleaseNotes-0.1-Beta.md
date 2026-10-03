# Crop Cold Tolerance Overhaul — 0.1 Beta

Git tag: `v0.1.0-beta`

## Summary

The first public Beta of Crop Cold Tolerance Overhaul (CCTO).

CCTO separates minimum growth temperature from cold-death behavior and gives supported crops explicit fixed cold-death temperatures or cold dormancy.

## Included

- Vanilla RimWorld 1.6 cold-tolerance rebalance for 12 supported crop/tree Defs.
- Optional Medieval Overhaul 1.6 balance for 21 MO-specific plant Defs.
- Explicit `Cold death temperature / 枯死温度` display.
- Explicit `Dormancy temperature / 休眠温度` display.
- Cold dormancy with delayed Vanilla-style leafless recovery.
- Indoor behavior based on the plant's actual ambient room temperature.
- Nice Plants Menu soft compatibility.
- Dubs Mint Menus compatibility through the standard RimWorld Info Card path.
- XML-facing framework API for other mods to assign fixed cold-death temperatures and dormancy.

## Balance direction

The current numbers are generally more demanding than the original Vanilla / Medieval Overhaul cold settings.

Most supported crops require warmer conditions for active growth, and many ordinary crops die at substantially warmer temperatures than Vanilla's generic derived threshold.

Selected overwintering/perennial crops are deliberate exceptions and gain cold dormancy.

## Verification

The release candidate has passed:

- integrated automated framework + Vanilla/MO gate: 13/13;
- final loaded Def validation for all 12 Vanilla and 21 MO targets;
- normal-game startup and standard Info Card smoke;
- explicit dormancy-temperature visual recheck;
- Dubs Mint Menus display verification;
- Nice Plants Menu compatibility automated and manual smoke checks.

## Beta feedback requested

Feedback is especially useful on:

- fixed species-level death thresholds versus deterministic per-plant variation;
- whole-field synchronized cold death;
- seasonal planning clarity;
- cold-death / dormancy UI clarity;
- compatibility with other crop and plant-menu mods.

The archived deterministic range design remains available as a possible future option if Beta feedback favors individual variation.

## Dependencies

Required:

- Harmony

Optional:

- Medieval Overhaul 1.6
- Nice Plants Menu
- Dubs Mint Menus

## License

MIT License.

## AI-assisted development

AI was used primarily for code implementation, documentation, research, and test development. Design decisions, balance decisions, and final testing/review remain the author's responsibility.
