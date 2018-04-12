using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrimitiveSvgBuilder;

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

        [TestMethod]
        public void Area_IsPositive_WithClockwiseWinding()
        {
            var shape = new Vector2[] {
                new Vector2(0, 0),
                new Vector2(0, 10),
                new Vector2(10, 10),
                new Vector2(10, 0)
            };

            Assert.AreEqual(100, shape.Area());
        }

        [TestMethod]
        public void Area_IsNegative_WithAntiClockwiseWinding()
        {
            var shape = new Vector2[] {
                new Vector2(0, 0),
                new Vector2(10, 0),
                new Vector2(10, 10),
                new Vector2(0, 10)
            };

            Assert.AreEqual(-100, shape.Area());
        }

        [TestMethod]
        public void Area_IsZero_WithSelfIntersection()
        {
            //Figure of 8 shape, one half clockwise the other half anticlockwise. Adding up to nothing
            var shape = new Vector2[] {
                new Vector2(0, 0),
                new Vector2(0, 10),
                new Vector2(20, 10),
                new Vector2(20, 20),
                new Vector2(10, 20),
                new Vector2(10, 0),
            };

            var svgBuilder = new SvgBuilder(10);
            svgBuilder.Outline(shape);
            Console.WriteLine(svgBuilder);

            Assert.AreEqual(0, shape.Area());
        }

        [TestMethod]
        public void AreaEnumerable_IsPositive_WithClockwiseWinding()
        {
            var shape = new Vector2[] {
                new Vector2(0, 0),
                new Vector2(0, 10),
                new Vector2(10, 10),
                new Vector2(10, 0)
            };

            Assert.AreEqual(100, ((IEnumerable<Vector2>)shape).Area());
        }

        [TestMethod]
        public void AreaEnumerable_IsNegative_WithAntiClockwiseWinding()
        {
            var shape = new Vector2[] {
                new Vector2(0, 0),
                new Vector2(10, 0),
                new Vector2(10, 10),
                new Vector2(0, 10)
            };

            Assert.AreEqual(-100, ((IEnumerable<Vector2>)shape).Area());
        }

        [TestMethod]
        public void AreaEnumerable_IsZero_WithSelfIntersection()
        {
            //Figure of 8 shape, one half clockwise the other half anticlockwise. Adding up to nothing
            var shape = new Vector2[] {
                new Vector2(0, 0),
                new Vector2(0, 10),
                new Vector2(20, 10),
                new Vector2(20, 20),
                new Vector2(10, 20),
                new Vector2(10, 0),
            };

            var svgBuilder = new SvgBuilder(10);
            svgBuilder.Outline(shape);
            Console.WriteLine(svgBuilder);

            Assert.AreEqual(0, ((IEnumerable<Vector2>)shape).Area());
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
        public void IsConvex_True_WithClockwiseWinding()
        {
            var shape = new Vector2[] {
                new Vector2(0, 0),
                new Vector2(0, 10),
                new Vector2(10, 10),
                new Vector2(10, 0)
            };

            Assert.IsTrue(shape.IsConvex());
        }

        [TestMethod]
        public void IsConvex_True_WithAntiClockwiseWinding()
        {
            var shape = new Vector2[] {
                new Vector2(0, 0),
                new Vector2(10, 0),
                new Vector2(10, 10),
                new Vector2(0, 10)
            };

            Assert.IsTrue(shape.IsConvex());
        }

        [TestMethod]
        public void IsConvex_True_WithStraightSection()
        {
            var shape = new Vector2[] {
                new Vector2(0, 0),
                new Vector2(10, 0),
                new Vector2(10, 2),
                new Vector2(10, 4),
                new Vector2(10, 6),
                new Vector2(10, 8),
                new Vector2(10, 10),
                new Vector2(0, 10)
            };

            Assert.IsTrue(shape.IsConvex());
        }

        [TestMethod]
        public void IsConvex_False_WithSelfIntersecting()
        {
            //Figure of 8 shape, one half clockwise the other half anticlockwise. Adding up to nothing
            var shape = new Vector2[] {
                new Vector2(0, 0),
                new Vector2(0, 10),
                new Vector2(20, 10),
                new Vector2(20, 20),
                new Vector2(10, 20),
                new Vector2(10, 0),
            };

            var svgBuilder = new SvgBuilder(10);
            svgBuilder.Outline(shape);
            Console.WriteLine(svgBuilder);

            Assert.IsFalse(shape.IsConvex());
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

        [TestMethod]
        public void ArrayArea_Convex()
        {
            var rect = new Vector2[] { new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10), new Vector2(10, 0) };
            Assert.AreEqual(100, rect.Area());
        }

        [TestMethod]
        public void ArrayArea_Convex_Anticlockwise()
        {
            var rect = new Vector2[] { new Vector2(10, 0), new Vector2(10, 10), new Vector2(0, 10), new Vector2(0, 0) };
            Assert.AreEqual(-100, rect.Area());
        }

        [TestMethod]
        public void ArrayArea_Concave()
        {
            var rect = new Vector2[] { new Vector2(0, 0), new Vector2(0, 10), new Vector2(5, 10), new Vector2(5, 5), new Vector2(10, 5), new Vector2(10, 0) };
            Assert.AreEqual(75, rect.Area());
        }

        [TestMethod]
        public void ArrayArea_Concave_AntiClockwise()
        {
            var rect = new Vector2[] { new Vector2(10, 0), new Vector2(10, 5), new Vector2(5, 5), new Vector2(5, 10), new Vector2(0, 10), new Vector2(0, 0) };
            Assert.AreEqual(-75, rect.Area());
        }

        [TestMethod]
        public void EnumerableArea_Convex()
        {
            var rect = new Vector2[] { new Vector2(0, 0), new Vector2(0, 10), new Vector2(10, 10), new Vector2(10, 0) };
            Assert.AreEqual(100, rect.AsEnumerable().Area());
        }

        [TestMethod]
        public void EnumerableArea_Convex_Anticlockwise()
        {
            var rect = new Vector2[] { new Vector2(10, 0), new Vector2(10, 10), new Vector2(0, 10), new Vector2(0, 0) };
            Assert.AreEqual(-100, rect.AsEnumerable().Area());
        }

        [TestMethod]
        public void EnumerableArea_Concave()
        {
            var rect = new Vector2[] { new Vector2(0, 0), new Vector2(0, 10), new Vector2(5, 10), new Vector2(5, 5), new Vector2(10, 5), new Vector2(10, 0) };
            Assert.AreEqual(75, rect.AsEnumerable().Area());
        }

        [TestMethod]
        public void EnumerableArea_Concave_AntiClockwise()
        {
            var rect = new Vector2[] { new Vector2(10, 0), new Vector2(10, 5), new Vector2(5, 5), new Vector2(5, 10), new Vector2(0, 10), new Vector2(0, 0) };
            Assert.AreEqual(-75, rect.AsEnumerable().Area());
        }
    }
}
