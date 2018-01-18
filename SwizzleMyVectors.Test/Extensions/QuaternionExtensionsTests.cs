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

        [TestMethod]
        public void NLerp_Interpolates()
        {
            var a = new Quaternion(1, 1, 1, 0);
            var b = new Quaternion(1, 0, 1, 1);
            var c = Quaternion.Normalize(new Quaternion(1, 0.75f, 1, 0.25f));

            Assert.AreEqual(c, a.Nlerp(b, 0.25f));
        }
    }
}
