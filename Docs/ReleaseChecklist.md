# CCTO Beta Release Checklist

This is the release-preparation checklist for the first public Beta.

## Completed technical gates

- [x] Framework behavior implemented.
- [x] Vanilla cold-tolerance balance implemented.
- [x] Medieval Overhaul 1.6 balance implemented.
- [x] Explicit cold-death temperature shown in the standard Info Card.
- [x] Explicit dormancy temperature shown in the standard Info Card.
- [x] Dormancy recovery semantics covered by regression tests.
- [x] Indoor actual-ambient-temperature behavior covered by regression tests.
- [x] Published 0.1 Beta baseline integrated automated gate passed: 13/13.
- [x] Expanded all-living-plant balance full automated gate passes: 14/14.
- [x] Normal-game startup/Info Card smoke passed.
- [x] Dubs Mint Menus display verified through the standard Info Card path.
- [x] Nice Plants Menu soft compatibility implemented and visually verified.
- [x] PR #2 merged into `main`.
- [x] Public README states that CCTO is a cold-tolerance rebalance and can also be used as a framework for assigning explicit cold-death temperatures / dormancy to other mods' plants.
- [x] Public README shows before/after cold-tolerance values and behavior for all 49 living Core targets and all 29 Medieval Overhaul-specific targets, with MO Healroot documented as using the Core `Plant_Healroot` value.
- [x] Public descriptions define the scope as all living PlantDefs in supported Core/MO sets, including wild-only vegetation, while excluding dead stump/remnant Defs.
- [x] Public descriptions explain that plant-specific thresholds are informed by real-world cold-tolerance / frost-damage / lethal-temperature data, then rounded and tuned for gameplay rather than treated as exact universal biological constants.
- [x] Public README includes an AI-assisted development disclosure covering implementation, documentation, research, and test assistance while retaining author control over design, balance, and final review.
- [x] Public README and Steam Workshop description list the supported languages: English and Japanese.
- [x] Japanese Steam Workshop localization prepared: title `作物耐寒性オーバーホール` plus a paste-ready Japanese BBCode description kept under Steam's UTF-8 size limit.
- [x] Private Steam Workshop subscribed-package smoke check completed with no apparent issues before public visibility; the post-publication clean subscription check remains pending.

Integration merge commit: `a1616054a7d081f3167db501a49dfe3630ce4402`.

## Public-release status

- [x] GitHub release notes drafted in `Docs/ReleaseNotes/ReleaseNotes-0.1-Beta.md`.
- [x] `v0.1.1-beta` release notes finalized in `Docs/ReleaseNotes/ReleaseNotes-0.1.1-Beta.md`.
- [x] GitHub release/tag `v0.1.1-beta` already published.
- [x] Post-0.1.1 Info Card ordering update documented separately in `Docs/ReleaseNotes/ReleaseNotes-0.1.2-Beta.md`.
- [x] Medieval Overhaul duplicate-extension error fix documented separately in `Docs/ReleaseNotes/ReleaseNotes-0.1.3-Beta.md`.
- [x] Preview image technical/art brief fixed in `Docs/PreviewBrief.md`: 640×360 PNG, 16:9, under 1 MB.

- [x] `About/Preview.png` added to `main` from the approved preview image; current blob SHA `c3b690262b92ce06e753fb0ac6e6929ff96bcf73`.
- [x] Steam Workshop title, short description, long description, and tag set drafted in `Docs/WorkshopDescription.md`; final publication-time visual review remains tied to `About/Preview.png`.
- [x] Paste-ready English/Japanese Workshop BBCode bodies updated for the 49-Core + 29-MO all-living-PlantDef scope and linked to the README for complete Core/MO tables.
- [x] YADA upload filtering configured through repository-root `.rimignore`; runtime `About/`, `Assemblies/`, `Languages/`, `Patches/`, and `LICENSE` are retained while repository/development files are excluded.
- [x] License confirmed: existing repository `LICENSE` is MIT License, copyright 2026 sucRo0629.
- [x] Public version spelling fixed: **0.1 Beta**. Git tag/release tag: **`v0.1.0-beta`**.
- [x] GitHub release/tag `v0.1.0-beta` created; the tag resolves successfully on GitHub.
- [x] Steam Workshop item published publicly: `https://steamcommunity.com/sharedfiles/filedetails/?id=3812412548`.
- [x] Post-publication subscribed Workshop package checked; representative CCTO balance values were confirmed correct with no apparent issue.

## Expanded all-living-plant publication refresh

- [x] `run-tests.bat` completed with **14/14** passing scenarios, including the all-loaded-supported-living-plant coverage scenario.
- [x] Normal-profile wild-plant Info Card smoke passed. Verified Wild Healroot (0 C / death -9 C), Berry bush (0 C / dormancy 0 C), Bush (0 C / dormancy 0 C), and Oak (5 C / dormancy 5 C).
- [x] Post-Info-Card-ordering-change `run-tests.bat` rerun passed: **14/14**.
- [x] Cold death / dormancy row alignment visually rechecked in the standard Info Card.
- [x] Live Steam Workshop English/Japanese descriptions updated from the maintained paste-ready files.
- [x] Workshop package updated on existing item `3812412548`; the post-0.1.1 Info Card ordering fix is live.
- [x] MO wild alchemy inheritance fix implemented: four wild alchemy Defs now inherit the cultivated CCTO extension instead of appending a duplicate.
- [x] E2E MO fixture now mirrors the real MO Named-Parent inheritance for those four wild alchemy Defs.
- [x] Automated runtime-log gate added so a CCTO-origin ERROR fails the local E2E gate even if Pickle scenarios otherwise pass.
- [x] Reran `run-tests.bat` after the MO inheritance/runtime-log-gate changes: **14/14 PASS** with zero CCTO-origin runtime ERROR entries.
- [x] In-game verification of the Medieval Overhaul duplicate-extension fix passed.
- [ ] Re-upload the corrected Workshop package as the 0.1.3 Beta update.

## Beta feedback targets

The first Beta should explicitly ask players about:

- exact fixed species-level cold-death thresholds versus deterministic per-plant variation;
- predictability and seasonal planning;
- synchronized whole-field cold death;
- usefulness and clarity of the cold-death/dormancy temperature display.

Feedback informs balance but its absence neither proves correctness nor prevents
stable release. Author decision, 2026-10-06 JST: stable/1.0 is gated by the
applicable verified conditions below, independently of feedback volume or time
spent in Beta. Existing regression PASS records retain their original scope.

## Stable-release conditions

- [ ] Verify the actual release/subscribed package, recording version, hash,
      supported Mod combinations and load order; confirm the intended loaded
      plant values and no duplicate cold-tolerance extensions.
- [ ] In an isolated running game, demonstrate cooling and warming through the
      configured growth, dormancy and death thresholds: growth stops/resumes as
      intended, death occurs strictly below the fixed threshold, and dormancy
      recovery preserves the intended 60,000-tick delay. Verify actual plant
      AmbientTemperature, not merely requested outdoor settings.
- [ ] Verify heated indoor protection and genuinely cold indoor behavior using
      actual room temperatures, with the same production package.
- [ ] Demonstrate a seasonal cycle with representative cultivated and wild plants
      from each supported set, including practical sowing/growth/harvest where
      applicable. Record winter losses, spring recovery and observed balance.
      Direct timer aging or targeted TickLong regression alone is insufficient
      evidence for this seasonal gameplay check.
- [ ] Save and reload during cold dormancy, then verify retained state, recovery
      timing and subsequent growth; also verify death does not revive on reload.
- [ ] Verify advertised supported combinations, including actual Medieval
      Overhaul and its dependencies for MO support. Distinguish fixture coverage
      from real-Mod integration and document untested combinations explicitly.
- [ ] No unresolved major defect and no CCTO-owned runtime ERROR entry in these
      runs. Preserve isolated logs, assertions, tested payload and limitations.

These are open verification gates, not claims that the existing 14/14 regression
has failed or that these checks have never been performed individually. Close
each item only with applicable retained evidence. Automate reproducible behavior;
manual review is reserved for readability and practical play/balance judgement.
Use test saves/profiles and preserve normal saves/settings. Planned additional
plant support does not block stable release of the currently advertised scope.
