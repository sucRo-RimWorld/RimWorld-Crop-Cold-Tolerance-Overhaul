# CCTO 0.1 Alpha Release Checklist

This is the release-preparation checklist for the first public Alpha.

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

Integration merge commit: `a1616054a7d081f3167db501a49dfe3630ce4402`.

## Public-release preparation still required

- [ ] Add an `About/Preview.png` suitable for the RimWorld mod list / public presentation.
- [ ] Finalize Steam Workshop title, short description, long description, and tags.
- [ ] Decide whether a license should be added; do not infer a license automatically.
- [ ] Decide the exact public version/tag spelling (current design target: **0.1 Alpha**).
- [ ] Create the GitHub release/tag only after the public version string is confirmed.
- [ ] Upload/publish the Steam Workshop item only after the presentation assets and description are approved.
- [ ] After publication, perform one clean install/subscription smoke test from the published package.

## Alpha feedback targets

The first Alpha should explicitly ask players about:

- exact fixed species-level cold-death thresholds versus deterministic per-plant variation;
- predictability and seasonal planning;
- synchronized whole-field cold death;
- usefulness and clarity of the cold-death/dormancy temperature display.

Do not call the release Beta until the Alpha feedback phase and post-publication validation justify that change.
