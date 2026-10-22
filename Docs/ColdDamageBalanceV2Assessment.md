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

## V2 acceptance verdict and next decisions

- **Numerical progression:** 4 unapproved annual candidates (tomato, cabbage, onion, pumpkin) meet their respective predeclared *gameplay* scenario preferences under ideal continuous air-temperature sampling. Wheat and Pine/Great Fir retain **3 conditionally computed but unapproved** candidate rows. The C# thaw threshold blocker for the latter two is corrected, but source/ecological and native-test acceptance remain open. The other **41 death profiles** (including rice, maize, strawberry and potato) have no V2 coefficients. The 30 dormancy definitions stay intact.
- **Not approved:** 7 proposed numeric rows **are not author-approved**. Their consistency with scenarios is narrow and must not be promoted to 48 "BALANCE PASS" claims. The rejected V1 review state remains `rejected_for_xml`, and all production XML opt-in stays blocked.
- **Material next step:** select a minimal, explicitly optional **physiological-state policy** for wheat/rice without confusing cold acclimation and injury recovery; revisit long-season wild flora outcomes and extend independently chosen weather/ecology targets to the remaining supported groups. Do not blindly inherit tomato/cabbage values across unrelated species.
- **Testing discipline:** these are offline analytic scenarios, no new game launch. Prior 1.6 native Pickle + actual .rws verify engine mechanics only, not V2 numerical/ecological gameplay.
