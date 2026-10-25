# CCTO Coordination

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
