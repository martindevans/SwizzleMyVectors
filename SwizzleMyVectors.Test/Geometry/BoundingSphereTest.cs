using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors.Geometry;

namespace SwizzleMyVectors.Test.Geometry
{
    [TestClass]
    public class BoundingSphereTest
    {
        [TestMethod]
        public void AssertThat_Inflate_IncreasesRadius()
        {
            BoundingSphere s = new BoundingSphere(Vector3.Zero, 10);

            Assert.AreEqual(15, s.Inflate(10).Radius);
        }

        [TestMethod]
        public void AssertThat_Inflate_ByNegativeValue_DecreasesRadius()
        {
            BoundingSphere s = new BoundingSphere(Vector3.Zero, 10);

            Assert.AreEqual(5, s.Inflate(-10).Radius);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AssertThat_Inflate_ByNegativeValueLargerThanDiameter_Throws()
        {
            var s = new BoundingSphere(Vector3.Zero, 10);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            s.Inflate(-50);
        }

        [TestMethod]
        public void Equals_ReturnsTrueForEqual()
        {
            var a = new BoundingSphere(new Vector3(1, 2, 3), 4);
            var b = a;

            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(a.Equals((object)b));
            Assert.IsTrue(a == b);
        }

        [TestMethod]
        public void Equals_ReturnsFalseForNonEqual()
        {
            var a = new BoundingSphere(new Vector3(1, 2, 3), 4);
            var b = new BoundingSphere(new Vector3(5, 6, 7), 8);

            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(a.Equals((object)b));
            Assert.IsFalse(a == b);
        }

        [TestMethod]
        public void NotEquals_ReturnsTrueForNotEqual()
        {
            var a = new BoundingSphere(new Vector3(1, 2, 3), 4);
            var b = new BoundingSphere(new Vector3(5, 6, 7), 8);

            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void NotEquals_ReturnsFalseForEqual()
        {
            var a = new BoundingSphere(new Vector3(1, 2, 3), 4);
            var b = a;

            Assert.IsFalse(a != b);
        }

        [TestMethod]
        public void GetHashCode_IsSameForEqual()
        {
            var a = new BoundingSphere(new Vector3(1, 2, 3), 4);
            var b = new BoundingSphere(new Vector3(1, 2, 3), 4);

            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [TestMethod]
        public void Contains_ContainsSphere()
        {
            var a = new BoundingSphere(Vector3.Zero, 10);
            var b = new BoundingSphere(Vector3.One, 2);

            Assert.AreEqual(ContainmentType.Contains, a.Contains(b));
        }

        [TestMethod]
        public void Contains_ContainsSphere_Small()
        {
            var a = new BoundingSphere(Vector3.Zero, 1);
            var b = new BoundingSphere(new Vector3(0.5f), 0.1f);

            Assert.AreEqual(ContainmentType.Contains, a.Contains(b));
        }

        [TestMethod]
        public void Contains_IntersectsSphere()
        {
            var a = new BoundingSphere(Vector3.Zero, 10);
            var b = new BoundingSphere(new Vector3(10), 8);

            Assert.AreEqual(ContainmentType.Intersects, a.Contains(b));
        }

        [TestMethod]
        public void Contains_DisjointSphere()
        {
            var a = new BoundingSphere(Vector3.Zero, 10);
            var b = new BoundingSphere(new Vector3(15), 8);

            Assert.AreEqual(ContainmentType.Disjoint, a.Contains(b));
        }

        [TestMethod]
        public void ContainsPoint_Fuzz()
        {
            var r = new Random(2436544);

            for (var i = 0; i < 1024; i++)
            {
                var s = new BoundingSphere(new Vector3((float)r.NextDouble() * 100, (float)r.NextDouble() * 100, (float)r.NextDouble() * 100), (float)r.NextDouble() * 100);
                var p = new Vector3((float)r.NextDouble() * 100, (float)r.NextDouble() * 100, (float)r.NextDouble() * 100);
                var d = Vector3.Distance(s.Center, p);

                if (d > s.Radius)
                    Assert.AreEqual(ContainmentType.Disjoint, s.Contains(p));
                else if (d < s.Radius)
                    Assert.AreEqual(ContainmentType.Contains, s.Contains(p));
                else
                    Assert.AreEqual(ContainmentType.Intersects, s.Contains(p));
            }
        }

        [TestMethod]
        public void CreateMerged_Disjoint()
        {
            var a = new BoundingSphere(Vector3.Zero, 5);
            var b = new BoundingSphere(new Vector3(20), 5);
            var m = BoundingSphere.CreateMerged(a, b);

            Assert.AreEqual(ContainmentType.Contains, m.Contains(a));
            Assert.AreEqual(ContainmentType.Contains, m.Contains(b));
        }

        [TestMethod]
        public void CreateMerged_Intersect()
        {
            var a = new BoundingSphere(Vector3.Zero, 5);
            var b = new BoundingSphere(new Vector3(6, 0, 0), 5);
            var m = BoundingSphere.CreateMerged(a, b);

            Assert.AreEqual(ContainmentType.Contains, m.Contains(a));
            Assert.AreEqual(ContainmentType.Contains, m.Contains(b));
        }

        [TestMethod]
        public void CreateMerged_Contains()
        {
            var a = new BoundingSphere(Vector3.Zero, 5);
            var b = new BoundingSphere(new Vector3(1, 0, 0), 2);
            var m = BoundingSphere.CreateMerged(a, b);

            Assert.AreEqual(a, m);
        }

        [TestMethod]
        public void CreateMerged_Contains2()
        {
            var a = new BoundingSphere(new Vector3(1, 0, 0), 2);
            var b = new BoundingSphere(Vector3.Zero, 5);
            var m = BoundingSphere.CreateMerged(a, b);

            Assert.AreEqual(ContainmentType.Contains, b.Contains(a));
            Assert.AreEqual(ContainmentType.Intersects, a.Contains(b));
            Assert.AreEqual(b, m);
        }

        [TestMethod]
        public void CreateMerged_Fuzz()
        {
            var r = new Random(2436544);

            for (var i = 0; i < 1024; i++)
            {
                var a = new BoundingSphere(new Vector3((float)r.NextDouble() * 100, (float)r.NextDouble() * 100, (float)r.NextDouble() * 100), (float)r.NextDouble() * 100);
                var b = new BoundingSphere(new Vector3((float)r.NextDouble() * 100, (float)r.NextDouble() * 100, (float)r.NextDouble() * 100), (float)r.NextDouble() * 100);
                var m = BoundingSphere.CreateMerged(a, b);

                if (a.Contains(b) == ContainmentType.Contains)
                    Assert.AreEqual(a, m, $"a={a} b={b}");
                else if (b.Contains(a) == ContainmentType.Contains)
                    Assert.AreEqual(b, m);
                else
                    Assert.IsTrue(m.Contains(a) != ContainmentType.Disjoint && m.Contains(b) != ContainmentType.Disjoint);
            }
        }
    }
}
