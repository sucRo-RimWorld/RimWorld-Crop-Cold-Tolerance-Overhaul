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

## 自動テスト優先方針（AMJ共通）

AMJおよび関連Modでは、RimTest Redux・Pickleを積極的に用いた自動テストを優先し、人間による手動テストを最小限にする。

- ロジック・計算・設定検証などはRimTest Redux、ロード後のDef・実際のゲーム内挙動・統合回帰などはPickleを中心に、適した自動テストで確認する。
- 新機能・不具合修正では、再現可能な確認を可能な限り自動化し、リリース前の回帰確認も自動テストへ寄せる。既存のビルド・XML・静的検証は併用する。
- 手動テストは、画像の見た目、UIの読みやすさ、操作感・遊び心地など、人間の目視・操作が必要な項目に限定する。自動で確認済みの数値や挙動を毎回手動で再確認させない。
- 自動化が未整備の項目は、未検証範囲と自動化する対象を明示する。静的検証の成功を実行時テストの成功として扱わない。
- RimWorldを起動する自動テストでは、既存の実行時ERROR検出方針を必ず適用する。シナリオが全件成功しても対象Mod由来のERRORがあれば全体を失敗とする。

## Automated runtime-error gate

For any automated test that launches RimWorld, a passing scenario/test count is not sufficient by itself.

The test harness must capture an isolated runtime log and fail the overall test run if the repository-owned mod emits any ERROR-level entry. Do this even when all Pickle/RimTest scenarios otherwise pass. Warnings remain non-fatal unless a repository-specific test explicitly promotes them.

Any new RimWorld runtime-test harness added to this repository must include this mod-origin ERROR gate from the start. Static-only validation does not fabricate a runtime-log result; add the gate when runtime automation is introduced.

## Public mod descriptions

Use the CCTO-based shared [mod description guidelines](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Core/blob/main/Docs/ModDescriptionGuidelines.md) when writing or updating public descriptions. Include save compatibility in every mod description, stating addition/removal conditions accurately for the mod's implementation. Keep README, Workshop English/Japanese BBCode, and About.xml consistent; refine the shared baseline as presentation improves.


## Historical description audit (AMJ common)

The shared AMJ historical-description policy lives in Ancient-Medieval-Japan-Core `Docs/HistoricalDescriptionGuidelines.md`.

When CCTO is explicitly asked to contribute AMJ-facing description/localization work for Vanilla or Medieval Overhaul items, plants, or animals, follow that policy: audit from an ancient/medieval Japanese perspective, ground historical claims, include meaningful modern differences when supportable, and use the Japanese-first approval workflow before English translation.

CCTO remains a generic cold-tolerance framework and data owner for its own Vanilla/MO temperature support. Do not rewrite unrelated Vanilla/MO historical descriptions merely because CCTO supports those plants; ordinary AMJ historical presentation belongs in the relevant AMJ content repository unless explicitly coordinated otherwise.

## Framework-consumer data ownership

AMJC and AMJE own their custom plant values, balance/design tables (including archived candidate ranges), Def mappings, and optional CCTO compatibility XML. Do not add those plants to CCTO's data or supported/planned content sets. CCTO provides the reusable API; changes to consumer-owned plant data belong in the owning repository.
