using System.Numerics;
using System.Runtime.CompilerServices;

namespace SwizzleMyVectors
{
    // ReSharper disable once InconsistentNaming
    public static class Matrix4x4Extensions
    {
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Scale(this Matrix4x4 matrix)
        {
            return new Vector3(matrix.M11, matrix.M22, matrix.M33);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Translation(this Matrix4x4 matrix)
        {
            return new Vector3(matrix.M41, matrix.M42, matrix.M43);
        }
    }
}
