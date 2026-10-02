using HarmonyLib;
using RimWorld;
using Verse;

namespace CropColdToleranceOverhaul
{
    internal static class ColdToleranceUtility
    {
        private static readonly AccessTools.FieldRef<Plant, int> MadeLeaflessTick =
            AccessTools.FieldRefAccess<Plant, int>("madeLeaflessTick");

        internal static ColdToleranceExtension ExtensionFor(Plant plant)
        {
            return plant?.def?.GetModExtension<ColdToleranceExtension>();
        }

        internal static void EnterColdDormancy(Plant plant)
        {
            if (plant == null || plant.Destroyed || !plant.Spawned)
            {
                return;
            }

            bool wasLeafless = plant.LeaflessNow;
            MadeLeaflessTick(plant) = Find.TickManager.TicksGame;

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
                    "MessagePlantDiedOfCold".Translate(plant.GetCustomLabelNoCount(includeHp: false)),
                    new TargetInfo(plant.Position, map),
                    MessageTypeDefOf.NegativeEvent);
            }

            plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f));
        }
    }
}
