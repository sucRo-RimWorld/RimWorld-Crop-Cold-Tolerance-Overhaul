# CCTO V2: independent scenario review of numeric reset (2026-10-22)

**Status: INCOMPLETE / NOT XML-READY.** V1's 48 profile-wide approval remains withdrawn. This V2 evaluation used the **predeclared targets** in [ColdDamageBalanceV2Targets.md](ColdDamageBalanceV2Targets.md), which was written **before** any V2 candidate numbers. The exact CCTO-owned population [V2 candidate table](ColdDamageBalanceV2Candidates.csv) has **78 unchanged identities**: **30** original dormancy, **41** death profiles with no independently grounded new numeric values, **2** non-blocked candidate-only annual profiles, **3** high-risk conditioning/intensity problems (rice, wheat, corn), **2** wild evergreen profiles requiring a recovery model correction. **No production XML opt-in has been made.**

## Unambiguous candidate meaning

All numerical results use *ideal, continuous temperature* segments in game-hours, initially zero injury, a uniform persistent individual lethal-dose multiplier, exact current linear injury formula, and, unless otherwise stated, current recovery above injury onset. They do not include the live first-sample delay, 2,000-tick update cap, weather interpolation, stage/acclimation shifts or crown/soil thermal buffering. These are **gameplay modeling choices**, not source-derived coefficients.

For each temperature segment, normalized individual injury increases by `h * max(0, onset − T) / (D * [1 + w(2p−1)])`. At or above onset, the current model subtracts `recovery*h`. In a *proposed* thaw-gated evergreen variant, recovery is permitted only at temperatures **≥0°C**. These simulations are independent outcome checks, **not** simply recomputing chosen half-death times.

### Selected numerical candidates

| Def / condition | Onset °C | D °C·game-hour | Uniform halfwidth | Recovery /game-hour | Status |
|---|---:|---:|---:|---:|---|
| MO Tomato | **0** | **18** | **0.25** | **0.010** | CANDIDATE, not approved; existing C# compatible |
| MO Cabbage | **−3** | **72** | **0.25** | **0.007** | CANDIDATE, not approved; existing C# compatible |
| MO Wheat | **−1** | **42.67** | **0.25** | **0.016** | **BLOCKED**: a proxy for non-acclimated W13 wheat only; not the loaded generic crop |
| Vanilla Pine / MO Great Fir | **−31** | **3300** | **0.20** | **0.004** | **BLOCKED**: requires **no recovery below 0°C**, not current C# behavior |

**No V2 coefficient is provided for rice or maize.** Their independent source constraints reveal a model-selection issue, not an arithmetic gap that can safely be filled with a guessed parameter. The remaining 41 fixed-death species likewise have no V2 adoption candidate until they have a relevant climate, stage and organism-side analogy. A species getting a blank coefficient is an **explicit decision to avoid the V1 failure**, not a missing CSV row.

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
| S13 Pine/Great Fir −35°C ×12h, −30°C ×12h, 15 days | ≥95% survive | **100% survive**, with or without false −30°C repair | Numerically safe for *one* quadrum, but physiological recovery semantics still FAIL |
| S14 Pine/Great Fir constant −40°C ×15 days | ≥50% survive | **≈54.5% survive** | Candidate ecological stress threshold satisfied |
| S15 Pine/Great Fir after severe frost then thaw | Repair only when thawed; no unsupported −30°C repair | **Current C# FAIL**: T=−30°C is above −31°C onset and immediately repairs | **BLOCKED until minimal recovery semantics chosen and implemented** |
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

### Pine/Great Fir: minimal repair semantics prerequisite

Existing code recovers at every `T≥onset`; with onset −31°C, the tree recovers **at −30°C**. Assigning `recovery=0` to suppress this would prevent spring repair too, accumulating frost damage over years. To keep a simple scheme, one **possible** optional rule for later implementation is `repairEnabled = T≥max(0°C, A)`, a constant thaw floor requiring **no new per-plant state** and no per-species numeric field. Its biological accuracy is **not established**; 0°C ambient is a gameplay approximation for thawed tissue. Don't change the C# runtime or claim S15 PASS until this rule has been selected and separately evaluated.

## V2 acceptance verdict and next decisions

- **Numerical progression:** 2 non-blocked annual candidates (tomato, cabbage) satisfy their declared *gameplay* scenarios and have their assumptions exposed. Wheat and Pine/Great Fir get **3 conditionally computed**, but blocked, candidate rows. The other **43 death profiles** (including rice and maize) have no V2 coefficients. The 30 dormancy definitions stay intact.
- **Not approved:** 5 proposed numeric rows **are not author-approved**. Their consistency with scenarios is narrow and must not be promoted to 48 "BALANCE PASS" claims. The rejected V1 review state remains `rejected_for_xml`, and all production XML opt-in stays blocked.
- **Material next step:** select the smallest possible **recovery-condition** correction and physiological-state policy (especially wheat/rice) consistent with CCTO's simplicity objective; then extend independently chosen weather/ecology targets to the remaining supported groups. Do not blindly inherit tomato/cabbage values across unrelated species.
- **Testing discipline:** these are offline analytic scenarios, no new game launch. Prior 1.6 native Pickle + actual .rws verify engine mechanics only, not V2 numerical/ecological gameplay.
