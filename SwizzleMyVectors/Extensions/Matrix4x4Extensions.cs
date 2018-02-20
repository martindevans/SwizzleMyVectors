using System.Numerics;
using System.Runtime.CompilerServices;

namespace SwizzleMyVectors
{
    // ReSharper disable once InconsistentNaming
    public static class Matrix4x4Extensions
    {
        /// <summary>
        /// Determine is any element of this matrix is NaN
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN(this Matrix4x4 matrix)
        {
            return matrix.M11.IsNaN()
                || matrix.M12.IsNaN()
                || matrix.M13.IsNaN()
                || matrix.M14.IsNaN()
                || matrix.M21.IsNaN()
                || matrix.M22.IsNaN()
                || matrix.M23.IsNaN()
                || matrix.M24.IsNaN()
                || matrix.M31.IsNaN()
                || matrix.M32.IsNaN()
                || matrix.M33.IsNaN()
                || matrix.M34.IsNaN()
                || matrix.M41.IsNaN()
                || matrix.M42.IsNaN()
                || matrix.M43.IsNaN()
                || matrix.M44.IsNaN();
        }
    }
}
