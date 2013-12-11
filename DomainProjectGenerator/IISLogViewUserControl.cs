using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using DomainProjectGenerator.Modals;
using System.IO;

namespace DomainProjectGenerator
{
    public partial class IISLogViewUserControl : UserControl
    {
        public class ReportDTO
        {
            public string KeyName;
            public string Value;
        }

        public IISLogViewUserControl()
        {
            InitializeComponent();
            InitializeReportList();
            InitializeQueryTextBox(SummarySyntaxRichTextBox);
            InitializeContextMenuItems();
        }

        private void InitializeContextMenuItems()
        {
            string[] splitCols = ConfigurationManager.AppSettings["Columns"].Split(',');

            foreach (string s in splitCols)
            {
                ToolStripMenuItem mi = new ToolStripMenuItem();
                mi.Name = s;
                mi.Tag = mi.Text = s;
                KeyWordsContextMenuStrip.Items.Add(mi);
            }
        }

        private void InitializeQueryTextBox<T>(T tb) where T : SyntaxHighlighter.SyntaxRichTextBox
        {
            // Add the keywords to the list.
            tb.Settings.Keywords.Add("select");
            tb.Settings.Keywords.Add("from");
            tb.Settings.Keywords.Add("where");
            tb.Settings.Keywords.Add("group");
            tb.Settings.Keywords.Add("by");
            tb.Settings.Keywords.Add("order");
            tb.Settings.Keywords.Add("and");
            tb.Settings.Keywords.Add("or");
            tb.Settings.Keywords.Add("[");
            tb.Settings.Keywords.Add("]");
            tb.Settings.Keywords.Add("as");
            tb.Settings.Comment = "--";
            tb.Settings.KeywordColor = Color.Blue;
            tb.Settings.CommentColor = Color.Green;
            tb.Settings.StringColor = Color.Gray;
            tb.Settings.IntegerColor = Color.Red;
            tb.Settings.EnableStrings = false;
            tb.Settings.EnableIntegers = false;
            tb.CompileKeywords();
            tb.ProcessAllLines();
        }

        private void InitializeReportList()
        {
            ConfigurationManager.RefreshSection("appSettings");
            ReportsTableLayoutPanel.Controls.Clear();
            foreach (string s in ConfigurationManager.AppSettings.AllKeys)
            {
                if (!s.ToLower().Equals("columns"))
                {
                    LinkLabel lbl = new LinkLabel();
                    lbl.ForeColor = Color.Black;
                    lbl.AutoSize = true;
                    lbl.ActiveLinkColor = System.Drawing.Color.DarkRed;
                    lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    lbl.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
                    lbl.LinkColor = System.Drawing.Color.Black;
                    lbl.VisitedLinkColor = System.Drawing.Color.Black;

                    lbl.Text = "- " + s;
                    lbl.Tag = new ReportDTO { KeyName = s, Value = ConfigurationManager.AppSettings[s] };
                    lbl.Click += lbl_Click;
                    ReportsTableLayoutPanel.Controls.Add(lbl);
                }
            }
        }


        void lbl_Click(object sender, EventArgs e)
        {
            try
            {
                string value = ((ReportDTO)((LinkLabel)sender).Tag).Value;
                RunReport(value);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Unexpected error: " + exc.Message);
            }
        }

        private void RunReport(string lPath)
        {
            try
            {
                if (FilePathTextBox.Text.Equals(string.Empty))
                {
                    MessageBox.Show("Please pick a log path!");
                    return;
                }

                //if (!LogFilesExist(FilePathTextBox.Text))
                //{
                //    MessageBox.Show(FilePathTextBox.Text + " does not have any log files in it." + Environment.NewLine + " Please make sure the directory chosen contains log files.");
                //    return;
                //}

                MSUtil.LogQueryClass oLogQuery = new MSUtil.LogQueryClass();
                MSUtil.COMIISW3CInputContextClass oEVTInputFormat = new MSUtil.COMIISW3CInputContextClass();
                //MSUtil.COMW3CInputContextClass oEVTInputFormat = new MSUtil.COMW3CInputContextClass();
                oEVTInputFormat.recurse = -1;
                
                //#Path#
                string path = lPath.Replace("#Path#", FilePathTextBox.Text + "*.log");
                string query = path;

                MSUtil.ILogRecordset oRecordSet = oLogQuery.Execute(query, oEVTInputFormat);
                PopulateGrid(oRecordSet);
                oRecordSet.close();
                SummarySyntaxRichTextBox.Clear();
                SummarySyntaxRichTextBox.Text = lPath;
                SummarySyntaxRichTextBox.ProcessAllLines();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void RangeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RangeCheckBox.Checked)
            {
                RangePanel.Enabled = true;
            }
            else
            {
                RangePanel.Enabled = false;
            }
        }

        private void PopulateGrid(MSUtil.ILogRecordset oRecordSet)
        {
            ResultsDataGridView.Rows.Clear();
            ResultsDataGridView.Columns.Clear();
            for (int i = 0; i < oRecordSet.getColumnCount(); i++)
            {
                string colName = oRecordSet.getColumnName(i);
                ResultsDataGridView.Columns.Add(colName + "_" + i.ToString(), colName);
            }


            // Browse the recordset
            for (; !oRecordSet.atEnd(); oRecordSet.moveNext())
            {
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(ResultsDataGridView);

                string[] split = oRecordSet.getRecord().toNativeString("|").Split('|');
                for (int i = 0; i < split.Length; i++)
                {
                    dgvr.Cells[i].Value = split[i];
                    //ResultsDataGridView.Columns[oRecordSet.getColumnName(i) + "_" + i].Width =
                    //    split[i].Length > oRecordSet.getColumnName(i).Length ? split[i].Length : oRecordSet.getColumnName(i).Length;
                }

                ResultsDataGridView.Rows.Add(dgvr);
            }
        }

        private void FilePathButton_Click(object sender, EventArgs e)
        {
            DialogResult result = LogPathFolderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                FilePathTextBox.Text = LogPathFolderBrowserDialog.SelectedPath + @"\";
            }
        }

        private bool LogFilesExist(string p)
        {
            bool result = true;
            var allFileNames = Directory.EnumerateFiles(p).Select(r => Path.GetFileName(r));
            var logs = allFileNames.Where(fn => Path.GetExtension(fn) == ".log");
            if (logs.Count() == 0)
            {
                result = false;
            }
            return result;
        }

        private void SummarySyntaxRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                RunAdHocQuery();
                e.Handled = true;
            }
        }

        private void RunAdHocQuery()
        {
            RunReport(SummarySyntaxRichTextBox.Text);
        }

        private void KeyWordsContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            SummarySyntaxRichTextBox.SelectionLength = 0;
            SummarySyntaxRichTextBox.SelectedText = e.ClickedItem.Tag.ToString();
        }

        private void SummarySyntaxRichTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Tab)
            //{
            //    KeyWordsContextMenuStrip.Show(Cursor.Position);
            //    int p = SummarySyntaxRichTextBox.GetCharIndexFromPosition(new Point(SummarySyntaxRichTextBox.SelectionStart));
            //    e.Handled = true;
            //}
        }

        private void RunToolStripButton_Click(object sender, EventArgs e)
        {
            if (SummarySyntaxRichTextBox.Text.Length == 0)
                return;

            RunReport(SummarySyntaxRichTextBox.Text);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!SummarySyntaxRichTextBox.Text.Equals(string.Empty))
            {
                SaveQueryForm sqf = new SaveQueryForm(SummarySyntaxRichTextBox.Text);
                DialogResult result = sqf.ShowDialog();
                if (result == DialogResult.OK)
                {
                    InitializeReportList();
                }
            }
        }
    }
}
