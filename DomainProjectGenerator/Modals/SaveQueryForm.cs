using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DomainProjectGenerator.Modals
{
    public partial class SaveQueryForm : Form
    {
        public SaveQueryForm(string query)
        {
            InitializeComponent();
            QueryPreviewSyntaxRichTextBox.Text = query;
            InitializeQueryTextBox(QueryPreviewSyntaxRichTextBox);
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection app = config.AppSettings;
            app.Settings.Add(NameTextBox.Text, QueryPreviewSyntaxRichTextBox.Text);
            config.Save(ConfigurationSaveMode.Modified);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void QueryPreviewSyntaxRichTextBox_TextChanged(object sender, EventArgs e)
        {
            QueryPreviewSyntaxRichTextBox.ProcessAllLines();
        }
    }
}
