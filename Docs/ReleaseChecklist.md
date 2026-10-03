# CCTO 0.1 Beta Release Checklist

This is the release-preparation checklist for the first public Beta.

## Completed technical gates

- [x] Framework behavior implemented.
- [x] Vanilla cold-tolerance balance implemented.
- [x] Medieval Overhaul 1.6 balance implemented.
- [x] Explicit cold-death temperature shown in the standard Info Card.
- [x] Explicit dormancy temperature shown in the standard Info Card.
- [x] Dormancy recovery semantics covered by regression tests.
- [x] Indoor actual-ambient-temperature behavior covered by regression tests.
- [x] Full integrated automated gate passed: 13/13.
- [x] Normal-game startup/Info Card smoke passed.
- [x] Dubs Mint Menus display verified through the standard Info Card path.
- [x] Nice Plants Menu soft compatibility implemented and visually verified.
- [x] PR #2 merged into `main`.
- [x] Public README states that CCTO is a cold-tolerance rebalance and can also be used as a framework for assigning explicit cold-death temperatures / dormancy to other mods' plants.
- [x] Public README shows before/after cold-tolerance values and behavior for all 12 Vanilla targets and 21 Medieval Overhaul-specific targets, with MO Healroot documented as using the Vanilla `Plant_Healroot` value.
- [x] Public README includes an AI-assisted development disclosure covering implementation, documentation, research, and test assistance while retaining author control over design, balance, and final review.

Integration merge commit: `a1616054a7d081f3167db501a49dfe3630ce4402`.

## Public-release preparation still required

- [ ] Add an `About/Preview.png` suitable for the RimWorld mod list / public presentation.
- [ ] Finalize Steam Workshop title, short description, long description, and tags. The long description should explain that CCTO was split out of the planned AMJ project because the mechanic is independently useful; clearly state that CCTO rebalances cold-tolerance values and can also serve as a framework for other mods to assign explicit cold-death temperatures / dormancy; include before/after Vanilla/MO value tables (or an equally clear comparison list); include the same AI-assisted development disclosure used in the README; and note that no display-only edition is currently planned because Vanilla uses a generic derived cold-threshold rule rather than crop-specific fixed death temperatures.
- [ ] Decide whether a license should be added; do not infer a license automatically.
- [ ] Decide the exact public version/tag spelling (current design target: **0.1 Beta**).
- [ ] Create the GitHub release/tag only after the public version string is confirmed.
- [ ] Upload/publish the Steam Workshop item only after the presentation assets and description are approved.
- [ ] After publication, perform one clean install/subscription smoke test from the published package.

## Beta feedback targets

The first Beta should explicitly ask players about:

- exact fixed species-level cold-death thresholds versus deterministic per-plant variation;
- predictability and seasonal planning;
- synchronized whole-field cold death;
- usefulness and clarity of the cold-death/dormancy temperature display.

Do not call the release stable/1.0 until the Beta feedback phase and post-publication validation justify that change.
