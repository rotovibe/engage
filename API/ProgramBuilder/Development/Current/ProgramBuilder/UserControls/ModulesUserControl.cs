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
            addItems();
        }

        private void ModulesUserControl_Load(object sender, EventArgs e)
        {
            uonTextBox.Text = System.DateTime.Now.ToString();
            nmTextBox.Text = moduleNameText;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void addItems()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr = null;
            DataColumn dc = dt.Columns.Add("PropertyName");
            DataColumn dc2 = dt.Columns.Add("PropertyValue");
            dr = dt.NewRow();
            dr["PropertyName"] = "Module Name:";
            dt.Rows.Add(dr);
            DataRow dr2 = dt.NewRow();
            dr2["PropertyName"] = "Description:";
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow();
            dr3["PropertyName"] = "Status:";
            dt.Rows.Add(dr3);
            

            ds.Tables.Add(dt);
            dataGridView1.DataSource = dt.DefaultView;

            dataGridView1.Columns["PropertyName"].ReadOnly = true;

        }

    }
}
