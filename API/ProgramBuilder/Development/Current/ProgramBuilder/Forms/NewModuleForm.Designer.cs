namespace ProgramBuilder
{
    partial class NewModuleForm
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
            this.closeButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.nameLabel = new System.Windows.Forms.Label();
            this.descLabel = new System.Windows.Forms.Label();
            this.stsLabel = new System.Windows.Forms.Label();
            this.nmTextBox = new System.Windows.Forms.TextBox();
            this.descTextBox = new System.Windows.Forms.TextBox();
            this.stsTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(62, 234);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(143, 234);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(30, 30);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.Text = "Name:";
            // 
            // descLabel
            // 
            this.descLabel.AutoSize = true;
            this.descLabel.Location = new System.Drawing.Point(30, 85);
            this.descLabel.Name = "descLabel";
            this.descLabel.Size = new System.Drawing.Size(63, 13);
            this.descLabel.TabIndex = 3;
            this.descLabel.Text = "Description:";
            // 
            // stsLabel
            // 
            this.stsLabel.AutoSize = true;
            this.stsLabel.Location = new System.Drawing.Point(30, 140);
            this.stsLabel.Name = "stsLabel";
            this.stsLabel.Size = new System.Drawing.Size(40, 13);
            this.stsLabel.TabIndex = 4;
            this.stsLabel.Text = "Status:";
            // 
            // nmTextBox
            // 
            this.nmTextBox.Location = new System.Drawing.Point(32, 45);
            this.nmTextBox.Name = "nmTextBox";
            this.nmTextBox.Size = new System.Drawing.Size(100, 20);
            this.nmTextBox.TabIndex = 5;
            // 
            // descTextBox
            // 
            this.descTextBox.Location = new System.Drawing.Point(32, 100);
            this.descTextBox.Name = "descTextBox";
            this.descTextBox.Size = new System.Drawing.Size(100, 20);
            this.descTextBox.TabIndex = 6;
            // 
            // stsTextBox
            // 
            this.stsTextBox.Location = new System.Drawing.Point(32, 155);
            this.stsTextBox.Name = "stsTextBox";
            this.stsTextBox.Size = new System.Drawing.Size(100, 20);
            this.stsTextBox.TabIndex = 7;
            // 
            // NewModuleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.stsTextBox);
            this.Controls.Add(this.descTextBox);
            this.Controls.Add(this.nmTextBox);
            this.Controls.Add(this.stsLabel);
            this.Controls.Add(this.descLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.closeButton);
            this.Name = "NewModuleForm";
            this.Text = "New Module";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label descLabel;
        private System.Windows.Forms.Label stsLabel;
        private System.Windows.Forms.TextBox nmTextBox;
        private System.Windows.Forms.TextBox descTextBox;
        private System.Windows.Forms.TextBox stsTextBox;
    }
}