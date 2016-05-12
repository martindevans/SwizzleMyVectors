using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SwizzleMyVectors.Test.Extensions
{
    [TestClass]
    public class PlaneExtensions
    {
        [TestMethod]
        public void AssertThat_SignedDistance_IsNegative_WithPointBelowPlane()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), 10);

            //Point below plane (negative distance)
            var d = p.PerpendicularDistance(new Vector3(0, 0, 0));

            Assert.AreEqual(-10, d);
        }

        [TestMethod]
        public void AssertThat_SignedDistance_IsPositive_WithPointAbovePlane()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), 10);

            //Point above plane (positive distance)
            var d = p.PerpendicularDistance(new Vector3(0, 20, 0));

            Assert.AreEqual(10, d);
        }

        [TestMethod]
        public void AssertThat_SignedDistance_IsNegative_WithPointBelowPlane_WithNegativeD()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), -10);

            //Point below plane (negative distance)
            var d = p.PerpendicularDistance(new Vector3(0, -20, 0));

            Assert.AreEqual(-10, d);
        }

        [TestMethod]
        public void AssertThat_SignedDistance_IsPositive_WithPointAbovePlane_WithNegativeD()
        {
            Plane p = new Plane(new Vector3(0, 1, 0), -10);

            //Point above plane (positive distance)
            var d = p.PerpendicularDistance(new Vector3(0, 0, 0));

            Assert.AreEqual(10, d);
        }
    }
}
