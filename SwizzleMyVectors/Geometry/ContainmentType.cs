
namespace SwizzleMyVectors.Geometry
{
    public enum ContainmentType
    {
        /// <summary>
        /// Queried objects do not intersect in any way
        /// </summary>
        Disjoint,

        /// <summary>
        /// First query object contains second query object
        /// </summary>
        Contains,

        /// <summary>
        /// Objects intersect (but first does not contain second)
        /// </summary>
        Intersects,
    }
}
