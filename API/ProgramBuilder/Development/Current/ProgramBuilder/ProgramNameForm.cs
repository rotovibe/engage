using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramBuilder
{
    public partial class ProgramNameForm : Form
    {
        string programNameText;

        public ProgramNameForm()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please enter a name for the new Program");
            }
            else
            {
                programNameText = textBox1.Text;
                this.Close();
            }
        }

        public string getProgramName()
        {
            return programNameText;
        }

    }
}
