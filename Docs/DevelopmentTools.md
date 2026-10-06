# CCTO Development Workflow

Before code/framework work, read the authoritative cross-workstream handoff log at `main:Docs/Coordination.md`. Update it directly instead of asking the user to relay messages between chats.

This file contains **CCTO-specific** development and test guidance only.

General RimWorld mod-development tools, enablement policy, Local Work/Codex usage, and Sol/Astra escalation rules are maintained separately in the ChatGPT Library:

`/Ancient & Medieval Japan/RimWorld_Mod_Development_Environment.md`

That shared document is the source of truth for cross-project tooling policy. Do not duplicate those general rules here.


## 自動テスト優先方針（AMJ共通）

AMJおよび関連Modでは、RimTest Redux・Pickleを積極的に用いた自動テストを優先し、人間による手動テストを最小限にする。

- ロジック・計算・設定検証などはRimTest Redux、ロード後のDef・実際のゲーム内挙動・統合回帰などはPickleを中心に、適した自動テストで確認する。
- 新機能・不具合修正では、再現可能な確認を可能な限り自動化し、リリース前の回帰確認も自動テストへ寄せる。既存のビルド・XML・静的検証は併用する。
- 手動テストは、画像の見た目、UIの読みやすさ、操作感・遊び心地など、人間の目視・操作が必要な項目に限定する。自動で確認済みの数値や挙動を毎回手動で再確認させない。
- 自動化が未整備の項目は、未検証範囲と自動化する対象を明示する。静的検証の成功を実行時テストの成功として扱わない。
- RimWorldを起動する自動テストでは、既存の実行時ERROR検出方針を必ず適用する。シナリオが全件成功しても対象Mod由来のERRORがあれば全体を失敗とする。

## 対話型開発補助ツール（AMJ共通）

人間の目視・操作が必要な確認を短時間で準備するため、次のツールを開発専用として利用してよい。これらは自動テストの代替ではなく、出荷Modの依存関係にも含めない。

- **DevKit — Better Dev Mode Menu** (Workshop `3814373104`): Def / defName 検索、Thing・植物・建築物・Pawn等の配置、Debug Action検索、Favorites / Recent等を使い、目視確認・デバッグ用の盤面準備を高速化する。
- **Rim Control** (Workshop `3774299554`): ゲーム中の数値・visual・placement等を一時変更し、バランス値や表示設定の候補を素早く比較するためのプロトタイピング用途に使う。

運用ルール:

- DevKitでスポーン・配置できたこと自体を、実装またはテスト成功の証拠として扱わない。
- Rim Controlで変更した値は試作値にすぎない。採用する場合は、必ず所有リポジトリのXML / C# / Def / 正式設計書へ反映して正本化する。
- 正本へ反映した後、Rim Controlの上書きを無効化し、必要な静的検証・RimTest Redux・Pickle・通常のruntime gateで再検証する。
- 自動テスト、リリースゲート、正式な通常プロファイル確認では DevKit / Rim Control を無効化する。例外は、そのツール自身との互換性を明示的に調べるテストだけとする。
- 可視実行を行う場合も、用途は最終テクスチャ、通常ズーム視認性、UI、操作感、遊び心地など、人間判断が必要な項目に限定する。

## 1. CCTO responsibility split

CCTO development is intentionally split into two workstreams.

### Balance/XML workstream

Owns:

- plant-specific `minGrowthTemperature` values;
- plant-specific fixed `coldDeathTemperature` values;
- Vanilla / Medieval Overhaul balance XML;
- CCTO-owned supported-set balance decisions.

AMJC/AMJE plant values, balance documents, and framework-consumer compatibility XML are maintained in their owning repositories, not in this workstream.

The code workstream must not silently redefine decided balance values.

### Code/framework workstream

Owns:

- `ColdToleranceExtension`;
- Harmony integration with RimWorld plant cold behavior;
- fixed cold-death handling;
- low-temperature dormancy support;
- Info Card display;
- English/Japanese framework UI strings;
- validation and diagnostics;
- build tooling;
- automated framework tests.

## 2. Local repository layout

Recommended local checkout:

`D:\SteamLibrary\steamapps\common\RimWorld\Mods\CropColdToleranceOverhaul`

This allows the repository to be both the Git checkout and the active local RimWorld mod.

Build with:

`build.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

Expected output:

`Assemblies\CropColdToleranceOverhaul.dll`

## 3. CCTO-specific development helpers

Enable only what the current CCTO task needs.

| Task | Preferred helper |
|---|---|
| framework logic / Harmony regression | RimTest Redux |
| actual temperature, tick, death, dormancy, recovery scenarios | Pickle |
| verify final loaded plant Def values | Things Explorer |
| verify XPath / XML patch results | XML Patch Helper |
| startup/runtime exception investigation | RimDoctor + Better Stacktraces |
| rapid reproducible map entry | Quickstarts |

The shared Library document contains the general enable/disable policy for these tools.

### Steam Workshop upload via YADA

YADA (Yet Another Dev Assistant) may be used for CCTO's in-game Steam Workshop upload workflow.

CCTO keeps a repository-root `.rimignore` specifically for YADA. Its purpose is to exclude repository/development material from the Workshop package while preserving runtime content.

The intended uploaded runtime set includes:

- `About/`
- `Assemblies/`
- `Languages/`
- `Patches/`
- `LICENSE`

The filter excludes repository-only documentation, source, tests, scripts, batch tooling, test reports, local test patches, version-control metadata, and debug/build leftovers. Do not add `Assemblies/`, `Languages/`, or `Patches/` wholesale to `.rimignore`.

Workshop presentation sources are kept separately:

- `README.md` — public-content source of truth;
- `Docs/WorkshopDescription.md` — title, version, categories, and maintained Workshop presentation;
- `Docs/SteamWorkshopDescription.txt` — paste-ready BBCode body for the Steam Workshop description field;
- `Docs/2GamePresentation.md` — RimWorld Mod データベース (2game) presentation/tag policy;
- `Docs/2GameDescription-ja.txt` — paste-ready Japanese description for 2game.

When public feature/balance information changes, keep the relevant publication surfaces synchronized without copying README-only development/test detail into them.

## 4. CCTO validation sequence

### Automated release gate

The authoritative automated gate is `run-tests.bat`. On the integrated balance branch/final mod it performs both static balance validation and the Pickle + Quickstarts runtime suite, and returns a process exit code.

Build/run with:

`run-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

The full gate requires the installed Medieval Overhaul source tree (Workshop 3219596926) so the validator can check the real 1.6 plant DefNames and XML structure.

It intentionally does **not** activate Medieval Overhaul's runtime assemblies in the isolated Pickle profile. MO's own Harmony startup is outside CCTO's responsibility and can fail under an artificially stripped mod set even when the Def XML CCTO patches is valid. Instead, the gate stages a developer-only mod named `Medieval Overhaul` containing lightweight versions of the 29 MO-specific target plant Defs. RimWorld's `PatchOperationFindMod` matches active mods by display name, so CCTO's real MO patch file executes against these fixture Defs.

This is paired with static checks against the installed real MO 1.6 source: every target DefName must exist and must contain the local `<plant>` node required by CCTO's XPath. The four wild alchemy Defs are also checked for their real Named-Parent links to the cultivated alchemy Defs. Their CCTO extension is inherited from that parent rather than appended a second time. Thus the runtime fixture tests CCTO patch application and inheritance while the static validator protects against drift in the actual MO source structure.

The same static validator also enforces framework-consumer ownership: CCTO's runtime patches, source, tests, and implementation mapping must not contain `AMJC_` Def/data identifiers. AMJC-owned plants and compatibility data belong in the AMJC repository. Historical coordination notes are intentionally outside this gate.

`run-e2e.bat` remains usable by itself for framework-only testing. `run-tests.bat` invokes its `with-mo-fixture` mode for the complete balance integration gate.

The underlying E2E runner:

- builds the shipping CCTO DLL;
- creates a separate local developer test mod at
  `D:\SteamLibrary\steamapps\common\RimWorld\Mods\CropColdToleranceOverhaul.E2E`;
- creates a second developer-only MO Def fixture at
  `D:\SteamLibrary\steamapps\common\RimWorld\Mods\CropColdToleranceOverhaul.MOFixture`;
- compiles CCTO-specific Quickstarts and Pickle step assemblies;
- copies all CCTO `.feature` files;
- prepares an isolated RimWorld save-data profile under `TestResults\SaveData`;
- copies the normal `Prefs.xml` into that isolated profile and forces only `devMode=True`, because Quickstarts does nothing when RimWorld dev mode is off;
- gives that profile a minimal test-only mod list; framework-only runs contain Harmony, Core, RimLogging, Pickle, Quickstarts, CCTO, and the CCTO E2E companion mod, while the full balance gate additionally enables only the lightweight MO Def fixture before CCTO;
- launches RimWorld with `-savedatafolder` pointing at that isolated profile and redirects Unity/RimWorld output to an isolated `TestResults\Pickle\Player.log`;
- runs framework regression scenarios and live cold-behavior scenarios;
- clears the previous isolated save-data and Pickle report directories before every run, preventing a startup failure from being mistaken for a stale earlier PASS;
- writes fresh reports to `TestResults\Pickle`;
- in full balance mode, parses the fresh `summary.json` and requires exactly 14 clean passes plus explicit presence of the Core loaded-balance scenario, the Medieval Overhaul loaded-balance scenario, and the all-loaded-supported-living-plant coverage scenario;
- after scenario validation, scans the isolated runtime log and fails the full gate if CCTO itself emitted any ERROR-level entry; a clean 14/14 scenario count is not sufficient when the mod logged an error;
- exits with Pickle's pass/fail/error result, an integration-summary error if the expected balance scenarios were not actually run, or a runtime-log error-gate failure;
- is wrapped by an outer five-minute process watchdog, so a RimWorld/Pickle startup or runtime freeze cannot leave the batch file waiting indefinitely.

The user's normal RimWorld `ModsConfig.xml` is read only to reuse the current RimWorld version/known-expansion metadata. It is not rewritten. This prevents unrelated gameplay mods and their log errors from causing false Pickle failures.

Both generated test mods are development-only and must not be included in the Workshop release. Remove them with:

`clean-e2e.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

The automated Pickle gate no longer depends on the user's normal active mod preset. `Scripts/Prepare-TestSaveData.ps1` writes a separate test-only `ModsConfig.xml` inside `TestResults\SaveData`, and `run-e2e.bat` launches RimWorld against that profile.

The normal gameplay preset is therefore left untouched, and unrelated mods do not need to be manually disabled before an E2E run.

`Scripts/Check-TestModList.ps1` is retained only for workflows that intentionally run development helpers against the normal active profile; it is not part of the authoritative Pickle release gate.

### Current Pickle gate coverage

Loaded balance-Def scenarios in the full integration gate check:

- all 49 living Core PlantDefs have the final expected `minGrowthTemperature`, exactly one CCTO extension, and the expected fixed death temperature or dormancy flag;
- all 29 targeted Medieval Overhaul fixture Defs have the same final loaded-value checks after CCTO's real `PatchOperationFindMod`/XPath patch runs; the fixture mirrors MO's four wild-alchemy Named-Parent links, so the loaded checks also require exactly one inherited/direct CCTO extension per Def;
- a coverage scenario scans all loaded living PlantDefs in the isolated Core/MO profile and fails if a living plant lacks CCTO balance or falls outside the curated 49-Core / 29-MO target set; dead stump/remnant Defs are excluded;
- Medieval Overhaul's installed 1.6 source XML contains every targeted DefName **and** a local `<plant>` node before the runtime suite starts;
- the MO patch contains **25 direct CCTO extension additions + 4 inherited wild-alchemy extensions**, preventing duplicate `ColdToleranceExtension` entries on the four wild alchemy Defs.

Framework regression scenarios check:

- fixed death thresholds are identical across different plant identities;
- dormancy uses native `minGrowthTemperature` as its cold-response threshold;
- unconfigured plants retain the vanilla per-plant threshold range;
- extension validation accepts dormancy-only and finite fixed death values while rejecting incomplete/infinite configurations;
- CCTO Info Card stat construction matches fixed-death, dormancy-only, and combined configurations.

Live-map scenarios check:

- a configured plant survives safely above its fixed death threshold;
- the same plant dies when its actual `Plant.TickLong()` path runs below the threshold;
- at exactly the fixed threshold the plant survives, while below it the plant dies (strict `<` boundary);
- a `dieIfLeafless` plant can enter CCTO dormancy without dying;
- a dormant plant with an explicit extreme-cold death threshold still dies below that threshold;
- cold dormancy preserves RimWorld's delayed 60,000-tick leafless recovery window after warming; the test verifies the stored `madeLeaflessTick` and advances that private recovery timer directly rather than simulating an entire in-game day;
- a heated indoor plant survives lethal outdoor cold when its actual room temperature is safe;
- a configured plant in a genuinely cold indoor room responds to its actual `AmbientTemperature`.

Each live scenario starts from a fixed-seed Quickstarts world. CCTO test temperatures are controlled by setting the relevant RimWorld `Room.Temperature` directly and verifying the spawned plant's resulting `AmbientTemperature`. This avoids mutating global world temperature caches merely to exercise CCTO's ambient-temperature checks. The indoor regression uses separate indoor and outdoor rooms to model warm-inside/cold-outside behavior.

Full-world fast waits are avoided for CCTO cold-response assertions. Earlier `I wait 2100 ticks` steps advanced all Vanilla world/pawn systems and could surface unrelated WorldPawns/AI errors. CCTO now calls the spawned test plant's `TickLong()` directly, which exercises the real plant long-tick cold path without ticking unrelated simulation systems.

### RimTest Redux developer suite

A separate RimTest Redux suite remains available as a fast interactive/unit-style development tool.

Build it with:

`build-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

It creates:

`D:\SteamLibrary\steamapps\common\RimWorld\Mods\CropColdToleranceOverhaul.Tests`

This keeps `RimTestRedux.dll` out of the shipping CCTO assembly. Remove the generated test mod with:

`clean-tests.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

Its current coverage overlaps the framework portion of the Pickle gate: extension validation, fixed-vs-vanilla thresholds, dormancy threshold behavior, and Info Card stat construction. RimTest Redux is useful for rapid development feedback; Pickle is the release gate because it can also exercise a live RimWorld session and produce an unattended process result.

### Final pre-beta checks

After the full automated gate passes, prepare the normal-game smoke environment with:

`prepare-smoke.bat "D:\SteamLibrary\steamapps\common\RimWorld"`

The script:

- removes generated CCTO E2E/MO-fixture and RimTest companion mods;
- rebuilds the shipping CCTO DLL;
- reads, but does not rewrite, the normal RimWorld `ModsConfig.xml`;
- requires Harmony, Core, Medieval Overhaul, and CCTO to be active;
- requires CCTO E2E/MO-fixture/RimTest, Pickle, Quickstarts, and RimTest Redux to be inactive.

Automated loaded-value assertions are authoritative for the numeric expectations below. Normal-profile startup/compatibility checks are targets for further automation; retain only layout/readability and other human judgment checks as manual work. Do not repeat numeric checks manually after an unchanged automated gate has passed.

Then launch RimWorld normally, without `-savedatafolder`, and perform this representative smoke/visual check:

| Case | Expected Info Card |
|---|---|
| Vanilla rice | minimum growth temperature 10 C; `枯死温度` -1 C; no `低温反応: 休眠` |
| Vanilla hops | minimum growth temperature 5 C; `休眠温度` 5 C; no `枯死温度` |
| Vanilla bamboo | minimum growth temperature 10 C; `枯死温度` -18 C; no `休眠温度` |
| MO wheat | minimum growth temperature 0 C; `枯死温度` -6 C; no `休眠温度` |
| MO apple tree | minimum growth temperature 5 C; `休眠温度` 5 C; no `枯死温度` |

The exact spacing/unit formatting around Celsius is RimWorld-native and is not a CCTO wording requirement.

Pass criteria:

1. normal startup completes with the real gameplay mod profile and no new CCTO-origin error;
2. the five representative cards show the expected data above;
3. `枯死温度` and `休眠温度` are readable and visually grouped sensibly with RimWorld's native plant temperature entries;
4. no duplicate CCTO temperature rows appear;
5. opening/closing the Info Card and returning to play causes no visible error or UI breakage.

Things Explorer/XML Patch Helper are optional diagnostics only if a discrepancy is found; exact underlying Vanilla/MO values are already asserted by the automated loaded-`DefDatabase` gate.

The normal-game smoke/visual check and the Nice Plants Menu compatibility smoke passed for the published 0.1 Beta baseline. After the 49-Core / 29-MO all-living-plant expansion, rerun the full gate and normal-profile smoke before publishing the updated balance.

### Nice Plants Menu compatibility smoke

When **Nice Plants Menu** (Workshop 3685058533) is active, repeat the normal-game visual check in its compact plant-summary panel:

| Case | Expected Nice Plants Menu additions |
|---|---|
| Vanilla rice | `枯死温度` -1 C |
| Vanilla hops | `休眠温度` 5 C |
| MO wheat | `枯死温度` -6 C |
| MO apple tree | `休眠温度` 5 C |

Pass criteria:

1. CCTO rows appear immediately after Nice Plants Menu's growth-temperature row;
2. fixed-death crops show only `枯死温度`;
3. dormancy-only crops show only `休眠温度`;
4. there are no duplicate CCTO rows;
5. opening/closing the plant menu produces no new CCTO error;
6. Nice Plants Menu remains optional: the same CCTO build starts normally when that mod is absent.

Dubs Mint Menus needs no dedicated compatibility smoke beyond confirming that its mouse-over standard Info Card continues to show CCTO rows; that path has already been verified.

## 5. Framework behavior that tests must protect

Regression tests should cover at least:

- plants without `ColdToleranceExtension` retain vanilla behavior;
- configured plants use one fixed Def-level `coldDeathTemperature`, not per-plant randomness;
- ordinary configured plants die below the configured threshold;
- they survive above the configured threshold;
- dormancy begins below native `minGrowthTemperature`;
- dormant plants do not die merely because vanilla `dieIfLeafless` is true;
- a dormant plant with an explicit `coldDeathTemperature` can still die under more extreme cold;
- dormancy clears after temperature recovery according to the intended CCTO behavior;
- Info Card shows the configured fixed death temperature;
- Info Card shows dormancy for dormancy-type plants;
- duplicate CCTO extensions are diagnosed;
- invalid extension configuration is diagnosed.

Items involving actual spawned-map temperature, death, dormancy persistence/recovery, and save/runtime interaction remain E2E targets rather than unit-style RimTest targets.

## 6. Current integrated source

The tested release source of truth is now `main`.

Historical development branches `framework-code` and `balance-xml` produced the framework and integrated balance work, but new release validation should be run against the current `main` state unless a new work branch is intentionally created.

## 7. Agent/model use for CCTO

Use the shared Library document for the general Sol/Astra rules.

CCTO-specific default:

- ordinary framework edits, compiler fixes, test authoring, XML API work: **Sol is sufficient**;
- escalate to **Astra** only if CCTO develops a difficult cross-mod Harmony/IL conflict, unclear multi-assembly runtime behavior, or another problem that meets the shared escalation criteria.

Do not spend Astra merely on routine CCTO file edits or compile-error loops.

