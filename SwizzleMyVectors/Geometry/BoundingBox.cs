using System;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;

namespace SwizzleMyVectors.Geometry
{
    public struct BoundingBox
    {
        /// <summary>
        /// Specifies the total number of corners (8) in the BoundingBox.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const int CornerCount = 8;

        /// <summary>
        /// The minimum point the BoundingBox contains.
        /// </summary>
        public Vector3 Min;

        /// <summary>
        /// The maximum point the BoundingBox contains.
        /// </summary>
        public Vector3 Max;

        public Vector3 Extent => Max - Min;

        /// <summary>
        /// Creates an instance of BoundingBox around a BoundingSphere
        /// </summary>
        /// <param name="sphere">The sphere to constain within this box</param>
        public BoundingBox(BoundingSphere sphere)
            : this(sphere.Center - new Vector3(sphere.Radius), sphere.Center + new Vector3(sphere.Radius))
        {
        }

        /// <summary>
        /// Creates an instance of BoundingBox.
        /// </summary>
        /// <param name="min">The minimum point the BoundingBox includes.</param><param name="max">The maximum point the BoundingBox includes.</param>
        public BoundingBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        #region static factories
        /// <summary>
        /// Creates the smallest BoundingBox that contains the two specified BoundingBox instances.
        /// </summary>
        /// <param name="original">One of the BoundingBoxs to contain.</param><param name="additional">One of the BoundingBoxs to contain.</param>
        public static BoundingBox CreateMerged(BoundingBox original, BoundingBox additional)
        {
            CreateMerged(ref original, ref additional, out var result);
            return result;
        }

        /// <summary>
        /// Creates the smallest BoundingBox that contains the two specified BoundingBox instances.
        /// </summary>
        /// <param name="original">One of the BoundingBox instances to contain.</param><param name="additional">One of the BoundingBox instances to contain.</param><param name="result">[OutAttribute] The created BoundingBox.</param>
        public static void CreateMerged(ref BoundingBox original, ref BoundingBox additional, out BoundingBox result)
        {
            result = new BoundingBox(
                Vector3.Min(original.Min, additional.Min),
                Vector3.Max(original.Max, additional.Max)
            );
        }

        /// <summary>
        /// Creates the smallest BoundingBox that will contain the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to contain.</param>
        public static BoundingBox CreateFromSphere(BoundingSphere sphere)
        {
            CreateFromSphere(ref sphere, out BoundingBox result);
            return result;
        }

        /// <summary>
        /// Creates the smallest BoundingBox that will contain the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to contain.</param><param name="result">[OutAttribute] The created BoundingBox.</param>
        public static void CreateFromSphere(ref BoundingSphere sphere, out BoundingBox result)
        {
            var corner = new Vector3(sphere.Radius);
            result.Min = sphere.Center - corner;
            result.Max = sphere.Center + corner;
        }

        /// <summary>
        /// Creates the smallest BoundingBox that will contain a group of points.
        /// </summary>
        /// <param name="points">A list of points the BoundingBox should contain.</param>
        public static BoundingBox CreateFromPoints([NotNull] IEnumerable<Vector3> points)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));

            var empty = true;
            var min = new Vector3(float.MaxValue);
            var max = new Vector3(float.MinValue);
            foreach (var point in points)
            {
                min = Vector3.Min(min, point);
                max = Vector3.Max(max, point);

                empty = false;
            }

            if (empty)
                throw new ArgumentException("points");

            return new BoundingBox(min, max);
        }
        #endregion

        /// <summary>
        /// Determines whether two instances of BoundingBox are equal.
        /// </summary>
        /// <param name="a">BoundingBox to compare.</param><param name="b">BoundingBox to compare.</param>
        [Pure]
        public static bool operator ==(BoundingBox a, BoundingBox b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether two instances of BoundingBox are not equal.
        /// </summary>
        /// <param name="a">The object to the left of the inequality operator.</param><param name="b">The object to the right of the inequality operator.</param>
        [Pure]
        public static bool operator !=(BoundingBox a, BoundingBox b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Expand bounding box by distance / 2 at min and max (total of distance)
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        [Pure]
        public BoundingBox Inflate(float distance)
        {
            var self = this;
            Inflate(ref self, distance);
            return self;
        }

        /// <summary>
        /// Expand bounding box by distance / 2 at min and max (total of distance)
        /// </summary>
        /// <param name="box">The box to mutate</param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static void Inflate(ref BoundingBox box, float distance)
        {
            var min = box.Min - new Vector3(distance / 2);
            var max = box.Max + new Vector3(distance / 2);

            if (distance < 0)
            {
                if (Vector3.Min(min, max) != min)
                    throw new ArgumentOutOfRangeException("distance", "Distance specified to inflate rectangle is a negative value larger than the total diagonal size of the rectangle (doing this would invert the rectangle!");
            }

            box.Min = min;
            box.Max = max;
        }

        /// <summary>
        /// Gets an array of points that make up the corners of the BoundingBox.
        /// </summary>
        [Pure, NotNull]
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public Vector3[] GetCorners()
        {
            var arr = new Vector3[CornerCount];
            GetCorners(arr);
            return arr;
        }

        /// <summary>
        /// Gets the array of points that make up the corners of the BoundingBox.
        /// </summary>
        /// <param name="corners">An existing array of at least 8 Vector3 points where the corners of the BoundingBox are written.</param>
        public void GetCorners([NotNull] Vector3[] corners)
        {
            if (corners.Length < CornerCount)
                throw new ArgumentException("Array too small", nameof(corners));

            corners[0] = new Vector3(Min.X, Max.Y, Max.Z);
            corners[1] = new Vector3(Max.X, Max.Y, Max.Z);
            corners[2] = new Vector3(Max.X, Min.Y, Max.Z);
            corners[3] = new Vector3(Min.X, Min.Y, Max.Z);
            corners[4] = new Vector3(Min.X, Max.Y, Min.Z);
            corners[5] = new Vector3(Max.X, Max.Y, Min.Z);
            corners[6] = new Vector3(Max.X, Min.Y, Min.Z);
            corners[7] = new Vector3(Min.X, Min.Y, Min.Z);
        }

        /// <summary>
        /// Determines whether two instances of BoundingBox are equal.
        /// </summary>
        /// <param name="other">The BoundingBox to compare with the current BoundingBox.</param>
        [Pure]
        public bool Equals(BoundingBox other)
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
            return obj is BoundingBox && Equals((BoundingBox)obj);
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        [Pure]
        public override int GetHashCode()
        {
// This seems ugly, but it's inline with how MS designed their vector types!
// ReSharper disable NonReadonlyFieldInGetHashCode

            var hash = 17;
            hash = hash * 31 + Min.GetHashCode();
            hash = hash * 31 + Max.GetHashCode();
            return hash;

// ReSharper restore NonReadonlyFieldInGetHashCode
        }

        /// <summary>
        /// Returns a String that represents the current BoundingBox.
        /// </summary>
        [Pure]
        public override string ToString()
        {
            return $"Min:{Min},Max:{Max}";
        }

        /// <summary>
        /// Calculate the volume of this bounding box
        /// </summary>
        /// <returns></returns>
        [Pure]
        public float Volume()
        {
            var sz = Max - Min;
            return sz.X * sz.Y * sz.Z;
        }

        #region intersection
        /// <summary>
        /// Checks whether the current BoundingBox intersects another BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check for intersection with.</param>
        [Pure]
        public bool Intersects(BoundingBox box)
        {
            Intersects(ref box, out var result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects another BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingBox instances intersect; false otherwise.</param>
        public void Intersects(ref BoundingBox box, out bool result)
        {
            result = box.Min.X < Max.X && box.Max.X > Min.X
                   && box.Min.Y < Max.Y && box.Max.Y > Min.Y
                   && box.Min.Z < Max.Z && box.Max.Z > Min.Z;
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects a BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to check for intersection with.</param>
        [Pure]
        public bool Intersects(BoundingFrustum frustum)
        {
            Intersects(ref frustum, out bool result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects a BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to check for intersection with.</param>
        /// <param name="result">[OutAttribute] A boolean indicating whether the BoundingBox intersects the BoundingFrustum.</param>
        public void Intersects(ref BoundingFrustum frustum, out bool result)
        {
            result = frustum.Intersects(this);
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects a Plane.
        /// </summary>
        /// <param name="plane">The Plane to check for intersection with.</param>
        [Pure]
        public PlaneIntersectionType Intersects(Plane plane)
        {
            Intersects(ref plane, out PlaneIntersectionType result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects a Plane.
        /// </summary>
        /// <param name="plane">The Plane to check for intersection with.</param>
        /// <param name="result">[OutAttribute] An enumeration indicating whether the BoundingBox intersects the Plane.</param>
        public void Intersects(ref Plane plane, out PlaneIntersectionType result)
        {
            // See http://zach.in.tu-clausthal.de/teaching/cg_literatur/lighthouse3d_view_frustum_culling/index.html

            Vector3 positiveVertex;
            Vector3 negativeVertex;

            if (plane.Normal.X >= 0)
            {
                positiveVertex.X = Max.X;
                negativeVertex.X = Min.X;
            }
            else
            {
                positiveVertex.X = Min.X;
                negativeVertex.X = Max.X;
            }

            if (plane.Normal.Y >= 0)
            {
                positiveVertex.Y = Max.Y;
                negativeVertex.Y = Min.Y;
            }
            else
            {
                positiveVertex.Y = Min.Y;
                negativeVertex.Y = Max.Y;
            }

            if (plane.Normal.Z >= 0)
            {
                positiveVertex.Z = Max.Z;
                negativeVertex.Z = Min.Z;
            }
            else
            {
                positiveVertex.Z = Min.Z;
                negativeVertex.Z = Max.Z;
            }

            // Inline Vector3.Dot(plane.Normal, negativeVertex) + plane.D;
            var distance = plane.Normal.X * negativeVertex.X + plane.Normal.Y * negativeVertex.Y + plane.Normal.Z * negativeVertex.Z + plane.D;
            if (distance > 0)
            {
                result = PlaneIntersectionType.Front;
                return;
            }

            // Inline Vector3.Dot(plane.Normal, positiveVertex) + plane.D;
            distance = plane.Normal.X * positiveVertex.X + plane.Normal.Y * positiveVertex.Y + plane.Normal.Z * positiveVertex.Z + plane.D;
            if (distance < 0)
            {
                result = PlaneIntersectionType.Back;
                return;
            }

            result = PlaneIntersectionType.Intersecting;
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects a Ray.
        /// </summary>
        /// <param name="ray">The Ray to check for intersection with.</param>
        [Pure]
        public float? Intersects(Ray3 ray)
        {
            Intersects(ref ray, out float? result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects a Ray.
        /// </summary>
        /// <param name="ray">The Ray to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingBox, or null if there is no intersection.</param>
        public void Intersects(ref Ray3 ray, out float? result)
        {
            result = ray.Intersects(this);
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects a BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with.</param>
        [Pure]
        public bool Intersects(BoundingSphere sphere)
        {
            Intersects(ref sphere, out bool result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingBox intersects a BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingBox and BoundingSphere intersect; false otherwise.</param>
        public void Intersects(ref BoundingSphere sphere, out bool result)
        {
            if (sphere.Center.X - Min.X > sphere.Radius
                && sphere.Center.Y - Min.Y > sphere.Radius
                && sphere.Center.Z - Min.Z > sphere.Radius
                && Max.X - sphere.Center.X > sphere.Radius
                && Max.Y - sphere.Center.Y > sphere.Radius
                && Max.Z - sphere.Center.Z > sphere.Radius)
            {
                result = true;
                return;
            }

            double dmin = 0;

            if (sphere.Center.X - Min.X <= sphere.Radius)
                dmin += (sphere.Center.X - Min.X) * (sphere.Center.X - Min.X);
            else if (Max.X - sphere.Center.X <= sphere.Radius)
                dmin += (sphere.Center.X - Max.X) * (sphere.Center.X - Max.X);

            if (sphere.Center.Y - Min.Y <= sphere.Radius)
                dmin += (sphere.Center.Y - Min.Y) * (sphere.Center.Y - Min.Y);
            else if (Max.Y - sphere.Center.Y <= sphere.Radius)
                dmin += (sphere.Center.Y - Max.Y) * (sphere.Center.Y - Max.Y);

            if (sphere.Center.Z - Min.Z <= sphere.Radius)
                dmin += (sphere.Center.Z - Min.Z) * (sphere.Center.Z - Min.Z);
            else if (Max.Z - sphere.Center.Z <= sphere.Radius)
                dmin += (sphere.Center.Z - Max.Z) * (sphere.Center.Z - Max.Z);

            if (dmin <= sphere.Radius * sphere.Radius)
            {
                result = true;
                return;
            }

            result = false;
        }
        #endregion

        #region containment
        /// <summary>
        /// Tests whether the BoundingBox contains another BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to test for overlap.</param>
        [Pure]
        public ContainmentType Contains(BoundingBox box)
        {
            Contains(ref box, out ContainmentType result);
            return result;
        }

        /// <summary>
        /// Tests whether the BoundingBox contains a BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains(ref BoundingBox box, out ContainmentType result)
        {
            //test if all corner is in the same side of a face by just checking min and max
            if (box.Max.X < Min.X
                || box.Min.X > Max.X
                || box.Max.Y < Min.Y
                || box.Min.Y > Max.Y
                || box.Max.Z < Min.Z
                || box.Min.Z > Max.Z)
            {
                result = ContainmentType.Disjoint;
                return;
            }


            if (box.Min.X >= Min.X
                && box.Max.X <= Max.X
                && box.Min.Y >= Min.Y
                && box.Max.Y <= Max.Y
                && box.Min.Z >= Min.Z
                && box.Max.Z <= Max.Z)
            {
                result = ContainmentType.Contains;
                return;
            }

            result = ContainmentType.Intersects;
        }

        /// <summary>
        /// Tests whether the BoundingBox contains a BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to test for overlap.</param>
        //[Pure]
        //public ContainmentType Contains(BoundingFrustum frustum);

        /// <summary>
        /// Tests whether the BoundingBox contains a point.
        /// </summary>
        /// <param name="point">The point to test for overlap.</param>
        [Pure]
        public ContainmentType Contains(Vector3 point)
        {
            Contains(ref point, out ContainmentType result);
            return result;
        }

        /// <summary>
        /// Tests whether the BoundingBox contains a point.
        /// </summary>
        /// <param name="point">The point to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains(ref Vector3 point, out ContainmentType result)
        {
            //first we get if point is out of box
            if (point.X < Min.X
                || point.X > Max.X
                || point.Y < Min.Y
                || point.Y > Max.Y
                || point.Z < Min.Z
                || point.Z > Max.Z)
            {
                result = ContainmentType.Disjoint;
                return;
            }
                //or if point is on box because coordonate of point is lesser or equal
// ReSharper disable CompareOfFloatsByEqualityOperator
            if (point.X == Min.X
                || point.X == Max.X
                || point.Y == Min.Y
                || point.Y == Max.Y
                || point.Z == Min.Z
                || point.Z == Max.Z)
// ReSharper restore CompareOfFloatsByEqualityOperator
                result = ContainmentType.Intersects;
            else
                result = ContainmentType.Contains;
        }

        /// <summary>
        /// Tests whether the BoundingBox contains a BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to test for overlap.</param>
        [Pure]
        public ContainmentType Contains(BoundingSphere sphere)
        {
            Contains(ref sphere, out ContainmentType result);
            return result;
        }

        /// <summary>
        /// Tests whether the BoundingBox contains a BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains(ref BoundingSphere sphere, out ContainmentType result)
        {
            if (sphere.Center.X - Min.X >= sphere.Radius
                && sphere.Center.Y - Min.Y >= sphere.Radius
                && sphere.Center.Z - Min.Z >= sphere.Radius
                && Max.X - sphere.Center.X >= sphere.Radius
                && Max.Y - sphere.Center.Y >= sphere.Radius
                && Max.Z - sphere.Center.Z >= sphere.Radius)
            {
                result = ContainmentType.Contains;
                return;
            }

            double dmin = 0;

            double e = sphere.Center.X - Min.X;
            if (e < 0)
            {
                if (e < -sphere.Radius)
                {
                    result = ContainmentType.Disjoint;
                    return;
                }
                dmin += e * e;
            }
            else
            {
                e = sphere.Center.X - Max.X;
                if (e > 0)
                {
                    if (e > sphere.Radius)
                    {
                        result = ContainmentType.Disjoint;
                        return;
                    }
                    dmin += e * e;
                }
            }

            e = sphere.Center.Y - Min.Y;
            if (e < 0)
            {
                if (e < -sphere.Radius)
                {
                    result = ContainmentType.Disjoint;
                    return;
                }
                dmin += e * e;
            }
            else
            {
                e = sphere.Center.Y - Max.Y;
                if (e > 0)
                {
                    if (e > sphere.Radius)
                    {
                        result = ContainmentType.Disjoint;
                        return;
                    }
                    dmin += e * e;
                }
            }

            e = sphere.Center.Z - Min.Z;
            if (e < 0)
            {
                if (e < -sphere.Radius)
                {
                    result = ContainmentType.Disjoint;
                    return;
                }
                dmin += e * e;
            }
            else
            {
                e = sphere.Center.Z - Max.Z;
                if (e > 0)
                {
                    if (e > sphere.Radius)
                    {
                        result = ContainmentType.Disjoint;
                        return;
                    }
                    dmin += e * e;
                }
            }

            if (dmin <= sphere.Radius * sphere.Radius)
            {
                result = ContainmentType.Intersects;
                return;
            }

            result = ContainmentType.Disjoint;
        }
        #endregion
    }
}
