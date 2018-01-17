using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SwizzleMyVectors.Test.Extensions
{
    [TestClass]
    public class Vector2ExtensionsTests
    {
        [TestMethod]
        public void IsNaN_TrueForNaN()
        {
            Assert.IsTrue(new Vector2(0, float.NaN).IsNaN());
            Assert.IsTrue(new Vector2(float.NaN, 1).IsNaN());
        }

        [TestMethod]
        public void IsNaN_FalseForNotNaN()
        {
            Assert.IsFalse(new Vector2(0, 1).IsNaN());
        }

        [TestMethod]
        public void PerpendicularRight_IsRight()
        {
            var v = new Vector2(0.5f, 1);
            var r = v.PerpendicularRight();

            Assert.AreEqual(new Vector2(1, -0.5f), r);
        }

        [TestMethod]
        public void PerpendicularLeft_IsLeft()
        {
            var v = new Vector2(0.5f, 1);
            var l = v.PerpendicularLeft();

            Assert.AreEqual(new Vector2(-1, 0.5f), l);
        }
    }
}
