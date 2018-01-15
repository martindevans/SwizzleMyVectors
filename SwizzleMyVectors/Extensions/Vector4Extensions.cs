using System.Numerics;

namespace SwizzleMyVectors
{
    public static class Vector4Extensions
    {
        /// <summary>
        /// Determines whether this Vector4 contains any components which are not a number.
        /// </summary>
        /// <param name="v"></param>
        /// <returns>
        /// 	<c>true</c> if either X or Y or Z or W are NaN; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNaN(this Vector4 v)
        {
            return float.IsNaN(v.X) || float.IsNaN(v.Y) || float.IsNaN(v.Z) || float.IsNaN(v.W);
        }
    }
}
