using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwizzleMyVectors.Geometry;

namespace SwizzleMyVectors.Test.Geometry
{
    [TestClass]
    public class BoundingSphereTest
    {
        [TestMethod]
        public void AssertThat_Inflate_IncreasesRadius()
        {
            BoundingSphere s = new BoundingSphere(Vector3.Zero, 10);

            Assert.AreEqual(15, s.Inflate(10).Radius);
        }

        [TestMethod]
        public void AssertThat_Inflate_ByNegativeValue_DecreasesRadius()
        {
            BoundingSphere s = new BoundingSphere(Vector3.Zero, 10);

            Assert.AreEqual(5, s.Inflate(-10).Radius);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AssertThat_Inflate_ByNegativeValueLargerThanDiameter_Throws()
        {
            BoundingSphere s = new BoundingSphere(Vector3.Zero, 10);

            var result = s.Inflate(-50);
        }
    }
}
