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
    }
}
