# CCTO Coordination

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
