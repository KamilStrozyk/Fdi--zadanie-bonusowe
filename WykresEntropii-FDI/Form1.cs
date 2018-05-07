using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace WykresEntropii_FDI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string path = "Wyniki.txt";
            StreamReader r=new StreamReader(path,true);
            int counter = 1;
            double val;
            string fval;
           while(!r.EndOfStream)
            {
                fval= r.ReadLine();
                if (fval != "")
                {
                    val = double.Parse(fval);
                    chart1.Series[0].Points.AddXY(counter, val);

                counter++;
                }
            }

            chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;
   
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].IsValueShownAsLabel = !chart1.Series[0].IsValueShownAsLabel;
        }
    }
}
