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
    public partial class ActionListForm : Form
    {
        public ActionListForm()
        {
            InitializeComponent();
        }

        private void addActionButton_Click(object sender, EventArgs e)
        {
            NewActionForm newAction = new NewActionForm();
            newAction.ShowDialog();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
