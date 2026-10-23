# CCTO V3 — four-crop survival **and harvest-yield** screening (2026-10-23)

**Status: INDEPENDENT COMPARATIVE REVIEW, NOT NUMERICAL ACCEPTANCE.** **Author direction approved 2026-10-23:** cabbage and onion prioritize survival with HP-driven yield loss instead of old S04/S20 70%-death goals. All crop A/D numbers, the proposed ±20% spread and production XML remain **unapproved**. See [revised V3 criteria](ColdDamageBalanceV2Targets.md#v3-s04s20-supersession--author-approved-direction-2026-10-23). This is a deliberately compact follow-up to [ColdDamageHpV3Assessment.md](ColdDamageHpV3Assessment.md) and [ColdDamageHpV3Candidates.csv](ColdDamageHpV3Candidates.csv). No changes to original candidate tuples, production XML or dormancy definitions. The last section records a **shortlist for comparison only**, not decisions or approved defaults.

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

These numbers **cannot be adopted until the corresponding player-facing tradeoff is chosen** and onset uncertainty (±0.5°C), ordinary seasonal exposure, real sampling gaps and actual plant HP/harvest timing are assessed. The onion hard-frost *mortality target* S20 remains an unverified preference and must not force D18. Values may be revised outside this list if independent evidence appears; there is no false claim that these are tight physiological bounds.

### End-of-stage status

- 48 V3 tuples and common half-width 0.20 remain **unapproved**; CSV status unchanged (4 failed, 44 otherwise unapproved).
- 30 dormancy existing values and 48 fixed-death production XML remain unchanged.
- No new runtime C#, growing-stage simulation or cold recovery added.
- No game launch now. The compiled draft's technical PASS does not establish frost/yield balance; acceptance should be consolidated **after** gameplay tradeoffs and remaining species are selected.

## Provisional direction after comparative review — 2026-10-23 (not an XML authorization)

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
