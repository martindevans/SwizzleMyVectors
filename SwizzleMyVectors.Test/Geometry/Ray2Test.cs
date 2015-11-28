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
    }
}
