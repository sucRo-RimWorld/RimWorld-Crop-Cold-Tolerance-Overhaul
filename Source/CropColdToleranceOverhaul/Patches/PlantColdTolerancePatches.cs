using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CropColdToleranceOverhaul.Patches
{
    [HarmonyPatch]
    internal static class PlantLeaflessTemperatureThreshPatch
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.PropertyGetter(typeof(Plant), "LeaflessTemperatureThresh");
        }

        [HarmonyPostfix]
        private static void Postfix(Plant __instance, ref float __result)
        {
            ColdToleranceExtension extension = ColdToleranceUtility.ExtensionFor(__instance);
            if (extension == null)
            {
                return;
            }

            if (extension.coldDormancy)
            {
                __result = __instance.def.plant.minGrowthTemperature;
            }
            else if (extension.HasColdDeathTemperature)
            {
                __result = extension.coldDeathTemperature;
            }
        }
    }

    [HarmonyPatch(typeof(Plant), "MakeLeafless")]
    internal static class PlantMakeLeaflessPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(Plant __instance, Plant.LeaflessCause cause, bool sendMessage)
        {
            if (cause != Plant.LeaflessCause.Cold)
            {
                return true;
            }

            ColdToleranceExtension extension = ColdToleranceUtility.ExtensionFor(__instance);
            if (extension == null)
            {
                return true;
            }

            float temperature = __instance.AmbientTemperature;

            if (extension.HasColdDeathTemperature
                && temperature < extension.coldDeathTemperature)
            {
                ColdToleranceUtility.KillFromCold(__instance, sendMessage);
                return false;
            }

            if (extension.coldDormancy
                && temperature < __instance.def.plant.minGrowthTemperature)
            {
                ColdToleranceUtility.EnterColdDormancy(__instance);
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Plant), "CheckMakeLeafless")]
    internal static class PlantCheckMakeLeaflessPatch
    {
        [HarmonyPostfix]
        private static void Postfix(Plant __instance)
        {
            if (__instance == null || __instance.Destroyed || !__instance.Spawned)
            {
                return;
            }

            ColdToleranceExtension extension = ColdToleranceUtility.ExtensionFor(__instance);
            if (extension == null)
            {
                return;
            }

            float temperature = __instance.AmbientTemperature;

            if (extension.HasColdDeathTemperature
                && temperature < extension.coldDeathTemperature)
            {
                ColdToleranceUtility.KillFromCold(__instance);
                return;
            }

            if (extension.coldDormancy
                && temperature < __instance.def.plant.minGrowthTemperature)
            {
                ColdToleranceUtility.EnterColdDormancy(__instance);
            }
        }
    }
}
