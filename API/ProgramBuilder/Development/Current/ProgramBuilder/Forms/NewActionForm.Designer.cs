namespace ProgramBuilder
{
    partial class NewActionForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.stsNumericUpDwn)).BeginInit();
            this.SuspendLayout();
            // 
            // stsNumericUpDwn
            // 
            this.stsNumericUpDwn.Location = new System.Drawing.Point(133, 79);
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
            this.stsNumericUpDwn.TabIndex = 103;
            this.stsNumericUpDwn.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // stsLabel
            // 
            this.stsLabel.AutoSize = true;
            this.stsLabel.Location = new System.Drawing.Point(51, 81);
            this.stsLabel.Name = "stsLabel";
            this.stsLabel.Size = new System.Drawing.Size(40, 13);
            this.stsLabel.TabIndex = 102;
            this.stsLabel.Text = "Status:";
            // 
            // descTextBox
            // 
            this.descTextBox.Location = new System.Drawing.Point(133, 48);
            this.descTextBox.Name = "descTextBox";
            this.descTextBox.Size = new System.Drawing.Size(100, 20);
            this.descTextBox.TabIndex = 98;
            // 
            // nmTextBox
            // 
            this.nmTextBox.Location = new System.Drawing.Point(133, 19);
            this.nmTextBox.Name = "nmTextBox";
            this.nmTextBox.Size = new System.Drawing.Size(100, 20);
            this.nmTextBox.TabIndex = 97;
            // 
            // descLabel
            // 
            this.descLabel.AutoSize = true;
            this.descLabel.Location = new System.Drawing.Point(51, 51);
            this.descLabel.Name = "descLabel";
            this.descLabel.Size = new System.Drawing.Size(63, 13);
            this.descLabel.TabIndex = 93;
            this.descLabel.Text = "Description:";
            // 
            // nmLabel
            // 
            this.nmLabel.AutoSize = true;
            this.nmLabel.Location = new System.Drawing.Point(51, 22);
            this.nmLabel.Name = "nmLabel";
            this.nmLabel.Size = new System.Drawing.Size(71, 13);
            this.nmLabel.TabIndex = 92;
            this.nmLabel.Text = "Action Name:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(144, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 89;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(63, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 88;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // NewActionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 159);
            this.Controls.Add(this.stsNumericUpDwn);
            this.Controls.Add(this.stsLabel);
            this.Controls.Add(this.descTextBox);
            this.Controls.Add(this.nmTextBox);
            this.Controls.Add(this.descLabel);
            this.Controls.Add(this.nmLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Name = "NewActionForm";
            this.Text = "NewActionForm";
            ((System.ComponentModel.ISupportInitialize)(this.stsNumericUpDwn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.NumericUpDown stsNumericUpDwn;
        public System.Windows.Forms.Label stsLabel;
        private System.Windows.Forms.Label descLabel;
        private System.Windows.Forms.Label nmLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.TextBox descTextBox;
        public System.Windows.Forms.TextBox nmTextBox;

    }
}