# Steam Workshop Presentation — CCTO

`README.md` is the detailed public-content source of truth. The Workshop description is intentionally shorter: it should surface only the information needed to understand, evaluate, and install the mod, then link to GitHub for full values and technical detail. Paste-ready BBCode is stored in `Docs/SteamWorkshopDescription.txt`.

## Title

Crop Cold Tolerance Overhaul

## Release stage

RimWorld 1.6 — Beta

## Japanese localization

- Title: `作物耐寒性オーバーホール`
- Paste-ready title: `Docs/SteamWorkshopTitle-ja.txt`
- Paste-ready description: `Docs/SteamWorkshopDescription-ja.txt`

## Short description

A realism-focused plant cold-tolerance overhaul based on real-world cold-hardiness information, with explicit cold-death/dormancy behavior and a lightweight XML framework for other plant mods.

## Workshop categories / tags

- Category: `Mod`
- Version: `1.6`
- Required DLC: none

## Long description

[h1]Crop Cold Tolerance Overhaul (CCTO)[/h1]

CCTO is a [b]realism-focused overhaul of plant cold tolerance[/b]. It uses real-world information on frost injury, chilling sensitivity, overwintering behavior, and lethal temperatures as reference points, then converts those differences into clear RimWorld gameplay thresholds.

Instead of relying mainly on Vanilla's generic relationship between minimum growth temperature and cold death, supported plants receive plant-specific minimum growth temperatures plus either a fixed cold-death temperature or cold dormancy.

[h2]Key features[/h2]
[list]
[*]Real-world cold tolerance used as the basis for balance
[*]Plant-specific minimum growth temperatures
[*]Explicit fixed cold-death temperatures
[*]Cold dormancy for selected perennial and overwintering plants
[*]Cold death / dormancy temperature shown in plant information
[*]Cold-tolerance changes only — yield, growth time, fertility, storage, processing, and research are not rebalanced
[/list]

The result is generally more demanding than Vanilla / Medieval Overhaul: crop choice, sowing season, forestry, heating, greenhouses, and cold snaps matter more. It is not a blanket nerf; plants suited to overwintering can instead survive through dormancy.

[h2]Supported content[/h2]
[list]
[*]All 49 living Core PlantDefs in RimWorld 1.6
[*]All 29 Medieval Overhaul 1.6-specific PlantDefs when MO is installed
[*]Wild vegetation is included; dead stump/remnant Defs are excluded
[*]English and Japanese
[/list]

[url=https://github.com/sucRo-RimWorld/RimWorld-Crop-Cold-Tolerance-Overhaul#current-rebalance-values][b]Full before/after values and balance rationale on GitHub[/b][/url]

[h2]Framework for other mods[/h2]

CCTO is also a [b]lightweight XML-facing framework[/b]. Other mods can assign explicit fixed cold-death temperatures or cold dormancy to their own plants without replacing the plant class.

[h2]Dependencies[/h2]
[b]Required:[/b] Harmony

[b]Optional:[/b] Medieval Overhaul, Nice Plants Menu, Dubs Mint Menus

[h2]Save compatibility[/h2]
[b]Safe to add to or remove from an existing save.[/b]

Temperature rules apply to existing plants after loading. Plants already killed by cold are not restored by removing CCTO. Mods that require CCTO must remain installed with their dependency.

[h2]Development[/h2]

Beta. AI assistance is used for implementation, documentation, research, and test development; design, balance decisions, and final review remain the author's responsibility.

MIT License.

[h2]Support[/h2]
[url=https://ko-fi.com/sucro0629][img]https://img.shields.io/badge/Ko--fi-Support%20me-ff5e5b?logo=ko-fi&logoColor=white[/img][/url]
