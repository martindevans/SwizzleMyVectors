﻿using System;
using System.Numerics;

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
        public static float Side(this Plane plane, Vector3 point)
        {
            return point.X * plane.Normal.X + point.Y * plane.Normal.Y + point.Z * plane.Normal.Z + plane.D;
        }

        /// <summary>
        /// Returns the perpendicular distance from a point to a plane
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <param name="plane">The place to check</param>
        /// <returns>The perpendicular distance from the point to the plane</returns>
        public static float PerpendicularDistance(this Plane plane, Vector3 point)
        {
            // dist = (ax + by + cz + d) / sqrt(a*a + b*b + c*c)
            return (float)Math.Abs((plane.Normal.X * point.X + plane.Normal.Y * point.Y + plane.Normal.Z * point.Z)
                                    / Math.Sqrt(plane.Normal.X * plane.Normal.X + plane.Normal.Y * plane.Normal.Y + plane.Normal.Z * plane.Normal.Z));
        }
    }
}
