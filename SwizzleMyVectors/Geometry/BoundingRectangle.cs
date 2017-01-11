using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Numerics;

namespace SwizzleMyVectors.Geometry
{
    public struct BoundingRectangle
    {
        /// <summary>
        /// Specifies the total number of corners (4) in the BoundingRectangle.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const int CornerCount = 4;
        /// <summary>
        /// The minimum point the BoundingBox contains.
        /// </summary>
        public Vector2 Min;
        /// <summary>
        /// The maximum point the BoundingBox contains.
        /// </summary>
        public Vector2 Max;

        public Vector2 Extent
        {
            get { return Max - Min; }
        }

        /// <summary>
        /// Creates an instance of BoundingBox.
        /// </summary>
        /// <param name="min">The minimum point the BoundingRectangle includes.</param><param name="max">The maximum point the BoundingRectangle includes.</param>
        public BoundingRectangle(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        #region static factories
        /// <summary>
        /// Creates the smallest BoundingRectangle that contains the two specified BoundingRectangle instances.
        /// </summary>
        /// <param name="original">One of the BoundingRectangles to contain.</param><param name="additional">One of the BoundingRectangles to contain.</param>
        public static BoundingRectangle CreateMerged(BoundingRectangle original, BoundingRectangle additional)
        {
            BoundingRectangle result;
            CreateMerged(ref original, ref additional, out result);
            return result;
        }

        /// <summary>
        /// Creates the smallest BoundingRectangle that contains the two specified BoundingRectangle instances.
        /// </summary>
        /// <param name="original">One of the BoundingRectangle instances to contain.</param><param name="additional">One of the BoundingRectangle instances to contain.</param><param name="result">[OutAttribute] The created BoundingRectangle.</param>
        public static void CreateMerged(ref BoundingRectangle original, ref BoundingRectangle additional, out BoundingRectangle result)
        {
            result = new BoundingRectangle(
                Vector2.Min(original.Min, additional.Min),
                Vector2.Max(original.Max, additional.Max)
            );
        }

        /// <summary>
        /// Creates the smallest BoundingBox that will contain a group of points.
        /// </summary>
        /// <param name="points">A list of points the BoundingBox should contain.</param>
        public static BoundingRectangle CreateFromPoints(IEnumerable<Vector2> points)
        {
            var min = new Vector2(float.MaxValue);
            var max = new Vector2(float.MinValue);

            foreach (var point in points)
            {
                min = Vector2.Min(min, point);
                max = Vector2.Max(max, point);
            }

            return new BoundingRectangle(min, max);

        }
        #endregion

        /// <summary>
        /// Determines whether two instances of BoundingBox are equal.
        /// </summary>
        /// <param name="a">BoundingBox to compare.</param><param name="b">BoundingBox to compare.</param>
        [Pure]
        public static bool operator ==(BoundingRectangle a, BoundingRectangle b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether two instances of BoundingBox are not equal.
        /// </summary>
        /// <param name="a">The object to the left of the inequality operator.</param><param name="b">The object to the right of the inequality operator.</param>
        [Pure]
        public static bool operator !=(BoundingRectangle a, BoundingRectangle b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Expand bounding rectangle by distance / 2 at min and max (total of distance)
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        [Pure]
        public BoundingRectangle Inflate(float distance)
        {
            var self = this;
            Inflate(ref self, distance);
            return self;
        }

        /// <summary>
        /// Expand bounding rectangle by distance / 2 at min and max (total of distance)
        /// </summary>
        /// <param name="rectangle">The rectangle to mutate</param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static void Inflate(ref BoundingRectangle rectangle, float distance)
        {
            var min = rectangle.Min - new Vector2(distance / 2);
            var max = rectangle.Max + new Vector2(distance / 2);

            if (distance < 0)
            {
                if (Vector2.Min(min, max) != min)
                    throw new ArgumentOutOfRangeException("distance", "Distance specified to inflate rectangle is a negative value larger than the total diagonal size of the rectangle (doing this would invert the rectangle!");
            }

            rectangle.Min = min;
            rectangle.Max = max;
        }

        /// <summary>
        /// Gets an array of points that make up the corners of the BoundingRectangle.
        /// </summary>
        [Pure]
        public Vector2[] GetCorners()
        {
            var arr = new Vector2[CornerCount];
            GetCorners(arr);
            return arr;
        }

        /// <summary>
        /// Gets the array of points that make up the corners of the BoundingRectangle.
        /// </summary>
        /// <param name="corners">An existing array of at least 4 Vector2 points where the corners of the BoundingRectangle are written.</param>
        public void GetCorners(Vector2[] corners)
        {
            if (corners.Length < CornerCount)
                throw new ArgumentException("Array too small", nameof(corners));

            GetCorners(out corners[0], out corners[1], out corners[2], out corners[3]);
        }

        public void GetCorners(out Vector2 a, out Vector2 b, out Vector2 c, out Vector2 d)
        {
            a = Min;
            b = new Vector2(Min.X, Max.Y);
            c = Max;
            d = new Vector2(Max.X, Min.Y);
        }

        /// <summary>
        /// Determines whether two instances of BoundingBox are equal.
        /// </summary>
        /// <param name="other">The BoundingBox to compare with the current BoundingBox.</param>
        [Pure]
        public bool Equals(BoundingRectangle other)
        {
            return Min.Equals(other.Min) && Max.Equals(other.Max);
        }

        /// <summary>
        /// Determines whether two instances of BoundingBox are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current BoundingBox.</param>
        [Pure]
        public override bool Equals(object obj)
        {
            return obj is BoundingRectangle && Equals((BoundingRectangle)obj);
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        [Pure]
        public override int GetHashCode()
        {
// This seems ugly, but it's inline with how MS designed their vector types!
// ReSharper disable NonReadonlyFieldInGetHashCode

            int hash = 17;
            hash = hash * 31 + Min.GetHashCode();
            hash = hash * 31 + Max.GetHashCode();
            return hash;

// ReSharper restore NonReadonlyFieldInGetHashCode
        }

        /// <summary>
        /// Returns a String that represents the current BoundingRectangle.
        /// </summary>
        [Pure]
        public override string ToString()
        {
            return string.Format("Min:{0},Max:{1}", Min, Max);
        }

        /// <summary>
        /// Calculate the area of this bounding rectangle
        /// </summary>
        /// <returns></returns>
        [Pure]
        public float Area()
        {
            var sz = (Max - Min);
            return sz.X * sz.Y;
        }

        #region intersection
        /// <summary>
        /// Checks whether the current BoundingRectangle intersects another BoundingRectangle.
        /// </summary>
        /// <param name="box">The BoundingBox to check for intersection with.</param>
        [Pure]
        public bool Intersects(BoundingRectangle box)
        {
            bool result;
            Intersects(ref box, out result);
            return result;
        }

        /// <summary>
        /// Calculate the intersection of this rectangle and another
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        [Pure]
        public BoundingRectangle? Intersection(BoundingRectangle box)
        {
            return Intersection(ref box);
        }

        /// <summary>
        /// Calculate the intersection of this rectangle and another
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        [Pure]
        public BoundingRectangle? Intersection(ref BoundingRectangle box)
        {
            var min = Vector2.Max(Min, box.Min);
            var max = Vector2.Min(Max, box.Max);

            //Check if the min of the overlap is not the min, this means the overlap is inverted and not actually an overlap
            if (Vector2.Min(min, max) != min)
                return null;

            return new BoundingRectangle(min, max);
        }

        /// <summary>
        /// Checks whether the current BoundingRectangle intersects another BoundingRectangle.
        /// </summary>
        /// <param name="box">The BoundingRectangle to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingRectangle instances intersect; false otherwise.</param>
        public void Intersects(ref BoundingRectangle box, out bool result)
        {
            result = box.Min.X < Max.X && box.Max.X > Min.X
                  && box.Min.Y < Max.Y && box.Max.Y > Min.Y;
        }

        [Pure]
        public LinesIntersection2? Intersects(LineSegment2 segment)
        {
            Vector2 a, b, c, d;
            GetCorners(out a, out b, out c, out d);

            var abi = segment.Intersects(new LineSegment2(a, b));
            var bci = segment.Intersects(new LineSegment2(b, c));
            var cdi = segment.Intersects(new LineSegment2(c, d));
            var dai = segment.Intersects(new LineSegment2(d, a));

            var min = abi;

            if (bci.HasValue && (!min.HasValue || bci.Value.DistanceAlongA < min.Value.DistanceAlongA))
                min = bci;
            if (cdi.HasValue && (!min.HasValue || cdi.Value.DistanceAlongA < min.Value.DistanceAlongA))
                min = cdi;
            if (dai.HasValue && (!min.HasValue || dai.Value.DistanceAlongA < min.Value.DistanceAlongA))
                min = dai;

            return min;
        }
        #endregion

        #region containment
        /// <summary>
        /// Gets whether or not the provided coordinates lie within the bounds of this <see cref="BoundingRectangle"/>.
        /// </summary>
        /// <param name="x">The x coordinate of the point to check for containment.</param>
        /// <param name="y">The y coordinate of the point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="BoundingRectangle"/>; <c>false</c> otherwise.</returns>
        [Pure]
        public bool Contains(float x, float y)
        {
            return Contains(new Vector2(x, y));
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Vector2"/> lies within the bounds of this <see cref="BoundingRectangle"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="BoundingRectangle"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Vector2"/> lies inside this <see cref="BoundingRectangle"/>; <c>false</c> otherwise.</returns>
        [Pure]
        public bool Contains(Vector2 value)
        {
            bool result;
            Contains(ref value, out result);
            return result;
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="Vector2"/> lies within the bounds of this <see cref="BoundingRectangle"/>.
        /// </summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="BoundingRectangle"/>.</param>
        /// <param name="result"><c>true</c> if the provided <see cref="Vector2"/> lies inside this <see cref="BoundingRectangle"/>; <c>false</c> otherwise. As an output parameter.</param>
        public void Contains(ref Vector2 value, out bool result)
        {
            result = Vector2.Min(value, Min) == Min
                  && Vector2.Max(value, Max) == Max;
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="BoundingRectangle"/> lies within the bounds of this <see cref="BoundingRectangle"/>.
        /// </summary>
        /// <param name="value">The <see cref="BoundingRectangle"/> to check for inclusion in this <see cref="BoundingRectangle"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="BoundingRectangle"/>'s bounds lie entirely inside this <see cref="BoundingRectangle"/>; <c>false</c> otherwise.</returns>
        [Pure]
        public bool Contains(BoundingRectangle value)
        {
            bool result;
            Contains(ref value, out result);
            return result;
        }

        /// <summary>
        /// Gets whether or not the provided <see cref="BoundingRectangle"/> lies within the bounds of this <see cref="BoundingRectangle"/>.
        /// </summary>
        /// <param name="value">The <see cref="BoundingRectangle"/> to check for inclusion in this <see cref="BoundingRectangle"/>.</param>
        /// <param name="result"><c>true</c> if the provided <see cref="BoundingRectangle"/>'s bounds lie entirely inside this <see cref="BoundingRectangle"/>; <c>false</c> otherwise. As an output parameter.</param>
        public void Contains(ref BoundingRectangle value, out bool result)
        {
            result = Vector2.Min(value.Min, Min) == Min
                  && Vector2.Max(value.Max, Max) == Max;
        }
        #endregion
    }
}
