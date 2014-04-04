namespace ProgramBuilder.Forms
{
    partial class NewStepForm
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
            this.stsNumericUpDwn = new System.Windows.Forms.NumericUpDown();
            this.stsLabel = new System.Windows.Forms.Label();
            this.descTextBox = new System.Windows.Forms.TextBox();
            this.nmTextBox = new System.Windows.Forms.TextBox();
            this.descLabel = new System.Windows.Forms.Label();
            this.nmLabel = new System.Windows.Forms.Label();
            this.addButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.stsNumericUpDwn)).BeginInit();
            this.SuspendLayout();
            // 
            // stsNumericUpDwn
            // 
            this.stsNumericUpDwn.Location = new System.Drawing.Point(139, 83);
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
            this.stsNumericUpDwn.TabIndex = 95;
            this.stsNumericUpDwn.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // stsLabel
            // 
            this.stsLabel.AutoSize = true;
            this.stsLabel.Location = new System.Drawing.Point(57, 85);
            this.stsLabel.Name = "stsLabel";
            this.stsLabel.Size = new System.Drawing.Size(40, 13);
            this.stsLabel.TabIndex = 94;
            this.stsLabel.Text = "Status:";
            // 
            // descTextBox
            // 
            this.descTextBox.Location = new System.Drawing.Point(139, 53);
            this.descTextBox.Name = "descTextBox";
            this.descTextBox.Size = new System.Drawing.Size(100, 20);
            this.descTextBox.TabIndex = 93;
            // 
            // nmTextBox
            // 
            this.nmTextBox.Location = new System.Drawing.Point(139, 24);
            this.nmTextBox.Name = "nmTextBox";
            this.nmTextBox.Size = new System.Drawing.Size(100, 20);
            this.nmTextBox.TabIndex = 92;
            // 
            // descLabel
            // 
            this.descLabel.AutoSize = true;
            this.descLabel.Location = new System.Drawing.Point(57, 56);
            this.descLabel.Name = "descLabel";
            this.descLabel.Size = new System.Drawing.Size(63, 13);
            this.descLabel.TabIndex = 91;
            this.descLabel.Text = "Description:";
            // 
            // nmLabel
            // 
            this.nmLabel.AutoSize = true;
            this.nmLabel.Location = new System.Drawing.Point(57, 27);
            this.nmLabel.Name = "nmLabel";
            this.nmLabel.Size = new System.Drawing.Size(63, 13);
            this.nmLabel.TabIndex = 90;
            this.nmLabel.Text = "Step Name:";
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(150, 120);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 89;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(69, 120);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 88;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // NewStepForm
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
            this.Name = "NewStepForm";
            this.Text = "NewStepForm";
            ((System.ComponentModel.ISupportInitialize)(this.stsNumericUpDwn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.NumericUpDown stsNumericUpDwn;
        public System.Windows.Forms.Label stsLabel;
        public System.Windows.Forms.TextBox descTextBox;
        public System.Windows.Forms.TextBox nmTextBox;
        public System.Windows.Forms.Label descLabel;
        public System.Windows.Forms.Label nmLabel;
        public System.Windows.Forms.Button addButton;
        public System.Windows.Forms.Button closeButton;

    }
}