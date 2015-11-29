using System.Numerics;

namespace SwizzleMyVectors.Geometry
{
    public struct LinesIntersection2
    {
        /// <summary>
        /// The position where the two rays intersect
        /// </summary>
        public readonly Vector2 Position;

        /// <summary>
        /// The distance along ray A. Units are in ray lengths, so 0 indicates the start, 1 indicates the end.
        /// </summary>
        public readonly float DistanceAlongA;

        /// <summary>
        /// The distance along ray B. Units are in ray lengths, so 0 indicates the start, 1 indicates the end.
        /// </summary>
        public readonly float DistanceAlongB;

        public LinesIntersection2(Vector2 position, float distanceAlongLineA, float distanceAlongLineB)
        {
            Position = position;
            DistanceAlongA = distanceAlongLineA;
            DistanceAlongB = distanceAlongLineB;
        }
    }
}
