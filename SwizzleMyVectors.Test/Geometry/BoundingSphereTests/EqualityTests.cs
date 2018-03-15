using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors.Geometry;

namespace SwizzleMyVectors.Test.Geometry.BoundingSphereTests
{
    [TestClass]
    public class EqualityTests
    {
        [TestMethod]
        public void Equals_ReturnsTrueForEqual()
        {
            var a = new BoundingSphere(new Vector3(1, 2, 3), 4);
            var b = a;

            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(a.Equals((object)b));
            Assert.IsTrue(a == b);
        }

        [TestMethod]
        public void Equals_ReturnsFalseForNonEqual()
        {
            var a = new BoundingSphere(new Vector3(1, 2, 3), 4);
            var b = new BoundingSphere(new Vector3(5, 6, 7), 8);

            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(a.Equals((object)b));
            Assert.IsFalse(a == b);
        }

        [TestMethod]
        public void NotEquals_ReturnsTrueForNotEqual()
        {
            var a = new BoundingSphere(new Vector3(1, 2, 3), 4);
            var b = new BoundingSphere(new Vector3(5, 6, 7), 8);

            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void NotEquals_ReturnsFalseForEqual()
        {
            var a = new BoundingSphere(new Vector3(1, 2, 3), 4);
            var b = a;

            Assert.IsFalse(a != b);
        }

        [TestMethod]
        public void GetHashCode_IsSameForEqual()
        {
            var a = new BoundingSphere(new Vector3(1, 2, 3), 4);
            var b = new BoundingSphere(new Vector3(1, 2, 3), 4);

            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}
