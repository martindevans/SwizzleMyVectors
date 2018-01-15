using System.Runtime.CompilerServices;

namespace SwizzleMyVectors
{
    public static class SingleExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(this float value)
        {
            return float.IsNaN(value);
        }

        public static float Clamp(this float value, float min, float max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
    }
}
