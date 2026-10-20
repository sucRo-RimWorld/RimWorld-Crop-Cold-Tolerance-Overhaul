# CCTO cumulative cold injury — preliminary numerical calibration

**2026-10-21; target = general-purpose CCTO, not AMJ-only. STATUS: CALIBRATION CANDIDATES — NOT APPROVED BALANCE / NOT ACTIVE XML.**

## Why numerical calibration precedes native testing

The current draft implementation in PR #24 uses a *linear* temperature-degree-hour injury rate. Numerical comparison with published field studies identifies a material limitation: severe freezes can injure far faster than the simple linear ratio predicts. Do **not** copy the numbers below into the current linear code or claim native evidence before revising the engine.

CCTO main `Docs/ImplementationTable.md` currently inventories **78 PlantDefs: 48 with finite fixed cold-death thresholds and 30 cold-dormancy plants**. New mode's onset/dose/recovery must be specified per species or defensible functional/ecological group; no mass migration by simple replacement of `coldDeathTemperature` is authorized. CCTO owns Core/MO numerical values; AMJ Grains, Environment, and CCTO-for-VPE own their own value mapping and XML. The old fields and existing Info Card remain the production semantics until explicit migration.

## Candidate curve to calibrate

Screen a **convex severity** rule, initially quadratic rather than linear:

```text
If ambientTemperature < damageStartTemperature:
    damage += (damageStartTemperature - ambientTemperature)^2 * elapsedHours / lethalDose
Else:
    damage = max(0, damage - recoveryPerHour * elapsedHours)
Death at damage >= 1
```

`lethalDose` units are **°C²·in-game-hour**, not the current branch's °C·hour. `recoveryPerHour` is normalized [0,1] lethal-dose fractions per in-game hour. Below-onset temperature is ambient, not automatically the outdoor world-tile temperature. Squaring requires one multiplication rather than a general exponentiation; update at existing plant checks, not a per-tick scan. This is an engineering candidate to compare against linear and real evidence, not a universal plant physiology law.

**Published severity-ratio anchor.** Corn emergence injury literature (Lindsey 2024, citing Buican 1969) reports *roughly 50% mortality* from −2°C for 48 hours, versus −6°C for 2 hours. For onset −1°C, severity ratios for −6°C vs −2°C are **5** under a linear model and **25** under a quadratic model; observed duration ratio is **24**. This is evidence for a convex response shape, **not permission to equate experimental 50% mortality with deterministic death of every simulated plant**.
- https://acsess.onlinelibrary.wiley.com/doi/10.1002/csc2.21212
- https://doi.org/10.1111/ppl.70633 (Arora 2025: cold duration and recovery can materially change irreversible injury)
- https://pubmed.ncbi.nlm.nih.gov/39862458/ (Kosová et al. 2025: wheat and barley acclimation/deacclimation)
- https://www.fao.org/4/y7223e/y7223e0a.htm (some rapid freeze injuries depend less on duration; the model is a gameplay abstraction)

## Phase A: explicit provisional comparison points

The table is **not a set of biological measurements**. Each row is a simulation target chosen for different climate/plant functions to be compared against real agronomy, loaded RimWorld climates and duration curves. Constant test temperatures are synthetic; time-to-death below excludes intervening warm recovery.

| Plant | Current death °C | Candidate onset °C | Lethal dose °C²·h | Warm recovery /h | Example constant °C | Computed fatal hours |
|---|---:|---:|---:|---:|---:|---:|
| Rice (Vanilla) | -1 | 2 | 54 | 0.03 | -1 | 6.0 |
| Corn (Vanilla) | -2 | -1 | 48 | 0.03 | -6 | 1.9 |
| Potato (Vanilla) | -2 | -1 | 32 | 0.02 | -4 | 3.6 |
| Tomato (MO) | -1 | 2 | 36 | 0.03 | -1 | 4.0 |
| Pumpkin (MO) | -1 | 2 | 36 | 0.03 | -1 | 4.0 |
| Cabbage (MO) | -6 | -4 | 256 | 0.01 | -8 | 16.0 |
| Wheat (MO) | -6 | -5 | 500 | 0.007 | -10 | 20.0 |
| Strawberry (Vanilla) | -9 | -8 | 384 | 0.01 | -12 | 24.0 |
| Pine (Vanilla) | -35 | -30 | 2400 | 0.004 | -40 | 24.0 |
| Teak (Vanilla) | 3 | 5 | 150 | 0.02 | 0 | 6.0 |
| Lemon (MO) | -4 | -2 | 128 | 0.01 | -6 | 8.0 |
| Cocoa (Vanilla) | 0 | 4 | 96 | 0.02 | 0 | 6.0 |

**Reference-case implications (not native results):**
- Corn: at −2°C, 48h; at −6°C, 1.92h. The observed study reports 50% mortality, unlike this deterministic game model.
- MO wheat: at −8°C, 55.6h continuous; at −10°C, 20h; at −15°C, 5h. Current values represent an *unacclimated/general wheat abstraction*; winter wheat varieties and snow protection cannot be generalized into this constant curve.
- Rice: at −1°C, 6h continuous; at 0°C, 13.5h. A short frost is survivable, sustained frost lethal.
- Cabbage: at −6°C, 64h; −8°C, 16h; −12°C, 4h.
- Pine: strong resistance remains meaningful even in cold-biome winters; verify coldest supported biomes before any migration.
- Recovery is provisional: existing draft engine recovers at/above onset, including temperatures below minimum growth. Reconsider whether recovery requires reaching the plant's growth threshold before accepting these values.

## Phase B: finish all CCTO-owned plant values *before* production adoption

The finite-death inventory is **48**, not merely cultivated crops. The Phase A group above covers 12 entries. The remaining **36** require species/ecology group mapping and numeric duration targets before enabling the new mode for the supported CCTO set. Include cold-sensitive crops, medicinals, forestry trees, wild-only plants, cave fungi, desert plants, and fictional Defs separately. Confirm species ecology where evidence exists; use transparent gameplay/range constraints for fictional Defs. Wild-only plants must remain viable in native tundra, boreal forest, desert and wetland biomes, rather than experiencing chronic cold-related ecosystem die-off.

The **30 currently dormancy-only** Defs remain a separate calibration group. Do not assign arbitrary finite injury to them: decide whether surviving shoots, underground roots, dormant woody organs, or evergreen foliage is being represented, and sample actual native biome conditions. Keep the existing `休眠温度` display unchanged. No dormant-plant death value has been approved.

**Dependency order for meaningful final validation:** (1) numerical survival targets and ecological constraints; (2) correct/choose injury curve and recovery semantics; (3) map candidate numbers to owner-owned XML; (4) then minimal representative native scenarios, baseline cold-gradient comparison, save/load and winter TPS. Prior native fixed-threshold PASS does not prove new cold damage. New native test automation must not run on PR/push without author approval.
