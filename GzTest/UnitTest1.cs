using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using GzDll;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GzTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void EqualsContTest() {
            Assert.IsFalse(GzCalc.GzCont(new[] {1.0, 2, 3}, new[] {1.0, 2, 3}));
        }

        [TestMethod]
        public void LambdaAllContTest() {
            double d = 1;
            Assert.IsTrue(GzCalc.GzCont(new[] { 1.0, 2, 3 }, new[] { 1+d, 2+d, 3+d }));
        }

        [TestMethod]
        public void LambdaOneContTest()
        {
            double d = 1;
            Assert.IsTrue(GzCalc.GzCont(new[] { 1.0, 2, 3 }, new[] { 1 + d, 2, 3}));
        }

        [TestMethod]
        public void IdentMatrixTest()
        {
            double[,] a = {{1,0,0},{0,1,0},{0,0,1}};
            double[] b = {1,2,3};
            CollectionAssert.AreEqual(GzCalc.GzMethodCalc(a,b),b);
        }

        [TestMethod]
        public void ReverceIdentMatrixTest()
        {
            double[,] a = { { 0, 0, 1 }, { 0, 1, 0 }, { 1, 0, 0 } };
            double[] b = { 1, 2, 3 };
            CollectionAssert.AreEqual(GzCalc.GzMethodCalc(a, b), b);
        }
    }
}
