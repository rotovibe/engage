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
            this.uonTextBox = new System.Windows.Forms.TextBox();
            this.ubyTextBox = new System.Windows.Forms.TextBox();
            this.ttlTextBox = new System.Windows.Forms.TextBox();
            this.descTextBox = new System.Windows.Forms.TextBox();
            this.nmTextBox = new System.Windows.Forms.TextBox();
            this.uonLabel = new System.Windows.Forms.Label();
            this.ubyLabel = new System.Windows.Forms.Label();
            this.ttlLabel = new System.Windows.Forms.Label();
            this.descLabel = new System.Windows.Forms.Label();
            this.nmLabel = new System.Windows.Forms.Label();
            this.cbyTextBox = new System.Windows.Forms.TextBox();
            this.cbyLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.stsNumericUpDwn)).BeginInit();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(66, 226);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(147, 226);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // stsNumericUpDwn
            // 
            this.stsNumericUpDwn.Location = new System.Drawing.Point(136, 189);
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
            this.stsLabel.Location = new System.Drawing.Point(54, 191);
            this.stsLabel.Name = "stsLabel";
            this.stsLabel.Size = new System.Drawing.Size(40, 13);
            this.stsLabel.TabIndex = 86;
            this.stsLabel.Text = "Status:";
            // 
            // uonTextBox
            // 
            this.uonTextBox.Location = new System.Drawing.Point(136, 160);
            this.uonTextBox.Name = "uonTextBox";
            this.uonTextBox.Size = new System.Drawing.Size(100, 20);
            this.uonTextBox.TabIndex = 85;
            // 
            // ubyTextBox
            // 
            this.ubyTextBox.Location = new System.Drawing.Point(136, 131);
            this.ubyTextBox.Name = "ubyTextBox";
            this.ubyTextBox.Size = new System.Drawing.Size(100, 20);
            this.ubyTextBox.TabIndex = 84;
            // 
            // ttlTextBox
            // 
            this.ttlTextBox.Location = new System.Drawing.Point(136, 103);
            this.ttlTextBox.Name = "ttlTextBox";
            this.ttlTextBox.Size = new System.Drawing.Size(100, 20);
            this.ttlTextBox.TabIndex = 83;
            // 
            // descTextBox
            // 
            this.descTextBox.Location = new System.Drawing.Point(136, 74);
            this.descTextBox.Name = "descTextBox";
            this.descTextBox.Size = new System.Drawing.Size(100, 20);
            this.descTextBox.TabIndex = 82;
            // 
            // nmTextBox
            // 
            this.nmTextBox.Location = new System.Drawing.Point(136, 45);
            this.nmTextBox.Name = "nmTextBox";
            this.nmTextBox.Size = new System.Drawing.Size(100, 20);
            this.nmTextBox.TabIndex = 81;
            // 
            // uonLabel
            // 
            this.uonLabel.AutoSize = true;
            this.uonLabel.Location = new System.Drawing.Point(54, 163);
            this.uonLabel.Name = "uonLabel";
            this.uonLabel.Size = new System.Drawing.Size(68, 13);
            this.uonLabel.TabIndex = 80;
            this.uonLabel.Text = "Updated On:";
            // 
            // ubyLabel
            // 
            this.ubyLabel.AutoSize = true;
            this.ubyLabel.Location = new System.Drawing.Point(54, 134);
            this.ubyLabel.Name = "ubyLabel";
            this.ubyLabel.Size = new System.Drawing.Size(66, 13);
            this.ubyLabel.TabIndex = 79;
            this.ubyLabel.Text = "Updated By:";
            // 
            // ttlLabel
            // 
            this.ttlLabel.AutoSize = true;
            this.ttlLabel.Location = new System.Drawing.Point(54, 106);
            this.ttlLabel.Name = "ttlLabel";
            this.ttlLabel.Size = new System.Drawing.Size(30, 13);
            this.ttlLabel.TabIndex = 78;
            this.ttlLabel.Text = "TTL:";
            // 
            // descLabel
            // 
            this.descLabel.AutoSize = true;
            this.descLabel.Location = new System.Drawing.Point(54, 77);
            this.descLabel.Name = "descLabel";
            this.descLabel.Size = new System.Drawing.Size(63, 13);
            this.descLabel.TabIndex = 77;
            this.descLabel.Text = "Description:";
            // 
            // nmLabel
            // 
            this.nmLabel.AutoSize = true;
            this.nmLabel.Location = new System.Drawing.Point(54, 48);
            this.nmLabel.Name = "nmLabel";
            this.nmLabel.Size = new System.Drawing.Size(76, 13);
            this.nmLabel.TabIndex = 76;
            this.nmLabel.Text = "Module Name:";
            // 
            // cbyTextBox
            // 
            this.cbyTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.cbyTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cbyTextBox.Location = new System.Drawing.Point(136, 18);
            this.cbyTextBox.Name = "cbyTextBox";
            this.cbyTextBox.Size = new System.Drawing.Size(100, 20);
            this.cbyTextBox.TabIndex = 75;
            // 
            // cbyLabel
            // 
            this.cbyLabel.AutoSize = true;
            this.cbyLabel.Location = new System.Drawing.Point(54, 21);
            this.cbyLabel.Name = "cbyLabel";
            this.cbyLabel.Size = new System.Drawing.Size(62, 13);
            this.cbyLabel.TabIndex = 74;
            this.cbyLabel.Text = "Created By:";
            // 
            // NewModuleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 280);
            this.Controls.Add(this.stsNumericUpDwn);
            this.Controls.Add(this.stsLabel);
            this.Controls.Add(this.uonTextBox);
            this.Controls.Add(this.ubyTextBox);
            this.Controls.Add(this.ttlTextBox);
            this.Controls.Add(this.descTextBox);
            this.Controls.Add(this.nmTextBox);
            this.Controls.Add(this.uonLabel);
            this.Controls.Add(this.ubyLabel);
            this.Controls.Add(this.ttlLabel);
            this.Controls.Add(this.descLabel);
            this.Controls.Add(this.nmLabel);
            this.Controls.Add(this.cbyTextBox);
            this.Controls.Add(this.cbyLabel);
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
        private System.Windows.Forms.Button addButton;
        public System.Windows.Forms.NumericUpDown stsNumericUpDwn;
        public System.Windows.Forms.Label stsLabel;
        private System.Windows.Forms.TextBox uonTextBox;
        private System.Windows.Forms.TextBox ubyTextBox;
        private System.Windows.Forms.TextBox ttlTextBox;
        private System.Windows.Forms.TextBox descTextBox;
        private System.Windows.Forms.TextBox nmTextBox;
        private System.Windows.Forms.Label uonLabel;
        private System.Windows.Forms.Label ubyLabel;
        private System.Windows.Forms.Label ttlLabel;
        private System.Windows.Forms.Label descLabel;
        private System.Windows.Forms.Label nmLabel;
        public System.Windows.Forms.TextBox cbyTextBox;
        public System.Windows.Forms.Label cbyLabel;
    }
}