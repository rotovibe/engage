using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramBuilder
{
    public partial class ProgramsUserControl : UserControl
    {
        public string programNameText;

        public ProgramsUserControl()
        {
            InitializeComponent();
        }

        public ProgramsUserControl(string s)
        {
            programNameText = s;
            InitializeComponent();
        }


        private void athbyTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void cliLabel_Click(object sender, EventArgs e)
        {

        }

        private void cliTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void nmTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void nmLabel_Click(object sender, EventArgs e)
        {

        }

        private void athbyLabel_Click(object sender, EventArgs e)
        {

        }

        private void snLabel_Click(object sender, EventArgs e)
        {

        }

        private void snTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void descLabel_Click(object sender, EventArgs e)
        {

        }

        private void descTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void sdLabel_Click(object sender, EventArgs e)
        {

        }

        private void sdTextBox_TextChanged(object sender, EventArgs e)
        {
            esdTextBox.Text = sdTextBox.Text;
        }

        private void edLabel_Click(object sender, EventArgs e)
        {

        }

        private void edTextBox_TextChanged(object sender, EventArgs e)
        {
            eedTextBox.Text = edTextBox.Text;
        }

        private void stsLabel_Click(object sender, EventArgs e)
        {

        }

        private void stsNumericUpDwn_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void ProgramsUserControl_Load(object sender, EventArgs e)
        {
            vTextBox.Text = "1.0";
            uonTextBox.Text = System.DateTime.Now.ToString();
            nmTextBox.Text = programNameText;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

    }
}
