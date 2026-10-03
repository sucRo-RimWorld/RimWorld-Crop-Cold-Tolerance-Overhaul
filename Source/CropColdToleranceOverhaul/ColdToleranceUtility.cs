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

        internal static void KillFromCold(Plant plant, bool sendMessage = true)
        {
            if (plant == null || plant.Destroyed || !plant.Spawned)
            {
                return;
            }

            Map map = plant.Map;

            if (sendMessage
                && plant.IsCrop
                && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfCold-" + plant.def.defName, 240f))
            {
                Messages.Message(
                    "MessagePlantDiedOfCold".Translate(plant.GetCustomLabelNoCount(false)),
                    new TargetInfo(plant.Position, map),
                    MessageTypeDefOf.NegativeEvent);
            }

            plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f));
        }
    }
}
