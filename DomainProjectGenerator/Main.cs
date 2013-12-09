using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DomainProjectGenerator
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            DirectoryService.RaiseMessageEvent += DirectoryService_RaiseMessageEvent;
            IISLogViewUserControl iislvc = new IISLogViewUserControl();
            iislvc.Dock = DockStyle.Fill;
            IISLogViewerTabPage.Controls.Add(iislvc);
        }

        void DirectoryService_RaiseMessageEvent(TextMessageEventArg e)
        {
            ResultsTextBox.AppendText(e.Message + Environment.NewLine);
        }

        private void OpenDirectoryBrowserBtn_Click(object sender, EventArgs e)
        {
            if (FolderBrowserDialog.ShowDialog().Equals(DialogResult.OK))
            {
                WorkingDirectoryTextBox.Text = FolderBrowserDialog.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (FolderBrowserDialog.ShowDialog().Equals(DialogResult.OK))
            {
                TargetFolderPath.Text = FolderBrowserDialog.SelectedPath;
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.WorkingDirectoryTextBox.Text) || string.IsNullOrEmpty(this.TargetFolderPath.Text) || string.IsNullOrEmpty(this.EntityNameTxtBx.Text))
            {
                MessageBox.Show("Working/ Destination folder paths and target entity name cannot be null. Please enter values for each.");
                return;
            }

            Invoke((MethodInvoker)delegate
            {
                ExecuteProcedure();
            });
        }

        private void ExecuteProcedure()
        {
            string fullDomainName = "Phytel.API.DataDomain." + EntityNameTxtBx.Text;
            DirectoryInfo tdir = new DirectoryInfo(TargetFolderPath.Text)
                .CreateSubdirectory(fullDomainName);
            string workingTemplatePath = WorkingDirectoryTextBox.Text + "\\Phytel.API.DataDomain.Template";

            DirectoryService.DirectoryCopy(workingTemplatePath, tdir.FullName, EntityNameTxtBx.Text, true);

            DirectoryService.ChangeStringsInFiles(EntityNameTxtBx.Text,
                TargetFolderPath.Text + "\\" + fullDomainName);

            ResultsTextBox.AppendText("done!");
        }
    }
}
