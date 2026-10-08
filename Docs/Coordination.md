# CCTO Coordination

### CCTO-WORKSHOP-BILINGUAL-HR-20261008 — single English-field description and language rule

**Owner:** CCTO publication/docs  
**Status:** GITHUB SOURCE PREPARED; STEAM UPDATE NOT PERFORMED; CI RESULT NOT ASSERTED

The public title now reads `Crop Cold Tolerance Overhaul（作物耐寒性オーバーホール）`. The paste-ready `Docs/SteamWorkshopDescription.txt` is English text followed by exactly one `[hr][/hr]` divider and Japanese text, with no license/AI/donation notices. `Docs/SteamWorkshopDescription-ja.txt` remains the Japanese source. A static validation script and CI gate prevent omissions and verify combined byte budget and title. No change to packageId, PublishedFileId, framework/cold behavior or source assets. The author must apply changes to live Steam item `3812412548` separately; prior separate-language publication records remain historical.


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

### TEST-POLICY-003 — Non-interactive runtime tests

**Requested by:** author (2026-10-05 JST)  
**Owner:** Testing/tooling  
**Status:** DONE — shared policy adopted; existing runtime harness migration pending

CCTO も AMJ 関連 Mod として、目視不要の Pickle / RimTest Redux / runtime regression は可視 RimWorld ウィンドウを出さない非対話実行を標準とする。描画を検証しない CCTO ロジックテストで可視 UI を要求しない。可視実行は人間判断が必要な確認だけに限定する。

Shared durable source: AMJ Core `Docs/DevelopmentGoldenPathGuidelines.md`, commit `a81c0389fb1834a098a462291b3f4814abcffef3`. Repository instruction: `AGENTS.md`, commit `5ec14463ed758d2bef7754156db0d3ff3680f758`.

**Next action:** runtime test launcher を次回重要変更時に非対話実行へ移行し、既存の isolated save-data profile、watchdog、ERROR gate を維持する。

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
