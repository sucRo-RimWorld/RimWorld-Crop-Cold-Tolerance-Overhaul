# CCTO cold injury V2 — independently specified outcomes BEFORE selecting coefficients

**2026-10-22; calibration input, NOT numerical coefficient acceptance.**
The following checks are defined **before producing any V2 plant dose/onset/recovery/individual-width table**. Values described as *gameplay preference* are **not laboratory measurements or author-approved release criteria**. The point is to falsify candidate parameters, not force a PASS by choosing self-referential target values.

## Common comparison terms

A scenario fixes *plant*, **ambient** air temperature, exposure duration and assumed history, and predicts **fraction of alive plants** (not visual leaf damage). The model is the as-yet-unactivated sparse cold injury C#; math scenarios assume 2,500 ticks per game-hour, exact constant piecewise intervals, deterministic uniform individual lethal-dose percentiles, zero pre-existing injury, and no sample-delivery gaps. Actual RimWorld weather varies; these are intentionally **stress cases**, not claims about prevailing average climate in a named biome. All temperature values are **air targets**, not leaf/crown/soil readings. Experimental cohorts remain separate. Terms: S=alive fraction, `h`=uninterrupted game-hours, `n`=number of 24h cycles.

## Independent outcomes, status and biological limits

| id | Scope | Predeclared schedule / treatment | Target and provenance | What a conflict means |
|---|---|---|---|---|
| S01 | tender **tomato**, untreated annual | −1°C ×2h | Prefer **S≥0.8** (gameplay: brief frost not instant wipeout) | Too vulnerable to incidental cold |
| S02 | tender **tomato**, untreated annual | −3°C ×8h | Prefer **S≤0.3** (gameplay: meaningful severe frost threat) | Too resistant to sustained freeze |
| S03 | **cabbage**, growing outdoors | −4°C ×8h | Prefer **S≥0.8** (gameplay: cold-tolerant field crop) | Winter crop usefulness lost |
| S04 | **cabbage**, growing outdoors | −8°C ×24h | Prefer **S≤0.3** (gameplay: substantial prolonged hard freeze threat) | Winter crop too resistant |
| S05 | **cabbage**, new injury initially zero | −4°C ×8h, +5°C ×16h, five cycles | Prefer **S≥0.7**, with genuine recovery only in warmer interval | Repeated mild nights erase crop |
| S06 | **wheat**, W13 *non-acclimated 22°C controls* | −3°C ×24 lab hours, plant-stage matched | **M39=35±2%, Zlata=15±2% S** directly observed; one generic wheat may at best span genotypes with explicitly acceptable approximation | Do not use an acclimated curve as generic unacclimated |
| S07 | **wheat**, same W13 non-acclimated cohorts | −5°C ×24 lab hours | Both **0% observed** (not absolute population-wide zero) | Failure of source plausibility |
| S08 | **wheat**, distinct 4°C×7d acclimated W13 cohorts | −3°C ×24 lab hours | **M39=97±3%, Zlata=60±3%** (measured); **state must be explicit** | A single state-independent curve cannot satisfy S06 and S08; **release-blocking modeling gap**, not a parameter to average invisibly |
| S09 | **rice**, 12°C×5d acclimated R02 seedlings | +4°C ×240 lab hours | **88–100% alive** (observed across 6 Hokkaido varieties) | State mismatch: never label rice dose as experimentally calibrated to this result without acclimation |
| S10 | **rice**, unacclimated R02 seedlings | +4°C ×240 lab hours | Reported **0% alive** in contrasted group; distinguish physiological stage and recovery scoring | S09 and S10 cannot both hold for one uncoupled constant-parameter profile |
| S11 | **maize**, M01 secondary mortality synthesis | −2°C ×48h and −6°C ×2h | **~50% mortality each**, approximate laboratory endpoints, different tissues/temperature trajectory may matter | Inspect *both* and near-boundary sensitivity; do not demand exact two-point fit if it creates a spike |
| S12 | **maize** temperature sensitivity | −1.9°C and −2.0°C, constant exposure | Prefer **median lethal-hour ratio ≤1.5** across 0.1°C for gameplay stability (not a published biological constant) | Current simple linear model cannot simultaneously satisfy exact M01 endpoints and this player-facing robustness bound |
| S13 | **pine / Great Fir** living wild trees | −35°C ×12h, −30°C ×12h for **15 game-days** | Prefer **S≥0.95**; protect forests from routine repeated extreme frost in model's stress envelope | Widespread wild flora die-off |
| S14 | **pine / Great Fir**, living wild trees | −40°C **continuously** ×15 game-days | Prefer **S≥0.5** in this *extreme stress case* pending regional/biome temperature cross-check; wild plant persistence takes priority over forced mortality | Loss of forest in one quadrum |
| S15 | **pine / Great Fir** tissue repair | After −35°C cold period, +0°C to +10°C thawing weather | Prefer injury **decreases** after thaw; **do NOT claim physiological repair at −30°C** without explicit separate hypothesis | Current implementation treats all T≥−31°C as repair |
| S16 | **representative warm annual**, repeated nights | −1°C ×8h, +10°C ×16h for **3 days** | Prefer at least some mortality but **not** 100% unless source warrants; report full S instead of hiding it | No gradual consequences / full random wipe |
| S17 | **all eligible lifeforms**, data/mapping | Existing **48 fixed-death** or **30 dormancy** entries | No missing/extra Defs; preserve wild/cultivated same-taxon parity and 30 dormancy; **this is structural evidence only** | Do not report as a biological/game-balance PASS |
| S18 | **cool/warm reversals**, representative living plant | Mild night then warming above **freezing**, second night | Damage persists across cold intervals; partial recovery should be bounded and supported as **gameplay design**, never an experimentally measured rate | Repair model choice must precede coefficient lock |

**Precedence/decision rules:**

- S06–S10 are evidence **comparisons**, not asserted survival thresholds for a state-independent generic Crop Def. The mismatch is evidence of **missing acclimation state**, not a license to invent a mean.
- S12–S15 explicitly evaluate weaknesses in the **current model semantics**. Do not force-fit incompatible numerical targets with arbitrary parameter magnitudes. If the formula cannot represent a needed biological/UX distinction, stop that cohort's XML migration and propose the *smallest* bounded model change with tradeoffs.
- Where primary data are missing (43 non-special plants), the stated gameplay target is a **proposal requiring author review**. One predeclared analogous lifecycle scenario is not a measured plant-species lethality coefficient.
- Outcomes for long-lived wildlife must be checked over a whole **15-day quadrum** and repeated thaw/frost, not just one median-hour anchor at an old instant-kill temperature.
- No model fitting until this target matrix is archived. No production/XML change, no native game run for numerical evaluation. Keep existing [V1 rejection](ColdDamageBalanceReview.json) unchanged.
- A newly calculated CSV may be described as a **V2 candidate** if matched to these outcomes and explicit failures. It cannot be marked **accepted_for_xml** merely because source files exist or arithmetic is correct.

### Source identities

W13 Deryabin et al. 2024 *Metabolites* 14(4):199, primary Table 2: https://pmc.ncbi.nlm.nih.gov/articles/PMC11052526/

R02 Sagehashi et al. 2023 *Plant Production Science* 26:287–296: https://doi.org/10.1080/1343943X.2023.2236344

M01 in Lindsey et al. 2024 *Crop Science* 64: review of Buican (1969), DOI https://doi.org/10.1002/csc2.21212
