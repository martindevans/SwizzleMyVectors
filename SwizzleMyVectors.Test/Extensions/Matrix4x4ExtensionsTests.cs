using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SwizzleMyVectors.Test.Extensions
{
    // ReSharper disable once InconsistentNaming
    [TestClass]
    public class Matrix4x4ExtensionsTests
    {
        [TestMethod]
        public void IsNaN_TrueWithNaN()
        {
            Assert.IsTrue(new Matrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, float.NaN).IsNaN());
            Assert.IsTrue(new Matrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, float.NaN, 0).IsNaN());
            Assert.IsTrue(new Matrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, float.NaN, 0, 0).IsNaN());
            Assert.IsTrue(new Matrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, float.NaN, 0, 0, 0).IsNaN());
            Assert.IsTrue(new Matrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, float.NaN, 0, 0, 0, 0).IsNaN());
            Assert.IsTrue(new Matrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, float.NaN, 0, 0, 0, 0, 0).IsNaN());
            Assert.IsTrue(new Matrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, float.NaN, 0, 0, 0, 0, 0, 0).IsNaN());
            Assert.IsTrue(new Matrix4x4(0, 0, 0, 0, 0, 0, 0, 0, float.NaN, 0, 0, 0, 0, 0, 0, 0).IsNaN());
            Assert.IsTrue(new Matrix4x4(0, 0, 0, 0, 0, 0, 0, float.NaN, 0, 0, 0, 0, 0, 0, 0, 0).IsNaN());
            Assert.IsTrue(new Matrix4x4(0, 0, 0, 0, 0, 0, float.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0).IsNaN());
            Assert.IsTrue(new Matrix4x4(0, 0, 0, 0, 0, float.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0).IsNaN());
            Assert.IsTrue(new Matrix4x4(0, 0, 0, 0, float.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0).IsNaN());
            Assert.IsTrue(new Matrix4x4(0, 0, 0, float.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0).IsNaN());
            Assert.IsTrue(new Matrix4x4(0, 0, float.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0).IsNaN());
            Assert.IsTrue(new Matrix4x4(0, float.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0).IsNaN());
            Assert.IsTrue(new Matrix4x4(float.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0).IsNaN());
        }

        [TestMethod]
        public void IsNaN_FalseWithNoNaN()
        {
            var r = new Random(129308);

            for (var i = 0; i < 1024; i++)
            {
                var m = new Matrix4x4(
                    (float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(),
                    (float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(),
                    (float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(),
                    (float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()
                );

                Assert.IsFalse(m.IsNaN());
            }
        }

        [TestMethod]
        public void ManhattanLength_IsSumOfElements()
        {
            Assert.AreEqual(10, new Vector2(8, 2).ManhattanLength());
            Assert.AreEqual(10, new Vector2(8, -2).ManhattanLength());
            Assert.AreEqual(10, new Vector2(-8, 2).ManhattanLength());
            Assert.AreEqual(10, new Vector2(-8, -2).ManhattanLength());
        }

        [TestMethod]
        public void LargestElement_IsLargestElement()
        {
            Assert.AreEqual(8, new Vector2(8, 2).LargestElement());
            Assert.AreEqual(8, new Vector2(8, -2).LargestElement());
            Assert.AreEqual(2, new Vector2(-8, 2).LargestElement());
            Assert.AreEqual(-2, new Vector2(-8, -2).LargestElement());
        }

        [TestMethod]
        public void Swizzle2()
        {
            Assert.AreEqual(new Vector2(1, 1), new Vector2(1, 2).XX());
            Assert.AreEqual(new Vector2(1, 2), new Vector2(1, 2).XY());
            Assert.AreEqual(new Vector2(2, 1), new Vector2(1, 2).YX());
            Assert.AreEqual(new Vector2(2, 2), new Vector2(1, 2).YY());
        }

        [TestMethod]
        public void Swizzle3()
        {
            Assert.AreEqual(new Vector3(3, 1, 2), new Vector2(1, 2)._XY(3));
            Assert.AreEqual(new Vector3(1, 3, 2), new Vector2(1, 2).X_Y(3));
            Assert.AreEqual(new Vector3(1, 2, 3), new Vector2(1, 2).XY_(3));

            Assert.AreEqual(new Vector3(3, 1, 1), new Vector2(1, 2)._XX(3));
            Assert.AreEqual(new Vector3(1, 3, 1), new Vector2(1, 2).X_X(3));
            Assert.AreEqual(new Vector3(1, 1, 3), new Vector2(1, 2).XX_(3));

            Assert.AreEqual(new Vector3(3, 2, 2), new Vector2(1, 2)._YY(3));
            Assert.AreEqual(new Vector3(2, 3, 2), new Vector2(1, 2).Y_Y(3));
            Assert.AreEqual(new Vector3(2, 2, 3), new Vector2(1, 2).YY_(3));

            Assert.AreEqual(new Vector3(1, 1, 1), new Vector2(1, 2).XXX());
            Assert.AreEqual(new Vector3(1, 1, 2), new Vector2(1, 2).XXY());
            Assert.AreEqual(new Vector3(1, 2, 1), new Vector2(1, 2).XYX());
            Assert.AreEqual(new Vector3(1, 2, 2), new Vector2(1, 2).XYY());
            Assert.AreEqual(new Vector3(2, 1, 1), new Vector2(1, 2).YXX());
            Assert.AreEqual(new Vector3(2, 1, 2), new Vector2(1, 2).YXY());
            Assert.AreEqual(new Vector3(2, 2, 1), new Vector2(1, 2).YYX());
            Assert.AreEqual(new Vector3(2, 2, 2), new Vector2(1, 2).YYY());
        }
    }
}
