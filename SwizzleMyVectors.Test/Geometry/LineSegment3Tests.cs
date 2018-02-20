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
    public class LineSegment3Tests
    {
        [TestMethod]
        public void Equals_TrueForEqual()
        {
            var a = new LineSegment3(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = a;

            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(a.Equals((object)b));
            Assert.IsTrue(a == b);
        }

        [TestMethod]
        public void Equals_FalseForNotEqual()
        {
            var a = new LineSegment3(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new LineSegment3(new Vector3(6, 5, 4), new Vector3(3, 2, 1));

            Assert.IsFalse(a.Equals(b));
            Assert.IsFalse(a.Equals((object)b));
            Assert.IsFalse(a == b);
        }

        [TestMethod]
        public void NotEquals_FalseForEqual()
        {
            var a = new LineSegment3(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = a;

            Assert.IsFalse(a != b);
        }

        [TestMethod]
        public void NotEquals_TrueForNotEqual()
        {
            var a = new LineSegment3(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new LineSegment3(new Vector3(6, 5, 4), new Vector3(3, 2, 1));

            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void GetHashCode_SameForEqual()
        {
            var a = new LineSegment3(new Vector3(1, 2, 3), new Vector3(4, 5, 6));
            var b = new LineSegment3(new Vector3(1, 2, 3), new Vector3(4, 5, 6));

            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}
