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
    public static class InfoCardStatTests
    {
        private static readonly MethodInfo AppendCctoStatsMethod = FindAppendCctoStatsMethod();

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
