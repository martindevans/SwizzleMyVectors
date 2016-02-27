using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors.Geometry;

namespace SwizzleMyVectors.Test.Geometry
{
    [TestClass]
    public class BoundingRectangleTest
    {
        [TestMethod]
        public void AssertThat_GetCorners_GetsCorners()
        {
            var rect = new BoundingRectangle(new Vector2(0, 0), new Vector2(10, 10));

            var corners = rect.GetCorners();

            Assert.AreEqual(4, corners.Length);
            Assert.IsTrue(corners.Contains(new Vector2(0, 0)));
            Assert.IsTrue(corners.Contains(new Vector2(0, 10)));
            Assert.IsTrue(corners.Contains(new Vector2(10, 0)));
            Assert.IsTrue(corners.Contains(new Vector2(10, 10)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AssertThat_GetCorners_Throws_WhenPresentedAnArrayWhichIsTooSmall()
        {
            var rect = new BoundingRectangle(new Vector2(0, 0), new Vector2(10, 10));

            Vector2[] arr = new Vector2[2];
            rect.GetCorners(arr);
        }

        [TestMethod]
        public void AssertThat_Intersection_ReturnsIntersectingArea()
        {
            var a = new BoundingRectangle(new Vector2(0, 0), new Vector2(10, 10));
            var b = new BoundingRectangle(new Vector2(5, 5), new Vector2(15, 15));

            // ReSharper disable once PossibleInvalidOperationException
            var m = a.Intersection(b).Value;

            Assert.AreEqual(new Vector2(5, 5), m.Min);
            Assert.AreEqual(new Vector2(10, 10), m.Max);
        }

        [TestMethod]
        public void AssertThat_Intersection_ReturnsNothing_WhenNotOverlappingWithGapTopRight()
        {
            var a = new BoundingRectangle(new Vector2(0, 0), new Vector2(10, 10));
            var b = new BoundingRectangle(new Vector2(15, 15), new Vector2(25, 25));

            // ReSharper disable once PossibleInvalidOperationException
            Assert.IsFalse(a.Intersection(b).HasValue);
        }

        [TestMethod]
        public void AssertThat_Intersection_ReturnsNothing_WhenNotOverlappingWithGapTopLeft()
        {
            var a = new BoundingRectangle(new Vector2(0, 0), new Vector2(10, 10));
            var b = new BoundingRectangle(new Vector2(-5, 15), new Vector2(-25, 25));

            // ReSharper disable once PossibleInvalidOperationException
            Assert.IsFalse(a.Intersection(b).HasValue);
        }

        [TestMethod]
        public void AssertThat_Intersection_ReturnsNothing_WhenNotOverlappingWithGapBottomLeft()
        {
            var a = new BoundingRectangle(new Vector2(0, 0), new Vector2(10, 10));
            var b = new BoundingRectangle(new Vector2(-5, -5), new Vector2(-15, -15));

            // ReSharper disable once PossibleInvalidOperationException
            Assert.IsFalse(a.Intersection(b).HasValue);
        }

        [TestMethod]
        public void AssertThat_Intersection_ReturnsNothing_WhenNotOverlappingWithGapBottomRight()
        {
            var a = new BoundingRectangle(new Vector2(0, 0), new Vector2(10, 10));
            var b = new BoundingRectangle(new Vector2(15, -5), new Vector2(25, -15));

            // ReSharper disable once PossibleInvalidOperationException
            Assert.IsFalse(a.Intersection(b).HasValue);
        }

        [TestMethod]
        public void AssertThat_Intersection_ReturnsNothing_WhenNotOverlappingWithGapTop()
        {
            var a = new BoundingRectangle(new Vector2(0, 0), new Vector2(10, 10));
            var b = new BoundingRectangle(new Vector2(2, 15), new Vector2(8, 25));

            // ReSharper disable once PossibleInvalidOperationException
            Assert.IsFalse(a.Intersection(b).HasValue);
        }

        [TestMethod]
        public void AssertThat_Inflate_IncreasesSize()
        {
            BoundingRectangle s = new BoundingRectangle(new Vector2(0), new Vector2(10));

            Assert.AreEqual(new Vector2(-5), s.Inflate(10).Min);
            Assert.AreEqual(new Vector2(15), s.Inflate(10).Max);
        }

        [TestMethod]
        public void AssertThat_Inflate_ByNegativeValue_DecreasesSize()
        {
            BoundingRectangle s = new BoundingRectangle(new Vector2(0), new Vector2(10));

            Assert.AreEqual(new Vector2(3), s.Inflate(-6).Min);
            Assert.AreEqual(new Vector2(7), s.Inflate(-6).Max);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AssertThat_InflateSphere_ByNegativeValueLargerThanSize_Throws()
        {
            BoundingRectangle s = new BoundingRectangle(Vector2.Zero, new Vector2(10));

            var result = s.Inflate(-50);
        }
    }
}
