# CCTO — REJECTED cold injury balance candidate V1 (archived)

**CORRECTIVE STATUS 2026-10-22: REJECTED FOR XML. Earlier 'DECIDED / LOCKED' claims have been withdrawn. All figures below are an archived failed proposal, not approved gameplay or measured biology. See [independent failure audit](ColdDamageBalanceFailureAudit.md) and [review state](ColdDamageBalanceReview.json). Production XML remains untouched; PR #24 stays Draft.**  
**Canonical exact 78-Def values:** [ColdDamageBalanceV1.csv](ColdDamageBalanceV1.csv).  
**Biological evidence and limits:** [ColdDamageCalibration.md](ColdDamageCalibration.md), [ColdDamageEvidenceLedger.md](ColdDamageEvidenceLedger.md).
**Existing live values:** [ImplementationTable.md](ImplementationTable.md).

This REJECTED V1 was a **proposed game-balance approximation**, not a
claim that every coefficient was measured in botanical experiments.
All 48 existing finite fixed-death CCTO-owned Defs receive a numerically
specified *design target* for cumulative injury. **All 30 existing
dormancy-only Defs keep dormancy and receive no cumulative injury values.**
No AMJ Culture/Environment, VPE, or other consumer-owned Def is included.

## Mathematical rule used to generate the rejected candidate

The already implemented optional engine uses the **current plant ambient
temperature** `T`, species injury start `A` and median lethal dose
`D` (degree × game-hour), plus a stable individual dose rank.
This document **does not change the linear formula**:

```text
if T < A:
    injury += (A - T) * elapsed_game_hours / individual_lethal_degree_hours
else:
    injury -= recovery_fraction_per_game_hour * elapsed_game_hours
clamp injury to [0,1]; injury >= 1 kills the individual plant

individual_lethal_degree_hours = D * (1 + w*(2*stable_rank - 1))
where 0 < stable_rank < 1 and the half-width w is a numerical
GAMEPLAY ASSUMPTION, not the between-cultivar standard deviation.
```

Thus under *uninterrupted constant temperature*, starting at zero injury,
the **median individual lethal exposure time** at `T < A` equals
`D/(A-T)`. The earliest/latest uniform-tail death is approximately
`(1-w)` and `(1+w)` times that duration. For a real map the
first cold sample is uncharged and inter-sample elapsed ticks are
bounded at 2,000; these ideal calculations are **balance
comparisons**, not promises of a precise in-game stopwatch.

**Normal class design rule:** preserve the previous immediate-death
temperature as a **reference temperature only** and replace the
instant kill with a deliberately long enough constant exposure.
Class onset is `legacyDeath + delta`; class lethal dose is
`delta * targetMedianHoursAtLegacy`. That old temperature no
longer acts as a separate instant-kill limit when cumulative mode
is enabled. Five plants have source/ecology-informed **explicit
exceptions** below. The 60-day in-game year is not an extra
multiplier on individual freeze hours.

## Previously proposed class-level numbers — rejected

| Class | Count | New injury starts above legacy death | Median cold at legacy death | Dose D (°C·game-hour) | Individual half-width w | Warm recovery per game-hour |
|---|---:|---:|---:|---:|---:|---:|
| TENDER — warm-season/tropical fragile | 12 | +2°C | 12 h | 24 | 0.25 | 0.012 |
| TEMPERATE — ordinary herb/root crop | 9 | +2°C | 24 h | 48 | 0.25 | 0.008 |
| HARDY — frost-hardy crops/groundcover | 10 | +3°C | 30 h | 90 | 0.30 | 0.006 |
| WOODY — frost-sensitive established trees | 3 | +3°C | 36 h | 108 | 0.20 | 0.004 |
| EVERGREEN — winter biome/natural forest | 3 | +4°C | **720 h (30 game-days)** | **2880** | 0.30 | 0.003 |
| FUNGUS — exposed fruiting body/cave plant | 3 | +1°C | 16 h | 16 | 0.20 | 0.006 |
| SUCCULENT — hardy dryland cactus/agave | 3 | +2°C | 20 h | 40 | 0.30 | 0.004 |
| Five explicit exceptions | 5 | See below | See below | See below | See below | See below |
| KEEP_DORMANCY — original dormancy plant | **30** | No onset | Not lethal in V1 | — | — | — |

**All 78 individual Def identities and computed absolute onsets are
in the canonical CSV.** The above class groupings and numbers are
gameplay policy; the underlying research does not identify common
recovery rates or a true uniform within-genotype susceptibility width.
A 0.012/h recovery clears one unit of injury in ~83.3
uninterrupted warm hours; 0.003/h clears it in ~333.3 hours,
assuming no further cold.

### Previously proposed exceptions — rejected

| Owned Def | Existing fixed death | V1 onset A | Dose D | w | Warm recovery/h | Median cold exposure at reference |
|---|---:|---:|---:|---:|---:|---:|
| `Plant_Rice` | −1°C | **+6°C** | **96** | 0.25 | 0.010 | 13.71h at −1°C |
| `Plant_Corn` | −2°C | **−1.826°C** | **8.348** | 0.25 | 0.012 | ~48h at −2°C |
| `Plant_Potato` | −2°C | **+1°C** | **16** | 0.25 | 0.008 | 5.33h at −2°C |
| `DankPyon_Plant_Wheat` | −6°C | **−1°C** | **84** | 0.45 | 0.006 | 16.8h at −6°C |
| `DankPyon_Plant_Sugarcane` | −5°C | **+3°C** | **60** | 0.25 | 0.010 | 7.5h at −5°C |

- **Rice**: the onset above zero is intentional to allow cumulative
  *chilling* injury even without a freeze. At +4°C continuously its
  **model median** is 48 game-hours. R02 actually observed stark
  non-acclimated versus pre-acclimated survival after **10 real days
  at +4°C**; the V1 48h **does not reproduce this protocol**,
  its acclimation state or its 10-day mortality. This is a
  compressed-world gameplay compromise, not a source-derived dose.
- **Corn**: the special frost curve uses the approximate secondary
  M01 ~50%-seedling-death points at −2°C×48h and −6°C×2h.
  Solving a **linear** injury model conditionally yields onset
  ~−1.826°C and median D ~8.348°C·h. **Both are approximate
  derived medians, not independent onset measurements**;
  the chosen w and recovery are explicitly gameplay assumptions.
  M03 documents non-freezing ~+0.3°C seedling collapse after
  prolonged exposure, which this frost-only corn profile **does
  not model**. This limitation is visible and accepted for V1,
  rather than silently pooling different experimental cohorts.
- **Potato**: fragile aerial stems/leaves, rather than deep tuber
  winter survival, inform the short-frost behavior. The onset,
  dose and susceptibility remain entirely gameplay values.
- **Wheat**: this is an **unconditional generic-winter-wheat
  gameplay compromise**, deliberately **not** either pre-acclimated
  M39 or Zlata parameter fit, and does **not** simulate
  hardening/dehardening. For continuous 24h freezer-target
  −3/−5/−7°C this uniform model predicts approximately
  **97.62% / 34.13% / 0%** survival; original W13
  4°C×7d-acclimated M39 measured **97%/65%/15%** and
  Zlata measured **60%/7%/0%**. The V1 predictions
  sit roughly between two different conditioned genotypes,
  **not** between their unacclimated responses. Treat the
  comparison as a sanity check only. W13 is a laboratory
  target temperature, not measured canopy/crown temperature
  for real farm-grown MO wheat.
- **Sugarcane**: fixes the gameplay problem of treating the
  plant as hardy to −5°C merely because its previous
  *instant* death setting was −5°C. With onset +3°C,
  continuous 0°C has **20h median death** under the
  abstract plant-level model. Root/ratoon overwinter
  survival and plant-stage variation are not simulated.

### Cold-forest and other ecological guardrails

- Pine and MO Great Fir **share identical numerical cold
  injury profiles**, with injury onset −31°C and median
  lethal exposure **30 game-days at −35°C**. At −40°C
  the model median is **2880/9 = 320h**. This long
  delay is intentional: natural trees in ordinary
  boreal winters must **not** be stripped by a
  sequence of normal −35°C nights.
- Temperate bamboo's same evergreen class uses onset
  −14°C and median 30 game-days at −18°C. No
  wind-chill, snow-insulated crown, sun exposure
  or below-ground organ survival is simulated.
- Original cultivated/wild counterpart pairs
  (Healroot, Mindwort, Poppy, Fleawort) preserve
  exact paired values. Their Defs are different
  but the represented plant behavior is the same.
- The three wild/cave fungus Defs in the fixed-death
  category are **not** unconditionally equated to
  dormant cave fungi: the exact legacy per-Def
  death threshold is retained as the reference.
- Preserving all 30 dormancy-only Defs unchanged
  avoids gratuitous winter tree/grass deaths and
  a premature perennial root/crown simulation.

## Illustrative outcomes exposing problems — NOT acceptance evidence

All values below are **ideal median** time in constant
air temperature (zero initial injury, no above-onset
breaks), not observed horticultural outcomes.

| Plant | Ambient temperature | Median exposure to death | Why it matters |
|---|---:|---:|---|
| Rice | +4°C | 48h | Prolonged chilling matters without a freeze |
| Rice | −1°C | 13.71h | Brief frost does not instantly kill all rice |
| Corn | −2°C | ~48h | Weak freezing can be endured longer |
| Corn | −6°C | ~2h | Hard frost is much more dangerous |
| Potato | 0°C | 16h | A cold night can be risky for green stems |
| Tomato | −1°C | 12h | Delicate annuals die after an extended freeze |
| MO wheat | −5°C | 21h | Moderate frost can produce mixed individual outcomes |
| MO wheat | −6°C | 16.8h | Sustained colder frost is more lethal |
| MO cabbage | −6°C | 30h | Short freezes need not kill cold-hardy crops |
| Saguaro | −6°C | 20h | Sustained desert freeze becomes dangerous |
| Pine / Great Fir | −35°C | **720h** | Normal severe winter should not wipe natural forestry |
| Pine / Great Fir | −40°C | 320h | Extended extreme exposure remains fatal |

Warm spells may reduce positive injury at the selected
class-specific recovery rate. Repeated overnight frosts
therefore have different outcomes from one uninterrupted
freezing event, as intended.

## Independent failure audit overrides former acceptance

**This entire 48-tuple candidate is REJECTED for production XML.**
Its individual values were created using circular reference-hour
targets and were **not** checked against independently chosen
player/ecology acceptance before being called "locked".
The prior implementation-order list and approval statement
are withdrawn. The only source of current readiness status
is [ColdDamageBalanceReview.json](ColdDamageBalanceReview.json)
supported by [ColdDamageBalanceFailureAudit.md](ColdDamageBalanceFailureAudit.md).

Keep the old table solely to reproduce how the mistaken conclusion
was reached. Do not use it to write Vanilla/MO XML, C# defaults,
Workshop claims or downstream consumer data. The 30 legacy
dormancy entries remain unchanged. New numbers require separate
evidence evaluation, independent typical-weather/long-winter
scenarios, accepted recovery semantics and actual review *before*
any "numeric values decided" report. A schema/math validator
returning success is NOT a balance audit PASS.

**Source status:** ON03 onion, W13 wheat, M01 corn,
M03 maize chilling, R02 rice chilling and other
entries in the evidence ledger constrain plausibility;
their genuine experimental endpoints do not measure
the 48 selected onset+dose+recovery+variation tuples.
V1 is an explicit game design decision motivated
by those sources and the existing gameplay baseline.
