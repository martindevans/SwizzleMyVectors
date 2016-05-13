using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors.Geometry;

namespace SwizzleMyVectors.Test.Geometry
{
    [TestClass]
    public class Ray3Test
    {
        [TestMethod]
        public void AssertThat_UpwardRayIntersectsPlane_ReturnsCorrectValue()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), 0);
            Ray3 r = new Ray3(new Vector3(0, -10, 0), new Vector3(0, 1, 0));

            var i = r.Intersects(p);

            Assert.AreEqual(10, i);
        }

        [TestMethod]
        public void AssertThat_UpwardRayIntersectsPlane_ReturnsCorrectValue_WithPositivePlaneDistance()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), 5);
            Ray3 r = new Ray3(new Vector3(0, -10, 0), new Vector3(0, 1, 0));

            var i = r.Intersects(p);

            Assert.AreEqual(5, i);
        }

        [TestMethod]
        public void AssertThat_UpwardRayIntersectsPlane_ReturnsCorrectValue_WithNegativePlaneDistance()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), -5);
            Ray3 r = new Ray3(new Vector3(0, -10, 0), new Vector3(0, 1, 0));

            var i = r.Intersects(p);

            Assert.AreEqual(15, i);
        }

        [TestMethod]
        public void AssertThat_DownwardRayIntersectsPlane_ReturnsCorrectValue()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), 0);
            Ray3 r = new Ray3(new Vector3(0, 10, 0), new Vector3(0, -1, 0));

            var i = r.Intersects(p);

            Assert.AreEqual(10, i);
        }

        [TestMethod]
        public void AssertThat_DownwardRayIntersectsPlane_ReturnsCorrectValue_WithPositivePlaneDistance()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), 5);
            Ray3 r = new Ray3(new Vector3(0, 10, 0), new Vector3(0, -1, 0));

            var i = r.Intersects(p);

            Assert.AreEqual(15, i);
        }

        [TestMethod]
        public void AssertThat_DownwardRayIntersectsPlane_ReturnsCorrectValue_WithNegativePlaneDistance()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), -5);
            Ray3 r = new Ray3(new Vector3(0, 10, 0), new Vector3(0, -1, 0));

            var i = r.Intersects(p);

            Assert.AreEqual(5, i);
        }

        [TestMethod]
        public void AssertThat_ParallelRayDoesNotIntersect_WithPositivePlaneDistance()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), 5);
            Ray3 r = new Ray3(new Vector3(0, 10, 0), new Vector3(1, 0, 0));

            var i = r.Intersects(p);

            Assert.IsNull(i);
        }

        [TestMethod]
        public void AssertThat_ParallelRayDoesNotIntersect_WithNegativePlaneDistance()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), -5);
            Ray3 r = new Ray3(new Vector3(0, 10, 0), new Vector3(1, 0, 0));

            var i = r.Intersects(p);

            Assert.IsNull(i);
        }

        [TestMethod]
        public void AssertThat_RayPointingAwayDoesNotIntersect_WithPositivePlaneDistance()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), 5);
            Ray3 r = new Ray3(new Vector3(0, 10, 0), new Vector3(0, 1, 0));

            var i = r.Intersects(p);

            Assert.IsNull(i);
        }

        [TestMethod]
        public void AssertThat_RayPointingAwayDoesNotIntersect_WithNegativePlaneDistance()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), -5);
            Ray3 r = new Ray3(new Vector3(0, -10, 0), new Vector3(0, -1, 0));

            var i = r.Intersects(p);

            Assert.IsNull(i);
        }

        [TestMethod]
        public void AssertThat_RayStartingOnPlane_Intersects_WithPositivePlaneDistance()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), 5);
            Ray3 r = new Ray3(new Vector3(0, -5, 0), new Vector3(0, -1, 0));

            var i = r.Intersects(p);

            Assert.AreEqual(0, i);
        }

        [TestMethod]
        public void AssertThat_RayStartingOnPlane_Intersects_WithNegativePlaneDistance()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), -5);
            Ray3 r = new Ray3(new Vector3(0, 5, 0), new Vector3(0, -1, 0));

            var i = r.Intersects(p);

            Assert.AreEqual(0, i);
        }

        [TestMethod]
        public void AssertThat_RayAlmostStartingOnPlane_Intersects_WithPositivePlaneDistance()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), 5);
            Ray3 r = new Ray3(new Vector3(0, -4.999999f, 0), new Vector3(0, 1, 0));

            var i = r.Intersects(p);

            Assert.AreEqual(0, i);
        }

        [TestMethod]
        public void AssertThat_RayAlmostStartingOnPlane_Intersects_WithNegativePlaneDistance()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), -5);
            Ray3 r = new Ray3(new Vector3(0, 4.999999f, 0), new Vector3(0, -1, 0));

            var i = r.Intersects(p);

            Assert.AreEqual(0, i);
        }

        [TestMethod]
        public void AssertThat_RayClosestPoint_IsOnLine()
        {
            var ray = new Ray3(new Vector3(0, 1, 0), new Vector3(0, 10, 0));
            var point = new Vector3(5, 5, 5);
            
            var closest = ray.ClosestPoint(point);
            Assert.AreEqual(new Vector3(0, 5, 0), closest);
        }

        [TestMethod]
        public void AssertThat_RayClosestPoint_IsOnLine_BeforeStart()
        {
            var ray = new Ray3(new Vector3(0, 1, 0), new Vector3(0, 10, 0));
            var point = new Vector3(5, 0, 5);

            var closest = ray.ClosestPoint(point);
            Assert.AreEqual(new Vector3(0, 0, 0), closest);
        }

        [TestMethod]
        public void AssertThat_RayClosestPoint_IsOnLine_AfterOneLength()
        {
            var ray = new Ray3(new Vector3(0, 1, 0), new Vector3(0, 10, 0));
            var point = new Vector3(5, 150, 5);

            var closest = ray.ClosestPoint(point);
            Assert.AreEqual(new Vector3(0, 150, 0), closest);
        }
    }
}
