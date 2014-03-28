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
    public partial class ActionsUserControl : UserControl
    {
        public string actionNameText;

        public ActionsUserControl()
        {
            InitializeComponent();
        }

        public ActionsUserControl(string s)
        {
            actionNameText = s;
            InitializeComponent();
        }

        private void ActionsUserControl_Load(object sender, EventArgs e)
        {
            uonTextBox.Text = System.DateTime.Now.ToString();
            nmTextBox.Text = actionNameText;
        }
    }
}
