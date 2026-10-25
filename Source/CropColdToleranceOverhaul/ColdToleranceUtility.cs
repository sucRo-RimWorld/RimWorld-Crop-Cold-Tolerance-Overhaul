using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CropColdToleranceOverhaul
{
    internal static class ColdToleranceUtility
    {
        private static readonly FieldInfo MadeLeaflessTickField =
            AccessTools.Field(typeof(Plant), "madeLeaflessTick");

        internal static ColdToleranceExtension ExtensionFor(Plant plant)
        {
            if (plant == null || plant.def == null)
            {
                return null;
            }

            return plant.def.GetModExtension<ColdToleranceExtension>();
        }

        internal static void EnterColdDormancy(Plant plant)
        {
            if (plant == null || plant.Destroyed || !plant.Spawned)
            {
                return;
            }

            bool wasLeafless = plant.LeaflessNow;
            MadeLeaflessTickField.SetValue(plant, Find.TickManager.TicksGame);

            if (!wasLeafless)
            {
                plant.Map.mapDrawer.MapMeshDirty(plant.Position, MapMeshFlagDefOf.Things);
            }
        }

        // A cold-damage HP kill can destroy the Plant before a message is sent.
        // Capture its identity, label and location before TakeDamage, then call
        // this only after confirming an actual fatal HP loss.
        internal static void NotifyCropDiedOfCold(
            string defName, string label, IntVec3 position, Map map)
        {
            if (MessagesRepeatAvoider.MessageShowAllowed(
                "MessagePlantDiedOfCold-" + defName, 240f))
            {
                Messages.Message(
                    "MessagePlantDiedOfCold".Translate(label),
                    new TargetInfo(position, map),
                    MessageTypeDefOf.NegativeEvent);
            }
        }

        internal static void KillFromCold(Plant plant, bool sendMessage = true)
        {
            if (plant == null || plant.Destroyed || !plant.Spawned)
            {
                return;
            }

            Map map = plant.Map;

            if (sendMessage && plant.IsCrop)
            {
                NotifyCropDiedOfCold(plant.def.defName,
                    plant.GetCustomLabelNoCount(false), plant.Position, map);
            }

            plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f));
        }
    }
}
