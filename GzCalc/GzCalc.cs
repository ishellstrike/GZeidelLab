using System;
using System.Linq;

namespace GzDll {
    public class GzCalc
    {
        public static double Eps = 0.01;
        public static int iters;

        public static bool GzCont(double[] xk, double[] xkp)
        {
            return xk.Where((t, i) => Math.Abs(t - xkp[i]) > Eps).Any();
        }

        //((3;4;-4)(4;4;0)(-4;0;5))(3;4;3,1)
        //((1;0;0)(0;1;0)(0;0;1))(1;2;3)
        public static double[] GzMethodCalc(double[,] a, double[] b)
        {
            int n;
            n = 3;
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

        /*
         *  var n = a.GetLength(0);
            var x = new double[n];
            var p = new double[n];

            //סמגלוסעטלמסעü
            //for (int j = 0; j < n; j++) {
            //    var s = 0.0;
            //    for (int k = 0; k < n; k++) {
            //        if (j != k) {
            //            s = s + Math.Abs(a[j, k]);
            //        }
            //        if (s >= Math.Abs(a[j, j])) {
            //            return null;
            //        }
            //    }
            //}


            iters = 0;
            double m;
            do {
                m = 0;
                for (int i = 0; i < n; i++)
                {
                    p[i] = x[i];
                }

                for (int i = 0; i < n; i++) {
                    double var = 0;
                    for (int j = 0; j < i; j++)
                    {
                        var += (a[i, j] * x[j]);
                    }
                    for (int j = i + 1; j < n; j++)
                    {
                        if (i != j) {
                            var += (a[i, j]*p[j]);
                        }
                    }
                    double v = x[i];
                    x[i] = (b[i] - var) / a[i, i];
                    m=Math.Abs(x[i])-Math.Abs(v);
                }
                iters++;
            } while (m < Eps);

            return x;
         */
    }
}