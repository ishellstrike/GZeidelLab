using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GZeidelSamsonovLab
{
    public partial class GzMethod : Form {
        public const double Epsilon = 0.1;
        public double[,] Input;
        public double[] InputRight;

        public GzMethod()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            wrongSyntaxLabel.Visible = false;
            textBox1_TextChanged(null, null);
        }

        public bool GzEnding(double[] xk, double[] xkp) {
            return !xk.Where((element, index) => Math.Abs(element - xkp[index]) >= Epsilon).Any();
        }

        public double[] GzMethodCalc(double[,] a, double[] b) {
            var n = a.GetLength(0);
            var x = new double[n];
            var p = new double[n];
            do
            {
                for (int i = 0; i < n; i++) {
                    p[i] = x[i];
                }

                for (int i = 0; i < n; i++) {
                    double var = 0;
                    for (int j = 0; j < i; j++) {
                        var += (a[i, j]*x[j]);
                    }
                    for (int j = i + 1; j < n; j++) {
                        var += (a[i, j]*p[j]);
                    }
                    x[i] = (b[i] - var) / a[i, i];
                }
            }
            while (!GzEnding(x, p));

            return x;
        }

        //syntax ((1,2,3),(1,2,3),(1,2,3))(1,2,3)
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try {
                string extracted = textBox1.Text;
                var lastbr = extracted.LastIndexOf('('); 
                string right = extracted.Substring(lastbr).Trim(' ');
                string left = extracted.Substring(0, lastbr).Trim(' ');

                var l = LeftParser(left);
                var r = RightParser(right);
                wrongSyntaxLabel.Visible = false;

                label2.Text = ExtMatrixString(l, r);

                var ountp = GzMethodCalc(l, r);

                label3.Text = VectorString(ountp);
            }
            catch (Exception ex) {
                wrongSyntaxLabel.Visible = true;
            }
        }

        private static string VectorString(double[] l)
        {
            string s = "";
            for (int i = 0; i < l.GetLength(0); i++)
            {
                    s += l[i] + "; ";
                s += Environment.NewLine;
            }
            return s;
        }

        private static string ExtMatrixString(double[,] l, double[] r) {
            string s = "";
            for (int i = 0; i < l.GetLength(0); i++) {
                for (int j = 0; j < l.GetLength(1); j++) {
                    s += l[i, j] + "; ";
                }
                s += " | " + r[i];
                s += Environment.NewLine;
            }
            return s;
        }

        private double[,] LeftParser(string s) {
            var firstCol = new List<List<double>>();
            bool afterFirst = false;
            var parts = s.Split('(');


            int y = 0;
            foreach (var part in parts.Where(part => !string.IsNullOrEmpty(part))) {
                if (!string.IsNullOrEmpty(part)) {
                    var nums = part.Split(';').Select(t=>t.Trim(' ').Trim(')'));
                    firstCol.Add(new List<double>());
                    foreach (var num in nums.Where(num => !string.IsNullOrEmpty(num))) {
                        firstCol[y].Add(Convert.ToDouble(num));
                    }
                    y++;
                }
            }
            var o = new double[firstCol.Count, firstCol[0].Count];

            for (int i = 0; i < firstCol.Count; i++) {
                for (int j = 0; j < firstCol[0].Count; j++) {
                    o[i, j] = firstCol[i][j];
                }
            }

            return o;
        }

        private double[] RightParser(string s)
        {
            var firstCol = new List<double>();
            var nums = s.Split(';').Select(t => t.Trim(' ').Trim(')').Trim('('));
            foreach (var num in nums.Where(num => !string.IsNullOrEmpty(num)))
            {
                firstCol.Add(double.Parse(num));
            }
            var o = new double[firstCol.Count];

            for (int i = 0; i < firstCol.Count; i++)
            {
                o[i] = firstCol[i];
            }

            return o;
        }
    }
}
