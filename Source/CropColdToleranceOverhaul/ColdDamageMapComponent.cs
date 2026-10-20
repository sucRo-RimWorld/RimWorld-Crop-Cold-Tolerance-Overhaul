using System;
using System.Collections.Generic;
using Verse;

namespace CropColdToleranceOverhaul
{
    // A sparse state store. No map-wide plant scan and no per-tick callback.
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
                        if (record == null || record.plant == null
                            || record.plant.Destroyed || !record.plant.Spawned
                            || record.damage <= 0f || record.damage >= 1f
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
                record = new ColdDamageRecord { plant = plant, lastTick = now - 2000 };
                states.Add(plant, record);
            }

            // Never charge a skipped season at a single sampled temperature.
            int elapsed = Math.Max(0, Math.Min(2000, now - record.lastTick));
            if (elapsed == 0) return;
            record.damage = ColdDamageProgress.Advance(
                record.damage, temperature, extension.coldDamageStartTemperature,
                extension.coldDamageDegreeHoursToDie,
                extension.coldDamageRecoveryPerHour, elapsed);
            record.lastTick = now;
            if (record.damage >= 1f)
            {
                states.Remove(plant);
                ColdToleranceUtility.KillFromCold(plant);
            }
            else if (record.damage <= 0f)
            {
                states.Remove(plant);
            }
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
        public float damage;
        public int lastTick;

        public void ExposeData()
        {
            Scribe_References.Look(ref plant, "plant");
            Scribe_Values.Look(ref damage, "damage", 0f);
            Scribe_Values.Look(ref lastTick, "lastTick", 0);
        }
    }
}
