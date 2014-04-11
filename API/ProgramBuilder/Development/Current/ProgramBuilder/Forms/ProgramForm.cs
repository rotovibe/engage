using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.Action.DTO;
using Phytel.API.DataDomain.Module.DTO;
using Phytel.API.DataDomain.Program.DTO;
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
        public string programNameText;
        public string moduleNameText;
        public string actionNameText;
        public string stepNameText;
        List<ProgramsUserControl> pucList = new List<ProgramsUserControl>();
        List<ModulesUserControl> mucList = new List<ModulesUserControl>();
        List<ActionsUserControl> aucList = new List<ActionsUserControl>();
        List<StepsUserControl> sucList = new List<StepsUserControl>();

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
                ProgramsUserControl npc = new ProgramsUserControl();
                npc.addName(programNameText);
                pucList.Add(npc);
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
                    ProgramTree.SelectedNode = node;
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
                }
            }
        }

        private void ProgramTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.mainPanel.Controls.Clear();
            switch (Convert.ToString(ProgramTree.SelectedNode.Tag))
            {
                case "Program":
                    int p = pucList.FindIndex(x => x.programNameText == ProgramTree.SelectedNode.Text);
                    if(p >= 0)
                        this.mainPanel.Controls.Add(pucList[p]);
                    break;
                case "Module":
                    int m = mucList.FindIndex(x => x.m.Name == ProgramTree.SelectedNode.Text);
                    if(m >= 0)
                        this.mainPanel.Controls.Add(mucList[m]);
                    break;
                case "Action":
                    int a = aucList.FindIndex(x => x.a.Name == ProgramTree.SelectedNode.Text);
                    if(a >= 0)
                        this.mainPanel.Controls.Add(aucList[a]);
                    break;
                case "Step":
                    int s = sucList.FindIndex(x => x.stepNameText == ProgramTree.SelectedNode.Text);
                    if(s >= 0)
                        this.mainPanel.Controls.Add(sucList[s]);
                    break;
                default:
                    break;
            }
        }

        private void mnuNewModule_Click(object sender, EventArgs e)
        {
            ModuleListForm moduleList = new ModuleListForm();
            moduleList.ShowDialog();
            if (moduleList.DialogResult.Equals(DialogResult.OK))
            {
                foreach (Phytel.API.DataDomain.Module.DTO.Module mod in moduleList.listModules)
                {
                    m = new TreeNode(mod.Name);
                    m.Tag = "Module";
                    ProgramTree.SelectedNode.Nodes.Add(m);
                    ModulesUserControl nmc = new ModulesUserControl();
                    nmc.addModule(mod);
                    mucList.Add(nmc);
                }
            }
        }

        private void mnuNewAction_Click(object sender, EventArgs e)
        {
            ActionListForm actionList = new ActionListForm();
            actionList.ShowDialog();
            if(actionList.DialogResult.Equals(DialogResult.OK))
            {
                foreach(ActionData ad in actionList.listActions)
                {
                    a = new TreeNode(ad.Name);
                    a.Tag = "Action";
                    ProgramTree.SelectedNode.Nodes.Add(a);
                    ActionsUserControl nac = new ActionsUserControl();
                    nac.addAction(ad);
                    aucList.Add(nac);
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
                    ProgramTree.SelectedNode.Nodes.Add(s);
                    StepsUserControl nsc = new StepsUserControl();
                    stepNameText = s.Text;
                    nsc.addName(stepNameText);
                    sucList.Add(nsc);
                }
            }
        }

        private void mnuDeleteProgram_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this Program?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int p = pucList.FindIndex(x => x.programNameText == ProgramTree.SelectedNode.Text);
                pucList.RemoveAt(p);
                ProgramTree.SelectedNode.Remove();
                this.mainPanel.Controls.Clear();
            }
        }

        private void mnuDeleteModule_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this Module?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int m = mucList.FindIndex(x => x.m.Name == ProgramTree.SelectedNode.Text);
                mucList.RemoveAt(m);
                ProgramTree.SelectedNode.Remove();
            }
        }

        private void mnuDeleteAction_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this Action?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int a = aucList.FindIndex(x => x.a.Name == ProgramTree.SelectedNode.Text);
                aucList.RemoveAt(a);
                ProgramTree.SelectedNode.Remove();
            }
        }

        private void mnuDeleteStep_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this Step?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int s = sucList.FindIndex(x => x.stepNameText == ProgramTree.SelectedNode.Text);
                sucList.RemoveAt(s);
                ProgramTree.SelectedNode.Remove();
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            for (int p = 0; p < ProgramTree.GetNodeCount(false); p++)
            {
                MEProgram newProgram = makeProgram(p);
                List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module> modulesList = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Module>();
                for (int mo = 0; mo < ProgramTree.Nodes[p].GetNodeCount(false); mo++)
                {
                    Phytel.API.DataDomain.Program.MongoDB.DTO.Module newModule = makeModule(mo);
                    List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action> actionsList = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action>();
                    for (int ac = 0; ac < ProgramTree.Nodes[p].Nodes[mo].GetNodeCount(false); ac++ )
                    {
                        Phytel.API.DataDomain.Program.MongoDB.DTO.Action newAction = makeAction(ac);
                        List<Step> stepsList = new List<Step>();
                        for (int st = 0; st < ProgramTree.Nodes[p].Nodes[mo].Nodes[ac].GetNodeCount(false); st++)
                        {
                            Step newStep = makeStep(st);
                            stepsList.Add(newStep);
                        }

                        newAction.Steps = stepsList;
                        actionsList.Add(newAction);
                    }

                    newModule.Actions = actionsList;
                    modulesList.Add(newModule);
                }

                newProgram.Modules = modulesList;

            }

              //Possible fields needed for Program
        //    //{
        //    //    //ContractId
        //    //    Client = ObjectId.Parse(puc.cliTextBox.Text),
        //    //    //Completed
        //    //    DeleteFlag = false,
        //    //    Description = puc.descTextBox.Text,
        //    //    //Enabled
        //    //    EndDate = System.DateTime.Parse(puc.edTextBox.Text),
        //    //    Name = puc.nmTextBox.Text,
        //    //    //Next = ObjectId.Parse(""),
        //    //    //Order = puc.oNumericUpDown.Value,
        //    //    //Objectives
        //    //    //Population = 
        //    //    //Previous = ObjectId.Parse(""),
        //    //    //ProgramTemplateId
        //    //    StartDate = System.DateTime.Parse(puc.sdTextBox.Text),
        //    //    ShortName = puc.snTextBox.Text,
        //    //    //SourceId
        //    //    //Status = puc.stsNumericUpDwn.Value
        //    //    //UpdatedBy
        //    //    //LastUpdatedOn
        //    //    //Version
        //    //};

        //    //MEProgramAttribute newProgramAttribute = new MEProgramAttribute("000000000000000000000000")
        //    //{
        //    //    AuthoredBy = puc.athbyTextBox.Text,
        //    //    //Completed
        //    //    DeleteFlag = false,
        //    //    EndDate = System.DateTime.Parse(puc.edTextBox.Text),
        //    //    //EligibilityEndDate
        //    //    //EligibilityRequirements
        //    //    //EligibilityStartDate
        //    //    //Locked
        //    //    //Population
        //    //    StartDate = System.DateTime.Parse(puc.sdTextBox.Text)
        //    //    //Status puc.stsNumericUpDwn.Value,
        //    //    //UpdatedBy
        //    //    //LastUpdatedOn
        //    //    //Version
        //    //};


        }

        public MEProgram makeProgram(int n)
        {
            MEProgram newProgram = new MEProgram("000000000000000000000000")
            {
                Id = ObjectId.GenerateNewId()
            };

            foreach (DataGridViewRow r in pucList[n].dataGridView1.Rows)
            {
                String rValue = r.Cells[1].Value.ToString();
                if (!(String.IsNullOrEmpty(rValue)))
                {
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
                            newProgram.StartDate = System.DateTime.Parse(rValue);
                            break;
                        case "End Date:":
                            newProgram.EndDate = System.DateTime.Parse(rValue);
                            break;
                        case "Order:":
                                newProgram.Order = Convert.ToInt32(rValue);
                                break;
                    }
                }
            }

            return newProgram;
            
        }

        public Phytel.API.DataDomain.Program.MongoDB.DTO.Module makeModule(int n)
        {
            Phytel.API.DataDomain.Program.MongoDB.DTO.Module newModule = new Phytel.API.DataDomain.Program.MongoDB.DTO.Module()
            {
                Id = ObjectId.GenerateNewId()
            };

            foreach (DataGridViewRow r in mucList[n].dataGridView1.Rows)
            {
                String rValue = r.Cells[1].Value.ToString();
                if (!(String.IsNullOrEmpty(rValue)))
                {
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
            }

            return newModule;
        }

        public Phytel.API.DataDomain.Program.MongoDB.DTO.Action makeAction(int n)
        {
            Phytel.API.DataDomain.Program.MongoDB.DTO.Action newAction = new Phytel.API.DataDomain.Program.MongoDB.DTO.Action()
            {
                Id = ObjectId.GenerateNewId()
            };
            foreach(DataGridViewRow r in aucList[n].dataGridView1.Rows)
            {
                String rValue = r.Cells[1].Value.ToString();
                if (!(String.IsNullOrEmpty(rValue)))
                {
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
            }

            return newAction;
        }

        public Step makeStep(int n)
        {
            Step newStep = new Step()
            {
                Id = ObjectId.GenerateNewId()
            };
            foreach(DataGridViewRow r in sucList[n].dataGridView1.Rows)
            {
                String rValue = r.Cells[1].Value.ToString();
                if (!(String.IsNullOrEmpty(rValue)))
                {
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
            }

            return newStep;
        }
    }
}
