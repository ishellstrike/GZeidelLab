using System;
using System.Linq;

namespace GzDll {
    public class GzCalc
    {
        public static double Eps = 0e-15;
        public static int iters;

        public static bool GzCont(double[] xk, double[] xkp)
        {
            return xk.Where((t, i) => Math.Abs(t - xkp[i]) > Eps).Any();
        }

        //((3;4;-4)(4;4;0)(-4;0;5))(3;4;3,1)
        //((1;0;0)(0;1;0)(0;0;1))(1;2;3)
        public static double[] GzMethodCalc(double[,] a, double[] b) {
            if (a.GetLength(1) != b.Length) {
                throw new WrongExtendedMatrixException();
            }
            int n = b.Length;
            double[] calc1 = new double[n];
            double[] calc2 = new double[n];
            iters = 0;

            //סמגלוסעטלמסעü
            for (int j = 0; j < n; j++)
            {
                var s = 0.0;
                for (int k = 0; k < n; k++)
                {
                    if (j != k)
                    {
                        s = s + Math.Abs(a[j, k]);
                    }
                    if (s >= Math.Abs(a[j, j]))
                    {
                        throw new InconsistentException();
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                calc2[i] = 1;
            }
            do
            {
                for (int i = 0; i < n; i++)
                {
                    double var = 0;
                    calc1[i] = calc2[i];
                    for (int j = 0; j < i; j++)
                        var += (a[i, j] * calc2[j]);
                    //skipping i==j
                    for (int j = i + 1; j < n; j++)
                        var += (a[i, j] * calc2[j]);

                    calc2[i] = (b[i] - var) / a[i, i];
                }
                iters++;
                if (iters > 99999)
                {
                    throw new TooMuchItersException();
                }
            }
            while (GzCont(calc2, calc1));

            return calc2;
        }
    }
}