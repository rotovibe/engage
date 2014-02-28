namespace NGTestData
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numPatients = new System.Windows.Forms.NumericUpDown();
            this.numProblems = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.rdoDev = new System.Windows.Forms.RadioButton();
            this.rdoModel = new System.Windows.Forms.RadioButton();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPatients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numProblems)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(220, 106);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "How Many Patients:";
            // 
            // numPatients
            // 
            this.numPatients.Location = new System.Drawing.Point(160, 27);
            this.numPatients.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numPatients.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numPatients.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPatients.Name = "numPatients";
            this.numPatients.Size = new System.Drawing.Size(160, 22);
            this.numPatients.TabIndex = 2;
            this.numPatients.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numProblems
            // 
            this.numProblems.Location = new System.Drawing.Point(160, 59);
            this.numProblems.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numProblems.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numProblems.Name = "numProblems";
            this.numProblems.Size = new System.Drawing.Size(160, 22);
            this.numProblems.TabIndex = 4;
            this.numProblems.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Max Problems Per:";
            // 
            // rdoDev
            // 
            this.rdoDev.AutoSize = true;
            this.rdoDev.Checked = true;
            this.rdoDev.Location = new System.Drawing.Point(16, 95);
            this.rdoDev.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoDev.Name = "rdoDev";
            this.rdoDev.Size = new System.Drawing.Size(54, 21);
            this.rdoDev.TabIndex = 5;
            this.rdoDev.TabStop = true;
            this.rdoDev.Text = "Dev";
            this.rdoDev.UseVisualStyleBackColor = true;
            // 
            // rdoModel
            // 
            this.rdoModel.AutoSize = true;
            this.rdoModel.Location = new System.Drawing.Point(16, 123);
            this.rdoModel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoModel.Name = "rdoModel";
            this.rdoModel.Size = new System.Drawing.Size(67, 21);
            this.rdoModel.TabIndex = 6;
            this.rdoModel.Text = "Model";
            this.rdoModel.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(220, 142);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 28);
            this.button2.TabIndex = 7;
            this.button2.Text = "Create Users";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 211);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.rdoModel);
            this.Controls.Add(this.rdoDev);
            this.Controls.Add(this.numProblems);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numPatients);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.numPatients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numProblems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numPatients;
        private System.Windows.Forms.NumericUpDown numProblems;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdoDev;
        private System.Windows.Forms.RadioButton rdoModel;
        private System.Windows.Forms.Button button2;
    }
}

