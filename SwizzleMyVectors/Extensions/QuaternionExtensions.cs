using System.Numerics;

namespace SwizzleMyVectors
{
    /// <summary>
    /// 
    /// </summary>
    public static class QuaternionExtensions
    {
        /// <summary>
        /// Checks if any member of the given quaternion is NaN
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool IsNaN(this Quaternion v)
        {
            return float.IsNaN(v.X) || float.IsNaN(v.Y) || float.IsNaN(v.Z) || float.IsNaN(v.W);
        }

        /// <summary>
        /// Normalizing lerp from a to b, shortest path/non-constant velocity
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Quaternion Nlerp(this Quaternion a, Quaternion b, float t)
        {
            Nlerp(a, ref b, t, out var q);
            return q;
        }

        /// <summary>
        /// Normalizing lerp from a to b, shortest path/non constant velocity
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static void Nlerp(this Quaternion a, ref Quaternion b, float t, out Quaternion result)
        {
            result = Quaternion.Normalize(Quaternion.Lerp(a, b, t));
        }
    }
}
