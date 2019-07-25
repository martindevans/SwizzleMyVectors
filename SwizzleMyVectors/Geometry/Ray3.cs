using System;
using System.Numerics;

namespace SwizzleMyVectors.Geometry
{
    public struct Ray3
    {
        /// <summary>
        /// Specifies the starting point of the Ray.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Unit vector specifying the direction the Ray is pointing.
        /// </summary>
        public Vector3 Direction;

        /// <summary>
        /// Creates a new instance of Ray.
        /// </summary>
        /// <param name="position">The starting point of the Ray.</param><param name="direction">Unit vector describing the direction of the Ray.</param>
        public Ray3(Vector3 position, Vector3 direction)
        {
            Position = position;
            Direction = direction;
        }

        #region equality
        /// <summary>
        /// Determines whether two instances of Ray are equal.
        /// </summary>
        /// <param name="a">The object to the left of the equality operator.</param><param name="b">The object to the right of the equality operator.</param>
        public static bool operator ==(Ray3 a, Ray3 b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether two instances of Ray are not equal.
        /// </summary>
        /// <param name="a">The object to the left of the inequality operator.</param><param name="b">The object to the right of the inequality operator.</param>
        public static bool operator !=(Ray3 a, Ray3 b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Determines whether the specified Ray is equal to the current Ray.
        /// </summary>
        /// <param name="other">The Ray to compare with the current Ray.</param>
        public bool Equals(Ray3 other)
        {
            return Position.Equals(other.Position) && Direction.Equals(other.Direction);
        }

        /// <summary>
        /// Determines whether two instances of Ray are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Ray.</param>
        public override bool Equals(object obj)
        {
            return obj is Ray3 ray3 && Equals(ray3);
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
// ReSharper disable NonReadonlyFieldInGetHashCode

            int hash = 17;
            hash = hash * 31 + Position.GetHashCode();
            hash = hash * 31 + Direction.GetHashCode();
            return hash;

// ReSharper restore NonReadonlyFieldInGetHashCode
        }
        #endregion

        /// <summary>
        /// Returns a String that represents the current Ray.
        /// </summary>
        public override string ToString()
        {
            return $"Position:{Position},Direction:{Direction}";
        }

        /// <summary>
        /// Calculate the closest point on this ray to the given point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector3 ClosestPoint(Vector3 point)
        {
            return ClosestPoint(ref point, out _);
        }

        /// <summary>
        /// Calculate the closest point on this ray to the given point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="result">distance along the ray at which the closest point lies</param>
        /// <returns></returns>
        public Vector3 ClosestPoint(ref Vector3 point, out float result)
        {
            ClosestPointDistanceAlongLine(ref point, out result);
            return Position + Direction * result;
        }

        /// <summary>
        /// Gets how far along this line the closest point is (in units of direction length)
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float ClosestPointDistanceAlongLine(Vector3 point)
        {
            ClosestPointDistanceAlongLine(ref point, out var dist);
            return dist;
        }

        /// <summary>
        /// Gets how far along this line the closest point is (in units of direction length)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public void ClosestPointDistanceAlongLine(ref Vector3 point, out float distance)
        {
            var direction = Direction;
            var lengthSq = direction.LengthSquared();

            distance = Vector3.Dot((point - Position), direction) / lengthSq;
        }

        public void ClosestPoint(in Ray3 other, out float distanceAlongThis, out float distanceAlongOther)
        {
            // http://geomalgorithms.com/a07-_distance.html
            // a = u · u, b = u · v, c = v · v, d = u · w0, and e = v · w0

            var v = Direction;
            var u = other.Direction;
            var w0 = other.Position - Position;

            var a = u.LengthSquared();
            var b = Vector3.Dot(u, v);
            var c = v.LengthSquared();
            var d = Vector3.Dot(u, w0);
            var e = Vector3.Dot(v, w0);

            var denom = a * c - b * b;

            if (Math.Abs(denom) < float.Epsilon)
            {
                distanceAlongThis = 0;
                distanceAlongOther = 0;
            }
            else
            {
                distanceAlongOther = (b * e - c * d) / denom;
                distanceAlongThis = (a * e - b * d) / denom;
            }
        }

        //public float DistanceToPoint(Vector3 point)
        //{
        //}

        //public void DistanceToPoint(ref Vector3 point, out float distance)
        //{
        //float result;
        //DistanceToPoint(ref point, out result);
        //return result;
        //}

        //public RayRayIntersection? Intersects(Ray3 ray, out Parallelism parallelism)
        //{
        //}

        //public RayRayIntersection? Intersects(Ray3 ray)
        //{
        //}

        //public struct RayRayIntersection
        //{
        //    /// <summary>
        //    /// The position where the two rays intersect
        //    /// </summary>
        //    public readonly Vector3 Position;

        //    /// <summary>
        //    /// The distance along ray A. Units are in ray lengths, so 0 indicates the start, 1 indicates the end.
        //    /// </summary>
        //    public readonly float DistanceAlongA;

        //    /// <summary>
        //    /// The distance along ray B. Units are in ray lengths, so 0 indicates the start, 1 indicates the end.
        //    /// </summary>
        //    public readonly float DistanceAlongB;

        //    public RayRayIntersection(Vector3 position, float distanceAlongLineA, float distanceAlongLineB)
        //    {
        //        Position = position;
        //        DistanceAlongA = distanceAlongLineA;
        //        DistanceAlongB = distanceAlongLineB;
        //    }
        //}

        /// <summary>
        /// Checks whether the Ray intersects a specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check for intersection with the Ray.</param>
        public float? Intersects(BoundingBox box)
        {
            Intersects(ref box, out var result);
            return result;
        }

        /// <summary>
        /// Checks whether the current Ray intersects a BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingBox or null if there is no intersection.</param>
        public void Intersects(ref BoundingBox box, out float? result)
        {
            const float epsilon = 1e-6f;

            float? tMin = null, tMax = null;

            if (Math.Abs(Direction.X) < epsilon)
            {
                if (Position.X < box.Min.X || Position.X > box.Max.X)
                {
                    result = null;
                    return;
                }
            }
            else
            {
                tMin = (box.Min.X - Position.X) / Direction.X;
                tMax = (box.Max.X - Position.X) / Direction.X;

                if (tMin > tMax)
                {
                    var temp = tMin;
                    tMin = tMax;
                    tMax = temp;
                }
            }

            if (Math.Abs(Direction.Y) < epsilon)
            {
                if (Position.Y < box.Min.Y || Position.Y > box.Max.Y)
                {
                    result = null;
                    return;
                }
            }
            else
            {
                var tMinY = (box.Min.Y - Position.Y) / Direction.Y;
                var tMaxY = (box.Max.Y - Position.Y) / Direction.Y;

                if (tMinY > tMaxY)
                {
                    var temp = tMinY;
                    tMinY = tMaxY;
                    tMaxY = temp;
                }

                if ((tMin.HasValue && tMin > tMaxY) || (tMax.HasValue && tMinY > tMax))
                {
                    result = null;
                    return;
                }

                if (!tMin.HasValue || tMinY > tMin) tMin = tMinY;
                if (!tMax.HasValue || tMaxY < tMax) tMax = tMaxY;
            }

            if (Math.Abs(Direction.Z) < epsilon)
            {
                if (Position.Z < box.Min.Z || Position.Z > box.Max.Z)
                {
                    result = null;
                    return;
                }
            }
            else
            {
                var tMinZ = (box.Min.Z - Position.Z) / Direction.Z;
                var tMaxZ = (box.Max.Z - Position.Z) / Direction.Z;

                if (tMinZ > tMaxZ)
                {
                    var temp = tMinZ;
                    tMinZ = tMaxZ;
                    tMaxZ = temp;
                }

                if ((tMin.HasValue && tMin > tMaxZ) || (tMax.HasValue && tMinZ > tMax))
                {
                    result = null;
                    return;
                }

                if (!tMin.HasValue || tMinZ > tMin) tMin = tMinZ;
                if (!tMax.HasValue || tMaxZ < tMax) tMax = tMaxZ;
            }

            // having a positive tMin and a negative tMax means the ray is inside the box
            // we expect the intesection distance to be 0 in that case
            if ((tMin.HasValue && tMin < 0) && tMax > 0)
            {
                result = 0;
                return;
            }

            // a negative tMin means that the intersection point is behind the ray's origin
            // we discard these as not hitting the AABB
            if (tMin < 0)
            {
                result = null;
                return;
            }

            result = tMin;
        }

        ///// <summary>
        ///// Checks whether the Ray intersects a specified BoundingFrustum.
        ///// </summary>
        ///// <param name="frustum">The BoundingFrustum to check for intersection with the Ray.</param>
        //public float? Intersects(BoundingFrustum frustum)
        //{
        //    float? result;
        //    Intersects(ref frustum, out result);
        //    return result;
        //}

        ///// <summary>
        ///// Checks whether the Ray intersects a specified BoundingFrustum.
        ///// </summary>
        ///// <param name="frustum">The BoundingFrustum to check for intersection with the Ray.</param>
        ///// <param name="result">[OutAttribute] The distance at which this ray intersects the specified BoundingFrustum, or null if there is no intersection</param>
        //public void Intersects(ref BoundingFrustum frustum, out float? result)
        //{
        //}

        /// <summary>
        /// Determines whether this Ray intersects a specified Plane.
        /// </summary>
        /// <param name="plane">The Plane with which to calculate this Ray's intersection.</param>
        public float? Intersects(Plane plane)
        {
            Intersects(ref plane, out var result);
            return result;
        }

        /// <summary>
        /// Determines whether this Ray intersects a specified Plane.
        /// </summary>
        /// <param name="plane">The Plane with which to calculate this Ray's intersection.</param><param name="result">[OutAttribute] The distance at which this Ray intersects the specified Plane, or null if there is no intersection.</param>
        public void Intersects(ref Plane plane, out float? result)
        {
            var den = Vector3.Dot(Direction, -plane.Normal);
            if (Math.Abs(den) < 0.00001f)
            {
                result = null;
                return;
            }

            result = (plane.D - Vector3.Dot(-plane.Normal, Position)) / den;

            if (result < 0.0f)
            {
                if (result < -0.00001f)
                {
                    result = null;
                    return;
                }

                result = 0.0f;
            }
        }

        /// <summary>
        /// Checks whether the Ray intersects a specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with the Ray.</param>
        public float? Intersects(BoundingSphere sphere)
        {
            Intersects(ref sphere, out var result);
            return result;
        }

        /// <summary>
        /// Checks whether the current Ray intersects a BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingSphere or null if there is no intersection.</param>
        public void Intersects(ref BoundingSphere sphere, out float? result)
        {
            // Find the vector between where the ray starts the the sphere's centre
            Vector3 difference = sphere.Center - Position;

            float differenceLengthSquared = difference.LengthSquared();
            float sphereRadiusSquared = sphere.Radius * sphere.Radius;

            // If the distance between the ray start and the sphere's centre is less than
            // the radius of the sphere, it means we've intersected. N.B. checking the LengthSquared is faster.
            if (differenceLengthSquared < sphereRadiusSquared)
            {
                result = 0.0f;
                return;
            }

            var distanceAlongRay = Vector3.Dot(Direction, difference);
            // If the ray is pointing away from the sphere then we don't ever intersect
            if (distanceAlongRay < 0)
            {
                result = null;
                return;
            }

            // Next we kinda use Pythagoras to check if we are within the bounds of the sphere
            // if x = radius of sphere
            // if y = distance between ray position and sphere centre
            // if z = the distance we've travelled along the ray
            // if x^2 + z^2 - y^2 < 0, we do not intersect
            float dist = sphereRadiusSquared + distanceAlongRay * distanceAlongRay - differenceLengthSquared;

            result = (dist < 0) ? null : distanceAlongRay - (float?)Math.Sqrt(dist);
        }

        /// <summary>
        /// Given a distance along the line (in units of line length) get the point this distance evaluates to
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector3 PointAlongLine(float t)
        {
            return Position + Direction * t;
        }
    }
}
