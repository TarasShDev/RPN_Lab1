using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPN_Lab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listBox1.Items.Add("x\t\tf(x)");
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            chart1.Series[0].Points.Clear();
            if (string.IsNullOrEmpty(textBoxExpression.Text))
                return;

            RPNClass obj1 = new RPNClass();
            bool res = obj1.GetRPNString(textBoxExpression.Text, out string rpnString);
            textBoxRPN.Text = rpnString;
            if (!res)
                return;

            double x1 = double.Parse(textBoxX1.Text), x2 = double.Parse(textBoxX2.Text);
            double step = double.Parse(textBox1.Text);
            if (x1 >= x2)
            {
                MessageBox.Show("x2 має бути більше x1!!!");
                return;
            }

            Dictionary<double, double> table = new Dictionary<double, double>();
            try
            {
                for (double x = x1; x <= x2; x += step)
                {
               
                    double result = obj1.Evaluate(rpnString: rpnString, x: x);
                    if (double.IsInfinity(result))
                        continue;
                    table.Add(key: x, value: result);
                
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            listBox1.Items.AddRange(table.Select(x => $"{x.Key.ToString("F4")}\t\t{x.Value.ToString("F4")}").ToArray());
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            foreach(var point in table)
            {
                chart1.Series[0].Points.AddXY(point.Key, point.Value);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
