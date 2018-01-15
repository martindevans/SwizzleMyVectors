using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SwizzleMyVectors.Test.Extensions
{
    [TestClass]
    public class QuaternionExtensionsTests
    {
        [TestMethod]
        public void IsNaN_TrueForNaN()
        {
            Assert.IsTrue(new Quaternion(0, 1, 2, float.NaN).IsNaN());
            Assert.IsTrue(new Quaternion(0, 1, float.NaN, 3).IsNaN());
            Assert.IsTrue(new Quaternion(0, float.NaN, 2, 3).IsNaN());
            Assert.IsTrue(new Quaternion(float.NaN, 1, 2, 3).IsNaN());
        }

        [TestMethod]
        public void IsNaN_FalseForNotNaN()
        {
            Assert.IsFalse(new Quaternion(0, 1, 2, 3).IsNaN());
        }
    }
}
