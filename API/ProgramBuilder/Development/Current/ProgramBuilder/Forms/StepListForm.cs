using MongoDB.Bson;
using ProgramBuilder.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Phytel.API.DataDomain.Step.DTO;


namespace ProgramBuilder
{
    public partial class StepListForm : Form_Base
    {
        public List<StepData> listSteps = new List<StepData>();

        public StepListForm()
        {
            InitializeComponent();
        }

        private void addStepButton_Click(object sender, EventArgs e)
        {
            NewStepForm newStep = new NewStepForm();
            newStep.ShowDialog();
            if (newStep.DialogResult.Equals(DialogResult.OK))
            {
                StepData nStep = new StepData
                {
                    ID = ObjectId.GenerateNewId().ToString()
                };
                String sName = newStep.nmTextBox.Text;
                String sId = ObjectId.GenerateNewId().ToString();
                ListViewItem lvi = stepListView.Items.Add(new ListViewItem(sName, sId));
                lvi.Checked = true;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void StepListForm_Load(object sender, EventArgs e)
        {
            List<YesNoData> yesnolist = GetYesNoStepDataResponseServiceCall();
            List<TextData> textlist = GetTextStepDataResponseServiceCall();

            List<StepData> totallist = new List<StepData>();
            totallist.AddRange(yesnolist);
            totallist.AddRange(textlist);


            foreach (StepData step in totallist)
            {
                //actionListView.Items.Add(new ListViewItem(action.Name, action.ID));

            }
        }

        public List<YesNoData> GetYesNoStepDataResponseServiceCall()
        {
            try
            {
                GetAllYesNoStepDataResponse response = GetData(DataDomainTypes.Step, "step/yesno") as GetAllYesNoStepDataResponse;
                return response.Steps;
            }
            catch (Exception ex)
            {
                //TODO
                return null;
            }
        }

        public List<TextData> GetTextStepDataResponseServiceCall()
        {
            try
            {
                GetAllTextStepDataResponse response = GetData(DataDomainTypes.Step, "text") as GetAllTextStepDataResponse;
                return response.Steps;
            }
            catch (Exception ex)
            {
                //TODO
                return null;
            }
        }
    }
}
