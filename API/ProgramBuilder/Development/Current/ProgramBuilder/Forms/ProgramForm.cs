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
    public partial class ProgramForm : Form
    {
        private TreeNode m_OldSelectNode;
        public string programNameText;
        public string moduleNameText;
        public string actionNameText;
        public string stepNameText;

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
                TreeNode prNT = new TreeNode(programNameText);
                prNT.Tag = "Program";
                ProgramTree.Nodes.Add(prNT);
                TreeNode test = new TreeNode("ModuleTest");
                test.Tag = "Module";
                prNT.Nodes.Add(test);
                TreeNode test1 = new TreeNode("ActionTest");
                test1.Tag = "Action";
                test.Nodes.Add(test1);
                TreeNode test2 = new TreeNode("StepTest");
                test2.Tag = "Step";
                test1.Nodes.Add(test2);
            }
        }

        private void mnuNewModule_Click(object sender, EventArgs e)
        {
            ModuleListForm moduleName = new ModuleListForm();
            moduleName.ShowDialog();            
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
                    programContextMenuStrip.Show(ProgramTree, p);
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
                    ProgramsUserControl puc = new ProgramsUserControl(programNameText);
                    this.mainPanel.Controls.Add(puc);
                    break;
                case "Module":
                    moduleNameText = ProgramTree.SelectedNode.Text;
                    ModulesUserControl muc = new ModulesUserControl(moduleNameText);
                    this.mainPanel.Controls.Add(muc);
                    break;
                case "Action":
                    actionNameText = ProgramTree.SelectedNode.Text;
                    ActionsUserControl auc = new ActionsUserControl(actionNameText);
                    this.mainPanel.Controls.Add(auc);
                    break;
                case "Step":
                    stepNameText = ProgramTree.SelectedNode.Text;
                    StepsUserControl suc = new StepsUserControl();
                    this.mainPanel.Controls.Add(suc);
                    break;
            }
        }

        private void mnuNewAction_Click(object sender, EventArgs e)
        {
            ActionListForm actionList = new ActionListForm();
            actionList.ShowDialog();
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

        private void mnuNewStep_Click(object sender, EventArgs e)
        {
            StepListForm stepList = new StepListForm();
            stepList.ShowDialog();
        }

        private void updateProgramButton_Click(object sender, EventArgs e)
        {
            //MEProgram newProgram = new MEProgram("000000000000000000000000")
            //{
            //    Name = ProgramTree.SelectedNode.Text,
            //    Description = descTextBox.Text,
            //    StartDate = System.DateTime.Parse(sdTextBox.Text),
            //    EndDate = System.DateTime.Parse(edTextBox.Text)

            //};
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            ProgramsUserControl puc = new ProgramsUserControl();

            //MEProgram newProgram = new MEProgram("000000000000000000000000")
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

        public string getProgramName()
        {
            return programNameText;
        }
    }
}
