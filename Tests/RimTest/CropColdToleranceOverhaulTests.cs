using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CropColdToleranceOverhaul;
using RimTestRedux;
using RimWorld;
using Verse;

namespace CropColdToleranceOverhaul.Tests
{
    [TestSuite]
    public static class ColdToleranceExtensionTests
    {
        [Test]
        public static void DefaultConfigurationRequiresDeathTemperature()
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            List<string> errors = extension.ConfigErrors().ToList();

            Assert.That(extension.HasColdDeathTemperature).Is.False();
            Assert.ThatCollection(errors).Has.Count(1);
        }

        [Test]
        public static void DormancyOnlyConfigurationIsValid()
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDormancy = true;

            List<string> errors = extension.ConfigErrors().ToList();

            Assert.That(extension.HasColdDeathTemperature).Is.False();
            Assert.ThatCollection(errors).Is.Empty();
        }

        [Test]
        public static void FiniteDeathTemperatureIsValid()
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDeathTemperature = -12f;

            List<string> errors = extension.ConfigErrors().ToList();

            Assert.That(extension.HasColdDeathTemperature).Is.True();
            Assert.ThatCollection(errors).Is.Empty();
        }

        [Test]
        public static void OptInDamageNeedsStartAndLethalDose()
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDamageStartTemperature = -5f;
            Assert.That(extension.HasColdDamageModel).Is.False();

            extension.coldDamageDegreeHoursToDie = 48f;
            Assert.That(extension.HasColdDamageModel).Is.True();
            Assert.ThatCollection(extension.ConfigErrors().ToList()).Is.Empty();
        }

        [Test]
        public static void LethalDoseVariationIsOptInAndMustBeBounded()
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDamageStartTemperature = -5f;
            extension.coldDamageDegreeHoursToDie = 48f;
            // Keep a legal legacy fallback to isolate the variation validation.
            extension.coldDeathTemperature = -15f;

            Assert.That(extension.coldDamageLethalDoseVariation).Is.EqualTo(0f);
            Assert.That(extension.HasColdDamageModel).Is.True();

            extension.coldDamageLethalDoseVariation = 0.25f;
            Assert.That(extension.HasColdDamageModel).Is.True();
            Assert.ThatCollection(extension.ConfigErrors().ToList()).Is.Empty();

            extension.coldDamageLethalDoseVariation = 1f;
            Assert.That(extension.HasColdDamageModel).Is.False();
            Assert.ThatCollection(extension.ConfigErrors().ToList()).Has.Count(1);

            extension.coldDamageLethalDoseVariation = float.NaN;
            Assert.That(extension.HasColdDamageModel).Is.False();
            Assert.ThatCollection(extension.ConfigErrors().ToList()).Has.Count(1);

            // An orphan variation setting must not silently be ignored.
            extension.coldDamageLethalDoseVariation = 0.25f;
            extension.coldDamageStartTemperature = float.NaN;
            extension.coldDamageDegreeHoursToDie = float.NaN;
            Assert.ThatCollection(extension.ConfigErrors().ToList()).Has.Count(1);
        }

        [Test]
        public static void InfiniteDeathTemperatureIsRejected()
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDormancy = true;
            extension.coldDeathTemperature = float.PositiveInfinity;

            List<string> errors = extension.ConfigErrors().ToList();

            Assert.ThatCollection(errors).Has.Count(1);
        }
    }

    [TestSuite]
    public static class ColdThresholdHarmonyTests
    {
        private static readonly PropertyInfo LeaflessTemperatureThresholdProperty =
            typeof(Plant).GetProperty(
                "LeaflessTemperatureThresh",
                BindingFlags.Instance | BindingFlags.NonPublic);

        [Test]
        public static void FixedDeathTemperatureOverridesVanillaRandomThreshold()
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDeathTemperature = -12f;

            float thresholdA = ReadLeaflessTemperatureThreshold(
                CreatePlant(8f, extension, 101));
            float thresholdB = ReadLeaflessTemperatureThreshold(
                CreatePlant(8f, extension, 987654));

            Assert.That(thresholdA).Is.EqualTo(-12f);
            Assert.That(thresholdB).Is.EqualTo(-12f);
        }

        [Test]
        public static void DormancyUsesMinimumGrowthTemperatureAsLeaflessThreshold()
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDormancy = true;
            extension.coldDeathTemperature = -25f;

            float threshold = ReadLeaflessTemperatureThreshold(
                CreatePlant(3f, extension, 202));

            Assert.That(threshold).Is.EqualTo(3f);
        }

        [Test]
        public static void UnconfiguredPlantKeepsVanillaThresholdRange()
        {
            const float minimumGrowthTemperature = 10f;

            float threshold = ReadLeaflessTemperatureThreshold(
                CreatePlant(minimumGrowthTemperature, null, 303));

            Assert.That(threshold).Is.BetweenInclusive(
                minimumGrowthTemperature - 18f,
                minimumGrowthTemperature - 10f);
        }

        private static Plant CreatePlant(
            float minimumGrowthTemperature,
            ColdToleranceExtension extension,
            int thingId)
        {
            ThingDef def = new ThingDef();
            def.defName = "CCTO_TestPlant_" + thingId;
            def.label = "CCTO test plant";
            def.category = ThingCategory.Plant;
            def.plant = new PlantProperties();
            def.plant.minGrowthTemperature = minimumGrowthTemperature;

            if (extension != null)
            {
                def.modExtensions = new List<DefModExtension>();
                def.modExtensions.Add(extension);
            }

            Plant plant = new Plant();
            plant.def = def;
            plant.thingIDNumber = thingId;
            return plant;
        }

        private static float ReadLeaflessTemperatureThreshold(Plant plant)
        {
            if (LeaflessTemperatureThresholdProperty == null)
            {
                throw new InvalidOperationException(
                    "Plant.LeaflessTemperatureThresh was not found.");
            }

            object value = LeaflessTemperatureThresholdProperty.GetValue(plant, null);
            return (float)value;
        }
    }

    [TestSuite]
    public static class ColdDamageProgressTests
    {
        [Test]
        public static void ColdExposureConsumesHpAndWarmWeatherDoesNotHeal()
        {
            Type type = typeof(ColdToleranceExtension).Assembly.GetType(
                "CropColdToleranceOverhaul.ColdDamageProgress", true);
            MethodInfo loss = type.GetMethod(
                "HitPointLoss", BindingFlags.Static | BindingFlags.NonPublic);
            Assert.That((float)loss.Invoke(null,
                new object[] { -10f, -5f, 10f, 2500, 20 })).Is.EqualTo(10f);
            Assert.That((float)loss.Invoke(null,
                new object[] { 5f, -5f, 10f, 2500, 20 })).Is.EqualTo(0f);
        }
    }

    [TestSuite]
    public static class IndividualColdDamageTests
    {
        [Test]
        public static void PersistentPlantIdentityKeepsLethalDoseBoundedAndRepeatable()
        {
            Type type = typeof(ColdToleranceExtension).Assembly.GetType(
                "CropColdToleranceOverhaul.ColdDamageProgress", true);
            MethodInfo multiplier = type.GetMethod(
                "LethalDoseMultiplier", BindingFlags.Static | BindingFlags.NonPublic);
            if (multiplier == null)
                throw new InvalidOperationException("Missing stable dose mapper.");

            float disabled = (float)multiplier.Invoke(null, new object[] { 101, 0f });
            float a = (float)multiplier.Invoke(null, new object[] { 101, 0.25f });
            float samePlant = (float)multiplier.Invoke(null, new object[] { 101, 0.25f });
            float anotherPlant = (float)multiplier.Invoke(null, new object[] { 987654, 0.25f });

            Assert.That(disabled).Is.EqualTo(1f);
            Assert.That(a).Is.EqualTo(samePlant);
            Assert.That(a).Is.BetweenInclusive(0.75f, 1.25f);
            Assert.That(anotherPlant).Is.BetweenInclusive(0.75f, 1.25f);
            Assert.That(a == anotherPlant).Is.False();

            MethodInfo loss = type.GetMethod(
                "HitPointLoss", BindingFlags.Static | BindingFlags.NonPublic);
            float damageA = (float)loss.Invoke(null,
                new object[] { -10f, -5f, 10f * a, 2500, 20 });
            float damageB = (float)loss.Invoke(null,
                new object[] { -10f, -5f, 10f * anotherPlant, 2500, 20 });
            Assert.That(damageA > damageB).Is.True();
        }
    }

    [TestSuite]
    public static class ColdDamageObservationTests
    {
        [Test]
        public static void FirstColdSampleRecordsTimeWithoutRetroactiveInjury()
        {
            // The first reading must never assume that the previous
            // 2,000 unobserved game ticks had the same temperature.
            Plant plant = new Plant();
            plant.thingIDNumber = 404;
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDamageStartTemperature = -5f;
            extension.coldDamageDegreeHoursToDie = 10f;

            ColdDamageMapComponent component = new ColdDamageMapComponent(null);
            MethodInfo update = typeof(ColdDamageMapComponent).GetMethod(
                "UpdatePlant", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo field = typeof(ColdDamageMapComponent).GetField(
                "states", BindingFlags.Instance | BindingFlags.NonPublic);

            if (update == null || field == null)
                throw new InvalidOperationException("Cold observation API missing.");

            int readingTick = Find.TickManager.TicksGame;
            update.Invoke(component, new object[] { plant, extension, -10f });

            Dictionary<Plant, ColdDamageRecord> states =
                (Dictionary<Plant, ColdDamageRecord>)field.GetValue(component);
            Assert.That(states.ContainsKey(plant)).Is.True();
            Assert.That(states[plant].pendingHitPointDamage).Is.EqualTo(0f);
            Assert.That(states[plant].lastTick).Is.EqualTo(readingTick);
        }
    }

    [TestSuite]
    public static class InfoCardStatTests
    {
        private static readonly MethodInfo AppendCctoStatsMethod = FindAppendCctoStatsMethod();

        [Test]
        public static void OptInDamageInfoCardTakesPriorityOverLegacyDeath()
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDeathTemperature = -12f;
            extension.coldDamageStartTemperature = -5f;
            extension.coldDamageDegreeHoursToDie = 48f;

            List<StatDrawEntry> entries = BuildCctoStats(extension, 0f);
            Assert.ThatCollection(entries).Has.Count(1);
            Assert.That(entries[0].ValueString).Is.EqualTo((-5f).ToStringTemperature());
        }

        [Test]
        public static void FixedDeathTemperatureProducesOneCctoStat()
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDeathTemperature = -12f;

            List<StatDrawEntry> entries = BuildCctoStats(extension, 5f);

            Assert.ThatCollection(entries).Has.Count(1);
        }

        [Test]
        public static void DormancyOnlyProducesOneCctoStat()
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDormancy = true;

            List<StatDrawEntry> entries = BuildCctoStats(extension, 5f);

            Assert.ThatCollection(entries).Has.Count(1);
            Assert.That(entries[0].ValueString).Is.EqualTo(5f.ToStringTemperature());
        }

        [Test]
        public static void DormancyAndExtremeColdDeathProduceTwoCctoStats()
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDormancy = true;
            extension.coldDeathTemperature = -25f;

            List<StatDrawEntry> entries = BuildCctoStats(extension, 5f);

            Assert.ThatCollection(entries).Has.Count(2);
        }

        [Test]
        public static void ColdDeathAndDormancyUseTheSameInfoCardPriority()
        {
            ColdToleranceExtension deathExtension = new ColdToleranceExtension();
            deathExtension.coldDeathTemperature = -12f;

            ColdToleranceExtension dormancyExtension = new ColdToleranceExtension();
            dormancyExtension.coldDormancy = true;

            List<StatDrawEntry> deathEntries =
                BuildCctoStats(deathExtension, 5f);
            List<StatDrawEntry> dormancyEntries =
                BuildCctoStats(dormancyExtension, 5f);

            Assert.ThatCollection(deathEntries).Has.Count(1);
            Assert.ThatCollection(dormancyEntries).Has.Count(1);
            Assert.That(
                deathEntries[0].DisplayPriorityWithinCategory)
                .Is.EqualTo(4151);
            Assert.That(
                dormancyEntries[0].DisplayPriorityWithinCategory)
                .Is.EqualTo(4151);
        }

        private static MethodInfo FindAppendCctoStatsMethod()
        {
            Type patchType = typeof(ColdToleranceExtension).Assembly.GetType(
                "CropColdToleranceOverhaul.Patches.ThingDefSpecialDisplayStatsPatch",
                false);

            if (patchType == null)
            {
                return null;
            }

            return patchType.GetMethod(
                "AppendCctoStats",
                BindingFlags.Static | BindingFlags.NonPublic);
        }

        private static List<StatDrawEntry> BuildCctoStats(
            ColdToleranceExtension extension,
            float dormancyTemperature)
        {
            if (AppendCctoStatsMethod == null)
            {
                throw new InvalidOperationException(
                    "CCTO Info Card stat builder was not found.");
            }

            IEnumerable<StatDrawEntry> original =
                new List<StatDrawEntry>();

            object result = AppendCctoStatsMethod.Invoke(
                null,
                new object[] { original, extension, dormancyTemperature });

            return ((IEnumerable<StatDrawEntry>)result).ToList();
        }
    }
}

