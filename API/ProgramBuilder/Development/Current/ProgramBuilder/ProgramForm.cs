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
            string programNameText = programName.getProgramName();
            TreeNode prNT = new TreeNode(programNameText);
            ProgramTree.Nodes.Add(prNT);
        }

        private void mnuNewModule_Click(object sender, EventArgs e)
        {
            ModuleNameForm moduleName = new ModuleNameForm();
            moduleName.ShowDialog();
        }

        private void ProgramTree_MouseUp(object sender, MouseEventArgs e)
        {
            
            // Show menu only if the right mouse button is clicked.
            if (e.Button == MouseButtons.Right)
            {

                // Point where the mouse is clicked.
                Point p = new Point(e.X, e.Y);

                // Get the node that the user has clicked.
                TreeNode node = treeView1.GetNodeAt(p);
                //if (node != null)
                //{

                    // Select the node the user has clicked.
                    // The node appears selected until the menu is displayed on the screen.
                    //m_OldSelectNode = treeView1.SelectedNode;
                    //treeView1.SelectedNode = node;

                    // Find the appropriate ContextMenu depending on the selected node.
                    contextMenuStrip1.Show(ProgramTree, p);
                    //switch (Convert.ToString(node.Tag))
                    //{
                    //    case "TextFile":
                    //        mnuTextFile.Show(treeView1, p);
                    //        break;
                    //    case "File":
                    //        mnuFile.Show(treeView1, p);
                    //        break;
                    //}

                    // Highlight the selected node.
                    treeView1.SelectedNode = m_OldSelectNode;
                    m_OldSelectNode = null;
                //}
            }
        }



        
    }
}
