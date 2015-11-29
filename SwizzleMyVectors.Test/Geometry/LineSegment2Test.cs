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
    }
}
