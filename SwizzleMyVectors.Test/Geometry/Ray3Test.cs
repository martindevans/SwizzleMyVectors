using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors.Geometry;

namespace SwizzleMyVectors.Test.Geometry
{
    [TestClass]
    public class Ray3Test
    {
        [TestMethod]
        public void AssertThat_RayIntersectsPlane_ReturnsCorrectValue()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), 0);
            Ray3 r = new Ray3(new Vector3(0, -10, 0), new Vector3(0, 1, 0));

            var i = r.Intersects(p);

            Assert.AreEqual(10, i);
        }

        [TestMethod]
        public void AssertThat_RayIntersectsPlane_ReturnsCorrectValue_WithPositivePlaneDistance()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), 5);
            Ray3 r = new Ray3(new Vector3(0, -10, 0), new Vector3(0, 1, 0));

            var i = r.Intersects(p);

            Assert.AreEqual(15, i);
        }

        [TestMethod]
        public void AssertThat_RayIntersectsPlane_ReturnsCorrectValue_WithNegativePlaneDistance()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), -5);
            Ray3 r = new Ray3(new Vector3(0, -10, 0), new Vector3(0, 1, 0));

            var i = r.Intersects(p);

            Assert.AreEqual(5, i);
        }
    }
}
