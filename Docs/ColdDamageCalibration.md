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
