namespace SwizzleMyVectors.Geometry
{
    public enum Parallelism
    {
        /// <summary>
        /// Lines are not parallel
        /// </summary>
        None,

        /// <summary>
        /// Lines are parallel (i.e. point in the same direction)
        /// </summary>
        Parallel,

        /// <summary>
        /// Line are collinear (i.e. point in the same direction and pass through the same point)
        /// </summary>
        Collinear
    }
}
