using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramBuilder.UserControls
{
    public partial class StepsUserControl : UserControl
    {
        public string stepNameText;

        public StepsUserControl()
        {
            InitializeComponent();
        }

        public StepsUserControl(string s)
        {
            stepNameText = s;
            InitializeComponent();
        }

        private void StepsUserControl_Load(object sender, EventArgs e)
        {
            uonTextBox.Text = System.DateTime.Now.ToString();
            tTextBox.Text = stepNameText;
        }
    }
}
