# CCTO Framework API

Crop Cold Tolerance Overhaul can be used as a lightweight XML-facing framework for setting explicit cold-death temperatures and cold dormancy behavior on plants added by other mods.

## What CCTO adds

RimWorld 1.6 already provides `PlantProperties.minGrowthTemperature`. CCTO leaves that native field in place and adds a separate fixed cold-death threshold through a `DefModExtension`. Other mods can therefore use CCTO to define plant-specific cold-death temperatures instead of relying on RimWorld's derived Vanilla cold-death rule.

CCTO does not replace plant `thingClass`.

## Add CCTO cold tolerance to a plant

```xml
<Operation Class="PatchOperationAddModExtension">
  <xpath>/Defs/ThingDef[defName="YourPlant"]</xpath>
  <value>
    <li Class="CropColdToleranceOverhaul.ColdToleranceExtension">
      <coldDeathTemperature>-10</coldDeathTemperature>
    </li>
  </value>
</Operation>
```

The value above is only an XML syntax example, not a CCTO balance recommendation.

## Enable cold dormancy

Cold dormancy starts below the plant's existing `minGrowthTemperature`.

```xml
<li Class="CropColdToleranceOverhaul.ColdToleranceExtension">
  <coldDormancy>true</coldDormancy>
</li>
```

A dormant plant stops being harvestable while dormant and resumes normal behavior when temperature recovers.

A dormant plant may also define `coldDeathTemperature`. In that case, ordinary cold causes dormancy while more extreme cold below the fixed death threshold kills the plant.

## Override a CCTO value

If CCTO already supplies an extension for the plant, replace its value rather than appending a second extension.

```xml
<Operation Class="PatchOperationReplace">
  <xpath>/Defs/ThingDef[defName="YourPlant"]/modExtensions/li[@Class="CropColdToleranceOverhaul.ColdToleranceExtension"]/coldDeathTemperature</xpath>
  <value>
    <coldDeathTemperature>-15</coldDeathTemperature>
  </value>
</Operation>
```

Again, the value is only a syntax example.

## Growth minimum

Use RimWorld's native field:

```xml
<Operation Class="PatchOperationReplace">
  <xpath>/Defs/ThingDef[defName="YourPlant"]/plant/minGrowthTemperature</xpath>
  <value>
    <minGrowthTemperature>0</minGrowthTemperature>
  </value>
</Operation>
```

CCTO does not introduce a second growth-minimum field.


## Data ownership for framework consumers

A mod consuming the framework owns its plants' temperature values, design/balance tables, Def mapping, compatibility XML, and validation. AMJC and AMJE use this pattern; their custom plant data is not shipped or maintained in CCTO.

For an optional CCTO dependency, add the extension only while CCTO is active (for example, through a `PatchOperationFindMod` guard). Keep the native minimum growth temperature in the owning PlantDef so that the plant remains usable without CCTO. CCTO-owned Vanilla/MO data remains in CCTO; consuming the framework does not transfer ownership of those existing sets.


## Optional HP-only cold-damage API (V3 draft)

The existing fixed `coldDeathTemperature` and `coldDormancy` remain unchanged. For plants explicitly opted into the V3 model, the draft API is:

```xml
<li Class="CropColdToleranceOverhaul.ColdToleranceExtension">
  <coldDamageStartTemperature>-5</coldDamageStartTemperature>
  <coldDamageDegreeHoursToDie>48</coldDamageDegreeHoursToDie>
</li>
```

These are **syntax examples, not approved plant values**. Below onset, exposure degree-hours subtract a fraction of the plant's **maximum HP**, through the ordinary damage system. The degree-hour dose is the amount needed to remove the full HP of an otherwise undamaged median plant. Warming stops new cold HP loss; CCTO does **not** heal HP or model acclimation. Other forms of damage share the same HP. Optional `coldDamageLethalDoseVariation` supplies deterministic variation in required dose (default zero; uncalibrated); no separate injury gauge is maintained. The UI reports **損傷開始温度 / Cold damage starts** and existing **休眠温度 / Dormancy temperature**.

Onset and lethal dose are specified together. The existing leafless check provides temperature samples; a sample interval is capped at 2,000 game ticks. The first cold reading starts the observation without inferring earlier temperatures. A sparse saved observation record retains time, whether the preceding observation was cold, and damage fractions below one HP, not accumulated cold injury. A cold reading immediately after a warm one restarts cold timing without charging the preceding warm interval. This cannot capture temperature spikes between readings. Save compatibility and interactions with other mods require actual game testing.

The former `coldDamageRecoveryPerHour` option has been **removed from the V3 draft code**. Existing experimental V2 injury-save data has no supported conversion path; no such experimental model has shipped. No Vanilla or Medieval Overhaul plant is opted into the HP model in production yet. Previous V2 candidate figures under the hidden-injury/recovery model are no longer acceptance evidence.

