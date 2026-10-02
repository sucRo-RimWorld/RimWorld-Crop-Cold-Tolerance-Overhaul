# CCTO Framework API

Crop Cold Tolerance Overhaul can be used as a lightweight XML-facing framework.

## What CCTO adds

RimWorld 1.6 already provides `PlantProperties.minGrowthTemperature`. CCTO leaves that native field in place and adds a separate fixed cold-death threshold through a `DefModExtension`.

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
