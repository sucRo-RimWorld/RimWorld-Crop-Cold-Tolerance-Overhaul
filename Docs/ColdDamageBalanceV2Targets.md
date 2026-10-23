# CCTO cold injury V2 — independently specified outcomes BEFORE selecting coefficients

**Current V3 direction update (author-approved 2026-10-23):** For MO cabbage (S04) and MO onion (S20), **survival with ordinary-HP harvest reduction takes priority over the historical hard-frost ≥70% mortality preference**. The original 2026-10-22 table below is preserved verbatim as a predeclared *historical* test, not the active gate. See the [current V3 replacement criteria](#v3-s04s20-supersession--author-approved-direction-2026-10-23). This paragraph records the earlier direction-only authorization; the **later 2026-10-23 author approval selected Cabbage A−3.5/D240 and Onion A−5/D120 as numeric V3 candidates** (not XML or variation), documented at the end.

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



### Extended targets recorded before new coefficients (2026-10-22)

These are **additional independent test conditions**. Their
gameplay survival inequalities are **review proposals**,
not published plant-mortality measurements. The
reference studies below provide comparative physiology
and expose where a measured **crop part** cannot be
mistaken for the **whole living Plant**.

| id | Owned plant / state | Exposure | Independent target | Basis and exclusion |
|---|---|---|---|---|
| S19 | MO **onion**, actively growing seedling | −2.5°C ×3h | **S≥0.95** | [Hokkaido onion agronomy paper](https://www.jstage.jst.go.jp/article/hortj/89/4/89_UTD-155/_html/-char/ja) cites Tanaka (1978): nearly all seedlings survived −2.5°C for 1–3h. Source is **secondary report** of Tanaka, not a measured lethal threshold for MO onion |
| S20 | MO **onion**, growing crop | −6°C ×8h | **S≤0.30** (gameplay only) | Severe/long frost ought to remain dangerous. Source only validates weak short frost, **not this numerical mortality** |
| S21 | MO **onion**, growing crop | −4°C ×8h followed by +5°C ×16h, 3 cycles | **S≥0.70** (gameplay only) | Avoid three ordinary frost nights wiping a crop without source evidence |
| S22 | MO **pumpkin**, warm-season growing annual | −1°C ×2h | **S≥0.80** (gameplay only) | Tender vegetable; not the same biological crop as tomato, and [FAO frost product rankings](https://www.fao.org/4/y7223e/y7223e0a.htm) concern harvested produce rather than living-vine mortality |
| S23 | MO **pumpkin**, growing annual | −3°C ×8h | **S≤0.30** (gameplay only) | A severe freeze should be dangerous, without assuming the produce's damaged skin means root/crown death |
| S24 | MO **pumpkin**, growing annual | −1°C ×8h then +10°C ×16h, 3 cycles | **0.05≤S≤0.90** (gameplay only) | Require genuinely gradual multi-night crop loss; derived independently from S22/S23 |
| S25 | Vanilla **strawberry**, dormant mature crown | −5°C ×12h | **S≥0.95** | [AHDB/ADAS](https://potatoes.ahdb.org.uk/knowledge-library/strawberry-cold-injury-providing-protection) reports short −5°C freezes not damaging dormant crowns. **Not** a non-dormant seedling, flower or harvested fruit mortality measurement |
| S26 | Vanilla **strawberry**, established crown | −10°C ×12h | **S≥0.70** (safety target only) | [OSU Extension](https://extension.oregonstate.edu/catalog/ec-1307-growing-strawberries-your-home-garden) documents dormant crown injury dependent on hardening/weather around −6.7 to −12.2°C **at the crown**, not ambient air. Requires dormancy/tissue temperature context; no universal whole-plant dose yet |
| S27 | Vanilla **potato**, vegetative mature plant | −2°C ×2h | **No fixed whole-plant mortality criterion** | [Ontario agriculture](https://www.ontario.ca/page/freeze-protection-methods-crops) says freezing potato tops may cause partial tuber yield loss, not automatically whole-plant death. Keep numeric **blocked**, do not fit leaf/tuber damage to lethal Plant disappearance |
| S28 | Vanilla **pine** / MO Great Fir, wild mature evergreen | −35°C ×12h followed by −30°C ×12h for **60 days** | **S≥0.95**, gameplay ecosystem safety proposal | Two quadrums of identical extreme cold are a **stress condition**, not documented normal RimWorld biome weather; no −30°C recovery is allowed |
| S29 | Same wild evergreen, with occasional thaw | Four cycles −35°C ×12h/−30°C ×12h, then one day +5°C ×24h; repeat for 60 days | **S≥0.95**; repair occurs only during thaw | Verify a winter-thaw spell can reduce existing injury without fictional subzero tissue repair |
| S30 | MO Onion and MO Pumpkin | Single -6°C ×8h followed by +8°C ×16h then another hard freeze | **Deaths must remain irreversible**; only live plants recover damage | Do not represent tissue repair as undoing an already dead plant |
| S31 | All wild/harvestable survival groups | 15 and 60 day winter exposure | **Do not treat yield, flower bud injury, surviving crown, dormant stem and whole-plant death as interchangeable** | Mortality must be evaluated on an appropriate whole-plant endpoint; otherwise use explicit uncalibrated gameplay approximation or retain legacy compatibility |

**Dataset/method caveat:** short-stress "S≥..." values
are *not* sourced mortality confidence limits except
where the statement matches the cited experiment.
The 60-day forest thresholds are design objectives
without actual temperature-frequency evidence.
Before selecting any further coefficients, preserve
species/organ/stage and source unit distinctions
in [the V2 assessment](ColdDamageBalanceV2Assessment.md).
Do not declare all 48 crops evaluated by analogy.
    
### Source identities

W13 Deryabin et al. 2024 *Metabolites* 14(4):199, primary Table 2: https://pmc.ncbi.nlm.nih.gov/articles/PMC11052526/

R02 Sagehashi et al. 2023 *Plant Production Science* 26:287–296: https://doi.org/10.1080/1343943X.2023.2236344

M01 in Lindsey et al. 2024 *Crop Science* 64: review of Buican (1969), DOI https://doi.org/10.1002/csc2.21212

## V3 S04/S20 supersession — author-approved direction, 2026-10-23

The author approved changing **only** the cabbage/onion balance priority from "strong frost should kill at least 70% of plants" to **"survival first, frost HP loss and reduced harvest yield"**. This supersedes the former game-only S04 `S≤0.30` and S20 `S≤0.30` mortality thresholds as **current V3 acceptance requirements**. Preserve the original dated rows above for audit; do not misrepresent those numerical death preferences as controlled agronomic mortality findings.

| Current V3 ID | Representative exposure retained from historical scenario | Active qualitative review criterion | Distinct failure mode / non-criterion |
|---|---|---|---|
| S04-V3 (MO Cabbage) | −8°C for 24 game-hours, from previously undamaged growing plants | Prefer plants **to survive** the episode and suffer **meaningful ordinary-HP / harvest-yield reduction**; prolonged or repeated deeper frost can still eventually kill | Avoid treating winter-useful cabbage as universally fragile; **do not require** 70% immediate crop death. No numerical minimum survival rate or yield reduction percent is approved |
| S20-V3 (MO Onion) | −6°C for 8 game-hours, from previously undamaged growing plants | Prefer plants **to survive** the episode with some ordinary-HP injury and harvest-yield penalty; recurring marginal frost (S19/S21 context) should not cause disproportionate loss | Do not force D18 merely to cause 70% immediate death. **Do not require** 70% mortality. No numerical minimum survival rate or yield reduction percent is approved |

The tomato/pumpkin severe-frost roles S02/S23 are **not changed by this decision**; neither their temperature/duration nor any published plant values are amended. S03/S05 and S19/S21 provide separate mild/repeated-cold context but legacy wording that requires CCTO-specific HP healing is already superseded by the approved **no cold-only recovery** architecture. Vanilla `Plant.YieldNow()` uses ordinary HP; damaged survivors lose yield without adding a second crop-yield system.

**2026-10-23 subsequent author A/D-candidate decision:** Following independent comparisons in `Docs/ColdDamageHpV3YieldReview.md`, **Cabbage A−3.5°C/D240°C·h** and **Onion A−5°C/D120°C·h** were expressly **selected as V3 numerical candidates**, superseding the historical D160 cabbage and A−3.5°C onion proposals. This is **not** an approval of their `lethal_dose_variation=0.20`, empirical LT50 precision, native game acceptance or production XML activation. S04-V3 and S20-V3 are still *qualitative* survival/yield criteria. Do not use the superseded historical ≥70% death gate for these two crops.
