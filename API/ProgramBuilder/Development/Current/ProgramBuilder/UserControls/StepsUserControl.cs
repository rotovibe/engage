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
            addItems();
        }

        private void StepsUserControl_Load(object sender, EventArgs e)
        {
            //uonTextBox.Text = System.DateTime.Now.ToString();
            //tTextBox.Text = stepNameText;
        }

        private void addItems()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr = null;
            DataColumn dc = dt.Columns.Add("PropertyName");
            DataColumn dc2 = dt.Columns.Add("PropertyValue");
            dr = dt.NewRow();
            dr["PropertyName"] = "Title:";
            dt.Rows.Add(dr);
            DataRow dr2 = dt.NewRow();
            dr2["PropertyName"] = "Description:";
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow();
            dr3["PropertyName"] = "Notes:";
            dt.Rows.Add(dr3);
            DataRow dr4 = dt.NewRow();
            dr4["PropertyName"] = "Question:";
            dt.Rows.Add(dr4);
            DataRow dr5 = dt.NewRow();
            dr5["PropertyName"] = "Response:";
            dt.Rows.Add(dr5);
            DataRow dr6 = dt.NewRow();
            dr6["PropertyName"] = "Text:";
            dt.Rows.Add(dr6);
            DataRow dr7 = dt.NewRow();
            dr7["PropertyName"] = "Type:";
            dt.Rows.Add(dr7);
            DataRow dr8 = dt.NewRow();
            dr8["PropertyName"] = "Status:";
            dt.Rows.Add(dr8);
            DataRow dr9 = dt.NewRow();
            dr9["PropertyName"] = "Contract ID:";
            dt.Rows.Add(dr9);
            DataRow dr10 = dt.NewRow();
            dr10["PropertyName"] = "Select Type:";
            dt.Rows.Add(dr10);
            DataRow dr11 = dt.NewRow();
            dr11["PropertyName"] = "Include Time:";
            dt.Rows.Add(dr11);
            DataRow dr12 = dt.NewRow();
            dr12["PropertyName"] = "Next:";
            dt.Rows.Add(dr12);
            DataRow dr13 = dt.NewRow();
            dr13["PropertyName"] = "Previous:";
            dt.Rows.Add(dr13);

            ds.Tables.Add(dt);
            dataGridView1.DataSource = dt.DefaultView;

            dataGridView1.Columns["PropertyName"].ReadOnly = true;

        }
    }
}
