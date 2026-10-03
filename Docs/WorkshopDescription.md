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

[b]Crop Cold Tolerance Overhaul (CCTO)[/b] separates a plant's minimum growth temperature from what actually happens when the weather becomes lethally cold.

In Vanilla RimWorld, the low-temperature leafless/death threshold is derived from the plant's minimum growth temperature with a deterministic per-plant offset. CCTO instead gives supported crops explicit species-level cold behavior.

[b]What CCTO changes[/b]

• Sets crop-specific minimum growth temperatures.
• Gives ordinary supported crops a fixed cold-death temperature.
• Gives selected perennial/overwintering crops cold dormancy instead of ordinary winter death.
• Displays [b]Cold death temperature / 枯死温度[/b] or [b]Dormancy temperature / 休眠温度[/b] in plant information.
• Adds soft compatibility for Nice Plants Menu.
• Works with Dubs Mint Menus through the normal RimWorld Info Card path.

CCTO only rebalances cold-tolerance-related behavior. It does [b]not[/b] change crop yield, growth time, fertility, storage life, processing, or research.

[b]Balance direction[/b]

The current balance is generally more demanding than the original Vanilla / Medieval Overhaul cold settings.

Most supported crops stop growing at warmer temperatures, and ordinary crops usually die at much warmer temperatures than Vanilla's generic derived cold threshold. This makes crop selection, seasonal timing, greenhouses, heating, and cold snaps more meaningful.

This is not a universal nerf. Selected perennial and overwintering crops instead gain cold dormancy and can survive ordinary winter cold.

[b]Supported crop sets[/b]

• Vanilla RimWorld 1.6 crops
• Medieval Overhaul 1.6 crops

Medieval Overhaul is optional. Its balance patches are only applied when MO is present.

[b]Vanilla before / after[/b]

Vanilla's normal cold threshold is:

minGrowthTemperature + deterministic per-plant offset from -18°C to -10°C

[table]
[tr][th]Crop[/th][th]Original min growth[/th][th]CCTO min growth[/th][th]Original cold behavior[/th][th]CCTO cold behavior[/th][/tr]
[tr][td]Rice[/td][td]0°C[/td][td]10°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -1°C[/td][/tr]
[tr][td]Potato[/td][td]0°C[/td][td]5°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -2°C[/td][/tr]
[tr][td]Corn[/td][td]0°C[/td][td]8°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -2°C[/td][/tr]
[tr][td]Strawberry[/td][td]0°C[/td][td]5°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -9°C[/td][/tr]
[tr][td]Haygrass[/td][td]0°C[/td][td]0°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -9°C[/td][/tr]
[tr][td]Cotton[/td][td]0°C[/td][td]10°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -1°C[/td][/tr]
[tr][td]Devilstrand[/td][td]0°C[/td][td]8°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -1°C[/td][/tr]
[tr][td]Healroot[/td][td]0°C[/td][td]0°C[/td][td]leafless below -18 to -10°C, survives[/td][td]fixed death at -9°C[/td][/tr]
[tr][td]Hops[/td][td]0°C[/td][td]5°C[/td][td]death at -18 to -10°C, per plant[/td][td]cold dormancy below 5°C[/td][/tr]
[tr][td]Smokeleaf[/td][td]0°C[/td][td]5°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -4°C[/td][/tr]
[tr][td]Psychoid[/td][td]0°C[/td][td]8°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -1°C[/td][/tr]
[tr][td]Cocoa[/td][td]0°C[/td][td]12°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at 0°C[/td][/tr]
[/table]

[b]Medieval Overhaul before / after[/b]

MO Healroot uses Vanilla [i]Plant_Healroot[/i] and therefore uses the Vanilla row above.

[table]
[tr][th]Crop[/th][th]Original min growth[/th][th]CCTO min growth[/th][th]Original cold behavior[/th][th]CCTO cold behavior[/th][/tr]
[tr][td]Mindwort[/td][td]0°C[/td][td]5°C[/td][td]leafless below -18 to -10°C, survives[/td][td]fixed death at -3°C[/td][/tr]
[tr][td]Poppy[/td][td]0°C[/td][td]5°C[/td][td]leafless below -18 to -10°C, survives[/td][td]fixed death at -5°C[/td][/tr]
[tr][td]Fleawort[/td][td]0°C[/td][td]3°C[/td][td]leafless below -18 to -10°C, survives[/td][td]fixed death at -6°C[/td][/tr]
[tr][td]Fly agaric[/td][td]0°C[/td][td]0°C[/td][td]leafless below -18 to -10°C, survives[/td][td]cold dormancy below 0°C[/td][/tr]
[tr][td]Onion[/td][td]0°C[/td][td]5°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -3°C[/td][/tr]
[tr][td]Lentil[/td][td]0°C[/td][td]5°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -4°C[/td][/tr]
[tr][td]Cabbage[/td][td]-18°C[/td][td]0°C[/td][td]death at -36 to -28°C, per plant[/td][td]fixed death at -6°C[/td][/tr]
[tr][td]Garlic[/td][td]0°C[/td][td]0°C[/td][td]death at -18 to -10°C, per plant[/td][td]cold dormancy below 0°C[/td][/tr]
[tr][td]Mushroom[/td][td]0°C[/td][td]5°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -1°C[/td][/tr]
[tr][td]Wheat[/td][td]0°C[/td][td]0°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -6°C[/td][/tr]
[tr][td]Flax[/td][td]0°C[/td][td]5°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -5°C[/td][/tr]
[tr][td]Sugarcane[/td][td]0°C[/td][td]10°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -5°C[/td][/tr]
[tr][td]Carrot[/td][td]0°C[/td][td]0°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -4°C[/td][/tr]
[tr][td]Herb[/td][td]0°C[/td][td]5°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -3°C[/td][/tr]
[tr][td]Tomato[/td][td]0°C[/td][td]10°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -1°C[/td][/tr]
[tr][td]Pumpkin[/td][td]0°C[/td][td]10°C[/td][td]death at -18 to -10°C, per plant[/td][td]fixed death at -1°C[/td][/tr]
[tr][td]Grape[/td][td]0°C[/td][td]5°C[/td][td]death at -18 to -10°C, per plant[/td][td]cold dormancy below 5°C[/td][/tr]
[tr][td]Apple[/td][td]0°C[/td][td]5°C[/td][td]leafless below -18 to -10°C, survives[/td][td]cold dormancy below 5°C[/td][/tr]
[tr][td]Mulberry[/td][td]0°C[/td][td]5°C[/td][td]leafless below -18 to -10°C, survives[/td][td]cold dormancy below 5°C[/td][/tr]
[tr][td]Griffon berry[/td][td]0°C[/td][td]5°C[/td][td]leafless below -18 to -10°C, survives[/td][td]cold dormancy below 5°C[/td][/tr]
[tr][td]Lemon[/td][td]0°C[/td][td]10°C[/td][td]leafless below -18 to -10°C, survives[/td][td]fixed death at -4°C[/td][/tr]
[/table]

[b]Framework for other mods[/b]

CCTO can also be used as a lightweight XML-facing framework. Other mods can assign their own plants explicit fixed cold-death temperatures or cold dormancy without replacing the plant class.

This framework role is also how the planned [b]Ancient & Medieval Japan (AMJ)[/b] mod will consume CCTO.

[b]Why this is a standalone mod[/b]

CCTO was originally designed while planning AMJ. The cold-tolerance system turned out to be independently useful, so it was split into its own mod rather than being locked inside a larger historical-content project.

[b]Beta feedback wanted[/b]

The first Beta intentionally uses one exact species-level cold-death threshold instead of per-plant variation.

Feedback is especially useful on:

• whether fixed thresholds make seasonal planning clearer;
• whether synchronized whole-field cold death feels too abrupt;
• whether deterministic individual variation would be preferable;
• whether the cold-death and dormancy displays are clear enough.

A deterministic per-plant range mode is preserved as a possible future design, but is not enabled in 0.1 Beta.

[b]Display-only version?[/b]

No separate display-only edition is currently planned.

Vanilla does not contain independently balanced fixed cold-death temperatures for each crop. Its threshold is derived from the growth minimum using a common rule. CCTO's display is intended to report CCTO's actual species-specific behavior, not just expose Vanilla's generic formula.

[b]Dependencies[/b]

Required:
• Harmony

Optional:
• Medieval Overhaul
• Nice Plants Menu
• Dubs Mint Menus

[b]Verification[/b]

The 0.1 Beta code has passed:

• full automated framework + Vanilla/MO integration gate: 13/13;
• loaded Def verification for all supported Vanilla and MO targets;
• normal-game Info Card smoke checks;
• Dubs Mint Menus display verification;
• Nice Plants Menu compatibility smoke checks.

[b]AI-assisted development[/b]

AI was used primarily for code implementation, documentation, research, and test development. Design decisions, balance decisions, and final testing/review remain the author's responsibility.

[b]License[/b]

MIT License.

[b]Version[/b]

0.1 Beta
