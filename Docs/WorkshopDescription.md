# Steam Workshop Presentation — CCTO

`README.md` is the detailed public-content source of truth. Workshop copy is intentionally shorter and contains only the information needed to understand, evaluate, and install the mod.

## Authoring order

Workshop descriptions are written **Japanese first**.

1. Draft and polish the Japanese description as natural Japanese, using README/design sources for facts.
2. Treat the finalized Japanese Workshop text as the wording/content source for the Workshop page.
3. Translate that Japanese text into English without adding or removing substantive claims.
4. Keep both language versions synchronized when the Workshop copy changes.

Source files:
- Single paste-ready Steam description: `Docs/SteamWorkshopDescription.txt` with **English → `[hr][/hr]` → Japanese**. Paste the full text once in Steam's English description field; do not duplicate it in the Japanese field.
- Japanese authoring source: `Docs/SteamWorkshopDescription-ja.txt` (the combined file contains it verbatim).

## Title

- Canonical English/Japanese Workshop and About title: `Crop Cold Tolerance Overhaul（作物耐寒性オーバーホール）`

## Release stage

RimWorld 1.6 — Beta

## Short description

**Japanese source**

現実の植物に見られる耐寒性の違いを参考に、植物ごとの最低生育温度・枯死温度・低温休眠をより現実的に調整するMod。ほかの植物Modから利用できる軽量XMLフレームワークとしても機能する。

**English translation**

A realism-focused plant cold-tolerance overhaul based on real differences in cold hardiness, with plant-specific growth/death thresholds, dormancy, and a lightweight XML framework for other plant mods.

## Workshop categories / tags

- Category: `Mod`
- Version: `1.6`
- Required DLC: none

## Content policy

The Workshop description should emphasize:
- realism-focused cold-tolerance balance;
- the practical gameplay effect;
- supported content and clearly labeled future compatibility plans;
- framework use by other mods;
- dependencies;
- save compatibility.

Detailed per-plant values, full rationale, implementation details, test results, and release-history detail belong in README/development/release records rather than the Workshop body.

CCTO public descriptions must not list AMJ-owned crops or plants planned for future AMJ releases as CCTO-supported or CCTO-planned content. AMJ consumes CCTO's framework from the AMJ side; those plant plans belong in AMJ documentation.


## Planned compatibility shown on Workshop

- Vanilla Plants Expanded (VPE) support is planned, not currently supported.
- VPE contains more than 100 plants across its wider catalog, so the first compatibility target is the 20-plant basic set.
- Additional VPE coverage is a later consideration rather than a promise of full immediate support.

## Combined public text check

Run `python Tests/validate_workshop_description.py` before publication. It confirms About/Workshop title agreement, one `[hr][/hr]` between languages, Japanese source synchronization, balanced BBCode, required save/dependency disclosures, and a combined UTF-8 budget below 8,000 bytes. Keep license/AI-development/donation disclosures in GitHub documentation, not the Workshop body. Any future image appears only once with a bilingual caption. Updating this repository does not update the live Steam page.
