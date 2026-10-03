using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CropColdToleranceOverhaul.Patches
{
    [HarmonyPatch(typeof(ThingDef), "SpecialDisplayStats")]
    internal static class ThingDefSpecialDisplayStatsPatch
    {
        [HarmonyPostfix]
        private static void Postfix(
            ThingDef __instance,
            ref IEnumerable<StatDrawEntry> __result)
        {
            if (__instance == null)
            {
                return;
            }

            ColdToleranceExtension extension =
                __instance.GetModExtension<ColdToleranceExtension>();

            if (extension == null || __instance.plant == null)
            {
                return;
            }

            __result = AppendCctoStats(
                __result,
                extension,
                __instance.plant.minGrowthTemperature);
        }

        private static IEnumerable<StatDrawEntry> AppendCctoStats(
            IEnumerable<StatDrawEntry> original,
            ColdToleranceExtension extension,
            float dormancyTemperature)
        {
            foreach (StatDrawEntry entry in original)
            {
                yield return entry;
            }

            if (extension.coldDormancy)
            {
                yield return new StatDrawEntry(
                    StatCategoryDefOf.Basics,
                    "CCTO_DormancyTemperature".Translate(),
                    dormancyTemperature.ToStringTemperature(),
                    "CCTO_DormancyTemperature_Desc".Translate(),
                    4151);
            }

            if (extension.HasColdDeathTemperature)
            {
                yield return new StatDrawEntry(
                    StatCategoryDefOf.Basics,
                    "CCTO_ColdDeathTemperature".Translate(),
                    extension.coldDeathTemperature.ToStringTemperature(),
                    "CCTO_ColdDeathTemperature_Desc".Translate(),
                    4150);
            }
        }
    }
}
