using System;
using System.Numerics;

namespace SwizzleMyVectors.Geometry
{
    public struct BoundingFrustum
    {
        /// <summary>
        /// Specifies the total number of corners (8) in the BoundingFrustum.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const int CornerCount = 8;

        /// <summary>
        /// The number of planes (6) in the frustum.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const int PlaneCount = 6;

        private bool _cornersDirty;
        private readonly Vector3[] _corners;

        private bool _planesDirty;
        private readonly Plane[] _planes;

        private Matrix4x4 _matrix;
        /// <summary>
        /// Gets or sets the Matrix that describes this bounding frustum.
        /// </summary>
        public Matrix4x4 Matrix
        {
            get { return _matrix; }
            set
            {
                _matrix = value;
                _planesDirty = true;
                _cornersDirty = true;
            }
        }

        /// <summary>
        /// Gets the near plane of the frustum.
        /// </summary>
        public Plane Near
        {
            get { return CreatePlanes()[0]; }
        }

        /// <summary>
        /// Gets the far plane of the frustum.
        /// </summary>
        public Plane Far
        {
            get { return CreatePlanes()[1]; }
        }

        /// <summary>
        /// Gets the left plane of the frustum.
        /// </summary>
        public Plane Left
        {
            get { return CreatePlanes()[2]; }
        }

        /// <summary>
        /// Gets the right plane of the frustum.
        /// </summary>
        public Plane Right
        {
            get { return CreatePlanes()[3]; }
        }

        /// <summary>
        /// Gets the top plane of the frustum.
        /// </summary>
        public Plane Top
        {
            get { return CreatePlanes()[4]; }
        }

        /// <summary>
        /// Gets the bottom plane of the frustum.
        /// </summary>
        public Plane Bottom
        {
            get { return CreatePlanes()[5]; }
        }

        /// <summary>
        /// Creates a new instance of BoundingFrustum.
        /// </summary>
        /// <param name="value">Combined matrix that usually takes view × projection matrix.</param>
        public BoundingFrustum(Matrix4x4 value)
        {
            _planesDirty = true;
            _planes = new Plane[PlaneCount];

            _cornersDirty = true;
            _corners = new Vector3[CornerCount];

            _matrix = value;
        }

        private Plane[] CreatePlanes()
        {
            if (_planesDirty)
            {
                _planes[0] = Plane.Normalize(new Plane(-_matrix.M13, -_matrix.M23, -_matrix.M33, -_matrix.M43));
                _planes[1] = Plane.Normalize(new Plane(_matrix.M13 - _matrix.M14, _matrix.M23 - _matrix.M24, _matrix.M33 - _matrix.M34, _matrix.M43 - _matrix.M44));
                _planes[2] = Plane.Normalize(new Plane(-_matrix.M14 - _matrix.M11, -_matrix.M24 - _matrix.M21, -_matrix.M34 - _matrix.M31, -_matrix.M44 - _matrix.M41));
                _planes[3] = Plane.Normalize(new Plane(_matrix.M11 - _matrix.M14, _matrix.M21 - _matrix.M24, _matrix.M31 - _matrix.M34, _matrix.M41 - _matrix.M44));
                _planes[4] = Plane.Normalize(new Plane(_matrix.M12 - _matrix.M14, _matrix.M22 - _matrix.M24, _matrix.M32 - _matrix.M34, _matrix.M42 - _matrix.M44));
                _planes[5] = Plane.Normalize(new Plane(-_matrix.M14 - _matrix.M12, -_matrix.M24 - _matrix.M22, -_matrix.M34 - _matrix.M32, -_matrix.M44 - _matrix.M42));

                _planesDirty = false;
            }

            return _planes;
        }

        private Vector3[] CreateCorners()
        {
            CreatePlanes();

            if (_cornersDirty)
            {
                IntersectionPoint(ref _planes[0], ref _planes[2], ref _planes[4], out _corners[0]);
                IntersectionPoint(ref _planes[0], ref _planes[3], ref _planes[4], out _corners[1]);
                IntersectionPoint(ref _planes[0], ref _planes[3], ref _planes[5], out _corners[2]);
                IntersectionPoint(ref _planes[0], ref _planes[2], ref _planes[5], out _corners[3]);
                IntersectionPoint(ref _planes[1], ref _planes[2], ref _planes[4], out _corners[4]);
                IntersectionPoint(ref _planes[1], ref _planes[3], ref _planes[4], out _corners[5]);
                IntersectionPoint(ref _planes[1], ref _planes[3], ref _planes[5], out _corners[6]);
                IntersectionPoint(ref _planes[1], ref _planes[2], ref _planes[5], out _corners[7]);

                _cornersDirty = false;
            }

            return _corners;
        }

        private static void IntersectionPoint(ref Plane a, ref Plane b, ref Plane c, out Vector3 result)
        {
            // Formula used
            //                d1 ( N2 * N3 ) + d2 ( N3 * N1 ) + d3 ( N1 * N2 )
            //P =   -------------------------------------------------------------------------
            //                             N1 . ( N2 * N3 )
            //
            // Note: N refers to the normal, d refers to the displacement. '.' means dot product. '*' means cross product

            Vector3 cross = Vector3.Cross(b.Normal, c.Normal);

            float f = Vector3.Dot(a.Normal, cross);
            f *= -1.0f;

            Vector3 v1 = (a.D * (Vector3.Cross(b.Normal, c.Normal)));
            Vector3 v2 = (b.D * (Vector3.Cross(c.Normal, a.Normal)));
            Vector3 v3 = (c.D * (Vector3.Cross(a.Normal, b.Normal)));

            result.X = (v1.X + v2.X + v3.X) / f;
            result.Y = (v1.Y + v2.Y + v3.Y) / f;
            result.Z = (v1.Z + v2.Z + v3.Z) / f;
        }

        /// <summary>
        /// Determines whether two instances of BoundingFrustum are equal.
        /// </summary>
        /// <param name="a">The BoundingFrustum to the left of the equality operator.</param><param name="b">The BoundingFrustum to the right of the equality operator.</param>
        public static bool operator ==(BoundingFrustum a, BoundingFrustum b)
        {
            return a._matrix == b._matrix;
        }

        /// <summary>
        /// Determines whether two instances of BoundingFrustum are not equal.
        /// </summary>
        /// <param name="a">The BoundingFrustum to the left of the inequality operator.</param><param name="b">The BoundingFrustum to the right of the inequality operator.</param>
        public static bool operator !=(BoundingFrustum a, BoundingFrustum b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Gets an array of points that make up the corners of the BoundingFrustum.
        /// </summary>
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public Vector3[] GetCorners()
        {
            return (Vector3[])CreateCorners().Clone();
        }

        /// <summary>
        /// Gets an array of points that make up the corners of the BoundingFrustum.
        /// </summary>
        /// <param name="corners">An existing array of at least 8 Vector3 points where the corners of the BoundingFrustum are written.</param>
        public void GetCorners(Vector3[] corners)
        {
            if (corners == null) throw new ArgumentNullException("corners");
            if (corners.Length < CornerCount) throw new ArgumentOutOfRangeException("corners");

            CreateCorners().CopyTo(corners, 0);
        }

        /// <summary>
        /// Determines whether the specified BoundingFrustum is equal to the current BoundingFrustum.
        /// </summary>
        /// <param name="other">The BoundingFrustum to compare with the current BoundingFrustum.</param>
        public bool Equals(BoundingFrustum other)
        {
            return this == other;
        }

        /// <summary>
        /// Determines whether the specified Object is equal to the BoundingFrustum.
        /// </summary>
        /// <param name="obj">The Object to compare with the current BoundingFrustum.</param>
        public override bool Equals(object obj)
        {
            return obj is BoundingFrustum && Equals((BoundingFrustum)obj);
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            // Ugly, but inline with how MS define their vector types
            // ReSharper disable once NonReadonlyFieldInGetHashCode
            return _matrix.GetHashCode();
        }

        /// <summary>
        /// Returns a String that represents the current BoundingFrustum.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{{Near: {0} Far:{1} Left:{2} Right:{3} Top:{4} Bottom:{5}}}", _planes[0], _planes[1], _planes[2], _planes[3], _planes[4], _planes[5]);
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum intersects the specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check for intersection.</param>
        public bool Intersects(BoundingBox box)
        {
            bool result;
            Intersects(ref box, out result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum intersects a BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingFrustum and BoundingBox intersect; false otherwise.</param>
        public void Intersects(ref BoundingBox box, out bool result)
        {
            ContainmentType containment;
            Contains(ref box, out containment);
            result = containment != ContainmentType.Disjoint;
        }

        ///// <summary>
        ///// Checks whether the current BoundingFrustum intersects the specified BoundingFrustum.
        ///// </summary>
        ///// <param name="frustum">The BoundingFrustum to check for intersection.</param>
        //public bool Intersects(BoundingFrustum frustum);
        ///// <summary>
        ///// Checks whether the current BoundingFrustum intersects the specified Plane.
        ///// </summary>
        ///// <param name="plane">The Plane to check for intersection.</param>
        //public PlaneIntersectionType Intersects(Plane plane);
        ///// <summary>
        ///// Checks whether the current BoundingFrustum intersects a Plane.
        ///// </summary>
        ///// <param name="plane">The Plane to check for intersection with.</param><param name="result">[OutAttribute] An enumeration indicating whether the BoundingFrustum intersects the Plane.</param>
        //public void Intersects(ref Plane plane, out PlaneIntersectionType result);
        ///// <summary>
        ///// Checks whether the current BoundingFrustum intersects the specified Ray.
        ///// </summary>
        ///// <param name="ray">The Ray to check for intersection.</param>
        //public float? Intersects(Ray ray);
        ///// <summary>
        ///// Checks whether the current BoundingFrustum intersects a Ray.
        ///// </summary>
        ///// <param name="ray">The Ray to check for intersection with.</param><param name="result">[OutAttribute] Distance at which the ray intersects the BoundingFrustum or null if there is no intersection.</param>
        //public void Intersects(ref Ray ray, out float? result);

        /// <summary>
        /// Checks whether the current BoundingFrustum intersects the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check for intersection.</param>
        public bool Intersects(BoundingSphere sphere)
        {
            bool result;
            Intersects(ref sphere, out result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum intersects a BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check for intersection with.</param><param name="result">[OutAttribute] true if the BoundingFrustum and BoundingSphere intersect; false otherwise.</param>
        public void Intersects(ref BoundingSphere sphere, out bool result)
        {
            ContainmentType containment;
            Contains(ref sphere, out containment);
            result = containment != ContainmentType.Disjoint;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum contains the specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to check against the current BoundingFrustum.</param>
        public ContainmentType Contains(BoundingBox box)
        {
            ContainmentType result;
            Contains(ref box, out result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum contains the specified BoundingBox.
        /// </summary>
        /// <param name="box">The BoundingBox to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains(ref BoundingBox box, out ContainmentType result)
        {
            CreatePlanes();

            var intersects = false;
            for (var i = 0; i < PlaneCount; ++i)
            {
                PlaneIntersectionType planeIntersectionType;
                box.Intersects(ref _planes[i], out planeIntersectionType);
                switch (planeIntersectionType)
                {
                    case PlaneIntersectionType.Front:
                        result = ContainmentType.Disjoint;
                        return;
                    case PlaneIntersectionType.Intersecting:
                        intersects = true;
                        break;
                }
            }
            result = intersects ? ContainmentType.Intersects : ContainmentType.Contains;
        }

        ///// <summary>
        ///// Checks whether the current BoundingFrustum contains the specified BoundingFrustum.
        ///// </summary>
        ///// <param name="frustum">The BoundingFrustum to check against the current BoundingFrustum.</param>
        //public ContainmentType Contains(BoundingFrustum frustum);

        /// <summary>
        /// Checks whether the current BoundingFrustum contains the specified point.
        /// </summary>
        /// <param name="point">The point to check against the current BoundingFrustum.</param>
        public ContainmentType Contains(Vector3 point)
        {
            ContainmentType result;
            Contains(ref point, out result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum contains the specified point.
        /// </summary>
        /// <param name="point">The point to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains(ref Vector3 point, out ContainmentType result)
        {
            CreatePlanes();

            for (var i = 0; i < PlaneCount; ++i)
            {
                if (_planes[i].Side(point) > 0)
                {
                    result = ContainmentType.Disjoint;
                    return;
                }
            }
            result = ContainmentType.Contains;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum contains the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to check against the current BoundingFrustum.</param>
        public ContainmentType Contains(BoundingSphere sphere)
        {
            ContainmentType result;
            Contains(ref sphere, out result);
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum contains the specified BoundingSphere.
        /// </summary>
        /// <param name="sphere">The BoundingSphere to test for overlap.</param><param name="result">[OutAttribute] Enumeration indicating the extent of overlap.</param>
        public void Contains(ref BoundingSphere sphere, out ContainmentType result)
        {
            CreatePlanes();

            var intersects = false;
            for (var i = 0; i < PlaneCount; ++i)
            {
                PlaneIntersectionType planeIntersectionType;
                sphere.Intersects(ref _planes[i], out planeIntersectionType);

                switch (planeIntersectionType)
                {
                    case PlaneIntersectionType.Front:
                        result = ContainmentType.Disjoint;
                        return;
                    case PlaneIntersectionType.Intersecting:
                        intersects = true;
                        break;
                }
            }
            result = intersects ? ContainmentType.Intersects : ContainmentType.Contains;
        }
    }
}
