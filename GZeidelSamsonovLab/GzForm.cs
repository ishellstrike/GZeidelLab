using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GzDll;

namespace GZeidelSamsonovLab
{
    public partial class GzForm : Form {
        public double[,] Input;
        public double[] InputRight;

        public GzForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            wrongSyntaxLabel.Visible = false;
            textBox1_TextChanged(null, null);
            ifp = new CultureInfo("en-US");
        }

        private IFormatProvider ifp;
    
        //syntax ((1,2,3),(1,2,3),(1,2,3))(1,2,3)
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try {
                string extracted = textBox1.Text;
                var lastbr = extracted.LastIndexOf('{'); 
                string right = extracted.Substring(lastbr).Trim(' ');
                string left = extracted.Substring(0, lastbr).Trim(' ');

                var l = LeftParser(left);
                var r = RightParser(right);
                wrongSyntaxLabel.Visible = false;

                label2.Text = ExtMatrixString(l, r);

                var ountp = GzCalc.GzMethodCalc(l, r);

                label3.Text = VectorString(ountp);
                label4.Text = GzCalc.iters.ToString();
            }
            catch (Exception ex) {
                wrongSyntaxLabel.Visible = true;
                label4.Text = ex.GetType().Name;
            }
        }

        private static string VectorString(double[] l)
        {
            string s = "";
            for (int i = 0; i < l.GetLength(0); i++)
            {
                    s += l[i] + ", ";
                s += Environment.NewLine;
            }
            return s;
        }

        private static string ExtMatrixString(double[,] l, double[] r) {
            string s = "";
            for (int i = 0; i < l.GetLength(0); i++) {
                for (int j = 0; j < l.GetLength(1); j++) {
                    s += l[i, j] + ", ";
                }
                s += " | " + r[i];
                s += Environment.NewLine;
            }
            return s;
        }

        private double[,] LeftParser(string s) {
            var firstCol = new List<List<double>>();
            bool afterFirst = false;
            var parts = s.Split('{');


            int y = 0;
            foreach (var part in parts.Where(part => !string.IsNullOrEmpty(part))) {
                if (!string.IsNullOrEmpty(part)) {
                    var nums = part.Split(',').Select(t=>t.Trim(' ').Trim('}'));
                    firstCol.Add(new List<double>());
                    foreach (var num in nums.Where(num => !string.IsNullOrEmpty(num))) {
                        firstCol[y].Add(Convert.ToDouble(num, ifp));
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
            var nums = s.Split(',').Select(t => t.Trim(' ').Trim('}').Trim('{'));
            foreach (var num in nums.Where(num => !string.IsNullOrEmpty(num)))
            {
                firstCol.Add(Convert.ToDouble(num, ifp));
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
