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
            this.stsNumericUpDwn = new System.Windows.Forms.NumericUpDown();
            this.stsLabel = new System.Windows.Forms.Label();
            this.descTextBox = new System.Windows.Forms.TextBox();
            this.nmTextBox = new System.Windows.Forms.TextBox();
            this.descLabel = new System.Windows.Forms.Label();
            this.nmLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.stsNumericUpDwn)).BeginInit();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(66, 119);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(147, 119);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // stsNumericUpDwn
            // 
            this.stsNumericUpDwn.Location = new System.Drawing.Point(136, 82);
            this.stsNumericUpDwn.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.stsNumericUpDwn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.stsNumericUpDwn.Name = "stsNumericUpDwn";
            this.stsNumericUpDwn.Size = new System.Drawing.Size(34, 20);
            this.stsNumericUpDwn.TabIndex = 87;
            this.stsNumericUpDwn.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // stsLabel
            // 
            this.stsLabel.AutoSize = true;
            this.stsLabel.Location = new System.Drawing.Point(54, 84);
            this.stsLabel.Name = "stsLabel";
            this.stsLabel.Size = new System.Drawing.Size(40, 13);
            this.stsLabel.TabIndex = 86;
            this.stsLabel.Text = "Status:";
            // 
            // descTextBox
            // 
            this.descTextBox.Location = new System.Drawing.Point(136, 52);
            this.descTextBox.Name = "descTextBox";
            this.descTextBox.Size = new System.Drawing.Size(100, 20);
            this.descTextBox.TabIndex = 82;
            // 
            // nmTextBox
            // 
            this.nmTextBox.Location = new System.Drawing.Point(136, 23);
            this.nmTextBox.Name = "nmTextBox";
            this.nmTextBox.Size = new System.Drawing.Size(100, 20);
            this.nmTextBox.TabIndex = 81;
            // 
            // descLabel
            // 
            this.descLabel.AutoSize = true;
            this.descLabel.Location = new System.Drawing.Point(54, 55);
            this.descLabel.Name = "descLabel";
            this.descLabel.Size = new System.Drawing.Size(63, 13);
            this.descLabel.TabIndex = 77;
            this.descLabel.Text = "Description:";
            // 
            // nmLabel
            // 
            this.nmLabel.AutoSize = true;
            this.nmLabel.Location = new System.Drawing.Point(54, 26);
            this.nmLabel.Name = "nmLabel";
            this.nmLabel.Size = new System.Drawing.Size(76, 13);
            this.nmLabel.TabIndex = 76;
            this.nmLabel.Text = "Module Name:";
            // 
            // NewModuleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 166);
            this.Controls.Add(this.stsNumericUpDwn);
            this.Controls.Add(this.stsLabel);
            this.Controls.Add(this.descTextBox);
            this.Controls.Add(this.nmTextBox);
            this.Controls.Add(this.descLabel);
            this.Controls.Add(this.nmLabel);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.closeButton);
            this.Name = "NewModuleForm";
            this.Text = "New Module";
            ((System.ComponentModel.ISupportInitialize)(this.stsNumericUpDwn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        public System.Windows.Forms.NumericUpDown stsNumericUpDwn;
        public System.Windows.Forms.Label stsLabel;
        private System.Windows.Forms.Label descLabel;
        private System.Windows.Forms.Label nmLabel;
        public System.Windows.Forms.TextBox descTextBox;
        public System.Windows.Forms.TextBox nmTextBox;
        public System.Windows.Forms.Button addButton;
    }
}