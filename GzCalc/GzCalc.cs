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

        static void Conversion(ref double[,] matrix, ref double[] extension) {
            var diver = new double[matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                diver[i] = matrix[i, i];
            }

            for (int i = 0; i < matrix.GetLength(0); i++) {
                for (int j = 0; j < matrix.GetLength(1); j++) {
                    matrix[i, j] /= diver[i];
                }
                extension[i] /= diver[i];
            }
        }

        public static double[] GzMethodCalc(double[,] matrix, double[] extension) {
            if (matrix.GetLength(1) != extension.Length) {
                throw new WrongExtendedMatrixException();
            }
            int n = extension.Length;
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
                        s = s + Math.Abs(matrix[j, k]);
                    }
                    if (s >= Math.Abs(matrix[j, j]))
                    {
                        Conversion(ref matrix, ref extension);
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
                        var += (matrix[i, j] * calc2[j]);
                    //skipping i==j
                    for (int j = i + 1; j < n; j++)
                        var += (matrix[i, j] * calc2[j]);

                    calc2[i] = (extension[i] - var) / matrix[i, i];
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