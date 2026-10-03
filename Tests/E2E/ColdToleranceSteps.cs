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
        private readonly List<Thing> testRoomWalls = new List<Thing>();
        private CellRect? testRoomRect;
        private int? rememberedDormancyTick;

        private static readonly FieldInfo MadeLeaflessTickField =
            typeof(Plant).GetField(
                "madeLeaflessTick",
                BindingFlags.Instance | BindingFlags.NonPublic);

        [Then("loaded Vanilla sowable plant Defs match the CCTO balance table")]
        public void AssertLoadedVanillaBalanceDefs(PickleContext ctx)
        {
            AssertLoadedBalanceDef(ctx, "Plant_Rice", 10f, -1f, false);
            AssertLoadedBalanceDef(ctx, "Plant_Potato", 5f, -2f, false);
            AssertLoadedBalanceDef(ctx, "Plant_Corn", 8f, -2f, false);
            AssertLoadedBalanceDef(ctx, "Plant_Strawberry", 5f, -9f, false);
            AssertLoadedBalanceDef(ctx, "Plant_Haygrass", 0f, -9f, false);
            AssertLoadedBalanceDef(ctx, "Plant_Cotton", 10f, -1f, false);
            AssertLoadedBalanceDef(ctx, "Plant_Devilstrand", 8f, -1f, false);
            AssertLoadedBalanceDef(ctx, "Plant_Healroot", 0f, -9f, false);
            AssertLoadedBalanceDef(ctx, "Plant_Hops", 5f, null, true);
            AssertLoadedBalanceDef(ctx, "Plant_Smokeleaf", 5f, -4f, false);
            AssertLoadedBalanceDef(ctx, "Plant_Psychoid", 8f, -1f, false);
            AssertLoadedBalanceDef(ctx, "Plant_TreeCocoa", 12f, 0f, false);
            AssertLoadedBalanceDef(ctx, "Plant_TreeBamboo", 10f, -18f, false);
            AssertLoadedBalanceDef(ctx, "Plant_TreeBirch", 5f, null, true);
            AssertLoadedBalanceDef(ctx, "Plant_TreeCecropia", 10f, 0f, false);
            AssertLoadedBalanceDef(ctx, "Plant_TreeCypress", 5f, null, true);
            AssertLoadedBalanceDef(ctx, "Plant_Dandelion", 0f, null, true);
            AssertLoadedBalanceDef(ctx, "Plant_Daylily", 0f, null, true);
            AssertLoadedBalanceDef(ctx, "Plant_TreeDrago", 8f, 0f, false);
            AssertLoadedBalanceDef(ctx, "Plant_TreeMaple", 5f, null, true);
            AssertLoadedBalanceDef(ctx, "Plant_TreeOak", 5f, null, true);
            AssertLoadedBalanceDef(ctx, "Plant_TreePalm", 10f, 0f, false);
            AssertLoadedBalanceDef(ctx, "Plant_TreePine", 0f, -35f, false);
            AssertLoadedBalanceDef(ctx, "Plant_TreePoplar", 5f, null, true);
            AssertLoadedBalanceDef(ctx, "Plant_Rose", 5f, null, true);
            AssertLoadedBalanceDef(ctx, "Plant_SaguaroCactus", 8f, -6f, false);
            AssertLoadedBalanceDef(ctx, "Plant_TreeTeak", 12f, 3f, false);
            AssertLoadedBalanceDef(ctx, "Plant_Timbershroom", 0f, null, true);
            AssertLoadedBalanceDef(ctx, "Plant_Tinctoria", 5f, -4f, false);
            AssertLoadedBalanceDef(ctx, "Plant_TreeWillow", 5f, null, true);
        }

        [Then("loaded Medieval Overhaul crop Defs match the CCTO balance table")]
        public void AssertLoadedMedievalOverhaulBalanceDefs(PickleContext ctx)
        {
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Onions", 5f, -3f, false);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Lentils", 5f, -4f, false);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Cabbages", 0f, -6f, false);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Garlic", 0f, null, true);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Mushrooms", 5f, -1f, false);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Wheat", 0f, -6f, false);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Flax", 5f, -5f, false);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Sugarcane", 10f, -5f, false);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Carrots", 0f, -4f, false);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Herb", 5f, -3f, false);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Tomatoes", 10f, -1f, false);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Grape", 5f, null, true);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Pumpkins", 10f, -1f, false);
            AssertLoadedBalanceDef(ctx, "DankPyon_Tree_Apple", 5f, null, true);
            AssertLoadedBalanceDef(ctx, "DankPyon_Tree_Lemon", 10f, -4f, false);
            AssertLoadedBalanceDef(ctx, "DankPyon_Tree_Mulberry", 5f, null, true);
            AssertLoadedBalanceDef(ctx, "DankPyon_Tree_GriffonBerry", 5f, null, true);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Mindwort", 5f, -3f, false);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Poppy", 5f, -5f, false);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_Fleawort", 3f, -6f, false);
            AssertLoadedBalanceDef(ctx, "DankPyon_Plant_FlyAgaric", 0f, null, true);
        }

        [Then("all loaded sowable plant Defs are covered by CCTO balance")]
        public void AssertAllLoadedSowablePlantsCovered(PickleContext ctx)
        {
            string[] expectedNames =
            {
                "Plant_Rice",
                "Plant_Potato",
                "Plant_Corn",
                "Plant_Strawberry",
                "Plant_Haygrass",
                "Plant_Cotton",
                "Plant_Devilstrand",
                "Plant_Healroot",
                "Plant_Hops",
                "Plant_Smokeleaf",
                "Plant_Psychoid",
                "Plant_TreeCocoa",
                "Plant_TreeBamboo",
                "Plant_TreeBirch",
                "Plant_TreeCecropia",
                "Plant_TreeCypress",
                "Plant_Dandelion",
                "Plant_Daylily",
                "Plant_TreeDrago",
                "Plant_TreeMaple",
                "Plant_TreeOak",
                "Plant_TreePalm",
                "Plant_TreePine",
                "Plant_TreePoplar",
                "Plant_Rose",
                "Plant_SaguaroCactus",
                "Plant_TreeTeak",
                "Plant_Timbershroom",
                "Plant_Tinctoria",
                "Plant_TreeWillow",
                "DankPyon_Plant_Onions",
                "DankPyon_Plant_Lentils",
                "DankPyon_Plant_Cabbages",
                "DankPyon_Plant_Garlic",
                "DankPyon_Plant_Mushrooms",
                "DankPyon_Plant_Wheat",
                "DankPyon_Plant_Flax",
                "DankPyon_Plant_Sugarcane",
                "DankPyon_Plant_Carrots",
                "DankPyon_Plant_Herb",
                "DankPyon_Plant_Tomatoes",
                "DankPyon_Plant_Grape",
                "DankPyon_Plant_Pumpkins",
                "DankPyon_Tree_Apple",
                "DankPyon_Tree_Lemon",
                "DankPyon_Tree_Mulberry",
                "DankPyon_Tree_GriffonBerry",
                "DankPyon_Plant_Mindwort",
                "DankPyon_Plant_Poppy",
                "DankPyon_Plant_Fleawort",
                "DankPyon_Plant_FlyAgaric"
            };

            HashSet<string> expected =
                new HashSet<string>(expectedNames, StringComparer.Ordinal);

            List<ThingDef> sowable =
                DefDatabase<ThingDef>.AllDefsListForReading
                    .Where(def =>
                        def != null
                        && def.plant != null
                        && def.plant.sowTags != null
                        && def.plant.sowTags.Count > 0
                        && (
                            !def.plant.mustBeWildToSow
                            || (
                                def.plant.sowResearchPrerequisites != null
                                && def.plant.sowResearchPrerequisites.Any(
                                    research =>
                                        research != null
                                        && research.defName == "TreeSowing")
                            )
                        ))
                    .ToList();

            List<string> missingExtension =
                sowable
                    .Where(def =>
                        def.GetModExtension<ColdToleranceExtension>() == null)
                    .Select(def => def.defName)
                    .OrderBy(name => name)
                    .ToList();

            List<string> unexpectedSowable =
                sowable
                    .Where(def => !expected.Contains(def.defName))
                    .Select(def => def.defName)
                    .OrderBy(name => name)
                    .ToList();

            List<string> expectedMissing =
                expected
                    .Where(name =>
                        !sowable.Any(def => def.defName == name))
                    .OrderBy(name => name)
                    .ToList();

            ctx.Assert(
                missingExtension.Count == 0,
                "Loaded sowable plant Defs without CCTO balance: "
                + string.Join(", ", missingExtension.ToArray()));

            ctx.Assert(
                unexpectedSowable.Count == 0,
                "Loaded sowable plant Defs are outside the curated CCTO balance set: "
                + string.Join(", ", unexpectedSowable.ToArray()));

            ctx.Assert(
                expectedMissing.Count == 0,
                "Expected CCTO sowable plant Defs were not loaded as sowable: "
                + string.Join(", ", expectedMissing.ToArray()));
        }

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
                BuildCctoStats(deathOnly, 5f).Count == 1,
                "Fixed-death-only configuration should add one CCTO Info Card stat.");
            List<StatDrawEntry> dormancyStats =
                BuildCctoStats(dormancyOnly, 5f);

            ctx.Assert(
                dormancyStats.Count == 1,
                "Dormancy-only configuration should add one CCTO Info Card stat.");
            ctx.Assert(
                dormancyStats[0].ValueString == 5f.ToStringTemperature(),
                "Dormancy Info Card stat should show the minimum-growth temperature as the explicit dormancy threshold.");
            ctx.Assert(
                BuildCctoStats(both, 5f).Count == 2,
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
            Plant plant = RequireTestPlant(ctx);
            Room room = plant.GetRoom();

            ctx.Require(
                room != null,
                "CCTO test plant has no room/region temperature context.");
            ctx.Require(
                room.UsesOutdoorTemperature,
                "CCTO test plant must still be outdoors for this step.");

            SetRoomTemperatureAndVerify(
                ctx,
                plant,
                room,
                target,
                "outdoor test room");
        }

        [When("I enclose the CCTO test plant in a roofed indoor room")]
        public void EncloseTestPlant(PickleContext ctx)
        {
            Plant plant = RequireTestPlant(ctx);
            Map map = RequireCurrentMap(ctx);

            CellRect rect = CellRect.CenteredOn(plant.Position, 2);
            ctx.Require(
                RectCanBecomeTestRoom(rect, plant.Position, map),
                "The selected CCTO test plant position cannot be enclosed "
                + "into the deterministic 5x5 test room.");

            foreach (IntVec3 cell in rect.EdgeCells)
            {
                Thing wall =
                    ThingMaker.MakeThing(
                        ThingDefOf.Wall,
                        ThingDefOf.Steel);

                GenSpawn.Spawn(
                    wall,
                    cell,
                    map,
                    WipeMode.Vanish);

                testRoomWalls.Add(wall);
            }

            foreach (IntVec3 cell in rect.Cells)
            {
                map.roofGrid.SetRoof(
                    cell,
                    RoofDefOf.RoofConstructed);
            }

            map.regionAndRoomUpdater.RebuildAllRegionsAndRooms();

            Room room = plant.GetRoom();
            ctx.Require(
                room != null,
                "The enclosed CCTO test plant has no room after region rebuild.");
            ctx.Require(
                !room.UsesOutdoorTemperature,
                "The CCTO indoor regression room unexpectedly uses outdoor temperature.");

            testRoomRect = rect;
        }

        [When("I set only the CCTO test outdoor temperature to {float}")]
        public void SetOutdoorTemperatureOnly(
            PickleContext ctx,
            float target)
        {
            Map map = RequireCurrentMap(ctx);
            Plant plant = RequireTestPlant(ctx);
            Room plantRoom = plant.GetRoom();

            ctx.Require(
                plantRoom != null,
                "CCTO indoor test plant has no room.");

            Room outdoorRoom = FindOutdoorRoom(ctx, map, plantRoom);
            outdoorRoom.Temperature = target;

            ctx.Require(
                Math.Abs(outdoorRoom.Temperature - target) < 0.25f,
                "Failed to set deterministic outdoor room temperature. Target="
                + target.ToString("F2")
                + " C, actual="
                + outdoorRoom.Temperature.ToString("F2")
                + " C.");
        }

        [When("I set the CCTO test plant room temperature to {float}")]
        public void SetPlantRoomTemperature(
            PickleContext ctx,
            float target)
        {
            Plant plant = RequireTestPlant(ctx);
            Room room = plant.GetRoom();

            ctx.Require(
                room != null,
                "CCTO test plant has no room/region temperature context.");
            ctx.Require(
                !room.UsesOutdoorTemperature,
                "CCTO test plant room must be a true indoor room for this step.");

            SetRoomTemperatureAndVerify(
                ctx,
                plant,
                room,
                target,
                "indoor plant room");
        }

        [When("I run one CCTO plant long tick")]
        public void RunPlantLongTick(PickleContext ctx)
        {
            Plant plant = RequireTestPlant(ctx);
            plant.TickLong();
        }

        [When("I force the CCTO plant cold check")]
        public void ForcePlantColdCheck(PickleContext ctx)
        {
            Plant plant = RequireTestPlant(ctx);

            MethodInfo check =
                typeof(Plant).GetMethod(
                    "CheckMakeLeafless",
                    BindingFlags.Instance
                    | BindingFlags.NonPublic);

            ctx.Require(
                check != null,
                "Plant.CheckMakeLeafless was not found.");

            check.Invoke(plant, null);
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

        [When("I remember the CCTO dormancy refresh tick")]
        public void RememberDormancyRefreshTick(PickleContext ctx)
        {
            Plant plant = RequireTestPlant(ctx);

            ctx.Require(
                MadeLeaflessTickField != null,
                "Plant.madeLeaflessTick was not found.");

            rememberedDormancyTick =
                (int)MadeLeaflessTickField.GetValue(plant);
        }

        [Then("the CCTO dormancy refresh tick has not changed")]
        public void AssertDormancyRefreshTickUnchanged(PickleContext ctx)
        {
            Plant plant = RequireTestPlant(ctx);

            ctx.Require(
                MadeLeaflessTickField != null,
                "Plant.madeLeaflessTick was not found.");
            ctx.Require(
                rememberedDormancyTick.HasValue,
                "CCTO dormancy refresh tick was not remembered.");

            int current =
                (int)MadeLeaflessTickField.GetValue(plant);

            ctx.Assert(
                current == rememberedDormancyTick.Value,
                "CCTO refreshed the dormancy timer after temperature recovery. "
                + "Remembered="
                + rememberedDormancyTick.Value
                + ", current="
                + current
                + ".");
        }

        [When("I expire the CCTO dormancy recovery timer")]
        public void ExpireDormancyRecoveryTimer(PickleContext ctx)
        {
            Plant plant = RequireTestPlant(ctx);

            ctx.Require(
                MadeLeaflessTickField != null,
                "Plant.madeLeaflessTick was not found.");

            MadeLeaflessTickField.SetValue(
                plant,
                Find.TickManager.TicksGame - 60000);
        }

        [Then("the CCTO test plant has recovered from dormancy")]
        public void AssertPlantRecoveredFromDormancy(PickleContext ctx)
        {
            Plant plant = RequireTestPlant(ctx);
            ctx.Assert(
                !plant.Destroyed,
                "Recovered CCTO test plant should still be alive.");
            ctx.Assert(
                !plant.LeaflessNow,
                "CCTO test plant should no longer be leafless after "
                + "the delayed recovery window expires.");
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
                RemoveTestRoom();

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
                rememberedDormancyTick = null;
            }
        }

        [PickleStateDump]
        public string DumpCctoState()
        {
            string plantState = "no test plant";
            if (testPlant != null)
            {
                string ambient =
                    testPlant.Spawned
                        ? testPlant.AmbientTemperature.ToString("F2") + " C"
                        : "unspawned";

                plantState =
                    "ambient=" + ambient
                    + ", destroyed=" + testPlant.Destroyed
                    + ", leafless=" + testPlant.LeaflessNow;
            }

            return plantState;
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

            IntVec3 cell = FindOutdoorPlantCell(ctx, map, def);
            Room spawnRoom = cell.GetRoom(map);

            ctx.Require(
                spawnRoom != null,
                "CCTO test spawn cell has no room.");
            ctx.Require(
                spawnRoom.UsesOutdoorTemperature,
                "CCTO test spawn cell must use outdoor temperature.");

            spawnRoom.Temperature = safeTemperature;

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

                CellRect roomRect =
                    CellRect.CenteredOn(cell, 2);

                if (!RectCanBecomeTestRoom(
                    roomRect,
                    cell,
                    map))
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

        private static void SetRoomTemperatureAndVerify(
            PickleContext ctx,
            Plant plant,
            Room room,
            float target,
            string context)
        {
            room.Temperature = target;

            float ambient = plant.AmbientTemperature;
            ctx.Require(
                Math.Abs(ambient - target) < 0.25f,
                "Failed to set deterministic " + context + " temperature. Target="
                + target.ToString("F2")
                + " C, ambient="
                + ambient.ToString("F2")
                + " C.");
        }

        private static Room FindOutdoorRoom(
            PickleContext ctx,
            Map map,
            Room excludedRoom)
        {
            foreach (IntVec3 cell in map.AllCells)
            {
                Room room = cell.GetRoom(map);
                if (room != null
                    && room != excludedRoom
                    && room.UsesOutdoorTemperature)
                {
                    return room;
                }
            }

            ctx.Require(
                false,
                "No outdoor room was found for the CCTO indoor regression test.");

            return null;
        }

        private static bool RectCanBecomeTestRoom(
            CellRect rect,
            IntVec3 plantCell,
            Map map)
        {
            foreach (IntVec3 cell in rect.Cells)
            {
                if (!cell.InBounds(map))
                {
                    return false;
                }

                if (cell == plantCell)
                {
                    continue;
                }

                if (rect.IsOnEdge(cell)
                    && cell.GetEdifice(map) != null)
                {
                    return false;
                }
            }

            return true;
        }

        private void RemoveTestRoom()
        {
            Map map = Find.CurrentMap;

            foreach (Thing wall in testRoomWalls)
            {
                if (wall != null && !wall.Destroyed)
                {
                    wall.Destroy(DestroyMode.Vanish);
                }
            }

            testRoomWalls.Clear();

            if (map != null && testRoomRect.HasValue)
            {
                foreach (IntVec3 cell in testRoomRect.Value.Cells)
                {
                    if (cell.InBounds(map))
                    {
                        map.roofGrid.SetRoof(cell, null);
                    }
                }

                map.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
            }

            testRoomRect = null;
        }

        private static void AssertLoadedBalanceDef(
            PickleContext ctx,
            string defName,
            float expectedMinimumGrowthTemperature,
            float? expectedDeathTemperature,
            bool expectedDormancy)
        {
            ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(defName);

            ctx.Assert(
                def != null,
                "Expected loaded balance Def '" + defName + "' was not found.");

            if (def == null)
            {
                return;
            }

            ctx.Assert(
                def.plant != null,
                "Loaded balance Def '" + defName + "' is not a plant.");

            if (def.plant == null)
            {
                return;
            }

            ctx.Assert(
                Math.Abs(
                    def.plant.minGrowthTemperature
                    - expectedMinimumGrowthTemperature) < 0.001f,
                "Loaded minGrowthTemperature mismatch for "
                + defName
                + ": expected "
                + expectedMinimumGrowthTemperature.ToString("F2")
                + " C, got "
                + def.plant.minGrowthTemperature.ToString("F2")
                + " C.");

            List<ColdToleranceExtension> extensions =
                def.modExtensions == null
                    ? new List<ColdToleranceExtension>()
                    : def.modExtensions
                        .OfType<ColdToleranceExtension>()
                        .ToList();

            ctx.Assert(
                extensions.Count == 1,
                "Loaded balance Def '"
                + defName
                + "' should have exactly one CCTO extension, got "
                + extensions.Count
                + ".");

            if (extensions.Count != 1)
            {
                return;
            }

            ColdToleranceExtension extension = extensions[0];

            ctx.Assert(
                extension.coldDormancy == expectedDormancy,
                "Loaded dormancy flag mismatch for "
                + defName
                + ": expected "
                + expectedDormancy
                + ", got "
                + extension.coldDormancy
                + ".");

            if (expectedDeathTemperature.HasValue)
            {
                ctx.Assert(
                    extension.HasColdDeathTemperature,
                    "Loaded balance Def '"
                    + defName
                    + "' is missing its fixed cold-death temperature.");

                if (extension.HasColdDeathTemperature)
                {
                    ctx.Assert(
                        Math.Abs(
                            extension.coldDeathTemperature
                            - expectedDeathTemperature.Value) < 0.001f,
                        "Loaded coldDeathTemperature mismatch for "
                        + defName
                        + ": expected "
                        + expectedDeathTemperature.Value.ToString("F2")
                        + " C, got "
                        + extension.coldDeathTemperature.ToString("F2")
                        + " C.");
                }
            }
            else
            {
                ctx.Assert(
                    !extension.HasColdDeathTemperature,
                    "Loaded balance Def '"
                    + defName
                    + "' unexpectedly has coldDeathTemperature="
                    + extension.coldDeathTemperature.ToString("F2")
                    + " C.");
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
            ColdToleranceExtension extension,
            float dormancyTemperature)
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
                        extension,
                        dormancyTemperature
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
