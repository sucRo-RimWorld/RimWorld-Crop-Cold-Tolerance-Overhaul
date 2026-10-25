# CCTO Coordination

## CCTO 0.1.4 fixed-threshold micro-adjustment — 2026-10-26

**Owner:** Core and MO `Patches/*_ColdTolerance.xml`, [Design](Design.md#014-fixed-death-threshold-decision--boreal-pine-and-great-fir-2026-10-26), [ReleaseChecklist](ReleaseChecklist.md#014-fixed-threshold-adjustment--prepared-not-published), `About/Changelog.txt` and About/Manifest metadata. **Author version decision:** small fixed-threshold update is **0.1.4 Beta**, following 0.1.3. Selected Pine `Plant_TreePine` and optional MO `DankPyon_GreatFir` `coldDeathTemperature` **−35 → −40°C**, to reduce the risk of implausible loss of winter-hardening conifers on cold BorealForest tiles. Original 1.6 hourly tile minimum −37.835°C (not actual loaded-map death), and literature indicates midwinter conifer cold-hardiness; −40 is a game proxy, not experimental whole-tree LT100.
- Align the canonical numeric XML, Design, ImplementationTable, README, About/Manifest 0.1.4, and **bilingual reason** in Changelog/ReleaseNotes. Keep all 46 other fixed thresholds, 30 dormancy cases, C#, growth minima, crop economics and HP Draft #60 unchanged. Prior 0.2 plan remains deferred.
- Release/source metadata is **not** Steam proof. Final 0.1.4 game and subscribed-package verification plus explicit publisher approval remain separate. Previous 0.1.3 original game tests do not establish loaded 0.1.4 numerical behavior.


## CCTO fixed-death-only numeric review replaces HP cutover — 2026-10-26

**Owner:** [Design](Design.md#current-development-decision--fixed-cold-death-temperature-values-only-2026-10-26), [ReleaseChecklist](ReleaseChecklist.md#current-fixed-threshold-tuning-only--2026-10-26), Core/MO `Patches/*_ColdTolerance.xml` own actual fixed values. **Current instruction:** preserve 0.1.3 fixed cold-death mechanism, 48 fixed-death + 30 dormancy baseline; restrict next tuning to independently supported `coldDeathTemperature` *numbers* only. Do not change C#, HP model, growth minima/optimum, dormancy assignments, agriculture economics or plant identities.
- No fixed-temperature numeric edit is yet selected solely by this decision; previous thresholds remain until specific evidence-backed replacements pass independent physiological and typical/extreme/ecological review. Do not copy V3 injury onset A or cumulative fatal dose D as a fixed mortality temperature.
- Earlier next-public-version **0.2** plan depended on the HP switch, now **on hold**. Public metadata stays 0.1.3; version for any eventual fixed-only update is not yet chosen. HP Draft [PR #60](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/60) retained for possible later work, **do not merge/publish under current scope**. Historical HP evidence and decisions remain accessible, not live current requirements.


## CCTO next public version 0.2 — 2026-10-26

**Owner:** [Design](Design.md#next-public-release-identity--02-planned-2026-10-26) and [ReleaseChecklist](ReleaseChecklist.md#next-public-release-02-not-yet-published). **Status:** author chooses **0.2** as the next public release after 0.1.3 Beta for the cumulative visible-HP cold-injury overhaul. Existing "V2"/"V3" names are internal design iterations; Draft [PR #60](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/60) prepares 0.2, not a public V3.
- No intervening public V2 release is implied. Current `About.xml`/`Manifest.xml`, README release header, live Workshop and DLL remain 0.1.3-series until a separately accepted 0.2 release.
- At release preparation, update metadata, README (required for version changes), release notes/changelog, English/Japanese public copy and distribution payload together; do not claim Steam publication or game acceptance from this decision alone. Runtime, numeric and real-climate gates remain unchanged.


## CCTO realism-first scope clarification — 2026-10-26

**Owner:** [Design](Design.md#governing-objective-and-verification-boundaries--clarified-2026-10-26) is the canonical policy; [CropGameplayRoles-ja](CropGameplayRoles-ja.md) is optional player-facing context. **Status:** interpretation/design updated, V3 Draft #60 not merged, no new runtime or numerical values, publication unchanged.

- Original V1 realism-first purpose governs V3. Only plant-specific growth temperatures, cold-injury/exposure, dormancy and normal native-ecosystem survival determine CCTO coefficients. Vanilla+CCTO and optional MO+CCTO are supported; Agriculture economics are not a CCTO dependency, numeric target or release gate.
- Earlier seven crop-economy roles are not required V3 acceptance. Preserve the factual scope of prior MO+Agriculture telemetry but do not use external growDays, recipes or yield competitiveness to tune CCTO. Independent realism/representative cold-climate review and technical compatibility gates remain required.
- MO 1.6 Cabbage malformed optimum tag is confirmed, yet corrected 0–25°C full-rate growth remains biologically unapproved. 15–20°C is a candidate only. V3 finalized 46 HP/32 dormancy w=0 unchanged; no C#/XML, native test, distributable or Steam change follows from this documentation decision.


## MO 1.6 cabbage growth optimum typo — Draft correction and original 1.6 evidence, 2026-10-25

**Owner:** [CCTO V3 Draft PR #60](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/60), canonical before/after values in Draft `Docs/CropGameplayRoles-ja.md`. **Status:** real native 1.6 source-corrected growth probe PASS; Draft-only compatibility patch, **0–25°C all-optimal-rate biologically inconsistent and NOT RELEASE ACCEPTED**. No production main merge, distributable update or Steam publication.

- Actual MO 1.6.2.2 cultivated farm XML contains one duplicated `minOptimalGrowthTemperature` for Cabbage: **0, 25** and omits `maxOptimalGrowthTemperature`. MO 1.4/1.5 do not have this malformed set. With CCTO `minGrowthTemperature=0`, the loaded Cabbage runs at just **28% of 25°C growth at 7°C** and **12% at 3°C**, matching 7/25 and 3/25. This is an upstream XML error, not evidence CCTO A/D is wrong.
- Previously manually accepted [original game run #40197930220](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/40197930220), CCTO `fb44fe23b2bee640636a399b1ecdc7d56c5188a0`: 12 independent actual MO + Agriculture plant-only simulated-calendar growth rows, Pickle1/1 PASS, Unity ERROR0. At 7°C Cabbage first harvest **38.23 simulated days**, at 3°C no harvest in 50 days (36.8% growth), whereas Onion harvests at 7°C in 11.77 days and Carrot at 3°C in 16.83 days.
- Draft-only targeted conditional CCTO MO PatchOperationReplace corrects the **second** duplicated minOptimal tag to `maxOptimalGrowthTemperature=25` while keeping original 0; skips future upstream fixed Def. [Follow-up native original 1.6 run #40198682954](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/40198682954), source `c6022cf3dd5f5abce227497c5118ad098091e154`, original MO/VEF/SYR + Agriculture `c2bd116`, 12/12 row validation, Pickle1/1 PASS, Unity ERROR0, exit0. Loaded optimum bound **0–25°C** verified. Cabbage 7°C and 3°C both **11.27 simulated days to harvest / 17.23 to maturity**, versus Carrot 3°C **16.83 / 25.23**, and Onion+Lentil 3°C no growth. First-maturity stop prevents postharvest simulated lifespan effects. These are **Plant.TickLong calendar jumps** with controlled actual ambient, not normal 50-day map ticks/pawn jobs.
- This corrects misinterpreted MO XML, **not** an independently verified biological optimum: Cabbage grows as fast at 3°C as 25°C with minOptimal0, which needs realism review before full V3 release acceptance. Primary source comparison: [JAあいち三河](https://www.ja-aichimikawa.or.jp/garden/kyabetu.php) gives suitable growth 15–20°C, still grows at 5°C; [University of Florida IFAS](https://ask.ifas.ufl.edu/publication/PI042) distinguishes **growth range 0–25°C** from **optimum 15–20°C**. The MO 0/25 fields may have mislabeled the *range* as *optimal range*. Owner Draft role document includes these references. No automatic replacement with 15/20 nor A/D rebalance is approved; resolve optimum realism against carrot/onion economy before merging the Draft. The approved V3 46 HP+32 dormancy canonical A/D values, Agricultural growDays and other plants' temperature fields were not changed. No new numerical tuning is approved by this test.


## V3 actual MO + Agriculture crop telemetry — 2026-10-25

**Owner:** [V3 implementation Draft PR #60](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/60), formal loaded role/harvest summary at its `Docs/CropGameplayRoles-ja.md`. **Status:** actual original 1.6 provider mature-yield observations PASS (10 crops × three controlled conditions); seven gameplay economy roles remain INCOMPLETE. No V3 numerical change, production merge, or publication.

- Original licensed Linux RimWorld 1.6.4871 with **actual Medieval Overhaul, Agriculture and CCTO V3** (not 29-Def MOFixture) [manual Project Actions #40188270406](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/40188270406): source CCTO `9f69168f207c456f004cbc8ee805da7d65edca9c`, Agriculture `c2bd11656aabc150841a1d5e13fd6b32f306be2d`; 30 unique plant/condition rows, 1/1 Pickle passed, 0 Unity ERROR, exit 0. Original MO/VEF/SYR providers loaded, temporary copy-only sound conversion. Project manual single-run source runner main [PR #1038](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/1038), merge `082bb38b5c5487cb1df7709cc9ef68cac12b8fcf`.
- Effective loaded `growDays`: Rice5, Potato6, Corn13, MO Onion8, Carrot6, Cabbage8, Lentil8, Wheat12, Tomato7, Pumpkin8. Lentil sow Plants2. Mature `YieldNow` in warm / −5°C×8h-equivalent / −10°C×4h-equivalent frost: Onion15/15/14, Carrot7/7/7, Cabbage12/11/11, Lentil12/12/11, Wheat33/33/33, Tomato9/0/0 and Pumpkin24/0/0. These are true loaded-game YieldNow/HP measurements, **not** an actual pawn harvest or crop-growth calendar. Frost hours were simulated via 2000-tick cold-observation offsets rather than normal TickManager elapsed time. Check source-produced `balance-rows.csv` artifact (run #40188270406).
- **Material role warnings:** Onion is not the originally assumed fast 4-day crop and Lentil is not 3-day/Plants8 in MO+Agriculture. Onion beats Cabbage at same source 8 growDays in three bounded exposures, but Cabbage's lower minimum growth temperature may matter for the untested shoulder season. Tomato/Pumpkin survive no full -5°C×8h or -10°C×4h exposures; no claim of typical-climate frequent death. No food/labor/processing cost measurements, real season, 1.7, or 7/7 role PASS.
- Keep the finalized V3 46 HP/32 dormancy w0 canonical values and released old fixed-death package unchanged. No follow-on native matrix without a concrete gap and author approval.


## V3 original 1.6 native technical acceptance — 2026-10-25

**Owner:** [V3 production Draft PR #60](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/60). **Status:** 1.6 SOURCE BUILD + BOUNDED NORMAL TICK + REAL TWO-PROCESS .RWS ACCEPTED; source not merged / 1.7 and crop production balance open / Steam unchanged.

- Final main numeric authority remains [78-Def V3 table](ColdDamageHpV3Candidates.csv): 46 HP, 32 dormancy, w=0. The final production/fixture patch compiled as an isolated actual original RimWorld 1.6 runtime. Original [Project manual run #40177263201](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/40177263201): CCTO source `e71341f78d9802bbc6d79e99f392e443d7cd16d4`, compiled DLL SHA256 `522d3e526d00ce9efabdf5592532de39727d491716c77e7945bb5585f841f56c`, all **19/19 original Pickle PASS, original Unity ERROR0, exit0**. Includes full native TickManager exposure with temperature held to the test-defined value; production Potato loses HP (without automatic cold healing), Healroot dormancy, Pine ordinary cold survival. It **does not** validate full-year unforced weather, real MO binaries/actual harvest output or 1.7.
- Original [Project manual run #40177836437](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/40177836437): production source `17b9522569825f76e91f3ddfd8ff631af12a82f0` (only test-only save-probe change from above), source-built DLL SHA256 `cbdaeaf68960dc966bf9e969e25e5f48f840dce1b18e2d3427a8772bcdc70fe1`; one true 706,705-byte `.rws`, save SHA256 `3231dee15b2bd0d195ca983ed01cdcfefcdae8003e86bbab6621d9a3bec1dd2c`, separate second original 1.6 process restoring exact Plant_Potato2204 identity, HP injury 0.20, lastTick30 at Scribe, then resumed cold injury. Both processes exit0, original Unity ERROR0. Warm zero-remainder auxiliary record cleanup is permitted; lost Plant HP remains.
- Previous test-only blockers: [#40176185303](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/40176185303) and [#40176830246](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/40176830246) 17/19 because test-controlled outdoor temperature was overwritten by simulated weather. The extra production `TickLong` hook was tried and **removed**; real world Tick lists now pass when intentional constant cold exposure is actually maintained. Initial save [#40177474917](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/40177474917) restored Scribe state but old test incorrectly rejected legal warm record cleanup. No claims that old failures were biological balance failures.
- **Still open:** full MO source/runtime gameplay, true 1.7 compiled/native acceptance, native crop harvest/fatal-notification/late-season and biome/ecology, long-period performance, build/install of a distributable production DLL, and author-controlled release. Never promote 19 synthetic/bounded native PASS to 7/7 crop economic balance. No automatic game jobs, no Steam publication or CCTO-for-VPE migration.


## V3 production integration and native acceptance — in progress, 2026-10-25

**Owner:** [CCTO Draft PR #60](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/60) at source `4f43985948d49e3ffc284885e8c0a2e7442d3d89`. **Status:** proposed production source/XML integration; **NOT MERGED, NOT GAME-TEST ACCEPTED, NOT PUBLISHED**. The finalized main [78-Def table](ColdDamageHpV3Candidates.csv) remains sole numeric authority (46 HP, 32 dormancy, w=0), not historic Draft #31 CSV.

- Implementation branch brings over intended HP engine and new UI, rewrites Vanilla/MO XML to final V3, retains growth minima and wild MO inheritance, uses dormancy for both Healroots and removes their old fixed-death fallback. Loaded balance assertions and static validator now check V3 fields. Native source checks must not be mislabeled as gameplay acceptance.
- Project manual 1.6 bridge [PR #1027](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/1027) merged `a16bec54df001ba43f4bb1234216dc43c7943f25`; prelaunch uniqueness correction [PR #1028](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/1028) merged `92e9a38bc89d608eaa839dd2c780807218cfe232`. Its exact source pin is this PR's `4f4398...`, 19 Pickle cases, plus the genuine two-process Scribe probe now using production Potato A−2/D100. Earlier run [#40176000425](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/40176000425) failed **before compile or game launch** due to 16-vs-19 preflight mismatch and is not a model failure or PASS.
- Manual game acceptance started with Project issue #39 `/amj ccto-cold-16`; evaluate actual original-game result, exact compile hashes, normal Tick, fatal notification, production save/load and any regressions. Only affected cases should be rerun after repairs. Full seasonal crop efficiency and realistic weather/forest/ecology remain distinct untested balance questions. Do not enable PR/push-triggered game tests, optional VPE expansion or Steam publication.


## Current V3 numerical design — finalized and reconciled, 2026-10-25

**Numerical decisions complete; production cutover and native acceptance remain open.** The author delegated audit/correction/re-audit and final value selection, then authorized reconciling and merging these values and Japanese crop-use documentation to main. Current authority: [Design](Design.md#current-v3-numerical-specification--finalized-2026-10-25), [78-Def value table](ColdDamageHpV3Candidates.csv), [final audit](Research/ColdDamageV3BatchReview.md), [Japanese gameplay roles](CropGameplayRoles-ja.md).

- **46 HP + 32 dormancy, zero pending values; HP variation w=0.** All seventeen previously selected A/D pairs are retained. The later author selection **Haygrass A-8/D220** supersedes this chat's older A-12; both Healroots use dormancy, consistent with the later owner choice. Saguaro is A-3/D180. Remaining values are finalized under delegated review, not falsely attributed to individually specified author numbers or measured laboratory LD100.
- Reconciliation source: main `895a7506c6719919387e3e253d7ef03887dbaf6e`, owner draft `980ea65` (including later flax research). Flax A-6/D140 is a reviewer-only alternative there, not a new author choice; final A-5/D140 is retained and the research is linked in the audit. No other later A/D decision was found in the inspected delta.
- Current production XML still uses the old fixed-death/dormancy model. This integration is **documents + balance data + offline calculator only**, with no runtime C#, DLL, language, production patch or game-test changes. No old native evidence is promoted to V3 acceptance. V3 runtime source remains on Draft [PR #31](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/31), parent Draft [PR #24](https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul/pull/24).
- Next: implement the finalized XML against the intended HP runtime, remove the old Healroot fixed-death extensions rather than keeping -9 as a fallback, build the exact source, then consolidate normal-tick HP/harvest/fatal notification/real-save load acceptance. The author chose **CCTO completion before for-VPE migration**. Do not start optional VPE expansion or publication as part of this integration.
- Dormancy's source implementation refreshes leafless state; it does **not** implement a harvest prohibition. Framework documentation is corrected. Retain this distinction for the actual Healroot cutover.
- Historical mixed-survival S16/S24 preferences are explicitly retired for the finalized w=0 design; gradual HP loss does not require a surviving subgroup of identical plants. Historical lab endpoints and no-cold-healing limitations remain documented and are not relabeled PASS.

## Runtime evidence boundaries still open

Earlier original 1.6 source compile evidence is build-only; the V3 normal-tick, integer HP, fatal message and real .rws save/load acceptance remains pending. The older native/Scribe run records in Git history do not establish acceptance of this final profile. Do not re-run costly native jobs for each documentation edit.

Original world-climate evidence: [Project Actions #39776410000](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39776410000), original RimWorld Linux 1.6.4871 rev598, 4,320 hourly world/tile samples, source CSV SHA256 6eac0d50124fbd622d37737abd1a86223b3649b27133e3e9349a43797e37b8c5. This is not 60 days of loaded-map weather/crop gameplay or a V3 forest PASS. Prior V1 false balance approval stays retracted; see pre-integration main history and the owner draft research linked from the final audit.

## Retained independent work

The superseded plant-by-plant selection chains have been compacted from the former 65k-character log. Canonical evidence remains in owner documents, draft history and prior main commits; the resolved numerical queue is not repeated as pending work.

### CCTO-VPE-ADDON-OWNERSHIP-20261013 — VPE family expansion routed

**Owner:** [CCTO-for-VPE](https://github.com/sucRo-RimWorld/CCTO-for-VPE) owns all VPE-family plant-provider cold-tolerance data, [issue #8](https://github.com/sucRo-RimWorld/CCTO-for-VPE/issues/8).
**Status:** SINGLE ADDON OWNER CONFIRMED; OPTIONAL PACK IMPLEMENTATION OPEN; CCTO BASE UNCHANGED.

More Plants, Mushrooms, Succulents and Flowers are inventoried as candidates only. Future VPE family pack-specific XML and numeric values are assigned to the existing CCTO for VPE Mod (conditionally applied only when each verified pack exists), not to base CCTO or a new separately published Mod per pack. Base Vanilla/MO values and MO integration remain unchanged; source parity now covers nine explicit counterpart/analogue pairs in addon CI, including Cabbage and Orange→Lemon. No external game test or Steam release has been performed for the new packs.



### CCTO-VPE-PARITY-20261013 — nine counterpart values guarded in addon CI

**Owner:** CCTO owns Vanilla/MO original XML; [CCTO-for-VPE](https://github.com/sucRo-RimWorld/CCTO-for-VPE) owns its VPE-only values/test.
**Status:** SOURCE PARITY FOR 9/9 CHECKED ON ADDON PR AND MAIN; VPE NATIVE ACCEPTANCE OPEN.

For original seven counterpart crops (grass, tall grass, agave, onion, pumpkin, tomato, apple), the addon matches CCTO main XML across growth-minimum, cold-death and dormancy; this now includes Cabbage matching MO Cabbage and Orange matching MO Lemon as an explicitly labeled citrus gameplay analogue. Addon PR #6 (main `26ca4bb1ed0ee4007a582ecd0161f8bbc1bdd7ab`) introduced a CI checkout of CCTO main XML to catch divergence. When changing any counterpart value in this CCTO repository, consult the addon owner and run its manual `workflow_dispatch` parity check (CCTO-only pushes do not run the addon CI). No CCTO XML/source behavior has changed. The addon's runtime/save behavior is unverified by author decision (not a pre-publication test gate), while Steam publication remains author-controlled and user bug reporting remains open on [CCTO-for-VPE issue #4](https://github.com/sucRo-RimWorld/CCTO-for-VPE/issues/4).



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
