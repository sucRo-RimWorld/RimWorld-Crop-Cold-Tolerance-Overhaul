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

        // Two thousand five hundred game ticks per hour; cold HP loss does not heal.
        internal static float HitPointLoss(float temperature, float onset,
            float lethalDegreeHours, int elapsedTicks, int maxHitPoints)
        {
            if (elapsedTicks <= 0 || temperature >= onset || maxHitPoints <= 0)
                return 0f;
            double degreeHours = (double)(onset - temperature) * elapsedTicks / 2500d;
            double hp = degreeHours * maxHitPoints / lethalDegreeHours;
            return (float)Math.Min(maxHitPoints, hp);
        }
    }
}
