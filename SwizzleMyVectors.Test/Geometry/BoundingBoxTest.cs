using System;
using System.Linq;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors.Geometry;

namespace SwizzleMyVectors.Test.Geometry
{
    [TestClass]
    public class BoundingBoxTest
    {
        readonly BoundingBox _a = new BoundingBox(new Vector3(0), new Vector3(10));
        readonly BoundingBox _b = new BoundingBox(new Vector3(5), new Vector3(15));
        readonly BoundingBox _c = new BoundingBox(new Vector3(3), new Vector3(5));
        readonly BoundingBox _d = new BoundingBox(new Vector3(100), new Vector3(110));

        [TestMethod]
        public void Intersects()
        {
            Assert.IsTrue(_a.Intersects(_b));
            Assert.IsTrue(_a.Intersects(_c));
            Assert.IsFalse(_a.Intersects(_d));
        }

        [TestMethod]
        public void Contains()
        {

            Assert.IsFalse(_a.Contains(_b) == ContainmentType.Contains);
            Assert.IsTrue(_a.Contains(_c) == ContainmentType.Contains);
            Assert.IsFalse(_a.Contains(_d) == ContainmentType.Contains);
        }

        [TestMethod]
        public void AssertThat_Inflate_IncreasesSize()
        {
            BoundingBox s = new BoundingBox(new Vector3(0), new Vector3(10));

            Assert.AreEqual(new Vector3(-5), s.Inflate(10).Min);
            Assert.AreEqual(new Vector3(15), s.Inflate(10).Max);
        }

        [TestMethod]
        public void AssertThat_Inflate_ByNegativeValue_DecreasesSize()
        {
            BoundingBox s = new BoundingBox(new Vector3(0), new Vector3(10));

            Assert.AreEqual(new Vector3(3), s.Inflate(-6).Min);
            Assert.AreEqual(new Vector3(7), s.Inflate(-6).Max);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AssertThat_InflateSphere_ByNegativeValueLargerThanSize_Throws()
        {
            BoundingBox s = new BoundingBox(Vector3.Zero, new Vector3(10));

            var result = s.Inflate(-50);
        }

        [TestMethod]
        public void Construct_BoxAroundSphere_ContainsSphere()
        {
            var sphere = new BoundingSphere(Vector3.One, 10);
            var bounds = new BoundingBox(sphere);

            Assert.AreEqual(new Vector3(-9), bounds.Min);
            Assert.AreEqual(new Vector3(11), bounds.Max);
        }

        [TestMethod]
        public void CreateFromSphere_ContainsSphere()
        {
            var sphere = new BoundingSphere(Vector3.One, 10);
            var bounds = BoundingBox.CreateFromSphere(sphere);

            Assert.AreEqual(new Vector3(-9), bounds.Min);
            Assert.AreEqual(new Vector3(11), bounds.Max);
        }

        [TestMethod]
        public void CreateMerged_ExpandsMax()
        {
            var a = new BoundingBox(new Vector3(5, 6, 7), new Vector3(8, 9, 10));
            var b = new BoundingBox(new Vector3(6, 7, 8), new Vector3(9, 10, 11));

            var m = BoundingBox.CreateMerged(a, b);

            Assert.AreEqual(Vector3.Min(a.Min, b.Min), m.Min);
            Assert.AreEqual(Vector3.Max(a.Max, b.Max), m.Max);
        }

        [TestMethod]
        public void CreateMerged_ExpandsMin()
        {
            var a = new BoundingBox(new Vector3(5, 6, 7), new Vector3(8, 9, 10));
            var b = new BoundingBox(new Vector3(4, 5, 6), new Vector3(7, 8, 9));

            var m = BoundingBox.CreateMerged(a, b);

            Assert.AreEqual(Vector3.Min(a.Min, b.Min), m.Min);
            Assert.AreEqual(Vector3.Max(a.Max, b.Max), m.Max);
        }

        [TestMethod]
        public void CreateFromPoints_ContainsAllPoints()
        {
            var r = new Random(2234897);
            var p = Enumerable.Range(0, 1024).Select(_ => new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble())).ToArray();

            var b = BoundingBox.CreateFromPoints(p);

            foreach (var point in p)
                Assert.IsTrue(b.Contains(point) != ContainmentType.Disjoint);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFromPoints_ThrowsWithNull()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            BoundingBox.CreateFromPoints(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateFromPoints_ThrowsWithNoPoints()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            BoundingBox.CreateFromPoints(new Vector3[0]);
        }

        [TestMethod]
        public void Equals_TrueWithEqual()
        {
            var a = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));

            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void Equals_FalseWithNotEqual()
        {
            var a = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new BoundingBox(new Vector3(2, 2, 3), new Vector3(4, 5, 6));

            Assert.AreNotEqual(a, b);
        }

        [TestMethod]
        public void OpEquals_TrueWithEqual()
        {
            var a = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));

            Assert.IsTrue(a == b);
        }

        [TestMethod]
        public void OpEquals_FalseWithNotEqual()
        {
            var a = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new BoundingBox(new Vector3(2, 2, 3), new Vector3(4, 5, 6));

            Assert.IsFalse(a == b);
        }

        [TestMethod]
        public void OpNotEquals_FalseWithEqual()
        {
            var a = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));

            Assert.IsFalse(a != b);
        }

        [TestMethod]
        public void OpNotEquals_TrueWithNotEqual()
        {
            var a = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new BoundingBox(new Vector3(2, 2, 3), new Vector3(4, 5, 6));

            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void GetHashCode_EqualForSame()
        {
            var a = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));

            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [TestMethod]
        public void Volume()
        {
            var a = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));

            Assert.AreEqual(27, a.Volume());
        }

        [TestMethod]
        public void GetCorners_GetsAllEightCorners()
        {
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var c = b.GetCorners();

            Assert.AreEqual(8, c.Length);

            Assert.IsTrue(c.Contains(new Vector3(1, 2, 3)));
            Assert.IsTrue(c.Contains(new Vector3(1, 2, 6)));
            Assert.IsTrue(c.Contains(new Vector3(1, 5, 3)));
            Assert.IsTrue(c.Contains(new Vector3(1, 5, 6)));
            Assert.IsTrue(c.Contains(new Vector3(4, 2, 3)));
            Assert.IsTrue(c.Contains(new Vector3(4, 2, 6)));
            Assert.IsTrue(c.Contains(new Vector3(4, 5, 3)));
            Assert.IsTrue(c.Contains(new Vector3(4, 5, 6)));
        }

        [TestMethod]
        public void GetCorners_GetsAllEightCorners_WithInputArray()
        {
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var c = new Vector3[8];
            b.GetCorners(c);

            Assert.AreEqual(8, c.Length);

            Assert.IsTrue(c.Contains(new Vector3(1, 2, 3)));
            Assert.IsTrue(c.Contains(new Vector3(1, 2, 6)));
            Assert.IsTrue(c.Contains(new Vector3(1, 5, 3)));
            Assert.IsTrue(c.Contains(new Vector3(1, 5, 6)));
            Assert.IsTrue(c.Contains(new Vector3(4, 2, 3)));
            Assert.IsTrue(c.Contains(new Vector3(4, 2, 6)));
            Assert.IsTrue(c.Contains(new Vector3(4, 5, 3)));
            Assert.IsTrue(c.Contains(new Vector3(4, 5, 6)));
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void GetCorners_ThrowsWithSmallArray()
        {
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var c = new Vector3[7];
            b.GetCorners(c);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void GetCorners_ThrowsWithNullArray()
        {
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));

            // ReSharper disable once AssignNullToNotNullAttribute
            b.GetCorners(null);
        }

        [TestMethod]
        public void ToString_ContainsMinMax()
        {
            var min = new Vector3(1, 2, 3);
            var max = new Vector3(4, 5, 6);
            var b = new BoundingBox(min, max);

            Assert.IsTrue(b.ToString().Contains(min.ToString()));
            Assert.IsTrue(b.ToString().Contains(max.ToString()));
        }

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
        public void PlaneIntersections()
        {
            TestPlaneInDirection(Vector3.UnitX);
            TestPlaneInDirection(Vector3.UnitY);
            TestPlaneInDirection(Vector3.UnitZ);

            TestPlaneInDirection(-Vector3.UnitX, true);
            TestPlaneInDirection(-Vector3.UnitY, true);
            TestPlaneInDirection(-Vector3.UnitZ, true);
        }

        [TestMethod]
        public void ContainsPoint_Contains()
        {
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var p = new Vector3(2, 3, 4);

            Assert.AreEqual(ContainmentType.Contains, b.Contains(p));
        }

        [TestMethod]
        public void ContainsPoint_Intersects()
        {
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));

            //Touching min axes
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new Vector3(1, 3, 4)));
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new Vector3(2, 2, 4)));
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new Vector3(2, 3, 3)));

            //Touching max axes
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new Vector3(4, 3, 4)));
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new Vector3(2, 5, 4)));
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new Vector3(2, 3, 6)));
        }

        [TestMethod]
        public void ContainsPoint_Disjoint()
        {
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));

            //Below min axes
            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(new Vector3(0, 3, 4)));
            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(new Vector3(2, 1, 4)));
            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(new Vector3(2, 3, 2)));

            //Above max axes
            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(new Vector3(5, 3, 4)));
            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(new Vector3(2, 6, 4)));
            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(new Vector3(2, 3, 7)));
        }

        [TestMethod]
        public void ContainsSphere_Contains()
        {
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(6, 7, 8));
            var s = new BoundingSphere(new Vector3(3, 4, 5), 1);

            Assert.AreEqual(ContainmentType.Contains, b.Contains(s));
        }

        [TestMethod]
        public void ContainsSphere_Intersects()
        {
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(6, 7, 8));

            //on min axes
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new BoundingSphere(new Vector3(1, 4, 5), 1)));
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new BoundingSphere(new Vector3(3, 2, 5), 1)));
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new BoundingSphere(new Vector3(3, 4, 3), 1)));

            //on max axes
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new BoundingSphere(new Vector3(6, 4, 5), 1)));
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new BoundingSphere(new Vector3(3, 7, 5), 1)));
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new BoundingSphere(new Vector3(3, 4, 8), 1)));

            //touching min axes
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new BoundingSphere(new Vector3(0, 4, 5), 1)));
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new BoundingSphere(new Vector3(1, 1, 3), 1)));
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new BoundingSphere(new Vector3(1, 2, 2), 1)));

            //touching max axes
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new BoundingSphere(new Vector3(7, 7, 8), 1)));
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new BoundingSphere(new Vector3(6, 8, 8), 1)));
            Assert.AreEqual(ContainmentType.Intersects, b.Contains(new BoundingSphere(new Vector3(6, 7, 9), 1)));
        }

        [TestMethod]
        public void ContainsSphere_Disjoint()
        {
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(6, 7, 8));

            //below min axes
            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(new BoundingSphere(new Vector3(-1, 4, 5), 1)));
            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(new BoundingSphere(new Vector3(3, 0, 5), 1)));
            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(new BoundingSphere(new Vector3(3, 4, 1), 1)));

            //above max axes
            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(new BoundingSphere(new Vector3(8, 4, 5), 1)));
            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(new BoundingSphere(new Vector3(6, 9, 5), 1)));
            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(new BoundingSphere(new Vector3(6, 7, 10), 1)));

            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(new BoundingSphere(new Vector3(1, 2, 0), 1)));
            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(new BoundingSphere(new Vector3(1, 0, 3), 1)));
            Assert.AreEqual(ContainmentType.Disjoint, b.Contains(new BoundingSphere(new Vector3(-1, 2, 3), 1)));
        }
    }
}
