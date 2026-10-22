using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CropColdToleranceOverhaul
{
    // Only timing and sub-HP rounding remainder are stored.
    // Actual cold injury is ordinary visible Plant HitPoints damage.
    public sealed class ColdDamageMapComponent : MapComponent
    {
        private Dictionary<Plant, ColdDamageRecord> states;
        private List<ColdDamageRecord> records;
        private int lastPruneTick;

        public ColdDamageMapComponent(Map map) : base(map) { }

        public override void ExposeData()
        {
            base.ExposeData();
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                Prune();
                records = states == null ? new List<ColdDamageRecord>()
                    : new List<ColdDamageRecord>(states.Values);
            }
            Scribe_Collections.Look(ref records, "cctoColdDamageStates", LookMode.Deep);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                states = null;
                if (records != null)
                {
                    foreach (ColdDamageRecord record in records)
                    {
                        // References may resolve before spawning completes.
                        if (record == null || record.plant == null || record.plant.Destroyed
                            || record.lastTick < 0
                            || float.IsNaN(record.pendingHitPointDamage)
                            || record.pendingHitPointDamage < 0f
                            || record.pendingHitPointDamage >= 1f
                            || float.IsNaN(record.legacyDamage)
                            || record.legacyDamage < 0f || record.legacyDamage >= 1f
                            || ColdToleranceUtility.ExtensionFor(record.plant)?.HasColdDamageModel != true)
                            continue;
                        if (states == null) states = new Dictionary<Plant, ColdDamageRecord>();
                        states[record.plant] = record;
                    }
                }
                records = null;
            }
        }

        internal void UpdatePlant(Plant plant, ColdToleranceExtension extension, float temperature)
        {
            int now = Find.TickManager.TicksGame;
            ColdDamageRecord record = null;
            if (states != null)
            {
                states.TryGetValue(plant, out record);
                if (now - lastPruneTick >= 60000)
                {
                    Prune();
                    lastPruneTick = now;
                }
            }
            if (record == null && temperature >= extension.coldDamageStartTemperature)
                return;

            if (record == null)
            {
                if (states == null) states = new Dictionary<Plant, ColdDamageRecord>();
                // No retroactive cold exposure on the first observation.
                record = new ColdDamageRecord { plant = plant, lastTick = now, hpModelStored = true };
                states.Add(plant, record);
                return;
            }

            // Migrate unreleased V2 trial saves once: their old normalized cold
            // injury was not represented by HP. Never grant recovery.
            if (!record.hpModelStored)
            {
                record.hpModelStored = true;
                float oldLoss = record.legacyDamage * plant.MaxHitPoints;
                record.legacyDamage = 0f;
                ApplyHitPointLoss(plant, record, oldLoss);
                if (plant.Destroyed) { states.Remove(plant); return; }
            }

            int elapsed = Math.Max(0, Math.Min(2000, now - record.lastTick));
            if (elapsed == 0) return;
            record.lastTick = now;
            if (temperature >= extension.coldDamageStartTemperature)
            {
                // HP is never automatically restored; no need to retain a
                // warm-only observation. Fractional remainder is preserved
                // after frost even when this sample is warm.
                if (record.pendingHitPointDamage <= 0f) states.Remove(plant);
                return;
            }

            float dose = extension.coldDamageDegreeHoursToDie;
            if (extension.coldDamageLethalDoseVariation > 0f)
                dose *= ColdDamageProgress.LethalDoseMultiplier(
                    plant.thingIDNumber, extension.coldDamageLethalDoseVariation);
            float lostHp = ColdDamageProgress.HitPointLoss(
                temperature, extension.coldDamageStartTemperature, dose,
                elapsed, plant.MaxHitPoints);
            ApplyHitPointLoss(plant, record, lostHp);
            if (plant.Destroyed) states.Remove(plant);
        }

        private static void ApplyHitPointLoss(Plant plant, ColdDamageRecord record, float loss)
        {
            if (loss <= 0f || plant.Destroyed) return;
            float total = record.pendingHitPointDamage + loss;
            int hitPoints = (int)Math.Min(plant.HitPoints, Math.Floor(total));
            record.pendingHitPointDamage = total - (float)Math.Floor(total);
            if (hitPoints > 0)
                plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, hitPoints));
        }

        private void Prune()
        {
            if (states == null || states.Count == 0) return;
            List<Plant> stale = null;
            foreach (var item in states)
            {
                if (item.Key != null && !item.Key.Destroyed && item.Key.Spawned
                    && ColdToleranceUtility.ExtensionFor(item.Key)?.HasColdDamageModel == true)
                    continue;
                if (stale == null) stale = new List<Plant>();
                stale.Add(item.Key);
            }
            if (stale != null) foreach (Plant plant in stale) states.Remove(plant);
        }
    }

    public sealed class ColdDamageRecord : IExposable
    {
        public Plant plant;
        public int lastTick;
        public float pendingHitPointDamage;
        public bool hpModelStored;
        public float legacyDamage;

        public void ExposeData()
        {
            Scribe_References.Look(ref plant, "plant");
            Scribe_Values.Look(ref lastTick, "lastTick", 0);
            Scribe_Values.Look(ref pendingHitPointDamage, "pendingHitPointDamage", 0f);
            Scribe_Values.Look(ref hpModelStored, "hpModelStored", false);
            // Read old experimental normalized injury for one-time migration.
            // New saves always write zero here.
            Scribe_Values.Look(ref legacyDamage, "damage", 0f);
        }
    }
}
