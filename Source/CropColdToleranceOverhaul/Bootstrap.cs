using System.Linq;
using CropColdToleranceOverhaul.Compatibility;
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
            Harmony harmony = new Harmony(HarmonyId);
            harmony.PatchAll();
            NicePlantsMenuCompatibility.TryPatch(harmony);
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
                        string.Format(
                            "[CCTO] {0} has {1} ColdToleranceExtension entries. Only one is supported; dependent mods should replace the existing value instead of appending another extension.",
                            def.defName,
                            extensions.Length));
                }

                ColdToleranceExtension extension = extensions[0];

                if (def.plant == null)
                {
                    Log.Error(
                        string.Format(
                            "[CCTO] {0} has ColdToleranceExtension but is not a plant ThingDef.",
                            def.defName));
                    continue;
                }

                if (extension.HasColdDeathTemperature
                    && extension.coldDeathTemperature > def.plant.minGrowthTemperature)
                {
                    Log.Warning(
                        string.Format(
                            "[CCTO] {0}: coldDeathTemperature ({1}) is above minGrowthTemperature ({2}). This is allowed, but is probably unintended.",
                            def.defName,
                            extension.coldDeathTemperature,
                            def.plant.minGrowthTemperature));
                }
            }
        }
    }
}
