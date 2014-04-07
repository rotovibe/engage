namespace ProgramBuilder
{
    partial class ModuleListForm
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
            this.moduleListView = new System.Windows.Forms.ListView();
            this.addModuleButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // moduleListView
            // 
            this.moduleListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.moduleListView.CheckBoxes = true;
            this.moduleListView.Location = new System.Drawing.Point(12, 12);
            this.moduleListView.Name = "moduleListView";
            this.moduleListView.Size = new System.Drawing.Size(427, 364);
            this.moduleListView.TabIndex = 0;
            this.moduleListView.UseCompatibleStateImageBehavior = false;
            this.moduleListView.View = System.Windows.Forms.View.List;
            this.moduleListView.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // addModuleButton
            // 
            this.addModuleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addModuleButton.Location = new System.Drawing.Point(12, 399);
            this.addModuleButton.Name = "addModuleButton";
            this.addModuleButton.Size = new System.Drawing.Size(75, 23);
            this.addModuleButton.TabIndex = 1;
            this.addModuleButton.Text = "Add Module";
            this.addModuleButton.UseVisualStyleBackColor = true;
            this.addModuleButton.Click += new System.EventHandler(this.addModuleButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.closeButton.Location = new System.Drawing.Point(142, 399);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.addButton.Location = new System.Drawing.Point(223, 399);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // ModuleListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 446);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.addModuleButton);
            this.Controls.Add(this.moduleListView);
            this.Location = new System.Drawing.Point(5, 5);
            this.MinimumSize = new System.Drawing.Size(467, 484);
            this.Name = "ModuleListForm";
            this.Text = "Module List";
            this.Load += new System.EventHandler(this.ModuleListForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addModuleButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button addButton;
        public System.Windows.Forms.ListView moduleListView;
    }
}