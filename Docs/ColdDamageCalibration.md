# CCTO — empirical calibration of cumulative cold damage

**Reviewed 2026-10-21. Status: empirical-source verification in progress. No numeric parameters are approved for the new injury model; no Vanilla/MO, AMJ or VPE plant XML is migrated.**

CCTO is a generic realism-oriented plant tolerance framework, not a patch for
a Japanese double-cropping exception. Its numerical values must be traceable
to botanical/agricultural evidence and must not be invented to achieve a
convenient survival time.

## Correction of the earlier 12-row candidate table

The previous revision of this file proposed unverified onset temperatures,
lethal doses, recovery rates and synthetic 'hours until death' for 12 crops
(rice, corn, potato, tomato, pumpkin, cabbage, wheat, strawberry, pine, teak,
lemon and cocoa). **WITHDRAWN AS CALIBRATION VALUES.** The table was an
illustrative numerical experiment, not a plant-specific measured dataset; its
numbers must not enter XML. Git history retains that discarded exercise.

The prior statement that maize's two-temperature duration experiment
establishes a universal quadratic model was too strong. Two 50%-mortality
points cannot independently determine injury onset, severity exponent,
treatment-stage response and individual variation. In particular, the
study endpoints are *approximately 50% population mortality*, whereas the
draft CCTO code causes *deterministic death* of every identically exposed
plant when cumulative damage reaches 1.0. These are not interchangeable.

The current PR #24 code still uses a **linear** temperature-degree-hour
accumulator (°C × game-hour). A **quadratic** accumulator (°C² × game-hour)
was suggested but is NOT selected, empirically fitted or implemented.
Changing the equation also changes dose units. Neither curve has passed
source-level numerical calibration. **Do not promote the Draft PR or run
meaningless acceptance tests against guessed species values.**

## Verified source measurements — their endpoints are different

Read each linked original paper or institutional summary *with its biological
endpoint and protocol*. A reported LT50 may mean 50% whole-plant mortality,
50% leaf injury, or 50% electrolyte leakage, depending on the study.

| Plant / literature | Treatment or observation | Published outcome | What CCTO may infer | Why it is NOT a death setting |
|---|---|---|---|---|
| Maize, Buican (1969) via Lindsey et al. (2024) | Post-emergence freezing; −2°C for 48 h versus −6°C for 2 h | Each treatment produced about **50% seedling mortality**, assessed 10 days after freezing | Both temperature and duration strongly affect death likelihood | Not 100% mortality; specific emergence stage, conditions and cultivar context |
| Winter wheat, Skinner & Garland-Campbell (2008), USDA ARS | Fully cold-acclimated seedlings held frozen at **−5°C for 15 or 20 weeks** | 20-week survival ranged **0–33%** across cultivars | Long-term survival can extend far beyond hours even below freezing | Acclimated winter wheat, not generic wheat; exposure at plant/crown conditions, not a universal air-temperature threshold |
| Winter wheat, Skinner et al. (2018), PLOS ONE | Twelve winter wheat lines after **5 weeks cold acclimation** | Norstar's reported LT50 **−18.9°C** versus **−17.1°C** at different light-cycle sampling phases | Even LT50 varies with physiological conditions | These are 50%-mortality estimates under a specific laboratory protocol, not exact onset temperatures |
| Winter / facultative barley, Wójcik-Jagła & Rapacz (2023), Scientific Reports | Three-week acclimation at **4/2°C** day/night; later seven-day deacclimation at **12/5°C** for some plants; freezing **−2 to −12°C for 2 h** | LT50 changed significantly after deacclimation and differed among accessions | Acclimation/deacclimation and cultivar differences must not be erased | No unique barley threshold; protocol spans different prior states |
| Cabbage, GB frost-tolerance study (2021) | Three-week-old cultivar 'Myeong-Sung'; frozen leaf samples and electrolyte-leakage curve | Leaf injury LT50 approximately **−2.9°C** | Cabbage leaves can be injured at these conditions | Leaf tissue electrolyte leakage is not whole-plant death |
| Cabbage, NARO experiment (1997) | Two- to three-leaf seedlings; **5°C** acclimation; leaf samples assessed at **−8°C** | Acclimation improved leaf freezing tolerance; effect modified by soil water and deacclimation | Acclimation and organ-specific injury are important | Injury was assessed as electrolyte leakage, not a universal plant mortality point |
| Rice, da Cruz et al. (2013) review | Observed responses across developmental stages | Roughly **10–13°C** early-stage chilling injury versus **18–20°C** reproductive-stage injury | Nonfreezing cold can reduce growth or cause grain sterility; timing matters | These are *injury/yield* thresholds, not freezing death, and japonica/indica differ |
| Citrus, Plant Physiology (1989) | Field Valencia orange leaves across winter; also chamber-acclimated potted plants | Leaf LT50 varied **−2.8 to −3.8°C** in field; about **−6°C** after 10°C chamber acclimation | Citrus freezing tolerance depends on environment and acclimation | Orange leaf results cannot simply become MO lemon whole-tree death |

### Exact sources

1. **Corn** — Lindsey et al. (2024), *Crop Science*, DOI
   [10.1002/csc2.21212](https://acsess.onlinelibrary.wiley.com/doi/10.1002/csc2.21212),
   §2.1.2; explicitly attributes the 48 h / 2 h mortality experiment to
   Buican (1969). The review also says nonlethal injury across these exposure
   conditions has **not** been quantified.
2. **Prolonged frozen winter wheat** — Skinner & Garland-Campbell (2008),
   *Canadian Journal of Plant Science* 88:885–889, DOI
   [10.4141/CJPS08007](https://www.ars.usda.gov/research/publications/publication/?seqNo115=221641);
   USDA-ARS peer-reviewed publication summary.
3. **Wheat LT50 after acclimation** — Skinner et al. (2018),
   *PLOS ONE* 13:e0198042,
   [PMC6005534](https://pmc.ncbi.nlm.nih.gov/articles/PMC6005534/);
   Table 2, LT50 of 12 winter wheat lines.
4. **Barley acclimation** — Wójcik-Jagła & Rapacz (2023),
   *Scientific Reports* 13:19931,
   [doi:10.1038/s41598-023-47318-y](https://www.nature.com/articles/s41598-023-47318-y);
   methods and Fig. 5.
5. **Cabbage leaf LT50** — *Exogenous Glycine Betaine Application
   Improves Freezing Tolerance of Cabbage Leaves* (2021),
   [PMC8703899](https://pmc.ncbi.nlm.nih.gov/articles/PMC8703899/);
   §2.2, leaf electrolyte leakage. The indexed abstract/full-text excerpt
   was accessible, but the PMC full page intermittently returned a captcha;
   verify treatment details again before numerical fitting.
6. **Cabbage cold acclimation** — NARO (1997),
   [低温時の土壌水分制限によるキャベツ幼植物の耐凍性向上と糖蓄積](https://www.naro.go.jp/project/results/laboratory/vegetea/1997/vegetea97-004.html).
7. **Rice chilling stage dependence** — da Cruz et al. (2013),
   *Food and Energy Security* 2:96–119, DOI
   [10.1002/fes3.25](https://onlinelibrary.wiley.com/doi/10.1002/fes3.25);
   temperature bands are cold **damage**, not whole-plant mortality.
8. **Citrus leaf tolerance** — *Freezing tolerance of citrus, spinach,
   and petunia leaf tissue* (1989), *Plant Physiology*, DOI
   [10.1104/pp.89.2.444](https://pubmed.ncbi.nlm.nih.gov/16666563/).

## What a numerical setting must document

Before a species is approved, record: `DefName`; represented biological
species and (where possible) cultivar / spring-or-winter growth habit;
growth stage; acclimated/deacclimated state; plant organ; environmental
temperature metric (**air / leaf / soil / crown / water**); freeze protocol
(minimum temperature, ramp, exposure duration, thaw and recovery); result
measure (**whole-plant mortality %, survival %, leaf injury %, yield loss %
or dormancy**); source/DOI; evidence grade; and extrapolation limitations.

- **Direct** — specific species, defined exposure and **whole-plant survival
  or mortality** in peer-reviewed measurements; suitable for a survival
  calibration point **only under the reported conditions**.
- **Indirect** — same species but injury/yield/leaf-only metric, review
  summary, incompatible season or part, or closely related species; useful
  for bounding expectations, **not** directly for lethal dose.
- **Unverified** — a handbook range lacking protocol, a fictional species,
  or an unsupported inferred value; no claim of measured equivalence.
  Gameplay-only values must be conspicuously flagged, not passed off as fact.

A 50%-mortality temperature `LT50` is a **population statistic**, not a
deterministic threshold where every plant dies. Before parameter fitting,
decide whether CCTO will allow individual susceptibility/plant-state
variation or intentionally approximate population outcomes with an abstract
single-plant rule. If the latter, explicitly record that loss of fidelity.

## Game time and sampling: separate real biology from RimWorld abstraction

RimWorld uses **60 days per year** versus about 365 calendar days in
biological literature; one game day still has 24 game hours (2,500 ticks
per hour). Hence **48 real hours must not silently become 48 game hours**
as an assumed empirical calibration when comparing seasonal overwinter
outcomes. Decide separately whether short weather events use same-hour
duration and how compressed annual winter exposure maps to the literature.
Do not scale both duration and damage rate without documenting the chosen
calendar model.

The draft engine reads instantaneous ambient temperature through the existing
plant cold-check callback and caps elapsed exposure at 2,000 ticks. It cannot
infer past short freeze spikes. For per-plant injury, the temperature history
must represent the planted location (for instance, heated greenhouse rather
than global outdoor temperature). Numerical fitting must tolerate or quantify
the effect of this time sampling rather than choosing an expensive every-tick
temperature audit by default.

## Inventory and immediate decisions

`Docs/ImplementationTable.md` on main lists **78 supported PlantDefs**:
**48** with finite instant-death thresholds and **30** dormancy-only.
These counts are inventory, **not** 78 verified injury measurements.

- **All 48 finite-threshold Defs** require a species-appropriate source
  record before automatic migration. No source-backed onset, dose and
  recovery triple is approved even for the eight source examples above.
- **All 30 dormancy Defs** need an ecological check before anyone assigns
  lethal low-temperature injury to perennial roots, woody crowns or
  cold-region natural vegetation. Some may just retain dormancy.
- AMJ Grains, Environment and CCTO-for-VPE own their own plant balance;
  CCTO cannot populate those XMLs solely because a generic curve exists.
- No fixed one-exponent law (linear or quadratic), dose, universal recovery
  constant, arbitrary cold-acclimation bonus, or snow-temperature bonus is
  currently certified by the cited data.

**Correct order:** (1) compile primary source measurements by species /
state / endpoint; (2) decide a minimally complex model that fits several
independent organisms and exposure regimes; (3) derive and audit plant
parameters with explicit uncertainty; (4) update actual owning XML and
documentation; (5) only then minimal real-game acceptance and winter-map
performance measurements. No native test automation is authorized on
PR/push, and PR #24 remains Draft.


## Practical-fit criteria (author clarification 2026-10-21)

The goal is a **realistic approximation**, not a claim that all 78 PlantDefs
obey one universal physical injury equation. The exact same numerical
severity curve is not required for chilling-sensitive tropical crops,
weakly frost-tolerant annuals, winter-hardy cereals, woody perennials and
winter-dormant vegetation. Still, a small number of reusable response
profiles and species-specific configuration should be preferred over an
unmaintainable, hand-coded formula for each crop.

**Fit a profile against bounded observable outcomes:**
1. **Exposure severity:** weak frost should not routinely kill a plant
   sooner than substantially colder exposure when holding its biological
   state and duration equal.
2. **Time course:** match the *order of magnitude* and plausible range of
   survival time at multiple treatment temperatures; include whether the
   source endpoint is 50% *whole-plant* mortality or only tissue injury.
   Do not claim a measured accuracy or introduce a global percentage
   tolerance unless supported by independent validation data.
3. **Condition dependence:** compare seedlings vs established plants,
   acclimated vs unacclimated plants, and dormancy where those distinctions
   change decisions. Add dynamic physiological state only for meaningful
   mismatches, not because it appears in a study.
4. **Ecological plausibility:** test repeated ordinary winters against
   typical local biomes, especially naturally spawning trees, shrubs and
   wild grasses. Distinguish survival from damage to leaves, buds and yield.
5. **Evidence grades:** direct species-specific whole-plant outcomes can
   constrain survival probabilities; related species and organ-only data
   bound estimates but cannot set a universal lethal parameter. Record
   the source, represented organism, proxy reason, endpoint and uncertainty.
6. **Runtime budget:** reuse existing plant update hooks, use a small
   precomputed per-Def profile or coefficients, store state only when
   needed and avoid per-Tick full-map scans. Reject complexity with no
   material improvement in representative survival outcomes.

**Result classification:**
- **Empirically constrained:** relevant whole-plant survival/mortality
  measurements across multiple known conditions (with stage/state noted).
- **Evidence-supported approximation:** related taxon, organ-only or
  narrower-stage evidence with stated limitations and conservative bounds.
- **Gameplay/ecology estimate:** insufficient biological data or fictional
  plant, explicitly labeled; never present its numeric value as a measured
  field tolerance.

**Calibration approach:** select representative plants from each
functionally distinct response class, assess whether one existing curve
can cover that class with species-specific coefficients, then fit the
rest by direct evidence or a documented ecological proxy. Use another
profile **only when it substantially improves realistic outcomes**.
The prior 12-row ad hoc values remain withdrawn. No dose units are chosen
until the profile family is selected; no per-species parameters are
finalized in this note.


## Plant-by-plant source ledger (2026-10-21)

[ColdDamageEvidenceLedger](ColdDamageEvidenceLedger.md) now includes one row
for each of the **78 CCTO-owned PlantDefs** (48 finite-death, 30 dormancy),
with source IDs, original links, biological endpoints, and deliberate
unmapped gaps. The initial evidence inventory contains **53 source-mapped
Defs** (including nontransferable related-species/organ evidence) and
**25 without a mapped adequate reference**. The mapping is not evidence
that 53 plants are numerically calibrated. Published FAO frost *damage*
ranges are not whole-plant lethal values; postharvest product freezing
temperatures are excluded. Saguaro, Agave and sugarcane reveal substantial
species/organ/stage/duration effects, so do not migrate numerical settings
from one generic group label to all plants in that group.

**Next scientific pass:** map the remaining Def identities to actual taxa
before claiming species-specific values, collect whole-plant mortality or
post-freeze regrowth endpoints where possible, and bound group-level values
only when evidence supports the analogy. No new CCTO XML values are approved.


## New primary controlled-survival evidence: onion (2026-10-21)

The [species evidence ledger](ColdDamageEvidenceLedger.md) now includes
**ON03: Peffley, Ortiz & Corgan 1981** ([DOI](https://doi.org/10.21273/HORTSCI.16.6.773);
[full original two-page paper](https://pdfs.semanticscholar.org/4f3a/3c0c6125529759251b863550fb12b392e020.pdf)).
The original article, Table 1 and survival-curve Figure 1 were examined
rather than relying only on a university breeding-project summary.

- Species: *Allium cepa* 'New Mexico Yellow Grano' and 'Yellow Granex'.
- Material: **58-, 74-, and 90-day seedlings**, harvested after autumn
  field establishment; leaves clipped and roots shortened.
- Treatment: **-4°C overnight followed by about 1°C/hour cooling**;
  replicate groups of 25 plants were removed at each one-degree interval
  from -6 to -14°C and thawed.
- Endpoint: seedlings transplanted into a warm greenhouse; **root
  regeneration after three weeks** determined *whole-plant survival*.
  A visually intact shoot was NOT accepted as survival when its stem plate
  had died.
- Original LT50 (Table 1): **-8.7°C at 58 d, -10.5°C at 74 d, -10.8°C
  at 90 d**; abstract rounds the 58 d result to -8.8°C. Figure 1
  provides survival percentages against final freeze temperature.
- **Secondary-source conflict:** NMSU project page (ON01) says about
  -14°C LT50 for December seedlings of Yellow Grano and Granex. It does
  not agree with ON03, so do not transfer that figure to CCTO or attribute
  it to this verified primary experiment without reconciling sources.

**Interpretation:** valuable population-level *survival* data with
plant-age differences; unlike many prior sources, this study measures
post-freeze regrowth and not merely electrolyte leakage or frost damage
to a leaf or bud. It is **not a duration-response trial at fixed
temperatures**: the rate and total elapsed freezing time change with
the temperature reached. A single set of measured endpoints therefore
cannot independently fit injury onset, exposure-time exponent and
recovery speed. Keep new CCTO numeric parameters unapproved until
independent observations or a clearly documented approximation are
available.

**Taxon gate update:** `Plant_Tinctoria` is an explicitly engineered,
fictional dye plant, not a known *Isatis tinctoria* clone; `Plant_Rafflesia`
depicts a giant parasitic flowering plant, where visible flower death and
endoparasite death must not be equated. The separate taxonomic notes in
the ledger are **not** cold-treatment evidence and do not increase the
53/78 count of mapped cold references.


## Per-plant mortality variability fitting (2026-10-21)

**Direction accepted:** fit the *population survival fraction* rather than
killing every identically exposed plant at a shared lethal dose. The
species-specific onset and thermal-injury law remain empirical fitting
problems. A plant's **stable susceptibility** can vary its lethal
accumulated dose without rerolling each time the plant is checked.
Numerical spread is **unapproved**.

Only use **whole-plant mortality LT50** as the 50%-mortality reference.
LT50 from leaf electrolyte leakage, buds, fruit or biomass yield is
not interchangeable with whole-plant mortality. Experimental LT50 alone
fixes one quantile under a specified stage, acclimation and exposure
protocol; it **does not identify LT10/LT90, slope, or the standard deviation
of the per-plant susceptibility distribution**. Test candidate bounded
or positive-valued distributions against at least two additional survival
fractions or treatment conditions when available. Never infer a 2°C,
5°C or 10% arbitrary tolerance spread from a single published LT50.

When the model response to constant/falling temperature is nonlinear,
fit susceptibility against **total accumulated dose**, not a randomly
altered temperature threshold, initially; this keeps the label
`損傷開始温度` a meaningful species-level onset. A candidate
implementation uses each plant's persistent seeded identity to generate a
fixed rank u in (0,1) once, with its lethal dose equal to a calibrated
inverse-CDF transform. This is an implementation candidate only, not
an adopted probability law. No per-callback random draw, extra whole-map
tick loop or arbitrary save-breaking reseeding. Check cold-damage clamp
semantics (`Advance` currently clamps at 1) before introducing a
plant-dependent lethal limit.

Population-level validation after fitting should compare expected
survival proportions across uniform batches at matching doses and
multiple temperatures, check large-plant-batch synchronization, and
confirm fixed plant outcomes across saves. In-game single-plant
survival is not a proof of study-level LT50; where real data include
variety, age, acclimation or microclimate differences, state explicitly
how much the abstract distribution approximates each effect.

Relevant primary examples:
- [Boechera stricta freezing tolerance; whole-plant LT50](https://pmc.ncbi.nlm.nih.gov/articles/PMC4416058/)
  reports group-level 50%-mortality temperatures and variation among
  genetic lines; do not reuse its dispersion for unrelated crops.
- [Arabidopsis lyrata subsp. petraea natural cold tolerance](https://pmc.ncbi.nlm.nih.gov/articles/PMC6233594/)
  reports plant survival **14 days after freezing**, and different
  population-specific curves with LT50 among populations of approx
  -8.6°C to -6.2°C after 2-day acclimation; this is evidence of
  variable susceptibility and acclimation, *not* a universal variance.
- [Barley freezing LT50](https://www.nature.com/articles/s41598-023-47318-y)
  uses temperature which killed 50% of plants after specified
  acclimation/deacclimation, unlike LT50 electrolyte-leakage measures
  elsewhere.


## Empirical multi-point mortality curves — targeted review 2026-10-21

**Status:** source-backed *constraints*, not approved CCTO parameters or a
species-wide distribution. Evidence IDs refer to
[ColdDamageEvidenceLedger](ColdDamageEvidenceLedger.md) (V03–V05, ON03).
This section compares **whole-plant survival/recovery**, not leaf
electrolyte leakage. No model coefficients are changed in C# or XML.

| Source | Plant / physiological state | Exposure and follow-up | Whole-plant mortality / survival constraints | What is actually identified |
|---|---|---|---|---|
| `ON03` | *Allium cepa* Yellow Grano/Granex, 58/74/90-day field seedlings | Overnight -4°C, ~1°C/h cooling ramp, removed at different final temperature; surviving if root regeneration at 3 weeks | LT50 **-8.7/-10.5/-10.8°C** respectively (Table 1); Fig. 1 contains age-specific **probit-fitted percentage-survival curves** covering the transition from few survivors to almost all survivors | Within-age mortality slope is visibly nonzero; **age differences in LT50 are not a measure of within-age variation**. Individual curve quantiles can be digitized later with a documented reading error, not guessed from the diagram |
| `V03` | *Arabidopsis thaliana* Ler, Columbia, esk-1, frs-1, with/without cold acclimation | Whole potted plants frozen for **2h** at treatment temperature 0 to -15°C; judged alive if producing a new leaf within 10d | Original reports **LT10/LT50/LT90 from plant mortality**. Most genotypes have LT10/LT90 close to LT50 (roughly 1–2°C variation around it); **Ler** before acclimation or after 1 day is strongly **asymmetric**, LT10 ~-1.5°C while LT50 ~-7.5°C and LT90 ~-8.5°C | **Both slope and shape are conditional**: a universal symmetric/normal logistic thermal-threshold distribution fails to represent this exceptional treatment |
| `V04` | *Schedonorus arundinaceus* 2 cultivars, 1–2 leaf seedlings (n=1594), 5–6 leaf tillering (n=1845) | ~16h freezer experiment, 24h cooling chamber, then **14d greenhouse recovery**; plants rated alive/dead | Seedling LT50 **-5.7/-5.5°C**; tillered LT50 **-6.0/-5.7°C** (summer-active/SD). **Every sampled seedling survived ≥-4°C**, and **none survived ≤-8°C**. Source models individual binary outcomes with logistic regression | The finite-sample **observed transition band** spans at most the observed ~4°C endpoints, but these are *not* published LT10/LT90 and do not identify exact variance or universal tail bounds |
| `V05` | *Cicer arietinum* KAKA genotype, after ~4 months natural cold acclimation | Cooled at 2°C/h, ice nucleation induced at -2°C, held **2h** at each 0/-6/-10/-12/-14°C treatment, then **4 weeks recovery** | KAKA whole-plant survival **100 / 95 / 80 / 70 / 0%** at the five listed temperatures. In this experimental protocol, the 50% survival point falls **between -12 and -14°C** under monotonic interpolation (not an original reported decimal estimate) | A temperature–survival curve with a sharp final fall is directly observed. No second duration at the same temperature is provided, so it **does not determine severity exponent or time-dose parameters** |

### Reasoning and implications for CCTO

- **Biological reference is not the CCTO death time.** All four sources
  either cool through a ramp or impose one exposure duration, and follow
  survivors for different recovery intervals. Sampling a minimum air
  temperature without these conditions does not make its reported LT50 a
  game-ready parameter.
- **Separate state changes from random susceptibility:** onion age shifts
  LT50 by up to ~2°C; the fescue two developmental stages and Arabidopsis
  cold acclimation also shift cohort response. Moving the median between
  states cannot be substituted by simply widening one fixed
  per-plant distribution. The **width at a fixed stage and physiological
  state** is the only part initially modeled as stable individual
  susceptibility.
- **LT50 is only one percentile.** For sources publishing LT10/LT50/LT90
  of whole-plant death, estimate the shape/dispersion of mortality **under
  each source's exact protocol**. If only 0/50/100% are known as in V04,
  record them as **observed sample boundaries**, not genuine LT0/LT100
  of an infinite population. If source proportions jump abruptly as in
  V05, do not invent intermediate LT10/LT90 or force a smooth symmetric
  shape.
- **Two candidate simulation distributions, neither selected:**
  positive (e.g. lognormal/log-logistic) susceptibility in **lethal
  accumulated cold dose**, or a bounded/piecewise quantile mapping
  for profiles that require non-symmetric tails. Both can have median
  1.0 before normalization and produce stable per-plant thresholds.
  Median matching alone does **not** validate the tails, skew or width.
  Do not automatically transfer *Arabidopsis* or chickpea spread to
  wheat, corn, wild trees, or fictional plants.
- **Engineering simplification:** if the accepted model keeps
  `ColdDamageProgress.Advance` clamped at 1 and kills when injury
  reaches 1, encode per-plant lethal-dose variation as the **reciprocal
  sensitivity multiplier on each increment** (or equivalently normalize
  damage by that plant's dose first); a separate variable death threshold
  above 1 would conflict with the current clamp. Avoid extra per-tick
  random sampling. Stable identity/hash and save guarantees remain
  untested and must be confirmed before implementation.
- **What can be calibrated now:** species- and state-specific **relative
  mortality curves at the protocols above** and categorical response
  differences (symmetric/narrow vs asymmetric, flat vs rapid transition).
  **What cannot be calibrated yet:** a generic CCTO dose–mortality
  distribution for each of its 78 plants, universal variance width,
  acclimation kinetics, the winter-cold time exponent, or recovery
  speed. Preserve unresolved uncertainty rather than constructing
  invented precision or silently moving production XML.

**References:**
- Peffley et al. 1981: [original Figure 1 and Table 1](https://pdfs.semanticscholar.org/4f3a/3c0c6125529759251b863550fb12b392e020.pdf).
- Reyes-Díaz et al. 2006: [Oxford Academic, Fig. 2, LT10–LT90](https://academic.oup.com/jxb/article/57/14/3687/584622).
- Interrante et al. 2020: [Wiley article, Figures 1–2](https://acsess.onlinelibrary.wiley.com/doi/10.1002/ael2.20017).
- Nabati et al. 2026: [Springer Nature, Methods/Survival results](https://link.springer.com/article/10.1186/s12870-026-08352-w).


## Population-variation model shape audit (2026-10-21, exploratory, **not approved parameters**)

This is a **numerical comparison of survival observations**, not a fitted
thermal injury-over-time model. The key question is whether a symmetric
two-parameter susceptibility/temperature CDF can fit an observed
survival-versus-final-freeze-temperature curve. Source IDs and full direct
links are retained in `ColdDamageEvidenceLedger.md` (V03–V05, ON03).

### Whole-plant evidence with multiple quantiles

- **V03 (*Arabidopsis thaliana*, Ler, unacclimated or 1-day cold
  acclimation):** the original authors report LT10 around **-1.5°C**,
  LT50 around **-7.5°C**, LT90 around **-8.5°C** (rounded results).
  On the temperature axis, the 10%-to-50%-mortality distance is
  **~6°C**, but the 50%-to-90%-mortality distance is **~1°C**.
  A normal/probit or single-scale logistic distribution **cannot**
  simultaneously match those three quantiles because their equal-probability
  tails have equal distances from the median. Many other source genotypes
  do have approximately symmetric, relatively narrow LT10/LT90 around
  LT50 (often of order 1–2°C each side). Do not make all plants share
  the unusual Ler distribution.
- **V04 (tall fescue):** the original investigators themselves used a
  logit-link binary-survival regression and reported seedling LT50 values
  -5.7/-5.5°C, with **observed** all-survived above -4°C and all-died
  at or below -8°C in finite samples. The specific fitted logistic
  slope/temperature-level sample counts cannot be recovered from the
  article text alone; those bounds do **not** establish true
  population LT0/LT100 or exact LT10/LT90.
- **ON03 (onion):** Fig. 1 shows age-specific curves fitted by probit,
  but age-dependent differences among -8.7/-10.5/-10.8°C medians
  **do not** establish within-age spread; curve slopes/quantiles must
  be read from raw source data or figure with stated digitization error.
  It uses cooling ramps, not fixed-temperature time-dose treatments.

### V05 (*Cicer arietinum*, KAKA): reproducible five-point comparison

Nabati et al. (2026) directly state these survival observations for one
cold-acclimated genotype, after cooling 2°C/h to the treatment
temperature, holding 2h, and observing regrowth four weeks later:

| Final treatment °C | 0 | -6 | -10 | -12 | -14 |
|---|---:|---:|---:|---:|---:|
| Observed survival % | 100 | 95 | 80 | 70 | 0 |
| **Exploratory two-parameter logistic** % | 100.0 | 100.0 | 99.0 | 68.3 | 4.4 |
| **Exploratory two-parameter normal CDF/probit** % | 100.0 | 100.0 | 100.0 | 69.8 | 1.1 |

The two-parameter survival functions were fit by **unweighted least
squares to the five published rounded percentages**, not as binomial
maximum-likelihood models:

```text
logistic: S(T) = 1 / (1 + exp(-(T-m)/s))
probit:   S(T) = Phi((T-m)/s)

with T in Celsius and s>0.
logistic estimated m = -12.399 °C, s = 0.520 °C
probit estimated m = -12.368 °C, s = 0.709 °C
```

The logistic curve misses the observed **80% at -10°C** by about
**19 percentage points**; probit misses by about **20 points**.
Root-mean-square survival discrepancies across the five quoted
temperatures are **9.0/9.2 percentage points** (logistic/probit).
These numbers illustrate a **specific fit failure**: a conventional
smooth symmetric CDF cannot reproduce the relatively gradual
0→-12°C decline and the sudden -12→-14°C drop at the same time.
Do **not** convert the logistic m or s to CCTO species values,
or call the residual a cross-validated prediction error.

**Major uncertainty:** the article describes 3 replicates and
3-plant biological replicate samples for several biochemical assays,
but does not clearly provide the **denominators and treatment-level
variance for each listed survival percentage**. In particular a
literal 95% cannot be exactly computed from just 9 binary plants.
Thus the displayed percentages can be compared geometrically but
not fitted with defensible binomial uncertainty bands or formal
distribution-model selection without the full underlying survival
counts. The reported 0%/100% are **sample outcomes**, not guaranteed
mathematical support bounds.

A piecewise monotonic interpolation can pass through all five points
**by construction**, but that is not evidence that its shape
generalizes between temperatures, to other genotypes, or to varying
exposure durations. An extra asymmetry parameter can also improve
in-sample fit but risks simply overfitting this five-point pattern.
Reserve such flexibility for repeatable independent profiles with
reported precision.

### Design-stage decision supported so far

- **Retain** a simple symmetric, positive-valued stable susceptibility
  distribution as the **default candidate**, because it is cheap
  and often adequate for tight symmetric study curves.
- **Keep** an asymmetric or bounded/piecewise response **as a
  conditional candidate** for cohorts whose reliable survival
  observations materially contradict a symmetric distribution.
- **Do not fix** any distribution family, width, tail cutoff,
  or species dose at present. The first useful calibration gate
  is a second independent curve (known sample counts, cultivar,
  acclimation and exposure history) for a plant class CCTO
  actually owns. More data are preferable to extra fitting knobs.
- This fits **population mortality as a function of observed
  treatment minimum temperature**, *not* lethal accumulated cold
  dose across arbitrary winter hours. Conversion to the
  accumulating injury engine additionally needs time–temperature
  observations. A one-duration mortality slope is not itself a
  duration-damage exponent.
- An LT50 plus calibrated *population survival curve* can anchor
  individual variation; an LT50 alone cannot. CCTO's
  `損傷開始温度` remains a species-level onset, not a
  per-individual predicted death temperature.

**Direct methods/results:**
[V03 original paper](https://academic.oup.com/jxb/article/57/14/3687/584622),
[V04 tall-fescue methods and logistic model](https://acsess.onlinelibrary.wiley.com/doi/full/10.1002/ael2.20017),
[V05 chickpea five percentages and recovery protocol](https://link.springer.com/article/10.1186/s12870-026-08352-w),
[ON03 onion original Table 1/Figure 1](https://pdfs.semanticscholar.org/4f3a/3c0c6125529759251b863550fb12b392e020.pdf).


## Winter wheat duration × severity studies (2026-10-21; new direct CCTO-owned analogue)

Three more primary papers (ledger IDs `W03`–`W05`) examine
**whole-plant survival/mortality under more than one duration**, a
better match to CCTO's requested dose mechanism than a single LT50
experiment. **No actual parameters are adopted.** Source outcomes
apply to acclimated winter wheat and are **not an identification
of the generic MO Wheat Def as winter-hardy wheat**.

| Study | Exposure and measured outcome | What it constrains | What it does not prove |
|---|---|---|---|
| `W03` Skinner 2014, *Crop Science* 54 | Six cultivars, -13.5 to -16.5°C for 2/3/4h; average survival dropped by about 50% as time **below -10°C** increased 325→525min, but **rose again** after about 675min to roughly the 325min survival level | Winter wheat's survival is not always a simple monotonic cumulative harm function of time; paper suggests temperature-triggered rapid hardening in some circumstances | Not an exact recovery rate, a generic crop rule, or evidence that every plant heals continuously while frozen; the full experimental tables and source diagnosis remain open |
| `W04` Zheng et al. 2018, *Crop Science* 58 | Four field-acclimated winter wheat cultivars held for increasing days at their **respective critical frost temperature**; paper reports 8.6, 22.3, 11.1 and 9.4 **% per additional day** plant mortality changes by cultivar | Duration sensitivity varies strongly *between cultivars*, even in one winter-cereal species. Whole-plant death differs from tiller and leaf damage | The publication's abstract does not resolve percent relative vs **percentage-point** absolute changes, exact treatment denominators or each critical temperature; do not use the 4 slopes as per-tick lethal rates |
| `W05` Zheng et al. 2018, *Agricultural and Forest Meteorology* 260–261 | Two-year controlled study: **soil minima** -14/-17/-20°C at differing 1–10-day exposures; proposes freezing degree-days below a **soil minimum -12°C** threshold. Reports plant/tiller mortality increases **1.0/1.4 % per °C·day** respectively | A source-tested thresholded accumulated-cold metric exists for **acclimated winter wheat in soil-temperature space**; provides an empirically motivated candidate response family | **-12°C is not the universal damage-start threshold of wheat**; soil-temperature degree-days cannot be copied directly to RimWorld's plant `AmbientTemperature`, and abstract effect sizes are not a plant-level CDF or per-individual lethal dose |

**Important:** `W03` demonstrates an apparent **non-monotonic**
survival-time curve. A pure incremental damage model with
`damage += nonnegative dose`, no acclimation during frost,
and fixed susceptibility *necessarily* predicts nonincreasing
survival with longer uninterrupted exposure at identical treatment
conditions. No static distribution can fix that exception:
variation changes *which* plants die, not the monotonic property.
A selective physiological-state/acclimation mechanism could
approximate this finding, but implementing it solely to reproduce
one counterintuitive study without independent verification would
add extra state and TPS costs. **Do not automatically select it now.**

**Practical model constraints after adding W03–W05:**

1. A light common frost and a winter-hardy wheat stand's
   root/crown exposure are **not the same temperature signal**.
   `W05` is measured using daily **soil** minima, so a game
   using instant above-ground ambient temperature must document
   the difference and test it against actual biome temperatures.
2. A cold accumulated-dose profile (possibly with a threshold)
   is a credible **plant-class candidate** for overwintering cereals,
   not a universal law. `W05` suggests a linear frost-degree-day
   relation for the *measured range*, whereas other classes may
   need a steeper severity curve.
3. Distinguish **within-cohort fixed susceptibility** from
   **changing cold hardiness** across cultivar, growth stage
   or acclimation, and from a transient cold-triggered response
   as discussed in `W03`.
4. Do not infer plant-level lethal-dose distribution width from
   regression of **population mean mortality vs duration**.
   Determine baseline severity/time response first; then match
   population LT10/LT50/LT90 or survival proportions under
   equivalent controlled treatment.
5. Source access: W03 and W04 primary journal **abstracts**
   were accessible, W05 abstract via journal indexing/FAO AGRIS.
   **Full original tables and uncertainty were not retrieved** in this
   pass. The figures above are original abstract descriptions,
   not freshly reanalysed raw survival data. No numerical
   parameter fitting is authorized from those summaries alone.

**References:**
- W03 [Skinner (2014) DOI 10.2135/cropsci2013.09.0623](https://acsess.onlinelibrary.wiley.com/doi/10.2135/cropsci2013.09.0623).
- W04 [Zheng et al. (2018) DOI 10.2135/cropsci2018.01.0014](https://acsess.onlinelibrary.wiley.com/doi/10.2135/cropsci2018.01.0014).
- W05 [Zheng et al. (2018) DOI 10.1016/j.agrformet.2018.05.011](https://doi.org/10.1016/j.agrformet.2018.05.011).


## W04 source-regression reconstruction — exact published Tables 2/4/5, 2026-10-21

**New source access:** the *Crop Science* 58:1740–1750 (2018)
Zheng et al. W04 article is available as [article text/embedded table
transcription hosted at Academia.edu](https://www.academia.edu/120431829/Tolerance_of_Different_Winter_Wheat_Cultivars_to_Prolonged_Freezing_Injury_at_their_Critical_Temperatures).
The [publisher DOI](https://doi.org/10.2135/cropsci2018.01.0014)
was checked; **full publisher download was not available** in this pass.
The hosted source contains the textual content of Tables 2, 4, 5 and
experimental protocol. These results are author-reported *fitted
regression outputs*, not the individual raw observations from Figure 3.

**Source conditions, essential for reproduction:** 3 years of
field-acclimated winter-wheat seedlings, 10–15 plants/pot, 3 replicates
per treatment, natural autumn growth and transfer to freezing chambers.
Temperature was monitored **2.5 cm inside soil**, with daily ramp
at ~1°C/h to a genotype-specific soil minimum (3 h held) and back up to
the programmed **soil maximum** (3 h held). One experimental freezing
'day' is a **cycled treatment day**, NOT 24 h at the minimum
temperature. Plants thawed over 2 days around +4°C and were then
regrown in a greenhouse; the counted endpoint was whole plants with
surviving growing tillers/leaves. Dead leaves and dead tillers were
analysed separately, so do not substitute their death rates.

### Original fitted values for whole plants only

| Cultivar/type | Soil min/max °C | Source Table 4 K (d) | Source P (conditional death per effective day) | Source R² | Source Table 5 LD10 / LD20 / LD50 (d) |
|---|---|---:|---:|---:|---|
| Jing411 / strong winter | -17 / -8 | 1.4 | 0.086 | 0.963 | 2.5 / 3.8 / **8.7** |
| Nongda211 / winter | -17 / -8 | 1.6 | 0.223 | 0.863 | 2.0 / 2.5 / **4.4** |
| Zhengmai366 / semi-winter | -14 / -5 | 0.3 | 0.111 | 0.665 | 1.2 / 2.2 / **6.2** |
| Yanzhan4110 / weak spring | -11 / -4 | 2.3 | 0.094 | 0.670 | 3.4 / 4.7 / **9.6** |

**These LD values are calculated by the source authors' mortality
regressions, not independent measurements** of LD10/LD20/LD50.
This distinction is critical: reproducing Table 5 with Table 4 is an
**internal reconstruction**, not cross-validation against held-out
individual trial data.

The original source fits a shifted survival process:

```text
n = elapsed experimental freeze-cycle DAYS
K = cultivar-specific ineffective freezing duration (days)
P = mortality fraction among SURVIVORS per additional effective freeze day

M(n) = 1 - (1 - P)^(n-K)         [for n>=K]
For safe extrapolation to n<K, set M(n) = 0, unverified below-data boundary.

S(n) = 1 - M(n)
h = -ln(1-P)                    [positive daily cumulative hazard]
LD_q = K + ln(1-q) / ln(1-P)    [days to q fraction dead, n>=K]
```

**Numerical reconstruction from rounded published K/P**, distinct from
the original rounded Table 5 values:

| Cultivar | Recomputed LD10 | LD20 | LD50 | Modelled surviving after 3 freeze days | After 5 days | After 8 days |
|---|---:|---:|---:|---:|---:|---:|
| Jing411 | 2.57 | 3.88 | 9.11 | 86.6% | 72.3% | 55.2% |
| Nongda211 | 2.02 | 2.48 | 4.35 | 70.2% | 42.4% | 19.9% |
| Zhengmai366 | 1.20 | 2.20 | 6.19 | 72.8% | 57.5% | 40.4% |
| Yanzhan4110 | 3.37 | 4.56 | 9.32 | 93.3% | 76.6% | 57.0% |

The largest Table 5 mismatch of the reconstructed LD50 is
**~0.41 days** for Jing411 (9.11 versus 8.7), followed by
Yanzhan4110 (~0.28 days); the source table displays *rounded*
K/P parameters and the original regression may have used
additional digits. **Do not silently tune or claim that the
published regression and all original LD values agree exactly.**
Original sample measurements would be needed to assess *predictive*
fit and mortality uncertainty, which are not recoverable from
the text table alone. The R² are the **authors' original per-plant
mortality model fits** (not validation scores for CCTO).

**Reproducible dataset:** [WinterWheatW04Regression.csv](Research/WinterWheatW04Regression.csv)
retains all 4 source Table 2 temperatures, Table 4 K/P/R², Table 5 LD
values and separately labelled deterministic recalculations. The
CSV comments preserve measurement endpoint and treatment protocol.

### What the reproduction proves — and does not

1. **A practical stable individual threshold implementation exists for
   the population survival equation at each specific experimental cycle.**
   Assign each plant a persistent uniform rank u in (0,1), then derive
   a latent lethal cycle count
   `N_i = K - ln(1-u)/h`, with `h = -ln(1-P)`.
   For a sufficiently large population, the fraction with
   `N_i <= n` approaches the published (M(n)) curve.
   This is a **mathematical equivalence of the fitted population model**,
   not evidence of genetic variance or the source's actual observed
   within-cohort thresholds. It involves no per-tick random mortality rolls.
2. **The cohort threshold K is a modeled delay, not the CCTO
   temperature-onset field.** Setting `coldDamageStartTemperature`
   to any of the four soil minima is incorrect. The regimen cycles
   between soil max and min; cold damage cannot be inferred from
   minimum alone.
3. **Population survival variability can be represented with a
   single scalar per-plant susceptibility**, but each cultivar has
   different K and conditional daily P; even with the SAME -17°C
   minimum, Jing411 LD50=8.7 and Nongda211 LD50=4.4 days. The
   distribution of lethal *cycle days* is shifted exponential at
   these study conditions, **not a demonstrated universal distribution
   of onset temperatures or degree-hour doses**.
4. The study does not independently identify
   **damage as a function of temperature** across two temperatures
   *for the same cultivar*, because each cultivar uses a distinct
   temperature cycle. W05 contains a *different* same-cultivar
   multiple-temperature design but does not yield numerical
   raw mortality-by-temperature/duration cells in accessible
   source text; the extractable abstract supports an FDD
   association, not a full injury-surface fit.
5. W03 reports nonmonotonic survival with longer time under
   another winter-wheat protocol, whereas W04's within-cycle
   mortality formula is necessarily monotonic. **Species/
   acclimation/study differences cannot be erased by selecting
   one global cumulative-mortality curve**. Retain a lightweight
   base model and document known deviations rather than add
   unmeasured parameters.
6. **RimWorld clock mapping unresolved:** experimental calendar days
   and in-game 24-hour days may be mapped one-to-one for brief
   frost events, but an annual winter in RimWorld lasts only
   60/365 of a real-year cycle. Do not blindly use '8.7 game
   days until LD50' without an explicitly selected time mapping.

**Outcome / stop:** This is a genuine source-calibrated
*population survival curve at four specific winter-wheat freeze cycles*,
and a tractable individual fixed-threshold mathematical implementation.
It is **not** a calibrated general CCTO injury equation, recovered
raw survival dataset, validated onset temperature, acclimation
mechanic or finalized per-Def XML. Keep PR Draft and avoid native
tests until numerical/ownership work is complete.


## W06 independent multi-temperature frost-dose cross-check — tillers only

An additional **fully accessible original 2024 paper**, Wu et al.
([W06, *Frontiers in Plant Science* 15,
doi:10.3389/fpls.2024.1419381](https://www.frontiersin.org/journals/plant-science/articles/10.3389/fpls.2024.1419381/full)),
has **three wheat cultivars each observed at multiple temperatures
AND across 1, 2 and 3 freeze days**. The measured endpoint is
**tiller mortality**, **not the death of whole plants**; its numeric
coefficients cannot replace W04's plant-survival regression.

The 2024 authors show that the minimum soil temperature at the
wheat crown (~2.5 cm deep) was typically **7.5°C higher than
the test air minimum after 1 day, 4.8°C higher after 2 days,
and 4.2°C higher after 3 days**. These time-dependent offsets
make a static equation based on game `AmbientTemperature`
especially unreliable as a *crown-freezing* mortality measure.

### Study-reported dose–response coefficients (Table 4)

The paper defines an accumulated *effective negative soil
temperature* `TSE_h` in °C·h below a crown-temperature threshold,
and describes a **local fitted linear regression** of the
**fraction of dead tillers**, `m = a*TSE_h+b`.
The response is fitted only across observed treatments; nonzero
baseline intercepts are **not** proof of mortality at physical
zero effective dose.

| Cultivar | Crown cold threshold used in study (°C) | Source regression `m = a*TSE_h+b` | Source R² | Source TSE_h for 10% / 30% / 50% dead tillers (°C·h) |
|---|---:|---|---:|---|
| Nongda211 | -8 | `0.0040*dose + 0.0725` | 0.83 | 6.9 / 56.9 / 106.9 |
| Zhengmai366 | -6 | `0.0051*dose + 0.059` | 0.93 | 8.0 / 47.3 / 86.5 |
| Yanzhan4110 | -3 | `0.0055*dose + 0.034` | 0.86 | 12.0 / 48.4 / 84.7 |

[Machine-readable W06 source and recomputed
quantiles](Research/WinterWheatW06TillerRegression.csv) retain the
original hourly and daily-dose fits, soil crown thresholds, and
separate derived/observed fields. Recomputing 50%-tiller-death
from the **rounded** `a,b` yields 106.875, 86.471, 84.727°C·h,
which agrees with the published rounded Table 4 values.
**This is reconstruction of the authors' own fitted regression,
not independent out-of-sample validation.**

Source Table 3 also fits *air minimum temperature* against
tiller death separately at each exposure day using logistic
curves. Important **internal consistency warning**:
for Yanzhan4110 **2-day** treatment the online table lists
`a=0.90,b=17.18`, which algebraically implies 50%-tiller
death at **-19.09°C** (because `T50=-b/a`), but the
same row prints a 50%-mortality temperature of **-12.7°C**.
It is impossible for both to be correct. **Quarantine the
2-day Yanzhan4110 regression coefficients** until the
original figure or author correction resolves the mismatch.
Other cohorts' rows are not automatically invalidated.

### Implications for CCTO numerical strategy

- **W04 = plant survival versus cold-cycle time** at a single
  cultivar-specific programmed soil temperature cycle; it
  can fit a stable plant susceptibility *population curve*.
- **W06 = tiller survival versus temperature and duration**,
  including within-cohort dose effects, but cannot directly
  fit the **whole-plant** mortality distribution or an
  acceptable CCTO lethal dose.
- **W05 = plant and tiller mortality versus soil frost
  degree-days** from the abstract, but detailed original
  per-treatment plant-survival cells remain unavailable,
  so full 2D CCTO plant-death calibration still lacks
  matching raw study data. The three papers are complementary,
  not interchangeable.
- **A lightweight profile is now plausible:** integrate
  nonnegative *effective cold exposure* below a class-specific
  onset; give each plant a persistent rank/dose threshold;
  match each available plant-class profile to its own
  mortality-versus-dose observations. **Do not assume a
  single global onset, linear slope, spread, ice/snow
  protection or recovery rate**; link air and crown
  conditions explicitly when simulating overwinter cereals.
- Do **not** apply a 1:1 map from the study's °C·hours
  or experimental cycle days to RimWorld's in-game time.
  The chosen time/calendar treatment and spatial
  temperature metric must be recorded with each future
  accepted plant-specific value. No XML or C# change
  is authorized by this dataset alone.

**Source access status:** W06 complete journal HTML tables
and methods were accessible, W04 article text and tables
were accessible from an author-hosted/mirrored reproduction,
W03 full original trial data remain inaccessible, and
W05 original treatment-level data remain inaccessible.
Do not claim that all three earlier full originals were obtained.


## Controlled constant-temperature survival duration: Kharkov wheat (W07 via W08)

**Materially new evidence (2026-10-21):** The 2025 open-access
Arora [review W08](https://pmc.ncbi.nlm.nih.gov/articles/PMC12624383/)
§5 quotes time-specific **whole-plant regrowth survival** from
[Pomeroy, Andrews & Fedak (1975) W07](https://doi.org/10.4141/cjps75-079)
for *cold-acclimated Kharkov winter wheat*. **The numerical survival
percentages have been verified against W08, not the original 1975
article's table/figure**, which was not directly obtainable in this
review. Do not count W07+W08 as two independent experiments.

| Constant freeze exposure for Kharkov | 1h | 3h | 6h | 12h | 24h | 120h |
|---|---:|---:|---:|---:|---:|---:|
| **−16°C: published survival** | 80% | 63% | 53% | 33% | 17% | 0% |
| **−16°C: fitted 1-parameter exponential** | 90.1% | 73.1% | 53.4% | 28.5% | 8.1% | ~0% |
| **−16°C: fitted 2-parameter Weibull** | 80.8% | 64.5% | 50.0% | 33.6% | 17.9% | 0.7% |

**Separately:** At **−6°C for 120h**, the same Kharkov wheat
cohort reportedly had **100% survival**. The dataset has **no
independent replicated temperatures×time points sufficient to fit a
temperature-severity exponent**; −16°C was the multi-time exposure,
whereas −6°C has an all-survived endpoint only.

**Model comparison (unweighted least squares; 1,3,6,12,24h only):**

```text
t = hours held at the quoted freezing temperature
Observations are percentage survival from a SECONDARY review,
with original sample sizes, raw binary outcomes and uncertainty
NOT yet available. Do not present this as binomial MLE.

Constant-hazard / exponential:
   S_E(t) = exp(-λ * t), λ = 0.104479 h^-1
   RMSE over 5 in-fit percent survival points = 7.77 percentage points

Variable-hazard / Weibull:
   S_W(t) = exp(-(t/τ)^k), τ = 10.500235 h, k = 0.656692
   RMSE over same 5 in-fit points = 1.59 percentage points
   Model 50%-survival time = τ*(ln 2)^(1/k) = 6.01 h
```

The fitted Weibull has **shape k < 1**, mathematically implying
a *decreasing instantaneous hazard* with frozen exposure time.
This is a convenient population-curve representation, NOT proof
that the Kharkov plant actually becomes more cold-hardy as it freezes.
The reported percentage survival also combines biological variation,
selection of earlier-susceptible plants, experimental stage/protocol
and unknown sampling noise. There is no reason to transfer this
fit to other varieties or conditions.

The 120h all-dead observation was **not used to fit either model**.
A Weibull extension predicts about 0.7% population survival there,
not literally zero. **Zero observed survivors in a finite experiment
is consistent with a small positive underlying probability**; no
numerical certainty or calibration claim is justified without
its sample denominator. Comparisons above are *in-sample fits*,
not cross-validation. Model shape and tail behavior are not
approved in CCTO.

[Reproducible source and calculation CSV](Research/WinterWheatW07DurationSurvival.csv)
contains every review-quoted survival fraction, both model
predictions and a per-row flag specifying which percentages were
used in the fit. The original 1975 DOI and 2025 full-text review
are retained separately in the evidence ledger as W07/W08.

### Independent protocol-level constraints from Gusta & Fowler 1977 (W09)

A separate [1977 original experiment](https://doi.org/10.4141/cjps77-029)
with winter-cereal **excised crown tissue** reports that
holding a frozen tissue **for 1h** did not materially shift the
50%-regrowth-injury temperature relative to removing it at
the instant the freezing target temperature was reached,
but **24h holding shifted the lethal temperature about 4°C
warmer**. It also reports pronounced supercooling/ice-nucleation,
freeze-thaw cycle and thaw-rate effects. This is **NOT**
independent biological replication of the Kharkov −16°C
cohort survival data; it is an independent experiment
supporting the broad principle that **duration matters**.
Because the tissue was excised and exposed under a laboratory
freeze protocol, this is not an exact whole-plant temperature
parameter either.

### Reassessment of W05 full numerical data accessibility

[Zheng et al. 2018 W05](https://www.sciencedirect.com/science/article/abs/pii/S0168192318301631)
publisher summary and [author-indexed ResearchGate record](https://www.researchgate.net/publication/325699751_Effect_of_freezing_temperature_and_duration_on_winter_survival_and_grain_yield_of_winter_wheat)
confirm one acclimated **Jing411** winter-wheat cultivar exposed
to distinct soil-minimum cycles of **−14, −17, and −20°C**.
The published experiment's freeze-cycle-day treatment sets
are **−14°C: 2/4/6/8/10 days**, **−17°C:
1/2/3/5/6/7/8/9 days**, and **−20°C:
1/2/3/4/5/6/8 days**. The study's abstract reports
approximately **1.0% plant mortality increase per
1°C·day** of soil freeze degree-days below its
study-specific **−12°C** threshold (and **1.4% tiller
mortality**), but **the full per-temperature/per-duration
*whole-plant* mortality cell values and uncertainty
remain inaccessible**. The paper uses *soil minimum
temperature*, which cannot be treated as continuously
maintained crown temperature or RimWorld ambient air
temperature. The abstract percentage slope may refer
to a regression of average mortality against dose and
should NOT be used as a per-plant random hazard or
absolute 100%-death point without model intercept,
population denominator and trial details.

**Model-fitting consequence:** W05 is genuinely
a **two-factor whole-plant trial**, but available public
summaries do not provide a defensible full 2D empirical
mortality surface. Do **not** invent its treatment cells,
digitize unreadable figures, or advertise a 2D
CCTO formula as validated on W05.

### Decision to carry forward

1. **Evidence now justifies a stable per-plant
   lethal-exposure *distribution* as a lightweight
   population-survival abstraction.** W04 models
   survival over full freeze cycles in one genotype's
   specific soil cycle, while W07-via-W08 supplies
   multiple duration/survival points under −16°C
   that fit a flexible two-coefficient survival curve.
   Their inferred time scales are **study-specific**.
2. **A single exponential population survival rate
   is inadequate** to describe this one Kharkov
   five-point series (in-sample 7.77 percentage-point
   RMSE), whereas a Weibull shape can reproduce it
   more closely (1.59pp). The fit alone does
   not authorize Weibull across all CCTO plants,
   because it uses an extra free parameter, five
   rounded points and unverified denominators.
3. **Do not equate** the Weibull time shape, a
   temperature-injury severity exponent, and a
   cross-plant variance coefficient. They model
   *different axes and biological phenomena*.
   Likewise a field-acclimated cultivar versus
   spring wheat is not an interchangeable parameter
   set. The survival CDF's role is fixed per-plant
   susceptibility, not repeat random checks.
4. **Missing critical data:** same-plant whole-
   survival over multiple freeze *temperatures AND
   durations*, preferably with shared acclimation,
   soil/air/crown temperature trace, raw live/dead
   counts and regrowth criterion. A controlled
   independent protocol supporting that space
   remains needed before production CCTO
   `coldDamageStartTemperature`/`coldDamageDegreeHoursToDie`
   and distribution spread are locked.
5. No XML, C#, SaveState, Workshop graph or
   native RimWorld tests were altered/performed
   as a result of this analysis.


## W10 full-plant cold-freezing factorial design: paired LD50 / LT50 (2026-10-21)

Husiny, Ficht, Whiting, Booker, Hooker, McElroy & Lyons (2024),
*Canadian Journal of Plant Science* 104:256–264,
[doi:10.1139/cjps-2023-0143](https://doi.org/10.1139/cjps-2023-0143),
was reviewed as original published article text, methods and **Table 2
and Table 3** via its [open article reproduction on ResearchGate](https://www.researchgate.net/publication/377906631_Improved_methods_to_estimate_days_and_temperature_to_fifty_percent_mortality_of_winter_wheat_Triticum_aestivum_L_under_low-temperature_flooding_and_ice_encasement).
The original paper provides a **real, publicly cited individual-plant
dataset** at [Borealis DOI 10.5683/SP3/YHOZUK](https://doi.org/10.5683/SP3/YHOZUK),
but attempts to open the hosting service currently return HTTP **403**.
Therefore **the primary paper/table is read; its archived *raw binary
treatment observations* are NOT accessed or reconstructed**.

**Experimental biological endpoint:** intact whole wheat in sand/peat,
Zadoks 21 (three leaves, one tiller), acclimated **3.5°C for 4 weeks**.
After freezing, thawed **2d at 4°C**, then **3-week greenhouse regrowth**;
individual plant alive = visible above-soil regrowth, dead = no regrowth.
Experiments tested ice encasement and no ice, with per-study pooled
temperature/duration means reported where ice effect was not statistically
significant. Crown soil temperature monitoring was included; do not treat
the prescribed air/chamber minimum as a recorded constant crown temperature.

**Two experiment arms are NOT an entire crossed temperature × duration
matrix of the same plant cohorts:**

1. **LD50 duration arm:** plants held approximately **-4°C**
   (ice/no-ice) and sampled at freeze days **0/4/8/12/16/20/25/32**,
   12 plants per cultivar/treatment/experimental unit, two replicates.
   Original authors used probit on log10(days) to infer LD50. In
   particular **Norstar's source LD50=33d is 1d beyond the highest
   *observed* duration of 32d**, so the numerical half-mortality
   duration is partly **model extrapolation**.
2. **LT50 temperature arm:** pre-held at **-4°C for 0 versus 7 days**,
   separate cohorts then progressively frozen to **-4/-7/-10/-13/
   -16/-19°C**, ~3°C/h cooling, with **1h at each step**. Original
   Table 2 reports LT50 from this staged freezing ramp, pooled over
   ice and no ice, with three replicates and 12 plants per experimental
   unit. No one acute-temperature hold lasted seven days.

### Published regression-derived whole-plant medians

| Cultivar | Growth habit | LD50 (days at -4°C) | LT50 before additional 7d at -4°C | LT50 after additional 7d at -4°C | LT50 shift in 7d (warmer = less hardy) |
|---|---|---:|---:|---:|---:|
| Norstar | winter-hardy | **33** (model extrapolation beyond last 32d observation) | -13.6°C | -13.2°C | +0.4°C |
| CM614 | winter | **20** | -12.7°C | -12.6°C | +0.1°C |
| Branson | winter | **18** | -11.9°C | -11.6°C | +0.3°C |
| AC Carberry | spring control | **not estimable** (all dead by day 4 in LD experiment) | -6.6°C | -2.7°C | +3.9°C |

**Warning about Carberry:** the duration and LT50 arms are *different
samples/experiment series*. The source reports 100% mortality by day 4
at -4°C in one, yet derives a day-7 LT50 of -2.7°C in the other.
Do **not** merge these endpoints as if the same identical plants
survived the entire seven-day pre-hold and then endured a new minimum.
Treatment handling, baseline mortality and conditional-probit
methods require raw-data reconciliation.

**Quantitative interpretation:** same-species/cultivar whole-plant
hardiness shifts **only +0.1 to +0.4°C after 7d** for the three winter
varieties but **+3.9°C** for the spring wheat control. That supports a
*cohort-state-dependent loss of hardiness*, not a global fixed random
per-plant tolerance spread. At -4°C prehold, median duration
differs dramatically among winter cultivars, even though
their additional acute-freezing LT50 scores are all around -12 to
-14°C. Duration and acute temperature stress are
**separate axes that cannot be conflated**.

[Original-table W10 source values and arm-separation
CSV](Research/WinterWheatW10HusinyWholePlantLT50.csv)
preserves all eight Table 2 original LT50 means, the three
LD50 values, spring control censored/non-estimable result,
the time points and the exact treatment caveats. Nothing
in this file is a plant-level observed cold-death time
for a particular temperature and no individual survival
rows were manufactured.

**Comparison against proposed CCTO model:** a single
plant-static lethal accumulated dose can approximate
population death under a **fixed treatment protocol**;
it is insufficient by itself to explain this genotype-
specific change in acute cold tolerance after long -4°C
pre-exposure. A selective, lightweight pre-acclimation/
de-acclimation or stage-dependent tolerance state
might eventually be justified for winter cereals; the
very different spring control indicates its dynamics
must not be universal. But the present 0/7d LT50
means do **not** identify onset temperature,
cold degree-hour severity exponent, random rank
distribution width or thermal recovery kinetics.

**High-yield next evidence step:** obtain and inspect
the **published Borealis dataset** (dataset DOI above)
only if accessible via a supported archive route.
The original design includes up to **two duration arms,
six freezing-temperature levels and 12 individual
plant outcomes per experimental unit**, making it a
much more relevant source for unbiased binary
population curve fitting than inventing extra
parameters from W05's abstract.
If the archive remains unavailable, do not claim
cross-validation or fit a full 2D death surface;
the published table/fit endpoints above remain
usable as **bounds/relative ordering only**.

**Status:** evidence collected, no CCTO numeric field approved;
no stock XML, gameplay C#, native tests or Workshop values
changed. Continue using W10 source ID in the
species evidence registry.


## W10 raw-data retrieval attempt and significance-aware state-model decision (2026-10-21)

### Provenance/access

Husiny et al. (2024) publish the raw individual plant responses at
[Borealis, DOI 10.5683/SP3/YHOZUK](https://doi.org/10.5683/SP3/YHOZUK).
The following read-only retrieval paths were attempted without success:

- Publisher-identified [Borealis dataset page](https://borealisdata.ca/dataset.xhtml?persistentId=doi:10.5683/SP3/YHOZUK): **HTTP 403** using public web retrieval.
- Dataverse native dataset metadata `/api/datasets/:persistentId/?persistentId=doi:10.5683/SP3/YHOZUK`: unavailable using web retrieval.
- Alternative Scholars Portal Dataverse hostname and DOI resolver: neither provided a readable data table.
- Current execution environment's outbound network access cannot resolve the Borealis hostname. This is **an access limitation here**, NOT evidence that the archive is unpublished, missing or restricted to registered users.

**Stop condition:** do not fabricate the paper's 12-plant binomial observations or assume that Table 2 LT50 medians are individual raw records. Do not persist a blank/fake raw dataset or build a new extraction/test infrastructure just to bypass the access issue. The source-linked published statistics remain the evidence until a verifiable raw dataset is available.

### Correction: winter-wheat 0-to-7-day LT50 movement is not significant

The source's original Table 2 gives Tukey HSD multiple-comparison
letter groups at **alpha = 0.05**. Sharing any letter means the two
published group means do **not** differ significantly.

| Cultivar | Day 0 (LT50 °C, letters) | Day 7 (LT50 °C, letters) | Observed shift | Statistically different? |
|---|---:|---:|---:|---|
| Norstar, winter | −13.6 a | −13.2 ab | +0.4°C | **No**, common `a` |
| CM614, winter | −12.7 bc | −12.6 bcd | +0.1°C | **No**, common `b/c` |
| Branson, winter | −11.9 cd | −11.6 d | +0.3°C | **No**, common `d` |
| AC Carberry, spring | −6.6 e | −2.7 f | +3.9°C | **Yes**, no common letters |

**Both groups already underwent 4 weeks of cold acclimation before
the LT50 experiment.** Day 0 is *not* unacclimated spring/summer wheat.
The observed winter cultivar changes were **not** demonstrated losses
of winter hardiness by the paper's inferential test. Earlier prose
describing their +0.1–0.4°C arithmetic differences as physiological
deacclimation **overstates the evidence** if stated without statistical
uncertainty. The statistically significant Carberry change is within
its *separate LT50 treatment arm*; Carberry had 100% death by day 4
in the different LD50 experiment. Therefore we cannot fit a common
7-day per-plant acclimation decay constant from these means.

[W10 primary-table CSV](Research/WinterWheatW10HusinyWholePlantLT50.csv)
now preserves the Tukey letter groups and a boolean within-cultivar
day0/day7 significance check. Original published LT50 values,
experimental-arm separation and the unverified raw-data status are
otherwise unchanged.

### Minimal state-model evaluation for CCTO

| Approach | Runtime state / cost | Does W10 require it? | Biological limit |
|---|---|---|---|
| **A: fixed cohort/Def baseline** with already approved elapsed damage + per-plant stable fatal-dose rank | **No separate acclimation timer**, no periodic new map scan; compute only when existing plant callback runs | **W10's three already-acclimated winter cultivars are compatible** with no statistically demonstrated median shift over this 7d window | Cannot represent an unacclimated plant acquiring hardiness over several prior cool weeks or losing it after spring warming |
| **B: per-map temperature/season proxy** shared by every plant | Few map-level values, but needs update/inference and ignores greenhouse/microclimate differences | **Not established** by W10 | Indoors, heaters, snow/soil temperature and growth stage differ per cell; a global mean confounds them |
| **C: stored per-plant acclimation state** updated through the existing callback | One extra scalar/history time per opted-in plant, must persist for *all acclimating plants even when undamaged*, not just sparse injury records; more memory/save work | **Not warranted by W10 winter 0/7d comparison alone** | Could capture actual cooling/deacclimation over weeks if the acquisition/loss kinetics and plant thermal signal are separately calibrated |

**First-implementation recommendation:** **defer C and B**, not
the general biological possibility of acclimation. Keep the
minimal fixed-Def median + stable individual susceptibility
candidate for already-defined physiological states, and prioritize
correct plant death/mortality-versus-dose curves. **Do not**
set a 0.1–0.4°C/7d hardiness decay coefficient for winter
wheat; that would be fitting *non-significant noise*.
Treat AC Carberry's +3.9°C as a clear *cohort response under that
protocol*, but not a reason to attach state timers to all
plants. Later adopt opt-in per-plant acclimation only if a
repeated experiment AND intended game conditions show
that omitting it materially breaks overwinter survival.

The recommendation is an evidence-based scope filter, **not**
a proof that winter wheat never acclimates in nature. Before
using class A for actual winter-colony gameplay, it still
needs a justified initial/acclimated state and a temperature×
duration fatal-dose response at the chosen game time scale.
Neither `coldDamageStartTemperature` nor the dose
distribution width is newly approved. **No new C#,
XML or native game tests were performed.**


## First identifiable crop-class response audit: maize (2026-10-21)

**Practical result:** plant-level fatal exposure and same-species
non-freezing chilling are supported by primary studies; nevertheless
there are currently not enough interchangeable observations to select
a unique `(onset, severity exponent, fatal dose, susceptibility spread,
recovery)` tuple. This section defines the **minimal viable next
implementation boundary** rather than indefinitely collecting
unrelated endpoints.

### Distinct maize sources that must NOT be merged into one cohort

- **M01**: [Lindsey et al. (2024) reviewing Buican (1969)](https://doi.org/10.1002/csc2.21212)
  states approximately 50% seedling mortality at **-2°C / 48h**,
  and separately **-6°C / 2h**, followed by **10d recovery**.
  Buican's original plant cultivar/stage/sample counts and full
  temperature ramp have not been obtained. Thus these are
  *secondary-source, approximate population LT50 severity-time
  pairs*, not verified original continuous-temperature traces.
- **M03**: [Creencia & Bramlage (1971)](https://pmc.ncbi.nlm.nih.gov/articles/PMC365875/)
  measured *maize seedlings at +0.3 ± 0.3°C*. Leaf injury after
  **36h** was reversible at 21°C; after **48–60h** some
  seedlings collapsed irreversibly while most recovered, and after
  **72h** the transferred seedlings ultimately died. **Non-freezing
  / near-zero chilling can be lethal**, and ordinary leaf symptoms
  can be reversible. The abstract supplies neither measured LT50
  nor individual sample counts at those durations. It is a
  *separate experiment* from M01 and cannot simply be treated as
  an additional M01 temperature-time measurement.
- **M04**: [Jiang et al. (2016)](https://pmc.ncbi.nlm.nih.gov/articles/PMC5054024/)
  tested **30 distinct maize inbred lines**, all at **-1°C for
  3h**, then 3d recovery. Genotype-specific cohort survival
  ranged from **5.01%** to **97.3%**. This is powerful evidence
  of *between-genotype differences*, **not the width of one
  genotype's individual susceptibility distribution**.

### Exact conditional fits do NOT identify the correct temperature law

Consider the candidate family (for an as-yet uncalibrated freezing
component only):

```text
h_p(T) = [max(0, T_onset - T)]^p
D(t) = integral h_p(T) dt
median mortality at D50, under one fixed physiological cohort
```

For **each independently chosen p**, the two M01 approximate
50% death points can be fit *exactly* by solving onset
and D50. The [numerically reproducible sensitivity table](Research/MaizeM01DegreeHourIdentifiability.csv)
reports:

| Assumed temperature exponent p | Solved hypothetical onset °C | Hypothetical median fatal dose (°C^p·h) | Predicted T50 at −4°C (h), **not measured** |
|---:|---:|---:|---:|
| 0.5 | −1.993 | 4.003 | 2.83 |
| 1.0 (linear degree-hours) | −1.826 | 8.348 | 3.84 |
| 1.5 | −1.454 | 19.388 | 4.77 |
| 2.0 (quadratic) | −0.974 | 50.520 | 5.52 |
| 3.0 | +0.123 | 459.022 | 6.55 |

**This is an algebraic sensitivity analysis, not five alternative
empirical measurements.** The source's -2/-6°C LT50 pair has
only two constraints; for every chosen p, temperature onset and
median dose can be adjusted to fit exactly, yet predicted -4°C
median time varies by **more than a factor of two**.
No `p`, onset, D50, recovery rate, or variability width is
identified without additional survival-temperature/time
observations at matching stage and acclimation.
The computed positive onset under p=3 is a
*fit artifact*, not evidence that that physiological
threshold exists.

More importantly, at the observed +0.3°C chilling
condition of M03, the simplistic linear/quadratic
**M01-fitted freezing-only curves yield zero dose**
and cannot explain the mortality under prolonged
chilling at nominally above-zero temperature.
This does **not** falsify M01 or M03: they are
distinct cohorts/studies, and the frozen/chilling
injury mechanisms can differ. It does show that
CCTO's **`損傷開始温度` cannot universally be equated
to the onset of actual ice formation or fixed below 0°C**
for every non-dormant crop. A light, source-backed
**cold-sensitive class profile** may need a mild
positive-temperature chilling contribution alongside
rapid subzero injury, without adding more per-plant
state or a second global map scan. Numerical onset,
profile breakpoint and transitions remain unapproved.

### Existing external realism precedent — FROSTOL

- **W11** [Bergjord Olsen et al. (2008)](https://doi.org/10.1016/j.eja.2007.10.002):
  FROSTOL models winter-wheat *changing LT50*
  using **soil temperature at 2cm depth, snow depth,
  vernalization and hardening/dehardening/stress**,
  reporting fitted LT50 model R²≈0.84 in its studied
  sites.
- **W12** [Bergjord Olsen (2018)](https://doi.org/10.1111/jac.12238):
  independent winter-survival **class** comparison
  averaged **75% agreement in Sweden** and
  **37% in Norway**, indicating meaningful
  environmental/other winterkill failure modes.
  These figures are **classification accuracy**,
  not individual mortality response or proof of
  universally correct LT50 values.

**Consequences:** complete FROSTOL would require
crown/soil temperature, snow, plant developmental
state and daily updates; none are available as
reliable universal plant-specific inputs in the
current CCTO accumulator. **Do not copy its
coefficients** or impose a global wintertime timer.
Preserve deferred, class-specific acclimation
as a future option, consistent with the W10
significance-gated initial-scope decision.

**Concrete next gating measurement:** one additional
matching **whole-seedling** mortality/survival
observation under a third freeze
temperature/duration for the same maize
cohort as M01, ideally with full cooling and
thaw history, would distinguish severity exponents.
For mild chilling, matched cohorts at 0–5°C
with repeated exposure/recovery rates would
identify an independent chilling contribution.
For within-genotype variation, use multiple
survival proportions under otherwise identical
conditions; do not infer the individual coefficient
distribution from M04's 30 distinct inbred-line
survival percentages. Until then **the table is
a sensitivity study and XML/C# values remain
unchanged**.

**Project scope decision:** do not block all
78 CCTO species on obtaining raw multi-dimensional
survival grids impossible for fictional/generic
plant labels; use a small number of defensible
class analogues, record which parameters are
measured, which are conditional estimates and
which are unsupported, and protect default
dormancy/biome vegetation. There is no scientific
basis to declare one experimental maize cohort's
quadratic or linear rate universally correct.


## First primary 5 × 5 temperature-and-duration WHOLE-PLANT study — chickpea (2026-10-21)

**Major evidence improvement over W04/W05/W10 and M01:** an
open-access original experiment includes **five temperatures × five
target-temperature holding durations** and judges **intact seedling
survival after four weeks**, not detached-leaf conductivity or
tiller-only injury. **C03:** Nabati et al. (2023),
[*Frontiers in Plant Science* 14:1239008,
doi:10.3389/fpls.2023.1239008](https://www.frontiersin.org/journals/plant-science/articles/10.3389/fpls.2023.1239008/full).
Six-leaf *Cicer arietinum* seedlings from the Mashhad collection,
four replicates, eight seedlings originally per pot, were
exposed to **0/-6/-8/-10/-12°C × 1/2/3/4/5h**.
The freezer was lowered from +5°C at **2°C/h** before
each target-temperature hold, then plants spent ~24h
at +5°C to thaw and 30d in a glasshouse to score
living vs dead plants. **The source does not identify
one verified uniform cultivar/clone**, and individual
binomial records are not available. This makes it
a *single growth-stage crop-group factorial protocol*,
not evidence that the results apply to every legume.

The source's **Figure 2A contains 25 plotted survival
values**. There is no machine-readable original
25-cell survival table in the article HTML; **do not
invent or visually guess the missing percentages**.
However, the original text gives **10 useful
source-derived numeric anchors**: five
survival percentages at the same −12°C and five
temperature-LT50 medians (computed by source's own
three-parameter logistic fitting) at each
holding duration.

| Held at -12°C, hours | 1 | 2 | 3 | 4 | 5 |
|---|---:|---:|---:|---:|---:|
| **Observed whole-plant survival %** | **75** | **78** | **25** | **8** | **0** |
| **Source-fitted LT50°C at this duration** | **-12.1** | **-12.1** | **-10.5** | **-11.0** | **-9.0** |

- Survival is **not exactly monotonic** at 1→2h
  (75→78%) despite a longer hold, and the
  source's LT50 becomes 0.5°C *colder* at
  3→4h rather than warmer. These
  details **must be preserved as published**.
  They are not proof of biological recovery
  while frozen; likely variation/rounding/source
  fitting contributes, but the exact cause and
  uncertainty cannot be determined without
  treatment-level measurements.
- "LT50" above is the **temperature giving
  50% *plant mortality* at each hold** as
  fitted by the source, not the CCTO individual
  death temperature. Five source LT50
  estimates are **derived from the same
  experimental grid**, not five independent
  external validation studies.
- **Target hold duration is not total cold
  exposure.** Reaching −12°C from +5°C
  at 2°C/h takes ~8.5h of ramping, while
  reaching −6°C takes ~5.5h; the time
  series below each temperature threshold
  differs. **Do not simply model the
  1h versus 5h target hold as total exposure.**
  Exact plant/tissue temperature and ice
  nucleation paths are not provided.

### Bounded numerical model-shape comparison at -12°C

The [C03 CSV](Research/LegumeC03ChickpeaFreezingSurvival.csv)
separates **observed and author-reported** values from
our **exploratory model predictions**. Unweighted
least squares to the **five published rounded
whole-plant survival percentages** (1–5h only),
using observed hold hours as the input:

```text
Exponential population survival
S(t) = exp(-lambda * t)
fit lambda ≈ 0.37148 per target-h
5-point in-sample RMSE ≈ 17.21 percentage points

Two-parameter Weibull population survival
S(t) = exp(-(t/tau)^k)
fit tau ≈ 2.8085 target-h, k ≈ 2.66565
5-point in-sample RMSE ≈ 10.11 percentage points
```

**The Weibull fit is still poor** for the
1h measurement (predicted 93.8% vs
observed 75%, **+18.8 percentage points**).
Its apparent smaller RMSE is **not evidence
of a good validated CCTO curve**, and the
exponential and Weibull models have unequal
numbers of free parameters. Exact survivor
counts, the covariance of shared source
LT50 fits and separate independent replicates
have not been recovered. The fit is not
binomial maximum likelihood, a scientific
effect-size estimate, or model cross-validation.
No coefficient or distribution shape is
approved for use.

### Direct CCTO-owning plant cross-check: lentil, not chickpea

The actual CCTO/MO plant `DankPyon_Plant_Lentils`
is *Lens culinaris*. Two additional **same-species
original studies** are now source IDs `L01`
and `L02` in the evidence ledger:

- **L01:** Nabati et al. (2020),
  [doi:10.22092/sppi.2020.123186](https://www.researchgate.net/publication/352977909_Freezing_Tolerance_in_Some_Lentil_Genotypes_under_Controlled_Conditions).
  Forty genotypes tested at **−13/−15/−18°C**.
  Its published summary says **100%, 93%, 30%
  *of genotypes*** at those temperatures
  were **not significantly different from
  their own maximum 100% survival**.
  **That is not a 100/93/30% seedling
  survival curve.** Only MLC12/17/95
  are described as maintaining
  100% observed survival across all
  three tested temperatures. Original
  method detail and individual genotype
  mortality cells not accessible.
- **L02:** Nabati et al. (2023),
  [FAO AGRIS primary-article summary](https://agris.fao.org/search/en/providers/122294/records/66fa71abdb9bf9ac61020558).
  Eighteen genotypes tested at
  **0/−15/−18/−20°C**, 2°C/h ramp
  from +5°C and **1h target hold**;
  after three weeks' recovery,
  **five named MLC genotypes**
  retained >80% plant survival at
  −15°C, while **none of the 18
  genotypes survived −20°C** in
  sampled treatments. Those are
  measured survival observations at
  specific temperatures for an
  actual CCTO plant species, **not
  exact whole-species deterministic
  death temperatures**. Their
  within-genotype raw counts and
  a multi-duration series remain
  unavailable.

**Combining C03 with L01/L02 as if
`Lens culinaris` and `Cicer arietinum`
were one species is forbidden.** C03
validates the *methodological need*
for duration×intensity treatment;
L01/L02 constrain *lentil-specific
field/experimental phenotype* under
one specified duration. The CCTO
whole-lentil lethal dose, chilling
onset and individual susceptibility
spread are **not identified**.

### Implementation decision from this bounded literature pass

1. The source now demonstrates directly
   why a shared **linear degree-hours**
   equation cannot simply claim
   population-level biological
   validation. **No model form
   wins robustly** after allowing
   the unknown underlying ramp
   and sampling error.
2. Preserve the **low-cost common
   accumulation engine**, a
   stable rank per plant and
   **class-specific candidate
   severity functions** for later
   numerical fitting. Do not
   enable a new plant class on
   guessed onset/dose/variance.
3. Prioritise **two measured
   biological regimes**, not
   78 incompatible hand-tuned
   formulas: severe below-zero
   freeze exposure, plus
   prolonged nonfreezing
   chilling where species
   evidence specifically
   requires it (maize M03).
   The number/shape of free
   coefficients is not yet
   approved.
4. **Stage and identity are
   nonoptional metadata.**
   Six-leaf chickpea, cold-
   acclimated winter wheat
   and specific lentil MLC
   genotypes do not represent
   identical colony plant
   cohorts; distinguish
   cohort medians from
   within-cohort susceptibility
   variability.
5. A separate study of
   identical *lentil genotype*
   across more than one
   freeze duration, or an
   independently recovered
   25-cell C03 source table
   with uncertainty, would
   permit more reliable shape
   testing. Until then preserve
   the *experimental* opt-in
   C# accumulator and current
   production XML unchanged;
   skip unnecessary native
   tests and new CI fixtures.


## Monotonic mortality audit / matching-lentil literature (2026-10-21)

**Source-ID correction:** the earlier chickpea CSV was erroneously
called `LegumeC01FreezingSurvival.csv` and carried rows with
`source_id=C01`. However **C01 in the bibliography is the NARO
cabbage leaf-injury study**. Its correct chickpea source ID is **C03**,
not C01. The file is now
[LegumeC03ChickpeaFreezingSurvival.csv](Research/LegumeC03ChickpeaFreezingSurvival.csv);
the inaccurate file has been deleted on this Draft branch, with
the original version retained in Git history. **No measured data
were changed by the source-ID correction.**

### Can a strict non-recovering cumulative injury law explain C03's minor inversions?

An incremental cold-dose model with **nonnegative** severity
and a fixed individual threshold must produce **nonincreasing
population survival** with *greater injury dose* under a
matching temperature trajectory. The five **reported**
whole-plant values at −12°C in C03 are 75/78/25/8/0%
for 1/2/3/4/5 **target-temperature hold hours**.
Fit the smallest **unweighted nonincreasing isotonic regression**
using pooled adjacent violators (PAVA); this imposes
only monotonicity, no exponential, Weibull or normal shape:

| Target hold (h) | Reported whole-plant survival % | Fitted monotonic % |
|---:|---:|---:|
| 1 | 75 | **76.5** |
| 2 | 78 | **76.5** |
| 3 | 25 | 25 |
| 4 | 8 | 8 |
| 5 | 0 | 0 |

**In-sample RMSE** over five equally weighted *published rounded
percentages* is **0.948683 percentage points**, compared with
**17.21pp** for the 1-parameter exponential and **10.11pp**
for the 2-parameter Weibull in the previous pass.
This comparison is **descriptive only**: the nonparametric
monotone fit effectively has multiple free knot values,
and it **does not predict outside the five source
time points**, correct for the long cooling ramp or
estimate population uncertainty. Its close fit does
**not** prove that a physically meaningful cumulative
cold-dose model has been identified.

Likewise, the *authors' separately model-derived*
temperature LT50 at 1/2/3/4/5h is
−12.1/−12.1/−10.5/−11.0/−9.0°C.
With the expected **nondecreasing LT50**
as the hold gets longer, unweighted isotonic
smoothing produces
**−12.1/−12.1/−10.75/−10.75/−9.0°C**
(in-sample RMSE **0.158114°C**). Again this
merely demonstrates that the apparent 3→4h
inversion is small; it is NOT an additional
experiment or a calibrated time-temperature
response. **Reported data remain intact**
alongside clearly labelled computed values
in the corrected C03 CSV.

**Interpretation:** the minor inversion between the
1h and 2h survivors does *not* by itself
justify implementing an extra cold-triggered
recovery/acclimation timer. A dose that grows
during fixed severe exposure remains
*compatible* with this small discrepancy
given unknown denominators, figure rounding,
varying cooling ramps and source-model fits.
A distribution fitting method is still
required to **interpolate across temperatures**,
rather than memorizing five observations.

### Additional direct studies of CCTO-owned lentil

- **L03** [Barrios et al. (2017), original open text](https://pmc.ncbi.nlm.nih.gov/articles/PMC5493078/):
  *Lens culinaris* Precoz×WA8649041
  recombinant inbred lines (RILs);
  **36 seedlings per RIL and frost level**
  across two trials, **three weeks
  acclimation (6/4°C day/night)**. Tests
  reach −3/−6/−9/−12/−15°C;
  hold **15h at −3°C**, then **1h at each
  colder level**. Living plants scored
  **after four weeks recovery**. Almost
  every RIL survived the −3°C
  regimen, with strong genotype-related
  survival differences at colder
  endpoints. **Figure 2 has counts of
  RILs in 12 frost-response categories**,
  not per-RIL binary survivor counts.
  Neither manuscript nor listed
  supplementary transcriptomics files
  provide a numeric whole-plant
  survival matrix for temperature×time.
- **L04** [Ali, Johnson & Stushnoff (1999), publisher abstract](https://doi.org/10.1017/S0021859699006917):
  after **6–8 weeks cold acclimation**,
  holding **−15°C for 3h** at
  2–3°C/h cooling was useful to
  distinguish lentil genotypes at
  P<0.01. **No original genotype
  mortality fractions are quoted
  in the accessible abstract**.
- **L05** [2026 seed study](https://doi.org/10.3389/fpls.2026.1802566):
  -2°C duration LD50 is measured for
  **lentil seed germination/viability**,
  e.g. CDC KR-1 34d, QG-1 15d,
  Shasta 0d. **Seeds are not growing
  plants**; do not adopt those
  figures as plant `coldDamageDegreeHoursToDie`.

### Implementability decision after reviewing owned lentil

**More direct same-species evidence exists**, but
there is still **no admissible within-genotype
*whole-plant* survival matrix across
both temperature and holding time**
for lentil available from these
papers. The 2023 C03 matrix
*establishes plausibility of a monotone
lethal-dose curve under a fixed
cold-sensitive seedling protocol*,
not the numerical threshold or dose
for `DankPyon_Plant_Lentils`.
Moreover CCTO's generic cultivated
MO Lentil is not known to be
3–8-week hardened Precoz/RIL material;
importing −15°C tolerance would
improperly assign winter-acclimated
genotype values to every new planting.

**Next bounded coding task, when severity
parameters have an accepted class/state
reference:** preserve sparse map updates,
use a **stable per-plant rank for a
source-calibrated lethal-dose
threshold**, retain unchanged
dormancy-only and legacy plants,
and validate one fully scoped
candidate profile. Do not add
state timers or multiple runtime
CDF families to explain the tiny
C03 sampling inversion. **No
CCTO production XML/code value
is newly approved by this pass.**


## Correction — audit the complete C03 figure, not only the -12°C text row (2026-10-21)

**Scope correction to the previous 5-point PAVA assessment:** the
**0.95 percentage-point** monotone-fit RMSE applies **ONLY** to
five exact *published in text* survival percentages at **-12°C**,
and cannot be described as a 5×5 study-wide model fit. The
[original C03 Figure 2A image](https://www.frontiersin.org/files/Articles/1239008/xml-images/fpls-14-1239008-g002.webp)
shows other survival-time inversions. In particular, **-10°C
for 3 h appears around 60% survival, while -10°C for 4 h
appears around 93% survival** (difference ~33 percentage
points). At **-8°C** the plotted 3-h survival is around 85%
and the 4/5-h samples appear at 100%. The source does not
supply a machine-readable raw 25-cell data table.
Do **not** treat visually read data as exact measured numbers,
or claim that these inversions are merely ±3% sampling noise.

A limited, clearly identified approximate transcription is
recorded in [ChickpeaC03Figure2Approx25.csv](Research/ChickpeaC03Figure2Approx25.csv).
The source's five verbatim values at -12°C are **exact
published text**. The other 20 displayed percentages are
**visually estimated from plotted curves**, nominally within
~5 percentage points where intermediate; this is *reading
tolerance*, NOT a published statistical confidence interval,
raw individual survival record or measured sample size.
The original plot has standard-error bars and may reflect
sampling differences between treatments; no claim of
statistical significance is made here.

**Monotone-only comparison across all graph-read points:**

| Treatment minimum | Reported/text or approximate 3h survival | Approximate 4h survival | The closest monotone PAVA 3h / 4h |
|---|---:|---:|---:|
| -8°C | ~85% | ~100% | ~95% / ~95% (also 5h ~95%) |
| -10°C | ~60% | ~93% | ~76.5% / ~76.5% |
| -12°C | 25% **(verbatim)** | 8% **(verbatim)** | 25% / 8% |

Columnwise, equally weighted nonincreasing PAVA applied
to the 25 text/graph-read values produces a **minimum**
in-sample RMSE of **~5.29 percentage points** among
all functions that are monotone in holding time
for each temperature (assuming the read values
are exact inputs for this descriptive calculation).
This is a **constraint-relaxation bound, NOT a fitted
physical CCTO dose law**. No monotone cumulative-dose
plus persistent-threshold function can fit the
approximate 25 observations with *less*
squared-error under the same sample weights,
and most parametric physical models will do worse.
The large apparent -10°C inversion remains
greater than two nominal ±5pp graph-reading
errors combined; **whether the study's treatment
differences are statistically distinguishable
cannot be decided without underlying raw
replicates and proper analysis**.

**Practical implementation implication:** retaining
an inexpensive **nonnegative accumulated-damage
engine is still justified as a deliberate
approximation**, but the original chickpea
study's complete 5×5 displayed results do
*not* constitute a close numerical
validation of such a model. Do not force
a dynamic cold-recovery state into RimWorld
merely to match one ambiguous plot, and
do not select a shared linear/quadratic/
Weibull exponent or an individual-variance
width from this source. The experiment is
for *Cicer arietinum*, whereas the CCTO
owning crop is *Lens culinaris*, with
different source-specific state and
growth-stage constraints. A source with
per-treatment whole-plant binomial counts,
temperature histories and repeated trials
is still required to claim quantitative
prediction of a temperature×time surface.

**Status:** corrected model-evidence
interpretation, no CCTO game-data or
source-code values changed, original
5-point text and original figure
remain separately and visibly attributed.


## Quantitative feasibility gate — wheat, maize, rice (2026-10-21)

**Purpose:** test the *shape and identifiability* of the current Draft C#
linear-degree-hour + deterministic uniformly dispersed lethal-dose model
before choosing plant settings. This is a numerical source audit, **not an
approved balance patch or validation on held-out data**. All calculations
below assume a steady treatment temperature, zero initial injury, no
recovery during exposure, identical physiological state within a cohort,
and an ideal uniform distribution of hashed plant ranks. Actual RimWorld
sampling and 60-day-calendar conversion are not included.

### W07/W08 Kharkov winter wheat: structural limit of uniform thresholds

The [W07 duration-series CSV](Research/WinterWheatW07DurationSurvival.csv)
records **secondary-review-quoted** survival at constant -16°C: 80, 63,
53, 33, 17% after 1, 3, 6, 12 and 24 hours. The W07 original
survival-count table, group denominators and uncertainties have not been
obtained; W08 is *not* an independent cohort.

For any fixed positive severity under the current linear accumulator,
the individual lethal hour is a uniform multiplicative threshold:
`t_lethal = t50 * (1 + w * (2u - 1))`,
`0 <= u <= 1`, `0 <= w < 1`. Here `t50` is the
**cohort-median lethal exposure in hours under this temperature only**,
not a CCTO XML calibration. In the idealized large-cohort limit,
`S(t) = clamp((1 + w - t/t50) / (2w), 0, 1)` for `w > 0`
(and a step at `w=0`).

- Requiring both S(1h)=0.80 and S(24h)=0.17 puts both points inside the
  uniform ramp. Algebra requires **w = 23 / 15.06 = 1.5272**,
  which violates the valid API range `w<1` and would give negative
  low-end lethal doses. At any valid `w`, these two exact fractions
  **cannot simultaneously be reproduced** by the fixed-state model.
- A numerical **unweighted five-point least-squares lower bound** over
  `t50>0, 0<w<1` occurs as `w -> 1`, `t50≈7.8061h`,
  yielding idealized predicted survival
  **93.59/80.78/61.57/23.14/0%** at 1/3/6/12/24h,
  **RMSE≈13.86 percentage points**. Boundary `w=1` itself
  is invalid in the Draft XML API; values are a limiting *infimum*,
  not an admissible optimum or proposed XML values.
- Existing in-sample W07 CSV comparators:
  one-parameter exponential **7.77pp RMSE**,
  two-parameter Weibull **1.59pp RMSE**. They use rounded,
  secondary-reported points and unverified counts. This comparison
  **does not choose a universal Weibull distribution or disprove a
  common nonnegative severity accumulator**; time-course shape and
  temperature-severity law are distinct parameters.

**Decision:** do not lock `coldDamageLethalDoseVariation` as a
source-calibrated uniform distribution for winter wheat. Retain stable
per-plant ranks, but make the threshold distribution a separately
audited *optional class choice* only if additional matched observations
support it. No new C# CDF implementation is authorized by this fit.

### M01 maize: two matched median points do not identify a severity law

The [existing M01 hypothesis CSV](Research/MaizeM01DegreeHourIdentifiability.csv)
uses Lindsey (2024) quoting Buican (1969): approximately **50%
seedling mortality** for -2°C × 48h and -6°C × 2h. Under the
**conditional assumption** of a common cohort, fixed state, matching
post-treatment endpoint, and linear severity
`h(T) = max(0, Tonset - T)`, equating median lethal doses gives

`48 * (Tonset + 2) = 2 * (Tonset + 6)`,
hence **Tonset = -84/46 ≈ -1.8261°C**, **D50 ≈8.3478
°C·h**, and an **unmeasured prediction** at -4°C of **3.84h**.
These are algebraically fitted *hypothetical* quantities, not measured
onset or approved XML. Shifting onset +0.1°C while keeping both
study durations fixed changes the weak-frost dose from ~8.35 to
~13.15°C·h, but the strong-frost dose only to ~8.55°C·h;
the fit is sensitive near -2°C. The already-tracked CSV also shows
different severity exponents reproducing the *same two observed
medians* but yielding distinct -4°C forecasts.

Same-species M03 independently reports **irreversible seedling death**
after prolonged **+0.3±0.3°C** chilling, whereas the M01 linear
onset above implies **zero injury** there. Those sources do not
share verified cultivar/stage/history, so their parameters must
**not** be fitted as if a single 3-point trial. Together they
identify a **physiological scope gap**: an exclusively
below-freezing onset cannot represent prolonged above-zero chilling
for at least some maize seedlings.

**Decision:** preserve separate severe-frost and nonfreezing-chilling
regimes as model *candidates*, not necessarily separate runtime
components; do not select onset, exponent, recovery or dose from two
rounded mortality observations.

### R02 rice: physiological conditioning is structurally absent

[Sagehashi (2023), DOI 10.1080/1343943X.2023.2236344](https://doi.org/10.1080/1343943X.2023.2236344)
reports 10-day-old rice seedlings exposed to **4°C for 10 days**:
**0% survival** unacclimated versus **88–100% survival** for
six cultivars preconditioned at **12°C for 5 days**, evaluated after
warm recovery. The original study attributes this difference to
pre-exposure chilling acclimation.

With the same 4°C exposure and the Draft's fixed
onset/dose/`w`, the initial-zero-damage cohort has the same
simulated survival distribution in both treatments: hashed
individual variation does *not* encode the prior 12°C
preconditioning. Even if an onset above 4°C produces injury,
a **static** profile does not reproduce the 0% versus 88–100%
conditioning contrast. These percentages are specific
experimental outcomes, not universal population probabilities.

**Decision:** realistic rice seedling chilling requires a
physiological-state distinction *or an explicitly acknowledged
single-state approximation*. Neither an across-the-board 4°C
death setting nor an unmeasured acclimation multiplier is
approved. Do not convert 10 laboratory days into 10 game days
without an explicit time-scale decision.

### Shared outcome and bounded next design gate

| Species evidence | What is mathematically supported | What remains non-identifiable |
|---|---|---|
| W07 via W08 winter wheat | Fixed-state **uniform lethal thresholds** cannot closely match the *reported* -16°C duration curve; in-sample limiting RMSE ~13.86pp | Source counts, uncertainty, independent conditions, class-wide survival CDF, temperature function |
| M01/M03 maize | Two approximate freeze-median points admit multiple severity laws; prolonged *above-zero* chilling can be lethal under other experimental conditions | Physiological onset, exponent, cross-condition cohort identity, recovery rate, distribution width |
| R02 rice | Measured acclimation treatment changes cohort survival **at the same 4°C exposure** | Quantitative acclimation kinetics, temperature/duration survival surface, game-calendar translation |

**Implementation boundary:** the existing cheap opt-in sparse accumulator and
persistent per-plant rank can remain a **Draft mechanism**. Do not
commit to one universal distribution family, a single chilling/frost
severity curve or 6 runtime categories from this comparison. Grouping
plants by study/response type is for *evidence triage*, not proof
of six implementation laws. Keep existing production
Vanilla/MO 48 finite thresholds and 30 dormancy behaviors unchanged;
consumer plants remain owned by AMJC/AMJE/CCTO-for-VPE.
No XML values, code semantics, game launch or Workshop graphs are
approved by this analysis.


## Distribution versus temperature severity: model-identifiability audit (2026-10-21)

**Scope:** compare what can be explained by (A) changing **individual lethal-dose distribution** while keeping the existing *additive linear accumulated exposure*, versus (B) changing **temperature severity** while leaving that distribution intact. This is mathematical in-sample diagnosis based on the existing [W07/W08 dataset](Research/WinterWheatW07DurationSurvival.csv), not a calibrated plant profile or approval to modify the experimental C# API.

### Separate the three biological/model axes

Let `H(t) = integral[g(T(t), state(t)) dt]` be nonnegative **accumulated cold exposure**. A persistent plant rank `u` maps to a fixed positive **lethal exposure** `Q(u)`; the plant dies when `H(t) >= Q(u)`. Cohort survival at exposure `H` is `S(H)=1-F_Q(H)`.

- **Temperature severity `g(T)`** determines how fast exposure accumulates at different temperatures. At **one constant T**, `H(t)=g(T)*t`. Changing just `g(T)` only rescales the **time axis**, so it cannot fix the shape of a survival curve at that temperature if the per-plant threshold distribution is otherwise unchanged.
- **Susceptibility distribution `F_Q`** determines how many plants die early or survive late at a fixed dose. Its shape can change the **survival curve at one temperature**, without changing the cheap existing accumulation or adding repeated random rolls.
- **Physiological state** (age, acclimation, deacclimation, organ protection) changes the temperature response and/or effective threshold over prior history. It cannot be identified from a fixed-condition duration curve, and is not equivalent to persistent plant-ID variability.

A time-nonlinear exposure `H(t) = [g(T)*t]^q` at constant T is a **fourth, different model intervention**: it does not just change the temperature-severity function `g(T)`; its non-additivity across stopped/restarted freezes generally requires event-duration or physiological-state handling.

### W07 via W08: one-temperature shape comparison

For Kharkov winter wheat, the **five rounded secondary-reported** -16°C whole-plant survival points are 80/63/53/33/17% at 1/3/6/12/24 hours. Assuming uniform fixed initial state, no interim recovery, uniform weighting and a constant -16°C signal, the following fits use *only those five points*. The original 1975 survival count/uncertainty table remains unchecked. Original W07 and review W08 are **one** trial, not independent evidence.

| Candidate at constant -16°C | Parameters fitted | RMSE, percentage points | Interpretation |
|---|---:|---:|---|
| **Additive linear exposure + bounded uniform lethal dose** (current Draft form) | 2 (median time, spread) | **13.86** (limiting boundary `w->1`) | Even best possible valid-`w` limit cannot reproduce both early and late reported survival; `w=1` itself is forbidden |
| Additive linear exposure + **exponential lethal-dose distribution** | 1 (scale) | **7.77** | Better shape, no extra time-state; not proof that deaths occur by independent repeated hazards |
| Additive linear exposure + **log-logistic lethal-dose distribution** | 2 (scale, shape) | **3.00** | Better illustrative finite-data fit, heavy tail not validated |
| Additive linear exposure + **Weibull lethal-dose distribution** | 2 (scale, shape) | **1.59** | Best of the shown *two-parameter distribution choices*, within the five fitted data; not plant-class validation |
| **Nonadditive power-of-time exposure** + bounded uniform lethal dose | 3 (median time, spread, time exponent) | **1.45** | Numerical alternative with an extra free coefficient, *not* changing `g(T)` alone; not valid additive degree-hours or demonstrated physiology |

Derived predictions and fit coefficients are retained **only as candidate computations** in [WinterWheatW07DurationSurvival.csv](Research/WinterWheatW07DurationSurvival.csv). In particular the 3-parameter nonadditive illustration uses `S(t)=clamp((1+w-(t/t50)^q)/(2w),0,1)`, with `t50~6.218h`, `w~0.637`, `q~0.264`. This can match the five fitted percentages to ~1.45pp RMSE, but it would behave differently for two separate cold events totaling the same duration; interpreting `q` as a temperature exponent would be a category error.

The Weibull comparison instead fits `S(t)=exp(-(t/10.500h)^0.6567)` for **this treatment only**: an inverse-CDF lethal-dose transform could theoretically use the existing persistent per-plant rank without a new timer. However **unbounded distribution tails** can create exceptionally hardy simulated plants, especially when extrapolating beyond the five observed treatment durations; truncation/tail behavior, save compatibility and biological correspondence are unsolved. A good 5-point in-sample fit is **not sufficient** to select this as the CCTO default. Do not present distribution parameters as a measured LT10/LT90 family.

**Identifiability finding:** with only one constant temperature, an altered lethal-dose CDF and a nonadditive time function can produce almost the same survivorship. The observed curve alone cannot decide which represents biology. The cheap additive accumulator with a **candidate alternative stable-threshold distribution** is the **smaller engineering change**, not yet an approved physiological model.

### M01 maize: distribution changes do not identify temperature severity

The separate -2°C/48h and -6°C/2h ~50%-mortality points constrain the **ratio** of severity at two temperatures **conditionally on identical cultivar, stage, state and endpoint**. Median lethal time cancels the cohort distribution if it is the same across treatments. They do not identify a unique onset and severity exponent simultaneously. As the [existing M01 candidate table](Research/MaizeM01DegreeHourIdentifiability.csv) demonstrates, linear and quadratic severity hypotheses both fit those two medians and disagree about an unmeasured -4°C exposure. Changing the Weibull/log-logistic **distribution** cannot resolve that temperature-law ambiguity.

M03's +0.3±0.3°C prolonged chilling and M04 genotype-dependent mortality are separate protocols, **not third/fourth observations of the same cohort**. Neither establishes a crop-wide damage onset, variance, dose or acclimation state transition. Do not combine the studies into one synthetic temperature-duration surface.

### R02 rice: neither static parameter family models the treatment contrast

The [original Sagehashi 2023 study](https://doi.org/10.1080/1343943X.2023.2236344) found **0%** survival without conditioning and **88–100%** after 12°C/5d preconditioning when both subsequently faced 4°C/10d. **Neither a changed `g(4°C)` nor a changed fixed within-cohort CDF alone** can give different expected outcomes for the two otherwise identically modelled cohorts: a memory of physiological conditioning, or explicitly separate known experimental states, is needed. It would be inaccurate to introduce a universal arbitrary acclimation multiplier on this single comparison.

### Bounded decision for PR #24

1. **Retain** the sparse, opt-in additive exposure engine and stable plant identity as an *engineering prototype*, preserving all fixed-threshold and dormancy production defaults.
2. **Do not change temperature-severity exponent** merely to improve W07's one-temperature survival curve. That curve provides no cross-temperature calibration; M01's two median points alone are underidentified.
3. **Prioritize threshold-CDF shape as the next bounded candidate comparison**, but do not declare Weibull, log-logistic, uniform spread, shape coefficients or lethal dose **approved**. Any eventual change must avoid extra RNG and address tail/extreme-individual behavior, existing normalized-injury save data, and deterministic save/load.
4. **Do not add** per-plant freeze-duration, daily acclimation counters, repeated hazard rolls or six distinct runtime engines from these fits. If rice chilling is an initial playable target, a separately bounded acclimation/state decision is necessary first.
5. For a real numerical activation, obtain sufficiently documented **same-stage, same-cultivar whole-plant** mortality/survival across temperatures and durations, with sample counts and the relevant thermal signal. Explicitly translate experimental vs RimWorld time without distorting measured endpoints. No game launch is justified solely by these unapproved numbers.

**Status:** source-data fitting only, not C# / XML or gameplay validation. Existing 48 finite-death + 30 dormancy Vanilla/MO Defs remain untouched; AMJ/VPE consumers keep their own numeric ownership. Draft PR #24 must remain Draft; no Steam/Workshop graph or release.


## W07 Weibull transfer and tail-risk gate (2026-10-21)

**Question:** Does the very good five-point Weibull fit in winter wheat W07
justify using a Weibull lethal-exposure distribution in all CCTO plants?
**Answer: no**; the fit is an in-sample description of a single
cold-acclimated Kharkov wheat protocol, and important physiological
and gameplay extrapolations are uncontrolled.

### 1. Other whole-plant wheat evidence has a different model shape

The [Zheng et al. (2018) original study, W04](https://doi.org/10.2135/cropsci2018.01.0014)
tracks **four different field-acclimated winter wheat cultivars** exposed
for **multiple days** to **cultivar-specific cycling soil temperatures**.
Its author's published fit is:

`S(n) = (1-P)^max(0,n-K)`

where `n` is a consecutive experimental freeze *day*, `K`
is a cultivar-specific delay, and `P` is a subsequent daily
conditional death fraction. This is a **shifted exponential time
response**, not the W07 `S(t)=exp(-(t/lambda)^0.65669)`
curve at constant -16°C. The two studies differ in cultivar,
temperature signal (soil-cycle versus constant treatment),
acclimation and hour/day exposure protocol.

**Bounded numerical transfer diagnostic (NOT an independent validation):**
retain the *W07-fitted Weibull shape* 0.6566917, allow its time
**scale** to refit separately for each W04 cultivar, and compare the
result to the **W04 author's own regression-predicted whole-plant
survival at 3, 5, 8 days**, as preserved in
[W04 regression CSV](Research/WinterWheatW04Regression.csv). Unweighted
least squares over those three *derived* points gives:

| W04 cultivar | W07-shape Weibull best scale (days) | RMSE versus W04 fitted predictions (pp) |
|---|---:|---:|
| Jing411 | 25.006 | 6.50 |
| Nongda211 | 6.629 | 11.26 |
| Zhengmai366 | 11.773 | 4.88 |
| Yanzhan4110 | 31.787 | 9.24 |

These RMSE numbers measure **disagreement between two fitted model
families under incompatible protocols**. None of the 3/5/8-day
W04 survival values is an independently recovered treatment-level
observation; all are computed from the W04 author's rounded
`K/P` regression. Accordingly **DO NOT** report the above as a
held-out performance score, cross-study prediction failure, or
evidence that any one CDF is biologically true. The defensible
finding is **that W07's fitted shape is not a transferable fixed
parameter merely because both studies use winter wheat**.
A family-level fixed shape remains unapproved.

### 2. Extreme-percentile behavior of the W07 Weibull fit

Take the W07 -16°C in-sample parameters from
[W07 comparison CSV](Research/WinterWheatW07DurationSurvival.csv):
`scale=10.500235h`, `shape=0.6566917`.
If interpreted as one persistent lethal-exposure quantile under
constant -16°C, its inverse survival quantile is

`t(u)=scale * [-ln(1-u)]^(1/shape)`, where `u` is
the fixed individual rank (0 to 1, **not** a new hourly
RNG draw). This yields the following **model predictions**, not
measured plant tolerances:

| Cumulative fraction dead (`u`) | Modelled lethal time at -16°C | Relative to model median |
|---|---:|---:|
| 50% | 6.01 h | 1.0x |
| 90% | 37.39 h | 6.22x |
| 99% | 107.44 h | 17.88x |
| 99.9% | 199.22 h | 33.15x |
| 99.99% | 308.73 h | 51.38x |

The model still predicts **0.707% survival at 120h**,
whereas W07/W08 reports **no observed survivors** at 120h.
This is *not* a statistical contradiction without the study's
actual trial denominator (with 30 independent plants, the
Weibull prediction has about **80.8% chance** of zero
observed survivors). It is likewise **not** evidence that the
0.707% survival tail accurately reflects biology.

The Draft C# stable identifier maps plant IDs to an approximately
uniform **24-bit percentile** strictly below 1. Under this
*unmodified illustrative Weibull*, the **maximum possible
24-bit rank** could map to approximately **808 game hours
(~33.7 game days)** of continuous -16°C survival if
experimental hours were naively equated to in-game hours.
This extreme rank is **not guaranteed to occur in a colony**;
its significance is potential extrapolation, not a measured
in-game or real-world survivor. For 1,000 independent
uniform ranks the expected largest percentile is near
99.9%, corresponding to the model's roughly 199h tail.
Real plant IDs and generated populations are not truly
independent biological samples, and the 60-day RimWorld
calendar mapping is **unresolved**.

**Implication:** an unbounded inverse-CDF is not automatically safe for
large outdoor crop fields or prolonged winters. Arbitrarily
clipping at 99%, 99.9%, 120h or 2x the median would
change the inferred mortality curve and has **no empirical
approval**. The appropriate action is to record an explicit
upper-tail extrapolation policy once matched whole-plant
survival evidence and gameplay-time scaling exist.

### 3. Decision / stop condition for choosing the CDF

1. **Do not promote Weibull as a universal distribution**, do not
   use W07's 0.6566917 shape as a fixed wheat-class constant,
   and do not assign its quantiles to MO Wheat or other plants.
2. **Preserve** the cheap sparse additive injury accumulator and
   stable plant percentile as the experimental mechanism.
   A fixed individual inverse-CDF needs neither repeated random
   death rolls nor a per-plant exposure-duration timer.
3. **No additional fitting families or tail truncation
   implementation now**: the next missing evidence is one
   same-cultivar/same-state whole-plant experiment across
   durations and, for a true severity curve, multiple
   controlled plant/crown temperatures, sample denominators
   and warm-recovery endpoints.
4. Before later selecting a distribution, check **LT10/LT50/LT90
   and extreme survivorship**, source stage/acclimation,
   finite rank bounds, mid-save XML change behavior,
   and **game-time versus biological-time conversion**.
5. Keep PR #24 **Draft** with all production XML unchanged.
   This audit does not authorize new C#, a native RimWorld
   test, Workshop graph or publication.


## W13 measured 24-hour wheat frost bounds and 2D activation gate (2026-10-21)

### Original source and complete published survival grid

**W13 primary study:** Deryabin, Zhukova, Naraikina & Venzhik (2024),
*Metabolites* 14(4):199, [DOI 10.3390/metabo14040199](https://doi.org/10.3390/metabo14040199),
[original Table 2](https://www.mdpi.com/2218-1989/14/4/199).
The table reports **whole-seedling survival**, not isolated
leaf electrolyte leakage or percent growth reduction, for two
*Triticum aestivum* genotypes: freezing-tolerant Moskovskaya 39
and cold-sustainable Zlata, comparing the **22°C non-acclimated
control** with **4°C for 7 days pre-acclimation**.
The freezer target was **0/−3/−5/−7°C, each for 24 hours**.
The original 16 published observations, including source-reported
± values where supplied, are preserved in
[W13 original table CSV](Research/WinterWheatW13Deryabin24hSurvival.csv).

| Genotype and pre-treatment | 0°C ×24h | -3°C ×24h | -5°C ×24h | -7°C ×24h |
|---|---:|---:|---:|---:|
| Moskovskaya 39 / unacclimated | 100% | 35±2% | 0% | 0% |
| Moskovskaya 39 / 4°C ×7d | 100% | 97±3% | 65±4% | 15±2% |
| Zlata / unacclimated | 100% | 15±2% | 0% | 0% |
| Zlata / 4°C ×7d | 100% | 60±3% | 7±2% | 0% |

**Literal, source-supported 24-hour LT50 bounds only** (assuming
population survival declines between adjacent tested freeze targets):
M39 unacclimated **between −3°C and 0°C**;
M39 acclimated **between −7°C and −5°C**;
Zlata unacclimated **between −3°C and 0°C**;
Zlata acclimated **between −5°C and −3°C**.
These are **intervals containing 50% survival**, not
source-calculated LT50s, model fitted intercepts,
physiological damage-onset temperatures or candidate
RimWorld cold-death XML values. Exact 0%/100%
experimental outcomes do not imply mathematical population 0/1.

**What this actually unlocks:** a direct *species- and
genotype-matched* 24h temperature-response constraint and
an independent empirical demonstration of acclimation for
wheat. The matching freeze duration and growth context must
be preserved when comparing each row. W13 does **NOT**
supply more than one exposure duration in any physiological
state, and its cooling/ramp details, sample denominators,
plant versus air temperature and post-thaw survival criterion
require a dedicated full-method check before parameter adoption.
The variety and physiological state of generic
`DankPyon_Plant_Wheat` remain unspecified.

### Bounded CCTO candidate comparison

| Candidate | Same organism and frozen-stage whole-plant endpoint? | Temperature × duration surface? | Status |
|---|---|---|---|
| **W05, Jing411 winter wheat** (Zheng et al. 2018) | **Yes**: one named cultivar over 3 minimum soil temperatures and multiple freeze days | **Study performed**, but treatment-level alive/dead numbers and complete original temperature histories have not been recovered from accessible primary text; 1%/°C·d aggregate gradient cannot reconstruct cells | **Best direct 2D dataset acquisition target; no 2D fit allowed now** |
| **W13, M39 and Zlata wheat** (Deryabin et al. 2024) | **Yes**: 24h seedling survival by genotype and pre-acclimation state | **No**: four temperatures, **one** duration in each state; extra conditioning axis is not extra freeze-duration evidence | **Valid 24h temperature and acclimation bounds only** |
| **W07 via W08, Kharkov winter wheat** | **Yes**, but source percentages are quoted second-hand | **No**: five duration points at only **one** temperature (-16°C) | **Duration-shape comparison only**; no independent cross-temperature law or universal Weibull |
| **W10 Husiny et al. 2024** | **Yes**: cultivar-specific whole-plant LD50 and LT50 | **No joint 2D grid**: LD50 and LT50 from separately structured experiment arms; Borealis raw data still unavailable | **Separate marginal constraints**, not synthetic paired factorial survival |
| **C03 chickpea 5×5** | **Yes** for whole *Cicer arietinum* regrowth; **not** MO *Lens culinaris* | **Yes in experimental design**, but five -12°C text results plus approximate graph readings, with conspicuous non-monotone plotted samples and no raw counts | **Methodology stress-test only**; never transcribe as MO Lentil lethality |

### Implementation decision, grounded in the original data

- **CCTO-owned best candidate for the first complete 2D
  calibration is MO Wheat / *Triticum aestivum* using W05
  Jing411 as an explicit *winter-cultivar single-state* study,
  NOT the unidentified generic MO Wheat variety. W13 can
  constrain the likely impact of physiological conditioning
  under a separate experimental protocol, but W05+W13
  **must not be pooled as one synthetic cultivar**.
- For a genuine two-dimensional fatal-degree-hour profile,
  source original **W05 per-temperature/per-duration whole
  plant mortality cells**, sampling denominator/variation,
  field-acclimation dates, **measured crown/soil thermal
  histories**, and thaw/regrowth criterion are required.
  Without these, **no reproducible comparison of linear vs
  nonlinear severity or source-backed onset/dose is possible**.
- No CCTO crop meets the entire acceptance gate *from the
  evidence currently obtained*. This is a scoped finding
  about **accessible validated evidence**, not proof that
  qualifying studies do not exist or that biological
  realism requires a universal equation.
- Do not add CDF shapes, acclimation counters or plant XML
  values just to make use of W13; **existing 48 legacy
  finite-death and 30 dormancy plants stay unchanged**.
  The 60-day game-year transformation and natural
  map-air versus measured root/crown temperature are
  independent calibration blockers even after finding a
  complete laboratory survival grid.
- **Stop unbounded literature expansion:** use existing
  W05 as the precise source acquisition target. A later
  author-approved *ecological approximation* can be
  explicitly marked approximate, but cannot be described
  as measured 2D mortality or accepted implicitly through
  a low-RMSE graph.

**Research complete for the current first-model
qualification decision:** direct W13 empirical evidence
added, original 16 points indexed, **no production-ready
plant-specific numeric injury profile qualified**. No
runtime code, native tests, release, or Steam page updated.


### W05 exact treatment grid now indexed; observed outcomes still unavailable (2026-10-21)

The *bounded W05 primary source acquisition attempt* checked
[the publisher-indexed abstract and accessible excerpt](https://www.sciencedirect.com/science/article/pii/S0168192318301631),
[the DOI/publisher page](https://doi.org/10.1016/j.agrformet.2018.05.011),
[FAO AGRIS](https://agris.fao.org/search/en/providers/122535/records/65defde17c7033e84bec586f)
and [ResearchGate's publication entry](https://www.researchgate.net/publication/325699751_Effect_of_freezing_temperature_and_duration_on_winter_survival_and_grain_yield_of_winter_wheat),
plus bounded searches for an open author's PDF/accepted manuscript,
figure/table transcript or downloadable supplement. ResearchGate
exposes a "Request full-text" route, not the required
treatment-level observations; direct publisher full-text retrieval
was not available. **Do not claim access to the original numerical
figure, its individual counts, or supplemental raw data.**

The accessible original-study abstract explicitly names **Jing411
strong-winter wheat** (the accessible methods excerpt additionally
identifies 2014–2016 trials) and **20 total temperature × duration
treatments**, enumerated verbatim as study design in
[W05 treatment-availability CSV](Research/WinterWheatW05TreatmentAvailability.csv):

| Study soil minimum target | Freezing days | Reported treatment cells | Whole-plant mortality data retrieved |
|---|---|---:|---|
| -14°C (T1) | 2, 4, 6, 8, 10 | 5 | **0/5** |
| -17°C (T2) | 1, 2, 3, 5, 6, 7, 8, 9 | 8 | **0/8** |
| -20°C (T3) | 1, 2, 3, 4, 5, 6, 8 | 7 | **0/7** |

**0/20 retrieved is a source-access status; it is NOT 0%
biological mortality.** The CSV carries blank measured-value
fields with explicit `NOT_RETRIEVED` status, not fabricated zero
observations.

The paper's abstract reports increased **whole-plant** and
*tiller* mortality of about **1.0%** and **1.4%** per soil-
minimum-defined freezing degree-day **relative to its −12°C
FDD threshold**, respectively. These are summarized
regression slopes, **not** an identified independent
whole-plant temperature×duration mortality surface:
the appropriate intercept, linked per-cell values,
nonlinearities, denominators and exact soil histories
are not accessible in that text. The paper also reports
grain yield and spike losses; those are separate
physiological endpoints, not whole-plant death.

**Identifiability decision:** an exact design matrix
without the **response column** contains no information
to estimate lethality, distribution shape or relative
severity beyond the aggregate stated slope. The
simulated `coldDamageStartTemperature` is **not**
simply the paper's −12°C FDD reference because the
source integrates soil daily minima and CCTO currently
samples map temperature every plant callback.
No production onset, fatal dose, spread, recovery or
crown/air conversion is approved. No numerical fitting
against this missing response matrix is valid.

**Stop condition:** obtain the original W05 whole-plant
mortality figure/table/supplement for the 20 treatments
with at least endpoint and sample counts/temperature
protocol; alternatively choose an **explicitly
approximate gameplay profile** in a separate design
decision, not as source-calibrated reality. Do not
repeat blind literature searches or replace absent
mortality by FDD's single slope. Draft PR #24 remains
nonactivated and unreleased.
