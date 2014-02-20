namespace NightingaleImport
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Browse = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.colFirstName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLastName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMiddleName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSuffix = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPreferredName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colGender = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDOB = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSystemID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSystemName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTimeZone = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPhone1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPhone1Preferred = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPhone1Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPhone2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPhone2Preferred = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPhone2Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEmail1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEmail1Preferred = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEmail2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEmail2Preferred = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1L1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1L2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1L3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1City = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1State = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1ZipCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1Preferred = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2L1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2L2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2L3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2City = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2State = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2ZipCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2Preferred = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.colEmail2Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEmail1Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(897, 12);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(75, 23);
            this.Browse.TabIndex = 1;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.VisibleChanged += new System.EventHandler(this.button1_VisibleChanged);
            this.Browse.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(33, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(858, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.VisibleChanged += new System.EventHandler(this.textBox1_VisibleChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(405, 505);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Import";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.VisibleChanged += new System.EventHandler(this.button1_VisibleChanged_1);
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(486, 505);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.VisibleChanged += new System.EventHandler(this.button2_VisibleChanged);
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFirstName,
            this.colLastName,
            this.colMiddleName,
            this.colSuffix,
            this.colPreferredName,
            this.colGender,
            this.colDOB,
            this.colSystemID,
            this.colSystemName,
            this.colTimeZone,
            this.colPhone1,
            this.colPhone1Preferred,
            this.colPhone1Type,
            this.colPhone2,
            this.colPhone2Preferred,
            this.colPhone2Type,
            this.colEmail1,
            this.colEmail1Preferred,
            this.colEmail1Type,
            this.colEmail2,
            this.colEmail2Preferred,
            this.colEmail2Type,
            this.colAddress1L1,
            this.colAddress1L2,
            this.colAddress1L3,
            this.colAddress1City,
            this.colAddress1State,
            this.colAddress1ZipCode,
            this.colAddress1Preferred,
            this.colAddress1Type,
            this.colAddress2L1,
            this.colAddress2L2,
            this.colAddress2L3,
            this.colAddress2City,
            this.colAddress2State,
            this.colAddress2ZipCode,
            this.colAddress2Preferred,
            this.colAddress2Type});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(33, 52);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1124, 426);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            // 
            // colFirstName
            // 
            this.colFirstName.Text = "First Name";
            this.colFirstName.Width = 70;
            // 
            // colLastName
            // 
            this.colLastName.Text = "Last Name";
            this.colLastName.Width = 70;
            // 
            // colMiddleName
            // 
            this.colMiddleName.Text = "Middle Name";
            this.colMiddleName.Width = 75;
            // 
            // colSuffix
            // 
            this.colSuffix.Text = "Suffix";
            this.colSuffix.Width = 45;
            // 
            // colPreferredName
            // 
            this.colPreferredName.Text = "Preferred Name";
            this.colPreferredName.Width = 36;
            // 
            // colGender
            // 
            this.colGender.Text = "Gender";
            this.colGender.Width = 50;
            // 
            // colDOB
            // 
            this.colDOB.Text = "DOB";
            this.colDOB.Width = 45;
            // 
            // colSystemID
            // 
            this.colSystemID.Text = "System ID";
            this.colSystemID.Width = 65;
            // 
            // colSystemName
            // 
            this.colSystemName.Text = "System Name";
            this.colSystemName.Width = 85;
            // 
            // colTimeZone
            // 
            this.colTimeZone.Text = "Time Zone";
            this.colTimeZone.Width = 70;
            // 
            // colPhone1
            // 
            this.colPhone1.Text = "Phone 1";
            // 
            // colPhone1Preferred
            // 
            this.colPhone1Preferred.Text = "Preferred";
            this.colPhone1Preferred.Width = 55;
            // 
            // colPhone1Type
            // 
            this.colPhone1Type.Text = "Type";
            this.colPhone1Type.Width = 45;
            // 
            // colPhone2
            // 
            this.colPhone2.Text = "Phone 2";
            // 
            // colPhone2Preferred
            // 
            this.colPhone2Preferred.Text = "Preferred";
            this.colPhone2Preferred.Width = 30;
            // 
            // colPhone2Type
            // 
            this.colPhone2Type.Text = "Type";
            this.colPhone2Type.Width = 29;
            // 
            // colEmail1
            // 
            this.colEmail1.Text = "Email 1";
            this.colEmail1.Width = 50;
            // 
            // colEmail1Preferred
            // 
            this.colEmail1Preferred.Text = "Preferred";
            this.colEmail1Preferred.Width = 55;
            // 
            // colEmail2
            // 
            this.colEmail2.Text = "Email 2";
            this.colEmail2.Width = 50;
            // 
            // colEmail2Preferred
            // 
            this.colEmail2Preferred.Text = "Preferred";
            this.colEmail2Preferred.Width = 55;
            // 
            // colAddress1L1
            // 
            this.colAddress1L1.Text = "Address 1";
            this.colAddress1L1.Width = 70;
            // 
            // colAddress1L2
            // 
            this.colAddress1L2.Text = "Line 2";
            this.colAddress1L2.Width = 45;
            // 
            // colAddress1L3
            // 
            this.colAddress1L3.Text = "Line 3";
            this.colAddress1L3.Width = 45;
            // 
            // colAddress1City
            // 
            this.colAddress1City.Text = "City";
            this.colAddress1City.Width = 40;
            // 
            // colAddress1State
            // 
            this.colAddress1State.Text = "State";
            this.colAddress1State.Width = 40;
            // 
            // colAddress1ZipCode
            // 
            this.colAddress1ZipCode.Text = "Zip Code";
            this.colAddress1ZipCode.Width = 22;
            // 
            // colAddress1Preferred
            // 
            this.colAddress1Preferred.Text = "Preferred";
            this.colAddress1Preferred.Width = 55;
            // 
            // colAddress2L1
            // 
            this.colAddress2L1.Text = "Address 2";
            this.colAddress2L1.Width = 70;
            // 
            // colAddress2L2
            // 
            this.colAddress2L2.Text = "Line 2";
            this.colAddress2L2.Width = 45;
            // 
            // colAddress2L3
            // 
            this.colAddress2L3.Text = "Line 3";
            this.colAddress2L3.Width = 26;
            // 
            // colAddress2City
            // 
            this.colAddress2City.Text = "City";
            this.colAddress2City.Width = 40;
            // 
            // colAddress2State
            // 
            this.colAddress2State.Text = "State";
            this.colAddress2State.Width = 40;
            // 
            // colAddress2ZipCode
            // 
            this.colAddress2ZipCode.Text = "Zip Code";
            // 
            // colAddress2Preferred
            // 
            this.colAddress2Preferred.Text = "Preferred";
            this.colAddress2Preferred.Width = 55;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(33, 484);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(70, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Select All";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // colEmail2Type
            // 
            this.colEmail2Type.Text = "Type";
            // 
            // colEmail1Type
            // 
            this.colEmail1Type.Text = "Type";
            // 
            // colAddress1Type
            // 
            this.colAddress1Type.Text = "Type";
            // 
            // colAddress2Type
            // 
            this.colAddress2Type.Text = "Type";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 540);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Browse);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.Button Browse;
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader colFirstName;
        private System.Windows.Forms.ColumnHeader colLastName;
        private System.Windows.Forms.ColumnHeader colMiddleName;
        private System.Windows.Forms.ColumnHeader colSuffix;
        private System.Windows.Forms.ColumnHeader colPreferredName;
        private System.Windows.Forms.ColumnHeader colGender;
        private System.Windows.Forms.ColumnHeader colDOB;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ColumnHeader colSystemID;
        private System.Windows.Forms.ColumnHeader colSystemName;
        private System.Windows.Forms.ColumnHeader colPhone1;
        private System.Windows.Forms.ColumnHeader colPhone1Preferred;
        private System.Windows.Forms.ColumnHeader colPhone1Type;
        private System.Windows.Forms.ColumnHeader colPhone2;
        private System.Windows.Forms.ColumnHeader colPhone2Preferred;
        private System.Windows.Forms.ColumnHeader colPhone2Type;
        private System.Windows.Forms.ColumnHeader colEmail1;
        private System.Windows.Forms.ColumnHeader colEmail1Preferred;
        private System.Windows.Forms.ColumnHeader colEmail2;
        private System.Windows.Forms.ColumnHeader colEmail2Preferred;
        private System.Windows.Forms.ColumnHeader colAddress1L1;
        private System.Windows.Forms.ColumnHeader colAddress1L2;
        private System.Windows.Forms.ColumnHeader colAddress1L3;
        private System.Windows.Forms.ColumnHeader colAddress1City;
        private System.Windows.Forms.ColumnHeader colAddress1State;
        private System.Windows.Forms.ColumnHeader colAddress1ZipCode;
        private System.Windows.Forms.ColumnHeader colAddress1Preferred;
        private System.Windows.Forms.ColumnHeader colAddress2L1;
        private System.Windows.Forms.ColumnHeader colAddress2L2;
        private System.Windows.Forms.ColumnHeader colAddress2L3;
        private System.Windows.Forms.ColumnHeader colAddress2City;
        private System.Windows.Forms.ColumnHeader colAddress2State;
        private System.Windows.Forms.ColumnHeader colAddress2ZipCode;
        private System.Windows.Forms.ColumnHeader colAddress2Preferred;
        private System.Windows.Forms.ColumnHeader colTimeZone;
        private System.Windows.Forms.ColumnHeader colEmail1Type;
        private System.Windows.Forms.ColumnHeader colEmail2Type;
        private System.Windows.Forms.ColumnHeader colAddress1Type;
        private System.Windows.Forms.ColumnHeader colAddress2Type;
    }
}

