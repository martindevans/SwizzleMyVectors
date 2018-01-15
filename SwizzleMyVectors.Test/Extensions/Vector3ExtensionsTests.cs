using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SwizzleMyVectors.Test.Extensions
{
    [TestClass]
    public class Vector3ExtensionsTests
    {
        #region IsNaN
        [TestMethod]
        public void IsNaN_TrueForNaN()
        {
            Assert.IsTrue(new Vector3(0, 1, float.NaN).IsNaN());
            Assert.IsTrue(new Vector3(0, float.NaN, 2).IsNaN());
            Assert.IsTrue(new Vector3(float.NaN, 1, 2).IsNaN());
        }

        [TestMethod]
        public void IsNaN_FalseForNotNaN()
        {
            Assert.IsFalse(new Vector3(0, 1, 2).IsNaN());
        }
        #endregion

        #region manhattan length
        [TestMethod]
        public void IsManhattan_ForPositiveElements()
        {
            Assert.AreEqual(6, new Vector3(1, 2, 3).ManhattanLength());
        }

        [TestMethod]
        public void IsManhattan_ForNegativeElements()
        {
            Assert.AreEqual(6, new Vector3(-1, -2, -3).ManhattanLength());
        }
        #endregion

        #region largest element
        [TestMethod]
        public void LargestElement_IsLargestElement()
        {
            Assert.AreEqual(3, new Vector3(1, 2, 3).LargestElement());
            Assert.AreEqual(6, new Vector3(6, 4, 5).LargestElement());
            Assert.AreEqual(9, new Vector3(8, 9, 7).LargestElement());
        }
        #endregion

        #region convert to vector2
        [TestMethod]
        public void XX()
        {
            Assert.AreEqual(new Vector2(3, 3), new Vector3(3, 2, 1).XX());
        }

        [TestMethod]
        public void XY()
        {
            Assert.AreEqual(new Vector2(3, 2), new Vector3(3, 2, 1).XY());
        }

        [TestMethod]
        public void XZ()
        {
            Assert.AreEqual(new Vector2(3, 1), new Vector3(3, 2, 1).XZ());
        }

        [TestMethod]
        public void YX()
        {
            Assert.AreEqual(new Vector2(2, 3), new Vector3(3, 2, 1).YX());
        }

        [TestMethod]
        public void YY()
        {
            Assert.AreEqual(new Vector2(2, 2), new Vector3(3, 2, 1).YY());
        }

        [TestMethod]
        public void YZ()
        {
            Assert.AreEqual(new Vector2(2, 1), new Vector3(3, 2, 1).YZ());
        }

        [TestMethod]
        public void ZX()
        {
            Assert.AreEqual(new Vector2(1, 3), new Vector3(3, 2, 1).ZX());
        }

        [TestMethod]
        public void ZY()
        {
            Assert.AreEqual(new Vector2(1, 2), new Vector3(3, 2, 1).ZY());
        }

        [TestMethod]
        public void ZZ()
        {
            Assert.AreEqual(new Vector2(1, 1), new Vector3(3, 2, 1).ZZ());
        }
        #endregion

        [TestMethod]
        public void Swizzle3()
        {
            Assert.AreEqual(new Vector3(1, 1, 1), new Vector3(1, 2, 3).XXX());
            Assert.AreEqual(new Vector3(1, 1, 2), new Vector3(1, 2, 3).XXY());
            Assert.AreEqual(new Vector3(1, 1, 3), new Vector3(1, 2, 3).XXZ());
            Assert.AreEqual(new Vector3(1, 2, 1), new Vector3(1, 2, 3).XYX());
            Assert.AreEqual(new Vector3(1, 2, 2), new Vector3(1, 2, 3).XYY());
            Assert.AreEqual(new Vector3(1, 2, 3), new Vector3(1, 2, 3).XYZ());
            Assert.AreEqual(new Vector3(1, 3, 1), new Vector3(1, 2, 3).XZX());
            Assert.AreEqual(new Vector3(1, 3, 2), new Vector3(1, 2, 3).XZY());
            Assert.AreEqual(new Vector3(1, 3, 3), new Vector3(1, 2, 3).XZZ());
            Assert.AreEqual(new Vector3(2, 1, 1), new Vector3(1, 2, 3).YXX());
            Assert.AreEqual(new Vector3(2, 1, 2), new Vector3(1, 2, 3).YXY());
            Assert.AreEqual(new Vector3(2, 1, 3), new Vector3(1, 2, 3).YXZ());
            Assert.AreEqual(new Vector3(2, 2, 1), new Vector3(1, 2, 3).YYX());
            Assert.AreEqual(new Vector3(2, 2, 2), new Vector3(1, 2, 3).YYY());
            Assert.AreEqual(new Vector3(2, 2, 3), new Vector3(1, 2, 3).YYZ());
            Assert.AreEqual(new Vector3(2, 3, 1), new Vector3(1, 2, 3).YZX());
            Assert.AreEqual(new Vector3(2, 3, 2), new Vector3(1, 2, 3).YZY());
            Assert.AreEqual(new Vector3(2, 3, 3), new Vector3(1, 2, 3).YZZ());
            Assert.AreEqual(new Vector3(3, 1, 1), new Vector3(1, 2, 3).ZXX());
            Assert.AreEqual(new Vector3(3, 1, 2), new Vector3(1, 2, 3).ZXY());
            Assert.AreEqual(new Vector3(3, 1, 3), new Vector3(1, 2, 3).ZXZ());
            Assert.AreEqual(new Vector3(3, 2, 1), new Vector3(1, 2, 3).ZYX());
            Assert.AreEqual(new Vector3(3, 2, 2), new Vector3(1, 2, 3).ZYY());
            Assert.AreEqual(new Vector3(3, 2, 3), new Vector3(1, 2, 3).ZYZ());
            Assert.AreEqual(new Vector3(3, 3, 1), new Vector3(1, 2, 3).ZZX());
            Assert.AreEqual(new Vector3(3, 3, 2), new Vector3(1, 2, 3).ZZY());
            Assert.AreEqual(new Vector3(3, 3, 3), new Vector3(1, 2, 3).ZZZ());
        }

        [TestMethod]
        public void SubstituteX()
        {
            Assert.AreEqual(new Vector3(4, 1, 1), new Vector3(1, 2, 3)._XX(4));
            Assert.AreEqual(new Vector3(4, 1, 2), new Vector3(1, 2, 3)._XY(4));
            Assert.AreEqual(new Vector3(4, 1, 3), new Vector3(1, 2, 3)._XZ(4));
            Assert.AreEqual(new Vector3(4, 2, 1), new Vector3(1, 2, 3)._YX(4));
            Assert.AreEqual(new Vector3(4, 2, 2), new Vector3(1, 2, 3)._YY(4));
            Assert.AreEqual(new Vector3(4, 2, 3), new Vector3(1, 2, 3)._YZ(4));
            Assert.AreEqual(new Vector3(4, 3, 1), new Vector3(1, 2, 3)._ZX(4));
            Assert.AreEqual(new Vector3(4, 3, 2), new Vector3(1, 2, 3)._ZY(4));
            Assert.AreEqual(new Vector3(4, 3, 3), new Vector3(1, 2, 3)._ZZ(4));
        }

        [TestMethod]
        public void SubstituteY()
        {
            Assert.AreEqual(new Vector3(1, 4, 1), new Vector3(1, 2, 3).X_X(4));
            Assert.AreEqual(new Vector3(1, 4, 2), new Vector3(1, 2, 3).X_Y(4));
            Assert.AreEqual(new Vector3(1, 4, 3), new Vector3(1, 2, 3).X_Z(4));
            Assert.AreEqual(new Vector3(2, 4, 1), new Vector3(1, 2, 3).Y_X(4));
            Assert.AreEqual(new Vector3(2, 4, 2), new Vector3(1, 2, 3).Y_Y(4));
            Assert.AreEqual(new Vector3(2, 4, 3), new Vector3(1, 2, 3).Y_Z(4));
            Assert.AreEqual(new Vector3(3, 4, 1), new Vector3(1, 2, 3).Z_X(4));
            Assert.AreEqual(new Vector3(3, 4, 2), new Vector3(1, 2, 3).Z_Y(4));
            Assert.AreEqual(new Vector3(3, 4, 3), new Vector3(1, 2, 3).Z_Z(4));
        }

        [TestMethod]
        public void SubstituteZ()
        {
            Assert.AreEqual(new Vector3(1, 1, 4), new Vector3(1, 2, 3).XX_(4));
            Assert.AreEqual(new Vector3(1, 2, 4), new Vector3(1, 2, 3).XY_(4));
            Assert.AreEqual(new Vector3(1, 3, 4), new Vector3(1, 2, 3).XZ_(4));
            Assert.AreEqual(new Vector3(2, 1, 4), new Vector3(1, 2, 3).YX_(4));
            Assert.AreEqual(new Vector3(2, 2, 4), new Vector3(1, 2, 3).YY_(4));
            Assert.AreEqual(new Vector3(2, 3, 4), new Vector3(1, 2, 3).YZ_(4));
            Assert.AreEqual(new Vector3(3, 1, 4), new Vector3(1, 2, 3).ZX_(4));
            Assert.AreEqual(new Vector3(3, 2, 4), new Vector3(1, 2, 3).ZY_(4));
            Assert.AreEqual(new Vector3(3, 3, 4), new Vector3(1, 2, 3).ZZ_(4));
        }
    }
}
