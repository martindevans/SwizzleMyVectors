using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace SwizzleMyVectors.Geometry
{
    public struct BoundingSphere
    {
        /// <summary>
        /// The center point of the sphere.
        /// </summary>
        public Vector3 Center;

        /// <summary>
        /// The radius of the sphere.
        /// </summary>
        public float Radius;

        /// <summary>
        /// Creates a new instance of BoundingSphere.
        /// </summary>
        /// <param name="center">Center point of the sphere.</param><param name="radius">Radius of the sphere.</param>
        public BoundingSphere(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        #region equality
        /// <summary>
        /// Determines whether two instances of BoundingSphere are equal.
        /// </summary>
        /// <param name="a">The object to the left of the equality operator.</param><param name="b">The object to the right of the equality operator.</param>
        public static bool operator ==(BoundingSphere a, BoundingSphere b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether two instances of BoundingSphere are not equal.
        /// </summary>
        /// <param name="a">The BoundingSphere to the left of the inequality operator.</param><param name="b">The BoundingSphere to the right of the inequality operator.</param>
        public static bool operator !=(BoundingSphere a, BoundingSphere b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Determines whether the specified BoundingSphere is equal to the current BoundingSphere.
        /// </summary>
        /// <param name="other">The BoundingSphere to compare with the current BoundingSphere.</param>
        public bool Equals(BoundingSphere other)
        {
            return Radius.Equals(other.Radius) && Center.Equals(other.Center);
        }

        /// <summary>
        /// Determines whether the specified Object is equal to the BoundingSphere.
        /// </summary>
        /// <param name="obj">The Object to compare with the current BoundingSphere.</param>
        public override bool Equals(object obj)
        {
            return (obj is BoundingSphere sphere) && Equals(sphere);
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            // Ugly, but it's how MS do it in their vector types
            // ReSharper disable NonReadonlyFieldInGetHashCode

            var hash = 17;
            hash = hash * 31 + Center.GetHashCode();
            hash = hash * 31 + Radius.GetHashCode();
            return hash;

            // ReSharper restore NonReadonlyFieldInGetHashCode
        }
        #endregion

        /// <summary>
        /// Expand bounding sphere diameter by distance
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        [Pure]
        public BoundingSphere Inflate(float distance)
        {
            var self = this;
            Inflate(ref self, distance);
            return self;
        }

        /// <summary>
        /// Expand bounding sphere diameter by distance
        /// </summary>
        /// <param name="sphere">The sphere to mutate</param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static void Inflate(ref BoundingSphere sphere, float distance)
        {
            var r = sphere.Radius + distance / 2;
            if (r < 0)
                throw new ArgumentOutOfRangeException(nameof(distance), "Distance specified to inflate sphere is a negative value larger than the total diameter (this would cause a negative radius!)");

            sphere.Radius = r;
        }

        /// <summary>
        /// Returns a String that represents the current BoundingSphere.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Center:{0},Radius:{1}", Center, Radius);
        }

        /// <summary>
        /// Creates a BoundingSphere that contains the two specified BoundingSphere instances.
        /// </summary>
        /// <param name="original">BoundingSphere to be merged.</param><param name="additional">BoundingSphere to be merged.</param>
        public static BoundingSphere CreateMerged(BoundingSphere original, BoundingSphere additional)
        {
            CreateMerged(ref original, ref additional, out var result);
            return result;
        }

        /// <summary>
        /// Creates a BoundingSphere that contains the two specified BoundingSphere instances.
        /// </summary>
        /// <param name="original">BoundingSphere to be merged.</param><param name="additional">BoundingSphere to be merged.</param><param name="result">[OutAttribute] The created BoundingSphere.</param>
        public static void CreateMerged(ref BoundingSphere original, ref BoundingSphere additional, out BoundingSphere result)
        {
            var origToAdditionalCenter = Vector3.Subtract(additional.Center, original.Center);
            var centersDistSq = origToAdditionalCenter.LengthSquared();
            var centersDist = (float)Math.Sqrt(centersDistSq);

            //Check if the spheres overlap, if they do there's the possibility one of the spheres completely contains the other already
            if (centersDist < original.Radius + additional.Radius)
            {
                //Check if original contains additional
                if (Contains(original.Radius, additional.Radius, centersDistSq) == ContainmentType.Contains)
                {
                    result = original;
                    return;
                }

                //Check if additional contains original
                if (Contains(additional.Radius, original.Radius, centersDistSq) == ContainmentType.Contains)
                {
                    result = additional;
                    return;
                }
            }

            //else find center of new sphere and radius
            var leftRadius = Math.Max(original.Radius - centersDist, additional.Radius);
            var rightradius = Math.Max(original.Radius + centersDist, additional.Radius);
            origToAdditionalCenter = origToAdditionalCenter + (((leftRadius - rightradius) / (2 * origToAdditionalCenter.Length())) * origToAdditionalCenter);

            result = new BoundingSphere
            {
                Center = original.Center + origToAdditionalCenter,
                Radius = (leftRadius + rightradius) / 2
            };
        }

        /// <summary>
        /// Creates the smallest BoundingSphere that can contain a specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to create the BoundingSphere from.</param>
        public static BoundingSphere CreateFromBoundingBox(BoundingBox box)
        {
            CreateFromBoundingBox(ref box, out var result);
            return result;
        }

        /// <summary>
        /// Creates the smallest BoundingSphere that can contain a specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to create the BoundingSphere from.</param><param name="result">[OutAttribute] The created BoundingSphere.</param>
        public static void CreateFromBoundingBox(ref BoundingBox box, out BoundingSphere result)
        {
            // Find the center of the box.
            var center = new Vector3((box.Min.X + box.Max.X) / 2.0f,
                                     (box.Min.Y + box.Max.Y) / 2.0f,
                                     (box.Min.Z + box.Max.Z) / 2.0f);

            // Find the distance between the center and one of the corners of the box.
            var radius = Vector3.Distance(center, box.Max);

            // Add a fudge factor to the radius to ensure the sphere contains the box completely
            result = new BoundingSphere(center, radius + 0.000001f);
        }

        /// <summary>
        /// Creates a BoundingSphere that can contain a specified list of points.
        /// </summary>
        /// <param name="points">List of points the BoundingSphere must contain.</param>
        public static BoundingSphere CreateFromPoints([NotNull] IEnumerable<Vector3> points)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));

            // From "Real-Time Collision Detection" (Page 89)

            var minx = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var maxx = -minx;
            var miny = minx;
            var maxy = -minx;
            var minz = minx;
            var maxz = -minx;

            // Find the most extreme points along the principle axis.
            var numPoints = 0;
            foreach (var pt in points)
            {
                ++numPoints;

                if (pt.X < minx.X)
                    minx = pt;
                if (pt.X > maxx.X)
                    maxx = pt;
                if (pt.Y < miny.Y)
                    miny = pt;
                if (pt.Y > maxy.Y)
                    maxy = pt;
                if (pt.Z < minz.Z)
                    minz = pt;
                if (pt.Z > maxz.Z)
                    maxz = pt;
            }

            if (numPoints == 0)
                throw new ArgumentException("You should have at least one point in points.");

            var sqDistX = Vector3.DistanceSquared(maxx, minx);
            var sqDistY = Vector3.DistanceSquared(maxy, miny);
            var sqDistZ = Vector3.DistanceSquared(maxz, minz);

            // Pick the pair of most distant points.
            var min = minx;
            var max = maxx;
            if (sqDistY > sqDistX && sqDistY > sqDistZ)
            {
                max = maxy;
                min = miny;
            }
            if (sqDistZ > sqDistX && sqDistZ > sqDistY)
            {
                max = maxz;
                min = minz;
            }

            var center = (min + max) * 0.5f;
            var radius = Vector3.Distance(max, center);

            // Test every point and expand the sphere.
            // The current bounding sphere is just a good approximation and may not enclose all points.            
            // From: Mathematics for 3D Game Programming and Computer Graphics, Eric Lengyel, Third Edition.
            // Page 218
            var sqRadius = radius * radius;
            foreach (var pt in points)
            {
                var diff = (pt - center);
                var sqDist = diff.LengthSquared();
                if (sqDist > sqRadius)
                {
                    var distance = (float)Math.Sqrt(sqDist); // equal to diff.Length();
                    var direction = diff / distance;
                    var g = center - radius * direction;
                    center = (g + pt) / 2;
                    radius = Vector3.Distance(pt, center);
                    sqRadius = radius * radius;
                }
            }

            return new BoundingSphere(center, radius);
        }

        /// <summary>
        /// Creates the smallest BoundingSphere that can contain a specified BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to create the BoundingSphere with.</param>
        public static BoundingSphere CreateFromFrustum(BoundingFrustum frustum)
        {
            return CreateFromPoints(frustum.GetCorners());
        }

        /// <summary>
        /// Calculate the volume of this sphere
        /// </summary>
        /// <returns></returns>
        [Pure]
        public float Volume()
        {
            // (4 / 3 * pi) * r^3
            // ^^^^^^^^^^^^
            // This constant is precalculated

            return 4.18879020479f * Radius * Radius * Radius;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects with a specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check for intersection with the current BoundingSphere.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(BoundingBox box)
        {
            Intersects(ref box, out bool result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects a BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingSphere and BoundingBox intersect; false otherwise.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Intersects(ref BoundingBox box, out bool result)
        {
            result = box.Intersects(this);
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects with a specified BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to check for intersection with the current BoundingSphere.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(BoundingFrustum frustum)
        {
            Intersects(ref frustum, out var result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects with a specified BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to check for intersection with the current BoundingSphere.</param>
        /// <param name="result"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Intersects(ref BoundingFrustum frustum, out bool result)
        {
            frustum.Intersects(ref this, out result);
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects with a specified Plane.
        /// </summary>
        /// <param name="plane">The Plane to check for intersection with the current BoundingSphere.</param>
        public PlaneIntersectionType Intersects(Plane plane)
        {
            Intersects(ref plane, out var result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects a Plane.
        /// </summary>
        /// <param name="plane">The Plane to check for intersection with.</param><param name="result">[OutAttribute] An enumeration indicating whether the BoundingSphere intersects the Plane.</param>
        public void Intersects(ref Plane plane, out PlaneIntersectionType result)
        {
            var distance = Vector3.Dot(plane.Normal, Center) + plane.D;

            if (distance > Radius)
                result = PlaneIntersectionType.Front;
            else if (distance < -Radius)
                result = PlaneIntersectionType.Back;
            else
                result = PlaneIntersectionType.Intersecting;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects with a specified Ray.
        /// </summary>
        /// <param name="ray">The Ray to check for intersection with the current BoundingSphere.</param>
        public float? Intersects(Ray3 ray)
        {
            return ray.Intersects(this);
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects a Ray.
        /// </summary>
        /// <param name="ray">The Ray to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingSphere or null if there is no intersection.</param>
        public void Intersects(ref Ray3 ray, out float? result)
        {
            ray.Intersects(ref this, out result);
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects with a specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with the current BoundingSphere.</param>
        public bool Intersects(BoundingSphere sphere)
        {
            Intersects(ref sphere, out var result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects another BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingSphere instances intersect; false otherwise.</param>
        public void Intersects(ref BoundingSphere sphere, out bool result)
        {
            var sqDistance = Vector3.DistanceSquared(sphere.Center, Center);

            var totalRadius = sphere.Radius + Radius;
            result = sqDistance < totalRadius * totalRadius;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check against the current BoundingSphere.</param>
        public ContainmentType Contains(BoundingBox box)
        {
            Contains(ref box, out var result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains(ref BoundingBox box, out ContainmentType result)
        {
            //check if all corners are inside the sphere
            box.GetCorners(out var a, out var b, out var c, out var d, out var e, out var f, out var g, out var h);
            if (Contains(a) && Contains(b) && Contains(c) && Contains(d) && Contains(e) && Contains(f) && Contains(g) && Contains(h))
            {
                result = ContainmentType.Contains;
                return;
            }

            box.Contains(ref this, out var boxContain);

            result = boxContain == ContainmentType.Disjoint ? ContainmentType.Disjoint : ContainmentType.Intersects;
        }

        ///// <summary>
        ///// Checks whether the current BoundingSphere contains the specified BoundingFrustum.
        ///// </summary>
        ///// <param name="frustum">The BoundingFrustum to check against the current BoundingSphere.</param>
        //public ContainmentType Contains(BoundingFrustum frustum)
        //{
        //    ContainmentType result;
        //    Contains(ref frustum, out result);
        //    return result;
        //}

        ///// <summary>
        ///// Checks whether the current BoundingSphere contains the specified BoundingFrustum.
        ///// </summary>
        ///// <param name="frustum">The BoundingFrustum to check against the current BoundingSphere.</param>
        ///// <param name="result"></param>
        //public void Contains(ref BoundingFrustum frustum, out ContainmentType result)
        //{
            
        //}

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified point.
        /// </summary>
        /// <param name="point">The point to check against the current BoundingSphere.</param>
        public bool Contains(Vector3 point)
        {
            Contains(ref point, out var result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified point.
        /// </summary>
        /// <param name="point">The point to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains(ref Vector3 point, out bool result)
        {
            result = Contains(Radius, 0, Vector3.DistanceSquared(point, Center)) != ContainmentType.Disjoint;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check against the current BoundingSphere.</param>
        public ContainmentType Contains(BoundingSphere sphere)
        {
            Contains(ref sphere, out var result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains(ref BoundingSphere sphere, out ContainmentType result)
        {
            result = Contains(Radius, sphere.Radius, Vector3.DistanceSquared(sphere.Center, Center));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ContainmentType Contains(float outerRadius, float innerRadius, float squareDistanceBetweenCenters)
        {
            var sqDistance = squareDistanceBetweenCenters;

            var radSum = innerRadius + outerRadius;
            if (sqDistance > radSum * radSum)
                return ContainmentType.Disjoint;

            if (outerRadius > innerRadius)
            {
                var radDif = outerRadius - innerRadius;
                if (sqDistance <= radDif * radDif)
                    return ContainmentType.Contains;
            }

            return ContainmentType.Intersects;
        }

        /// <summary>
        /// Translates and scales the BoundingSphere using a given Matrix.
        /// </summary>
        /// <param name="matrix">A transformation matrix that might include translation, rotation, or uniform scaling. Note that BoundingSphere.Transform will not return correct results if there are non-uniform scaling, shears, or other unusual transforms in this transformation matrix. This is because there is no way to shear or non-uniformly scale a sphere. Such an operation would cause the sphere to lose its shape as a sphere.</param>
        public BoundingSphere Transform(Matrix4x4 matrix)
        {
            Transform(ref matrix, out var result);
            return result;
        }

        /// <summary>
        /// Translates and scales the BoundingSphere using a given Matrix.
        /// </summary>
        /// <param name="matrix">A transformation matrix that might include translation, rotation, or uniform scaling. Note that BoundingSphere.Transform will not return correct results if there are non-uniform scaling, shears, or other unusual transforms in this transformation matrix. This is because there is no way to shear or non-uniformly scale a sphere. Such an operation would cause the sphere to lose its shape as a sphere.</param><param name="result">[OutAttribute] The transformed BoundingSphere.</param>
        public void Transform(ref Matrix4x4 matrix, out BoundingSphere result)
        {
            result.Center = Vector3.Transform(Center, matrix);
            result.Radius = Radius * ((float)Math.Sqrt(Math.Max(((matrix.M11 * matrix.M11) + (matrix.M12 * matrix.M12)) + (matrix.M13 * matrix.M13), Math.Max(((matrix.M21 * matrix.M21) + (matrix.M22 * matrix.M22)) + (matrix.M23 * matrix.M23), ((matrix.M31 * matrix.M31) + (matrix.M32 * matrix.M32)) + (matrix.M33 * matrix.M33)))));
        }
    }
}
