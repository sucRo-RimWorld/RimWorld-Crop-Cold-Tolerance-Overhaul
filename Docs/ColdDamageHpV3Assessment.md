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

## Representative-plant independent V3 check using original hourly Vanilla climate (2026-10-23)

**Source/evidence:** Downloaded the original GitHub Actions #39776410000 artifact 12424418617 `climate-native-hourly.csv`, verified raw SHA256 `6eac0d50124fbd622d37737abd1a86223b3649b27133e3e9349a43797e37b8c5`, and recomputed degree-hour exposure directly from its **4320 original hourly samples** (3 world tiles x 60 days x 24 hours). The derived daily low/high table alone is insufficient to compute reliable integrated dosage. Source is world tile calendar temperature from original 1.6, **not** live plant ambient-temperature sampling, growing/maturity, weather/ColdSnap events or actual injury/kill observations. Numerical results assume hourly continuous exposure, full initial HP, median dose, no competing damage, and no missed sample intervals. Losses can exceed 100% arithmetically only in the diagnostic aggregate; an actual plant dies at 100%.

### Observed 60-day world calendar: exact hourly integration, V3 unapproved candidates

| Plant | V3 onset / D | Temperate sample: HP loss first 20 days | Temperate: first day cumulative exposure reaches full HP | Coldest boreal: first full-HP day |
|---|---|---:|---:|---:|
| Rice | +3C / 60 C*h | 0% | 25 | 18 |
| Tomato | 0C / 60 C*h | 0% | 27 | 21 |
| Cabbage | -3.5C / 240 C*h | 0% | 31 | 26 |
| Wheat | -7C / 200 C*h | 0% | 32 | 27 |
| Pine | -40C / 5000 C*h | 0% | none (60d) | none (60d) |
| Cocoa | +5C / 180 C*h | 0% | 26 | 19 |

The `temperate_crop_marginal` tile is actual TemperateForest tile 2851 with annual world-tile temperature +5.672C; boreal cold tile 44000 is a different, much colder site. **This is not a claim that rice, wheat or cocoa can mature in those first 20 days**, that cocoa naturally grows in this temperate biome, or that every calendar-day slice corresponds to a viable outdoor planting window. Full-HP days apply only if plants unrealistically remain unharvested/alive throughout exposure and no other processes act. At pine's -40C onset, both real sampled BorealForest calendars remain entirely above threshold (minima -37.835C and -32.647C); thus no calendar-only cumulative tree mortality was found. ColdSnap remains out of scope for this record.

### Falsification against *pre-existing* S01-S05, S16, S19-S24 gameplay frost criteria

These targets were written BEFORE V3 values in `Docs/ColdDamageBalanceV2Targets.md` and are **gameplay preferences**, not biological mortality experiments. The former target requirement for dedicated healing in some V2 scenarios is **superseded by the user's explicit choice of no CCTO healing**; retain only their independently stated survival/mortality gameplay checks. Percent survival below is **predicted by an unvalidated deterministic uniform ±20% *dose* spread**, *not* real RimWorld deaths or empirically measured survival. Ordinary Plant HP damage and rounding can further alter exact game outcomes. Baseline assumes initially undamaged plants.

| Plant | Target scenario | Predeclared preferred survival | V3 old D: predicted survival | Assessment |
|---|---|---|---:|---|
| Tomato (D60) | S01: -1C x 2h | at least 80% | 100% | compatible |
| Tomato (D60) | S02: -3C x 8h | at most 30% | **100%** | **FAIL** |
| Tomato (D60) | S16: -1C x 8h, 3 nights | some but not all die | **100%** | **FAIL** |
| Cabbage (D240) | S03: -4C x 8h | at least 80% | 100% | compatible |
| Cabbage (D240) | S04: -8C x 24h | at most 30% | **100%** | **FAIL** |
| Cabbage (D240) | S05: -4C x 8h, 5 nights | at least 70% | 100% | compatible (without HP repair) |
| Onion (D120) | S19: -2.5C x 3h | at least 95% | 100% | compatible |
| Onion (D120) | S20: -6C x 8h | at most 30% | **100%** | **FAIL** |
| Onion (D120) | S21: -4C x 8h, 3 nights | at least 70% | 100% | compatible |
| Pumpkin (D68) | S22: -1C x 2h | at least 80% | 100% | compatible |
| Pumpkin (D68) | S23: -3C x 8h | at most 30% | **100%** | **FAIL** |
| Pumpkin (D68) | S24: -1C x 8h, 3 nights | 5–90% | **100%** | **FAIL** |

**Decision:** Tomato, Cabbage, Onion and Pumpkin current V3 dose numbers are **REJECTED for XML**; candidate CSV retains the original numbers for audit and marks `FAILED_PREDECLARED_FROST_GOALS`. These failures do not reject the HP model itself. Other profiles remain UNAPPROVED, not passed.

### Smaller-dose follow-up options (NOT ADOPTED; retain old CSV numbers until decision)

For the same onset and uncalibrated dose variation **w=0.20**, follow-up *candidate* D options can satisfy those gameplay targets algebraically. They are options for comparing against actual ordinary game temperatures, **not independent confirmation of physiology or player experience**:

| Plant | A | Old D -> option D (C*h) | Mild scenario survival | Hard frost survival | Repeated nights survival |
|---|---:|---:|---:|---:|---:|
| Tomato | 0C | 60 -> **21.5** | S01 100% | S02 **20.9%** | S16 **20.9%** |
| Cabbage | -3.5C | 240 -> **96** | S03 100% | S04 **18.8%** | S05 100% |
| Onion | -3.5C | 120 -> **18** | S19 100% | S20 **22.2%** | S21 100% |
| Pumpkin | 0C | 68 -> **22** | S22 100% | S23 **27.3%** | S24 **27.3%** |

Temperature-sequence checks on the same actual temperate tile 2851 show these options give **0% cold HP loss in its first 20 sampled days** (as do the old candidates); they reach median full-HP cumulative dose around calendar days **26 tomato, 29 cabbage, 28 onion, 26 pumpkin** if left standing year-round. Their old trial doses reached full-HP around days 27, 31, 29, 27 respectively. This is only a temperature-exposure scenario: it does not establish crop harvest availability, normal growth, or plant HP regeneration.

**Important:** The partial-survival percentages above **depend on the trial w=0.20 distribution**. At w=0, an otherwise undamaged plant either survives or dies at the deterministic threshold; in particular, the intended 5–90% partial loss across identical repeated pumpkin nights cannot occur. The 0.20 dose spread and numerical options are still unapproved; do not report fitted survival values as real mortality data.

### Scope boundary

Rice's same-onset +3C profile cannot represent opposite acclimated/unacclimated outcomes at +4C (R02); Wheat's -7C onset cannot represent nonacclimated W13 trial deaths at -3C or -5C. These are known **stage/acclimation mismatches**, not solved by changing only D. Do **not** add a plant-stage acclimation simulator in response; evaluate an explicit, bounded gameplay compromise separately if needed. Tropical cocoa was tested only in unsuitable northern climate samples; no native tropical ecology claim is made. For wild trees, avoid extrapolating continuous -45C for 15 days: prioritize real biome/calendar exposures and event conditions. Production stays at 48 fixed-death + 30 dormancy XML entries pending numerical author acceptance and consolidated native HP verification.
