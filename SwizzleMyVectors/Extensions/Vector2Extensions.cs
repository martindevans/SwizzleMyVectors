using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace SwizzleMyVectors
{
    /// <summary>
    /// A static class which contains extension methods for the Vector2 class.
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// Determines whether this Vector2 contains any components which are not a number.
        /// </summary>
        /// <param name="v"></param>
        /// <returns>
        /// 	<c>true</c> if either X or Y are NaN; otherwise, <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static bool IsNaN(this Vector2 v)
        {
            return float.IsNaN(v.X) || float.IsNaN(v.Y);
        }

        /// <summary>
        /// Creates a vector perpendicular to this vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector2 PerpendicularRight(this Vector2 v)
        {
            return new Vector2(v.Y, -v.X);
        }

        /// <summary>
        /// Creates a vector perpendicular to this vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector2 PerpendicularLeft(this Vector2 v)
        {
            return new Vector2(-v.Y, v.X);
        }

        /// <summary>
        /// Calculates the perpendicular dot product of this vector and another.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static float Cross(this Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        /// <summary>
        /// Determines the length of a vector using the manhattan length function
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static float ManhattanLength(this Vector2 v)
        {
            return Math.Abs(v.X) + Math.Abs(v.Y);
        }

        /// <summary>
        /// Returns the largest element in the vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static float LargestElement(this Vector2 v)
        {
            return Math.Max(v.X, v.Y);
        }

        /// <summary>
        /// Calculates the area of an irregular polygon. If the polygon is anticlockwise wound the area will be negative
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [Pure] public static float Area([NotNull] this IReadOnlyList<Vector2> v)
        {
            var area = 0f;

            var previous = v.Count - 1;
            for (var i = 0; i < v.Count; i++)
            {
                area += (v[i].X + v[previous].X) * (v[i].Y - v[previous].Y);
                previous = i;
            }

            return -area / 2;
        }

        /// <summary>
        /// Get the area of a set of points defining a polygon. Returns negative values for anticlockwise shapes. Correctly handles concave shapes.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        [Pure] public static float Area([NotNull] this IEnumerable<Vector2> points)
        {
            //This comment is a clever LINQ alternative... which is horribly inefficient since it enumerates the points enumeration twice
            //return points.Zip(points.Skip(1).Append(points), (a, b) => (b.X + a.X) * (b.Y - a.Y)).Sum() / -2;

            var area = 0f;

            //Calculate area of each part (matching point with previous)
            Vector2? first = null;
            Vector2? previous = null;
            foreach (var point in points)
            {
                //Save the first vector in the enumerable
                if (!first.HasValue)
                    first = point;

                if (previous.HasValue)
                    area += (point.X + previous.Value.X) * (point.Y - previous.Value.Y);

                previous = point;
            }

            //We have not yet joined the last and first points
            //We saved the first point, and previous must be the last point so we can do it now!
            if (first.HasValue)
                area += (first.Value.X + previous.Value.X) * (first.Value.Y - previous.Value.Y);

            return -area / 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        [Pure] public static bool IsConvex([NotNull] this IReadOnlyList<Vector2> v, float epsilon = float.Epsilon)
        {
            var sign = 0;
            var first = true;
            for (var i = 0; i < v.Count; i++)
            {
                var p = v[i];
                var p1 = v[(i + 1) % v.Count];
                var p2 = v[(i + 2) % v.Count];

                var d1 = p1 - p;
                var d2 = p2 - p1;

                var crossProduct = d1.X * d2.Y - d1.Y * d2.X;
                var crossProductSign = crossProduct > epsilon ? 1 : crossProduct < -epsilon ? -1 : 0;

                if (crossProductSign == 0)
                    continue;

                if (first)
                {
                    sign = crossProductSign;
                    first = false;
                }
                else if (crossProductSign != sign)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Rearrange elements in a vector2
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector2 XX(this Vector2 v)
        {
            return new Vector2(v.X, v.X);
        }

        /// <summary>
        /// Rearrange elements in a vector2
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector2 XY(this Vector2 v)
        {
            return new Vector2(v.X, v.Y);
        }

        /// <summary>
        /// Rearrange elements in a vector2
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector2 YX(this Vector2 v)
        {
            return new Vector2(v.Y, v.X);
        }

        /// <summary>
        /// Rearrange elements in a vector2
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector2 YY(this Vector2 v)
        {
            return new Vector2(v.Y, v.Y);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 _XY(this Vector2 v, float x)
        {
            return new Vector3(x, v.X, v.Y);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 X_Y(this Vector2 v, float y)
        {
            return new Vector3(v.X, y, v.Y);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 XY_(this Vector2 v, float z)
        {
            return new Vector3(v.X, v.Y, z);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 _XX(this Vector2 v, float x)
        {
            return new Vector3(x, v.X, v.X);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 X_X(this Vector2 v, float y)
        {
            return new Vector3(v.X, y, v.X);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 XX_(this Vector2 v, float z)
        {
            return new Vector3(v.X, v.X, z);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 _YY(this Vector2 v, float x)
        {
            return new Vector3(x, v.Y, v.Y);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 Y_Y(this Vector2 v, float y)
        {
            return new Vector3(v.Y, y, v.Y);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 YY_(this Vector2 v, float z)
        {
            return new Vector3(v.Y, v.Y, z);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 XXX(this Vector2 v)
        {
            return new Vector3(v.X, v.X, v.X);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 XXY(this Vector2 v)
        {
            return new Vector3(v.X, v.X, v.Y);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 XYX(this Vector2 v)
        {
            return new Vector3(v.X, v.Y, v.X);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 XYY(this Vector2 v)
        {
            return new Vector3(v.X, v.Y, v.Y);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 YXX(this Vector2 v)
        {
            return new Vector3(v.Y, v.X, v.X);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 YXY(this Vector2 v)
        {
            return new Vector3(v.Y, v.X, v.Y);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 YYX(this Vector2 v)
        {
            return new Vector3(v.Y, v.Y, v.X);
        }

        /// <summary>
        /// Create a new vector3
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining), Pure]
        public static Vector3 YYY(this Vector2 v)
        {
            return new Vector3(v.Y, v.Y, v.Y);
        }
    }
}
