using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors.Geometry;

namespace SwizzleMyVectors.Test.Geometry.BoundingBoxTests
{
    [TestClass]
    public class EqualityTests
    {
        [TestMethod]
        public void Equals_TrueWithEqual()
        {
            var a = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));

            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void Equals_FalseWithNotEqual()
        {
            var a = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new BoundingBox(new Vector3(2, 2, 3), new Vector3(4, 5, 6));

            Assert.AreNotEqual(a, b);
        }

        [TestMethod]
        public void OpEquals_TrueWithEqual()
        {
            var a = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));

            Assert.IsTrue(a == b);
        }

        [TestMethod]
        public void OpEquals_FalseWithNotEqual()
        {
            var a = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new BoundingBox(new Vector3(2, 2, 3), new Vector3(4, 5, 6));

            Assert.IsFalse(a == b);
        }

        [TestMethod]
        public void OpNotEquals_FalseWithEqual()
        {
            var a = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));

            Assert.IsFalse(a != b);
        }

        [TestMethod]
        public void OpNotEquals_TrueWithNotEqual()
        {
            var a = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new BoundingBox(new Vector3(2, 2, 3), new Vector3(4, 5, 6));

            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void GetHashCode_EqualForSame()
        {
            var a = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new BoundingBox(new Vector3(1, 2, 3), new Vector3(4, 5, 6));

            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}
