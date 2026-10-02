using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CropColdToleranceOverhaul;
using RimWorks.Pickle;
using RimWorld;
using Verse;

namespace CropColdToleranceOverhaul.E2E
{
    [PickleSteps]
    public sealed class ColdToleranceSteps
    {
        private const string TestPlantDefName = "Plant_Potato";

        private ThingDef testPlantDef;
        private List<DefModExtension> originalModExtensions;
        private float originalMinimumGrowthTemperature;
        private Plant testPlant;
        private GameCondition_TemperatureOffset temperatureCondition;

        [Then("CCTO fixed death thresholds are independent of plant identity")]
        public void AssertFixedThresholdIsNotPerPlantRandom(PickleContext ctx)
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDeathTemperature = -12f;

            float first = ReadLeaflessThreshold(
                CreateUnspawnedPlant(8f, extension, 101));
            float second = ReadLeaflessThreshold(
                CreateUnspawnedPlant(8f, extension, 987654));

            ctx.Assert(
                Math.Abs(first - (-12f)) < 0.001f,
                "First configured plant did not use the exact fixed -12 C threshold.");
            ctx.Assert(
                Math.Abs(second - (-12f)) < 0.001f,
                "Second configured plant did not use the exact fixed -12 C threshold.");
        }

        [Then("CCTO dormancy uses the plant minimum growth temperature as its cold threshold")]
        public void AssertDormancyThresholdUsesMinimumGrowthTemperature(PickleContext ctx)
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDormancy = true;
            extension.coldDeathTemperature = -25f;

            float threshold = ReadLeaflessThreshold(
                CreateUnspawnedPlant(3f, extension, 202));

            ctx.Assert(
                Math.Abs(threshold - 3f) < 0.001f,
                "Dormancy threshold should equal minGrowthTemperature (3 C).");
        }

        [Then("an unconfigured plant retains the vanilla per-plant cold threshold range")]
        public void AssertUnconfiguredPlantKeepsVanillaRange(PickleContext ctx)
        {
            const float minimumGrowthTemperature = 10f;
            float threshold = ReadLeaflessThreshold(
                CreateUnspawnedPlant(
                    minimumGrowthTemperature,
                    null,
                    303));

            ctx.Assert(
                threshold >= minimumGrowthTemperature - 18f
                && threshold <= minimumGrowthTemperature - 10f,
                "Unconfigured plant threshold should stay inside the vanilla "
                + "minGrowthTemperature-18..-10 range, but was "
                + threshold.ToString("F2") + " C.");
        }

        [Then("CCTO extension validation accepts dormancy-only and rejects incomplete ordinary configuration")]
        public void AssertExtensionValidation(PickleContext ctx)
        {
            ColdToleranceExtension incomplete =
                new ColdToleranceExtension();
            ColdToleranceExtension dormancy =
                new ColdToleranceExtension();
            dormancy.coldDormancy = true;
            ColdToleranceExtension finite =
                new ColdToleranceExtension();
            finite.coldDeathTemperature = -12f;
            ColdToleranceExtension infinite =
                new ColdToleranceExtension();
            infinite.coldDormancy = true;
            infinite.coldDeathTemperature =
                float.PositiveInfinity;

            ctx.Assert(
                incomplete.ConfigErrors().Count() == 1,
                "An ordinary CCTO extension without a death temperature "
                + "should report one configuration error.");
            ctx.Assert(
                !dormancy.ConfigErrors().Any(),
                "Dormancy-only CCTO configuration should be valid.");
            ctx.Assert(
                !finite.ConfigErrors().Any(),
                "A finite fixed death temperature should be valid.");
            ctx.Assert(
                infinite.ConfigErrors().Count() == 1,
                "An infinite death temperature should report one configuration error.");
        }

        [Then("CCTO Info Card stat construction matches fixed-death and dormancy configuration")]
        public void AssertInfoCardStatConstruction(PickleContext ctx)
        {
            ColdToleranceExtension deathOnly =
                new ColdToleranceExtension();
            deathOnly.coldDeathTemperature = -12f;

            ColdToleranceExtension dormancyOnly =
                new ColdToleranceExtension();
            dormancyOnly.coldDormancy = true;

            ColdToleranceExtension both =
                new ColdToleranceExtension();
            both.coldDormancy = true;
            both.coldDeathTemperature = -25f;

            ctx.Assert(
                BuildCctoStats(deathOnly).Count == 1,
                "Fixed-death-only configuration should add one CCTO Info Card stat.");
            ctx.Assert(
                BuildCctoStats(dormancyOnly).Count == 1,
                "Dormancy-only configuration should add one CCTO Info Card stat.");
            ctx.Assert(
                BuildCctoStats(both).Count == 2,
                "Dormancy plus extreme-cold death should add two CCTO Info Card stats.");
        }

        [Given("a fixed-death CCTO test plant with minimum growth {float} and death {float}")]
        public void SpawnFixedDeathPlant(
            PickleContext ctx,
            float minimumGrowthTemperature,
            float deathTemperature)
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDeathTemperature = deathTemperature;

            ConfigureAndSpawn(
                ctx,
                minimumGrowthTemperature,
                extension);
        }

        [Given("a dormancy CCTO test plant with minimum growth {float}")]
        public void SpawnDormancyOnlyPlant(
            PickleContext ctx,
            float minimumGrowthTemperature)
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDormancy = true;

            ConfigureAndSpawn(
                ctx,
                minimumGrowthTemperature,
                extension);
        }

        [Given("a dormancy CCTO test plant with minimum growth {float} and death {float}")]
        public void SpawnDormancyAndDeathPlant(
            PickleContext ctx,
            float minimumGrowthTemperature,
            float deathTemperature)
        {
            ColdToleranceExtension extension = new ColdToleranceExtension();
            extension.coldDormancy = true;
            extension.coldDeathTemperature = deathTemperature;

            ConfigureAndSpawn(
                ctx,
                minimumGrowthTemperature,
                extension);
        }

        [When("I set the CCTO test outdoor temperature to {float}")]
        public void SetTestOutdoorTemperature(PickleContext ctx, float target)
        {
            Map map = RequireCurrentMap(ctx);
            ReplaceTemperatureCondition(ctx, map, target);
        }

        [Then("the CCTO test plant is alive")]
        public void AssertPlantAlive(PickleContext ctx)
        {
            Plant plant = RequireTestPlant(ctx);
            ctx.Assert(
                !plant.Destroyed,
                "CCTO test plant should be alive, but it was destroyed.");
        }

        [Then("the CCTO test plant is dead")]
        public void AssertPlantDead(PickleContext ctx)
        {
            Plant plant = RequireTestPlant(ctx);
            ctx.Assert(
                plant.Destroyed,
                "CCTO test plant should be dead, but it is still alive.");
        }

        [Then("the CCTO test plant is dormant")]
        public void AssertPlantDormant(PickleContext ctx)
        {
            Plant plant = RequireTestPlant(ctx);
            ctx.Assert(
                !plant.Destroyed,
                "CCTO test plant should be dormant and alive, but it was destroyed.");
            ctx.Assert(
                plant.LeaflessNow,
                "CCTO test plant should be leafless while cold-dormant.");
        }

        [AfterScenario]
        public void Cleanup()
        {
            try
            {
                if (testPlant != null && !testPlant.Destroyed)
                {
                    testPlant.Destroy(DestroyMode.Vanish);
                }
            }
            finally
            {
                RemoveTemperatureCondition();

                if (testPlantDef != null)
                {
                    testPlantDef.modExtensions = originalModExtensions;
                    if (testPlantDef.plant != null)
                    {
                        testPlantDef.plant.minGrowthTemperature =
                            originalMinimumGrowthTemperature;
                    }
                }

                testPlant = null;
                testPlantDef = null;
                originalModExtensions = null;
            }
        }

        [PickleStateDump]
        public string DumpCctoState()
        {
            string temperature = "no map";
            if (Find.CurrentMap != null)
            {
                temperature =
                    Find.CurrentMap.mapTemperature.OutdoorTemp.ToString("F2") + " C";
            }

            string plantState = "no test plant";
            if (testPlant != null)
            {
                plantState =
                    "destroyed=" + testPlant.Destroyed
                    + ", leafless=" + testPlant.LeaflessNow;
            }

            return "outdoorTemp=" + temperature + "; " + plantState;
        }

        private void ConfigureAndSpawn(
            PickleContext ctx,
            float minimumGrowthTemperature,
            ColdToleranceExtension extension)
        {
            ctx.Require(
                testPlant == null,
                "CCTO test setup attempted to create a second test plant.");

            Map map = RequireCurrentMap(ctx);
            ThingDef def =
                DefDatabase<ThingDef>.GetNamedSilentFail(TestPlantDefName);

            ctx.Require(
                def != null,
                "Required vanilla test plant def '" + TestPlantDefName
                + "' was not found.");
            ctx.Require(
                def.plant != null,
                "Test def '" + TestPlantDefName + "' is not a plant.");
            ctx.Require(
                typeof(Plant).IsAssignableFrom(def.thingClass),
                "Test def '" + TestPlantDefName
                + "' does not use Plant or a Plant subclass.");

            testPlantDef = def;
            originalModExtensions = def.modExtensions;
            originalMinimumGrowthTemperature =
                def.plant.minGrowthTemperature;

            List<DefModExtension> extensions =
                originalModExtensions == null
                    ? new List<DefModExtension>()
                    : originalModExtensions
                        .Where(x => !(x is ColdToleranceExtension))
                        .ToList();

            extensions.Add(extension);
            def.modExtensions = extensions;
            def.plant.minGrowthTemperature =
                minimumGrowthTemperature;

            float safeTemperature =
                Math.Max(
                    minimumGrowthTemperature + 10f,
                    extension.HasColdDeathTemperature
                        ? extension.coldDeathTemperature + 10f
                        : minimumGrowthTemperature + 10f);

            ReplaceTemperatureCondition(
                ctx,
                map,
                safeTemperature);

            IntVec3 cell = FindOutdoorPlantCell(ctx, map, def);
            Thing thing = ThingMaker.MakeThing(def);
            Plant plant = thing as Plant;

            ctx.Require(
                plant != null,
                "ThingMaker did not create a Plant for '"
                + TestPlantDefName + "'.");

            GenSpawn.Spawn(
                plant,
                cell,
                map,
                WipeMode.Vanish);

            testPlant = plant;

            ctx.Require(
                !plant.Destroyed,
                "CCTO test plant was destroyed during spawn setup.");
        }

        private static IntVec3 FindOutdoorPlantCell(
            PickleContext ctx,
            Map map,
            ThingDef plantDef)
        {
            foreach (IntVec3 cell in map.AllCells)
            {
                if (map.roofGrid.Roofed(cell))
                {
                    continue;
                }

                if (!plantDef.CanEverPlantAt(
                    cell,
                    map,
                    false,
                    false))
                {
                    continue;
                }

                return cell;
            }

            ctx.Require(
                false,
                "No unroofed plantable cell was found on the CCTO quickstart map.");

            return IntVec3.Invalid;
        }

        private void ReplaceTemperatureCondition(
            PickleContext ctx,
            Map map,
            float target)
        {
            RemoveTemperatureCondition();

            Find.World.tileTemperatures.ClearCaches();
            float current =
                map.mapTemperature.OutdoorTemp;

            GameConditionDef def = new GameConditionDef();
            def.defName = "CCTO_E2E_TemperatureOffset";
            def.label = "CCTO E2E temperature offset";
            def.description =
                "Temporary game condition used only by automated CCTO tests.";
            def.conditionClass =
                typeof(GameCondition_TemperatureOffset);
            def.displayOnUI = false;
            def.natural = false;
            def.temperatureOffset = target - current;

            GameCondition_TemperatureOffset condition =
                (GameCondition_TemperatureOffset)
                GameConditionMaker.MakeCondition(
                    def,
                    3600000);

            condition.suppressEndMessage = true;
            map.gameConditionManager.RegisterCondition(condition);
            temperatureCondition = condition;

            Find.World.tileTemperatures.ClearCaches();
            float actual =
                map.mapTemperature.OutdoorTemp;

            ctx.Require(
                Math.Abs(actual - target) < 0.25f,
                "Failed to set deterministic CCTO test temperature. "
                + "Target=" + target.ToString("F2")
                + " C, actual=" + actual.ToString("F2") + " C.");
        }

        private void RemoveTemperatureCondition()
        {
            if (temperatureCondition != null)
            {
                try
                {
                    if (temperatureCondition.gameConditionManager != null
                        && temperatureCondition.gameConditionManager
                            .ActiveConditions
                            .Contains(temperatureCondition))
                    {
                        temperatureCondition.End();
                    }
                }
                finally
                {
                    temperatureCondition = null;
                    if (Find.World != null)
                    {
                        Find.World.tileTemperatures.ClearCaches();
                    }
                }
            }
        }

        private static Plant CreateUnspawnedPlant(
            float minimumGrowthTemperature,
            ColdToleranceExtension extension,
            int thingId)
        {
            ThingDef def = new ThingDef();
            def.defName = "CCTO_E2E_UnspawnedPlant_" + thingId;
            def.label = "CCTO E2E unspawned plant";
            def.category = ThingCategory.Plant;
            def.plant = new PlantProperties();
            def.plant.minGrowthTemperature =
                minimumGrowthTemperature;

            if (extension != null)
            {
                def.modExtensions =
                    new List<DefModExtension>();
                def.modExtensions.Add(extension);
            }

            Plant plant = new Plant();
            plant.def = def;
            plant.thingIDNumber = thingId;
            return plant;
        }

        private static float ReadLeaflessThreshold(Plant plant)
        {
            PropertyInfo property =
                typeof(Plant).GetProperty(
                    "LeaflessTemperatureThresh",
                    BindingFlags.Instance
                    | BindingFlags.NonPublic);

            if (property == null)
            {
                throw new InvalidOperationException(
                    "Plant.LeaflessTemperatureThresh was not found.");
            }

            return (float)property.GetValue(plant, null);
        }

        private static List<StatDrawEntry> BuildCctoStats(
            ColdToleranceExtension extension)
        {
            Type patchType =
                typeof(ColdToleranceExtension)
                    .Assembly
                    .GetType(
                        "CropColdToleranceOverhaul.Patches.ThingDefSpecialDisplayStatsPatch",
                        false);

            if (patchType == null)
            {
                throw new InvalidOperationException(
                    "CCTO Info Card patch type was not found.");
            }

            MethodInfo append =
                patchType.GetMethod(
                    "AppendCctoStats",
                    BindingFlags.Static
                    | BindingFlags.NonPublic);

            if (append == null)
            {
                throw new InvalidOperationException(
                    "CCTO Info Card stat builder was not found.");
            }

            IEnumerable<StatDrawEntry> original =
                new List<StatDrawEntry>();

            object result =
                append.Invoke(
                    null,
                    new object[]
                    {
                        original,
                        extension
                    });

            return ((IEnumerable<StatDrawEntry>)result)
                .ToList();
        }

        private static Map RequireCurrentMap(PickleContext ctx)
        {
            Map map = Find.CurrentMap;
            ctx.Require(
                map != null,
                "CCTO E2E scenario requires a live current map.");
            return map;
        }

        private Plant RequireTestPlant(PickleContext ctx)
        {
            ctx.Require(
                testPlant != null,
                "CCTO test plant has not been created.");
            return testPlant;
        }
    }
}
