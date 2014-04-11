using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Phytel.API.DataDomain.Module.DTO;
using Phytel.API.DataDomain.Action.DTO;

namespace ProgramBuilder.Forms
{
    public partial class ActionListForm : Form_Base
    {
        public List<ActionData> listActions = new List<ActionData>();

        public ActionListForm()
        {
            InitializeComponent();
        }

        private void addActionButton_Click(object sender, EventArgs e)
        {
            NewActionForm newAction = new NewActionForm();
            newAction.ShowDialog();
            if(newAction.DialogResult.Equals(DialogResult.OK))
            {
                ActionData nAction = new ActionData
                {
                    ID = ObjectId.GenerateNewId().ToString(),
                    Name = newAction.nmTextBox.Text
                };
                ListViewItem lvi = actionListView.Items.Add(new ListViewItem(nAction.Name, nAction.ID.ToString()));
                lvi.Checked = true;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ActionListForm_Load(object sender, EventArgs e)
        {
            List<ActionData> list = GetAllActionsRequestServiceCall().Actions ;

            foreach (ActionData action in list.OrderBy(x => x.Name))
            {
                actionListView.Items.Add(new ListViewItem(action.Name, action.ID));
                
            }
        }

        public GetAllActionsDataResponse GetAllActionsRequestServiceCall()
        {
            try
            {
                return GetData(DataDomainTypes.Action, "actions") as GetAllActionsDataResponse;
            }
            catch (Exception ex)
            {
                //TODO
                return null;
            }

            return null;
        }


        private void addButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }
    }
}
