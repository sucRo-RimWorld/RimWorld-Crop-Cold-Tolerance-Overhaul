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
        private const float Onset = -2f;
        private const float Freezing = -27f;
        private const float FatalDegreeHours = 100f;
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
            // Use the real finalized production Def without replacing it with
            // test-only values. -27 C for 2000 ticks gives 0.20 full-HP
            // injury with production A=-2 and D=100; integer rounding is
            // checked within one HP in AssertDamage.
            ColdToleranceExtension damage = def.GetModExtension<ColdToleranceExtension>();
            Require(damage != null && damage.HasColdDamageModel
                && !damage.coldDormancy && !damage.HasColdDeathTemperature
                && Math.Abs(damage.coldDamageStartTemperature - Onset) < 0.001f
                && Math.Abs(damage.coldDamageDegreeHoursToDie - FatalDegreeHours) < 0.001f
                && damage.coldDamageLethalDoseVariation == 0f,
                "Loaded production potato V3 cold response differs from canonical values");
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

        internal static float DamageFraction(Plant plant)
        {
            return 1f - (float)plant.HitPoints / plant.MaxHitPoints;
        }

        private static void AssertDamage(float actual, float expected, string phase, Plant plant)
        {
            Require(Math.Abs(actual - expected) <= 1f / plant.MaxHitPoints + 0.001f,
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
            AssertDamage(DamageFraction(plant), 0f, "initial", plant);
            record.lastTick = Find.TickManager.TicksGame - Elapsed;
            Observe(plant);
            record = States(map)[plant];
            AssertDamage(DamageFraction(plant), ExpectedDamage, "pre-save", plant);
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
                + " damage=" + DamageFraction(plant).ToString("R", System.Globalization.CultureInfo.InvariantCulture)
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
            // Check what Scribe actually restored at PostLoadInit. Ordinary
            // game ticks may validly release a zero-remainder cold record once
            // the plant is warm; the lost HP itself is stored on the Plant.
            Require(ColdDamageScribeSnapshot.Restored
                && ColdDamageScribeSnapshot.PlantId == expectedId
                && ColdDamageScribeSnapshot.PlantNumber == number,
                "Scribe PostLoadInit did not restore this precise Plant record");
            AssertDamage(ColdDamageScribeSnapshot.Damage, ExpectedDamage,
                "at immediate Scribe PostLoadInit", plant);
            Require(ColdDamageScribeSnapshot.LastTick == tick,
                "Original observation tick not restored at Scribe PostLoadInit");
            Require(ColdDamageScribeSnapshot.PendingHitPointDamage == 0f,
                "The saved test injury should have no fractional HP remainder");
            Require(DamageFraction(plant) >= ExpectedDamage - (1f / plant.MaxHitPoints + 0.001f),
                "The loaded plant recovered lost cold HP after actual Scribe reload");
            Require(plant.def.GetModExtension<ColdToleranceExtension>()?.HasColdDamageModel == true,
                "Loaded crop lost its production V3 cold damage extension");

            var states = States(map);
            ColdDamageRecord record;
            bool retained = states.TryGetValue(plant, out record);
            if (retained)
            {
                Require(object.ReferenceEquals(record.plant, plant)
                    && record.lastTick >= tick,
                    "Reloaded live cold record lost its plant reference or clock");
            }
            else
            {
                Require(plant.AmbientTemperature >= Onset,
                    "A cold-exposed plant lost its record before a warm observation");
                Log.Message("CCTO_COLD_RELOAD_WARM_CLEANUP"
                    + " originalScribeRecordRestored=true"
                    + " liveRecordReleased=true"
                    + " coldHpPreserved=true");
            }

            // Then exercise *the reloaded instance*, not a replacement Plant.
            Room room = plant.GetRoom();
            Require(room != null && room.UsesOutdoorTemperature,
                "Expected original plant to remain outdoors");
            room.Temperature = Freezing;
            Require(Math.Abs(plant.AmbientTemperature - Freezing) < 0.25f,
                "Reload continuation did not set the actual plant ambient temperature");
            // Establish the new cold interval, whether the historical
            // Scribe record was retained or validly released after warmth.
            if (!retained)
            {
                Observe(plant);  // First cold observation; no retroactive dose.
                Require(States(map).TryGetValue(plant, out record),
                    "First renewed cold observation did not create a record");
            }
            else if (!record.lastObservationWasCold)
            {
                record.lastTick = Find.TickManager.TicksGame - Elapsed;
                Observe(plant);  // Warm-to-cold transition; no retroactive dose.
                Require(States(map).TryGetValue(plant, out record)
                    && record.lastObservationWasCold,
                    "Warm-to-cold observation did not reset the cold interval");
            }

            float before = DamageFraction(plant);
            record.lastTick = Find.TickManager.TicksGame - Elapsed;
            Observe(plant);
            Require(!plant.Destroyed, "Restored plant died before expected dose");
            Require(States(map).TryGetValue(plant, out ColdDamageRecord updated),
                "Continued cold observation dropped the saved record");
            AssertDamage(DamageFraction(plant), before + ExpectedDamage, "after continuing frost", plant);
            Log.Message("CCTO_COLD_RELOAD_RESULT passed=True"
                + " actualScribeGameReload=true plantReferenceRestored=true"
                + " tickRestored=true damageRestored=true"
                + " continuedDamage=" + DamageFraction(plant).ToString("R", System.Globalization.CultureInfo.InvariantCulture));
        }
    }

    // Take the exact Scribe PostLoadInit snapshot *before* the reloaded
    // game ticks, before game ticks can apply additional damage.
    [HarmonyPatch(typeof(ColdDamageMapComponent), "ExposeData")]
    public static class ColdDamageScribeSnapshot
    {
        public static bool Restored;
        public static float Damage;
        public static float PendingHitPointDamage;
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
                Damage = ColdDamageSaveProbe.DamageFraction(pair.Key);
                PendingHitPointDamage = record.pendingHitPointDamage;
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
