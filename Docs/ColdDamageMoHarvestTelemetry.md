# CCTO V3 real-MO harvest and frost telemetry

## Purpose and scope

The owner of final numeric values remains Docs/ColdDamageHpV3Candidates.csv. This standalone native probe loads actual Medieval Overhaul 1.6.2.2, VEF, SYR and CCTO V3 (not a MO fixture), with a disposable original-game copy and no player save/Workshop changes. It compares ten loaded Core/MO crop Defs and native Plant.YieldNow.

For each real spawned, fully-grown crop the test records warm +25 C full-HP yield, -5 C for ten simulated 2000-tick cold observation intervals (8 game hours), and -10 C for five intervals (4 game hours). These are controlled *observation offsets* and not eight/four hours of ordinary clock nor seasonal weather. Earlier CCTO V3 1.6 native 19/19 and real Scribe evidence covers the separate normal-Tick/save mechanics.

Output is 30 rows with actual HP/yield or death, plus loaded plant growDays, minGrowth, fertility sensitivity, sow skill, raw product, sow/harvest work fields if present and V3 A/D. growDays are not calendar days; labor work units are not observed Pawn work time; raw items are not calorie-equivalent, and crop processing and actual seasons are not tested.

The seven concerns in Docs/CropGameplayRoles-ja.md are qualitative interpretation questions. A technically passing telemetry run never automatically approves these seven balance roles. Defer further native work unless the captured evidence exposes a concrete discrepancy that needs an actual job/season experiment. Release and Steam are out of scope. The single Project manual runner performs source-pinned building, actual original-MO check, Unity ERROR gate, unique-30-row gate, evidence export and cleanup.