# Steam Workshop Presentation — CCTO 0.1 Beta

`README.md` is the public-content source of truth. Keep this Workshop presentation synchronized with it. The paste-ready BBCode body is also stored in `Docs/SteamWorkshopDescription.txt`.

## Title

Crop Cold Tolerance Overhaul

## Version line

RimWorld 1.6 — 0.1 Beta

## Short description

Separates minimum growth temperature from cold-death behavior, rebalances Vanilla and Medieval Overhaul crop cold tolerance, adds explicit cold-death/dormancy temperatures to plant information, and provides a lightweight XML framework for other crop mods.

## Workshop categories / tags

Use the actual RimWorld Workshop categories rather than inventing free-form gameplay tags:

- Category: `Mod`
- Version: `1.6`
- Required DLC: none

## Long description

[h1]Crop Cold Tolerance Overhaul (CCTO)[/h1]

CCTO separates a plant's [b]minimum growth temperature[/b] from its [b]cold-death behavior[/b], and adds non-lethal [b]cold dormancy[/b] for selected plants: growth stops and the plant becomes leafless in the cold instead of dying, then recovers after temperatures rise.

Vanilla RimWorld derives cold leafless/death from minimum growth temperature with a deterministic per-plant offset. CCTO instead gives supported crops explicit species-level behavior: fixed cold-death temperatures or cold dormancy.

[h2]What CCTO changes[/h2]
[list]
[*]Crop-specific minimum growth temperatures
[*]Fixed cold-death temperatures for ordinary supported crops
[*]Cold dormancy for selected perennial/overwintering crops
[*]Cold death / dormancy temperature shown in plant information
[*]Soft compatibility with Nice Plants Menu
[*]Dubs Mint Menus support through the normal Info Card path
[/list]

CCTO only changes cold-tolerance-related behavior. It does [b]not[/b] rebalance yield, growth time, fertility, storage life, processing, or research.

[h2]Balance direction[/h2]

The current balance is generally [b]more demanding[/b] than the original Vanilla / Medieval Overhaul settings. Most supported crops stop growing at warmer temperatures, and many ordinary crops die at much warmer temperatures than Vanilla's generic cold threshold.

This makes crop choice, seasonal timing, heating, greenhouses, and cold snaps more important.

This is not a universal nerf: selected perennial and overwintering crops gain dormancy and can survive ordinary winter cold.

[h2]How the values were chosen[/h2]

The numbers are based on real-world crop cold-tolerance information, including frost-damage and lethal-temperature data where available.

Because real plants vary by cultivar, growth stage, acclimation, and exposure duration, CCTO uses those temperatures as reference points rather than copying a single reported value literally. They are rounded and tuned into clear gameplay thresholds while preserving meaningful differences between crops.

[h2]Supported crop sets[/h2]
[list]
[*]Vanilla RimWorld 1.6
[*]Medieval Overhaul 1.6
[/list]

Medieval Overhaul is optional. Its patches are applied only when MO is present.

[h2]Representative before / after examples[/h2]

Vanilla's ordinary cold threshold is normally derived per plant from minimum growth temperature with an offset of about -18°C to -10°C.

[b]Rice[/b]
Minimum growth: 0°C → 10°C
Cold: death at -18°C to -10°C per plant → [b]fixed death at -1°C[/b]

[b]Potato[/b]
Minimum growth: 0°C → 5°C
Cold: death at -18°C to -10°C per plant → [b]fixed death at -2°C[/b]

[b]Healroot[/b]
Minimum growth: 0°C → 0°C
Cold: leafless but survives → [b]fixed death at -9°C[/b]

[b]Hops[/b]
Minimum growth: 0°C → 5°C
Cold: death at -18°C to -10°C per plant → [b]cold dormancy below 5°C[/b]

[b]Cocoa[/b]
Minimum growth: 0°C → 12°C
Cold: death at -18°C to -10°C per plant → [b]fixed death at 0°C[/b]

[b]Medieval Overhaul — Cabbage[/b]
Minimum growth: -18°C → 0°C
Cold: death at -36°C to -28°C per plant → [b]fixed death at -6°C[/b]

[b]Medieval Overhaul — Apple[/b]
Minimum growth: 0°C → 5°C
Cold: leafless but survives → [b]cold dormancy below 5°C[/b]

[b]Medieval Overhaul — Lemon[/b]
Minimum growth: 0°C → 10°C
Cold: leafless but survives → [b]fixed death at -4°C[/b]

[url=https://github.com/sucRo0629/RimWorld-Crop-Cold-Tolerance-Overhaul#current-rebalance-values][b]Full Vanilla + Medieval Overhaul before/after values on GitHub[/b][/url]

[h2]Framework for other mods[/h2]

CCTO also acts as a lightweight XML-facing framework. Other mods can assign their plants explicit fixed cold-death temperatures or cold dormancy without replacing the plant class.

The planned [b]Ancient & Medieval Japan (AMJ)[/b] mod will use CCTO in this way.

[h2]Why standalone?[/h2]

CCTO began as part of AMJ planning. The cold-tolerance system was broadly useful enough to split into its own mod so other crop and overhaul mods can use it independently.

[h2]Beta feedback wanted[/h2]

0.1 Beta intentionally uses one exact species-level cold-death threshold instead of Vanilla-like per-plant variation.

Feedback is especially useful on:
[list]
[*]whether fixed thresholds make seasonal planning clearer
[*]whether synchronized whole-field cold death feels too abrupt
[*]whether deterministic individual variation would be preferable
[*]whether the cold-death and dormancy displays are clear
[/list]

A deterministic per-plant range mode remains a possible future option.

[h2]Display-only version?[/h2]

No separate display-only edition is currently planned. Vanilla does not contain independently balanced fixed cold-death temperatures for each crop; CCTO's display reports the species-specific behavior introduced by CCTO itself.

[h2]Dependencies[/h2]

[b]Required[/b]
[list]
[*]Harmony
[/list]

[b]Optional[/b]
[list]
[*]Medieval Overhaul
[*]Nice Plants Menu
[*]Dubs Mint Menus
[/list]

[h2]Verification[/h2]
[list]
[*]Automated framework + Vanilla/MO integration gate: 13/13
[*]Loaded Def verification for all supported Vanilla/MO targets
[*]Normal-game Info Card smoke checks
[*]Dubs Mint Menus display verification
[*]Nice Plants Menu compatibility smoke checks
[/list]

[h2]AI-assisted development[/h2]

AI was used primarily for code implementation, documentation, research, and test development. Design decisions, balance decisions, and final testing/review remain the author's responsibility.

[h2]License[/h2]
MIT License.

[h2]Version[/h2]
0.1 Beta

[h2]Support[/h2]
[url=https://ko-fi.com/sucro0629][img]https://img.shields.io/badge/Ko--fi-Support%20me-ff5e5b?logo=ko-fi&logoColor=white[/img][/url]
