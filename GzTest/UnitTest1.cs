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
        public void DeltaAllContTest() {
            double d = 1;
            Assert.IsTrue(GzCalc.GzCont(new[] { 1.0, 2, 3 }, new[] { 1+d, 2+d, 3+d }));
        }

        [TestMethod]
        public void DeltaOneContTest()
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
        public void SmallMatrixTest()
        {
            double[,] a = { {1,0},{0,1} };
            double[] b = { 1, 2 };
            CollectionAssert.AreEqual(GzCalc.GzMethodCalc(a, b), b);
        }

        [TestMethod]
        public void BigMatrixTest()
        {
            double[,] a = { { 1, 0, 0, 0, 0, 0 }, { 0, 1, 0, 0, 0, 0 }, { 0, 0, 1, 0, 0, 0 }, { 0, 0, 0, 1, 0, 0 }, { 0, 0, 0, 0, 1, 0 }, { 0, 0, 0, 0, 0, 1 } };
            double[] b = { 1, 2, 3, 4, 5, 6 };
            CollectionAssert.AreEqual(GzCalc.GzMethodCalc(a, b), b);
        }

        [TestMethod]
        public void DifferentExtMatrixTest()
        {
            bool pass = false;
            double[,] a = { { 0, 0, 1 }, { 0, 1, 0 }, { 1, 0, 0 } };
            double[] b = { 1, 2 };
            try
            {
                GzCalc.GzMethodCalc(a, b);
            }
            catch (WrongExtendedMatrixException)
            {
                pass = true;
            }
            Assert.IsTrue(pass);
        }

        [TestMethod]
        public void ComplMatrixTest()
        {
            double[,] a = { { 20.9, 1.2, 2.1, 0.9 }, { 1.2, 21.2, 1.5, 2.5 }, { 2.1, 1.5, 19.8, 1.3 }, {0.9, 2.5, 1.3, 32.1} };
            double[] b = { 21.7, 27.46, 28.76, 49.72 };
            double[] c = {0.8,1,1.2,1.4};//ansver
            var d = GzCalc.GzMethodCalc(a, b);

            for (int i = 0; i < c.Length; i++) {
                Assert.IsTrue(d[i] - c[i] < 0.0001);
            }
        }

        [TestMethod]
        public void RecombineTest()
        {
            double[,] a = {{ 1.2, 21.2, 1.5, 2.5 }, { 20.9, 1.2, 2.1, 0.9 }, { 2.1, 1.5, 19.8, 1.3 }, { 0.9, 2.5, 1.3, 32.1 } };
            double[] b = { 27.46, 21.7, 28.76, 49.72 };
            double[] c = { 0.8, 1, 1.2, 1.4 };//ansver
            var d = GzCalc.GzMethodCalc(a, b);

            for (int i = 0; i < c.Length; i++)
            {
                Assert.IsTrue(d[i] - c[i] < 0.0001);
            }
        }
    }
}
