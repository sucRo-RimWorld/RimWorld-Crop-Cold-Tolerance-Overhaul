using System;
namespace CropColdToleranceOverhaul
{
    internal static class ColdDamageProgress
    {
        // Each plant's persistent Thing ID maps to one fixed percentile without RNG
        // or extra saved state. The bounded uniform dose spread is experimental;
        // species-specific population variance has NOT been calibrated.
        internal static float LethalDoseMultiplier(int thingId, float halfWidth)
        {
            if (halfWidth <= 0f) return 1f;

            // Stable 32-bit avalanche; avoids randomized string.GetHashCode and
            // Verse.Rand state. IDs are persisted by RimWorld across save/load.
            uint h = unchecked((uint)thingId);
            unchecked
            {
                h ^= h >> 16;
                h *= 0x7feb352du;
                h ^= h >> 15;
                h *= 0x846ca68bu;
                h ^= h >> 16;
            }

            // 24 high bits: deterministic rank strictly within (0,1).
            double percentile = ((h >> 8) + 0.5d) / 16777216d;
            return (float)(1d + halfWidth * (2d * percentile - 1d));
        }

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
