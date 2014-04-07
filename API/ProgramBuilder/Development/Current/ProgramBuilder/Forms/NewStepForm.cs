using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramBuilder.Forms
{
    public partial class NewStepForm : Form
    {
        public NewStepForm()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(nmTextBox.Text))
            {
                MessageBox.Show("Please enter a name for the new Step");
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
        }

        private void closeButton_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
