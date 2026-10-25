# Crop Cold Tolerance Overhaul — 0.1.4 Beta

Planned source release: `0.1.4` (suggested tag `v0.1.4-beta`; **no tag, Steam upload or subscriber package is created by this document**).

## Changes / 変更内容

- **Vanilla Pine (`Plant_TreePine`):** fixed cold-death temperature **−35 → −40°C**.
- **Medieval Overhaul Great Fir (`DankPyon_GreatFir`, when MO is loaded):** fixed cold-death temperature **−35 → −40°C**.
- **バニラのマツとMOのGreat Fir：** 固定枯死温度を**−35℃から−40℃**に下げた。

## Reason / 変更理由

An original RimWorld 1.6 climate sample ([Project run #39776410000](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39776410000)) reached **−37.835°C** on a Pine-eligible BorealForest world tile, with **11 sampled hours colder than the prior −35°C instant-death threshold**. That creates a plausible risk of disproportionate evergreen losses in typical severe winters. Pine foliage and other conifers can acquire high frost tolerance through winter acclimation; published tissue studies support cold-hardiness in this range. **−40°C is a pragmatic species-role approximation, not an experimentally measured whole-tree instant-death point**. The cold tile was not itself run through an entire normal-map season, and actual Pine/Great Fir deaths were **not** demonstrated by this sample.

RimWorld 1.6の実気候サンプル（[Project実行 #39776410000](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39776410000)）では、マツが生育可能な寒冷林タイルの気温が**−37.835℃**に達し、旧枯死温度の**−35℃を11時間下回った**。厳冬期の気温だけで針葉樹が不自然に枯死するリスクを抑えるため、**−40℃**を採用。針葉樹の冬季耐寒性を踏まえたゲーム向け近似値であり、樹木全体の実測致死温度ではない。このタイルの実マップで枯死を確認したわけではない。

## Unchanged / 変更しないもの

- Fixed-temperature death mechanism, C# engine and XML framework.
- All other cold-death temperatures; 30 existing dormancy profiles.
- Minimum growth/optimal growth temperatures, yield, growDays, fertility and recipes.
- Save compatibility semantics. No cumulative Plant-HP cold-injury model (former internal V2/V3 / 0.2 development remains on hold).
- 固定枯死方式・既存の休眠・他の植物数値・最低生育温度・作物生産性は変更しない。HP累積寒害方式は未導入。

## Verification status

- Source comparison: **only the two intended fixed-temperature XML values** change from the 0.1.3 source; plant identities, growth fields and other cold values are preserved.
- Original 1.6 native world-tile climate evidence is **context**, not 0.1.4 loaded-map acceptance.
- Previous 0.1.3 **14/14** integration and game-smoke evidence are for unchanged baseline runtime, **not new-number test results**.
- **Still required before public release:** targeted game/profile check of the 0.1.4 package and author-controlled Steam publication/installed-package verification; 1.7 coverage should be stated explicitly in that review.
