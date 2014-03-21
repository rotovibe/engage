namespace ProgramBuilder
{
    partial class ProgramForm
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
            this.components = new System.ComponentModel.Container();
            this.NewProgramButton = new System.Windows.Forms.Button();
            this.programPanel = new System.Windows.Forms.Panel();
            this.edTextBox = new System.Windows.Forms.TextBox();
            this.edLabel = new System.Windows.Forms.Label();
            this.sdTextBox = new System.Windows.Forms.TextBox();
            this.sdLabel = new System.Windows.Forms.Label();
            this.descTextBox = new System.Windows.Forms.TextBox();
            this.descLabel = new System.Windows.Forms.Label();
            this.cliTextBox = new System.Windows.Forms.TextBox();
            this.cliLabel = new System.Windows.Forms.Label();
            this.athbyTextBox = new System.Windows.Forms.TextBox();
            this.athbyLabel = new System.Windows.Forms.Label();
            this.ProgramTree = new System.Windows.Forms.TreeView();
            this.mnuNewModule = new System.Windows.Forms.ToolStripMenuItem();
            this.programContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.moduleContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.actionContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.stepContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.modulePanel = new System.Windows.Forms.Panel();
            this.cbyLabel = new System.Windows.Forms.Label();
            this.actionPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.stepPanel = new System.Windows.Forms.Panel();
            this.programPanel.SuspendLayout();
            this.programContextMenuStrip.SuspendLayout();
            this.moduleContextMenuStrip.SuspendLayout();
            this.actionContextMenuStrip.SuspendLayout();
            this.stepContextMenuStrip.SuspendLayout();
            this.modulePanel.SuspendLayout();
            this.actionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // NewProgramButton
            // 
            this.NewProgramButton.Location = new System.Drawing.Point(12, 12);
            this.NewProgramButton.Name = "NewProgramButton";
            this.NewProgramButton.Size = new System.Drawing.Size(90, 23);
            this.NewProgramButton.TabIndex = 0;
            this.NewProgramButton.Text = "New Program";
            this.NewProgramButton.UseVisualStyleBackColor = true;
            this.NewProgramButton.Click += new System.EventHandler(this.NewProgramButton_Click);
            // 
            // programPanel
            // 
            this.programPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.programPanel.Controls.Add(this.edTextBox);
            this.programPanel.Controls.Add(this.edLabel);
            this.programPanel.Controls.Add(this.sdTextBox);
            this.programPanel.Controls.Add(this.sdLabel);
            this.programPanel.Controls.Add(this.descTextBox);
            this.programPanel.Controls.Add(this.descLabel);
            this.programPanel.Controls.Add(this.cliTextBox);
            this.programPanel.Controls.Add(this.cliLabel);
            this.programPanel.Controls.Add(this.athbyTextBox);
            this.programPanel.Controls.Add(this.athbyLabel);
            this.programPanel.Location = new System.Drawing.Point(166, 41);
            this.programPanel.Name = "programPanel";
            this.programPanel.Size = new System.Drawing.Size(451, 432);
            this.programPanel.TabIndex = 1;
            this.programPanel.Visible = false;
            // 
            // edTextBox
            // 
            this.edTextBox.Location = new System.Drawing.Point(26, 239);
            this.edTextBox.Name = "edTextBox";
            this.edTextBox.Size = new System.Drawing.Size(100, 20);
            this.edTextBox.TabIndex = 9;
            // 
            // edLabel
            // 
            this.edLabel.AutoSize = true;
            this.edLabel.Location = new System.Drawing.Point(23, 223);
            this.edLabel.Name = "edLabel";
            this.edLabel.Size = new System.Drawing.Size(55, 13);
            this.edLabel.TabIndex = 8;
            this.edLabel.Text = "End Date:";
            // 
            // sdTextBox
            // 
            this.sdTextBox.Location = new System.Drawing.Point(26, 186);
            this.sdTextBox.Name = "sdTextBox";
            this.sdTextBox.Size = new System.Drawing.Size(100, 20);
            this.sdTextBox.TabIndex = 7;
            // 
            // sdLabel
            // 
            this.sdLabel.AutoSize = true;
            this.sdLabel.Location = new System.Drawing.Point(23, 170);
            this.sdLabel.Name = "sdLabel";
            this.sdLabel.Size = new System.Drawing.Size(58, 13);
            this.sdLabel.TabIndex = 6;
            this.sdLabel.Text = "Start Date:";
            // 
            // descTextBox
            // 
            this.descTextBox.Location = new System.Drawing.Point(26, 135);
            this.descTextBox.Name = "descTextBox";
            this.descTextBox.Size = new System.Drawing.Size(100, 20);
            this.descTextBox.TabIndex = 5;
            // 
            // descLabel
            // 
            this.descLabel.AutoSize = true;
            this.descLabel.Location = new System.Drawing.Point(23, 119);
            this.descLabel.Name = "descLabel";
            this.descLabel.Size = new System.Drawing.Size(63, 13);
            this.descLabel.TabIndex = 4;
            this.descLabel.Text = "Description:";
            // 
            // cliTextBox
            // 
            this.cliTextBox.Location = new System.Drawing.Point(26, 82);
            this.cliTextBox.Name = "cliTextBox";
            this.cliTextBox.Size = new System.Drawing.Size(100, 20);
            this.cliTextBox.TabIndex = 3;
            // 
            // cliLabel
            // 
            this.cliLabel.AutoSize = true;
            this.cliLabel.Location = new System.Drawing.Point(23, 66);
            this.cliLabel.Name = "cliLabel";
            this.cliLabel.Size = new System.Drawing.Size(36, 13);
            this.cliLabel.TabIndex = 2;
            this.cliLabel.Text = "Client:";
            // 
            // athbyTextBox
            // 
            this.athbyTextBox.Location = new System.Drawing.Point(26, 29);
            this.athbyTextBox.Name = "athbyTextBox";
            this.athbyTextBox.Size = new System.Drawing.Size(100, 20);
            this.athbyTextBox.TabIndex = 1;
            // 
            // athbyLabel
            // 
            this.athbyLabel.AutoSize = true;
            this.athbyLabel.Location = new System.Drawing.Point(23, 13);
            this.athbyLabel.Name = "athbyLabel";
            this.athbyLabel.Size = new System.Drawing.Size(68, 13);
            this.athbyLabel.TabIndex = 0;
            this.athbyLabel.Text = "Authored By:";
            // 
            // ProgramTree
            // 
            this.ProgramTree.Location = new System.Drawing.Point(12, 41);
            this.ProgramTree.Name = "ProgramTree";
            this.ProgramTree.Size = new System.Drawing.Size(137, 432);
            this.ProgramTree.TabIndex = 2;
            this.ProgramTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ProgramTree_AfterSelect);
            this.ProgramTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ProgramTree_MouseUp);
            // 
            // mnuNewModule
            // 
            this.mnuNewModule.Name = "mnuNewModule";
            this.mnuNewModule.Size = new System.Drawing.Size(151, 22);
            this.mnuNewModule.Text = "New Module...";
            this.mnuNewModule.Click += new System.EventHandler(this.mnuNewModule_Click);
            // 
            // programContextMenuStrip
            // 
            this.programContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewModule,
            this.deleteToolStripMenuItem});
            this.programContextMenuStrip.Name = "contextMenuStrip1";
            this.programContextMenuStrip.Size = new System.Drawing.Size(152, 48);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(542, 481);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(461, 481);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // moduleContextMenuStrip
            // 
            this.moduleContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addActionToolStripMenuItem,
            this.deleteToolStripMenuItem1});
            this.moduleContextMenuStrip.Name = "moduleContextMenuStrip";
            this.moduleContextMenuStrip.Size = new System.Drawing.Size(144, 48);
            // 
            // addActionToolStripMenuItem
            // 
            this.addActionToolStripMenuItem.Name = "addActionToolStripMenuItem";
            this.addActionToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.addActionToolStripMenuItem.Text = "Add Action...";
            this.addActionToolStripMenuItem.Click += new System.EventHandler(this.addActionToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(143, 22);
            this.deleteToolStripMenuItem1.Text = "Delete";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem1_Click);
            // 
            // actionContextMenuStrip
            // 
            this.actionContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newStepToolStripMenuItem,
            this.deleteToolStripMenuItem2});
            this.actionContextMenuStrip.Name = "actionContextMenuStrip";
            this.actionContextMenuStrip.Size = new System.Drawing.Size(134, 48);
            // 
            // newStepToolStripMenuItem
            // 
            this.newStepToolStripMenuItem.Name = "newStepToolStripMenuItem";
            this.newStepToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.newStepToolStripMenuItem.Text = "New Step...";
            // 
            // deleteToolStripMenuItem2
            // 
            this.deleteToolStripMenuItem2.Name = "deleteToolStripMenuItem2";
            this.deleteToolStripMenuItem2.Size = new System.Drawing.Size(133, 22);
            this.deleteToolStripMenuItem2.Text = "Delete";
            this.deleteToolStripMenuItem2.Click += new System.EventHandler(this.deleteToolStripMenuItem2_Click);
            // 
            // stepContextMenuStrip
            // 
            this.stepContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem3});
            this.stepContextMenuStrip.Name = "stepContextMenuStrip";
            this.stepContextMenuStrip.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteToolStripMenuItem3
            // 
            this.deleteToolStripMenuItem3.Name = "deleteToolStripMenuItem3";
            this.deleteToolStripMenuItem3.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem3.Text = "Delete";
            this.deleteToolStripMenuItem3.Click += new System.EventHandler(this.deleteToolStripMenuItem3_Click);
            // 
            // modulePanel
            // 
            this.modulePanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.modulePanel.Controls.Add(this.cbyLabel);
            this.modulePanel.Location = new System.Drawing.Point(166, 41);
            this.modulePanel.Name = "modulePanel";
            this.modulePanel.Size = new System.Drawing.Size(451, 432);
            this.modulePanel.TabIndex = 5;
            this.modulePanel.Visible = false;
            // 
            // cbyLabel
            // 
            this.cbyLabel.AutoSize = true;
            this.cbyLabel.Location = new System.Drawing.Point(26, 13);
            this.cbyLabel.Name = "cbyLabel";
            this.cbyLabel.Size = new System.Drawing.Size(62, 13);
            this.cbyLabel.TabIndex = 0;
            this.cbyLabel.Text = "Created By;";
            // 
            // actionPanel
            // 
            this.actionPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.actionPanel.Controls.Add(this.label1);
            this.actionPanel.Location = new System.Drawing.Point(166, 41);
            this.actionPanel.Name = "actionPanel";
            this.actionPanel.Size = new System.Drawing.Size(451, 432);
            this.actionPanel.TabIndex = 6;
            this.actionPanel.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Action Panel";
            // 
            // stepPanel
            // 
            this.stepPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.stepPanel.Location = new System.Drawing.Point(166, 41);
            this.stepPanel.Name = "stepPanel";
            this.stepPanel.Size = new System.Drawing.Size(451, 432);
            this.stepPanel.TabIndex = 7;
            this.stepPanel.Visible = false;
            // 
            // ProgramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 513);
            this.Controls.Add(this.stepPanel);
            this.Controls.Add(this.actionPanel);
            this.Controls.Add(this.modulePanel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.ProgramTree);
            this.Controls.Add(this.programPanel);
            this.Controls.Add(this.NewProgramButton);
            this.Name = "ProgramForm";
            this.Text = "Program";
            this.programPanel.ResumeLayout(false);
            this.programPanel.PerformLayout();
            this.programContextMenuStrip.ResumeLayout(false);
            this.moduleContextMenuStrip.ResumeLayout(false);
            this.actionContextMenuStrip.ResumeLayout(false);
            this.stepContextMenuStrip.ResumeLayout(false);
            this.modulePanel.ResumeLayout(false);
            this.modulePanel.PerformLayout();
            this.actionPanel.ResumeLayout(false);
            this.actionPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button NewProgramButton;
        private System.Windows.Forms.Panel programPanel;
        private System.Windows.Forms.TreeView ProgramTree;
        private System.Windows.Forms.ToolStripMenuItem mnuNewModule;
        private System.Windows.Forms.ContextMenuStrip programContextMenuStrip;
        private System.Windows.Forms.TextBox edTextBox;
        private System.Windows.Forms.Label edLabel;
        private System.Windows.Forms.TextBox sdTextBox;
        private System.Windows.Forms.Label sdLabel;
        private System.Windows.Forms.TextBox descTextBox;
        private System.Windows.Forms.Label descLabel;
        private System.Windows.Forms.TextBox cliTextBox;
        private System.Windows.Forms.Label cliLabel;
        private System.Windows.Forms.TextBox athbyTextBox;
        private System.Windows.Forms.Label athbyLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ContextMenuStrip moduleContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addActionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip actionContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem newStepToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem2;
        private System.Windows.Forms.ContextMenuStrip stepContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem3;
        private System.Windows.Forms.Panel modulePanel;
        private System.Windows.Forms.Label cbyLabel;
        private System.Windows.Forms.Panel actionPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel stepPanel;
    }
}

