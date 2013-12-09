namespace DomainProjectGenerator
{
    partial class IISLogViewUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IISLogViewUserControl));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.FilePathButton = new System.Windows.Forms.Button();
            this.FilePathTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ReportsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.RangePanel = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.EndTimeDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.RangeCheckBox = new System.Windows.Forms.CheckBox();
            this.StartTimeDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ResultsDataGridView = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.RunToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.SummarySyntaxRichTextBox = new SyntaxHighlighter.SyntaxRichTextBox();
            this.KeyWordsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.LogPathFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.RangePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultsDataGridView)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.FilePathButton);
            this.splitContainer1.Panel1.Controls.Add(this.FilePathTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.ReportsTableLayoutPanel);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.RangePanel);
            this.splitContainer1.Panel1.Controls.Add(this.RangeCheckBox);
            this.splitContainer1.Panel1.Controls.Add(this.StartTimeDateTimePicker);
            this.splitContainer1.Panel1.Controls.Add(this.StartDateTimePicker);
            this.splitContainer1.Panel1MinSize = 336;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(694, 457);
            this.splitContainer1.SplitterDistance = 336;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 3;
            // 
            // FilePathButton
            // 
            this.FilePathButton.Location = new System.Drawing.Point(293, 19);
            this.FilePathButton.Name = "FilePathButton";
            this.FilePathButton.Size = new System.Drawing.Size(31, 23);
            this.FilePathButton.TabIndex = 10;
            this.FilePathButton.Text = "...";
            this.FilePathButton.UseVisualStyleBackColor = true;
            this.FilePathButton.Click += new System.EventHandler(this.FilePathButton_Click);
            // 
            // FilePathTextBox
            // 
            this.FilePathTextBox.Location = new System.Drawing.Point(18, 21);
            this.FilePathTextBox.Name = "FilePathTextBox";
            this.FilePathTextBox.Size = new System.Drawing.Size(272, 20);
            this.FilePathTextBox.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Reports:";
            // 
            // ReportsTableLayoutPanel
            // 
            this.ReportsTableLayoutPanel.AutoSize = true;
            this.ReportsTableLayoutPanel.ColumnCount = 1;
            this.ReportsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ReportsTableLayoutPanel.Location = new System.Drawing.Point(18, 135);
            this.ReportsTableLayoutPanel.Name = "ReportsTableLayoutPanel";
            this.ReportsTableLayoutPanel.Padding = new System.Windows.Forms.Padding(2);
            this.ReportsTableLayoutPanel.RowCount = 1;
            this.ReportsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ReportsTableLayoutPanel.Size = new System.Drawing.Size(312, 56);
            this.ReportsTableLayoutPanel.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Start";
            // 
            // RangePanel
            // 
            this.RangePanel.Controls.Add(this.label6);
            this.RangePanel.Controls.Add(this.EndTimeDateTimePicker);
            this.RangePanel.Controls.Add(this.EndDateTimePicker);
            this.RangePanel.Enabled = false;
            this.RangePanel.Location = new System.Drawing.Point(4, 72);
            this.RangePanel.Name = "RangePanel";
            this.RangePanel.Size = new System.Drawing.Size(327, 28);
            this.RangePanel.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "End";
            // 
            // EndTimeDateTimePicker
            // 
            this.EndTimeDateTimePicker.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.EndTimeDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.EndTimeDateTimePicker.Location = new System.Drawing.Point(170, 4);
            this.EndTimeDateTimePicker.Name = "EndTimeDateTimePicker";
            this.EndTimeDateTimePicker.ShowUpDown = true;
            this.EndTimeDateTimePicker.Size = new System.Drawing.Size(92, 20);
            this.EndTimeDateTimePicker.TabIndex = 4;
            // 
            // EndDateTimePicker
            // 
            this.EndDateTimePicker.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.EndDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.EndDateTimePicker.Location = new System.Drawing.Point(52, 4);
            this.EndDateTimePicker.Name = "EndDateTimePicker";
            this.EndDateTimePicker.Size = new System.Drawing.Size(110, 20);
            this.EndDateTimePicker.TabIndex = 3;
            // 
            // RangeCheckBox
            // 
            this.RangeCheckBox.AutoSize = true;
            this.RangeCheckBox.Location = new System.Drawing.Point(272, 52);
            this.RangeCheckBox.Margin = new System.Windows.Forms.Padding(1);
            this.RangeCheckBox.Name = "RangeCheckBox";
            this.RangeCheckBox.Size = new System.Drawing.Size(58, 17);
            this.RangeCheckBox.TabIndex = 3;
            this.RangeCheckBox.Text = "Range";
            this.RangeCheckBox.UseVisualStyleBackColor = true;
            this.RangeCheckBox.CheckedChanged += new System.EventHandler(this.RangeCheckBox_CheckedChanged);
            // 
            // StartTimeDateTimePicker
            // 
            this.StartTimeDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.StartTimeDateTimePicker.Location = new System.Drawing.Point(174, 50);
            this.StartTimeDateTimePicker.Name = "StartTimeDateTimePicker";
            this.StartTimeDateTimePicker.ShowUpDown = true;
            this.StartTimeDateTimePicker.Size = new System.Drawing.Size(92, 20);
            this.StartTimeDateTimePicker.TabIndex = 2;
            // 
            // StartDateTimePicker
            // 
            this.StartDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.StartDateTimePicker.Location = new System.Drawing.Point(56, 50);
            this.StartDateTimePicker.Name = "StartDateTimePicker";
            this.StartDateTimePicker.Size = new System.Drawing.Size(110, 20);
            this.StartDateTimePicker.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ResultsDataGridView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer2.Panel2.Controls.Add(this.SummarySyntaxRichTextBox);
            this.splitContainer2.Size = new System.Drawing.Size(352, 457);
            this.splitContainer2.SplitterDistance = 300;
            this.splitContainer2.TabIndex = 13;
            // 
            // ResultsDataGridView
            // 
            this.ResultsDataGridView.AllowUserToAddRows = false;
            this.ResultsDataGridView.AllowUserToDeleteRows = false;
            this.ResultsDataGridView.AllowUserToOrderColumns = true;
            this.ResultsDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ButtonFace;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ResultsDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ResultsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ResultsDataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.ResultsDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ResultsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ResultsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.ResultsDataGridView.Name = "ResultsDataGridView";
            this.ResultsDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ResultsDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.ResultsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ResultsDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.ResultsDataGridView.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ResultsDataGridView.RowTemplate.ReadOnly = true;
            this.ResultsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ResultsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ResultsDataGridView.Size = new System.Drawing.Size(352, 300);
            this.ResultsDataGridView.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RunToolStripButton,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(352, 25);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // RunToolStripButton
            // 
            this.RunToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RunToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("RunToolStripButton.Image")));
            this.RunToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RunToolStripButton.Name = "RunToolStripButton";
            this.RunToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.RunToolStripButton.Text = "toolStripButton2";
            this.RunToolStripButton.Click += new System.EventHandler(this.RunToolStripButton_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // SummarySyntaxRichTextBox
            // 
            this.SummarySyntaxRichTextBox.AcceptsTab = true;
            this.SummarySyntaxRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SummarySyntaxRichTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.SummarySyntaxRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SummarySyntaxRichTextBox.ContextMenuStrip = this.KeyWordsContextMenuStrip;
            this.SummarySyntaxRichTextBox.Location = new System.Drawing.Point(0, 28);
            this.SummarySyntaxRichTextBox.Name = "SummarySyntaxRichTextBox";
            this.SummarySyntaxRichTextBox.Size = new System.Drawing.Size(352, 125);
            this.SummarySyntaxRichTextBox.TabIndex = 12;
            this.SummarySyntaxRichTextBox.Text = "";
            this.SummarySyntaxRichTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SummarySyntaxRichTextBox_KeyDown);
            this.SummarySyntaxRichTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SummarySyntaxRichTextBox_KeyPress);
            // 
            // KeyWordsContextMenuStrip
            // 
            this.KeyWordsContextMenuStrip.Name = "KeyWordsContextMenuStrip";
            this.KeyWordsContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            this.KeyWordsContextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.KeyWordsContextMenuStrip_ItemClicked);
            // 
            // LogPathFolderBrowserDialog
            // 
            this.LogPathFolderBrowserDialog.ShowNewFolderButton = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Enter Logs location path";
            // 
            // IISLogViewUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "IISLogViewUserControl";
            this.Size = new System.Drawing.Size(694, 457);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.RangePanel.ResumeLayout(false);
            this.RangePanel.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ResultsDataGridView)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel RangePanel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker EndTimeDateTimePicker;
        private System.Windows.Forms.DateTimePicker EndDateTimePicker;
        private System.Windows.Forms.CheckBox RangeCheckBox;
        private System.Windows.Forms.DateTimePicker StartTimeDateTimePicker;
        private System.Windows.Forms.DateTimePicker StartDateTimePicker;
        private System.Windows.Forms.DataGridView ResultsDataGridView;
        private System.Windows.Forms.TableLayoutPanel ReportsTableLayoutPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button FilePathButton;
        private System.Windows.Forms.TextBox FilePathTextBox;
        private System.Windows.Forms.FolderBrowserDialog LogPathFolderBrowserDialog;
        private SyntaxHighlighter.SyntaxRichTextBox SummarySyntaxRichTextBox;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ContextMenuStrip KeyWordsContextMenuStrip;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton RunToolStripButton;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Label label2;

    }
}
