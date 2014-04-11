using Phytel.API.DataDomain.Action.DTO;
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
        public ActionData a;

        public ActionsUserControl()
        {
            InitializeComponent();
        }

        private void ActionsUserControl_Load(object sender, EventArgs e)
        {
            //uonTextBox.Text = System.DateTime.Now.ToString();
            //nmTextBox.Text = actionNameText;
        }

        private void addItems()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr = null;
            DataColumn dc = dt.Columns.Add("PropertyName");
            DataColumn dc2 = dt.Columns.Add("PropertyValue");
            dr = dt.NewRow();
            dr["PropertyName"] = "Action Name:";
            dr["PropertyValue"] = a.Name;
            dt.Rows.Add(dr);
            DataRow dr2 = dt.NewRow();
            dr2["PropertyName"] = "Description:";
            dr["PropertyValue"] = a.Description;
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow();
            dr3["PropertyName"] = "Status:";
            dt.Rows.Add(dr3);

            ds.Tables.Add(dt);
            dataGridView1.DataSource = dt.DefaultView;

            dataGridView1.Columns["PropertyName"].ReadOnly = true;
        }

        public void addAction(ActionData action)
        {
            a = action;
            addItems();
        }
    }
}
