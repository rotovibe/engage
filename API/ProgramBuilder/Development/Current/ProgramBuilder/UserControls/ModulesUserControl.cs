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
    public partial class ModulesUserControl : UserControl
    {
        public string moduleNameText;

        public ModulesUserControl()
        {
            InitializeComponent();
        }

        public ModulesUserControl(string s)
        {
            moduleNameText = s;
            InitializeComponent();
        }

        private void ModulesUserControl_Load(object sender, EventArgs e)
        {
            uonTextBox.Text = System.DateTime.Now.ToString();
            nmTextBox.Text = moduleNameText;
        }
    }
}
