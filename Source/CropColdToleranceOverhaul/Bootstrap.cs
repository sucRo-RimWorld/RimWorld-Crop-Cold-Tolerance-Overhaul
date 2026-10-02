using System.Linq;
using HarmonyLib;
using Verse;

namespace CropColdToleranceOverhaul
{
    [StaticConstructorOnStartup]
    internal static class Bootstrap
    {
        private const string HarmonyId = "sucro.cropcoldtoleranceoverhaul";

        static Bootstrap()
        {
            new Harmony(HarmonyId).PatchAll();
            LongEventHandler.ExecuteWhenFinished(ValidateDefinitions);
        }

        private static void ValidateDefinitions()
        {
            foreach (ThingDef def in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                if (def.modExtensions == null)
                {
                    continue;
                }

                ColdToleranceExtension[] extensions =
                    def.modExtensions.OfType<ColdToleranceExtension>().ToArray();

                if (extensions.Length == 0)
                {
                    continue;
                }

                if (extensions.Length > 1)
                {
                    Log.Error(
                        $"[CCTO] {def.defName} has {extensions.Length} ColdToleranceExtension entries. " +
                        "Only one is supported; dependent mods should replace the existing value instead of appending another extension.");
                }

                ColdToleranceExtension extension = extensions[0];

                if (def.plant == null)
                {
                    Log.Error(
                        $"[CCTO] {def.defName} has ColdToleranceExtension but is not a plant ThingDef.");
                    continue;
                }

                if (extension.HasColdDeathTemperature
                    && extension.coldDeathTemperature > def.plant.minGrowthTemperature)
                {
                    Log.Warning(
                        $"[CCTO] {def.defName}: coldDeathTemperature " +
                        $"({extension.coldDeathTemperature}) is above minGrowthTemperature " +
                        $"({def.plant.minGrowthTemperature}). This is allowed, but is probably unintended.");
                }
            }
        }
    }
}
