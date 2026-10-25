using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using CropColdToleranceOverhaul;
using RimWorks.Pickle;
using RimWorld;
using Verse;

namespace CropColdToleranceOverhaul.E2E
{
    [PickleSteps]
    public sealed class CctoMoBalanceSteps
    {
        private static readonly string[] Names = {
            "Plant_Rice", "Plant_Potato", "Plant_Corn",
            "DankPyon_Plant_Onions", "DankPyon_Plant_Carrots",
            "DankPyon_Plant_Cabbages", "DankPyon_Plant_Lentils",
            "DankPyon_Plant_Wheat", "DankPyon_Plant_Tomatoes",
            "DankPyon_Plant_Pumpkins"
        };
        private const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        private static readonly MethodInfo ColdCheck = typeof(Plant).GetMethod("CheckMakeLeafless", Private);
        private static readonly FieldInfo StateField = typeof(ColdDamageMapComponent).GetField("states", Private);

        [Then("original MO CCTO harvest balance telemetry is sampled")]
        public void Measure(PickleContext ctx)
        {
            ctx.Require(ColdCheck != null && StateField != null, "Missing cold observation entry");
            ctx.Require(LoadedModManager.RunningModsListForReading.Any(m =>
                string.Equals(m.PackageId, "dankpyon.medieval.overhaul", StringComparison.OrdinalIgnoreCase)),
                "Real Medieval Overhaul must be active; MO fixture is not accepted");
            ctx.Require(LoadedModManager.RunningModsListForReading.Any(m =>
                string.Equals(m.PackageId, "sucro.ancientmedievaljapan.core",
                    StringComparison.OrdinalIgnoreCase)),
                "Actual Agriculture/Grains patch provider must be active");
            // Fail on the exact loaded MO+Agriculture coefficients, not
            // merely successful activation of both package IDs.
            RequireAgricultureGrowth(ctx);
            Map map = Find.CurrentMap;
            ctx.Require(map != null, "Native map unavailable");
            IntVec3 cell = FindCell(ctx, map);
            Room room = cell.GetRoom(map);
            ctx.Require(room != null && room.UsesOutdoorTemperature, "Outdoor room required");
            float previous = room.Temperature;
            TerrainDef terrain = map.terrainGrid.TerrainAt(cell);
            map.terrainGrid.SetTerrain(cell, TerrainDefOf.Soil);
            int rows = 0;
            try
            {
                foreach (string name in Names)
                {
                    ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(name);
                    ctx.Require(def != null && def.plant != null &&
                        typeof(Plant).IsAssignableFrom(def.thingClass), "Loaded crop unavailable: " + name);
                    ColdToleranceExtension cold = def.GetModExtension<ColdToleranceExtension>();
                    ctx.Require(cold != null && cold.HasColdDamageModel &&
                        !cold.HasColdDeathTemperature && !cold.coldDormancy &&
                        cold.coldDamageLethalDoseVariation == 0f,
                        "Production V3 fields unavailable: " + name);
                    Observe(ctx, map, room, cell, def, cold, "warm", 25f, 0);
                    Observe(ctx, map, room, cell, def, cold, "frost5", -5f, 10);
                    Observe(ctx, map, room, cell, def, cold, "frost10", -10f, 5);
                    rows += 3;
                }
            }
            catch (Exception ex)
            {
                Log.Warning("CCTO_MO_BALANCE_SCENARIO_FAILURE detail=" + ex);
                throw;
            }
            finally
            {
                room.Temperature = previous;
                map.terrainGrid.SetTerrain(cell, terrain);
            }
            ctx.Assert(rows == 30, "Expected 30 actual planted crop observations");
            Log.Message("CCTO_MO_BALANCE_COMPLETE rows=30 provider=original-mo-1.6 naturalSeason=false actualPawnJobs=false");
        }

        private static void RequireAgricultureGrowth(PickleContext ctx)
        {
            string[] defs = {
                "DankPyon_Plant_Onions", "DankPyon_Plant_Carrots",
                "DankPyon_Plant_Lentils", "DankPyon_Plant_Tomatoes",
                "DankPyon_Plant_Cabbages", "DankPyon_Plant_Pumpkins",
                "DankPyon_Plant_Wheat"
            };
            float[] days = { 8f, 6f, 8f, 7f, 8f, 8f, 12f };
            int[] skills = { 3, 4, 2, 4, 4, 8, -1 };
            for (int i = 0; i < defs.Length; ++i)
            {
                ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(defs[i]);
                ctx.Require(def != null && def.plant != null,
                    "Missing actual MO agricultural plant " + defs[i]);
                ctx.Assert(Math.Abs(def.plant.growDays - days[i]) < 0.001f,
                    "Agriculture crop growth patch not loaded for " + defs[i]);
                if (skills[i] >= 0)
                    ctx.Assert(def.plant.sowMinSkill == skills[i],
                        "Agriculture/MO sow skill differs for " + defs[i]);
            }
        }

        private static void Observe(PickleContext ctx, Map map, Room room, IntVec3 cell,
            ThingDef def, ColdToleranceExtension cold, string phase, float target, int samples)
        {
            Plant plant = null;
            try
            {
                room.Temperature = 25f;
                plant = ThingMaker.MakeThing(def) as Plant;
                ctx.Require(plant != null, "Original Plant construction failed");
                GenSpawn.Spawn(plant, cell, map, WipeMode.Vanish);
                plant.HitPoints = plant.MaxHitPoints;
                plant.Growth = 1f;
                int baseline = StableYield(plant);
                ctx.Require(plant.Spawned && baseline > 0, "Baseline harvest unavailable: " + def.defName);
                if (samples > 0)
                {
                    room.Temperature = target;
                    ctx.Require(Math.Abs(plant.AmbientTemperature - target) < 0.25f,
                        "Actual plant temperature differs from controlled frost");
                    if (target >= cold.coldDamageStartTemperature)
                    {
                        // Onset is a strict below-threshold boundary.
                        // At / above it no state is required and HP stays whole.
                        ColdCheck.Invoke(plant, null);
                        ctx.Assert(plant.HitPoints == plant.MaxHitPoints,
                            "Native onset boundary incorrectly damaged plant: " + def.defName);
                    }
                    else
                    {
                        ColdCheck.Invoke(plant, null);
                        var states = States(map);
                        ColdDamageRecord state = null;
                        ctx.Require(states != null && states.TryGetValue(plant, out state),
                            "First below-onset observation did not initialize map state");
                        for (int i = 0; i < samples && !plant.Destroyed; ++i)
                        {
                            state.lastTick = Find.TickManager.TicksGame - 2000;
                            ColdCheck.Invoke(plant, null);
                            if (!plant.Destroyed)
                            {
                                states = States(map);
                                ctx.Require(states != null && states.TryGetValue(plant, out state),
                                    "Live frost sample lost map record");
                            }
                        }
                    }
                }
                int hp = plant.Destroyed ? 0 : plant.HitPoints;
                int harvest = plant.Destroyed ? 0 : StableYield(plant);
                ctx.Assert(hp <= plant.MaxHitPoints && harvest <= baseline,
                    "Cold should not increase plant HP or expected harvest");
                PlantProperties p = def.plant;
                Log.Message("CCTO_MO_BALANCE_ROW"
                    + " def=" + def.defName + " case=" + phase
                    + " ambientC=" + F(target) + " gameHours=" + F(samples * .8f)
                    + " hp=" + hp + " maxHp=" + plant.MaxHitPoints
                    + " yield=" + harvest + " baselineYield=" + baseline
                    + " growDays=" + F(p.growDays) + " minGrowthC=" + F(p.minGrowthTemperature)
                    + " fertilitySensitivity=" + F(p.fertilitySensitivity)
                    + " sowMinSkill=" + p.sowMinSkill
                    + " sowWorkUnits=" + F(Work(p, "sowWork"))
                    + " harvestWorkUnits=" + F(Work(p, "harvestWork"))
                    + " product=" + (p.harvestedThingDef == null ? "none" : p.harvestedThingDef.defName)
                    + " onsetC=" + F(cold.coldDamageStartTemperature)
                    + " lethalDegreeHours=" + F(cold.coldDamageDegreeHoursToDie)
                    + " destroyed=" + plant.Destroyed.ToString().ToLowerInvariant());
            }
            catch (Exception ex)
            {
                Log.Warning("CCTO_MO_BALANCE_FAILED def=" + def.defName + " case=" + phase + " detail=" + ex);
                throw;
            }
            finally
            {
                if (plant != null && !plant.Destroyed) plant.Destroy(DestroyMode.Vanish);
                room.Temperature = 25f;
            }
        }
        // Vanilla Plant.YieldNow uses stochastic rounding; compare both
        // observations from the SAME fixed Rand state without perturbing
        // global gameplay random numbers. The actual YieldNow engine remains
        // unmodified. This is reproducible native harvest telemetry.
        private static int StableYield(Plant plant)
        {
            Rand.PushState(1729);
            try
            {
                return plant.YieldNow();
            }
            finally
            {
                Rand.PopState();
            }
        }

        private static Dictionary<Plant, ColdDamageRecord> States(Map map)
        {
            return StateField.GetValue(map.GetComponent<ColdDamageMapComponent>())
                as Dictionary<Plant, ColdDamageRecord>;
        }
        private static float Work(PlantProperties p, string name)
        {
            FieldInfo field = p.GetType().GetField(name,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            object raw = field == null ? null : field.GetValue(p);
            return raw == null ? -1f : Convert.ToSingle(raw, CultureInfo.InvariantCulture);
        }
        private static string F(float value)
        {
            return value.ToString("R", CultureInfo.InvariantCulture);
        }
        private static IntVec3 FindCell(PickleContext ctx, Map map)
        {
            foreach (IntVec3 cell in map.AllCells)
            {
                if (!cell.Standable(map) || map.roofGrid.Roofed(cell)) continue;
                Room room = cell.GetRoom(map);
                if (room == null || !room.UsesOutdoorTemperature) continue;
                if (cell.GetThingList(map).Any(t => t is Plant || t is Pawn ||
                    t.def.category == ThingCategory.Building)) continue;
                return cell;
            }
            ctx.Require(false, "No safe native outdoor planting cell");
            return IntVec3.Invalid;
        }
    }
}
