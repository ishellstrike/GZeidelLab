using System;
using System.Collections.Generic;
using System.Linq;

namespace GzDll {
    public class GzCalc
    {
        public static double Eps = 0.00001;
        public static int iters;

        public static bool GzCont(double[] xk, double[] xkp)
        {
            //Новое условие, в соответствии с методичкой (from 04.03.2014)
            return xk.Where((t, i) => Math.Abs(t - xkp[i]) / Math.Abs(xkp[i]) > Eps).Any();
        }

        static void ToDiagonalOne(ref double[,] matrix, ref double[] extension)
        {
            var diver = new double[matrix.GetLength(1)];


            for (int i = 0; i < matrix.GetLength(0); i++) {
                double[,] doubles = matrix;
                //найти максимальный элемент i-ой строки
                diver[i] = Enumerable.Range(0,doubles.GetLength(0)).Select(x=>doubles[i,x]).Max();
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] /= diver[i];
                }
                extension[i] /= diver[i];
            }
        }

        //Перестановка строк для соблюдения горизонтального преобладания (from 04.03.2014)
        static void Conversion(ref double[,] matrix, ref double[] extension) {
            ToDiagonalOne(ref matrix, ref extension);
            double[,] temp =new double[matrix.GetLength(0),matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++) {
                for (int j = 0; j < matrix.GetLength(1); j++) {
                    temp[i, j] = matrix[i, j];
                }
            }
            double[] tempe = new double[extension.GetLength(0)];
            for (int i = 0; i < extension.GetLength(0); i++) {
                tempe[i] = extension[i];
            }
            List<int> uses = new List<int>();
            for (int m = 0; m < matrix.GetLength(0); m++) { //по столбцам
                for (int i = 0; i < matrix.GetLength(0); i++) //по строкам
                {
                    if (Math.Abs(matrix[m, i] - 1) < Eps) { //единичный элимент -- максимальный
                        if (!uses.Contains(i)) { //защита от повторного добавления
                            uses.Add(i);
                            for (int j = 0; j < matrix.GetLength(0); j++) {
                                temp[i, j] = matrix[m, j];
                            }
                            tempe[i] = extension[m];
                        }
                        else {
                            throw new InconsistentException();
                        }
                    }
                }
            }
            matrix = temp;
            extension = tempe;
        }

        public static double[] GzMethodCalc(double[,] matrix, double[] extension) {
            if (matrix.GetLength(1) != extension.Length) {
                throw new WrongExtendedMatrixException();
            }
            int n = extension.Length;
            double[] calc1 = new double[n];
            double[] calc2 = new double[n];
            iters = 0;

            //совместимость
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