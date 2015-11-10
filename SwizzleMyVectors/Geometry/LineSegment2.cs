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
        /// Return a new Ray2, pointing along this segment (with none normalized direction vector)
        /// </summary>
        /// <returns></returns>
        public Ray2 LongLine
        {
            get { return new Ray2(Start, End - Start); }
        }

        /// <summary>
        /// Creates a new instance of LineSegment2.
        /// </summary>
        /// <param name="start">The starting point of the Ray.</param><param name="end">end of the segment</param>
        public LineSegment2(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

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
            return obj is LineSegment2 && Equals((LineSegment2)obj);
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
// ReSharper disable NonReadonlyFieldInGetHashCode

            int hash = 17;
            hash = hash * 31 + Start.GetHashCode();
            hash = hash * 31 + End.GetHashCode();
            return hash;

// ReSharper restore NonReadonlyFieldInGetHashCode
        }

        /// <summary>
        /// Returns a String that represents the current Ray.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Start:{0},End:{1}", Start, End);
        }
    }
}
