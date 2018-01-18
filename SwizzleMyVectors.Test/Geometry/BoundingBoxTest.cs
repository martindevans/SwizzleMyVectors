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
    }
}
