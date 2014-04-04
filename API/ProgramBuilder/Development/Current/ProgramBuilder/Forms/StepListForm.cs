using MongoDB.Bson;
using ProgramBuilder.Forms;
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
    public partial class StepListForm : Form
    {
        public StepListForm()
        {
            InitializeComponent();
        }

        private void addStepButton_Click(object sender, EventArgs e)
        {
            NewStepForm newStep = new NewStepForm();
            newStep.ShowDialog();
            if(newStep.DialogResult.Equals(DialogResult.OK))
            {
                //Step nStep = new Step
                //{

                //};
                String sName = newStep.nmTextBox.Text;
                String sId = ObjectId.GenerateNewId().ToString();
                ListViewItem lvi = stepListView.Items.Add(new ListViewItem(sName, sId));
                lvi.Checked = true;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }
    }
}
