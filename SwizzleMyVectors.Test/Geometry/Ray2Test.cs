using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors.Geometry;

namespace SwizzleMyVectors.Test.Geometry
{
    [TestClass]
    public class Ray2Test
    {
        [TestMethod]
        public void AssertThat_DistanceToPoint_IsSignedDistance_Positive()
        {
            Ray2 r = new Ray2(new Vector2(1, 1), new Vector2(10, 0));

            Assert.AreEqual(9, r.DistanceToPoint(new Vector2(0, 10)));
        }

        [TestMethod]
        public void AssertThat_DistanceToPoint_IsSignedDistance_Negative()
        {
            var r = new Ray2(new Vector2(1, 1), new Vector2(20, 0));

            Assert.AreEqual(-9, r.DistanceToPoint(new Vector2(0, -8)));
        }

        [TestMethod]
        public void AssertThat_DistanceToPoint_AlwaysEqualsDistanceToClosestPoint()
        {
            Random r = new Random(1);
            for (int i = 0; i < 1000; i++)
            {
                var start = new Vector2((float)r.NextDouble(), (float)r.NextDouble());
                var end = new Vector2((float)r.NextDouble(), (float)r.NextDouble());

                var point = new Vector2((float)r.NextDouble(), (float)r.NextDouble());

                var ry = new Ray2(start, end - start);

                //Check that naieve distance to closest point (literally Distance(CalculateClosestPoint() - Point)
                //is the same as the more refined calculation
                Assert.AreEqual((ry.ClosestPoint(point) - point).Length(), Math.Abs(ry.DistanceToPoint(point)), 0.001f);
            }
        }

        [TestMethod]
        public void AssertThat_RayRayIntersection_FindsCorrectIntersection()
        {
            var i = new Ray2(new Vector2(0, 10), new Vector2(0, 1)).Intersects(new Ray2(new Vector2(20, 5), new Vector2(1, 0)));

            Assert.IsTrue(i.HasValue);
            Assert.AreEqual(-5, i.Value.DistanceAlongA);
            Assert.AreEqual(-20, i.Value.DistanceAlongB);
            Assert.AreEqual(new Vector2(0, 5), i.Value.Position);
        }

        [TestMethod]
        public void AssertThat_RayRayIntersection_FindsParallelLines()
        {
            var i = new Ray2(new Vector2(0, 10), new Vector2(0, 1)).Intersects(new Ray2(new Vector2(20, 5), new Vector2(0, 1)), out var para);

            Assert.AreEqual(Parallelism.Parallel, para);
            Assert.IsFalse(i.HasValue);
        }

        [TestMethod]
        public void Equals_TrueForEqual()
        {
            var a = new Ray2(new Vector2(1, 2), Vector2.UnitY);
            var b = a;

            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(a.Equals((object)b));
            Assert.IsTrue(a == b);
        }

        [TestMethod]
        public void Equals_FalseForNotEqual()
        {
            var a = new Ray2(new Vector2(1, 2), Vector2.UnitY);
            var b = new Ray2(new Vector2(1, 2), Vector2.UnitX);

            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(a.Equals((object)b));
            Assert.IsFalse(a == b);
        }

        [TestMethod]
        public void NotEquals_FalseForEqual()
        {
            var a = new Ray2(new Vector2(1, 2), Vector2.UnitX);
            var b = a;

            Assert.IsFalse(a != b);
        }

        [TestMethod]
        public void NotEquals_TrueForNotEqual()
        {
            var a = new Ray2(new Vector2(1, 2), Vector2.UnitX);
            var b = new Ray2(new Vector2(1, 2), Vector2.UnitY);

            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void GetHashCode_SameForEqual()
        {
            var a = new Ray2(new Vector2(1, 2), Vector2.UnitY);
            var b = new Ray2(new Vector2(1, 2), Vector2.UnitY);

            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}
