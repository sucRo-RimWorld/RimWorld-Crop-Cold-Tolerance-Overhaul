using System;
using System.Collections.Generic;
using Verse;

namespace CropColdToleranceOverhaul
{
    public sealed class ColdToleranceExtension : DefModExtension
    {
        public float coldDeathTemperature = float.NaN;
        public bool coldDormancy;
        // Opt-in exposure model; legacy temperatures remain unchanged.
        public float coldDamageStartTemperature = float.NaN;
        public float coldDamageDegreeHoursToDie = float.NaN;
        public float coldDamageRecoveryPerHour;

        public bool HasColdDamageModel
        {
            get
            {
                return !float.IsNaN(coldDamageStartTemperature)
                    && !float.IsInfinity(coldDamageStartTemperature)
                    && !float.IsNaN(coldDamageDegreeHoursToDie)
                    && !float.IsInfinity(coldDamageDegreeHoursToDie)
                    && coldDamageDegreeHoursToDie > 0f;
            }
        }

        public bool HasColdDeathTemperature
        {
            get { return !float.IsNaN(coldDeathTemperature); }
        }

        public override IEnumerable<string> ConfigErrors()
        {
            if (float.IsInfinity(coldDamageStartTemperature)
                || float.IsInfinity(coldDamageDegreeHoursToDie)
                || float.IsNaN(coldDamageRecoveryPerHour)
                || float.IsInfinity(coldDamageRecoveryPerHour)
                || coldDamageRecoveryPerHour < 0f)
            {
                yield return "[CCTO] Cold damage fields must be finite and recovery nonnegative.";
            }
            if (float.IsNaN(coldDamageStartTemperature)
                != float.IsNaN(coldDamageDegreeHoursToDie))
            {
                yield return "[CCTO] Damage start and lethal dose must be specified together.";
            }
            if (!float.IsNaN(coldDamageDegreeHoursToDie)
                && coldDamageDegreeHoursToDie <= 0f)
            {
                yield return "[CCTO] Lethal degree-hours must be positive.";
            }

            if (float.IsInfinity(coldDeathTemperature))
            {
                yield return "[CCTO] coldDeathTemperature must be a finite value.";
            }

            if (!coldDormancy && !HasColdDeathTemperature && !HasColdDamageModel)
            {
                yield return "[CCTO] coldDeathTemperature is required unless coldDormancy is enabled.";
            }
        }
    }
}
