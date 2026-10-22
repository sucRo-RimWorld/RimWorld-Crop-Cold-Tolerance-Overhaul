# CCTO HP-only V3 candidate assessment — 2026-10-22

**Status: TRIAL / NOT ACCEPTED FOR XML OR PUBLICATION.** See [ColdDamageHpV3Candidates.csv](ColdDamageHpV3Candidates.csv). The table covers exactly 48 formerly fixed-death plants, with 30 existing dormancy definitions retained. These are user-directed **HP direct damage with no cold-specific recovery** parameters. Each species has just two trial inputs: onset temperature `A` and full-HP degree-hours `D`; uniform deterministic variation half-width 0.20 is provisional. No added acclimation, tissue-organ model, history interpolation or periodic scan.

## Formula and irreversible HP implications

For each sample, with ambient temperature `T < A`, elapsed game hours `h` (currently capped at 0.8 per observation), and maximum plant HP `H`, lose `max(0,A-T) * h * H / (D * individualMultiplier)` HP, retaining sub-1 HP fractions. Actual damage is dealt through ordinary RimWorld Plant HP. At or above onset, HP stays at its existing value; there is **no special recovery**. Fire/animal/other HP loss therefore reduces the remaining frost margin. Replacing a reversible normalized injury pool with irreversible HP damage **changes long-run plant survival qualitatively**, and prior V2 survival fractions cannot validate this table.

## Independent illustrative exposure checks — not real-game observations

All rows below assume an undamaged median-susceptibility plant, precise piecewise constant temperatures, no ordinary regeneration, other damage or sampling gaps; results are **percentage of base maximum HP lost**, not experimentally measured mortality probabilities.

| Scenario | Trial plant (A, D) | Exposure | Median HP lost |
|---|---|---|---:|
| Mild recurring frost, tomato | 0C, 60 C*h | -1C x8h, +10C x16h, 3 cycles | 40.0% |
| Mild recurring frost, pumpkin | 0C, 68 C*h | -1C x8h, +10C x16h, 3 cycles | 35.3% |
| Mild recurring frost, onion | -3.5C, 120 C*h | -4C x8h, +5C x16h, 3 cycles | 10.0% |
| Mild recurring frost, cabbage | -3.5C, 240 C*h | -4C x8h, +5C x16h, 5 cycles | 8.3% |
| Long hard winter, pine/fir | -40C, 5000 C*h | -35C x12h, -30C x12h, 60 cycles | 0.0% |
| Extreme cold, pine/fir | -40C, 5000 C*h | -45C continuously for 15 days | 36.0% |
| Hypothetical extreme snap, pine/fir | -40C, 5000 C*h | -55C continuously for 3 days | 21.6% |

An extreme natural 60-day forest biome series sampled in real Vanilla 1.6 bottomed at -37.835C on one cold Boreal tile (Project Actions 39776410000), so this trial onset gives **zero loss on those world tile calendar temperatures**. That is a narrow observation, not a proof of surviving actual ColdSnap events, other seeds, weather, or years. The trial -40C onset is a gameplay ecology protection choice; it is **not a published Scots pine LT50**. Because cold HP damage is permanent, frequent sub-onset exposure every winter will eventually eliminate plants even where each single winter is survivable. Whole-year and multi-year wild ecology are necessary acceptance conditions, rather than a 15-day survival check alone.

## Risk findings and follow-up

- **High priority:** exact loaded-game maximum HP values and integer rounding matter; even a small conceptual HP-percent loss may look like no loss until fractional remainder accumulates. The existing fractional remainder in Draft handles this but needs acceptance.
- **High priority:** normal growth/no-temperature sampling and vanilla/third-party Plant HP regeneration are not calibrated. Absence of dedicated CCTO recovery does not demonstrate that any external effect will never heal HP.
- **High priority:** selected trial onset temperatures for rice (+3C), cold-perennial strawberry (-12C), citrus (-5C) and native trees are plausible gameplay brackets, not primary same-stage whole-plant survival measurements. Source organ/stage differences must stay explicit.
- **High priority:** 48 proposed rows are **not** 48 verified botanical fits. They are a finite initial gameplay matrix from thermal role/habitat groups, avoiding the V1 error of marking derived formulas as approved evidence. The only hard parity checks are cultivation/wild Mindwort, Poppy, Fleawort and Healroot, plus Pine/Great Fir ecological analogue.
- **Gameplay constraint:** two species-specific fields and no dedicated HP recovery. Do not add physiological acclimation or new runtime subsystems in order to force-fit W13/R02 laboratory survival results. If a proposed species profile yields unacceptable ordinary colony or wild-biome consequences, retune its two numeric fields or retain its current fixed-death behavior until reviewed.
- **Release gate:** keep current 48 fixed death and 30 dormancy production values intact. Do not blindly copy V3 to XML or claim the previous 1.6 engine/native saves validated HP rules. Stage a consolidated representative real-game acceptance only once the HP implementation is build-ready and new numerical bounds reviewed.

## Correction: pine mortality calibration (2026-10-22)

Constant -45C for 15 days is an arithmetic stress test, not a representative RimWorld forest climate; it must not be used to infer a need for damage healing or claim automatic long-term forest loss. The original native 1.6 world calendar temperatures in Docs/Research/CCTOVanilla16ActualWorldClimateDailyLoHi.csv show minima -37.835C (cold Boreal tile 44000) and -32.647C (pine-bearing Boreal tile 40777). At V3 pine onset -40C, both yield zero HP loss over the 60-day calendar series, before weather incidents.

For a hypothetical three-day cold snap shifting each observed daily minimum down by 20C, and unrealistically charging each day's minimum for the full 24h, the worst sliding three-day bounds are 1175.52 degree-hours / 23.5% HP on tile 44000 and 856.80 degree-hours / 17.1% HP on tile 40777 (D=5000). These are intentionally overestimated scenario bounds, NOT verified ColdSnap events, and real daytime warming normally reduces dose. The missing evidence is actual weather-event exposure below -40C, not physiological healing. The earlier assertion that these forests necessarily lose HP cumulatively each winter is withdrawn.
