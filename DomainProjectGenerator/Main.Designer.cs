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
            this.SuspendLayout();
            // 
            // WorkingDirectoryTextBox
            // 
            this.WorkingDirectoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WorkingDirectoryTextBox.Location = new System.Drawing.Point(6, 41);
            this.WorkingDirectoryTextBox.Name = "WorkingDirectoryTextBox";
            this.WorkingDirectoryTextBox.Size = new System.Drawing.Size(485, 20);
            this.WorkingDirectoryTextBox.TabIndex = 0;
            // 
            // OpenDirectoryBrowserBtn
            // 
            this.OpenDirectoryBrowserBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenDirectoryBrowserBtn.Location = new System.Drawing.Point(496, 39);
            this.OpenDirectoryBrowserBtn.Name = "OpenDirectoryBrowserBtn";
            this.OpenDirectoryBrowserBtn.Size = new System.Drawing.Size(31, 23);
            this.OpenDirectoryBrowserBtn.TabIndex = 1;
            this.OpenDirectoryBrowserBtn.Text = "...";
            this.OpenDirectoryBrowserBtn.UseVisualStyleBackColor = true;
            this.OpenDirectoryBrowserBtn.Click += new System.EventHandler(this.OpenDirectoryBrowserBtn_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Working Directory (required)";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Target Directory (required)";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(496, 86);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(31, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TargetFolderPath
            // 
            this.TargetFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TargetFolderPath.Location = new System.Drawing.Point(6, 88);
            this.TargetFolderPath.Name = "TargetFolderPath";
            this.TargetFolderPath.Size = new System.Drawing.Size(485, 20);
            this.TargetFolderPath.TabIndex = 3;
            // 
            // EntityNameTxtBx
            // 
            this.EntityNameTxtBx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EntityNameTxtBx.Location = new System.Drawing.Point(9, 134);
            this.EntityNameTxtBx.Name = "EntityNameTxtBx";
            this.EntityNameTxtBx.Size = new System.Drawing.Size(518, 20);
            this.EntityNameTxtBx.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Entity Name (required)";
            // 
            // GenerateButton
            // 
            this.GenerateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GenerateButton.Location = new System.Drawing.Point(439, 160);
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
            this.ResultsTextBox.Location = new System.Drawing.Point(9, 191);
            this.ResultsTextBox.Multiline = true;
            this.ResultsTextBox.Name = "ResultsTextBox";
            this.ResultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ResultsTextBox.Size = new System.Drawing.Size(518, 159);
            this.ResultsTextBox.TabIndex = 9;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 362);
            this.Controls.Add(this.ResultsTextBox);
            this.Controls.Add(this.GenerateButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.EntityNameTxtBx);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TargetFolderPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OpenDirectoryBrowserBtn);
            this.Controls.Add(this.WorkingDirectoryTextBox);
            this.Name = "Main";
            this.Text = "DataDomain Projects Generator";
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
    }
}

