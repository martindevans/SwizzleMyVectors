using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SwizzleMyVectors.Test.Extensions
{
    [TestClass]
    public class PlaneExtensionsTests
    {
        [TestMethod]
        public void AssertThat_SignedDistanceIsCorrect_WithPointAbovePlane()
        {
            var p = new Plane(new Vector3(0, 1, 0), 10);
            var d = p.DistanceTo(new Vector3(0, 0, 0));

            Assert.AreEqual(10, d);
        }

        [TestMethod]
        public void AssertThat_SignedDistanceIsCorrect_WithPointBelowPlane()
        {
            var p = new Plane(new Vector3(0, 1, 0), 10);
            var d = p.DistanceTo(new Vector3(0, -20, 0));

            Assert.AreEqual(-10, d);
        }

        [TestMethod]
        public void AssertThat_SignedDistanceIsCorrect_WithPointBelowPlane_WithNegativeD()
        {
            var p = new Plane(new Vector3(0, 1, 0), -10);
            var d = p.DistanceTo(new Vector3(0, -20, 0));

            Assert.AreEqual(-30, d);
        }

        [TestMethod]
        public void AssertThat_SignedDistanceIsCorrect_WithPointAbovePlane_WithNegativeD()
        {
            var p = new Plane(new Vector3(0, 1, 0), -10);
            var d = p.DistanceTo(new Vector3(0, 20, 0));

            Assert.AreEqual(10, d);
        }

        [TestMethod]
        public void AssertThat_SideIsCorrect_WithPointAbovePlane()
        {
            var p = new Plane(new Vector3(0, 1, 0), 10);

            var s = p.Side(new Vector3(0, 0, 0));

            Assert.IsTrue(s > 0);
        }

        [TestMethod]
        public void AssertThat_SideIsCorrect_WithPointBelowPlane()
        {
            var p = new Plane(new Vector3(0, 1, 0), 10);

            var s = p.Side(new Vector3(0, -20, 0));

            Assert.IsTrue(s < 0);
        }

        [TestMethod]
        public void AssertThat_SideIsCorrect_WithPointAbovePlane_WithNegativeD()
        {
            var p = new Plane(new Vector3(0, 1, 0), -10);

            var s = p.Side(new Vector3(0, 20, 0));

            Assert.IsTrue(s > 0);
        }

        [TestMethod]
        public void AssertThat_SideIsCorrect_WithPointBelowPlane_WithNegativeD()
        {
            var p = new Plane(new Vector3(0, 1, 0), -10);

            var s = p.Side(new Vector3(0, -20, 0));

            Assert.IsTrue(s < 0);
        }

        [TestMethod]
        public void IsNaN_TrueWithNaNElement()
        {
            Assert.IsTrue(new Plane(new Vector4(float.NaN, 0, 1, 2)).IsNaN());
            Assert.IsTrue(new Plane(new Vector4(0, float.NaN, 1, 2)).IsNaN());
            Assert.IsTrue(new Plane(new Vector4(0, 1, float.NaN, 2)).IsNaN());
            Assert.IsTrue(new Plane(new Vector4(0, 1, 2, float.NaN)).IsNaN());
        }

        [TestMethod]
        public void IsNaN_FalseWithoutNaN()
        {
            var r = new Random(234987);

            for (var i = 0; i < 1024; i++)
            {
                Assert.IsFalse(new Plane(new Vector4(
                    (float)((r.NextDouble() - 0.5) * int.MaxValue),
                    (float)((r.NextDouble() - 0.5) * int.MaxValue),
                    (float)((r.NextDouble() - 0.5) * int.MaxValue),
                    (float)((r.NextDouble() - 0.5) * int.MaxValue)
                )).IsNaN());
            }
        }
    }
}
