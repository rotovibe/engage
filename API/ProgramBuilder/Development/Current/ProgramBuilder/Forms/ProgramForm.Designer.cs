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
            this.mnuNewModule = new System.Windows.Forms.ToolStripMenuItem();
            this.programContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuDeleteProgram = new System.Windows.Forms.ToolStripMenuItem();
            this.moduleContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuNewAction = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteModule = new System.Windows.Forms.ToolStripMenuItem();
            this.actionContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuNewStep = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteAction = new System.Windows.Forms.ToolStripMenuItem();
            this.stepContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuDeleteStep = new System.Windows.Forms.ToolStripMenuItem();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.ProgramTree = new System.Windows.Forms.TreeView();
            this.NewProgramButton = new System.Windows.Forms.Button();
            this.programContextMenuStrip.SuspendLayout();
            this.moduleContextMenuStrip.SuspendLayout();
            this.actionContextMenuStrip.SuspendLayout();
            this.stepContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
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
            this.mnuDeleteProgram});
            this.programContextMenuStrip.Name = "contextMenuStrip1";
            this.programContextMenuStrip.Size = new System.Drawing.Size(152, 48);
            // 
            // mnuDeleteProgram
            // 
            this.mnuDeleteProgram.Name = "mnuDeleteProgram";
            this.mnuDeleteProgram.Size = new System.Drawing.Size(151, 22);
            this.mnuDeleteProgram.Text = "Delete";
            this.mnuDeleteProgram.Click += new System.EventHandler(this.mnuDeleteProgram_Click);
            // 
            // moduleContextMenuStrip
            // 
            this.moduleContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewAction,
            this.mnuDeleteModule});
            this.moduleContextMenuStrip.Name = "moduleContextMenuStrip";
            this.moduleContextMenuStrip.Size = new System.Drawing.Size(146, 48);
            // 
            // mnuNewAction
            // 
            this.mnuNewAction.Name = "mnuNewAction";
            this.mnuNewAction.Size = new System.Drawing.Size(145, 22);
            this.mnuNewAction.Text = "New Action...";
            this.mnuNewAction.Click += new System.EventHandler(this.mnuNewAction_Click);
            // 
            // mnuDeleteModule
            // 
            this.mnuDeleteModule.Name = "mnuDeleteModule";
            this.mnuDeleteModule.Size = new System.Drawing.Size(145, 22);
            this.mnuDeleteModule.Text = "Delete";
            this.mnuDeleteModule.Click += new System.EventHandler(this.mnuDeleteModule_Click);
            // 
            // actionContextMenuStrip
            // 
            this.actionContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewStep,
            this.mnuDeleteAction});
            this.actionContextMenuStrip.Name = "actionContextMenuStrip";
            this.actionContextMenuStrip.Size = new System.Drawing.Size(134, 48);
            // 
            // mnuNewStep
            // 
            this.mnuNewStep.Name = "mnuNewStep";
            this.mnuNewStep.Size = new System.Drawing.Size(133, 22);
            this.mnuNewStep.Text = "New Step...";
            this.mnuNewStep.Click += new System.EventHandler(this.mnuNewStep_Click);
            // 
            // mnuDeleteAction
            // 
            this.mnuDeleteAction.Name = "mnuDeleteAction";
            this.mnuDeleteAction.Size = new System.Drawing.Size(133, 22);
            this.mnuDeleteAction.Text = "Delete";
            this.mnuDeleteAction.Click += new System.EventHandler(this.mnuDeleteAction_Click);
            // 
            // stepContextMenuStrip
            // 
            this.stepContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDeleteStep});
            this.stepContextMenuStrip.Name = "stepContextMenuStrip";
            this.stepContextMenuStrip.Size = new System.Drawing.Size(108, 26);
            // 
            // mnuDeleteStep
            // 
            this.mnuDeleteStep.Name = "mnuDeleteStep";
            this.mnuDeleteStep.Size = new System.Drawing.Size(107, 22);
            this.mnuDeleteStep.Text = "Delete";
            this.mnuDeleteStep.Click += new System.EventHandler(this.mnuDeleteStep_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.Location = new System.Drawing.Point(198, 41);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(458, 432);
            this.mainPanel.TabIndex = 8;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(500, 481);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(581, 481);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // ProgramTree
            // 
            this.ProgramTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgramTree.Location = new System.Drawing.Point(12, 41);
            this.ProgramTree.Name = "ProgramTree";
            this.ProgramTree.ShowLines = false;
            this.ProgramTree.Size = new System.Drawing.Size(169, 432);
            this.ProgramTree.TabIndex = 2;
            this.ProgramTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ProgramTree_AfterSelect);
            this.ProgramTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ProgramTree_MouseUp);
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
            // ProgramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 513);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.ProgramTree);
            this.Controls.Add(this.NewProgramButton);
            this.Location = new System.Drawing.Point(30, 30);
            this.MinimumSize = new System.Drawing.Size(682, 551);
            this.Name = "ProgramForm";
            this.Text = "Program";
            this.programContextMenuStrip.ResumeLayout(false);
            this.moduleContextMenuStrip.ResumeLayout(false);
            this.actionContextMenuStrip.ResumeLayout(false);
            this.stepContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button NewProgramButton;
        private System.Windows.Forms.TreeView ProgramTree;
        private System.Windows.Forms.ToolStripMenuItem mnuNewModule;
        private System.Windows.Forms.ContextMenuStrip programContextMenuStrip;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ContextMenuStrip moduleContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuNewAction;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteProgram;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteModule;
        private System.Windows.Forms.ContextMenuStrip actionContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuNewStep;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteAction;
        private System.Windows.Forms.ContextMenuStrip stepContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteStep;
        private System.Windows.Forms.Panel mainPanel;
    }
}

