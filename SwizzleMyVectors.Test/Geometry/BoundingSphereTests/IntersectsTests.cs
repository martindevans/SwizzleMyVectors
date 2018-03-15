using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors.Geometry;

namespace SwizzleMyVectors.Test.Geometry.BoundingSphereTests
{
    [TestClass]
    public class IntersectsTests
    {
        #region Sphere/Box
        [TestMethod]
        public void IntersectsBox_Intersecting()
        {
            var sphere = new BoundingSphere(new Vector3(1, 2, 3), 10);
            var box = new BoundingBox(new Vector3(1, 2, 3), new Vector3(10, 20, 30));

            Assert.IsTrue(sphere.Intersects(box));
        }

        [TestMethod]
        public void IntersectsBox_Disjoint()
        {
            var sphere = new BoundingSphere(new Vector3(1, 2, 3), 10);
            var box = new BoundingBox(new Vector3(10, 20, 30), new Vector3(40, 50, 60));

            Assert.IsFalse(sphere.Intersects(box));
        }

        [TestMethod]
        public void IntersectsBox_Containing()
        {
            var sphere = new BoundingSphere(new Vector3(1, 2, 3), 10);
            var box = new BoundingBox(new Vector3(0, 0, 0), new Vector3(2, 2, 2));

            Assert.IsTrue(sphere.Intersects(box));
        }

        [TestMethod]
        public void IntersectsBox_Contained()
        {
            var sphere = new BoundingSphere(new Vector3(30, 40, 50), 10);
            var box = new BoundingBox(new Vector3(0, 0, 0), new Vector3(100, 100, 100));

            Assert.IsTrue(sphere.Intersects(box));
        }
        #endregion

        #region Sphere/Sphere
        [TestMethod]
        public void IntersectsSphere_Intersecting()
        {
            var a = new BoundingSphere(new Vector3(1, 2, 3), 10);
            var b = new BoundingSphere(new Vector3(5, 6, 7), 4);

            Assert.IsTrue(a.Intersects(b));
        }

        [TestMethod]
        public void IntersectsSphere_Disjoint()
        {
            var a = new BoundingSphere(new Vector3(1, 2, 3), 10);
            var b = new BoundingSphere(new Vector3(50, 60, 70), 4);

            Assert.IsFalse(a.Intersects(b));
        }

        [TestMethod]
        public void IntersectsSphere_Containing()
        {
            var a = new BoundingSphere(new Vector3(1, 2, 3), 10);
            var b = new BoundingSphere(new Vector3(3, 4, 5), 1);

            Assert.IsTrue(a.Intersects(b));
        }

        [TestMethod]
        public void IntersectsSphere_Contained()
        {
            var a = new BoundingSphere(new Vector3(1, 2, 3), 10);
            var b = new BoundingSphere(new Vector3(5, 6, 7), 40);

            Assert.IsTrue(a.Intersects(b));
        }
        #endregion

        #region Sphere/Frustum
        #endregion

        #region Sphere/Plane
        #endregion

        #region Sphere/Ray
        #endregion
    }
}
