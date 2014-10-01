namespace PhytelServicesManager
{
    partial class frmNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNew));
            this.label1 = new System.Windows.Forms.Label();
            this.txtConnectionName = new System.Windows.Forms.TextBox();
            this.rdoSQL = new System.Windows.Forms.RadioButton();
            this.rdoMongo = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection Name:";
            // 
            // txtConnectionName
            // 
            this.txtConnectionName.Location = new System.Drawing.Point(151, 15);
            this.txtConnectionName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtConnectionName.Name = "txtConnectionName";
            this.txtConnectionName.Size = new System.Drawing.Size(267, 22);
            this.txtConnectionName.TabIndex = 1;
            // 
            // rdoSQL
            // 
            this.rdoSQL.AutoSize = true;
            this.rdoSQL.Checked = true;
            this.rdoSQL.Location = new System.Drawing.Point(151, 47);
            this.rdoSQL.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoSQL.Name = "rdoSQL";
            this.rdoSQL.Size = new System.Drawing.Size(103, 21);
            this.rdoSQL.TabIndex = 2;
            this.rdoSQL.TabStop = true;
            this.rdoSQL.Text = "SQL Server";
            this.rdoSQL.UseVisualStyleBackColor = true;
            // 
            // rdoMongo
            // 
            this.rdoMongo.AutoSize = true;
            this.rdoMongo.Location = new System.Drawing.Point(308, 47);
            this.rdoMongo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoMongo.Name = "rdoMongo";
            this.rdoMongo.Size = new System.Drawing.Size(95, 21);
            this.rdoMongo.TabIndex = 3;
            this.rdoMongo.Text = "Mongo DB";
            this.rdoMongo.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(319, 79);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(211, 79);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 28);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 122);
            this.ControlBox = false;
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.rdoMongo);
            this.Controls.Add(this.rdoSQL);
            this.Controls.Add(this.txtConnectionName);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximumSize = new System.Drawing.Size(449, 167);
            this.Name = "frmNew";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Phytel Database";
            this.Load += new System.EventHandler(this.frmNew_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtConnectionName;
        private System.Windows.Forms.RadioButton rdoSQL;
        private System.Windows.Forms.RadioButton rdoMongo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}