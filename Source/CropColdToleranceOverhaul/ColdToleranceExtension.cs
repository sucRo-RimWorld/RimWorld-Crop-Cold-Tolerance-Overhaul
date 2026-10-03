using System;
using System.Collections.Generic;
using Verse;

namespace CropColdToleranceOverhaul
{
    public sealed class ColdToleranceExtension : DefModExtension
    {
        public float coldDeathTemperature = float.NaN;
        public bool coldDormancy;

        public bool HasColdDeathTemperature
        {
            get { return !float.IsNaN(coldDeathTemperature); }
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (float.IsInfinity(coldDeathTemperature))
            {
                yield return "[CCTO] coldDeathTemperature must be a finite value.";
            }

            if (!coldDormancy && !HasColdDeathTemperature)
            {
                yield return "[CCTO] coldDeathTemperature is required unless coldDormancy is enabled.";
            }
        }
    }
}
