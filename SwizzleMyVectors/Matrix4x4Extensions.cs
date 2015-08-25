using System.Numerics;
using System.Runtime.CompilerServices;

namespace SwizzleMyVectors
{
    // ReSharper disable once InconsistentNaming
    public static class Matrix4x4Extensions
    {
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
