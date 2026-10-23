# CCTO HP-only V3 candidate assessment — 2026-10-22

**Status: TRIAL / NOT ACCEPTED FOR XML OR PUBLICATION.** **2026-10-23 author direction update:** the old S04/S20 ≥70% cabbage/onion death preferences have been superseded by survival-first and HP-related reduced harvest. Current CSV therefore contains **2** obsolete-numeric failures (tomato/pumpkin) and **46** otherwise unapproved HP trials, not four active hard-frost failures. The 2026-10-22/early-23 four-failure table below is preserved as a dated historical comparison, **not** an active test gate. See [current criteria](ColdDamageBalanceV2Targets.md#v3-s04s20-supersession--author-approved-direction-2026-10-23). See [ColdDamageHpV3Candidates.csv](ColdDamageHpV3Candidates.csv). **Latest 2026-10-23 numerical narrowing:** for hardy cabbage, prefer comparing its existing unapproved A−3.5/D240 over previously proposed D160; for onion, inspect newly proposed A−5/D120 versus old unapproved A−3.5/D120 to limit cumulative injury over repeated −4°C nights; ±20% variation remains unapproved. No values adopted. **Latest four-crop survival and whole-field harvest-yield screening:** [ColdDamageHpV3YieldReview.md](ColdDamageHpV3YieldReview.md) (comparison shortlist only, no accepted values). The table covers exactly 48 formerly fixed-death plants, with 30 existing dormancy definitions retained. These are user-directed **HP direct damage with no cold-specific recovery** parameters. Each species has just two trial inputs: onset temperature `A` and full-HP degree-hours `D`; uniform deterministic variation half-width 0.20 is provisional. No added acclimation, tissue-organ model, history interpolation or periodic scan.

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

**Historical decision (superseded for S04/S20 on 2026-10-23):** Tomato, Cabbage, Onion and Pumpkin current V3 dose numbers are **REJECTED for XML**; candidate CSV retains the original numbers for audit and marks `FAILED_PREDECLARED_FROST_GOALS`. These failures do not reject the HP model itself. Other profiles remain UNAPPROVED, not passed. **CURRENT INTERPRETATION:** Only tomato and pumpkin still fail the retained predeclared hard-frost death expectations. Cabbage and onion are no longer rejected *for failing 70% mortality* because the author changed that priority; their existing D240/D120 remain **UNAPPROVED**, not automatically accepted for yield balance.

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

## Scientific and gameplay robustness re-audit — 2026-10-23 (supersedes premature small-D recommendation)

**Decision: KEEP ALL NUMBERS UNAPPROVED.** Do not treat the four smaller-dose examples above (tomato D21.5, cabbage D96, onion D18, pumpkin D22) as new approved or recommended defaults. They fit V2-predeclared *ideal continuous-exposure gameplay preferences* at fixed A and hypothetical uniform individual-dose half-width 0.20, but they have **not** demonstrated robustness to 0.5°C onset uncertainty, actual 2000-tick sampling, body-stage tolerance, or real colony impacts. Existing 48 HP entries stay unchanged, including the four failed-predeclared-target flags. Any later numeric revision needs an independent reason beyond hitting an old survival percentage.

### Why fitted four-dose examples are not defensible as final values

For each original V3 onset A, the table predicts the proportion of undamaged plants surviving the previously specified *hard frost scenario*, if A were instead 0.5°C lower/higher while D remains the smaller-dose example. This is **ideal continuous-exposure math** with assumed uniform ±20% lethal-dose variation, **not actual game observations**:

| Plant / nominal hard-frost exposure | Example D °C·h | Survival at A−0.5°C | Survival at A | Survival at A+0.5°C |
|---|---:|---:|---:|---:|
| Tomato −3°C ×8h, A=0 | 21.5 | 67.4% | 20.9% | 0% |
| Pumpkin −3°C ×8h, A=0 | 22 | 72.7% | 27.3% | 0% |
| Cabbage −8°C ×24h, A=−3.5 | 96 | 50.0% | 18.8% | 0% |
| Onion −6°C ×8h, A=−3.5 | 18 | 77.8% | 22.2% | 0% |

The candidate's survival fraction is not a smooth or stable calibration target near the population cutoff. This is not evidence of a C# mathematical bug: sharp lethality can occur physiologically too, but a static whole-plant model cannot claim experimental mortality accuracy from this fitted percentage. Every selected D also causes **irreversible partial HP loss**, which in Vanilla plants can lower harvest yield independently of death. Evaluate **survival and yield** together rather than mortality only.

For additional game-world context, the original real 1.6 TemperateForest tile 2851 *hourly world calendar* data (4320-sample dataset in this draft's prior section) first crosses **0°C on day 24** and **−3.5°C on day 26**. At the current first below-zero day (day 24), exposure below 0°C integrates to 3.89°C·h: the new tomato/pumpkin trial doses would lose about 18% median maximum HP before integer rounding, versus 6% for their existing D60/D68 V3 candidate doses. By the end of day 25, below-zero exposure has accumulated to about 18.9°C·h: tomato D21.5 and pumpkin D22 reach roughly 88% and 86% median HP loss. **These are calendar-only hypotheticals**, not proof that a growing crop is present or that its *ambient* temperature follows the unmodified world tile, and actual CCTO sparse sampling can differ. This shows why evaluation must include a realistic first frost and accumulated loss, not merely one idealized hard frost at fixed temperature.

### Evidence boundaries for the four crops

- **Tomato:** Ontario Ministry of Agriculture's [Freezing and chilling injury in tomato and pepper plants](https://www.ontario.ca/page/extreme-temperature-effects-tomato-and-pepper-crops) describes chilling injury at 0–5°C, potential freezing damage, and variation with cover, variety, vigor and freeze/thaw conditions. It does **not** give an exact whole-plant fatal dose at −3°C ×8h. A = 0°C is an uncalibrated gameplay onset, not a biological threshold inferred from that source.
- **Pumpkin:** [University of Maryland Extension, Growing Pumpkins](https://extension.umd.edu/resource/growing-pumpkins-home-garden) notes a light frost can kill the vines while the fruit may still be undamaged. Generic RimWorld Plant death can abstract the vines, but fruit injury/storage and whole-plant survival cannot be conflated. The D22 figure is a gameplay trial, not a published survival value.
- **Cabbage:** Sasaki et al. (1996), [Annals of Botany, doi:10.1006/anbo.1996.0131](https://doi.org/10.1006/anbo.1996.0131), report that cabbage seedlings acclimated at 5°C acquired frost tolerance to −6°C; acclimation duration matters. The fixed V3 A=−3.5°C therefore **cannot be read as a universal injury onset for acclimated cabbage**. [JIRCAS follow-up](https://www.jircas.go.jp/en/publication/jarq/34/4/261) confirms light during acclimation also changes responses. Do not introduce acclimation simulation merely to fit this observation; choose an explicitly game-wide compromise and label it as such.
- **Onion:** [J-STAGE, Hokkaido onion seedling work](https://www.jstage.jst.go.jp/article/hortj/89/4/89_UTD-155/_html/-char/en) reports a cited earlier experiment in which nearly all seedlings survived −2.5°C for 1–3h, but does not establish 70% mortality for −6°C ×8h. [University of Georgia Cooperative Extension](https://extension.uga.edu/publications/detail.html?number=B1198&title=onion-production-guide) says onions generally withstand light to heavy frosts, although hard freezes can injure bulbs and growing points. Thus **V2 target S20 (“at most 30% survive −6°C ×8h”) is a gameplay preference, NOT an independent measured death rate; reassess the target before reducing onion D120 to D18**. At their shared nominal A=−3.5°C, D18 would also make onion approximately 5.3× more sensitive per degree-hour to HP damage than cabbage D96; that ordering needs evidence. No source here determines a correct alternative D.
- **Rice/wheat:** Primary Rice 4°C chilling studies and wheat non-acclimated/acclimated cohorts already documented above show conditioning/stage effects the static two-parameter model cannot match. Keep those constraints open rather than inventing universal cultivar averages or new subsystems.

### Resulting data-review order and limits

1. **First: warm-season annuals** (tomato, pumpkin) — compare mild frost and extended frost for survival **and remaining-HP yield**, then check first-season exposed calendar with the CCTO sparse-sampling approximation. The existing smaller-D examples are merely sensitivity probes.
2. **Second: overwinter/cold-tolerant vegetables** (cabbage, onion) — establish defensible *whole-plant* frost consequences; in particular review whether the arbitrary onion S20 hard-frost mortality target should remain. Do not force a fitted D18 to achieve that target.
3. **Third: rice, wheat, corn, potato, strawberry** — avoid using seedling, leaf, corm, fruit or acclimated-tissue measurements as unconditional adult whole-plant death. Keep explicit approximation or current legacy death threshold as an option pending gameplay decision.
4. **Fourth: wild trees and native perennial flora**, beginning with naturally generated pine/Great Fir and habitat-sensitive broadleaves/tropical trees — test plausible native biome/calendar and ColdSnap impacts. Pine's two sampled boreal calendars are narrow evidence only, not all-biome tree clearance.
5. **Finally: remaining entries and 30 dormant** — check identity, same-taxon parity and role-based plausibility, not 48 invented calibration fits. Do not convert all 48 to XML, approve w=0.20, or repeat whole-game runs for numeric candidates yet.

**Current gate:** No new plant HP coefficient has been adopted here; no executable code or production XML changes. Previous predeclared S targets are useful *falsification prompts*, but none becomes an immutable botanical truth merely because it predates a candidate. A single later consolidated 1.6 native acceptance is warranted **after** a defensible candidate matrix and source-level sampler behavior are stable, with 1.7/MO scoped explicitly.

## Correction: duplicated V2 benchmark doses under V3 no-recovery HP model (2026-10-23)

**The old benchmark scenarios are not all independent under V3.** At A=0°C and with no recovery, tomato S02 (−3°C ×8h) and S16 (−1°C ×8h on 3 nights with warm intervals) each deliver **24°C·h**, so their ideal end-of-exposure Plant HP survival is **identical by construction** at every fixed D and per-plant lethal-dose multiplier, regardless of time segmentation. The same is true for pumpkin S23 versus S24 (also 24°C·h each). An implementation's 2000-tick capped last-sample approximation may break exact equality, but that would not provide independent biological evidence. Do **not** count passing both as two independent corroborations, and do **not** add a complex recovery/history system simply to make them differ.

If the provisional uniform individual lethal-dose half-width w=0.20 is retained purely for sensitivity analysis, the combination of the formerly predeclared preferences **algebraically forces fitted doses into narrow windows**:

- Tomato S02 (survival ≤30%) plus S16 (some but not all die), assuming exact continuous exposure: approximately **20.0 < D ≤ 22.22°C·h**, because both deliver 24°C·h.
- Pumpkin S23 (survival ≤30%) plus S24 (survival 5–90%): approximately **20.34 ≤ D ≤ 22.22°C·h**, again for identical 24°C·h.

Thus the prior D21.5/D22 options sit inside model-driven intervals by design, not because two separate climate/field observations converged. A narrow fitted interval near the chosen population lethality boundary predictably creates the ±0.5°C sensitivity documented above. This is a **validation-design flaw**, not a proof that these crops truly die at those D values. Retire the illusion of independent S02/S16 or S23/S24 acceptance; substitute evidence from genuinely different exposure shapes, realistic growing-season temperature windows, and harvest-yield consequences before recommending values. This is analysis only; the original predeclared targets remain preserved as historical context and no production value is changed.

## Reference comparison with existing instant-death XML (2026-10-23)

The same original **1.6 TemperateForest tile 2851 hourly world/calendar** shows how idealized V3 affects time to **whole-plant death versus existing published fixed-death thresholds**. These are not loaded plant death observations, actual map weather/plant ambient temperatures, or assumed viable sow/harvest windows. The time-of-day interpolation/2000-tick sampler and individual HP/other damage can shift results.

| Plant | Current production fixed-death T, first crossing day | Existing V3 trial D, 100% median integrated HP-loss day | Smaller-D *unapproved* example, 100% median integrated HP-loss day |
|---|---|---|---|
| Tomato | −1°C, day 24 | D60, day 27 | D21.5, day 26 |
| Pumpkin | −1°C, day 24 | D68, day 27 | D22, day 26 |
| Cabbage | −6°C, day 27 | D240, day 31 | D96, day 29 |
| Onion | −3°C, day 26 | D120, day 29 | D18, day 28 |

This means those illustrative smaller-D cases still provide a delay in **fatal** cold injury compared with existing instant-death XML at this single world-calendar site, **but they cause irreversible HP damage and reduced harvest yield before dying**, so they are not necessarily easier or harder overall. This single 60-day calendar cannot rank comparative in-game difficulty without sow date, crop growth, season length, shelter, actual weather and ordinary health/yield effects. Do not promote the example Ds on this basis.

## Additional high-priority experimental cohort mismatches (2026-10-23)

Before reviewing the other 44 unapproved profiles, prioritize these **three distinct evidence/model mismatch risks**. They do not establish acceptable replacements and they cannot be fixed by inventing hidden conditioning coefficients.

| Profile (original V3 A,D) | Independent prior evidence | Unmodified V3 ideal model prediction | Limit |
|---|---|---|---|
| MO Corn A=0°C, D=64°C·h, w=0.20 | Buican (1969) mortality results summarized in Lindsey et al. (2024): ~50% death at −2°C ×48h, and ~50% at −6°C ×2h | First exposure E=96°C·h, **all individuals die**; second E=12°C·h, **all survive** (dose range 51.2–76.8°C·h) | Fails both paired approximate mortality endpoints; the study spans maize plant stage/tissue contexts not represented by CCTO. Do not fit a near-knife-edge onset to enforce them. |
| Vanilla Rice A=+3°C, D=60°C·h | Sagehashi et al. (2023): 4°C ×10d seedlings, conditioned vs unconditioned survival differs sharply | At +4°C (>A), **zero CCTO cold HP loss regardless of duration** | Cannot reproduce unacclimated seedling long-chill mortality; also cannot reproduce acclimated and unacclimated outcomes simultaneously with fixed parameters |
| MO Wheat A=−7°C, D=200°C·h | W13 non-acclimated whole-seedling −3°C/−5°C ×24h results in prior Targets; acclimated cohorts survive differently | At −3°C and −5°C (both >A), **zero CCTO cold HP loss** | Does not reproduce unacclimated study; generic wheat intentionally lacks conditioning/cultivar-stage state, so a single laboratory curve is not an appropriate numeric lock |

Evidence references: [Lindsey et al., Crop Science 2024, doi:10.1002/csc2.21212](https://acsess.onlinelibrary.wiley.com/doi/10.1002/csc2.21212); [Sagehashi et al., 2023, doi:10.1080/1343943X.2023.2236344](https://www.tandfonline.com/doi/full/10.1080/1343943X.2023.2236344). Wheat source provenance and cohort separation are preserved in \`Docs/ColdDamageBalanceV2Targets.md\` and existing \`Docs/Research/WinterWheatW13*.csv\`.

**Disposition:** retain these values only as *unapproved gameplay placeholders* and mark the experiments as evidence of lossiness. Do not present coverage, compiler PASS or approximation as biological calibration. A simpler static two-value model is still preferred to stage acclimation simulation; the explicit design choice is which ordinary gameplay outcome it represents, not exact fidelity to mutually incompatible cohorts. No actual XML numeric change has been approved.

## Author-approved V3 cabbage/onion criterion change — 2026-10-23

**Approved design direction, not numerical values:** MO Cabbage and MO Onion should **survive** representative hard frost with an ordinary-HP **yield penalty**, rather than treating a ≥70% immediate death fraction as the success target. This replaces S04 and S20 as *active* V3 gameplay criteria. Exact survival fraction, harvest reduction fraction, onset and fatal dose remain **undecided**; no inference from a study establishes D160 or D120.

- **S04-V3 cabbage:** −8°C ×24h is now a test of cold-damaged **survival and lower harvest**; severe recurring exposure can still be fatal. The current *unapproved* D240 at A−3.5 mathematically results in ~45% maximum-HP loss over this precise ideal interval and about 77% survivor yield, not a failure against an obsolete 70% death requirement. The comparison proposal A−3.5 / D160 yields ~68% maximum-HP loss and ~66% yield. Neither is approved, and real HP integer/sampler behavior is unverified.
- **S20-V3 onion:** −6°C ×8h is now a test of **survival and harvest loss**, with tolerance of recurring light frost assessed separately. Unapproved A−3.5 / D120 predicts ~17% maximum-HP loss and ~92% harvest on ideal continuous exposure; this is a comparison, not an accepted coefficient. Previous D18 fitted to 70% mortality is no longer relevant for this criterion.
- **Unchanged:** S02 tomato and S23 pumpkin remain strong-frost danger criteria; 30 existing dormancy cases unchanged, no cold-only HP healing, no novel growth-stage subsystem. Candidate CSV counts are now **46 UNAPPROVED_HP_TRIAL, 2 FAILED_PREDECLARED_FROST_GOALS and 30 KEEP_EXISTING_DORMANCY** (all 48 HP tuples still unauthorized for XML). Do **not** treat a status change from failed to unapproved as a balance pass.

The numeric table above is archival evidence of why the old fitted hard-frost targets failed. See [current V3 criteria](ColdDamageBalanceV2Targets.md#v3-s04s20-supersession--author-approved-direction-2026-10-23) and [harvest screening](ColdDamageHpV3YieldReview.md); priority is independent plausible field-play balance for hardy crops, not reproduction of an invented percentage mortality threshold.
