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

RimWorld 1.6 / 1.7 (unstable) — Beta

## Short description

**Japanese source**

現実の植物に見られる耐寒性の違いを参考に、植物ごとの最低生育温度・枯死温度・低温休眠をより現実的に調整するMod。ほかの植物Modから利用できる軽量XMLフレームワークとしても機能する。

**English translation**

A realism-focused plant cold-tolerance overhaul based on real differences in cold hardiness, with plant-specific growth/death thresholds, dormancy, and a lightweight XML framework for other plant mods.

## Workshop categories / tags

- Category: `Mod`
- Version: `1.6 / 1.7 (unstable)`
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

- VPE support is handled by the separate optional [CCTO for VPE](https://steamcommunity.com/sharedfiles/filedetails/?id=3819486090) add-on, not CCTO base.
- The add-on has source-implemented cold-tolerance XML for the VPE 1.6 basic set (20 plants, including six custom-class plants), but has not been tested in-game. Its Steam item is currently private.
- Optional VPE add-on packs (More Plants, Mushrooms, Succulents, Flowers) are planned for the same compatibility add-on; they are not yet supported. No broader VPE support is claimed for base CCTO.

## Combined public text check

Run `python Tests/validate_workshop_description.py` before publication. It confirms About/Workshop title agreement, one `[hr][/hr]` between languages, Japanese source synchronization, balanced BBCode, required save/dependency disclosures, and a combined UTF-8 budget below 8,000 bytes. Keep license/AI-development/donation disclosures in GitHub documentation, not the Workshop body. Any future image appears only once with a bilingual caption. Updating this repository does not update the live Steam page.


## Future Workshop illustration candidate — cold-survival curves (2026-10-21)

**Idea only / not yet approved art or live Workshop content.** After
species-specific cold-injury models and parameter values have been
empirically calibrated and game behavior verified, publish a
**static, bilingual cold-tolerance comparison chart** as a Workshop
description image. Do not change the current Steam page or create
art from unapproved hypothetical species values.

**Most useful first chart: temperature × sustainable exposure time**

- **Horizontal axis:** actual ambient plant temperature (°C), colder to
  the left.
- **Vertical axis:** simulated *continuous exposure time until modelled
  plant death* (in-game hours/days) at that fixed temperature.
- Show only a few distinct, representative plant curves per image
  (for example cold-sensitive crop, intermediate crop, cold-hardy
  crop). Group plant classes into separate charts rather than crowding
  seventy-eight DefNames into one picture.
- Specify model conditions **in the image**, e.g. growth stage,
  cold acclimation state, assumed constant temperature, whether
  warming/recovery is excluded, and that this is the CCTO gameplay
  simulation rather than a measured biological survival curve.
- If modelled exposure exceeds the graph range or results in no
  cold death, say "over chart range"/"no modelled lethal damage"
  instead of plotting a fabricated finite death time.
- Show a separate visual/annotation for **dormancy-only plants**;
  dormancy temperature is already displayed in the Info Card, and
  dormancy must not be plotted as inevitable lethal injury.
- When a published source provides an LT50, leaf-injury or yield-loss
  result, do not directly plot it as the curve's 100%-death point.
  Cite data and distinguish literature results from model outcomes
  in a companion note or distinct, correctly labeled observation.
- Preserve small-screen readability: thick distinct curves, large
  labels, color-independent line styles, few species. Japanese and
  English labels/captions must be consistent with the current
  bilingual Workshop rule; don't bury scientific caveats in tiny type.

**Fallback if survival times are too sensitive to unstated conditions:**
a fixed-duration chart of modelled cold injury versus temperature,
clearly labeled with its reference duration and stage. Choose after
actual parameter fitting, not before. The purpose is understanding
relative cold tolerance, not making new scientific claims.

This is a future Workshop communication deliverable, **not** a
requirement to add new runtime UI, per-Tick calculations, live graphing
or release a chart before the new cold-damage model is accepted.
