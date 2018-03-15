using System;
using System.Linq;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors.Geometry;

namespace SwizzleMyVectors.Test.Geometry.BoundingBoxTests
{
    [TestClass]
    public class BoundingBoxTest
    {
        private readonly BoundingBox _a = new BoundingBox(new Vector3(0), new Vector3(10));
        private readonly BoundingBox _b = new BoundingBox(new Vector3(5), new Vector3(15));
        private readonly BoundingBox _c = new BoundingBox(new Vector3(3), new Vector3(5));
        private readonly BoundingBox _d = new BoundingBox(new Vector3(100), new Vector3(110));

        [TestMethod]
        public void ExtentIsMaxMinusMin()
        {
            Assert.AreEqual(_a.Max - _a.Min, _a.Extent);
        }

        [TestMethod]
        public void Contains_BoundingBox()
        {

            Assert.IsFalse(_a.Contains(_b) == ContainmentType.Contains);
            Assert.IsTrue(_a.Contains(_c) == ContainmentType.Contains);
            Assert.IsFalse(_a.Contains(_d) == ContainmentType.Contains);
        }

        [TestMethod]
        public void AssertThat_Inflate_IncreasesSize()
        {
            var s = new BoundingBox(new Vector3(0), new Vector3(10));

            Assert.AreEqual(new Vector3(-5), s.Inflate(10).Min);
            Assert.AreEqual(new Vector3(15), s.Inflate(10).Max);
        }

        [TestMethod]
        public void AssertThat_Inflate_ByNegativeValue_DecreasesSize()
        {
            var s = new BoundingBox(new Vector3(0), new Vector3(10));

            Assert.AreEqual(new Vector3(3), s.Inflate(-6).Min);
            Assert.AreEqual(new Vector3(7), s.Inflate(-6).Max);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AssertThat_InflateSphere_ByNegativeValueLargerThanSize_Throws()
        {
            var s = new BoundingBox(Vector3.Zero, new Vector3(10));

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            s.Inflate(-50);
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
