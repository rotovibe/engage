using Phytel.API.DataDomain.Module.DTO;
using MongoDB.Bson;
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
    public partial class NewModuleForm : Form
    {
        public NewModuleForm()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void addButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(nmTextBox.Text))
            {
                MessageBox.Show("Please enter a name for the new Module");
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
        }
    }
}
