using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors.Geometry;

namespace SwizzleMyVectors.Test.Geometry
{
    [TestClass]
    public class LineSegment2Test
    {
        [TestMethod]
        public void AssertThat_LineSegmentRayIntersection_FindsCorrectIntersection()
        {
            var i = new LineSegment2(new Vector2(0, 0), new Vector2(10, 0)).Intersects(new Ray2(new Vector2(5, 5), new Vector2(0, 1)));

            Assert.IsTrue(i.HasValue);
            Assert.AreEqual(new Vector2(5, 0), i.Value.Position);
            Assert.AreEqual(0.5f, i.Value.DistanceAlongA);
            Assert.AreEqual(-5, i.Value.DistanceAlongB);
        }

        [TestMethod]
        public void AssertThat_LineSegment_FindsIntersectionAtZero()
        {
            var a = new LineSegment2(new Vector2(0, 0), new Vector2(0, 10));
            var b = new LineSegment2(new Vector2(-10, -10), new Vector2(20, 20));

            var i = a.Intersects(b);

            Assert.IsTrue(i.HasValue);
            Assert.AreEqual(0, i.Value.DistanceAlongA);
        }

        [TestMethod]
        public void AssertThat_LineSegment_FindsIntersectionAtOne()
        {
            var a = new LineSegment2(new Vector2(10, 0), new Vector2(0, 0));
            var b = new LineSegment2(new Vector2(-10, -10), new Vector2(20, 20));

            var i = a.Intersects(b);

            Assert.IsTrue(i.HasValue);
            Assert.AreEqual(1, i.Value.DistanceAlongA);
        }
    }
}
