using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors.Geometry;

namespace SwizzleMyVectors.Test.Geometry.BoundingSphereClass
{
    [TestClass]
    public class ContainsTests
    {
        #region Sphere/Box
        [TestMethod]
        public void ContainsBoundingBox_Contains()
        {
            var s = new BoundingSphere(new Vector3(10, 20, 30), 17);
            var b = new BoundingBox(new Vector3(11, 21, 31), new Vector3(19, 23, 44));

            Assert.AreEqual(ContainmentType.Contains, s.Contains(b));
        }

        [TestMethod]
        public void ContainsBoundingBox_Intersects()
        {
            var s = new BoundingSphere(new Vector3(10, 20, 30), 17);
            var b = new BoundingBox(new Vector3(11, 21, 31), new Vector3(19, 23, 54));

            Assert.AreEqual(ContainmentType.Intersects, s.Contains(b));
        }

        [TestMethod]
        public void ContainsBoundingBox_Disjoint()
        {
            var s = new BoundingSphere(new Vector3(10, 20, 30), 17);
            var b = new BoundingBox(new Vector3(55, 21, 31), new Vector3(65, 23, 54));

            Assert.AreEqual(ContainmentType.Disjoint, s.Contains(b));
        }
        #endregion

        #region Sphere/Sphere
        [TestMethod]
        public void ContainsSphere_Contains()
        {
            var a = new BoundingSphere(Vector3.Zero, 10);
            var b = new BoundingSphere(Vector3.One, 2);

            Assert.AreEqual(ContainmentType.Contains, a.Contains(b));
        }

        [TestMethod]
        public void ContainsSphere_Contains_SmallSphere()
        {
            var a = new BoundingSphere(Vector3.Zero, 1);
            var b = new BoundingSphere(new Vector3(0.5f), 0.1f);

            Assert.AreEqual(ContainmentType.Contains, a.Contains(b));
        }

        [TestMethod]
        public void ContainsSphere_Intersects()
        {
            var a = new BoundingSphere(Vector3.Zero, 10);
            var b = new BoundingSphere(new Vector3(10), 8);

            Assert.AreEqual(ContainmentType.Intersects, a.Contains(b));
        }

        [TestMethod]
        public void ContainsSphere_Disjoint()
        {
            var a = new BoundingSphere(Vector3.Zero, 10);
            var b = new BoundingSphere(new Vector3(15), 8);

            Assert.AreEqual(ContainmentType.Disjoint, a.Contains(b));
        }
        #endregion

        #region Sphere/Point
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
                    Assert.IsFalse(s.Contains(p));
                else
                    Assert.IsTrue(s.Contains(p));
            }
        }
        #endregion
    }
}
