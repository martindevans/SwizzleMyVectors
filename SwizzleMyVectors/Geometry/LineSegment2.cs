using System.Numerics;

namespace SwizzleMyVectors.Geometry
{
    public struct LineSegment2
    {
        /// <summary>
        /// Specifies the starting point of the segment.
        /// </summary>
        public Vector2 Start;

        /// <summary>
        /// Specifies the ending point of the segment.
        /// </summary>
        public Vector2 End;

        /// <summary>
        /// Return a new Ray2, pointing along this segment (with normalized direction vector)
        /// </summary>
        /// <returns></returns>
        public Ray2 Line
        {
            get { return new Ray2(Start, Vector2.Normalize(End - Start)); }
        }

        /// <summary>
        /// Return a new Ray2, pointing along this segment (with non normalized direction vector)
        /// </summary>
        /// <returns></returns>
        public Ray2 LongLine => new Ray2(Start, End - Start);

        /// <summary>
        /// Creates a new instance of LineSegment2.
        /// </summary>
        /// <param name="start">The starting point of the Ray.</param><param name="end">end of the segment</param>
        public LineSegment2(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        #region equality
        /// <summary>
        /// Determines whether two instances of LineSegment2 are equal.
        /// </summary>
        /// <param name="a">The object to the left of the equality operator.</param><param name="b">The object to the right of the equality operator.</param>
        public static bool operator ==(LineSegment2 a, LineSegment2 b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether two instances of LineSegment2 are not equal.
        /// </summary>
        /// <param name="a">The object to the left of the inequality operator.</param><param name="b">The object to the right of the inequality operator.</param>
        public static bool operator !=(LineSegment2 a, LineSegment2 b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Determines whether the specified LineSegment2 is equal to the current Ray.
        /// </summary>
        /// <param name="other">The LineSegment2 to compare with the current LineSegment2.</param>
        public bool Equals(LineSegment2 other)
        {
            return Start.Equals(other.Start) && End.Equals(other.End);
        }

        /// <summary>
        /// Determines whether two instances of Ray are equal.
        /// </summary>
        /// <param name="obj">The Object to compare with the current Ray.</param>
        public override bool Equals(object obj)
        {
            return obj is LineSegment2 segment2 && Equals(segment2);
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
// ReSharper disable NonReadonlyFieldInGetHashCode

            var hash = 17;
            hash = hash * 31 + Start.GetHashCode();
            hash = hash * 31 + End.GetHashCode();
            return hash;

// ReSharper restore NonReadonlyFieldInGetHashCode
        }
        #endregion

        /// <summary>
        /// Returns a String that represents the current Ray.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Start:{0},End:{1}", Start, End);
        }

        #region closest point
        /// <summary>
        /// Calculate the closest point on this ray to the given point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2 ClosestPoint(Vector2 point)
        {
            return ClosestPoint(ref point, out var t);
        }

        /// <summary>
        /// Calculate the closest point on this ray to the given point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="result">distance along the ray at which the closest point lies</param>
        /// <returns></returns>
        public Vector2 ClosestPoint(ref Vector2 point, out float result)
        {
            var pos = LongLine.ClosestPoint(ref point, out result);

            if (result > 1)
            {
                result = 1;
                return End;
            }

            if (result < 0)
            {
                result = 0;
                return Start;
            }

            return pos;
        }

        /// <summary>
        /// Gets how far along this line the closest point is (in units of direction length)
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float ClosestPointDistanceAlongSegment(Vector2 point)
        {
            ClosestPointDistanceAlongSegment(ref point, out var dist);
            return dist;
        }

        /// <summary>
        /// Gets how far along this line the closest point is (in units of direction length)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public void ClosestPointDistanceAlongSegment(ref Vector2 point, out float distance)
        {
            LongLine.ClosestPointDistanceAlongLine(ref point, out distance);

            distance = distance.Clamp(0, 1);
        }

        public float DistanceToPoint(Vector2 point)
        {
            DistanceToPoint(ref point, out var result);
            return result;
        }

        public void DistanceToPoint(ref Vector2 point, out float distance)
        {
            distance = (point - ClosestPoint(point)).Length();
        }
        #endregion

        #region intersection
        public LinesIntersection2? Intersects(Ray2 ray, out Parallelism parallelism)
        {
            var intersection = LongLine.Intersects(ray, out parallelism);

            if (!intersection.HasValue)
                return null;

            if (intersection.Value.DistanceAlongA <= 0 || intersection.Value.DistanceAlongA >= 1)
                return null;

            return intersection;
        }

        public LinesIntersection2? Intersects(Ray2 ray)
        {
            return Intersects(ray, out var _);
        }

        public LinesIntersection2? Intersects(LineSegment2 segment, out Parallelism parallelism)
        {
            var intersection = LongLine.Intersects(segment.LongLine, out parallelism);

            if (!intersection.HasValue)
                return null;

            if (intersection.Value.DistanceAlongA < 0 || intersection.Value.DistanceAlongA > 1)
                return null;

            if (intersection.Value.DistanceAlongB < 0 || intersection.Value.DistanceAlongB > 1)
                return null;

            return intersection;
        }

        public LinesIntersection2? Intersects(LineSegment2 segment)
        {
            return Intersects(segment, out Parallelism _);
        }
        #endregion

        public LineSegment2 Transform(Matrix4x4 transform)
        {
            var s = Vector3.Transform(Start.X_Y(0), transform).XZ();
            var e = Vector3.Transform(End.X_Y(0), transform).XZ();

            return new LineSegment2(s, e);
        }
    }
}
