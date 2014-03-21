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
            this.SuspendLayout();
            // 
            // moduleListView
            // 
            this.moduleListView.Location = new System.Drawing.Point(12, 12);
            this.moduleListView.Name = "moduleListView";
            this.moduleListView.Size = new System.Drawing.Size(280, 260);
            this.moduleListView.TabIndex = 0;
            this.moduleListView.UseCompatibleStateImageBehavior = false;
            this.moduleListView.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // ModuleListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 328);
            this.Controls.Add(this.moduleListView);
            this.Name = "ModuleListForm";
            this.Text = "Module List";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView moduleListView;
    }
}