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
            this.colEmail1Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEmail2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEmail2Preferred = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEmail2Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1Line1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1Line2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1Line3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1City = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1State = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1Zip = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1Preferred = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress1Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2Line1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2Line2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2Line3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2City = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2State = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2Zip = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2Preferred = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAddress2Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCareManagerUserID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtContactID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblUrl = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(568, 15);
            this.Browse.Margin = new System.Windows.Forms.Padding(4);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(100, 28);
            this.Browse.TabIndex = 1;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.VisibleChanged += new System.EventHandler(this.button1_VisibleChanged);
            this.Browse.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(44, 18);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(523, 22);
            this.textBox1.TabIndex = 2;
            this.textBox1.VisibleChanged += new System.EventHandler(this.textBox1_VisibleChanged);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(382, 396);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 4;
            this.button1.Text = "Import";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.VisibleChanged += new System.EventHandler(this.button1_VisibleChanged_1);
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button2.Location = new System.Drawing.Point(490, 396);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 28);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.VisibleChanged += new System.EventHandler(this.button2_VisibleChanged);
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.colAddress1Line1,
            this.colAddress1Line2,
            this.colAddress1Line3,
            this.colAddress1City,
            this.colAddress1State,
            this.colAddress1Zip,
            this.colAddress1Preferred,
            this.colAddress1Type,
            this.colAddress2Line1,
            this.colAddress2Line2,
            this.colAddress2Line3,
            this.colAddress2City,
            this.colAddress2State,
            this.colAddress2Zip,
            this.colAddress2Preferred,
            this.colAddress2Type,
            this.colCareManagerUserID});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(44, 64);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1009, 306);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            // 
            // colFirstName
            // 
            this.colFirstName.Text = "First Name";
            this.colFirstName.Width = 80;
            // 
            // colLastName
            // 
            this.colLastName.Text = "Last Name";
            this.colLastName.Width = 73;
            // 
            // colMiddleName
            // 
            this.colMiddleName.Text = "Middle Name";
            this.colMiddleName.Width = 74;
            // 
            // colSuffix
            // 
            this.colSuffix.Text = "Suffix";
            this.colSuffix.Width = 40;
            // 
            // colPreferredName
            // 
            this.colPreferredName.Text = "Preferred Name";
            this.colPreferredName.Width = 95;
            // 
            // colGender
            // 
            this.colGender.Text = "Gender";
            this.colGender.Width = 50;
            // 
            // colDOB
            // 
            this.colDOB.Text = "DOB";
            this.colDOB.Width = 62;
            // 
            // colSystemID
            // 
            this.colSystemID.Text = "System ID";
            // 
            // colSystemName
            // 
            this.colSystemName.Text = "System Name";
            this.colSystemName.Width = 81;
            // 
            // colTimeZone
            // 
            this.colTimeZone.Text = "Time Zone";
            // 
            // colPhone1
            // 
            this.colPhone1.Text = "Phone 1";
            // 
            // colPhone1Preferred
            // 
            this.colPhone1Preferred.Text = "Preferred";
            // 
            // colPhone1Type
            // 
            this.colPhone1Type.Text = "Type";
            // 
            // colPhone2
            // 
            this.colPhone2.Text = "Phone 2";
            // 
            // colPhone2Preferred
            // 
            this.colPhone2Preferred.Text = "Preferred";
            // 
            // colPhone2Type
            // 
            this.colPhone2Type.Text = "Type";
            // 
            // colEmail1
            // 
            this.colEmail1.Text = "Email 1";
            // 
            // colEmail1Preferred
            // 
            this.colEmail1Preferred.Text = "Preferred";
            // 
            // colEmail1Type
            // 
            this.colEmail1Type.Text = "Type";
            // 
            // colEmail2
            // 
            this.colEmail2.Text = "Email 2";
            // 
            // colEmail2Preferred
            // 
            this.colEmail2Preferred.Text = "Preferred";
            // 
            // colEmail2Type
            // 
            this.colEmail2Type.Text = "Type";
            // 
            // colAddress1Line1
            // 
            this.colAddress1Line1.Text = "Address 1";
            // 
            // colAddress1Line2
            // 
            this.colAddress1Line2.Text = "Line 2";
            // 
            // colAddress1Line3
            // 
            this.colAddress1Line3.Text = "Line 3";
            // 
            // colAddress1City
            // 
            this.colAddress1City.Text = "City";
            // 
            // colAddress1State
            // 
            this.colAddress1State.Text = "State";
            // 
            // colAddress1Zip
            // 
            this.colAddress1Zip.Text = "Zip Code";
            // 
            // colAddress1Preferred
            // 
            this.colAddress1Preferred.Text = "Preferred";
            // 
            // colAddress1Type
            // 
            this.colAddress1Type.Text = "Type";
            // 
            // colAddress2Line1
            // 
            this.colAddress2Line1.Text = "Address 2";
            // 
            // colAddress2Line2
            // 
            this.colAddress2Line2.Text = "Line 2";
            // 
            // colAddress2Line3
            // 
            this.colAddress2Line3.Text = "Line 3";
            // 
            // colAddress2City
            // 
            this.colAddress2City.Text = "City";
            // 
            // colAddress2State
            // 
            this.colAddress2State.Text = "State";
            // 
            // colAddress2Zip
            // 
            this.colAddress2Zip.Text = "Zip Code";
            // 
            // colAddress2Preferred
            // 
            this.colAddress2Preferred.Text = "Preferred";
            // 
            // colAddress2Type
            // 
            this.colAddress2Type.Text = "Type";
            // 
            // colCareManagerUserID
            // 
            this.colCareManagerUserID.Text = "Care Manager User ID";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(44, 379);
            this.chkSelectAll.Margin = new System.Windows.Forms.Padding(4);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(88, 21);
            this.chkSelectAll.TabIndex = 7;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(656, 380);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Import Admin User Name:";
            // 
            // txtContactID
            // 
            this.txtContactID.Location = new System.Drawing.Point(836, 377);
            this.txtContactID.Name = "txtContactID";
            this.txtContactID.Size = new System.Drawing.Size(217, 22);
            this.txtContactID.TabIndex = 9;
            this.txtContactID.Text = "inhealthadmin";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(656, 407);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "Import URL:";
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(745, 407);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(0, 17);
            this.lblUrl.TabIndex = 11;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(41, 407);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(83, 17);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "Import URL:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 439);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtContactID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Browse);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nightingale Import Utility";
            this.Load += new System.EventHandler(this.Form1_Load);
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
        private System.Windows.Forms.CheckBox chkSelectAll;
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
        private System.Windows.Forms.ColumnHeader colEmail1Type;
        private System.Windows.Forms.ColumnHeader colEmail2;
        private System.Windows.Forms.ColumnHeader colEmail2Preferred;
        private System.Windows.Forms.ColumnHeader colEmail2Type;
        private System.Windows.Forms.ColumnHeader colAddress1Line1;
        private System.Windows.Forms.ColumnHeader colAddress1Line2;
        private System.Windows.Forms.ColumnHeader colAddress1Line3;
        private System.Windows.Forms.ColumnHeader colAddress1City;
        private System.Windows.Forms.ColumnHeader colAddress1State;
        private System.Windows.Forms.ColumnHeader colAddress1Zip;
        private System.Windows.Forms.ColumnHeader colAddress1Preferred;
        private System.Windows.Forms.ColumnHeader colAddress1Type;
        private System.Windows.Forms.ColumnHeader colAddress2Line1;
        private System.Windows.Forms.ColumnHeader colAddress2Line2;
        private System.Windows.Forms.ColumnHeader colAddress2Line3;
        private System.Windows.Forms.ColumnHeader colAddress2City;
        private System.Windows.Forms.ColumnHeader colAddress2State;
        private System.Windows.Forms.ColumnHeader colAddress2Zip;
        private System.Windows.Forms.ColumnHeader colAddress2Preferred;
        private System.Windows.Forms.ColumnHeader colAddress2Type;
        private System.Windows.Forms.ColumnHeader colCareManagerUserID;
        private System.Windows.Forms.ColumnHeader colTimeZone;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtContactID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.Label lblStatus;
    }
}

