using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramBuilder
{
    public partial class ProgramForm : Form
    {
        private TreeNode m_OldSelectNode;
        public ProgramForm()
        {
            InitializeComponent();
        }

        private void NewProgramButton_Click(object sender, EventArgs e)
        {
            ProgramNameForm programName = new ProgramNameForm();
            programName.ShowDialog();
            programName.NameBox.Focus();
            
            string programNameText = programName.getProgramName();
            TreeNode prNT = new TreeNode(programNameText);
            prNT.Tag = "Program";
            ProgramTree.Nodes.Add(prNT);
            TreeNode test = new TreeNode("ModuleTest");
            test.Tag = "Module";
            ProgramTree.Nodes.Add(test);
            TreeNode test1 = new TreeNode("ActionTest");
            test1.Tag = "Action";
            ProgramTree.Nodes.Add(test1);
            TreeNode test2 = new TreeNode("StepTest");
            test2.Tag = "Step";
            ProgramTree.Nodes.Add(test2);
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
            programPanel.Visible = false;
            modulePanel.Visible = false;
            actionPanel.Visible = false;
            stepPanel.Visible = false;
            switch (Convert.ToString(ProgramTree.SelectedNode.Tag))
            {
                case "Program":
                    programPanel.Visible = true;
                    break;
                case "Module":
                    modulePanel.Visible = true;
                    break;
                case "Action":
                    actionPanel.Visible = true;
                    break;
                case "Step":
                    stepPanel.Visible = true;
                    break;
            }
        }

        private void addActionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActionListForm actionList = new ActionListForm();
            actionList.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this Program?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ProgramTree.SelectedNode.Remove();
                programPanel.Visible = false;
            }
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this Module?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ProgramTree.SelectedNode.Remove();
                modulePanel.Visible = false;
            }
        }

        private void deleteToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this Action?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ProgramTree.SelectedNode.Remove();
                actionPanel.Visible = false;
            }
        }

        private void deleteToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this Step?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ProgramTree.SelectedNode.Remove();
                stepPanel.Visible = false;
            }
        }



        
    }
}
