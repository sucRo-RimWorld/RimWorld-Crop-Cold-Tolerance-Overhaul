using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace CropColdToleranceOverhaul.Compatibility
{
    internal static class NicePlantsMenuCompatibility
    {
        private const string PackageId = "andromeda.niceplantsmenu";
        private const int DrawFailureLogKey = 1381255471;

        private static MethodInfo drawInfoBlockMethod;
        private static FieldInfo drawInfoForField;
        private static FieldInfo plantField;

        [ThreadStatic]
        private static bool injectingRows;

        internal static void TryPatch(Harmony harmony)
        {
            if (harmony == null || !ModsConfig.IsActive(PackageId))
            {
                return;
            }

            Type dialogType =
                AccessTools.TypeByName("NicePlantsMenu.Dialog_PlantBrowser");
            Type plantRecordType =
                AccessTools.TypeByName("NicePlantsMenu.PlantRecord");

            if (dialogType == null || plantRecordType == null)
            {
                Log.Warning(
                    "[CCTO] Nice Plants Menu is active, but its expected UI types "
                    + "were not found. CCTO compatibility was not applied.");
                return;
            }

            drawInfoBlockMethod =
                AccessTools.Method(dialogType, "DrawInfoBlock");
            drawInfoForField =
                AccessTools.Field(dialogType, "drawInfoFor");
            plantField =
                AccessTools.Field(plantRecordType, "plant");

            if (drawInfoBlockMethod == null
                || drawInfoForField == null
                || plantField == null)
            {
                Log.Warning(
                    "[CCTO] Nice Plants Menu is active, but its expected plant-info "
                    + "members were not found. CCTO compatibility was not applied.");
                return;
            }

            harmony.Patch(
                drawInfoBlockMethod,
                postfix: new HarmonyMethod(
                    typeof(NicePlantsMenuCompatibility),
                    nameof(DrawInfoBlockPostfix)));
        }

        private static void DrawInfoBlockPostfix(
            object __instance,
            ref float __0,
            float __1,
            float __2,
            string __3,
            Texture2D __4)
        {
            if (injectingRows
                || __instance == null
                || !string.Equals(
                    __3,
                    "NPM_GrowthTemperature".Translate().ToString(),
                    StringComparison.Ordinal))
            {
                return;
            }

            try
            {
                ThingDef plantDef = CurrentPlantDef(__instance);
                if (plantDef == null || plantDef.plant == null)
                {
                    return;
                }

                ColdToleranceExtension extension =
                    plantDef.GetModExtension<ColdToleranceExtension>();

                if (extension == null)
                {
                    return;
                }

                if (extension.coldDormancy)
                {
                    DrawAdditionalRow(
                        __instance,
                        ref __0,
                        __1,
                        __2,
                        __4,
                        "CCTO_DormancyTemperature".Translate().ToString(),
                        plantDef.plant.minGrowthTemperature.ToStringTemperature(),
                        "CCTO_DormancyTemperature_Desc");
                }

                if (extension.HasColdDeathTemperature)
                {
                    DrawAdditionalRow(
                        __instance,
                        ref __0,
                        __1,
                        __2,
                        __4,
                        "CCTO_ColdDeathTemperature".Translate().ToString(),
                        extension.coldDeathTemperature.ToStringTemperature(),
                        "CCTO_ColdDeathTemperature_Desc");
                }
            }
            catch (Exception ex)
            {
                Log.ErrorOnce(
                    "[CCTO] Nice Plants Menu compatibility failed while drawing plant "
                    + "cold-tolerance rows: " + ex,
                    DrawFailureLogKey);
            }
        }

        private static ThingDef CurrentPlantDef(object dialog)
        {
            object plantRecord = drawInfoForField.GetValue(dialog);
            if (plantRecord == null)
            {
                return null;
            }

            return plantField.GetValue(plantRecord) as ThingDef;
        }

        private static void DrawAdditionalRow(
            object dialog,
            ref float y,
            float x,
            float totalWidth,
            Texture2D icon,
            string label,
            string value,
            string descriptionKey)
        {
            Func<TaggedString> tooltip =
                delegate
                {
                    return descriptionKey.Translate();
                };

            object[] arguments =
            {
                y,
                x,
                totalWidth,
                label,
                icon,
                value,
                tooltip,
                null
            };

            injectingRows = true;
            try
            {
                drawInfoBlockMethod.Invoke(dialog, arguments);
                y = (float)arguments[0];
            }
            finally
            {
                injectingRows = false;
            }
        }
    }
}
