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
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.ProgramTree = new System.Windows.Forms.TreeView();
            this.mnuNewModule = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
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
            // panel1
            // 
            this.panel1.Controls.Add(this.treeView1);
            this.panel1.Location = new System.Drawing.Point(166, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(278, 350);
            this.panel1.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(0, 4);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(275, 346);
            this.treeView1.TabIndex = 0;
            // 
            // ProgramTree
            // 
            this.ProgramTree.Location = new System.Drawing.Point(12, 41);
            this.ProgramTree.Name = "ProgramTree";
            this.ProgramTree.Size = new System.Drawing.Size(137, 346);
            this.ProgramTree.TabIndex = 2;
            this.ProgramTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ProgramTree_MouseUp);
            // 
            // mnuNewModule
            // 
            this.mnuNewModule.Name = "mnuNewModule";
            this.mnuNewModule.Size = new System.Drawing.Size(152, 22);
            this.mnuNewModule.Text = "New Module...";
            this.mnuNewModule.Click += new System.EventHandler(this.mnuNewModule_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewModule});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
            // 
            // ProgramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 399);
            this.Controls.Add(this.ProgramTree);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.NewProgramButton);
            this.Name = "ProgramForm";
            this.Text = "Program";
            this.panel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button NewProgramButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TreeView ProgramTree;
        private System.Windows.Forms.ToolStripMenuItem mnuNewModule;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}

