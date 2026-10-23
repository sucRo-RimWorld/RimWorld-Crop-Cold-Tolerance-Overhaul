# CCTO Coordination

### CCTO-V3-INDIVIDUAL-VARIATION-REVIEW-20261023 — 0 / ±10 / ±20% compared, width still unapproved

**Owner:** [CCTO Draft PR #31](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/31), formal `Docs/Design.md` (per-plant width gate) and `Docs/ColdDamageHpV3YieldReview.md` (0/10/20% constant-temperature survival and cohort-harvest matrix). Author-selected A/D candidates remain **Cabbage A−3.5°C/D240°C·h, Onion A−5°C/D120°C·h only**. Existing source maps stable Thing ID to a uniform **dose-only** width; no additional C# code is needed for optional per-species widths. The **default w=0** remains the baseline; the CSV's w=.20 for all 48 HP trials (including two A/D-approved rows) remains **UNAPPROVED**, not authorized for production copying.

**Independent numeric discrimination:** At constant −8°C×48h, Cabbage D240 survives 100% (w=0), 100% (w=.10), 75% (w=.20) of the hypothetical field; at constant −10°C×24h, Onion D120 survives 0% (w=0), 50% (w=.10), 50% (w=.20). Under unapproved Tomato A0/D21.5 / Pumpkin A0/D22 at −3°C×8h, w=.10 gives 0% / ~4.5% survival, versus w=.20 ~20.9% / ~27.3%. **These are idealized uniform-dose population predictions, NOT original-game observations, validated agronomic population spread, or a selected width.** Research supports plant freezing tolerance variability, not a universal ±10/±20% fatal-degree-hour coefficient.

**Handoff choice:** Preserve per-plant width optionality and w=0 framework default. **Prefer comparing w=.10 for hardy groups and w=.20 only for tender annuals** if actual game conditions require artificial partial survival; no global w=.20 rollout, numerical approval or XML changes. Other 44 A/D remain unapproved, tomato/pumpkin 2 failed under current target, 30 dormancy kept. Native normal-tick/ambient HP/harvest and save/load acceptance still pending consolidated late-stage test. Do not treat a source-build PASS or these calculations as game balance PASS.


### CCTO-V3-HARDY-AD-CANDIDATES-20261023 — author selected numeric candidate A/D; variation and XML excluded

**Source owner:** [CCTO Draft PR #31](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/31); authoritative `Docs/Design.md`, `Docs/ColdDamageBalanceV2Targets.md`, `Docs/ColdDamageHpV3Candidates.csv`, `Docs/ColdDamageHpV3Assessment.md`, `Docs/ColdDamageHpV3YieldReview.md`.

**Latest author decision (2026-10-23, supersedes earlier provisional D160/old onion onset notes below):** Approve **numerical V3 A/D candidates only**: MO Cabbage `DankPyon_Plant_Cabbages`: **injury onset −3.5°C, lethal full-HP dose 240°C·h**; MO Onion `DankPyon_Plant_Onions`: **injury onset −5°C, lethal full-HP dose 120°C·h**. Cabbage keeps D240; Onion A changed from −3.5°C to −5°C. Preserve accepted hardy-crop **survival plus reduced harvest** direction; historical high immediate death S04/S20 goals stay superseded.

**Exclusions:** The CSV's `lethal_dose_variation=0.20` is still an **unapproved trial assumption** for both species, NOT part of the numeric authorization; framework default is w=0. This author decision **does not authorize** production XML conversion, releasing HP V3, native gameplay PASS, or accepting any other plant A/D. Latest draft CSV statuses: **2 `APPROVED_A_D_CANDIDATE_VARIATION_PENDING`, 44 `UNAPPROVED_HP_TRIAL`, 2 `FAILED_PREDECLARED_FROST_GOALS` (tomato/pumpkin), 30 `KEEP_EXISTING_DORMANCY`**. Production still has 48 fixed-death + 30 dormancy defs; actual original-game 1.6 tick/HP/harvest and save-load acceptance remains pending. Do not mistake the approved *candidate A/D fields* for a ready-to-release enabled profile. Prior build-only [Actions #39875995396](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39875995396) covers C# source, not this numeric balance or native behavior.


### CCTO-V3-HARDY-NUMERICAL-SHORTLIST-20261023 — cabbage D240 preferred over D160; onion onset unresolved

**Owner:** [CCTO Draft PR #31](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/31), formal `Docs/ColdDamageHpV3YieldReview.md` (latest numerical review `de7409053fcf4095d5b8d20215d5c67ae5a11fe1`, current `Docs/ColdDamageHpV3Assessment.md` cross-link). **Numerical values NOT approved.** Author-approved design direction remains hardy MO Cabbage/Onion survival and HP-based reduced harvest (not high immediate S04/S20 death). Source-level comparison using MO Cabbage 85 MaxHP and Onion 100 MaxHP + Vanilla `Plant.YieldNow()` rule favors keeping *unapproved* Cabbage **A−3.5/D240**, not earlier weaker D160 (at continuous −8°C×48h: D160 0% survivors; D240 100% for w=0, 75% if optional w=.20). Onion `A−3.5/D120` currently costs 50% HP after fifteen −4°C×8h nights due to **no cold HP regeneration**; propose comparing alternative **A−5/D120** as an onset trial, **not** as accepted XML. Optional dose spread w=.20 has no independently measured physiological width; w=.10 vs .20 changes partial deaths, w=0 is code default. Keep 48 HP candidate tuples/CSV unchanged (46 unapproved, 2 failed hard-frost trials, 30 dormant), production fixed-death XML unchanged. No new gameplay run; exact-source prior 1.6 compile PASS is not numerical approval. The earlier D160 proposal in lower notes is superseded by this bounded owner comparison only.


### CCTO-V3-HARDY-CROP-DIRECTION-20261023 — author-approved survival/yield priority; numeric values UNAPPROVED

**Owner:** [CCTO Draft PR #31](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/31), canonical `Docs/Design.md`, `Docs/ColdDamageBalanceV2Targets.md` V3 supersession, `Docs/ColdDamageHpV3Assessment.md`, `Docs/ColdDamageHpV3YieldReview.md` and unchanged-value `Docs/ColdDamageHpV3Candidates.csv`.

**Author decision (2026-10-23):** MO **Cabbage** and MO **Onion** should **survive** representative hard frost with normal Plant HP injury and **lower harvest yield**, rather than satisfy legacy high-mortality S04/S20 (old S≤0.30). The historical 2026-10-22 predeclared S04/S20 rows are retained **for audit only**, explicitly superseded as current V3 balance gates. Tomato/Pumpkin severe-frost lethal roles are unchanged; 30 dormancy definitions remain intact. No cold-only HP healing or additional yield system.

**Approval boundary:** The direction is approved, **not** Cabbage A−3.5/D160, Onion A−3.5/D120, current candidate A/D, individual variation w=0.20, or XML activation. Original CSV numbers remain unchanged: **46 UNAPPROVED_HP_TRIAL, 2 FAILED_PREDECLARED_FROST_GOALS (tomato/pumpkin), 30 KEEP_EXISTING_DORMANCY**. Cabbage/onion statuses changed from old-frost-goal failure to **UNAPPROVED**, not PASS. Presently published 48 fixed-death XML untouched. Draft source has earlier actual .NET 1.6 compile PASS [Project Actions #39875995396](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39875995396); actual HP/yield gameplay and real .rws save/load remain unaccepted. Do not launch repeated native tests until consolidated acceptance near numerical finalization.

### CCTO-V3-YIELD-BALANCE-20261023 — mortality is insufficient; four-crop shortlist only

**Numerical/implementation/XML handoff.** Owner [Draft PR #31](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/31), formal [Docs/ColdDamageHpV3YieldReview.md](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/blob/feat/ccto-v2-direct-plant-hp-20261022/Docs/ColdDamageHpV3YieldReview.md) and `Docs/ColdDamageHpV3Assessment.md`. Source reviewed the normal Plant `YieldNow()` HP factor (alive with fraction p yields about `0.5+0.5*p` compared to its otherwise harvest-ready condition). Under **unapproved** per-plant dose variation ±20% and ideal continuous exposure, old severe-frost fitted D21.5 tomato, D22 pumpkin, D96 cabbage leave only ~11%, ~14%, ~10% expected total field yield when mortality is included. Onion D18 leaves only ~66% field yield after 3 mild -4°C×8h nights **despite 100% survival**; set it aside from immediate numeric comparison. For bounded owner review, compare `tomato D21.5/D40`, `pumpkin D22/D40`, `cabbage D96/D160`, `onion D60/D120` with existing unapproved onsets unchanged. **These are sensitivity alternatives, not scientifically validated or author-approved coefficients.** Real-world onion references disagree by growing context, so old S20 (70% kill at -6°C×8h) is an unverified gameplay preference, not measured mortality. **All 48 HP candidate values, common ±20% variation, and 30 dormant records retain prior authorization states.** Production 48 fixed-death XML remain untouched. No native test, V3 XML conversion, or balance PASS; downstream XML owner must not promote any suggested D without actual author numerical decision.


### CCTO-V3-FATAL-NOTIFICATION-20261023 — draft source patched, normal tick acceptance pending

**Owner:** CCTO Code/framework [Draft PR #31](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/31), latest reviewed source `b1adc4f30c9e61baff75164d26f77da6c6685527`. Fatal HP cold damage now captures crop identity/location before applying damage, then calls the existing shared cold-death notification **only if that damage destroys the plant**. Nonfatal HP damage does not notify; original fixed-cold-death notification remains. The existing Pickle cold/warm scenario now advances real map game TickLists (`TickManager.DoSingleTick`) instead of manipulating injury timestamps, but **the revised E2E has NOT run**. No production XML/numerical value changed, no main gameplay integration. Previous C# compile PASS only covers older `6c0643275f05ff96982f1215a95bcd16cc6109ab`; fresh compile-only [Project Actions #39819077281](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39819077281) was queued at handoff, **not evidence of PASS**. Do not launch repeated native game jobs; combine normal-tick, fatal message and Scribe verification in the later acceptance when runtime is available.


### CCTO-V3-BALANCE-CHECK-20261023 — four candidate frost profiles rejected

**Owner:** CCTO balance/XML workstream, formal source on [HP Draft PR #31](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/31) `Docs/ColdDamageHpV3Assessment.md` and `Docs/ColdDamageHpV3Candidates.csv` (draft evidence commit `684784f22a31b9d6aa9191daa3dd82e92ca3c7e4`). Original native 1.6 world climate **4320 hourly samples** recovered with verified source CSV digest. Under V3 trial HP formula, **Tomato, Cabbage, MO Onion and MO Pumpkin D values conflict with pre-existing serious-frost gameplay mortality targets** (S02/S04/S20/S23/S24, also tomato S16/pumpkin S24). Their four existing CSV rows retain numeric candidates but are explicitly marked `FAILED_PREDECLARED_FROST_GOALS`; candidate smaller-dose alternatives in assessment **not adopted/approved**. Rice and wheat acclimation-stage contradictions remain noted, not a reason to build a physiology subsystem. Pine onset -40C incurs zero loss in two sampled normal Boreal world-climate calendars; actual weather/ColdSnap remains unverified. **No V3 numerical XML approved, no gameplay/native acceptance, no published behavior changed**. Source C# 1.6 build PASS from previous coordination note is a separate verified engineering result. Do not pass draft values to production based on simple CSV completeness.


### CCTO-V3-HP-20261022 — HP-only cold injury, draft implementation

**Owner:** CCTO Code/framework [Draft PR #31](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/31) targeting [Draft PR #24](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/24). **Current state: source and tests revised on draft branch only; original RimWorld 1.6 C# COMPILE PASS, game/runtime acceptance NOT RUN.** Code head `6c0643275f05ff96982f1215a95bcd16cc6109ab`. Author chose ordinary visible Plant HP as low-temperature damage and no CCTO-only HP healing. Code now uses only sampling-tick and fractional-HP remainder saves; unreleased V2 normalized-injury conversion, misleading HP-as-cold-specific damage alias, and obsolete recovery config have been removed. Updated Pickle, RimTest, native actual .rws tests and canonical `Docs/Framework.md` document the HP behavior. An isolated compile-only Project runner using original licensed RimWorld Linux 1.6 references built this exact source on [Actions #39806727803](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39806727803): **0 compiler errors, 0 compiler warnings**, DLL SHA256 `f4f544fc0b7e819946cadda07a8bd09ca5ef46ce87976c761ff45a773f863f2e`. No game was launched and no DLL was deployed; the temporary Project compiler PR #989 was closed unmerged after extracting evidence.

**Balance/XML owner status:** `Docs/ColdDamageHpV3Candidates.csv` contains all 48 HP-damage two-parameter trial profiles; `Docs/ColdDamageHpV3Assessment.md` includes climate-corrected pine risk review. **All 48 V3 tuples and shared variation remain unapproved**. Do not promote superseded V1/V2 recovered-injury survival percentages. 30 dormancy entries remain unchanged. Current production XML still uses **48 fixed-temperature death and 30 dormancy** profiles; no published HP activation. Before production conversion, assess normal seasonal and actual cold-snap exposures, then perform a consolidated build and game acceptance. Do not merge the parent draft or publish based on static checks alone.

### CCTO-NATIVE-FOREST-CLIMATE-20261022 — original RimWorld 1.6 seasonal tile evidence

**Owner:** CCTO Balance/XML Draft [PR #24](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/24), Project native runtime. **State: REAL ORIGINAL-GAME WORLD/TILE CLIMATE DATA CAPTURED, 7 V2 NUMERIC CANDIDATES UNAPPROVED; 48 legacy fixed-death / 30 dormancy live XML unchanged.**

[Project manual native Actions #39776410000](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39776410000) used RimWorld **1.6.4871 rev598** at exact owner Quickstart source `23f3d9ac07a336c36fba3e32382612bec52e6d8f`, world seed `CCTO-NaturalColdClimate-20261022`. It generated an **unforced BorealForest map with 483 naturally spawned Vanilla pines** and sampled original World.tileTemperatures in two real natural BorealForest tiles plus one actual marginal TemperateForest tile, 60×24 calendar hours each (**4320** values). Actual game exited 0, Unity ERROR0, Quickstart3/3. Source hourly CSV SHA256 `6eac0d50124fbd622d37737abd1a86223b3649b27133e3e9349a43797e37b8c5`, artifact `12424418617`; durable derived 60-day Lo/Hi and full value/state distinctions on CCTO owner Draft `Docs/Research/CCTOVanilla16ActualWorldClimateDailyLoHi.csv` and `Docs/ColdDamageBalanceV2Assessment.md` / `Docs/Design.md`.

Native observed season-only temperature ranges: cold pine-eligible BorealForest tile 44000 annual −5.536°C min −37.835°C (92h below V2 onset −31°C); loaded naturally pine-bearing BorealForest tile 40777 annual −1.370°C min −32.647°C (6h below onset); cool TemperateForest tile 2851 annual +5.672°C min −23.709°C. Under the **offline CCTO candidate formula**, Pine/fir onset −31°C, D 3600°C·h survives 100% on both sampled Boreal climates; four trial annuals survive first 20 calendar days on sample crop tile but die if never harvested through full winter. These are **candidate simulations, not live installed XML or crop mortality observations**.

**Limit:** real RimWorld world generation and cached world climate *at hourly calendar coordinates* only; **NOT** 60 normally ticking days of weather events/ColdSnap, not whole-plant thermal sensors, not true random world/biome range, not full Medieval Overhaul or AMJE/WorldMap, and not actual crop maturity. No coefficient selected/approved by this data. Old V1 rejected-for-XML and seven V2 trial values still unapproved. No further native or Workshop run should be inferred; the source-run evidence is for seasonal world climate only. Owner formal numbers remain in Draft, not this main coordination handoff.


### CCTO-COLD-BALANCE-V2-20261022 — 7 proposals checked; 0 XML-ready

**Owner:** CCTO Balance/XML on Draft PR #24. **Status:** 7 candidate numerical Defs (4 annual gameplay trials, 1 wheat acclimation-blocked, 2 evergreen ecology-unapproved), 41 death profiles remain blank, original 30 dormancy unchanged; production 48 legacy fixed death and 30 dormancy XML unchanged.

Target-first V2 was extended with source-distinguished [S19–S31 frost, whole-plant/crown, and long-winter outcomes](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/blob/feat/opt-in-cold-damage-20261021/Docs/ColdDamageBalanceV2Targets.md), committed before the new numeric rows. V2 [candidate table](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/blob/feat/opt-in-cold-damage-20261021/Docs/ColdDamageBalanceV2Candidates.csv) now includes MO onion, MO pumpkin, and revised Pine/Great Fir winter-survival assumptions; [assessment](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/blob/feat/opt-in-cold-damage-20261021/Docs/ColdDamageBalanceV2Assessment.md) reports the actual stress-case predictions. A prior malformed comma-separated CSV scenario field affecting three rows was corrected; 78 rows and 13 columns now verified.

CCTO Draft C# thaw-only recovery has been implemented but is **not native-tested after change**. W13 wheat conditioning, R02 rice acclimation, corn intensity, strawberry crown-versus-air, potato foliage-versus-whole-plant and evergreen actual winter ecology remain **open numerical/model limitations**. 7 candidates are **unapproved**; original V1 rejected-for-XML gate remains in force. Numerical coefficients and source limits belong in Draft design docs, not in this coordination summary. No production XML, 1.7, native run or Workshop change was performed.

### CCTO-COLD-BALANCE-V1-20261022 — V1 false balance approval RETRACTED

**Owner:** CCTO Balance/XML and Code/framework. **Status: V1 REJECTED FOR XML; 48 candidate numerical tuples NOT APPROVED; 30 legacy dormancy profiles unchanged. NO GAMEPLAY OR PRODUCTION XML MIGRATION.**

**Latest correction supersedes previous '48 profiles DECIDED/LOCKED' handoff.** The assistant generated seven arbitrarily timed groups plus five exceptions by anchoring the previous instant-kill thresholds and back-solving lethal degree-hours. The pre-acceptance checks rechecked those exact formulas and 78 Def IDs—**schema and tautological arithmetic, not independent gameplay or botanical verification**. Material contradictions emerged only *after* reporting the table locked: W13 naive/acclimated wheat, R02 rice survival after conditioning, extremely steep maize near-onset response, repeated-night wheat loss, and pine recovery at −30°C. The instruction to decide the numbers first did not itself approve these numbers.

**Canonical corrective owner sources on [Draft PR #24](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/24):** `Docs/ColdDamageBalanceFailureAudit.md` (causal analysis, real-world consequence cases, independent acceptance requirements), `Docs/ColdDamageBalanceReview.json` (explicit `rejected_for_xml` and seven open blockers), archived failed `Docs/ColdDamageBalanceV1.csv` and `Docs/ColdDamageBalanceV1.md`. These numerical rows are for forensic reproducibility only. `Scripts/Review-ColdDamageBalance.py` separately reports arithmetic consistency and independent scenarios; `Scripts/Validate-BalanceXml.ps1` blocks copying cumulative fields into production XML while review is rejected/open.

**Next workstream:** reevaluate the recovery mechanism and independently define representative species/weather/biome acceptance scenarios **before** selecting replacement coefficients. Use measured source endpoints with matching cultivar/physiological state, clearly separate unmeasured gameplay assumptions, and record the actual review decision. Avoid uncritical group-wide coefficient propagation and avoid declaring PASS from schema checks. Existing C# opt-in 1.6 engine/Scribe tests retain their narrow technical PASS, but do **not** validate any species numerical balance; no extra RimWorld native testing for this audit. **main and Steam remain on the original 48 fixed-death / 30 dormancy settings.** V1 Draft #24 is unmerged, and release/production XML migration stays blocked.


### CCTO-WORKSHOP-BILINGUAL-HR-20261008 — single English-field description and language rule

**Owner:** CCTO publication/docs  
**Status:** GITHUB SOURCE PREPARED; STEAM UPDATE NOT PERFORMED; CI RESULT NOT ASSERTED

The public title now reads `Crop Cold Tolerance Overhaul（作物耐寒性オーバーホール）`. The paste-ready `Docs/SteamWorkshopDescription.txt` is English text followed by exactly one `[hr][/hr]` divider and Japanese text, with no license/AI/donation notices. `Docs/SteamWorkshopDescription-ja.txt` remains the Japanese source. A static validation script and CI gate prevent omissions and verify combined byte budget and title. No change to packageId, PublishedFileId, framework/cold behavior or source assets. The author must apply changes to live Steam item `3812412548` separately; prior separate-language publication records remain historical.

2026-10-14 public-copy refresh: README, bilingual Workshop source, 2game source and publication guidance now link the separate [CCTO for VPE Steam item](https://steamcommunity.com/sharedfiles/filedetails/?id=3819486090), distinguishing 20 source-implemented VPE 1.6 core plants (native gameplay unverified, Steam item private) from four unsupported optional VPE child packs. **CCTO live Steam and 2game publication of this copy is still pending**. This is a documentation change only; CCTO gameplay/support sets are unchanged.


This file is the shared coordination surface between separate ChatGPT chats/workstreams working on CCTO.

Use this instead of asking the user to manually relay messages between chats.

## Working rule

At the start of CCTO work:

1. Read this file.
2. Check whether there are open items owned by the current workstream.
3. Perform the relevant work without asking the user to relay the request again.
4. Update this file with the result, status, and commit/reference when the item is resolved or materially changed.

For durable specifications and decided values, update the appropriate design/framework document as well. This file is for coordination and handoff, not the final source of truth for design.

## Workstreams

- **Balance/XML**

  - crop-specific `minGrowthTemperature`
  - fixed `coldDeathTemperature`
  - Vanilla / Medieval Overhaul balance XML
  - CCTO-owned supported-set values and compatibility XML
  - AMJC/AMJE-owned plant data and framework-consumer XML are maintained in their owning repositories

- **Code/framework**
  - C# framework and Harmony patches
  - dormancy behavior
  - Info Card
  - validation
  - automated tests
  - build/test tooling

## Status vocabulary

- **OPEN** — needs work
- **IN PROGRESS** — currently being investigated/implemented
- **BLOCKED** — waiting on a specific prerequisite
- **DONE** — completed and reflected in the proper source-of-truth document/code

## Current coordination items

### CCTO-COLD-DAMAGE-SAVE-20261022 — Draft native Scribe persistence accepted

**Owner:** Code/framework CCTO [Draft PR #24](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/24). **Status:** original **RimWorld 1.6** cumulative cold-injury **16/16 Pickle** earlier source [Actions #39597538963](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39597538963), and separate actual **two-process .rws save/load PASS** for source `dbc555620c65e7b66666988eb29c51d5f8a31c8a` [Actions #39709845297](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39709845297), original game exit0/Unity ERROR0 each process. Source saves `Plant_Potato2204` with injury **.20**/`lastTick=30`, loads the actual game Scribe state at **.20/30** and correct Plant cross-reference, and continues cold injury to **.39304** after ordinary warm recovery and a deliberate test-only elapsed interval.

**Fixed actual regression:** MapComponent PostLoadInit previously rejected valid resolved Plant references where `Spawned=false` temporarily *before the map finished spawning*. It discarded otherwise valid injury records. Correction `4061fc57331fc0153558abd550c0675e687a1135` removes this premature condition while existing live-map `Prune()` still cleans stale/unspawned records. Owner test `dbc555620c65e7b66666988eb29c51d5f8a31c8a` snapshots Scribe before naturally advancing game ticks so subsequent warm recovery does not become a false save mismatch. Canonical technical contract: **Draft** `Docs/Framework.md`; runtime proof: **Draft** `Docs/DevelopmentTools.md`. Cross-owner runner/coordination is in Project main `Docs/Coordination.md` status `PROJECT-CCTO-COLD-INJURY-NATIVE-20261021`.

**No release activation:** all existing 48 fixed death/30 dormancy production profiles and XML unchanged; **1.7 revised C#**, real MO binaries, naturally scheduled frost observations, actual old player saves and species physiological calibration **remain unverified**. No rerun of unchanged 16-case suite, 1.7 test, merge of Draft PR, Steam/2game publish or crop profile migration is approved by this evidence alone.


### CCTO-VPE-ADDON-OWNERSHIP-20261013 — VPE family expansion routed

**Owner:** [CCTO-for-VPE](https://github.com/sucRo-RimWorld/CCTO-for-VPE) owns all VPE-family plant-provider cold-tolerance data, [issue #8](https://github.com/sucRo-RimWorld/CCTO-for-VPE/issues/8).
**Status:** SINGLE ADDON OWNER CONFIRMED; OPTIONAL PACK IMPLEMENTATION OPEN; CCTO BASE UNCHANGED.

More Plants, Mushrooms, Succulents and Flowers are inventoried as candidates only. Future VPE family pack-specific XML and numeric values are assigned to the existing CCTO for VPE Mod (conditionally applied only when each verified pack exists), not to base CCTO or a new separately published Mod per pack. Base Vanilla/MO values and MO integration remain unchanged; source parity now covers nine explicit counterpart/analogue pairs in addon CI, including Cabbage and Orange→Lemon. No external game test or Steam release has been performed for the new packs.


### CCTO-VPE-PARITY-20261013 — nine counterpart values guarded in addon CI

**Owner:** CCTO owns Vanilla/MO original XML; [CCTO-for-VPE](https://github.com/sucRo-RimWorld/CCTO-for-VPE) owns its VPE-only values/test.
**Status:** SOURCE PARITY FOR 9/9 CHECKED ON ADDON PR AND MAIN; VPE NATIVE ACCEPTANCE OPEN.

For original seven counterpart crops (grass, tall grass, agave, onion, pumpkin, tomato, apple), the addon matches CCTO main XML across growth-minimum, cold-death and dormancy; this now includes Cabbage matching MO Cabbage and Orange matching MO Lemon as an explicitly labeled citrus gameplay analogue. Addon PR #6 (main `26ca4bb1ed0ee4007a582ecd0161f8bbc1bdd7ab`) introduced a CI checkout of CCTO main XML to catch divergence. When changing any counterpart value in this CCTO repository, consult the addon owner and run its manual `workflow_dispatch` parity check (CCTO-only pushes do not run the addon CI). No CCTO XML/source behavior has changed. The addon's runtime/save behavior is unverified by author decision (not a pre-publication test gate), while Steam publication remains author-controlled and user bug reporting remains open on [CCTO-for-VPE issue #4](https://github.com/sucRo-RimWorld/CCTO-for-VPE/issues/4).


### POLICY-WORKSHOP-PAYLOAD-001 — Subscriber-only distribution (2026-10-07 JST)

**Requested by:** author
**Owner:** AMJ shared release / packaging
**Status:** DONE — repository policy/exclusions; actual Steam update remains separate

Core, Environment and CCTO now route subscriber-only Workshop packaging through
AGENTS and Core Docs/WorkshopPackaging.md. Root .rimignore excludes Art, Docs,
README, source, scripts/build tools, tests/fixtures/reports, VCS/editor metadata,
local overrides, archives and debug leftovers. Runtime assets, About identity,
loadFolders where used and required license/attribution remain. Development
originals stay in Git. YADA upstream Scanner.cs confirms inherited basename
rules; ineffective Patches/\_LocalTest.xml is corrected to \_LocalTest.xml.
Do not replace project filters with YADA's generic starter template.

Validation PASS: three tracked-file inventories; nested fixture/path-syntax,
accidental-runtime-exclusion and actual-payload leakage regressions; Core
archive/YADA equality and publisher adapter drift checks; corrected whole-Art
source-exclusion regression; Environment builder fixture retains production DLL
and root-only loader, and excludes README/Docs/Art/tests. Python/XML/workflow
syntax checks PASS. These prove packaging/static behavior, not new real-game
runtime or Steam publication success. Workshop filter CI is added with main-only
push and canceled superseded runs. Core's standard preparation gates source and
staged output; Environment's candidate builder gates the actual subscriber files.

**Next action:** apply current filters to the actual upload root before the next\nauthor-manual Workshop update; separately audit the downloaded package.\n

### ADD-CHANGENOTE-20261008 — Workshop release metadata

**Owner:** RimWorld-Crop-Cold-Tolerance-Overhaul release/packaging
**Status:** SOURCE IMPLEMENTED — CI verification required; no Steam publication claimed

Following Project `Docs/WorkshopChangenotes.md`, added `About/Manifest.xml`, `About/Changelog.txt`, and matching `About.xml` `modVersion=0.1.3`. A deterministic validator now runs in the existing Workshop payload CI. `Tests/validate_workshop_payload.py` requires both files in the subscriber archive/stage in addition to its existing YADA rules. This adds **author-side** Add Changenote support only; package ID, gameplay implementation, current live-site content, and prior test/release status are unchanged. `0.1.3` is the source metadata version, not a claim of a newly performed upload.

### RULE-AUDIT-20261008 — operating-rule consolidation

**Owner:** Project common rules; this repository retains its local specification and gates.
**Status:** SOURCE RESTRUCTURED; validation/publication evidence is recorded in Project `Docs/RuleAudit.md` and actual commit/CI results, not inferred here.

AGENTS now routes through Project `Docs/SharedRules.md` stop conditions and task procedures. New development requires VE and non-VE source/evidence comparison plus a justified implementation decision. Static/runtime/specification/distribution/publication remain separate states. Historical records below/above retain their original scope; this entry does not reopen paused work, change gameplay/dependencies/art/versions, or supersede owner runtime/release blockers. Main-only Coordination means one authoritative integrated log, not deleting branch snapshots. No Steam/2game update is claimed.
### RIMWORLD-17-20261013 — metadata and standard runtime entry

**Owner:** CCTO framework/testing
**Status:** LOCAL STANDARD ENTRY VERIFIED; PR INTEGRATION / AUTHOR STEAM UPDATE PENDING

SupportedVersions and public version labels include 1.7 unstable. The shared
production DLL remains unchanged. `run-tests.bat` passed its installed MO XML
check and all 14 existing framework, cold-runtime and loaded-balance scenarios
on 1.7.4976 rev1043, with native ERROR 0. Evidence:
`TestResults/Standard17-Runner.log` and
`TestResults/RimWorld17-20261013-080822-856/Reports/summary.json`.

`Docs/RimWorld17Tests.md` owns the procedure and coverage limits. The new
runtime helper keeps real rendering on a hidden desktop, captures the native
log with PID verification, rejects collisions/errors/timeouts, and preserves
product/config hashes. Its corrected lightweight MO fixture keeps temperature
values unchanged. Actual MO gameplay/old saves are not implied by those
fixture cases; independent Environment integration evidence is separate.
The old TEST-POLICY-003 migration handoff is superseded by this implementation.
CI/main and Steam states must be verified separately. Steam remains manual.
