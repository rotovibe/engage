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
            if (txtConnectionName.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please specify a valid Connection Name", "New Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ConnectionName = txtConnectionName.Text;
            ConnectionType = (rdoSQL.Checked ? "SQL" : "Mongo");

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
