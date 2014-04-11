using Phytel.API.DataDomain.Module.DTO;
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
        public Module m;

        public ModulesUserControl()
        {
            InitializeComponent();
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
            dr["PropertyValue"] = m.Name;
            dt.Rows.Add(dr);
            DataRow dr2 = dt.NewRow();
            dr2["PropertyName"] = "Description:";
            dr2["PropertyValue"] = m.Description;
            dt.Rows.Add(dr2);
            DataRow dr3 = dt.NewRow();
            dr3["PropertyName"] = "Status:";
            //dr2["PropertyValue"] = m.Status.ToString();
            dt.Rows.Add(dr3);
            

            ds.Tables.Add(dt);
            dataGridView1.DataSource = dt.DefaultView;

            dataGridView1.Columns["PropertyName"].ReadOnly = true;
        }

        public void addModule(Module mod)
        {
            m = mod;
            addItems();
        }
    }
}
