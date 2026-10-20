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
unmapped gaps. The initial evidence inventory contains **36 source-mapped
Defs** (including nontransferable related-species/organ evidence) and
**42 without a mapped adequate reference**. The mapping is not evidence
that 36 plants are numerically calibrated. Published FAO frost *damage*
ranges are not whole-plant lethal values; postharvest product freezing
temperatures are excluded. Saguaro, Agave and sugarcane reveal substantial
species/organ/stage/duration effects, so do not migrate numerical settings
from one generic group label to all plants in that group.

**Next scientific pass:** map the remaining Def identities to actual taxa
before claiming species-specific values, collect whole-plant mortality or
post-freeze regrowth endpoints where possible, and bound group-level values
only when evidence supports the analogy. No new CCTO XML values are approved.
