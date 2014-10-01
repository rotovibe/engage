using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PhytelServicesManager
{
    public partial class frmNew : Form
    {
        public string ConnectionName { get; internal set; }
        public string ConnectionType { get; internal set; }

        public frmNew()
        {
            InitializeComponent();
        }

        private void frmNew_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtConnectionName.Text.Trim() == string.Empty || cboType.SelectedIndex < 0)
            {
                MessageBox.Show("Please specify a valid Connection Name/Type", "New Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ConnectionName = txtConnectionName.Text;
            ConnectionType = cboType.Text;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
