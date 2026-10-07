# CCTO Agent Instructions

Before starting CCTO work, read the authoritative cross-workstream handoff log:

`main:Docs/Coordination.md`

Use that file instead of asking the user to relay messages between separate chats or agents.

Rules:

1. Check open items owned by your workstream.
2. Perform the work directly when possible.
3. Update `main:Docs/Coordination.md` with status/results when an item changes.
4. Put durable design decisions in the proper source-of-truth document/code; Coordination is only the handoff/status surface.
5. Do not create branch-specific copies of `Docs/Coordination.md`; the copy on `main` is authoritative.

## Context reconstruction / source hierarchy

For every new chat or agent session working on AMJ or a related mod, rebuild context from repository sources instead of treating accumulated chat history as the primary source of truth:

1. Read this repository's `AGENTS.md` first.
2. Read the authoritative `main:Docs/Coordination.md`.
3. Read the relevant authoritative design, code, XML/Defs/Patches, localization, Golden Path, or other repository-owned source-of-truth files for the task.
4. Use prior chat history or memory only as supplementary context. If it conflicts with repository sources, the repository sources win.

Store information according to this hierarchy:

- Confirmed specifications, design decisions, accepted values, and implementation facts -> the appropriate formal repository source of truth.
- Cross-chat / cross-agent / cross-workstream handoff, current status, blockers, and requests -> `main:Docs/Coordination.md`.
- Permanent operating rules that should govern future work -> `AGENTS.md`.
- Do not leave a durable decision only in chat history or only in `Docs/Coordination.md`.

## 自動テスト優先方針（AMJ共通）

AMJおよび関連Modでは、RimTest Redux・Pickleを積極的に用いた自動テストを優先し、人間による手動テストを最小限にする。

- ロジック・計算・設定検証などはRimTest Redux、ロード後のDef・実際のゲーム内挙動・統合回帰などはPickleを中心に、適した自動テストで確認する。
- 新機能・不具合修正では、再現可能な確認を可能な限り自動化し、リリース前の回帰確認も自動テストへ寄せる。既存のビルド・XML・静的検証は併用する。
- 手動テストは、画像の見た目、UIの読みやすさ、操作感・遊び心地など、人間の目視・操作が必要な項目に限定する。自動で確認済みの数値や挙動を毎回手動で再確認させない。
- 自動化が未整備の項目は、未検証範囲と自動化する対象を明示する。静的検証の成功を実行時テストの成功として扱わない。
- RimWorldを起動する自動テストでは、既存の実行時ERROR検出方針を必ず適用する。シナリオが全件成功しても対象Mod由来のERRORがあれば全体を失敗とする。

## 非対話ランタイムテスト方針（AMJ共通）

人間の目視判断を必要としない自動テストでは、RimWorldの可視ウィンドウをユーザーのデスクトップへ出さないことを標準とする。詳細な共通正本は Ancient-Medieval-Japan-Core `Docs/DevelopmentGoldenPathGuidelines.md` の **Non-interactive runtime-test rule**。

- Pickle / RimTest Redux / Quickstarts / 統合回帰 / runtime ERROR gate / map・気候・土壌サンプリング等は、原則として非対話・非表示で実行する。
- 描画・Texture Atlas・`Graphic.Draw`・BadTex等を検証する場合は、描画そのものを無効化しない。仮想／オフスクリーン／非表示の表示先など、プラットフォームに適した隔離実行で実描画経路を維持する。
- 描画経路がテスト対象なら `-nographics` 等で迂回しない。
- 可視実行は、最終的なテクスチャ見た目、通常ズーム視認性、UI、操作感、遊び心地など、人間の判断が必要な確認だけに限定する。
- 通常の自動ランナーと、visual / interactive / debug用途の可視ランナーを分離する。
- 既存ランナーが可視ウィンドウを出す場合は移行対象とし、次回の重要変更時または定期回帰ゲート化前に非対話実行経路を追加する。挙動忠実度を保てない場合のみ、理由を文書化した例外を認める。

## Automated runtime-error gate

For any automated test that launches RimWorld, a passing scenario/test count is not sufficient by itself.

The test harness must capture an isolated runtime log and fail the overall test run if the repository-owned mod emits any ERROR-level entry. Do this even when all Pickle/RimTest scenarios otherwise pass. Warnings remain non-fatal unless a repository-specific test explicitly promotes them.

Any new RimWorld runtime-test harness added to this repository must include this mod-origin ERROR gate from the start. Static-only validation does not fabricate a runtime-log result; add the gate when runtime automation is introduced.

## Mod naming rule (AMJ common)

AMJ Core, Environment, CCTO and future related Mods must not use ASCII `:` or full-width `：` in Mod names. Use ` - ` when a separator is needed. Apply this to `About/About.xml` `<name>` and the corresponding Workshop title / formal README name; check it when creating, renaming or preparing a Mod for publication. YADA uses the display name for an upload staging directory, and an ASCII colon causes that step to fail on Windows. Display-name corrections must preserve `packageId` and existing Workshop IDs.

The shared source of truth is [Mod description guidelines — Mod名のコロン禁止](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Core/blob/main/Docs/ModDescriptionGuidelines.md). Colons in description prose, URLs and code syntax are outside this naming rule.

## Public mod descriptions

Public-description preparation and updates must also include the Japanese 2game summary in `Docs/2GameDescription-ja.txt` and its presentation policy in `Docs/2GamePresentation.md`. Follow the shared guideline's **2game向け説明（AMJ共通）** section and CCTO's six-section, plain-style template. Check README, Workshop English/Japanese, 2game Japanese and About.xml together; link named related mods and this mod's own GitHub repository. Record repository preparation separately from live-site publication.

Use the CCTO-based shared [mod description guidelines](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Core/blob/main/Docs/ModDescriptionGuidelines.md) when writing or updating public descriptions. Include save compatibility in every mod description, stating addition/removal conditions accurately for the mod's implementation. Keep README, Workshop English/Japanese BBCode, and About.xml consistent; refine the shared baseline as presentation improves.


## Historical description audit (AMJ common)

The shared AMJ historical-description policy lives in Ancient-Medieval-Japan-Core `Docs/HistoricalDescriptionGuidelines.md`.

When CCTO is explicitly asked to contribute AMJ-facing description/localization work for Vanilla or Medieval Overhaul items, plants, or animals, follow that policy: audit from an ancient/medieval Japanese perspective, ground historical claims, include meaningful modern differences when supportable, and use the Japanese-first approval workflow before English translation. Japanese descriptions must also follow the shared name-form rule: begin with an established kanji form when one exists and include recognized aliases / alternate names or common alternate written forms at the opening; do not invent kanji or weakly sourced names.

CCTO remains a generic cold-tolerance framework and data owner for its own Vanilla/MO temperature support. Do not rewrite unrelated Vanilla/MO historical descriptions merely because CCTO supports those plants; ordinary AMJ historical presentation belongs in the relevant AMJ content repository unless explicitly coordinated otherwise.

## Framework-consumer data ownership

AMJC and AMJE own their custom plant values, balance/design tables (including archived candidate ranges), Def mappings, and optional CCTO compatibility XML. Do not add those plants to CCTO's data or supported/planned content sets. CCTO provides the reusable API; changes to consumer-owned plant data belong in the owning repository.

## Golden Path closeout rule

Follow the AMJ shared Golden Path policy in Ancient-Medieval-Japan-Core `Docs/DevelopmentGoldenPathGuidelines.md`.

After a non-trivial task succeeds, especially after debugging or failed attempts, do not move on with only the working implementation. Record the successful reusable procedure in the owning repository, automate deterministic/repetitive steps, and add regression guards for failure modes discovered during the work. For recurring work, completion includes the reusable documented/automated path, not only the one successful result.

`Docs/Coordination.md` remains status/handoff only; the procedure itself must live in durable repository documentation/scripts.

## Workshop distribution rule (AMJ common)

AMJ Core, Environment, CCTO and future related Mods must exclude **all files unnecessary for a Workshop subscriber** through the repository-root `.rimignore`. This includes Art masters/templates, design and development documentation (including README), source, tests/fixtures/reports, scripts/build tools, VCS/editor metadata, local overrides and debug/backup/archive files. Preserve runtime assets, About metadata, loadFolders.xml where used, and legally required licenses/attribution.

- `.rimignore` is the authoritative exclusion list. YADA uses inherited basename/wildcard rules, not Git-ignore path or negation syntax; exclude `_LocalTest.xml`, not `Patches/_LocalTest.xml`.
- Adding a file/folder includes deciding whether subscribers need it and updating exclusions when they do not. Preserve development/source material in Git; exclusion is not deletion.
- Every alternative publisher/archive/staging builder must produce the same subscriber-only payload. Keep adapters synchronized with `.rimignore`; do not maintain independent policy exceptions.
- Run `python Tests/validate_workshop_payload.py` before publication. Validate the final staging/installed package too; runtime-required DLLs and assets must actually be present. Repository filtering PASS alone is not build/runtime/Steam publication PASS.
- Shared procedure and payload contract: [Core Docs/WorkshopPackaging.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Core/blob/main/Docs/WorkshopPackaging.md).


## Unowned AMJ idea staging

When work in this repository discovers an AMJ idea that may become a separate Mod but does not yet have an owning repository, **do not develop its evolving design here**. Record the concept, research and roadmap state in `sucRo-RimWorld/Ancient-Medieval-Japan-Project` until the author creates/selects an owner repository. Keep only a concise compatibility or ownership-boundary pointer here when it materially affects this repository.

