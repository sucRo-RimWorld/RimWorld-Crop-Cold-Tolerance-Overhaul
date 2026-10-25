using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CropColdToleranceOverhaul;
using RimWorks.Pickle;
using RimWorld;
using Verse;

namespace CropColdToleranceOverhaul.E2E
{
    // Test-only native Plant.TickLong growth. Calendar jumps and controlled
    // weather are explicit: this is NOT a fully ticked map season or pawn job.
    [PickleSteps]
    public sealed class CctoMoGrowthSteps
    {
        private static readonly string[] Names = {
            "DankPyon_Plant_Onions", "DankPyon_Plant_Cabbages",
            "DankPyon_Plant_Carrots", "DankPyon_Plant_Lentils"
        };
        private static readonly float[] Temperatures = { 25f, 7f, 3f };
        private const int Samples = 1500; // 50 RimWorld days, 2000 tick increments

        [Then("original MO and Agriculture cool-season crop growth is recorded")]
        public async Task Measure(PickleContext ctx)
        {
            Fixture fixture = null;
            try
            {
                await OnMain(() => { fixture = new Fixture(ctx); });
                for (int condition = 0; condition < Temperatures.Length; condition++)
                {
                    int selected = condition;
                    for (int cropIndex = 0; cropIndex < Names.Length; cropIndex++)
                    {
                        int chosenCrop = cropIndex;
                        await OnMain(() => fixture.Begin(Temperatures[selected], chosenCrop));
                        for (int first = 1; first <= Samples; first += 40)
                        {
                            int begin = first;
                            await OnMain(() => fixture.Advance(begin, Math.Min(Samples + 1, begin + 40)));
                            await Task.Delay(1);
                        }
                        await OnMain(() => fixture.Record(chosenCrop));
                    }
                }
                await OnMain(() => fixture.Complete());
            }
            finally
            {
                if (fixture != null) await OnMain(() => fixture.Dispose());
            }
        }

        private static Task OnMain(Action action)
        {
            var completion = new TaskCompletionSource<bool>();
            RimWorks.Pickle.Runtime.PickleDriver.Post(() =>
            {
                try
                {
                    if (!UnityData.IsInMainThread)
                        throw new InvalidOperationException("Native plant growth must run on game main thread");
                    action();
                    completion.SetResult(true);
                }
                catch (Exception error) { completion.SetException(error); }
            });
            return completion.Task;
        }

        private sealed class Fixture
        {
            private readonly PickleContext ctx;
            private readonly Map map;
            private readonly Stopwatch deadline = Stopwatch.StartNew();
            private readonly int originalTick;
            private readonly TimeSpeed originalSpeed;
            private readonly float? originalOutdoor;
            private readonly float originalSky;
            private readonly List<IntVec3> cells = new List<IntVec3>();
            private readonly List<TerrainDef> terrains = new List<TerrainDef>();
            private readonly List<float> snow = new List<float>();
            private readonly List<ThingDef> defs = new List<ThingDef>();
            private readonly List<Plant> plants = new List<Plant>();
            private readonly int[] firstHarvest = new int[4];
            private readonly int[] firstMature = new int[4];
            private readonly float[] day10 = new float[4];
            private readonly float[] day20 = new float[4];
            private readonly float[] day50 = new float[4];
            private int simulatedStart;
            private int currentCrop;
            private float temperature;
            private int completed;
            private bool disposed;

            public Fixture(PickleContext ctx)
            {
                this.ctx = ctx;
                map = Find.CurrentMap;
                ctx.Require(map != null && map.Size.x == 50 && map.Size.z == 50,
                    "Expected isolated real-game 50x50 Quickstart map");
                ctx.Require(LoadedModManager.RunningModsListForReading.Any(m =>
                    string.Equals(m.PackageId, "dankpyon.medieval.overhaul", StringComparison.OrdinalIgnoreCase)),
                    "Original MO provider is required, not the substitute Def fixture");
                ctx.Require(LoadedModManager.RunningModsListForReading.Any(m =>
                    string.Equals(m.PackageId, "sucro.ancientmedievaljapan.core", StringComparison.OrdinalIgnoreCase)),
                    "Actual Agriculture growth patch provider is required");
                originalTick = Find.TickManager.TicksGame;
                originalSpeed = Find.TickManager.CurTimeSpeed;
                originalOutdoor = map.Biome.constantOutdoorTemperature;
                originalSky = map.skyManager.CurSkyGlow;
                Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
                foreach (string name in Names)
                {
                    ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(name);
                    ctx.Require(def != null && def.plant != null && def.plant.harvestedThingDef != null,
                        "Missing loaded original MO plant " + name);
                    ctx.Require(def.GetModExtension<ColdToleranceExtension>()?.HasColdDamageModel == true,
                        "V3 HP model not attached to original MO plant " + name);
                    ctx.Require(Math.Abs(def.plant.growDays -
                        (name == "DankPyon_Plant_Carrots" ? 6f : 8f)) < .001f,
                        "MO+Agriculture growth values not loaded " + name);
                    defs.Add(def);
                }
                foreach (IntVec3 c in CellRect.CenteredOn(map.Center, 12).Cells)
                {
                    if (cells.Count >= 1) break;
                    if (!c.InBounds(map) || c.Roofed(map) || !c.Standable(map)) continue;
                    if (c.GetThingList(map).Any(t => t is Plant || t is Pawn ||
                        t.def.category == ThingCategory.Building)) continue;
                    Room r = c.GetRoom(map);
                    if (r == null || !r.UsesOutdoorTemperature) continue;
                    cells.Add(c);
                    terrains.Add(map.terrainGrid.TerrainAt(c));
                    snow.Add(map.snowGrid.GetDepth(c));
                    map.terrainGrid.SetTerrain(c, TerrainDefOf.Soil);
                    map.snowGrid.SetDepth(c, 0f);
                }
                ctx.Require(cells.Count == 1, "Single native outdoor Soil test cell unavailable");
                for (int i = 0; i < cells.Count; i++)
                    ctx.Require(Math.Abs(map.fertilityGrid.FertilityAt(cells[i]) - 1f) < .01f,
                        "Native soil fertility not 100 percent");
            }

            public void Begin(float temp, int index)
            {
                temperature = temp;
                currentCrop = index;
                foreach (Plant old in plants)
                    if (old != null && !old.Destroyed) old.Destroy(DestroyMode.Vanish);
                plants.Clear();
                Plant plant = ThingMaker.MakeThing(defs[index]) as Plant;
                ctx.Require(plant != null, "Unable to create actual original MO Plant");
                GenSpawn.Spawn(plant, cells[0], map, WipeMode.Vanish);
                plant.Growth = 0f;
                plant.HitPoints = plant.MaxHitPoints;
                plants.Add(plant);
                firstHarvest[index] = -1;
                firstMature[index] = -1;
                day10[index] = 0f;
                day20[index] = 0f;
                day50[index] = 0f;
                int noon = (12 - GenLocalDate.HourOfDay(map) + 24) % 24;
                simulatedStart = originalTick + noon * GenDate.TicksPerHour;
                Find.TickManager.DebugSetTicksGame(simulatedStart);
                SetOutdoor();
            }

            private void SetOutdoor()
            {
                map.Biome.constantOutdoorTemperature = temperature;
                foreach (Room room in map.regionGrid.AllRooms)
                    if (room.UsesOutdoorTemperature)
                        room.TempTracker.EqualizeTemperature();
                // Match the previously accepted real MO harvest fixture:
                // set the outdoor cell's room, not Thing.GetRoom().
                Room cellRoom = cells[0].GetRoom(map);
                if (cellRoom != null) cellRoom.Temperature = temperature;
                float actual = plants[0].AmbientTemperature;
                ctx.Require(Math.Abs(actual - temperature) < .4f,
                    "Single-cell temperature drift for " + plants[0].def.defName
                    + " target=" + F(temperature)
                    + " plantC=" + F(actual)
                    + " cellC=" + F(cells[0].GetTemperature(map))
                    + " roomC=" + (cellRoom == null ? "null" : F(cellRoom.Temperature))
                    + " gameTick=" + Find.TickManager.TicksGame);
            }

            public void Advance(int begin, int stop)
            {
                ctx.Require(deadline.Elapsed.TotalSeconds < 220,
                    "Bounded native cool growth observation ran too long");
                for (int observation = begin; observation < stop; observation++)
                {
                    Find.TickManager.DebugSetTicksGame(simulatedStart + observation * 2000);
                    map.skyManager.ForceSetCurSkyGlow(GenCelestial.CurCelestialSunGlow(map));
                    SetOutdoor();
                    int i = currentCrop;
                    Plant p = plants[0];
                    ctx.Require(p.Spawned && !p.Destroyed,
                        "Native MO crop destroyed in nonfreezing exposure: " + defs[i].defName);
                    p.TickLong();
                    if (firstHarvest[i] < 0 && p.HarvestableNow)
                        firstHarvest[i] = observation * 2000;
                    if (firstMature[i] < 0 && p.Growth >= .999f)
                        firstMature[i] = observation * 2000;
                    if (observation == 300) day10[i] = p.Growth;
                    if (observation == 600) day20[i] = p.Growth;
                    if (observation == Samples) day50[i] = p.Growth;
                }
            }

            public void Record(int i)
            {
                Plant p = plants[0];
                ctx.Require(p.HitPoints == p.MaxHitPoints,
                    "Unexpected HP loss under growth-only conditions");
                Log.Message("CCTO_MO_GROWTH_ROW def=" + defs[i].defName
                    + " tempC=" + F(temperature)
                    + " minGrowthC=" + F(defs[i].plant.minGrowthTemperature)
                    + " growDays=" + F(defs[i].plant.growDays)
                    + " fertility=" + F(map.fertilityGrid.FertilityAt(cells[0]))
                    + " firstHarvestTicks=" + firstHarvest[i]
                    + " firstMatureTicks=" + firstMature[i]
                    + " growth10=" + F(day10[i])
                    + " growth20=" + F(day20[i])
                    + " growth50=" + F(day50[i])
                    + " hp=" + p.HitPoints
                    + " lightClockOnly=true normalWholeMapTicks=false"
                    + " plantTickLongInvoked=true nativeGrowerJobs=false");
                completed++;
            }

            public void Complete()
            {
                ctx.Assert(completed == Temperatures.Length * Names.Length,
                    "Expected twelve individually observed MO crop-temperature conditions");
                Log.Message("CCTO_MO_GROWTH_COMPLETE rows=12 simulatedDays=50"
                    + " realMo=true agriculture=true nativePlantTickLong=true"
                    + " normalMapCalendar=false");
            }

            public void Dispose()
            {
                if (disposed) return;
                disposed = true;
                foreach (Plant plant in plants)
                    if (plant != null && !plant.Destroyed) plant.Destroy(DestroyMode.Vanish);
                for (int i = 0; i < cells.Count; i++)
                {
                    map.terrainGrid.SetTerrain(cells[i], terrains[i]);
                    map.snowGrid.SetDepth(cells[i], snow[i]);
                }
                map.Biome.constantOutdoorTemperature = originalOutdoor;
                Find.TickManager.DebugSetTicksGame(originalTick);
                map.skyManager.ForceSetCurSkyGlow(originalSky);
                Find.TickManager.CurTimeSpeed = originalSpeed;
            }

            private static string F(float f) =>
                f.ToString("R", CultureInfo.InvariantCulture);
        }
    }
}
