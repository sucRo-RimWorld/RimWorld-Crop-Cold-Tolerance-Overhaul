using System;
namespace CropColdToleranceOverhaul
{
    internal static class ColdDamageProgress
    {
        // 2,500 ticks per game hour. One unit of damage is fatal.
        internal static float Advance(float prior, float temperature, float onset,
            float lethalDegreeHours, float recoveryPerHour, int elapsedTicks)
        {
            if (elapsedTicks <= 0) return prior;
            float hours = elapsedTicks / 2500f;
            if (temperature < onset)
                return Math.Min(1f, prior + (onset - temperature) * hours / lethalDegreeHours);
            return Math.Max(0f, prior - recoveryPerHour * hours);
        }
    }
}
