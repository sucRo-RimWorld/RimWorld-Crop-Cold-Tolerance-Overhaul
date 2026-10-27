using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using RimWorks.Quickstarts;
using RimWorks.Quickstarts.Verification;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace CropColdToleranceOverhaul.E2E
{
    // Read-only VANILLA world climate observation. The sampled temperatures
    // come from RimWorld's World.tileTemperatures, NOT an invented shift.
    // World weather incidents are NOT advanced over this synthetic calendar.
    public sealed class CctoColdClimateNativeWorldQuickstart : AbstractQuickstart
    {
        private readonly List<SurfaceTile> sampleTiles = new List<SurfaceTile>();
        private readonly List<string> sampleRoles = new List<string>();
        private int selectedRealForest;
        private int naturallySpawnedPines;
        private bool wrote;
        public override TaggedString description => "Observe original generated boreal pine and short-season farming tile climate";
        public override string seed => "CCTO-NaturalColdClimate-20261022";
        public override float planetCoverage => 0.30f;
        public override int mapSize => 100;

        public override void PostApplyConfiguration()
        {
            ThingDef pine = DefDatabase<ThingDef>.GetNamed("Plant_TreePine");
            var forest = new List<SurfaceTile>();
            var crop = new List<SurfaceTile>();
            for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
            {
                SurfaceTile tile = Find.WorldGrid[i] as SurfaceTile;
                if (tile == null || tile.WaterCovered || tile.PrimaryBiome == null
                    || !TileFinder.IsValidTileForNewSettlement(tile.tile))
                    continue;
                if (tile.PrimaryBiome.defName == "BorealForest"
                    && tile.PrimaryBiome.CommonalityOfPlant(pine) > 0f)
                    forest.Add(tile);
                if (tile.PrimaryBiome.defName == "TemperateForest" &&
                    tile.temperature >= -1f && tile.temperature <= 10f)
                    crop.Add(tile);
            }
            if (forest.Count < 10 || crop.Count < 10)
                throw new InvalidOperationException(
                    "Unforced, settlement-valid pine boreal and crop temperate tiles missing: "
                    + forest.Count + "/" + crop.Count);
            forest.Sort((a, b) => a.temperature.CompareTo(b.temperature));
            crop.Sort((a, b) => a.temperature.CompareTo(b.temperature));
            SurfaceTile cold = forest[forest.Count / 6];  // cold natural forest percentile
            SurfaceTile typical = forest[forest.Count / 2]; // median natural forest
            SurfaceTile farm = crop[Math.Min(crop.Count - 1, crop.Count / 5)]; // marginal farm
            sampleTiles.Add(cold);
            sampleRoles.Add("boreal_cold");
            sampleTiles.Add(typical);
            sampleRoles.Add("boreal_median");
            sampleTiles.Add(farm);
            sampleRoles.Add("temperate_crop_marginal");
            selectedRealForest = forest.Count;
            Find.GameInitData.startingTile = typical.tile; // ACTUAL unmodified forest map
            Log.Message("CCTO_CLIMATE_WORLD_SELECTED pineEligible=" + forest.Count
                + " cropEligible=" + crop.Count + " originalSeed=" + seed);
        }

        public override void PostLoaded()
        {
            Map map = Find.CurrentMap;
            if (map == null || map.Biome == null || map.Biome.defName != "BorealForest"
                || map.Tile != sampleTiles[1].tile)
                throw new InvalidOperationException("Original boreal map not generated at selected tile");
            ThingDef pine = DefDatabase<ThingDef>.GetNamed("Plant_TreePine");
            naturallySpawnedPines = map.listerThings.ThingsOfDef(pine).Count;
            string destination = Environment.GetEnvironmentVariable("AMJ_CCTO_CLIMATE_OUT");
            if (string.IsNullOrWhiteSpace(destination))
                throw new InvalidOperationException("Isolated output CSV destination missing");
            if (File.Exists(destination))
                throw new InvalidOperationException("Refusing existing climate output");
            MethodInfo getter = typeof(TileTemperaturesComp).GetMethods(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .SingleOrDefault(m => m.Name == "GetOutdoorTemp"
                    && m.GetParameters().Length == 1
                    && m.GetParameters()[0].ParameterType == typeof(PlanetTile));
            if (getter == null || getter.ReturnType != typeof(float))
                throw new InvalidOperationException(
                    "Expected original 1.6 TileTemperaturesComp.GetOutdoorTemp(PlanetTile):float unavailable");
            MethodInfo cached = typeof(TileTemperaturesComp).GetMethod(
                "RetrieveCachedData", BindingFlags.Instance | BindingFlags.NonPublic);
            if (cached == null)
                throw new InvalidOperationException("Original world temperature cache API missing");
            CultureInfo c = CultureInfo.InvariantCulture;
            var csv = new StringBuilder();
            csv.AppendLine("role,day,hour,ticks_abs,world_tile,biome,latitude,longitude,annual_tile_C,rainfall_mm,generated_pine_count,world_outdoor_C");
            int original = Find.TickManager.TicksGame;
            int originalAbs = Find.TickManager.TicksAbs;
            int count = 0;
            try
            {
                for (int day = 0; day < 60; day++)
                {
                    for (int hour = 0; hour < 24; hour++)
                    {
                        int ticks = original + (day * 24 + hour) * GenDate.TicksPerHour;
                        Find.TickManager.DebugSetTicksGame(ticks);
                        Find.World.tileTemperatures.WorldComponentTick();
                        for (int t = 0; t < sampleTiles.Count; t++)
                        {
                            SurfaceTile tile = sampleTiles[t];
                            object data = cached.Invoke(Find.World.tileTemperatures,
                                new object[] { tile.tile });
                            MethodInfo check = data.GetType().GetMethod(
                                "CheckCache", BindingFlags.Instance | BindingFlags.Public
                                | BindingFlags.NonPublic);
                            if (check == null) throw new InvalidOperationException("Missing temperature CheckCache");
                            check.Invoke(data, null);
                            float value = (float)getter.Invoke(Find.World.tileTemperatures,
                                new object[] { tile.tile });
                            if (float.IsNaN(value) || float.IsInfinity(value) ||
                                value < -100 || value > 100)
                                throw new InvalidOperationException("World climate invalid " + value);
                            Vector2 geo = Find.WorldGrid.LongLatOf(tile.tile);
                            csv.Append(sampleRoles[t]).Append(',')
                               .Append(day).Append(',').Append(hour).Append(',')
                               .Append(Find.TickManager.TicksAbs).Append(',')
                               .Append(tile.tile).Append(',')
                               .Append(tile.PrimaryBiome.defName).Append(',')
                               .Append(geo.y.ToString("F4", c)).Append(',')
                               .Append(geo.x.ToString("F4", c)).Append(',')
                               .Append(tile.temperature.ToString("F3", c)).Append(',')
                               .Append(tile.rainfall.ToString("F1", c)).Append(',')
                               .Append(t == 1 ? naturallySpawnedPines : -1).Append(',')
                               .Append(value.ToString("F4", c)).AppendLine();
                            count++;
                        }
                    }
                }
            }
            finally
            {
                Find.TickManager.DebugSetTicksGame(original);
                Find.World.tileTemperatures.WorldComponentTick();
            }
            if (count != 4320)
                throw new InvalidOperationException("Expected 3x60x24 hourly world climate rows");
            if (originalAbs != Find.TickManager.TicksAbs)
                throw new InvalidOperationException("Original game calendar was not restored");
            File.WriteAllText(destination, csv.ToString(), Encoding.UTF8);
            wrote = File.Exists(destination) && new FileInfo(destination).Length > 100000;
            if (!wrote)
                throw new InvalidOperationException("Climate CSV missing or truncated");
            Log.Message("CCTO_CLIMATE_WORLD_RESULT generated=true"
                + " worldTiles=3" + " dayHours=1440"
                + " values=" + count + " naturalPinesOnBorealMap=" + naturallySpawnedPines
                + " candidateForestTiles=" + selectedRealForest
                + " climateSource=original_RimWorld_1.6_world_tile_temperature_component"
                + " weatherEventsSimulated=false gameCalendarRestored=true");
        }

        public override QuickstartVerification Verify()
        {
            QuickstartVerification v = new QuickstartVerification();
            v.Assert("actual unforced BorealForest world map generated",
                () => Find.CurrentMap != null
                  && Find.CurrentMap.Biome.defName == "BorealForest"
                  && Find.CurrentMap.Tile == sampleTiles[1].tile);
            v.Assert("three actual original climate tile sequences recorded",
                () => wrote && sampleTiles.Count == 3);
            v.Assert("natural source forest has tree spawn eligibility",
                () => selectedRealForest > 0);
            return v;
        }
    }
}
