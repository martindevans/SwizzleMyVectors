using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Numerics;

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
                throw new ArgumentOutOfRangeException("distance", "Distance specified to inflate sphere is a negative value larger than the total diameter (this would cause a negative radius!)");

            sphere.Radius = r;
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
            return obj is BoundingSphere && Equals((BoundingSphere)obj);
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
// Ugly, but it's how MS do it in their vector types
// ReSharper disable NonReadonlyFieldInGetHashCode

            int hash = 17;
            hash = hash * 31 + Center.GetHashCode();
            hash = hash * 31 + Radius.GetHashCode();
            return hash;

// ReSharper restore NonReadonlyFieldInGetHashCode
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
            BoundingSphere result;
            CreateMerged(ref original, ref additional, out result);
            return result;
        }

        /// <summary>
        /// Creates a BoundingSphere that contains the two specified BoundingSphere instances.
        /// </summary>
        /// <param name="original">BoundingSphere to be merged.</param><param name="additional">BoundingSphere to be merged.</param><param name="result">[OutAttribute] The created BoundingSphere.</param>
        public static void CreateMerged(ref BoundingSphere original, ref BoundingSphere additional, out BoundingSphere result)
        {
            Vector3 ocenterToaCenter = Vector3.Subtract(additional.Center, original.Center);
            float distance = ocenterToaCenter.Length();
            if (distance <= original.Radius + additional.Radius)//intersect
            {
                if (distance <= original.Radius - additional.Radius)//original contain additional
                {
                    result = original;
                    return;
                }
                if (distance <= additional.Radius - original.Radius)//additional contain original
                {
                    result = additional;
                    return;
                }
            }
            //else find center of new sphere and radius
            float leftRadius = Math.Max(original.Radius - distance, additional.Radius);
            float rightradius = Math.Max(original.Radius + distance, additional.Radius);
            ocenterToaCenter = ocenterToaCenter + (((leftRadius - rightradius) / (2 * ocenterToaCenter.Length())) * ocenterToaCenter);//oCenterToResultCenter

            result = new BoundingSphere
            {
                Center = original.Center + ocenterToaCenter,
                Radius = (leftRadius + rightradius) / 2
            };
        }

        /// <summary>
        /// Creates the smallest BoundingSphere that can contain a specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to create the BoundingSphere from.</param>
        public static BoundingSphere CreateFromBoundingBox(BoundingBox box)
        {
            BoundingSphere result;
            CreateFromBoundingBox(ref box, out result);
            return result;
        }

        /// <summary>
        /// Creates the smallest BoundingSphere that can contain a specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to create the BoundingSphere from.</param><param name="result">[OutAttribute] The created BoundingSphere.</param>
        public static void CreateFromBoundingBox(ref BoundingBox box, out BoundingSphere result)
        {
            // Find the center of the box.
            Vector3 center = new Vector3((box.Min.X + box.Max.X) / 2.0f,
                                         (box.Min.Y + box.Max.Y) / 2.0f,
                                         (box.Min.Z + box.Max.Z) / 2.0f);

            // Find the distance between the center and one of the corners of the box.
            float radius = Vector3.Distance(center, box.Max);

            result = new BoundingSphere(center, radius);
        }

        /// <summary>
        /// Creates a BoundingSphere that can contain a specified list of points.
        /// </summary>
        /// <param name="points">List of points the BoundingSphere must contain.</param>
        public static BoundingSphere CreateFromPoints(IEnumerable<Vector3> points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

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
            float sqRadius = radius * radius;
            foreach (var pt in points)
            {
                Vector3 diff = (pt - center);
                float sqDist = diff.LengthSquared();
                if (sqDist > sqRadius)
                {
                    float distance = (float)Math.Sqrt(sqDist); // equal to diff.Length();
                    Vector3 direction = diff / distance;
                    Vector3 g = center - radius * direction;
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
        public bool Intersects(BoundingBox box)
        {
            return box.Intersects(this);
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects a BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingSphere and BoundingBox intersect; false otherwise.</param>
        public void Intersects(ref BoundingBox box, out bool result)
        {
            result = box.Intersects(this);
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects with a specified BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to check for intersection with the current BoundingSphere.</param>
        public bool Intersects(BoundingFrustum frustum)
        {
            bool result;
            Intersects(ref frustum, out result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects with a specified BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The BoundingFrustum to check for intersection with the current BoundingSphere.</param>
        /// <param name="result"></param>
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
            PlaneIntersectionType result;
            Intersects(ref plane, out result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects a Plane.
        /// </summary>
        /// <param name="plane">The Plane to check for intersection with.</param><param name="result">[OutAttribute] An enumeration indicating whether the BoundingSphere intersects the Plane.</param>
        public void Intersects(ref Plane plane, out PlaneIntersectionType result)
        {
            float distance = Vector3.Dot(plane.Normal, Center) + plane.D;

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
            bool result;
            Intersects(ref sphere, out result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere intersects another BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingSphere instances intersect; false otherwise.</param>
        public void Intersects(ref BoundingSphere sphere, out bool result)
        {
            float sqDistance = Vector3.DistanceSquared(sphere.Center, Center);

            var totalRadius = sphere.Radius + Radius;
            result = sqDistance < totalRadius * totalRadius;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check against the current BoundingSphere.</param>
        public ContainmentType Contains(BoundingBox box)
        {
            ContainmentType result;
            Contains(ref box, out result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains(ref BoundingBox box, out ContainmentType result)
        {
            //check if all corner is in sphere
            bool inside = true;
            foreach (Vector3 corner in box.GetCorners())
            {
                if (Contains(corner) == ContainmentType.Disjoint)
                {
                    inside = false;
                    break;
                }
            }

            if (inside)
            {
                result = ContainmentType.Contains;
                return;
            }

            //check if the distance from sphere center to cube face < radius
            double dmin = 0;

            if (Center.X < box.Min.X)
                dmin += (Center.X - box.Min.X) * (Center.X - box.Min.X);

            else if (Center.X > box.Max.X)
                dmin += (Center.X - box.Max.X) * (Center.X - box.Max.X);

            if (Center.Y < box.Min.Y)
                dmin += (Center.Y - box.Min.Y) * (Center.Y - box.Min.Y);

            else if (Center.Y > box.Max.Y)
                dmin += (Center.Y - box.Max.Y) * (Center.Y - box.Max.Y);

            if (Center.Z < box.Min.Z)
                dmin += (Center.Z - box.Min.Z) * (Center.Z - box.Min.Z);

            else if (Center.Z > box.Max.Z)
                dmin += (Center.Z - box.Max.Z) * (Center.Z - box.Max.Z);

            if (dmin <= Radius * Radius)
            {
                result = ContainmentType.Intersects;
                return;
            }

            //else disjoint
            result = ContainmentType.Disjoint;
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
        public ContainmentType Contains(Vector3 point)
        {
            ContainmentType result;
            Contains(ref point, out result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified point.
        /// </summary>
        /// <param name="point">The point to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains(ref Vector3 point, out ContainmentType result)
        {
            float sqRadius = Radius * Radius;
            float sqDistance = Vector3.DistanceSquared(point, Center);

            if (sqDistance > sqRadius)
                result = ContainmentType.Disjoint;

            else if (sqDistance < sqRadius)
                result = ContainmentType.Contains;

            else
                result = ContainmentType.Intersects;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check against the current BoundingSphere.</param>
        public ContainmentType Contains(BoundingSphere sphere)
        {
            ContainmentType result;
            Contains(ref sphere, out result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingSphere contains the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains(ref BoundingSphere sphere, out ContainmentType result)
        {
            float sqDistance = Vector3.DistanceSquared(sphere.Center, Center);

            if (sqDistance > (sphere.Radius + Radius) * (sphere.Radius + Radius))
                result = ContainmentType.Disjoint;

            else if (sqDistance <= (Radius - sphere.Radius) * (Radius - sphere.Radius))
                result = ContainmentType.Contains;

            else
                result = ContainmentType.Intersects;
        }

        /// <summary>
        /// Translates and scales the BoundingSphere using a given Matrix.
        /// </summary>
        /// <param name="matrix">A transformation matrix that might include translation, rotation, or uniform scaling. Note that BoundingSphere.Transform will not return correct results if there are non-uniform scaling, shears, or other unusual transforms in this transformation matrix. This is because there is no way to shear or non-uniformly scale a sphere. Such an operation would cause the sphere to lose its shape as a sphere.</param>
        public BoundingSphere Transform(Matrix4x4 matrix)
        {
            BoundingSphere result;
            Transform(ref matrix, out result);
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
