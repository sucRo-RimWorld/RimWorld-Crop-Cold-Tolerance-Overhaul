# CCTO V3 — four-crop survival **and harvest-yield** screening (2026-10-23)

**Status: INDEPENDENT COMPARATIVE REVIEW, NOT NUMERICAL ACCEPTANCE.** **LATEST AUTHOR NUMERICAL-CANDIDATE DECISION (2026-10-23):** Cabbage **A−3.5°C/D240°C·h** and Onion **A−5°C/D120°C·h** are **selected V3 A/D candidates**. These are not native-game validated coefficients or authorization to convert the XML; 0.20 variation is still unapproved. Earlier comparison sections are preserved as dated history, and the newest decision supersedes their candidate-only language.  **Author direction approved 2026-10-23:** cabbage and onion prioritize survival with HP-driven yield loss instead of old S04/S20 70%-death goals. The other 46 HP A/D candidates, proposed ±20% variation and production XML remain **unapproved**; cabbage/onion A/D have separate candidate authorization. See [revised V3 criteria](ColdDamageBalanceV2Targets.md#v3-s04s20-supersession--author-approved-direction-2026-10-23). This is a deliberately compact follow-up to [ColdDamageHpV3Assessment.md](ColdDamageHpV3Assessment.md) and [ColdDamageHpV3Candidates.csv](ColdDamageHpV3Candidates.csv). No changes to original candidate tuples, production XML or dormancy definitions. The last section records a **shortlist for comparison only**, not decisions or approved defaults.

## What the existing HP implementation actually implies

The source model removes `maxHP * Σ[max(0, A-T) * elapsedGameHours] / (D*m)` from ordinary Plant HP. Here A is plant-specific damage onset, D is full-HP lethal degree-hours and m the individual dose multiplier. Damage is cumulative and does **not** auto-heal. Actual 1.6 V3 draft uses last-sampled temperature at up to 2,000-tick intervals, skips unobserved entry from warm to cold and keeps sub-HP remainder; all tables below instead use **ideal continuous piecewise-constant exposure**. No native live-temperature/plant harvest outcome is inferred.

Vanilla `RimWorld.Plant.YieldNow()` applies the current-HP factor `Mathf.Lerp(0.5f, 1f, (float)HitPoints/MaxHitPoints)` to other harvest factors (source reference [RimWorld/Plant.cs](https://github.com/josh-m/RW-Decompile/blob/master/RimWorld/Plant.cs)); an **alive** plant with HP fraction p consequently has approximately `(0.5+0.5*p)` of its otherwise available harvest yield, before rounding/RNG/growth/other factors. A plant **killed** by frost has no later harvest. Individual plant subclasses or other mods may override the computation.

For a fair crop-cohort comparison, define **field-level expected yield** as `mean((alive? 0.5 + 0.5*max(0,1-E/(D*m)): 0))`, normalized to an equivalent undamaged, harvest-ready group. This includes **zero yield for dead plants**, unlike simply averaging harvest yield of survivors. Integrate m uniformly over [0.8,1.2] as an explicitly **UNAPPROVED** V3 variability hypothesis; the code's current default is **w=0**, not ±20%. Below are numerical integrations of that hypothetical cohort—not measured crop fatalities or actual game-plant HP observations.

## Two independent player-visible outcomes per crop

| Plant | Event | Onset A | Dose D trial | Survivors at w=.20 | Expected whole-cohort yield at w=.20 |
|---|---|---:|---:|---:|---:|
| Tomato | light recurring: −1°C ×8h | 0°C | 21.5 | 100% | 81.1% |
| Tomato | light recurring: −1°C ×8h | 0°C | 40 | 100% | 89.9% |
| Tomato | light recurring: −1°C ×8h | 0°C | 60 (old V3) | 100% | 93.2% |
| Tomato | severe frost: −3°C ×8h | 0°C | 21.5 | 20.9% | **10.8%** |
| Tomato | severe frost: −3°C ×8h | 0°C | 40 | 100% | 69.6% |
| Tomato | severe frost: −3°C ×8h | 0°C | 60 (old V3) | 100% | 79.7% |
| Pumpkin | light recurring: −1°C ×8h | 0°C | 22 | 100% | 81.6% |
| Pumpkin | light recurring: −1°C ×8h | 0°C | 40 | 100% | 89.9% |
| Pumpkin | light recurring: −1°C ×8h | 0°C | 68 (old V3) | 100% | 94.0% |
| Pumpkin | severe frost: −3°C ×8h | 0°C | 22 | 27.3% | **14.3%** |
| Pumpkin | severe frost: −3°C ×8h | 0°C | 40 | 100% | 69.6% |
| Pumpkin | severe frost: −3°C ×8h | 0°C | 68 (old V3) | 100% | 82.1% |
| Cabbage | 5 light nights: −4°C ×8h (total 40h) | −3.5°C | 96 | 100% | 89.4% |
| Cabbage | 5 light nights: −4°C ×8h | −3.5°C | 160 | 100% | 93.7% |
| Cabbage | 5 light nights: −4°C ×8h | −3.5°C | 240 (old V3) | 100% | 95.8% |
| Cabbage | severe prolonged freeze: −8°C ×24h | −3.5°C | 96 | 18.8% | **9.7%** |
| Cabbage | severe prolonged freeze: −8°C ×24h | −3.5°C | 160 | 100% | 65.8% |
| Cabbage | severe prolonged freeze: −8°C ×24h | −3.5°C | 240 (old V3) | 100% | 77.2% |
| Onion | 3 light nights: −4°C ×8h (total 24h) | −3.5°C | 18 | 100% | **66.2%** |
| Onion | 3 light nights: −4°C ×8h | −3.5°C | 60 | 100% | 89.9% |
| Onion | 3 light nights: −4°C ×8h | −3.5°C | 120 (old V3) | 100% | 94.9% |
| Onion | harsher frost: −6°C ×8h | −3.5°C | 18 | 22.2% | **11.5%** |
| Onion | harsher frost: −6°C ×8h | −3.5°C | 60 | 100% | 83.1% |
| Onion | harsher frost: −6°C ×8h | −3.5°C | 120 (old V3) | 100% | 91.6% |

**Crucial contrast:** Tomato and pumpkin near D≈22 can meet the pre-V3 *fatality* preference, but under severe frost yield only **~11–14% of normal** across the field, not ~21–27% of harvest. Onion D18 produces **~34% cohort harvest loss** under three mild nights even while **all plants survive**, a consequence missed by mortality-only S21 checks. Under **w=0** (the actual default unless XML opts in), identical full-HP plants respond deterministically: e.g. hard-frost Tomato D21.5, Pumpkin D22, Cabbage D96 and Onion D18 all reach fatal full-HP exposure; the mid/old trial values remain alive in their respective severe events. Thus nonzero mortality percentages in this table must not be treated as guaranteed gameplay outcomes.

## Horticultural evidence and missing mapping

- [Ontario Ministry of Agriculture: extreme temperatures in tomato crops](https://www.ontario.ca/page/extreme-temperature-effects-tomato-and-pepper-crops): tomatoes suffer near-freezing chilling, freezing severity also depends on duration, variety, vigor, soil and recovery conditions; it does **not** establish a unique whole-plant fatal D for our event windows.
- [University of Maryland Extension: pumpkin harvesting](https://extension.umd.edu/resource/growing-pumpkins-home-garden): even a light frost can kill **vines** while leaving mature **fruit** usable; plant HP/fatality and physical fruit damage are different game entities. Generic CCTO cannot exactly represent fruit frost storage behavior.
- [University of Minnesota Extension: cabbage growing](https://extension.umn.edu/garden-and-home/yard-and-garden/gardening-in-minnesota/growing-cabbage): developed cabbage tolerates sub-freezing temperatures, while seedlings can be sensitive. One fixed HP-onset value inevitably averages plant growth stage.
- [University of Georgia Cooperative Extension: onion guide](https://extension.uga.edu/publications/detail.html?number=B1198&title=onion-production-guide): onions can tolerate light to heavy frost but hard freeze can injure scales and growing points. **Counterpoint:** [University of Minnesota Extension crop planning](https://extension.umn.edu/vegetable-growing-guides-farmers/crop-and-field-planning-tools-vegetable-farmers) lists onion among crops not frost-tolerant in its own production context. These are not controlled mortality datasets and should not be fused into a definitive, global onion threshold. Stage, cultivar, growing system and harvested bulb versus top tissue matter.
- Neither the cited extension materials nor the previous V2 targets justify precise survival endpoints like “70% of onion plants die at −6°C for 8 hours.” Likewise the old S02/S16 and S23/S24 paired severe/night events have **equal cumulative degree-hours** in a no-recovery model, so they supply only one mathematical lethality constraint rather than two independent experiments.

## Narrower next-pass comparison only — **NOT APPROVED NUMBERS**

Do not compare all arbitrary D values or further tune a nominal 20% mortality rate. To avoid expanding testing overhead, restrict the next choice of game-feel tradeoff to **two interpretable positions per crop** in idealized HP/yield outcomes. These are *selection alternatives*, not biologically verified D intervals:

| Crop | Compare (A fixed provisionally) | What the choice actually means |
|---|---|---|
| Tomato | **D21.5 versus D40** (A0) | Strong frost usually destroys the crop and almost all expected yield **versus** survivors take substantial but not fatal loss at that exact event |
| Pumpkin | **D22 versus D40** (A0) | Vine-death abstraction vs survivable large wound; mature fruit might still be harvestable in the real world despite vine death |
| Cabbage | **D96 versus D160** (A−3.5) | Sustained −8°C for a full day is devastating **versus** winter crop survives with considerable yield penalty |
| Onion | **D60 versus D120** (A−3.5), **set D18 aside** | Both survive these particular illustrative frosts; differ in repeated mild-night harvest impact (≈10% vs ≈5% yield lost) |

The **hardy-crop survival/yield direction was approved on 2026-10-23**, but **these numerical choices cannot be adopted** until onset uncertainty (±0.5°C), ordinary seasonal exposure, real sampling gaps and actual plant HP/harvest timing are assessed. The onion hard-frost *mortality target* S20 remains an unverified preference and must not force D18. Values may be revised outside this list if independent evidence appears; there is no false claim that these are tight physiological bounds.

### End-of-stage status

- 48 V3 tuples and common half-width 0.20 remain **unapproved**; CSV remains 2 active hard-frost failures (tomato/pumpkin), 46 otherwise unapproved after cabbage/onion's old S04/S20 criteria were superseded.
- 30 dormancy existing values and 48 fixed-death production XML remain unchanged.
- No new runtime C#, growing-stage simulation or cold recovery added.
- No game launch now. The compiled draft's technical PASS does not establish frost/yield balance; acceptance should be consolidated **after** gameplay tradeoffs and remaining species are selected.

## Earlier provisional direction — 2026-10-23 (superseded below; not an XML authorization)

**Propose one baseline to scrutinize next, rather than creating another parallel list of arbitrary doses:**

| Crop | Directionally preferred working profile A / D (°C, °C·h) | Explicit game-facing purpose | Previous comparison ruled out for this purpose |
|---|---|---|---|
| MO Tomato | **0 / 21.5** | Brief marginal frost leaves most plants standing; a sustained hard frost destroys most warm-season crop and expected field harvest | D40 treats −3°C×8h as universally survivable (~70% field yield), inconsistent with intended high vulnerability |
| MO Pumpkin | **0 / 22** | Treat tender *living vine* destruction during strong frost as the injury target, while recognizing existing detached fruit can survive | D40 likewise gives guaranteed vine survival at −3°C×8h, understating warm-season vine sensitivity |
| MO Cabbage | **−3.5 / 160** | Keep a hardy cool-season crop alive even through a prolonged hard freeze, but penalize its harvest heavily as ordinary HP declines | D96 kills ~81% of plants and reduces field yield to ~10% at −8°C×24h under assumed variation; that would make this winter-usable crop almost as fragile as tender species |
| MO Onion | **−3.5 / 120** | Favor survival and modest yield loss across repeated marginal frost; permit larger injury only under more severe/prolonged freezes | D60 doubles damage from ordinary repeated cool nights; D18 from prior mortality-only fitting would cause ~34% field yield loss despite all plants surviving |

**2026-10-23 author decision (direction only):** For hardy cabbage/onion, prioritize **survival and HP-related reduced harvest**, not 70% immediate mortality under S04/S20. Tomato/pumpkin severe-frost danger remains. Their candidate A/D, optional spread w=0.20, sample timing and actual economy remain **unapproved**. The old S04/S20 numeric mortality goals are retained historically but are **no longer active V3 gates**. Cabbage/onion CSV statuses were restored to `UNAPPROVED_HP_TRIAL`, not passed. Do not move comparative numbers into production XML.

### Source-verified MO HP magnitudes, avoiding ungrounded generic plant assumptions

Inspected the original Medieval Overhaul **1.6.2.2** user-provided source archive `3219596926.zip`, `About/About.xml`, `1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml` directly. The four `<statBases><MaxHitPoints>` declarations are **Tomato 85, Pumpkin 100, Cabbage 85, Onion 100**. Their declared `plant/harvestYield` base amounts are **8, 20, 10, 13** respectively. These are exact source XML values *before* any conditional compatibility patch, stat adjustment or active-game factor; never report them as experimentally loaded final HP.

`Plant.YieldNow()` in the publicly readable decompiled plant implementation applies `Mathf.Lerp(0.5f, 1f, HitPoints/MaxHitPoints)` *after* the crop growth multiplier and before harvest rounding. Using these source HP maxima, truncating integer damage in the same way as the CCTO direct-HP draft (full-HP death if HP reaches 0), integrating hypothetical uniform m∈[0.8,1.2] and assigning **zero yield to destroyed plants**, produces the following **expected fractions of initially undamaged, harvest-ready cohort yield**. They intentionally omit the original game sampler's warm/cold boundaries, other damage, live growth/harvest scheduling and subclass overrides:

| Working crop D | Frost sequence | At **w=0**: survival / whole-field harvest | At **w=0.20**: survival / whole-field harvest |
|---|---|---|---|
| Tomato D21.5 | −1°C×2h | 100% / 95.9% | 100% / 95.6% |
| Tomato D21.5 | −1°C×8h | 100% / 81.8% | 100% / 81.4% |
| Tomato D21.5 | −3°C×8h | **0% / 0%** | **20.9% / 10.9%** |
| Pumpkin D22 | −1°C×2h | 100% / 95.5% | 100% / 95.6% |
| Pumpkin D22 | −1°C×8h | 100% / 82.0% | 100% / 81.8% |
| Pumpkin D22 | −3°C×8h | **0% / 0%** | **27.3% / 14.3%** |
| Cabbage D160 | −4°C×8h, 5 nights | 100% / 94.1% | 100% / 94.0% |
| Cabbage D160 | −8°C×24h | 100% / 66.5% | 100% / 66.1% |
| Onion D120 | −4°C×8h, 3 nights | 100% / 95.0% | 100% / 95.2% |
| Onion D120 | −6°C×8h | 100% / 92.0% | 100% / 91.8% |

When **w=0** (the code's default), no field-level individual variability exists, so four similar plants with identical HP/experience may die simultaneously at a severe threshold. The hypothetical **w=0.20** is useful to compare a less abrupt field outcome, but the 20% value has no species-specific empirical calibration. A decision to use variation is distinct from choosing A/D, and remains pending.

**External interpretation anchors:** [UMN Extension crop frost categories](https://extension.umn.edu/vegetable-growing-guides-farmers/crop-and-field-planning-tools-vegetable-farmers) and [UMN season/frost calendar](https://extension.umn.edu/garden-and-home/yard-and-garden/gardening-in-minnesota/planting-vegetables-in-midsummer-for-fall-harvest) classify tomato/winter squash as frost-sensitive and cabbage as hardy. [UMD Extension pumpkin harvest guidance](https://extension.umd.edu/resource/growing-pumpkins-home-garden) separates vine death from fruit injury. [UGA Extension onion production](https://extension.uga.edu/publications/detail.html?number=B1198&title=onion-production-guide) reports light-to-heavy frost tolerance in one growing system but damage from hard freezes. These **do not** provide a universal lethal-dose in game-degree-hours, nor resolve variety, planting stage or a seasonal temperature series.

### Narrow remaining choice and stop

The author has resolved cabbage/onion's **gameplay direction**: hardy crops should generally survive representative hard frost while losing HP and yield. The formal V3 S04/S20 goals and candidate CSV have been reconciled. The illustrative D160/D120 and their onset temperatures remain unapproved; optional individual-dose variation and natural-season consequences require review. Preserve historical comparisons, proceed to other crop groups, and avoid copying any unapproved V3 trial into production XML or launching game CI merely because the direction is approved.

## Updated hardy-crop numeric comparison (2026-10-23)

**REVIEW ONLY — numerical approval still open.** Author approved hardy cabbage/onion survival and HP-related reduced harvest, NOT their temperature onset, lethal dose or dose spread. The earlier recommendation of cabbage D160 is superseded here. Each figure assumes ideal constant plant ambient temperature (2500 ticks/hour), undamaged mature harvestable plants, MO 1.6.2.2 MaxHP cabbage 85/onion 100, integer HP loss, no other damage or healing; living harvest uses the normal Plant.YieldNow() HP multiplier, dead yield zero. This is not a live weather/plant test.

| Trial crop / event | w=0: survival / field yield | w=.20: survival / field yield |
|---|---|---|
| Cabbage A−3.5/D160, −8°C ×24h | 100% / 66.5% | 100% / 66.1% |
| Cabbage A−3.5/D240, −8°C ×24h | 100% / 77.6% | 100% / 77.5% |
| Cabbage A−3.5/D160, −8°C ×48h | **0% / 0%** | **0% / 0%** |
| Cabbage A−3.5/D240, −8°C ×48h | 100% / 55.3% | 75% / 42.9% |
| Cabbage A−3.5/D240, −10°C ×24h | 100% / 67.6% | 100% / 67.3% |
| Onion original A−3.5/D120, −4°C ×8h, 15 nights | 100% / 75% | ≈100% / 75% |
| Onion alternative A−5/D120, same nights | 100% / 100% | 100% / 100% |
| Onion original A−3.5/D120, −6°C ×24h | 100% / 75% | ≈100% / 75% |
| Onion alternative A−5/D120, −6°C ×24h | 100% / 90% | 100% / 90.1% |
| Onion alternative A−5/D120, −10°C ×24h | **0% / 0%** | 50% / 27.3% |

**Narrowed comparison preference, NOT approval:** retain the currently unapproved **cabbage A−3.5/D240**, rather than replacing D240 with D160. D240 already causes meaningful −8°C×24h damage while resisting immediate wholesale wipeout over 48h. Inspect **onion A−5/D120** as a new option against the unchanged unapproved A−3.5/D120; original onset charges −4°C×8h for 30 nights as 120°C·h, meaning median **complete Plant HP loss** despite supposed overwinter resilience.

Relevant independent context: [Croatian winter cabbage field study](https://acs.agr.hr/en/issues/article/36) documents seasonal field persistence and greater damage near −10°C, and [Japanese winter cabbage leaf LT50 study](https://www.jstage.jst.go.jp/article/agrmet1943/49/2/49_2_91/_article/-char/ja/) finds acclimation-sensitive tissue tolerance, **not whole-plant mortality**. [Virginia Tech onion guidance](https://welbaum.spes.vt.edu/seedproduction/onion.html) differentiates frost tolerance from prolonged deep freezing, and [UGA guidance](https://extension.uga.edu/publications/detail.html?number=B1198&title=onion-production-guide) cautions about hard-freeze bulb/growth damage. These do not provide valid measured game-degree-hour A or D values.

**Variation ±20% not independently justified:** it provides staggered population death but also changes expected field yield near lethal doses. Under cabbage A−3.5/D240 at −8°C×48h, w=0 and w=.10 both predict 100% survival, whereas w=.20 predicts 75%. At a symmetric exact full-dose threshold, both w=.10 and w=.20 yield roughly 50% survival; that case cannot select the width. Keep implementation default **w=0**, and w=.20 as an explicitly **unapproved gameplay hypothesis** (not a scientific LT50 distribution). Do not enable a universal variation width or copy any HP tuple into production XML on this analysis alone.

**Unchanged:** all 48 V3 candidate tuples and their status, 30 dormancy defs and production 48 legacy fixed-death entries. Original game temperature sampling, seasonal sow/harvest, integer rounding interactions and save/HP behavior still require consolidated later acceptance, not an extra native run for each balance guess.

## Author-selected A/D candidates — approval boundary (2026-10-23)

The author approved the specific **V3 candidate numbers** after comparing two hardy crops under normal, strong and repeated cold. The numeric decision is narrower than enabling the whole opt-in HP model:

| Crop | Selected injury onset A | Selected full-HP cumulative lethal dose D | Historical option not selected |
|---|---:|---:|---|
| MO Cabbage | **−3.5°C** | **240°C·h** | D160 was more vulnerable to repeated strong cold |
| MO Onion | **−5°C** | **120°C·h** | A−3.5 produced excessive irreversible damage across ordinary −4°C nights |

**Selection status:** author-selected **A/D candidate**; these two fields are now the intended numeric baseline for subsequent V3 acceptance. The recommendation rests on independent gameplay comparisons with known caveats, not on a claimed laboratory LT50 or actual in-game yield test. The previously approved intent—prefer **crop survival and yield loss** over instant mass mortality—remains unchanged.

**Still excluded from this approval:** `lethal_dose_variation` half-width of 0.20 (unverified; current code defaults to zero), every other HP profile, production XML conversion, release, native tick/HP and actual .rws save acceptance. The CSV uses `APPROVED_A_D_CANDIDATE_VARIATION_PENDING` only on the two authorized rows and retains the unapproved 0.20 field as a proposal. Nothing about the authored selection validates its actual numerical impact at real map ambient temperatures or applies the selected values to the game before XML activation. Do not count previously proposed D160 for cabbage or old A−3.5 for onion as current choices.

## Independent spread-width comparison for V3 — 2026-10-23

**Approval status: NO WIDTH SELECTED.** This section addresses the author's follow-up request to compare **w=0, ±10%, ±20%** before finalizing per-plant variation. The approved A/D **candidate-only** tuples are MO Cabbage A−3.5/D240 and MO Onion A−5/D120. Tomato A0/D21.5 and Pumpkin A0/D22 here are **unapproved sensitivity trials**, not newly chosen A/D. The published XML remains fixed-death and uses none of these HP inputs. The C# implementation's default variation is w=0.

### Mechanics (existing implementation, no new code needed)

`ColdDamageProgress.LethalDoseMultiplier` maps each persistent Plant Thing ID deterministically to a stable percentile, then scales **lethal dose D only** by a uniform multiplier in [1−w,1+w]. It does *not* change onset A, simulate genetics, acclimation, plant age or microclimate, or reroll every session. One species can choose a different width from another through the already existing per-Def extension; **a common nonzero width for all 48 is not required**.

The analytic comparisons below assume original MO XML full HP (Cabbage 85, Onion 100, Tomato 85, Pumpkin 100), full starting HP, no injury healing, ideal constant ambient air temperature, integer HP loss, a continuous-uniform population of multipliers, and the Vanilla `Plant.YieldNow()` current HP factor (dead crop yield zero). Actual sparse <=2000-tick last-observation integration, first-cold/warm boundaries, room/microclimate and growing-stage scheduling may materially change real plants; none of these are native-game results.

| Exposure and A/D trial | w=0 alive / field harvest | w=.10 alive / field harvest | w=.20 alive / field harvest |
|---|---:|---:|---:|
| Cabbage A−3.5/D240, −4°C×8h for 5 nights | 100% / 95.9% | 100% / 96.1% | 100% / 96.1% |
| Cabbage A−3.5/D240, −8°C×24h | 100% / 77.6% | 100% / 77.7% | 100% / 77.5% |
| Cabbage A−3.5/D240, −8°C×48h | **100% / 55.3%** | **100% / 55.1%** | **75% / 42.9%** |
| Onion A−5/D120, −4°C×8h for 15 nights | 100% / 100% | 100% / 100% | 100% / 100% |
| Onion A−5/D120, −6°C×24h | 100% / 90.0% | 100% / 90.2% | 100% / 90.1% |
| Onion A−5/D120, −10°C×24h | **0% / 0%** | **50% / 26.3%** | **50% / 27.3%** |
| Tomato A0/D21.5, −3°C×8h | **0% / 0%** | **0% / 0%** | **20.9% / 10.9%** |
| Pumpkin A0/D22, −3°C×8h | **0% / 0%** | **4.5% / 2.3%** | **27.3% / 14.3%** |

**Full-HP death transition timing** under precisely constant target ambient temperature (earliest susceptible, median, latest resistant among full-HP plants; rounded game-hours):

| Plant and frost | w=0 | w=.10 | w=.20 |
|---|---|---|---|
| Cabbage D240, −8°C | 53.33h | 48.0–58.67h | 42.67–64.0h |
| Onion D120, −10°C | 24h | 21.6–26.4h | 19.2–28.8h |
| Tomato D21.5, −3°C | 7.17h | 6.45–7.88h | 5.73–8.60h |
| Pumpkin D22, −3°C | 7.33h | 6.60–8.07h | 5.87–8.80h |

These intervals are **calculated from D(1±w)/(A−T)**, not an independent freezing experiment. A constant-width distribution makes the survival curve piecewise linear as a function of accumulated dose near D; physiological freezing response curves commonly use sigmoidal/logistic temperature fits instead. [Whole-plant frost-tolerance study, J-STAGE (2000)](https://www.jstage.jst.go.jp/article/jjshs1925/69/3/69_3_332/_article/-char/en) shows the distinction between leaf-electrolyte injury and whole-plant recovery; [iForest adult/seedling frost study (2014)](https://iforest.sisef.org/contents/?id=ifor1076-007) uses a logistic whole-seedling survival response; [UC IPM frost guidance](https://ipm.ucanr.edu/home-and-landscape/freezing-and-frost/) summarizes age, acclimation and exposure conditions. **None calibrates the V3 width to 10% or 20%.**

### Interpretation and bounded next decision

- **w=0** is a reproducible, scientifically *uncalibrated* no-extra-variation baseline. Equal A/D and identical cold histories give equal fractional cold injury, but real plants already differ in HP, shelter, temperature and event timing. Do not claim w=0 guarantees simultaneous die-off in real RimWorld.
- **w=.10** is a useful **narrow-spread comparison** for hardy crops: at the selected cabbage D240 and severe −8°C×48h stress, it avoids the 25% casualty rate projected by w=.20 while retaining a small individual death-time spread. It does not rescue Tomato D21.5 from −3°C×8h, so cannot fulfill the older gameplay S16 preference for a mixed surviving/dying cohort at exactly that event.
- **w=.20** creates more spread, including some tomato survivors after strong frost, but also **earlier deaths among weaker hardy crops** (Cabbage −8°C×48h: 25% die). It should **not be copied universally from the existing draft CSV**. Whether tender annuals should retain w=.20 as a gameplay compromise is independent of the confirmed hardy crop A/D candidate decision.
- At exactly E=D and any symmetric nonzero width, around 50% survival is *automatic from the chosen distribution*, not empirical support for either width. Combined cohort **survival and harvest**, rather than mortality alone, is the meaningful player-facing test.

**Provisional review policy (NOT author acceptance):** Retain **w=0 in production/framework defaults**; use **w=.10 as the first optional comparison for hardy crops** and **w=.20 only as a targeted tender annual comparison** if a mixed survival outcome is explicitly desired. No species' w has been approved, and no candidate CSV coefficient, C# engine, Def XML or release has been changed by this review. If actual map temperature/microclimate naturally staggers death enough, adding any artificial within-species lethal-dose spread may provide insufficient gameplay value to justify using it.

## Tender-annual A/D/variation combination review (2026-10-23)

**CURRENT DECISION: comparative shortlist only; Tomato/Pumpkin A/D and w remain unapproved.** The author authorized continuing balancing, **not selecting a numerical profile**. Historical V2 severe-frost preferences S02/S23 and the partial-survival S16/S24 remain game-design prompts; since CCTO has **no special HP recovery**, S02 vs S16 and S23 vs S24 are **not independent mortality evidence**: each pair has equal 24°C·h when A=0°C. Do not call two passed cases two independent confirmations of accuracy.

### Plausible tender-annual combinations and player-visible tradeoff

Use the original MO 1.6.2.2 MaxHP 85 Tomato / 100 Pumpkin, normal Plant.YieldNow() HP reduction, integer injury, uniform deterministic per-plant dose multiplier for w=.20 and an ideal constant-temperature reading. Full-field yield includes zero harvest for plants dead before harvest. This is **model math**, not a measured LT50, game load or a proof of an actually frost-exposed mature plant.

| Crop, **all unapproved** | A (°C) / D (°C·h) / w | −0.25°C ×8h alive / yield | −1°C ×2h alive / yield | −1°C ×8h alive / yield | −3°C ×8h alive / yield |
|---|---|---|---|---|---|
| Tomato first-choice reviewer profile | **0 / 21.5 / .20** | 100% / 95.6% | 100% / 95.6% | 100% / 81.4% | **20.9% / 10.9%** |
| Tomato lower-onset counterfactual | **−0.5 / 18 / .20** | 100% / 100% | 100% / 97.5% | 100% / 89.0% | 22.2% / 11.6% |
| Pumpkin first-choice reviewer profile | **0 / 22 / .20** | 100% / 95.6% | 100% / 95.6% | 100% / 81.8% | **27.3% / 14.3%** |
| Pumpkin lower-onset counterfactual | **−0.5 / 18.5 / .20** | 100% / 100% | 100% / 97.5% | 100% / 89.3% | 29.7% / 15.7% |

The existing first-choice A=0 option explicitly interprets **frost-temperature air exposure** as accumulating damage in a living plant. The alternative A=−0.5 option avoids harm under weak −0.25°C frost and cuts the −1°C×8h yield loss nearly in half while retaining similar severe −3°C×8h mortality. That distinction is an important **player-facing mild-frost priority**, not something the old hard-frost 70%-mortality target can settle.

### Width requirement for a mixed field at the old hard-frost test

At **A=0°C** and −3°C×8h, every plant accrues 24°C·h. With D21.5 Tomato, a theoretical field can retain *any* full-HP survivor at that event **only if w > 24/21.5−1 ≈ 0.1163**. With D22 Pumpkin, the corresponding threshold is **w > 24/22−1 ≈ 0.0909**. Therefore w=.10 predicts **0%** survivors in Tomato and only **about 4.5%** in Pumpkin, while w=.20 predicts the 20.9% / 27.3% above. These are mathematical dependencies of the selected A/D versus a *gameplay-selected* event, **not measured individual-level cold-tolerance dispersion**.

Other possible w/D combinations can of course yield partial mortality; w=.20 is not uniquely implied by independent evidence. The fixed deterministic Thing-ID rank avoids extra RNG and is already implemented. The biological distributions may be nonlinear or asymmetric and depend on developmental stage, cultivar and duration, so do not claim that the uniform ±20% is a scientifically calibrated universal coefficient.

### Check against original RimWorld 1.6 world climate, not just a laboratory-shaped single night

Reused the *original licensed 1.6* world/tile climate artifact (Project [Actions #39776410000](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39776410000), downloadable original CSV artifact 12424418617, source data SHA256 `6eac0d50124fbd622d37737abd1a86223b3649b27133e3e9349a43797e37b8c5`). Specifically, TemperateForest tile 2851's **60-day hourly world temperature calendar**, *not* live map ambient/weather and *not* a 60-day TickList/crop-growth simulation. At 2500 game ticks per hour and no cold recovery, the first hours under A=0 arrive on **day 24**. Hypothetically keeping full-HP mature plants outside and not harvesting them gives:

| End of calendar day | Tomato A0/D21.5/w=.20 | Tomato A−0.5/D18/w=.20 | Pumpkin A0/D22/w=.20 | Pumpkin A−0.5/D18.5/w=.20 |
|---|---|---|---|---|
| Day 24 | 100% survive / 91.1% yield | 100% / 92.5% | 100% / 91.3% | 100% / 92.7% |
| Day 25 | **80.3% / 46.3%** | **100% / 60.5%** | **85.3% / 49.6%** | **100% / 61.5%** |
| Day 26 | 0% / 0% | 0% / 0% | 0% / 0% | 0% / 0% |

**Interpretation:** A0 starts destroying exposed field harvest rapidly around the first severe autumn frost; shifting onset to −0.5 softens marginal-frost loss without allowing unharvested summer plants to survive winter in this single sampled climate. Neither output measures actual crop season or author-approved target. Seasonal timing may vary radically with longitude/latitude, Vanilla weather, indoor crops, MO season limits and sow date. **The sparse 2000-tick current-temperature sampler may disagree with any of these continuous-temperature estimates**, so no w/A/D combination may be declared native-valid from this calendar.

### Agronomic endpoints and bounded recommendation

- [Ontario Ministry of Agriculture, tomato temperature effects](https://www.ontario.ca/page/extreme-temperature-effects-tomato-and-pepper-crops): tomato frost causes wilt/necrosis; actual freezing depends on plant condition, thaw, wind, cover and duration; frost at 0°C air does not *always* damage crops. Chilling (0–5°C) also has different nonlethal effects. These do not establish a hard universal A0 or LT100 dose.
- [Field tomato winter survival study (2021)](https://pmc.ncbi.nlm.nih.gov/articles/PMC8761588/): Japanese outdoor tomato comparison observed frost-associated dieback across days with freezing lows down to −5.3°C, but not single-night −3°C×8h mortality. Do not mislabel this as calibrated dose-rate evidence.
- [University of Maryland Extension, growing pumpkins](https://extension.umd.edu/resource/growing-pumpkins-home-garden): light frost **can kill growing vines while mature fruits remain usable**; an already harvested fruit is not a CCTO living Plant. [Iowa State Extension (2024)](https://yardandgarden.extension.iastate.edu/faq/when-should-i-harvest-my-pumpkins) likewise distinguishes first vine-killing light frost from detached mature fruit. Thus choosing weaker onset to protect pumpkin vines from −0.25°C may conflict with vine fragility even if it improves late autumn harvest in game.

**Reviewer shortlist, NOT approved by author:** Retain **Tomato A0/D21.5** and **Pumpkin A0/D22** as the *first* A/D numeric candidates for explicit author decision. If **the actual user goal is to protect crops from the mildest frost** rather than reflect vine damage, the lower-onset **A−0.5/D18** and **A−0.5/D18.5** remain defensible *alternatives* for game balance. For either first choice, **w=.20 is a separately unapproved tentative gameplay width** to avoid all-plant simultaneous deaths at the artificial strong-frost target; w=.10 has no tomato survivors in that target. Avoid pretending any tuple is empirically precise. Prefer one focused real-game frost/yield confirmation during consolidated V3 testing **after author numerical selections**, not new tests now.

**No adoption has occurred:** keep the official CSV's Tomato A0/D60 and Pumpkin A0/D68 with `FAILED_PREDECLARED_FROST_GOALS`, the two previously author-selected hardy A/D profiles, current unapproved spread values, 30 dormancy definitions, C# and production XML exactly as before.
