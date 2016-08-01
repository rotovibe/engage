using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Phytel.Data.ETL;

namespace NGETL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ETLProcessor pro = new ETLProcessor("InHealth001");
            pro.Rebuild();
            MessageBox.Show("ETL process complete!!!");
        }
    }
}
