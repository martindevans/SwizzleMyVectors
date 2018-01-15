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
    }
}
