﻿using System;
using System.Numerics;

namespace SwizzleMyVectors.Geometry
{
    public struct Ray2
    {
        /// <summary>
        /// Specifies the starting point of the Ray.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Unit vector specifying the direction the Ray is pointing.
        /// </summary>
        public Vector2 Direction;

        /// <summary>
        /// Creates a new instance of Ray.
        /// </summary>
        /// <param name="position">The starting point of the Ray.</param><param name="direction">Unit vector describing the direction of the Ray.</param>
        public Ray2(Vector2 position, Vector2 direction)
        {
            Position = position;
            Direction = direction;
        }

        #region equality
        /// <summary>
        /// Determines whether two instances of Ray are equal.
        /// </summary>
        /// <param name="a">The object to the left of the equality operator.</param><param name="b">The object to the right of the equality operator.</param>
        public static bool operator ==(Ray2 a, Ray2 b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether two instances of Ray are not equal.
        /// </summary>
        /// <param name="a">The object to the left of the inequality operator.</param><param name="b">The object to the right of the inequality operator.</param>
        public static bool operator !=(Ray2 a, Ray2 b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Determines whether the specified Ray is equal to the current Ray.
        /// </summary>
        /// <param name="other">The Ray to compare with the current Ray.</param>
        public bool Equals(Ray2 other)
        {
            return Position.Equals(other.Position) && Direction.Equals(other.Direction);
        }

        /// <summary>
        /// Determines whether two instances of Ray are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Ray.</param>
        public override bool Equals(object obj)
        {
            return obj is Ray2 ray2 && Equals(ray2);
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
// ReSharper disable NonReadonlyFieldInGetHashCode

            var hash = 17;
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
            return string.Format("Position:{0},Direction:{1}", Position, Direction);
        }

        #region closest point
        /// <summary>
        /// Calculate the closest point on this ray to the given point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2 ClosestPoint(Vector2 point)
        {
            return ClosestPoint(ref point, out var _);
        }

        /// <summary>
        /// Calculate the closest point on this ray to the given point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="result">distance along the ray at which the closest point lies</param>
        /// <returns></returns>
        public Vector2 ClosestPoint(ref Vector2 point, out float result)
        {
            ClosestPointDistanceAlongLine(ref point, out result);
            return Position + Direction * result;
        }

        /// <summary>
        /// Gets how far along this line the closest point is (in units of direction length)
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float ClosestPointDistanceAlongLine(Vector2 point)
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
        public void ClosestPointDistanceAlongLine(ref Vector2 point, out float distance)
        {
            var direction = Direction;
            var lengthSq = direction.LengthSquared();

            distance = Vector2.Dot((point - Position), direction) / lengthSq;
        }

        public float DistanceToPoint(Vector2 point)
        {
            DistanceToPoint(ref point, out var result);
            return result;
        }

        public void DistanceToPoint(ref Vector2 point, out float distance)
        {
            distance = Direction.Cross(point - Position) / Direction.Length();
        }
        #endregion

        #region intersection
        public LinesIntersection2? Intersects(Ray2 ray, out Parallelism parallelism)
        {
            //http://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect

            var p = Position;
            var r = Direction;

            var q = ray.Position;
            var s = ray.Direction;

            // It isn't maths if the variable names make any sense
            // ReSharper disable InconsistentNaming
            var RxS = r.Cross(s);
            var QmP = q - p;
            // ReSharper restore InconsistentNaming

            if (Math.Abs(RxS - 0) < 0.001)
            {
                // ReSharper disable InconsistentNaming
                var QmPxR = QmP.Cross(r);
                // ReSharper restore InconsistentNaming
                parallelism = Math.Abs(QmPxR - 0) < 0.001 ? Geometry.Parallelism.Collinear : Geometry.Parallelism.Parallel;
                return null;
            }

            var t = QmP.Cross(s) / RxS;
            var u = QmP.Cross(r) / RxS;

            var point = p + (t * r);

            parallelism = Geometry.Parallelism.None;
            return new LinesIntersection2(point, t, u);
        }

        public LinesIntersection2? Intersects(Ray2 ray)
        {
            return Intersects(ray, out var _);
        }

        /// <summary>
        /// Checks whether the Ray intersects a specified BoundingRectangle.
        /// </summary>
        /// <param name="box">The BoundingRectangle to check for intersection with the Ray.</param>
        public float? Intersects(BoundingRectangle box)
        {
            Intersects(ref box, out var result);
            return result;
        }

        /// <summary>
        /// Checks whether the current Ray intersects a BoundingRectangle.
        /// </summary>
        /// <param name="box">The BoundingRectangle to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingBox or null if there is no intersection.</param>
        public void Intersects(ref BoundingRectangle box, out float? result)
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
        ///// Checks whether the Ray intersects a specified BoundingSphere.
        ///// </summary>
        ///// <param name="sphere">The BoundingSphere to check for intersection with the Ray.</param>
        //public float? Intersects(BoundingSphere sphere)
        //{
        //    float? result;
        //    Intersects(ref sphere, out result);
        //    return result;
        //}

        ///// <summary>
        ///// Checks whether the current Ray intersects a BoundingSphere.
        ///// </summary>
        ///// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingSphere or null if there is no intersection.</param>
        //public void Intersects(ref BoundingSphere sphere, out float? result)
        //{
        //    // Find the vector between where the ray starts the the sphere's centre
        //    Vector2 difference = sphere.Center - Position;

        //    float differenceLengthSquared = difference.LengthSquared();
        //    float sphereRadiusSquared = sphere.Radius * sphere.Radius;

        //    // If the distance between the ray start and the sphere's centre is less than
        //    // the radius of the sphere, it means we've intersected. N.B. checking the LengthSquared is faster.
        //    if (differenceLengthSquared < sphereRadiusSquared)
        //    {
        //        result = 0.0f;
        //        return;
        //    }

        //    var distanceAlongRay = Vector2.Dot(Direction, difference);
        //    // If the ray is pointing away from the sphere then we don't ever intersect
        //    if (distanceAlongRay < 0)
        //    {
        //        result = null;
        //        return;
        //    }

        //    // Next we kinda use Pythagoras to check if we are within the bounds of the sphere
        //    // if x = radius of sphere
        //    // if y = distance between ray position and sphere centre
        //    // if z = the distance we've travelled along the ray
        //    // if x^2 + z^2 - y^2 < 0, we do not intersect
        //    float dist = sphereRadiusSquared + distanceAlongRay * distanceAlongRay - differenceLengthSquared;

        //    result = (dist < 0) ? null : distanceAlongRay - (float?)Math.Sqrt(dist);
        //}
        #endregion

        /// <summary>
        /// Checks if the given point is to the left of the line (from an observer standing at the start looking along the line)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public bool IsLeft(Vector2 point, float epsilon)
        {
            return DistanceToPoint(point) > -epsilon;
        }

        public Parallelism Parallelism(Ray2 ray)
        {
            //http://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect

            var p = Position;
            var r = Direction;

            var q = ray.Position;
            var s = ray.Direction;

            // It isn't maths if the variable names make any sense
            // ReSharper disable InconsistentNaming
            var RxS = r.Cross(s);
            var QmP = q - p;
            // ReSharper restore InconsistentNaming

            if (Math.Abs(RxS - 0) < 0.001)
            {
                // ReSharper disable InconsistentNaming
                var QmPxR = QmP.Cross(r);
                // ReSharper restore InconsistentNaming
                return Math.Abs(QmPxR - 0) < 0.001 ? Geometry.Parallelism.Collinear : Geometry.Parallelism.Parallel;
            }

            return Geometry.Parallelism.None;
        }

        /// <summary>
        /// Given a distance along the line (in units of line length) get the point this distance evaluates to
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector2 PointAlongLine(float t)
        {
            return Position + Direction * t;
        }
    }
}
