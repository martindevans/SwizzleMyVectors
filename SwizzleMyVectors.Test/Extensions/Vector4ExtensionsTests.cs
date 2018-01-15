using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SwizzleMyVectors.Test.Extensions
{
    [TestClass]
    public class Vector4ExtensionsTests
    {
        [TestMethod]
        public void IsNaN_TrueForNaN()
        {
            Assert.IsTrue(new Vector4(0, 1, 2, float.NaN).IsNaN());
            Assert.IsTrue(new Vector4(0, 1, float.NaN, 3).IsNaN());
            Assert.IsTrue(new Vector4(0, float.NaN, 2, 3).IsNaN());
            Assert.IsTrue(new Vector4(float.NaN, 1, 2, 3).IsNaN());
        }

        [TestMethod]
        public void IsNaN_FalseForNotNaN()
        {
            Assert.IsFalse(new Vector4(0, 1, 2, 3).IsNaN());
        }
    }
}
