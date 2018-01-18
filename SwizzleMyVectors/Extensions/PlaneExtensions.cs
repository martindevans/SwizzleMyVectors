﻿using System.Numerics;
using System.Runtime.CompilerServices;

namespace SwizzleMyVectors
{
    public static class PlaneExtensions
    {
        /// <summary>
        /// Returns a value indicating what side (positive/negative) of a plane a point is
        /// </summary>
        /// <param name="point">The point to check with</param>
        /// <param name="plane">The plane to check against</param>
        /// <returns>Greater than zero if on the positive side, less than zero if on the negative size, 0 otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Side(this Plane plane, Vector3 point)
        {
            return point.X * plane.Normal.X + point.Y * plane.Normal.Y + point.Z * plane.Normal.Z + plane.D;
        }

        /// <summary>
        /// Returns the perpendicular distance from a plane to a point
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <param name="plane">The place to check</param>
        /// <returns>The perpendicular distance from the point to the plane (signed)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceTo(this Plane plane, Vector3 point)
        {
            return Vector3.Dot(plane.Normal, point) + plane.D;
        }
    }
}
