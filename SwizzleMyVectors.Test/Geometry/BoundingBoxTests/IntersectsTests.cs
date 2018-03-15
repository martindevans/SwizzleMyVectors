using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors.Geometry;

namespace SwizzleMyVectors.Test.Geometry.BoundingBoxTests
{
    [TestClass]
    public class IntersectsTests
    {
        private readonly BoundingBox _a = new BoundingBox(new Vector3(0), new Vector3(10));
        private readonly BoundingBox _b = new BoundingBox(new Vector3(5), new Vector3(15));
        private readonly BoundingBox _c = new BoundingBox(new Vector3(3), new Vector3(5));
        private readonly BoundingBox _d = new BoundingBox(new Vector3(100), new Vector3(110));

        #region box/box
        [TestMethod]
        public void OverlappingBoxesIntersect()
        {
            var a = new BoundingBox(new Vector3(0), new Vector3(10));
            var b = new BoundingBox(new Vector3(5), new Vector3(15));

            Assert.IsTrue(a.Intersects(b));
            Assert.IsTrue(b.Intersects(a));
        }

        [TestMethod]
        public void ContainedBoxesIntersect()
        {
            var a = new BoundingBox(new Vector3(0), new Vector3(10));
            var b = new BoundingBox(new Vector3(1), new Vector3(9));

            Assert.IsTrue(a.Intersects(b));
            Assert.IsTrue(b.Intersects(a));
        }

        [TestMethod]
        public void DisjointBoxesDoNotIntersect()
        {
            var a = new BoundingBox(new Vector3(0), new Vector3(10));
            var b = new BoundingBox(new Vector3(11), new Vector3(19));

            Assert.IsFalse(a.Intersects(b));
            Assert.IsFalse(b.Intersects(a));
        }
        #endregion

        #region box/plane
        private static void TestPlaneInDirection(Vector3 dir, bool inv = false)
        {
            var b = new BoundingBox(new Vector3(10), new Vector3(20));

            var p1 = new Plane(dir, -15 * (inv ? -1 : 1));
            Assert.AreEqual(PlaneIntersectionType.Intersecting, b.Intersects(p1));

            var p2 = new Plane(dir, -5);
            Assert.AreEqual(inv ? PlaneIntersectionType.Back : PlaneIntersectionType.Front, b.Intersects(p2));

            var p3 = new Plane(dir, -35);
            Assert.AreEqual(PlaneIntersectionType.Back, b.Intersects(p3));
        }


        [TestMethod]
        public void BoxIntersectsPlane()
        {
            TestPlaneInDirection(Vector3.UnitX);
            TestPlaneInDirection(Vector3.UnitY);
            TestPlaneInDirection(Vector3.UnitZ);

            TestPlaneInDirection(-Vector3.UnitX, true);
            TestPlaneInDirection(-Vector3.UnitY, true);
            TestPlaneInDirection(-Vector3.UnitZ, true);
        }
        #endregion

        #region box/sphere
        [TestMethod]
        public void OverlappingSphereIntersects()
        {
            var a = new BoundingBox(new Vector3(5), new Vector3(10));
            var b = new BoundingSphere(new Vector3(8, 9, 10), 1);

            Assert.IsTrue(a.Intersects(b));
        }

        [TestMethod]
        public void OverlappingSphereIntersects_Fuzz()
        {
            var r = new Random(23487);

            // Generate overlapping box and sphere by following process:
            // - Generate random bounding box
            // - Pick start point inside box
            // - Pick random radius
            // - Offset start point by radius or less
            for (var i = 0; i < 1000; i++)
            {
                var min = new Vector3((float)r.NextDouble() * 10, (float)r.NextDouble() * 10, (float)r.NextDouble() * 10);
                var siz = new Vector3((float)r.NextDouble() * 10, (float)r.NextDouble() * 10, (float)r.NextDouble() * 10);
                var a = new BoundingBox(min, min + siz);

                // Generate a sphere with a center point initially inside the box, offset by a distance less than the radius in a random direction
                var radi = (float)Math.Abs(r.NextDouble() * 10);
                var lerp = (float)r.NextDouble();
                var cent = (min * lerp) + a.Max * (1 - lerp) + Vector3.Normalize(new Vector3((float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f, (float)r.NextDouble() - 0.5f)) * radi * 0.99f * (float)r.NextDouble();
                var b = new BoundingSphere(cent, radi);

                Assert.IsTrue(a.Intersects(b), i.ToString());
            }
        }

        [TestMethod]
        public void NonOverlappingSphereDoesNotIntersect()
        {
            var a = new BoundingBox(new Vector3(10, 11, 12), new Vector3(40, 50, 60));

            Assert.IsFalse(a.Intersects(new BoundingSphere(new Vector3(100, 35, 35), 1)));
            Assert.IsFalse(a.Intersects(new BoundingSphere(new Vector3(35, 100, 100), 1)));
            Assert.IsFalse(a.Intersects(new BoundingSphere(new Vector3(100, 100, 55), 1)));
        }
        #endregion

        #region box/ray
        [TestMethod]
        public void Intersects_Ray3_Intersects()
        {
            var b = new BoundingBox(new Vector3(3, 4, 5), new Vector3(6, 7, 8));
            var r = new Ray3(new Vector3(0, 0, 0), new Vector3(1, 1, 1));

            Assert.IsNotNull(b.Intersects(r));
            Assert.AreEqual(r.Intersects(b), b.Intersects(r));
        }

        [TestMethod]
        public void Intersects_Ray3_Misses()
        {
            var b = new BoundingBox(new Vector3(3, 4, 5), new Vector3(6, 7, 8));
            var r = new Ray3(new Vector3(0, 0, 0), new Vector3(1, 0, 0));

            Assert.IsNull(b.Intersects(r));
            Assert.AreEqual(r.Intersects(b), b.Intersects(r));
        }
        #endregion

        [TestMethod]
        public void Intersects_BoundingSphere_Contains()
        {
            var b = new BoundingBox(new Vector3(0, 0, 0), new Vector3(10, 10, 10));
            var s = new BoundingSphere(new Vector3(5, 5, 5), 3);

            Assert.AreEqual(ContainmentType.Contains, b.Contains(s));
        }

        [TestMethod]
        public void Intersects_BoundingSphere_Intersects()
        {
            var b = new BoundingBox(new Vector3(0, 0, 0), new Vector3(10, 10, 10));
            var s = new BoundingSphere(new Vector3(8, 8, 8), 3);

            Assert.AreEqual(ContainmentType.Intersects, b.Contains(s));
        }

        [TestMethod]
        public void Intersects_BoundingSphere_Disjoint()
        {
            var b = new BoundingBox(new Vector3(0, 0, 0), new Vector3(10, 10, 10));
            var s = new BoundingSphere(new Vector3(18, 18, 18), 3);

            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(s));
        }
    }
}
