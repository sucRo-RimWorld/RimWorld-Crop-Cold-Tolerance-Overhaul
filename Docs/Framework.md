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


## Cumulative cold damage: optional XML API

Existing fixed `coldDeathTemperature` and `coldDormancy` extensions
continue working unchanged. A plant can explicitly opt into degree-hour
damage instead of instantaneous death:

```xml
<li Class="CropColdToleranceOverhaul.ColdToleranceExtension">
  <coldDamageStartTemperature>-5</coldDamageStartTemperature>
  <coldDamageDegreeHoursToDie>48</coldDamageDegreeHoursToDie>
  <coldDamageRecoveryPerHour>0.02</coldDamageRecoveryPerHour>
</li>
```

**These numbers are XML examples only; no plant has these values approved.**
Both onset and lethal dose must be supplied. The dose unit is **°C ×
in-game hours** below onset. `coldDamageRecoveryPerHour` is the fraction
of lethal injury cleared per game-hour only when ambient temperature is **at least max(0°C, injury onset)** (optional, default zero);
injury is capped to [0,1]. At 1 the plant dies. Calculations run on the
existing plant leafless check, not every tick, and cap elapsed time to
2,000 ticks per evaluation. They sample current ambient temperature and
cannot perfectly reconstruct short intervening extremes.

When these fields are valid, they take precedence over any legacy
`coldDeathTemperature` and the information card instead displays
**Cold damage starts / 損傷開始温度**. Existing dormancy still starts at
`minGrowthTemperature` and its **Dormancy temperature / 休眠温度**
label remains unchanged. Onset describes the beginning of accumulated
injury, not a guaranteed lethal temperature.

Only plants with an observed below-onset reading or remaining cold
damage consume per-plant saved state (map component). The **first cold
reading initializes a zero-damage observation** at the current game tick;
it does not retroactively charge the preceding 2,000 ticks at a temperature
that was not recorded. A second cold reading accumulates elapsed exposure
capped to 2,000 ticks. The zero-damage observation is also **saved and
reloaded**, preventing save/load from restarting that interval. Warm
observations at zero damage remove the record; recovered/dead plants are
removed, and stale references are pruned. This cannot reconstruct a
brief cold spike between two sampled readings. Changing XML between
old and new models, old saves, third-party subclasses and mod removal
still need actual runtime checks.
No Vanilla/MO/VPE/AMJ plant is opted in by this framework change.


### Stable per-plant lethal-dose variation (experimental, opt-in)

An optional field `coldDamageLethalDoseVariation` controls the **fractional
half-width** of the lethal-dose multiplier. Its default is **0**, meaning
no individual variation and **no change to any existing plant**.

```xml
<!-- Syntax example, NOT a crop balance recommendation: -->
<coldDamageLethalDoseVariation>0.2</coldDamageLethalDoseVariation>
```

For a nonzero half-width `w`, a deterministic rank `u` is derived by
mixing the plant's **persistent `thingIDNumber`**, and the effective
lethal accumulated dose is `baseDose * (1 + w * (2*u-1))`.
The 0.2 example produces a **bounded uniform simulation distribution**
between roughly 0.8× and 1.2× the base lethal degree-hours, with median
1.0×; the distribution family and 0.2 figure are **not fitted to
population LT10/LT50/LT90 data and not approved for any species**.
Accepted values require separate ecological calibration.

**Invalid recovery-rate values are a disabled-model condition, not only
a Def configuration warning.** The opt-in model requires
`coldDamageRecoveryPerHour` to be finite and nonnegative
(default zero). Negative, NaN, and infinite recovery rates now
fail both ConfigErrors and HasColdDamageModel; this prevents
a negative recovery rate from causing injury to increase
under warm temperatures. If the model is disabled, only
a separately valid legacy fixed-death/dormancy configuration
can provide the CCTO-managed behavior; invalid XML is not
a supported gameplay profile.

The field is valid only for a real finite number **0 ≤ w < 1**, and
must accompany a valid opt-in cold-damage model to have any effect.
No extra random draws occur during updates; no extra plant field,
additional map scan, or new save record is introduced. Existing
sparse positive-damage save entries remain the same format, with
damage always normalized to [0,1]. Changing XML variation or lethal-dose
values **mid-save** alters the meaning of existing accumulated injury:
this is not automatically re-scaled, so avoid treating it as a
lossless save migration. Verify identity stability across save/load
and re-spawning during the consolidated runtime acceptance stage.

A uniform threshold is a **reversible computational baseline** rather
than a validated universal survival probability law. More complex
skewed curves, chilling-vs-freezing severity, and physiological
acclimation are not added by this field. The Info Card continues to
show the **species-level 損傷開始温度** and existing **休眠温度**,
not a pseudo-exact death temperature for each plant.

### Injury recovery versus cold acclimation (2026-10-22)

The opt-in engine now has three states determined entirely by the current
ambient temperature `T` and plant-specific injury onset `A`:

- `T < A`: **accumulate cold injury**.
- `A ≤ T < max(A, 0°C)`: **preserve existing injury**; the plant does not
  take further injury but also does **not** repair frozen tissue merely
  because it is a little warmer than its injury onset.
- `T ≥ max(A, 0°C)`: **recover injury** at
  `coldDamageRecoveryPerHour`, if nonzero.

For `A ≥ 0°C` (a hypothetical rice chilling profile, for example),
the middle range is empty and recovery starts at onset. For `A < 0°C`
(a frost-hardy pine, for example), **−30°C no longer heals damage
when onset is −31°C**; +4°C can heal. This is a deliberately minimal,
species-agnostic **ambient thaw proxy**, not a measured tissue
temperature or repair-rate study. It changes no XML schema, exposes
no new UI control, and adds no persistent state or per-tick scan.
It applies **only to opted-in cumulative-damage plants**, not
existing 48 fixed-death or 30 dormancy definitions.

**Cold acclimation remains a distinct, unimplemented process.**
A frozen-but-no-longer-injuring plant does not thereby become hardened.
No `coldDormancy`, `minGrowthTemperature`, season, per-plant
lethal-dose rank or incidental freezing event grants prior
experimental cool-exposure history. Rice and wheat remain
**unmigrated**, because identical ambient trajectories following
different conditioning states have been measured to yield
different whole-plant survival. Future species-scoped hardening
will require its own saved temperature-history response and
source- or author-approved rates; this recovery change does not
justify an automatic hardening implementation.

Biological context: [freeze–thaw repair in Arabidopsis](https://pmc.ncbi.nlm.nih.gov/articles/PMC6971927/)
was observed after thawing and subsequent days at +4°C;
the [FROSTOL winter-wheat model](https://doi.org/10.1016/j.eja.2007.10.002)
separately handles hardening/dehardening and low-temperature stress,
using crown/soil temperature, not a universal ambient recovery threshold.
The **0°C switch is a gameplay convention**, not their numeric result.
No real-game testing of the changed branch has yet occurred.

### Calibration status and future compatibility

The optional XML fields above are an **unvalidated experimental draft
API in PR #24**, not a final calibrated model. The current implementation
assumes linear degree-hours; future evidence may justify a bounded set
of response profiles with per-plant overrides. Consumers must not
interpret example onset, lethal dose or recovery fields as biology-backed
values, and must not migrate their XML from the fixed model before
calibration and API stabilization. Existing legacy extension semantics
remain unchanged.


### Time unit and acclimation non-feature (2026-10-21)

The opt-in API's coldDamageDegreeHoursToDie
means **degrees × in-game hours**. The runtime
uses **2,500 ticks per in-game hour**; no
365/60 or 60/365 season-length multiplier
is applied to acute cold damage.
Treating a source's 24 laboratory hours
as 24 uninterrupted game-hours is a
**comparison assumption**, not evidence
of a source-matched fatal dose.

The engine reads **AmbientTemperature**
at CheckMakeLeafless samples, not soil/crown
temperature; it does not replay the actual
temperature trajectory between samples.
The initial sub-onset observation is not
backfilled, and elapsed intervals are
capped at 2,000 ticks. Consequently
continuous laboratory exposure cannot
be assumed to equal exactly accumulated
gameplay exposure without a scoped runtime
check.

**Cold acclimation is not implemented or
implicitly granted.** Setting coldDormancy
or minGrowthTemperature does **not** track
hardening; being in winter or at a
high latitude does not establish a
previous 4°C×7-day conditioning state.
A future optional per-plant hardening
feature must represent prior cool
exposure and warm dehardening separately
from damage/dormancy, with saved
history and independently justified
species parameters. The W13 7-day
conditioning duration cannot be taken
as universal 7 in-game days or
automatically compressed by the
60-day game year.

**Consumer warning:** do not copy the
published winter-wheat acclimated
one-state fit examples into generic
Medieval Overhaul wheat XML. They
have no approved default-state
mapping or short/long-freeze
behavior. For exact design constraints
see [time and acclimation boundary](Design.md#time-and-acclimation-implementation-boundary-2026-10-21).
Existing 48 production fixed-death
thresholds and 30 dormancy profiles
remain unchanged.


### Save/load ordering: PostLoadInit versus plant spawning (2026-10-22)

The opt-in per-map `cctoColdDamageStates` records are
serialized with a deep `ColdDamageRecord` and a
`Scribe_References` **Plant** reference; their
`damage` (0..1) and `lastTick` are saved. On
`PostLoadInit`, the resolved referenced `Plant`
**can still report `Spawned=false` even when it
will become an ordinary spawned plant moments
later**. Therefore `PostLoadInit` must not
discard records solely on that flag. Invalid
or deleted references, disabled opted-in Defs,
NaN/negative/fatal damage and ultimately stale
unspawned plants retain their respective
existing validation/pruning paths; when the
map is live, `Prune()` still excludes
unspawned/destroyed/unsupported plants.
This preserves legitimate in-progress frost
dose across real Scribe save/load without
rewinding or backdating the observation clock.

**Direct native evidence:** in [original 1.6
two-process save/reload Actions #39709845297](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/39709845297),
a real .rws restored `Plant_Potato2204`,
`damage=0.20` and `lastTick=30` unchanged
at Scribe `PostLoadInit`, with later actual
temperature recovery and subsequent manual
bounded cold observation supported. The
source correction is
`4061fc57331fc0153558abd550c0675e687a1135`.
This confirms **1.6 test-only opt-in save
compatibility**, not biological calibration,
real 1.7, preexisting player saves or normal
tick scheduling. No released Vanilla/MO
production Def has opted into accumulated
injury; default gameplay remains legacy.


### V1 candidate withdrawn after independent audit (2026-10-22)

The **rejected V1 gameplay candidate** for CCTO's
existing 48 fixed-death plant Defs is archived in
[ColdDamageBalanceV1.csv](ColdDamageBalanceV1.csv).
See [the owning design and explicit tradeoffs](ColdDamageBalanceV1.md).
The other 30 Defs retain existing nonlethal dormancy.
The prior **V1 values DECIDED/LOCKED** designation is withdrawn:
formula-valid reference-hour arithmetic did not independently
verify crop physiology, variable weather or warm recovery.
[Failure audit](ColdDamageBalanceFailureAudit.md) and
[review state](ColdDamageBalanceReview.json) prohibit XML activation.
These parameters are not approved gameplay settings or
independently measured biological coefficients.

**API consumers:** the earlier illustrative XML
snippet in this document is still **not a
production value**. CCTO-owned V1 values are **failed proposal rows**, neither future-approved XML targets nor live settings. Do not
copy the CSV to custom crops outside CCTO
(AMJC/AMJE/VPE retain value ownership), and
do not assume the current Steam DLL has
enabled any cumulative injury profile.
The engine remains opt-in pending integration,
compatibility review and the final scoped
real-map ecology acceptance.
