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
- [x] Public descriptions state that the current balance is generally higher difficulty than the original Vanilla / Medieval Overhaul cold settings, while noting dormancy crops as intentional exceptions rather than treating every change as a nerf.
- [x] Public README includes an AI-assisted development disclosure covering implementation, documentation, research, and test assistance while retaining author control over design, balance, and final review.

Integration merge commit: `a1616054a7d081f3167db501a49dfe3630ce4402`.

## Public-release preparation still required

- [ ] Add an `About/Preview.png` suitable for the RimWorld mod list / public presentation.
- [x] Steam Workshop title, short description, long description, and tag set drafted in `Docs/WorkshopDescription.md`; final publication-time visual review remains tied to `About/Preview.png`.
- [x] License confirmed: existing repository `LICENSE` is MIT License, copyright 2026 sucRo0629.
- [x] Public version spelling fixed: **0.1 Beta**. Git tag/release tag: **`v0.1.0-beta`**.
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
