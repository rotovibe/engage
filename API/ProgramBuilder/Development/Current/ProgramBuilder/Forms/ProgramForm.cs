using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using ProgramBuilder.Forms;
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
using ProgramBuilder.UserControls;

namespace ProgramBuilder
{
    public partial class ProgramForm : Form_Base
    {
        public TreeNode prNT;
        public TreeNode m;
        public TreeNode a;
        private TreeNode m_OldSelectNode;
        public string programNameText;
        public string moduleNameText;
        public string actionNameText;
        public string stepNameText;
        ProgramsUserControl puc = new ProgramsUserControl();
        ModulesUserControl muc = new ModulesUserControl();
        ActionsUserControl auc = new ActionsUserControl();
        StepsUserControl suc = new StepsUserControl();

        public ProgramForm()
        {
            InitializeComponent();
        }

        private void NewProgramButton_Click(object sender, EventArgs e)
        {
            ProgramNameForm programName = new ProgramNameForm();
            programName.ShowDialog();
            programName.NameBox.Focus();

            if (!(String.IsNullOrEmpty(programName.getProgramName())))
            {
                programNameText = programName.getProgramName();
                prNT = new TreeNode(programNameText);
                prNT.Tag = "Program";
                ProgramTree.Nodes.Add(prNT);
                //TreeNode test = new TreeNode("ModuleTest");
                //test.Tag = "Module";
                //prNT.Nodes.Add(test);
                //TreeNode test1 = new TreeNode("ActionTest");
                //test1.Tag = "Action";
                //test.Nodes.Add(test1);
                //TreeNode test2 = new TreeNode("StepTest");
                //test2.Tag = "Step";
                //test1.Nodes.Add(test2);
            }
        }

        private void ProgramTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point p = new Point(e.X, e.Y);
                TreeNode node = ProgramTree.GetNodeAt(p);
                if (node != null)
                {

                     //Select the node the user has clicked.
                     //The node appears selected until the menu is displayed on the screen.
                    m_OldSelectNode = ProgramTree.SelectedNode;
                    ProgramTree.SelectedNode = node;

                     //Find the appropriate ContextMenu depending on the selected node.
                    //programContextMenuStrip.Show(ProgramTree, p);
                    switch (Convert.ToString(node.Tag))
                    {
                        case "Program":
                            programContextMenuStrip.Show(ProgramTree, p);
                            break;
                        case "Module":
                            moduleContextMenuStrip.Show(ProgramTree, p);
                            break;
                        case "Action":
                            actionContextMenuStrip.Show(ProgramTree, p);
                            break;
                        case "Step":
                            stepContextMenuStrip.Show(ProgramTree, p);
                            break;
                    }

                    // Highlight the selected node.
                    ProgramTree.SelectedNode = m_OldSelectNode;
                    m_OldSelectNode = null;
                }
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ProgramTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.mainPanel.Controls.Clear();
            switch (Convert.ToString(ProgramTree.SelectedNode.Tag))
            {
                case "Program":
                    programNameText = ProgramTree.SelectedNode.Text;
                    puc.addName(programNameText);
                    this.mainPanel.Controls.Add(puc);
                    break;
                case "Module":
                    moduleNameText = ProgramTree.SelectedNode.Text;
                    muc.addName(moduleNameText);
                    this.mainPanel.Controls.Add(muc);
                    break;
                case "Action":
                    actionNameText = ProgramTree.SelectedNode.Text;
                    auc.addName(actionNameText);
                    this.mainPanel.Controls.Add(auc);
                    break;
                case "Step":
                    stepNameText = ProgramTree.SelectedNode.Text;
                    suc.addName(stepNameText);
                    this.mainPanel.Controls.Add(suc);
                    break;
            }
        }

        private void mnuNewModule_Click(object sender, EventArgs e)
        {
            ModuleListForm moduleList = new ModuleListForm();
            moduleList.ShowDialog();
            if (moduleList.DialogResult.Equals(DialogResult.OK))
            {
                foreach (ListViewItem l in moduleList.moduleListView.CheckedItems)
                {
                    m = new TreeNode(l.SubItems[0].Text);
                    m.Tag = "Module";
                    prNT.Nodes.Add(m);
                }
            }
        }

        private void mnuNewAction_Click(object sender, EventArgs e)
        {
            ActionListForm actionList = new ActionListForm();
            actionList.ShowDialog();
            if(actionList.DialogResult.Equals(DialogResult.OK))
            {
                foreach(ListViewItem l in actionList.actionListView.CheckedItems)
                {
                    a = new TreeNode(l.SubItems[0].Text);
                    a.Tag = "Action";
                    m.Nodes.Add(a);
                }
            }
        }

        private void mnuNewStep_Click(object sender, EventArgs e)
        {
            StepListForm stepList = new StepListForm();
            stepList.ShowDialog();
            if(stepList.DialogResult.Equals(DialogResult.OK))
            {
                foreach(ListViewItem l in stepList.stepListView.CheckedItems)
                {
                    TreeNode s = new TreeNode(l.SubItems[0].Text);
                    s.Tag = "Step";
                    a.Nodes.Add(s);
                }
            }
        }

        private void mnuDeleteProgram_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this Program?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ProgramTree.SelectedNode.Remove();
                this.mainPanel.Controls.Clear();
            }
        }

        private void mnuDeleteModule_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this Module?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ProgramTree.SelectedNode.Remove();
                //modulePanel.Visible = false;
            }
        }

        private void mnuDeleteAction_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this Action?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ProgramTree.SelectedNode.Remove();
                //actionPanel.Visible = false;
            }
        }

        private void mnuDeleteStep_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this Step?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ProgramTree.SelectedNode.Remove();
                //stepPanel.Visible = false;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            MEProgram newProgram = makeProgram();
            List<Module> modulesList = new List<Module>();

            foreach(TreeNode t in prNT.Nodes)
            {
                Module newModule = makeModule();
                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action> actionsList = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action>();
                modulesList.Add(newModule);

                foreach(TreeNode at in m.Nodes)
                {
                    Phytel.API.DataDomain.Program.MongoDB.DTO.Action newAction = makeAction();
                    List<Step> stepsList = new List<Step>();
                    actionsList.Add(newAction);

                    foreach(TreeNode st in a.Nodes)
                    {
                        Step newStep = makeStep();
                        stepsList.Add(newStep);
                    }

                    newAction.Steps = stepsList;

                }

                newModule.Actions = actionsList;

            }

            newProgram.Modules = modulesList;




            
            //{
            //    //ContractId
            //    Client = ObjectId.Parse(puc.cliTextBox.Text),
            //    //Completed
            //    DeleteFlag = false,
            //    Description = puc.descTextBox.Text,
            //    //Enabled
            //    EndDate = System.DateTime.Parse(puc.edTextBox.Text),
            //    Name = puc.nmTextBox.Text,
            //    //Next = ObjectId.Parse(""),
            //    //Order = puc.oNumericUpDown.Value,
            //    //Objectives
            //    //Population = 
            //    //Previous = ObjectId.Parse(""),
            //    //ProgramTemplateId
            //    StartDate = System.DateTime.Parse(puc.sdTextBox.Text),
            //    ShortName = puc.snTextBox.Text,
            //    //SourceId
            //    //Status = puc.stsNumericUpDwn.Value
            //    //TTLDate
            //    //UpdatedBy
            //    //LastUpdatedOn
            //    //Version
            //};

            //MEProgramAttribute newProgramAttribute = new MEProgramAttribute("000000000000000000000000")
            //{
            //    AuthoredBy = puc.athbyTextBox.Text,
            //    //Completed
            //    DeleteFlag = false,
            //    EndDate = System.DateTime.Parse(puc.edTextBox.Text),
            //    //EligibilityEndDate
            //    //EligibilityRequirements
            //    //EligibilityStartDate
            //    //Locked
            //    //Population
            //    StartDate = System.DateTime.Parse(puc.sdTextBox.Text)
            //    //Status puc.stsNumericUpDwn.Value,
            //    //TTLDate
            //    //UpdatedBy
            //    //LastUpdatedOn
            //    //Version
            //};

            //Module

        }

        public MEProgram makeProgram()
        {
            MEProgram newProgram = new MEProgram("000000000000000000000000")
            {
                Id = ObjectId.GenerateNewId()
            };

            foreach (DataGridViewRow r in puc.dataGridView1.Rows)
            {

                String rValue = r.Cells[1].Value.ToString();

                switch (r.Cells[0].Value.ToString())
                {
                    //case "Client:":
                    //    newProgram.Client = ObjectId.Parse(rValue);
                    //    break;
                    case "Program Name:":
                        newProgram.Name = rValue;
                        break;
                    case "Short Name:":
                        newProgram.ShortName = rValue;
                        break;
                    case "Description:":
                        newProgram.Description = rValue;
                        break;
                    case "Start Date:":
                        if (String.IsNullOrEmpty(rValue))
                            break;
                        else
                        {
                            newProgram.StartDate = System.DateTime.Parse(rValue);
                            break;
                        }
                    case "End Date:":
                        if (String.IsNullOrEmpty(rValue))
                            break;
                        else
                        {
                            newProgram.EndDate = System.DateTime.Parse(rValue);
                            break;
                        }
                    case "Order:":
                        if (String.IsNullOrEmpty(rValue))
                            break;
                        else
                        {
                            newProgram.Order = Convert.ToInt32(rValue);
                            break;
                        }

                }
            }

            return newProgram;
        }

        public Module makeModule()
        {
            Module newModule = new Module()
            {
                Id = ObjectId.GenerateNewId()
            };

            foreach (DataGridViewRow r in muc.dataGridView1.Rows)
            {
                String rValue = r.Cells[1].Value.ToString();

                switch (r.Cells[0].Value.ToString())
                {
                    case "Module Name:":
                        newModule.Name = rValue;
                        break;
                    case "Description:":
                        newModule.Description = rValue;
                        break;
                    case "Status:":
                        //newModule.Status =  rValue;
                        break;
                }
            }

            return newModule;
        }

        public Phytel.API.DataDomain.Program.MongoDB.DTO.Action makeAction()
        {
            Phytel.API.DataDomain.Program.MongoDB.DTO.Action newAction = new Phytel.API.DataDomain.Program.MongoDB.DTO.Action()
            {
                Id = ObjectId.GenerateNewId()
            };
            foreach(DataGridViewRow r in auc.dataGridView1.Rows)
            {
                String rValue = r.Cells[1].Value.ToString();

                switch (r.Cells[0].Value.ToString())
                {
                    case "Action Name:":
                        newAction.Name = rValue;
                        break;
                    case "Description:":
                        newAction.Description = rValue;
                        break;
                    case "Status:":
                        //newModule.Status =  rValue;
                        break;
                }
            }

            return newAction;
        }

        public Step makeStep()
        {
            Step newStep = new Step()
            {
                Id = ObjectId.GenerateNewId()
            };
            foreach(DataGridViewRow r in suc.dataGridView1.Rows)
            {
                String rValue = r.Cells[1].Value.ToString();

                switch (r.Cells[0].Value.ToString())
                {
                    case "Title:":
                        newStep.Title = rValue;
                        break;
                    case "Description:":
                        newStep.Description = rValue;
                        break;
                    case "Notes:":
                        newStep.Notes = rValue;
                        break;
                    case "Question:":
                        newStep.Question = rValue;
                        break;
                    case "Response:":
                        //newStep.Responses = rValue;
                        break;
                    case "Text:":
                        newStep.Text = rValue;
                        break;
                    case "Type:":
                        break;
                    case "Next:":
                        newStep.Next = ObjectId.Parse(rValue);
                        break;
                    case "Previous:":
                        newStep.Previous = ObjectId.Parse(rValue);
                        break;
                }
            }

            return newStep;
        }

        public string getProgramName()
        {
            return programNameText;
        }
    }
}
