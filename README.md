# Crop Cold Tolerance Overhaul

**RimWorld 1.6 — Alpha**

Crop Cold Tolerance Overhaul (CCTO) separates a crop's **minimum growth temperature** from its **cold-death behavior**.

Vanilla RimWorld derives cold death from the growth minimum with deterministic per-plant variation. CCTO instead gives supported crops a clear species-level cold response:

- ordinary crops use one fixed cold-death temperature;
- selected perennial/overwintering crops enter cold dormancy below their minimum growth temperature;
- the Info Card displays the relevant `枯死温度 / Cold death temperature` or `休眠温度 / Dormancy temperature` explicitly.

## Scope

CCTO changes only cold-tolerance behavior.

It does **not** rebalance:

- harvest yield;
- growth time;
- fertility;
- storage life;
- processing;
- research.

## Supported crop sets

The current Alpha includes balance data for:

- Vanilla RimWorld crops;
- Medieval Overhaul 1.6 crops.

Ancient & Medieval Japan (AMJ) uses CCTO's framework/API from its own PlantDefs rather than making CCTO depend on AMJ.

## Dependencies

Required:

- Harmony.

Optional:

- Medieval Overhaul — CCTO applies its MO crop balance only when MO is present;
- Nice Plants Menu — CCTO adds its cold-death/dormancy rows to the custom plant summary when the mod is present;
- Dubs Mint Menus — no special patch is needed; its mouse-over standard Info Card already shows CCTO entries.

## Framework use

Other mods can use CCTO without replacing the plant class.

Example fixed cold-death threshold:

```xml
<li Class="CropColdToleranceOverhaul.ColdToleranceExtension">
  <coldDeathTemperature>-10</coldDeathTemperature>
</li>
```

Example cold dormancy:

```xml
<li Class="CropColdToleranceOverhaul.ColdToleranceExtension">
  <coldDormancy>true</coldDormancy>
</li>
```

Dormancy begins below the plant's native `minGrowthTemperature`.

See [Docs/Framework.md](Docs/Framework.md) for the XML-facing API and [Docs/Design.md](Docs/Design.md) for the balance rationale.

## Alpha feedback

The initial Alpha intentionally uses one exact species-level cold-death threshold instead of Vanilla-like per-plant variation.

Useful feedback includes:

- whether the fixed threshold makes seasonal planning clearer;
- whether whole-field synchronized cold death feels too abrupt;
- whether deterministic individual variation would add useful gameplay;
- whether the Info Card and plant-menu temperature information is sufficiently clear.

A possible future deterministic per-plant range mode is documented, but is **not** part of the current Alpha.

## Verification

The current integrated build has passed:

- the full automated framework + Vanilla/MO integration gate: **13/13**;
- normal-game Info Card smoke checks;
- Dubs Mint Menus display verification;
- Nice Plants Menu compatibility smoke checks.

The tested release source of truth is `main`.

## Status

Current target: **0.1 Alpha**.

This is an Alpha release intended for real play and feedback, not a Beta declaration.
