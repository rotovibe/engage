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
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
