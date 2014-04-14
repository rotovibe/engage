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
    public partial class ProgramsUserControl : UserControl
    {
        public string programNameText;

        public ProgramsUserControl()
        {
            InitializeComponent();
        }

        private void ProgramsUserControl_Load(object sender, EventArgs e)
        {
            //vTextBox.Text = "1.0";
            //uonTextBox.Text = System.DateTime.Now.ToString();
            //nmTextBox.Text = programNameText;
        }

        private void addItems()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr = null;
            DataColumn dc = dt.Columns.Add("PropertyName");
            DataColumn dc2 = dt.Columns.Add("PropertyValue");
            dr = dt.NewRow();
            dr["PropertyName"] = "Authored By:";
            dt.Rows.Add(dr);
            DataRow dr2 = dt.NewRow();
            dr2["PropertyName"] = "Client:";
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow();
            dr3["PropertyName"] = "Program Name:";
            dr3["PropertyValue"] = programNameText;
            dt.Rows.Add(dr3);
            DataRow dr4 = dt.NewRow();
            dr4["PropertyName"] = "Short Name:";
            dt.Rows.Add(dr4);
            DataRow dr5 = dt.NewRow();
            dr5["PropertyName"] = "Description:";
            dt.Rows.Add(dr5);
            DataRow dr6 = dt.NewRow();
            dr6["PropertyName"] = "Start Date:";
            dt.Rows.Add(dr6);
            DataRow dr7 = dt.NewRow();
            dr7["PropertyName"] = "End Date:";
            dt.Rows.Add(dr7);
            DataRow dr8 = dt.NewRow();
            dr8["PropertyName"] = "Status:";
            dt.Rows.Add(dr8);
            DataRow dr9 = dt.NewRow();
            dr9["PropertyName"] = "Order:";
            dt.Rows.Add(dr9);
            DataRow dr10 = dt.NewRow();
            dr10["PropertyName"] = "Eligibility Start Date:";
            dt.Rows.Add(dr10);
            DataRow dr11 = dt.NewRow();
            dr11["PropertyName"] = "Eligibility End Date:";
            dt.Rows.Add(dr11);
            DataRow dr12 = dt.NewRow();
            dr12["PropertyName"] = "Eligibility Requirements:";
            dt.Rows.Add(dr12);
            DataRow dr13 = dt.NewRow();
            dr13["PropertyName"] = "Population:";
            dt.Rows.Add(dr13);
            DataRow dr14 = dt.NewRow();
            dr14["PropertyName"] = "Completed:";
            dt.Rows.Add(dr14);
            DataRow dr15 = dt.NewRow();
            dr15["PropertyName"] = "Enabled:";
            dt.Rows.Add(dr15);
            DataRow dr17 = dt.NewRow();
            dr17["PropertyName"] = "Contract ID:";
            dt.Rows.Add(dr17);
            DataRow dr18 = dt.NewRow();
            dr18["PropertyName"] = "Program Template ID:";
            dt.Rows.Add(dr18);
            DataRow dr19 = dt.NewRow();
            dr19["PropertyName"] = "Source ID:";
            dt.Rows.Add(dr19);
            
            
            ds.Tables.Add(dt);
            dataGridView1.DataSource = dt.DefaultView;
            DataGridViewComboBoxCell ce = new DataGridViewComboBoxCell();
            //dataGridView1.Columns[1].CellTemplate = ce;
            //string[] data = { "A", "B", "C" };
            //ce.Items.AddRange(data);
            //dataGridView1.Rows.Add();
            //dataGridView1.Rows[18].Cells[1] = ce;
            //DataGridViewComboBoxCell combo = dataGridView1.Rows[0].Cells[1] as DataGridViewComboBoxCell;
            //string[] data = { "item A", "item B", "item C" };
            //combo.Items.AddRange(data);
            //this.dataGridView1.Rows[9].Cells[1] = combo;
            //combo.ValueMember = "column1";
            //combo.DataSource = new BindingSource(cbdt, "column1");

            //dgvcbc.DisplayMember = "column1";
            //dgvcbc.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            //dataGridView1.Rows[0].Cells[0] = dgvcbc;
            
            
            dataGridView1.Columns["PropertyName"].ReadOnly = true;
        }

        public void addName(String programName)
        {
            programNameText = programName;
            addItems();
        }


    }
}
