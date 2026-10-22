using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CropColdToleranceOverhaul;
using HarmonyLib;
using RimWorks.Quickstarts;
using RimWorks.Quickstarts.Verification;
using RimWorld;
using UnityEngine;
using Verse;

namespace CropColdToleranceOverhaul.E2E
{
    // Developer-only two-process .rws probe. Never included in production Assemblies.
    // This must use the actual GameDataSaveLoader and real map MapComponent Scribe.
    [StaticConstructorOnStartup]
    public static class ColdDamageSaveProbeBootstrap
    {
        static ColdDamageSaveProbeBootstrap()
        {
            new Harmony("sucro.ccto.coldsave.native16").PatchAll(
                typeof(ColdDamageSaveProbeBootstrap).Assembly);
            ColdDamageSaveProbe.ConfigurePlantDef();
        }
    }

    public static class ColdDamageSaveProbe
    {
        public const string SaveName = "CCTO_ColdDamage_ActualSave16";
        private const string TestDef = "Plant_Potato";
        private const float Onset = -5f;
        private const float Freezing = -10f;
        private const float FatalDegreeHours = 20f;
        private const float ExpectedDamage = 0.2f;
        private const int Elapsed = 2000;
        private static readonly BindingFlags NonPublicInstance =
            BindingFlags.Instance | BindingFlags.NonPublic;
        private static readonly FieldInfo StatesField =
            typeof(ColdDamageMapComponent).GetField("states", NonPublicInstance);
        private static readonly MethodInfo ColdCheck =
            typeof(Plant).GetMethod("CheckMakeLeafless", NonPublicInstance);

        public static bool Prepared;
        public static bool Saved;

        public static void Require(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }

        public static ThingDef ConfigurePlantDef()
        {
            ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(TestDef);
            Require(def != null && def.plant != null, "Vanilla test potato Def unavailable");
            List<DefModExtension> extensions = def.modExtensions == null
                ? new List<DefModExtension>()
                : def.modExtensions.Where(x => !(x is ColdToleranceExtension)).ToList();
            ColdToleranceExtension damage = new ColdToleranceExtension
            {
                coldDamageStartTemperature = Onset,
                coldDamageDegreeHoursToDie = FatalDegreeHours,
                coldDamageRecoveryPerHour = 0.1f
            };
            Require(damage.HasColdDamageModel, "Test-only cold damage Def invalid");
            extensions.Add(damage);
            def.modExtensions = extensions;
            return def;
        }

        private static Dictionary<Plant, ColdDamageRecord> States(Map map)
        {
            Require(StatesField != null, "Cold damage state member missing");
            ColdDamageMapComponent component = map.GetComponent<ColdDamageMapComponent>();
            Require(component != null, "CCTO map component missing");
            var states = StatesField.GetValue(component) as
                Dictionary<Plant, ColdDamageRecord>;
            Require(states != null, "CCTO persisted plant state is missing");
            return states;
        }

        private static void Observe(Plant plant)
        {
            Require(ColdCheck != null, "Plant.CheckMakeLeafless unavailable");
            ColdCheck.Invoke(plant, null);
        }

        private static void AssertDamage(float actual, float expected, string phase)
        {
            Require(Math.Abs(actual - expected) < 0.002f,
                phase + ": expected damage " + expected + ", got " + actual);
        }

        public static void CreateSave()
        {
            ThingDef def = ConfigurePlantDef();
            Map map = Find.CurrentMap;
            Require(map != null, "Quickstart map missing");
            Plant plant = null;
            foreach (IntVec3 cell in map.AllCells)
            {
                Room room = cell.GetRoom(map);
                if (room == null || !room.UsesOutdoorTemperature
                    || map.roofGrid.Roofed(cell) || !cell.Standable(map)
                    || map.thingGrid.ThingsListAt(cell).Any(x => x is Plant))
                    continue;
                plant = ThingMaker.MakeThing(def) as Plant;
                Require(plant != null, "Failed to create real planted crop");
                GenSpawn.Spawn(plant, cell, map, WipeMode.Vanish);
                room.Temperature = Freezing;
                break;
            }
            Require(plant != null && plant.Spawned && !plant.Destroyed,
                "Failed to spawn real cold-test potato");
            Require(Math.Abs(plant.AmbientTemperature - Freezing) < 0.25f,
                "Save phase does not expose requested cold ambient temperature");

            Observe(plant);
            var states = States(map);
            Require(states.ContainsKey(plant), "First observed cold state was not created");
            ColdDamageRecord record = states[plant];
            AssertDamage(record.damage, 0f, "initial");
            record.lastTick = Find.TickManager.TicksGame - Elapsed;
            Observe(plant);
            record = States(map)[plant];
            AssertDamage(record.damage, ExpectedDamage, "pre-save");
            Require(!plant.Destroyed && record.lastTick == Find.TickManager.TicksGame,
                "Plant died or recorded observation time was not updated");

            string thingId = plant.ThingID;
            int lastTick = record.lastTick;
            int thingIDNumber = plant.thingIDNumber;
            Prepared = true;
            GameDataSaveLoader.SaveGame(SaveName);
            MethodInfo getPath = typeof(GenFilePaths).GetMethod(
                "FilePathForSavedGame", BindingFlags.Static | BindingFlags.Public
                | BindingFlags.NonPublic);
            Require(getPath != null, "Save path API missing");
            string file = (string)getPath.Invoke(null, new object[] { SaveName });
            Saved = System.IO.File.Exists(file)
                && new System.IO.FileInfo(file).Length > 1024;
            Require(Saved, "RimWorld real .rws save was not created");
            // Host extracts these exact reference values from the first-run log.
            Log.Message("CCTO_COLD_SAVE_CREATED saved=true"
                + " plantId=" + thingId
                + " plantNumber=" + thingIDNumber
                + " damage=" + record.damage.ToString("R", System.Globalization.CultureInfo.InvariantCulture)
                + " lastTick=" + lastTick);
        }

        public static void VerifyReload()
        {
            Require(ColdDamageLoadMarker.Loaded, "Game.LoadGame not invoked");
            Require(Current.Game != null && Find.CurrentMap != null,
                "No actual saved map loaded");
            string expectedId = Environment.GetEnvironmentVariable("AMJ_CCTO_EXPECT_ID");
            string expectedTick = Environment.GetEnvironmentVariable("AMJ_CCTO_EXPECT_TICK");
            string expectedNumber = Environment.GetEnvironmentVariable("AMJ_CCTO_EXPECT_NUMBER");
            int tick = 0;
            int number = 0;
            Require(!string.IsNullOrEmpty(expectedId)
                && int.TryParse(expectedTick, out tick)
                && int.TryParse(expectedNumber, out number),
                "Source phase persisted test metadata missing");
            Map map = Find.CurrentMap;
            Plant plant = map.listerThings.AllThings.OfType<Plant>()
                .SingleOrDefault(x => x.ThingID == expectedId);
            Require(plant != null && plant.Spawned && !plant.Destroyed,
                "The source plant was not restored by RimWorld Scribe");
            Require(plant.thingIDNumber == number,
                "Plant stable identity changed over real .rws reload");
            var states = States(map);
            ColdDamageRecord record;
            if (!states.TryGetValue(plant, out record))
            {
                Log.Message("CCTO_COLD_RELOAD_DEBUG dictionaryCount=" + states.Count
                    + " savedId=" + expectedId
                    + " keys=" + string.Join(", ", states.Keys
                        .Select(x => x == null ? "<null>" : x.ThingID).ToArray()));
                throw new InvalidOperationException(
                    "Saved damage record was not linked to the reloaded Plant");
            }
            Require(object.ReferenceEquals(record.plant, plant),
                "Cross-reference is not the actual reloaded plant instance");
            // The loaded game may run several hundred real ticks while
            // the Root_Play verification hook waits. Compare the exact
            // Scribe-phase snapshot to source, not later recovered damage.
            Require(ColdDamageScribeSnapshot.Restored
                && ColdDamageScribeSnapshot.PlantId == expectedId
                && ColdDamageScribeSnapshot.PlantNumber == number,
                "Scribe PostLoadInit did not restore this precise Plant record");
            AssertDamage(ColdDamageScribeSnapshot.Damage, ExpectedDamage,
                "at immediate Scribe PostLoadInit");
            Require(ColdDamageScribeSnapshot.LastTick == tick,
                "Original observation tick not restored at Scribe PostLoadInit");
            Require(record.lastTick >= tick && record.damage >= 0f
                && record.damage <= 1f,
                "Live loaded game produced invalid accumulated injury state");
            Require(plant.def.GetModExtension<ColdToleranceExtension>()?.HasColdDamageModel == true,
                "Loaded crop lost its opt-in cold damage extension");

            // Then exercise *the reloaded instance*, not a replacement Plant.
            Room room = plant.GetRoom();
            Require(room != null && room.UsesOutdoorTemperature,
                "Expected original plant to remain outdoors");
            room.Temperature = Freezing;
            Require(Math.Abs(plant.AmbientTemperature - Freezing) < 0.25f,
                "Reload continuation did not set the actual plant ambient temperature");
            float before = record.damage;
            record.lastTick = Find.TickManager.TicksGame - Elapsed;
            Observe(plant);
            Require(!plant.Destroyed, "Restored plant died before expected dose");
            Require(States(map).TryGetValue(plant, out ColdDamageRecord updated),
                "Continued cold observation dropped the saved record");
            AssertDamage(updated.damage, before + ExpectedDamage, "after continuing frost");
            Log.Message("CCTO_COLD_RELOAD_RESULT passed=True"
                + " actualScribeGameReload=true plantReferenceRestored=true"
                + " tickRestored=true damageRestored=true"
                + " continuedDamage=" + updated.damage.ToString("R", System.Globalization.CultureInfo.InvariantCulture));
        }
    }

    // Take the exact Scribe PostLoadInit snapshot *before* the reloaded
    // game ticks, so ambient warm recovery cannot contaminate persistence.
    [HarmonyPatch(typeof(ColdDamageMapComponent), "ExposeData")]
    public static class ColdDamageScribeSnapshot
    {
        public static bool Restored;
        public static float Damage;
        public static int LastTick;
        public static int PlantNumber;
        public static string PlantId;

        private static readonly FieldInfo StatesField =
            typeof(ColdDamageMapComponent).GetField("states",
                BindingFlags.Instance | BindingFlags.NonPublic);

        [HarmonyPostfix]
        public static void AfterScribe(ColdDamageMapComponent __instance)
        {
            if (Environment.GetEnvironmentVariable("AMJ_CCTO_SAVE_RELOAD") != "1"
                || Scribe.mode != LoadSaveMode.PostLoadInit) return;
            string id = Environment.GetEnvironmentVariable("AMJ_CCTO_EXPECT_ID");
            var states = StatesField.GetValue(__instance)
                as Dictionary<Plant, ColdDamageRecord>;
            if (states == null || string.IsNullOrEmpty(id)) return;
            foreach (var pair in states)
            {
                if (pair.Key == null || pair.Key.ThingID != id) continue;
                ColdDamageRecord record = pair.Value;
                if (record == null || !object.ReferenceEquals(record.plant, pair.Key))
                    continue;
                Restored = true;
                Damage = record.damage;
                LastTick = record.lastTick;
                PlantId = pair.Key.ThingID;
                PlantNumber = pair.Key.thingIDNumber;
                Log.Message("CCTO_COLD_SCRIBE_SNAPSHOT restored=true"
                    + " damage=" + Damage + " lastTick=" + LastTick
                    + " plant=" + PlantId);
                return;
            }
        }
    }

    public sealed class CctoColdDamageActualSaveQuickstart : AbstractQuickstart
    {
        public override TaggedString description =>
            "Test-only real Scribe save of a cold-damaged living potato";
        public override int mapSize => 50;
        public override string seed => "CCTO-Real-Cold-Injury-Save-16";

        public override void PostLoaded()
        {
            ColdDamageSaveProbe.CreateSave();
        }

        public override QuickstartVerification Verify()
        {
            var checks = new QuickstartVerification();
            checks.Assert("actual cold injury .rws created",
                () => ColdDamageSaveProbe.Prepared && ColdDamageSaveProbe.Saved);
            return checks;
        }
    }

    [HarmonyPatch(typeof(Root_Entry), "Update")]
    public static class ColdDamageEntryLoadHook
    {
        private static int frames;
        private static bool queued;

        [HarmonyPostfix]
        public static void Postfix()
        {
            if (queued || Environment.GetEnvironmentVariable("AMJ_CCTO_SAVE_RELOAD") != "1"
                || ++frames < 180) return;
            queued = true;
            Log.Message("CCTO_COLD_RELOAD_BEGIN actual_saved_game=true");
            LongEventHandler.QueueLongEvent(() =>
            {
                Current.Game = new Game
                {
                    InitData = new GameInitData { gameToLoad = ColdDamageSaveProbe.SaveName }
                };
            }, "Play", "LoadingLongEvent", true, null);
        }
    }

    [HarmonyPatch(typeof(Game), "LoadGame")]
    public static class ColdDamageLoadMarker
    {
        public static bool Loaded;
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (Environment.GetEnvironmentVariable("AMJ_CCTO_SAVE_RELOAD") == "1")
                Loaded = true;
        }
    }

    [HarmonyPatch(typeof(Root_Play), "Update")]
    public static class ColdDamagePlayVerifyHook
    {
        private static int frames;
        private static bool finished;
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (finished || !ColdDamageLoadMarker.Loaded
                || Environment.GetEnvironmentVariable("AMJ_CCTO_SAVE_RELOAD") != "1"
                || ++frames < 180) return;
            finished = true;
            try
            {
                ColdDamageSaveProbe.VerifyReload();
            }
            catch (Exception error)
            {
                Log.Error("CCTO_COLD_RELOAD_RESULT passed=False " + error);
            }
            Application.Quit();
        }
    }
}
