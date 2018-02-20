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

        [TestMethod]
        public void Equals_TrueForEqual()
        {
            var a = new LineSegment2(new Vector2(1, 2), new Vector2(3, 4));
            var b = a;

            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(a.Equals((object)b));
            Assert.IsTrue(a == b);
        }

        [TestMethod]
        public void Equals_FalseForNotEqual()
        {
            var a = new LineSegment2(new Vector2(1, 2), new Vector2(3, 4));
            var b = new LineSegment2(new Vector2(5, 6), new Vector2(7, 8));

            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(a.Equals((object)b));
            Assert.IsFalse(a == b);
        }

        [TestMethod]
        public void NotEquals_FalseForEqual()
        {
            var a = new LineSegment2(new Vector2(1, 2), new Vector2(3, 4));
            var b = a;

            Assert.IsFalse(a != b);
        }

        [TestMethod]
        public void NotEquals_TrueForNotEqual()
        {
            var a = new LineSegment2(new Vector2(1, 2), new Vector2(3, 4));
            var b = new LineSegment2(new Vector2(6, 2), new Vector2(3, 4));

            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void GetHashCode_SameForEqual()
        {
            var a = new LineSegment2(new Vector2(1, 2), new Vector2(3, 4));
            var b = new LineSegment2(new Vector2(1, 2), new Vector2(3, 4));

            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}
