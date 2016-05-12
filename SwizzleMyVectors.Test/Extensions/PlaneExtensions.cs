using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SwizzleMyVectors.Test.Extensions
{
    [TestClass]
    public class PlaneExtensions
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
    }
}
