namespace DomainProjectGenerator
{
    partial class Main
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.WorkingDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.OpenDirectoryBrowserBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.TargetFolderPath = new System.Windows.Forms.TextBox();
            this.EntityNameTxtBx = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.ResultsTextBox = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.IISLogViewerTabPage = new System.Windows.Forms.TabPage();
            this.DDGeneratorTabPage = new System.Windows.Forms.TabPage();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.tabControl1.SuspendLayout();
            this.DDGeneratorTabPage.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // WorkingDirectoryTextBox
            // 
            this.WorkingDirectoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WorkingDirectoryTextBox.Location = new System.Drawing.Point(19, 31);
            this.WorkingDirectoryTextBox.Name = "WorkingDirectoryTextBox";
            this.WorkingDirectoryTextBox.Size = new System.Drawing.Size(729, 20);
            this.WorkingDirectoryTextBox.TabIndex = 0;
            // 
            // OpenDirectoryBrowserBtn
            // 
            this.OpenDirectoryBrowserBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenDirectoryBrowserBtn.Location = new System.Drawing.Point(754, 29);
            this.OpenDirectoryBrowserBtn.Name = "OpenDirectoryBrowserBtn";
            this.OpenDirectoryBrowserBtn.Size = new System.Drawing.Size(31, 23);
            this.OpenDirectoryBrowserBtn.TabIndex = 1;
            this.OpenDirectoryBrowserBtn.Text = "...";
            this.OpenDirectoryBrowserBtn.UseVisualStyleBackColor = true;
            this.OpenDirectoryBrowserBtn.Click += new System.EventHandler(this.OpenDirectoryBrowserBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Working Directory (required)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Target Directory (required)";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(754, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(31, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TargetFolderPath
            // 
            this.TargetFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TargetFolderPath.Location = new System.Drawing.Point(19, 78);
            this.TargetFolderPath.Name = "TargetFolderPath";
            this.TargetFolderPath.Size = new System.Drawing.Size(729, 20);
            this.TargetFolderPath.TabIndex = 3;
            // 
            // EntityNameTxtBx
            // 
            this.EntityNameTxtBx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EntityNameTxtBx.Location = new System.Drawing.Point(22, 124);
            this.EntityNameTxtBx.Name = "EntityNameTxtBx";
            this.EntityNameTxtBx.Size = new System.Drawing.Size(763, 20);
            this.EntityNameTxtBx.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Entity Name (required)";
            // 
            // GenerateButton
            // 
            this.GenerateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GenerateButton.Location = new System.Drawing.Point(697, 150);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(88, 23);
            this.GenerateButton.TabIndex = 8;
            this.GenerateButton.Text = "Generate";
            this.GenerateButton.UseVisualStyleBackColor = true;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // ResultsTextBox
            // 
            this.ResultsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsTextBox.Location = new System.Drawing.Point(22, 181);
            this.ResultsTextBox.Multiline = true;
            this.ResultsTextBox.Name = "ResultsTextBox";
            this.ResultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ResultsTextBox.Size = new System.Drawing.Size(763, 299);
            this.ResultsTextBox.TabIndex = 9;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.IISLogViewerTabPage);
            this.tabControl1.Controls.Add(this.DDGeneratorTabPage);
            this.tabControl1.Location = new System.Drawing.Point(4, 34);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(833, 528);
            this.tabControl1.TabIndex = 10;
            // 
            // IISLogViewerTabPage
            // 
            this.IISLogViewerTabPage.Location = new System.Drawing.Point(4, 22);
            this.IISLogViewerTabPage.Name = "IISLogViewerTabPage";
            this.IISLogViewerTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.IISLogViewerTabPage.Size = new System.Drawing.Size(825, 502);
            this.IISLogViewerTabPage.TabIndex = 0;
            this.IISLogViewerTabPage.Text = "IIS Log Viewer";
            this.IISLogViewerTabPage.UseVisualStyleBackColor = true;
            // 
            // DDGeneratorTabPage
            // 
            this.DDGeneratorTabPage.Controls.Add(this.WorkingDirectoryTextBox);
            this.DDGeneratorTabPage.Controls.Add(this.ResultsTextBox);
            this.DDGeneratorTabPage.Controls.Add(this.OpenDirectoryBrowserBtn);
            this.DDGeneratorTabPage.Controls.Add(this.GenerateButton);
            this.DDGeneratorTabPage.Controls.Add(this.label1);
            this.DDGeneratorTabPage.Controls.Add(this.label3);
            this.DDGeneratorTabPage.Controls.Add(this.TargetFolderPath);
            this.DDGeneratorTabPage.Controls.Add(this.EntityNameTxtBx);
            this.DDGeneratorTabPage.Controls.Add(this.button1);
            this.DDGeneratorTabPage.Controls.Add(this.label2);
            this.DDGeneratorTabPage.Location = new System.Drawing.Point(4, 22);
            this.DDGeneratorTabPage.Name = "DDGeneratorTabPage";
            this.DDGeneratorTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.DDGeneratorTabPage.Size = new System.Drawing.Size(825, 502);
            this.DDGeneratorTabPage.TabIndex = 1;
            this.DDGeneratorTabPage.Text = "DataDomain Generator";
            this.DDGeneratorTabPage.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Enabled = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(840, 25);
            this.toolStrip1.TabIndex = 11;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.newToolStripButton.Text = "&New";
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "&Open";
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Save";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 565);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabControl1);
            this.Name = "Main";
            this.Text = "Phytel API Utilities";
            this.tabControl1.ResumeLayout(false);
            this.DDGeneratorTabPage.ResumeLayout(false);
            this.DDGeneratorTabPage.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
        private System.Windows.Forms.TextBox WorkingDirectoryTextBox;
        private System.Windows.Forms.Button OpenDirectoryBrowserBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox TargetFolderPath;
        private System.Windows.Forms.TextBox EntityNameTxtBx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.TextBox ResultsTextBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage DDGeneratorTabPage;
        private System.Windows.Forms.TabPage IISLogViewerTabPage;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
    }
}

