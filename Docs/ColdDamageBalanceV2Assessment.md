# CCTO V2: independent scenario review of numeric reset (2026-10-22)

**Status: INCOMPLETE / NOT XML-READY.** V1's 48 profile-wide approval remains withdrawn. This V2 evaluation used the **predeclared targets** in [ColdDamageBalanceV2Targets.md](ColdDamageBalanceV2Targets.md), which was written **before** any V2 candidate numbers. The exact CCTO-owned population [V2 candidate table](ColdDamageBalanceV2Candidates.csv) has **78 unchanged identities**: **30** original dormancy, **41** death profiles without numerical candidates, **4** unapproved annual candidate profiles (tomato, cabbage, onion, pumpkin), **3** high-risk conditioning/intensity problems (rice, wheat, corn), **2** wild evergreen profiles whose previous recovery-logic defect has been corrected in Draft C# but whose numerical ecology is still unapproved. **No production XML opt-in has been made.**

## Unambiguous candidate meaning

All numerical results use *ideal, continuous temperature* segments in game-hours, initially zero injury, a uniform persistent individual lethal-dose multiplier, exact current linear injury formula, and, unless otherwise stated, updated recovery at max(0°C, injury onset). They do not include the live first-sample delay, 2,000-tick update cap, weather interpolation, stage/acclimation shifts or crown/soil thermal buffering. These are **gameplay modeling choices**, not source-derived coefficients.

For each temperature segment, normalized individual injury increases by `h * max(0, onset − T) / (D * [1 + w(2p−1)])`. In the **updated Draft C#**, only `T ≥ max(0°C, onset)` subtracts `recovery*h`. For a frost-hardy plant in `onset ≤ T < 0°C`, existing injury remains unchanged. This 0°C air threshold is a gameplay thaw proxy, not an experimental tissue temperature. These simulations are independent outcome checks, **not** simply recomputing chosen half-death times.

### Selected numerical candidates

| Def / condition | Onset °C | D °C·game-hour | Uniform halfwidth | Recovery /game-hour | Status |
|---|---:|---:|---:|---:|---|
| MO Tomato | **0** | **18** | **0.25** | **0.010** | CANDIDATE, not approved; existing C# compatible |
| MO Cabbage | **−3** | **72** | **0.25** | **0.007** | CANDIDATE, not approved; existing C# compatible |
| MO Wheat | **−1** | **42.67** | **0.25** | **0.016** | **BLOCKED**: a proxy for non-acclimated W13 wheat only; not the loaded generic crop |
| Vanilla Pine / MO Great Fir | **−31** | **3300** | **0.20** | **0.004** | **UNAPPROVED**: now matches thaw-floor Draft C#; no revised-runtime/native or cross-season ecological acceptance |

**No V2 coefficient is provided for rice or maize.** Their independent source constraints reveal a model-selection issue, not an arithmetic gap that can safely be filled with a guessed parameter. All **41** remaining fixed-death profiles have no V2 numeric candidate, including strawberry and potato where crown/top injury cannot be equated to whole-plant mortality. A species getting a blank coefficient is an **explicit decision to avoid the V1 failure**, not a missing CSV row.

## Actual output against the targets declared FIRST

| Predeclared case | Expected | V2 prediction | Audit result |
|---|---|---|---|
| S01 Tomato −1°C ×2h | ≥80% survive | **100%** | Candidate scenario satisfied |
| S02 Tomato −3°C ×8h | ≤30% survive | **0%** | Candidate scenario satisfied; severe frost danger retained |
| S16 Tomato −1°C ×8h, +10°C ×16h, 3 days | Some but not 100% die | **≈48.0% survive** | Candidate scenario satisfied; no total wipe |
| S03 Cabbage −4°C ×8h | ≥80% survive | **100%** | Candidate scenario satisfied |
| S04 Cabbage −8°C ×24h | ≤30% survive | **0%** | Candidate scenario satisfied |
| S05 Cabbage −4°C ×8h, +5°C ×16h, 5 days | ≥70% survive | **100%** | Candidate scenario satisfied; implies rapid recovery not proven biologically |
| S06 Wheat unacclimated W13 −3°C ×24h | **M39 35±2%, Zlata 15±2% measured** | **25.0% generic** | **BLOCKED**: hypothetical genotype proxy; cannot represent both cultivars accurately |
| S07 Wheat unacclimated W13 −5°C ×24h | 0% in both measured cohorts | **0%** | Conditional numerical agreement, not an approved crop |
| S08 Wheat preacclimated W13 −3°C ×24h | **M39 97±3%, Zlata 60±3% measured** | **25.0% generic** | **FAIL**: physiological state absent; difference cannot be a constant offset or random individual width |
| S09/S10 Rice at +4°C ×240h | condition-dependent **88–100% versus 0%** | **No V2 value assigned** | **BLOCKED**: no history/conditioning state, cannot satisfy simultaneously |
| S11/S12 Corn experimental onset sensitivity | both approximate M01 50%-death endpoints plus stable near-onset response | **No V2 value assigned** | **BLOCKED**: see structural mathematical conflict below |
| S13 Pine/Great Fir −35°C ×12h, −30°C ×12h, 15 days | ≥95% survive | **100% survive**, with or without false −30°C repair | Numerically safe for 15 days **without** −30°C artificial healing in updated C#; 1.6 game behavior and field ecology still unverified |
| S14 Pine/Great Fir constant −40°C ×15 days | ≥50% survive | **≈54.5% survive** | Candidate ecological stress threshold satisfied |
| S15 Pine/Great Fir after severe frost then thaw | Repair only when thawed; no unsupported −30°C repair | **Draft C# corrected**: no repair at −30°C, but +5°C repairs | **CODE FIXED; NOT GAME-TESTED; target still lacks physical tissue data** |
| S17 All 78 owned Defs | 48 death +30 dormancy and paired wild/cultivated parity | All DefNames retained; 2 annual trial tuples and 3 other conditional tuples only | **STRUCTURAL CHECK ONLY**, not quality acceptance |
| S18 Mild-night/thaw recovery | Show partial damage retention then warming-based recovery | Demonstrated by S16; rates remain chosen gameplay values | Conditional, not empirical restoration law |

The small 4,001 equally spaced percentile population used for the three-day and five-day illustrative values is a **deterministic ideal population**. Percentages are not game runtime observations or confidence intervals.

### Why corn cannot be repaired by rounding the two-point linear fit

An unchanged CCTO linear onset model giving exactly 50% mortality at *both* secondary M01 values,
`(-2°C, 48h)` and `(-6°C, 2h)`, forces:

```text
48 × (A + 2) = 2 × (A + 6)
A = -84/46 = -1.82608696 °C
D = 48 × (A + 2) = 8.347826 °C·h
T = -1.9°C => median death after ~112.94h
T = -2.0°C => median death after 48.00h
near-boundary ratio ~= 2.35 (fails S12 player-facing max 1.5)
```

No tweak of dose `D`, within-plant spread `w`, or recovery `r` can fix that *specific normalized median-time ratio* while holding the two laboratory-derived exact equalities. M01 endpoint times are approximate and the intermediate temperatures are unobserved, so strict equality to both endpoints is **not scientifically required**. An **optional accelerated intensity law** (for example a quadratic severity term for maize only, with new °C²·hour units and a documented no-data interpolation) could produce gentler near-onset behavior, but **must not be applied universally** or silently installed in the current degree-hour API. That potential runtime design is a separate decision.

### Why rice and wheat require a model/representation decision

- W13 measured same-genotype seedling survival at −3°C×24h of **35% control vs 97% acclimated** (M39), and **15% vs 60%** (Zlata). A no-history default cannot produce these conditional differences at the same `T,h`. The proposed W13-naive wheat numerical candidate is a **single physiological-state proxy**, never approval for all MO wheat. A range in individual thresholds reflects differences among individual plants, not physiological preconditioning.
- R02 after a 12°C×5d preconditioning has 88–100% alive after +4°C×10d, versus 0% for unacclimated seedlings in the contrasted case. Again, a single fixed injury curve cannot predict both with identical ambient history following the conditioning period. Prior seasonal exposure, plant stage and a possible two-regime chill-versus-freeze rate need source-constrained decisions before XML.
- Switching either rice or wheat directly back into fixed instant death is **not** a physiological calibration; it is only the current safe compatible deployment baseline until the new feature is complete.

### Additional V2 candidates after independent S19–S31 targets

The new [S19–S31 scenarios](ColdDamageBalanceV2Targets.md)
were **committed before** the following replacement candidates.
Their forecasts use the **same current C# linear injury
equation** with recovery only at `T≥max(A,0°C)`,
the original stable **uniform lethal-dose spread** and
no retroactive cold observation. Values are selected
gameplay hypotheses. Source endpoints **do not measure**
fatal dose, repair speed or within-cultivar spread.

| Crop | Chosen onset | Baseline median D | w | Recovery/h | Independent source/species limit |
|---|---:|---:|---:|---:|---|
| MO onion | **−3°C** | **21°C·h** | **0.25** | **0.009** | ON04 is a *second-hand* short −2.5°C seedling survival observation, not dose |
| MO pumpkin | **0°C** | **20°C·h** | **0.25** | **0.008** | Warm annual crop analogy, FAO02 classifies harvested product frost injury, **not whole vines** |
| Pine and Great Fir | **−31°C** | **3600°C·h**, previously 3300 | **0.20** | **0.004** | Ecosystem stress-test target, **not species root/crown tissue data** |

#### Independent predictions (not a game test)

| ID / specified condition | Expected before selection | Calculated V2 outcome | Numeric result |
|---|---|---|---|
| S19 Onion −2.5°C ×3h | At least 95% alive | **100%** | Target met, no inferred real mortality onset |
| S20 Onion −6°C ×8h | At most 30% alive | **21.4%** | Target met, severe exposure dangerous |
| S21 Onion −4°C ×8h then +5°C ×16h, 3 days | At least 70% alive | **72.5%** | Target met, only ~2.5 percentage points margin |
| S22 Pumpkin −1°C ×2h | At least 80% alive | **100%** | Target met |
| S23 Pumpkin −3°C ×8h | At most 30% alive | **10.0%** | Target met |
| S24 Pumpkin −1°C ×8h, +10°C ×16h, 3 days | 5–90% alive | **58.9%** | Target met |
| S25 Dormant strawberry crown −5°C ×12h | At least 95% alive | **No V2 plant-mortality coefficient** | **Blocked**: living crown and dormancy physiology not modeled |
| S26 Strawberry crown −10°C ×12h | At least 70% alive | **No V2 plant-mortality coefficient** | **Blocked**: ambient air != crown temperature |
| S27 Potato −2°C ×2h | Do not equate dead tops to whole-plant death | **No V2 plant-mortality coefficient** | **Blocked**: foliage vs tuber organ endpoint |
| S28 Pine/fir −35°C ×12h, −30°C ×12h, 60 days | At least 95% alive | **100%** with D=3600 | Target met, but extreme assumed daily weather frequency not verified |
| S29 Pine/fir four frost cycles, one +5°C full thaw day, repeat to 60 days | At least 95% alive | **100%** | Target met, assuming this artificial thaw sequence |
| S30 Two annuals: fatal frost → warmth → fatal frost | Killed plants cannot revive | **Same map component kills at damage≥1** | Logical implementation invariant; actual new numeric run untested |
| S31 Nature/source boundary | Product or bud injury not equal plant death | **Strawberry and potato blocked** | Endpoint distinction applied |

The two annually cycling fractions and 60-day evergreen
percentages are numerical idealizations over **5,001
evenly spaced susceptibility percentiles**. A live
RimWorld map has discrete weather and observation
callbacks, making exact percentages in actual play
dependent on those conditions.

### Pine/Great Fir: why the previous 3300°·h value was revised

The **predeclared S28 two-quadrum persistence target**
exposed unacceptable cold carryover in the previous
candidate. With zero subzero healing, the original
D=3300 gives **~81.8% survival** at 60 days under
−35°C ×12h and −30°C ×12h. The revised,
still **unapproved**, D=3600 gives **100% survival**
for the same hypothetical 60-day case. However,
at **−40°C continuously for 15 days** it also
changes the previously predicted survival
from **~54.5%** to **75.0%**. This tradeoff
was not hidden: the >50% safety objective S14
is retained, but the very-severe-cold lethality
is reduced. Longer or colder exposure will
eventually be fatal. Forest balance is still
unverified against actual seasonal thermal
frequencies and insulating cover.

During an individual `−35°C×12h` segment the
median plant now gains `48/3600=0.01333` injury.
A following `−30°C×12h` **holds** that injury;
`+5°C×12h` reduces it by up to `0.048`.
The code-level thaw behavior is implemented
on Draft, not yet real-game-tested after change.
This section does **not** approve the
combined species profile as factual pine/fir
cold tolerance, and it does not call the 0°C
ambient thaw proxy a tissue measurement.


### Post-selection temperature-sensitivity check — NOT a predeclared acceptance PASS

A separate **robustness check conducted AFTER selection** of
the onion/pumpkin and revised evergreen candidate numbers
tested modest temperature perturbations. These perturbation
cases **were not in the original S19–S31 predeclared targets**,
so they are independent adversarial diagnostics, **not**
retrospectively predeclared acceptance tests or evidence
that the coefficients were source-calibrated.

All three-night/five-thousand-plus-individual survival
fractions use an ideal uniform population, unchanged
daily warm segment and changed overnight temperature.

| Candidate / schedule | Original night | Original S | Perturbed night | Perturbed S |
|---|---:|---:|---:|---:|
| Onion, 3 cycles 8h cold +5°C 16h | −4°C | **72.5%** | −4.1°C | **54.8%** |
| Onion, same | −4°C | **72.5%** | −4.2°C | **37.1%** |
| Pumpkin, 3 cycles 8h cold +10°C 16h | −1°C | **58.9%** | −1.1°C | **39.8%** |
| Pumpkin, same | −1°C | **58.9%** | −1.2°C | **20.7%** |
| Pine/Great Fir, 60 cycles 12h cold / −30°C 12h | −35°C | **100%** | −35.1°C | **95%** |
| Pine/Great Fir, same | −35°C | **100%** | −35.2°C | **90%** |
| Pine/Great Fir, same | −35°C | **100%** | −36°C | **50%** |

**Material concern:** onion meets the prior ≥70% survival
criterion by a narrow margin only at exactly −4°C,
and small sustained nightly cooling causes a very
large change. Pumpkin's wider output spread also
means it is highly sensitive to repeated-night
temperature uncertainty. Pine D=3600 is **exactly
at the uniform population's zero-mortality
boundary** after 60 assumed −35°C nights;
its **100%** forecast must never be described as
a robust winter-survival guarantee.
Cold threshold, physiological state, and ambient
versus plant-tissue offset dominate these
temperature perturbations.

These results **do not automatically prove
the candidates wrong**, since long exposure
can legitimately amplify small variations.
They **do invalidate any unconditional claim
that meeting the original nine exact scenarios
is sufficient for adoption**. Without a
reviewed realistic distribution of nightly
temperatures and plant states, the three
candidates remain explicitly *UNAPPROVED*.
Do not tune them using another arbitrarily
invented resilience target and immediately
certify the retuned values in the same step.
The right next decision is first to determine
how much uncertainty and long-season crop loss
is an acceptable player-facing outcome.


### Seasonal-temperature example and climate-envelope risk (2026-10-22)

**Scope/evidence:** RimWorld generated climate is location-dependent
(latitude, biome/world tile, season, daily and diurnal shifts, weather
and game conditions). The earlier constant −35°C night/−30°C
day **60-day** stress experiment is *not* a verified real
60-day forest temperature distribution.

A [publicly posted RimWorld/HugsLib game log
(2026-02-17)](https://gist.github.com/HugsLibRecordKeeper/ba53570c4204c9ed922f863a5a37c4b1)
contains a **60-day generated daily low/high forecast**
for **one** heavily modded game tile, latitude
**22.2169°**, reported mean annual tile temperature
**16.1°C**, annual seasonal shift **11.41637°C**.
Its [original Lo/Hi numerical extract with full
provenance and exclusions](Research/RimWorldPublicOneTile60DayLoHi2026.csv)
has winter-period minimum **−1.09°C** and
highest daily high **36.02°C**. It is **a
game-produced diagnostic for one tile**,
*not* a record of 60 actually played days,
not a vanilla-only reproducibility run,
not weather-event history, not AMJ Environment
or WorldMap regional data, and not
a forest-biome population sample*.

**How the following sensitivity exercise was computed:**
reconstruct 24 synthetic hourly temperatures
for each logged daily Lo/Hi, by scaling
the **single-day** hourly `SunEffect` shape
in that same log (minimum at its hourly
sample 9, maximum at sample 20) to each
recorded daily pair. This reuses one
hourly shape for all days; **hourly time
series, intervening game weather and daily
sun geometry were not observed**.
Apply the current Draft linear injury,
uniform individual susceptibility and
`T≥max(A,0°C)` thaw-only recovery rule,
1h per segment, initial zero injury.
Numbers below therefore are **synthetic
model predictions informed by a game
generated single-tile annual Lo/Hi trace**,
not measured field or CCTO gameplay results.
No first-sample/update-tick latency,
sunlit tissue buffering or seasonal hardening
is simulated.

| Profile and scenario | Simulated survival | Interpretation |
|---|---:|---|
| MO Onion V2 (A=−3°C, D=21°C·h), 60 days of **original** one-tile climate | **100%** | One source tile rarely reaches onion onset; does NOT mean onion is safe on other maps |
| MO Onion: **days 42–44** forecast + imposed uniform **−5°C** three-day anomaly | **≈67%** | Artificial anomaly, no proof such an event occurred |
| Same original days + **−5.2°C** anomaly | **≈48%** | 0.2°C difference still causes substantial survival swing |
| Pine/Great Fir V2 (A=−31°C, D=3600°C·h), 60 days of **original** one-tile climate | **100%** | Irrelevant as a meaningful wild pine hardiness test on a mild tile |
| Pine/Great Fir: 60 days with **all original temperature points shifted by −40°C** | **100%** | An illustrative **hypothetical** colder site; no evidence that a natural forest tile has the same annual amplitude |
| Same synthetic climate shifted by **−43°C** | **≈93%** | Seasonal range/spread creates a very different outcome |
| Same synthetic climate shifted by **−44°C** | **≈55%** | 1°C change sustained for the whole year shifts survival strongly |
| Same synthetic climate shifted by **−45°C** | **≈15%** | Extremely cold hypothetical mean ~−28.9°C; no proven pine/fir wild-spawn habitat here |

**Important:** applying a uniform offset to a
latitude-22° game's 60-day curve **does not
generate a valid colder world tile**. In
RimWorld the annual mean, seasonal swing,
latitude, biome, random daily variation
and cold snap eligibility can change
together. The offsets here isolate
coefficient response only. The source
winter minima do **not** establish that
cold woodland regularly sees −35°C or
for how many nights. If the actual
forest-world climate distribution is
substantially milder, fitting 60-day
constant −35°C stress would make
pines unrealistically invulnerable;
if markedly colder, it might still
extirpate trees. **No pine numeric
acceptance follows from these samples.**

**Independent RimWorld game-condition
check:** the [wiki cold snap
mechanics](https://rimworldwiki.com/wiki/Events)
describe an approximately **−20°C outdoor
offset**, ramped over **12,000 game ticks
(4.8 in-game hours)**, for **1.5–3.5
game days**, with occurrence limited
to maps meeting its temperature
eligibility and a refire cooldown.
This is **not** equivalent to permanent
−5°C nightly cooling or to a cold tile's
normal seasonal annual mean. The source
log's reported summer twelfths around
**23–27°C** do **not** establish
eligibility for the ordinary random
cold-snap condition, and a forced
condition behaves differently from
normal daily variability. Future
player-facing crop risk comparisons
must separate (a) seasonal nights,
(b) cold-snap temporary incident
and ramp, (c) hard long-winter
climates, and (d) protected/roofed
fields and plant/root temperatures.

**Audited decision:** the **seven existing
V2 numerical trial values remain
UNAPPROVED**, and none should be
retuned based on a one-site forecast
or invented forest climate offset.
The next meaningful missing *input* is
a small representative **actual generated
forest-eligible cold tile daily Lo/Hi
and season profile** (plus the relevant
world-gen temperature/biome/site
settings); ideally compare one
short-growing-season agricultural tile.
Collecting and verifying that input
does **not** itself require 78
independent native crop runs or any
change to the existing C# API.
Once available, compare the already
frozen V2 candidate outputs before
considering a modification. Do not
call a log of forecast extrema an
actual hourly trace, and do not call
a synthetic cold-offset result a
verified real forest distribution.



### Original RimWorld 1.6 cold-forest and farming tile generated calendar (2026-10-22)

**Actual original-game data was collected and verified** in
[Project Actions #39776410000](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39776410000),
manual command `/amj ccto-climate-16`.
RimWorld Linux **1.6.4871 rev598** generated one
world with fixed seed
`CCTO-NaturalColdClimate-20261022`,
30% coverage, then generated one *unmodified*
BorealForest map. **483 natural Vanilla pines**
(`Plant_TreePine`) were present on that map.
The original Vanilla
`TileTemperaturesComp.GetOutdoorTemp`
was sampled at **1,440 calendar hours
(60 days) per real selected tile**, giving
**4,320 original-game world temperature
values**, no synthetic site temperature
offsets. The compiled test quickstart
source at CCTO commit
`23f3d9ac07a336c36fba3e32382612bec52e6d8f`,
original results SHA256
`6eac0d50124fbd622d37737abd1a86223b3649b27133e3e9349a43797e37b8c5`.
Actions artifact ID **12424418617**
holds the unrounded *full hourly CSV*
plus game/version and log/Quickstart proof.
The archival [60×3 derived daily minimum
and maximum](Research/CCTOVanilla16ActualWorldClimateDailyLoHi.csv)
preserves a self-contained daily subset.

**Sampling limitation:** using `DebugSetTicksGame`
and an original world temperature cache
does **not** advance a real loaded map
for 60 days, evolve actual weather or
generate cold snap incidents. This is
the original RimWorld **world/tile
season-and-hour temperature component**
evaluated at 60×24 dates, *not* a
complete playthrough observation.
The world/map's original biome,
terrain, annual tile temperature and
latitude were **not overwritten**.
The three sites were deliberately
selected from a real seed:
cold-percentile and median qualifying
**BorealForest** tiles with the
pine Def's positive wild-biome weight,
and a cool **TemperateForest**
farming-candidate tile. This is a
**three-tile purposeful sample**, not
random regional frequency evidence,
a synthetic transform of a tropical
world nor AMJ Environment/WorldMap climate.
**Full Medieval Overhaul was not loaded**;
Great Fir's own native wild distribution
remains unverified.

| Original world tile | Biome / sampled world annual mean | Actual seasonal 1h sampled range | Time below −31°C | Pine generation |
|---|---|---:|---:|---|
| #44000, lat 53.1905 | BorealForest, **−5.536°C** | **−37.835 to +26.670°C** | **92/1,440h**; only **11h below −35°C** | Eligible biome, this specific tile was **not loaded as a map** |
| #40777, lat 49.2266 | BorealForest, **−1.370°C** | **−32.647 to +29.621°C** | **6/1,440h**; **0h below −35°C** | **483 pines naturally spawned on its actual map** |
| #2851, lat 45.0841 | TemperateForest, **+5.672°C** | **−23.709 to +34.605°C** | None | Farming candidate tile (world tile only, not a loaded map) |

**Independent V2 survival application.**
Do not use the mathematical world-temperature
probe as proof of current deployed CCTO
crop XML or actual live tick scheduling.
The postprocessed formula used the existing
Draft `ColdDamageProgress.Advance`
for each calendar **hour** at exactly
the recorded temperature, initial
zero damage, recovery only for
`T≥max(onset,0)`, and **10,001 equally
spaced lethal susceptibility ranks**.
Once injury reaches 1, a plant is
irreversibly killed. No first-sample
or 2,000-tick interval cap is simulated
because the probe samples single
2,500-tick hours, not actual map ticks.

| Candidate | Natural boreal cold tile, 60d | Natural pine-bearing boreal median tile, 60d | Marginal crop tile, 60d without harvest |
|---|---:|---:|---:|
| Vanilla Pine / proxy MO Great Fir, A=−31°C D=3600°C·h w=0.20 | **100% survive** | **100% survive** | 100% survive (not representative natural pine cultivation) |
| MO Onion A=−3°C D=21°C·h w=0.25 | 0% | 0% | **0%**, first deaths day **28** |
| MO Cabbage A=−3°C D=72°C·h w=0.25 | 0% | 0% | **0%**, first deaths day **28** |
| MO Tomato A=0°C D=18°C·h w=0.25 | 0% | 0% | **0%**, first deaths day **25** |
| MO Pumpkin A=0°C D=20°C·h w=0.25 | 0% | 0% | **0%**, first deaths day **25** |

**Do not misread a 60-day annual crop
overwinter failure as a farming balance
FAIL:** the trial assumes every annual
plant remains planted through winter
and no harvesting, resowing, greenhouses,
solar cooling, roof or warm rooms.
On the crop tile, **all four candidates
survive 100% of the first 20 calendar
days**, and all survive first 25
days from this particular start date.
However **this alone does not prove
the crop matures/harvests** in that
window; `minGrowthTemperature`,
sunlight, fertility, sowing calendar,
harvest maturity and weather incidents
were not evaluated. When exposure
starts around day 20 with the rapid
fall toward winter, all four
candidate cohorts die by day 30.
Thus seasonal growing-window/frost
risk is plausible but not a
numeric crop approval.

For the actual forest calendar, the
new Pine/fir candidate has much
more headroom than inferred from
the earlier invented **60 days of
constant −35°C nights**: because
real cold-tile temperatures only
cross −31°C for 92 hours, median
annual peak injury is **≈0.0576**
and it fully recovers after spring
thaw. On the median naturally
pine-bearing tile, peak injury is
only **≈0.0016**. Both yield 100%
cohort survival. These are
**one sampled world and one year
without weather incidents**, not
proof that **D=3600** is calibrated
or optimal for wild forests:
the data show a lower-risk thermal
path, but cannot distinguish
3600 from many substantially
smaller lethal doses that would
also keep pines alive there.

**Single cold-snap-like diagnostic,
not a triggered vanilla incident:**
as an additional *hypothetical
robustness stress*, imposing
an abrupt −20°C for **84h**
starting on calendar day 38
of the cold forest tile gives
**100% survival**, median peak
injury ≈**0.4263** (unshifted
peak ≈0.0576). Unlike the real
ColdSnap game condition, this
artificial step has **no
12,000-tick onset ramp or
actual event eligibility**.
It is **not an observed incident**
and is not included in the
original hourly data CSV.

**Current appraisal:** actual
vanilla BorealForest viability
at one cold and one median tile
is now source-evidenced; the
V2 Pine coefficient is *not*
demonstrated necessary, nor
have 1.7/full-MO/AMJE
cold-biome ranges, map weather
incident frequency or
crop maturity been checked.
All seven candidates stay
**UNAPPROVED FOR PRODUCTION XML**.
No source table value should be
changed merely because the two
sampled Vanilla forests survived:
first compare multiple actual
world seeds and any **materially
different forest ecology or
modded biome** only if it changes
the proposed release decision.
A 60-day weatherless calendar
sample is not sufficient evidence
for a real cold-region mortality
coefficient.

## V2 acceptance verdict and next decisions

- **Numerical progression:** 4 unapproved annual candidates (tomato, cabbage, onion, pumpkin) meet their respective predeclared *gameplay* scenario preferences under ideal continuous air-temperature sampling. Wheat and Pine/Great Fir retain **3 conditionally computed but unapproved** candidate rows. The C# thaw threshold blocker for the latter two is corrected, but source/ecological and native-test acceptance remain open. The other **41 death profiles** (including rice, maize, strawberry and potato) have no V2 coefficients. The 30 dormancy definitions stay intact.
- **Not approved:** 7 proposed numeric rows **are not author-approved**. Their consistency with scenarios is narrow and must not be promoted to 48 "BALANCE PASS" claims. The rejected V1 review state remains `rejected_for_xml`, and all production XML opt-in stays blocked.
- **Material next step:** select a minimal, explicitly optional **physiological-state policy** for wheat/rice without confusing cold acclimation and injury recovery; revisit long-season wild flora outcomes and extend independently chosen weather/ecology targets to the remaining supported groups. Do not blindly inherit tomato/cabbage values across unrelated species.
- **Testing discipline:** these are offline analytic scenarios, no new game launch. Prior 1.6 native Pickle + actual .rws verify engine mechanics only, not V2 numerical/ecological gameplay.

## HP-only model transition — 2026-10-22

The experimental engine now applies cold exposure as ordinary Plant HP damage; it does not maintain a separate injury pool or restore HP on warming. All prior survival percentages in this report relied on reversible, normalized hidden injury. They cannot be reused as acceptance for the HP model. The candidate coefficients remain unapproved. The conversion from a full-HP lethal dose to HP damage is documented in Docs/Framework.md. No production XML or native game test has yet accepted these HP changes.
