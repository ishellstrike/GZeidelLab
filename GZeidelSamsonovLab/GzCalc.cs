using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GZeidelSamsonovLab
{
    public partial class GzForm
    {
        public bool GzEnding(double[] xk, double[] xkp)
        {
            return !xk.Where((element, index) => Math.Abs(element - xkp[index]) >= Epsilon).Any();
        }

        public double[] GzMethodCalc(double[,] a, double[] b)
        {
            var n = a.GetLength(0);
            var x = new double[n];
            var p = new double[n];
            do
            {
                for (int i = 0; i < n; i++)
                {
                    p[i] = x[i];
                }

                for (int i = 0; i < n; i++)
                {
                    double var = 0;
                    for (int j = 0; j < i; j++)
                    {
                        var += (a[i, j] * x[j]);
                    }
                    for (int j = i + 1; j < n; j++)
                    {
                        var += (a[i, j] * p[j]);
                    }
                    x[i] = (b[i] - var) / a[i, i];
                }
            } while (!GzEnding(x, p));

            return x;
        }
    }
}
