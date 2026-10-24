# Cumulative cold damage — prior-art comparison (2026-10-21)

**Requested gap:** short cold spikes currently cause instant death. CCTO must
represent cold severity multiplied by exposure time, without AMJ-only plant
logic or costly full-map scanning. Scope: RimWorld 1.6/1.7 CCTO generic API.

Search coverage: English/Japanese combinations of RimWorld crop cold
damage, frost injury, cold duration, 冷害, 低温損傷, crop dormancy,
Vanilla Plants Expanded winter, Night Crops, Farming Expansion.

- **Vanilla/MO:** Vanilla Plant has a species-independent random threshold
  for cold leaflessness/death and no documented accumulation. MO is already
  covered by CCTO's fixed thresholds; adding a second MO plant controller
  is unnecessary. Source: https://rimworldwiki.com/wiki/Crop and CCTO
  `Docs/Design.md`, `Source/CropColdToleranceOverhaul/Patches/PlantColdTolerancePatches.cs`.
- **AMJ:** Grains/Environment require temperature-dependent crop choices,
  but CCTO remains the owner of the generic plant mortality mechanics.
  No AMJ-owned cold-injury engine is available. The owning Mod retains
  its own plant thresholds and compatibility XML.
- **VE:** Vanilla Plants Expanded has winter-related per-plant behavior
  (for example beet) but no documented generic elapsed frost injury API.
  This is a prior-art comparison, not an invitation to import VE content.
  https://rimworld-ve.fandom.com/wiki/Plants
- **Non-VE Night Crops 1.0.3 (2026-02-14):** toggleable dormancy with
  configurable *fixed* lethal temperature, and unrelated nighttime growth,
  sun-lamp and hay mechanics. It does not advertise dose accumulation.
  https://www.nexusmods.com/rimworld/mods/621?tab=description
- **Non-VE Farming Expansion:** pesticides, weeds and blight protection,
  not elapsed cold injury.
  https://rimworldbase.com/farming-expansion-mod/

**Decision:** implement independently *within existing CCTO*, rather than
adopting a content pack or adding an unrelated required Mod. New API is
opt-in and preserves existing plant values. The specific unmet feature is
cumulative temperature/time damage. Sources are public docs/workshop
descriptions and available CCTO code, not exhaustive verification of
undocumented third-party assembly internals. VPE runtime coexistence,
dose parameters and native performance remain unverified.
