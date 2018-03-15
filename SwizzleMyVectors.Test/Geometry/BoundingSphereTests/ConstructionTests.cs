using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors.Geometry;

namespace SwizzleMyVectors.Test.Geometry.BoundingSphereTests
{
    [TestClass]
    public class ConstructionTests
    {
        [TestMethod]
        public void CreateFromBoundingBox_ContainsBox()
        {
            var box = new BoundingBox(new Vector3(1, 7, 3), new Vector3(9, 0, 2));
            var sphere = BoundingSphere.CreateFromBoundingBox(box);

            Assert.AreEqual(ContainmentType.Contains, sphere.Contains(box));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateFromPoints_ThrowsWithNull()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            BoundingSphere.CreateFromPoints(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateFromPoints_ThrowsWithEmpty()
        {
            BoundingSphere.CreateFromPoints(new Vector3[0]);
        }

        [TestMethod]
        public void CreateFromPoints_ContainsPoints()
        {
            var r = new Random(12897);
            var p = new List<Vector3>();
            for (var i = 0; i < 500; i++)
                p.Add(new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()) * (float)r.NextDouble() * 100);

            var s = BoundingSphere.CreateFromPoints(p);

            foreach (var point in p)
                Assert.IsTrue(s.Contains(point));
        }
    }
}
