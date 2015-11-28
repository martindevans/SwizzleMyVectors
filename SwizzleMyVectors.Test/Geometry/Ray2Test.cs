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
            Ray2 r = new Ray2(new Vector2(1, 1), new Vector2(1, 0));

            Assert.AreEqual(9, r.DistanceToPoint(new Vector2(0, 10)));
        }

        [TestMethod]
        public void AssertThat_DistanceToPoint_IsSignedDistance_Negative()
        {
            var r = new Ray2(new Vector2(1, 1), new Vector2(1, 0));

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

                Assert.AreEqual((ry.ClosestPoint(point) - point).Length(), Math.Abs(ry.DistanceToPoint(point)), 0.001f);
            }
        }
    }
}
