namespace PhytelServicesManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.lstConnections = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbSQL = new System.Windows.Forms.GroupBox();
            this.chkSQLTrusted = new System.Windows.Forms.CheckBox();
            this.txtSQLOptions = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSQLPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSQLUserName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSQLDatabase = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSQLServerName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSQLConnName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.gbAPI = new System.Windows.Forms.GroupBox();
            this.txtAPIURL = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtAPIConnName = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.gbMongo = new System.Windows.Forms.GroupBox();
            this.txtMongoOptions = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtMongoReplSetName = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtMongoPassword = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtMongoUserName = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtMongoDatabase = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtMongoServerName = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtMongoConnName = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.gbSQL.SuspendLayout();
            this.gbAPI.SuspendLayout();
            this.gbMongo.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gbSQL);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnTest);
            this.groupBox1.Controls.Add(this.btnRemove);
            this.groupBox1.Controls.Add(this.btnAddNew);
            this.groupBox1.Controls.Add(this.gbMongo);
            this.groupBox1.Controls.Add(this.lstConnections);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(854, 472);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Existing Connections";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSave.Location = new System.Drawing.Point(569, 433);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(135, 28);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnTest
            // 
            this.btnTest.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnTest.Location = new System.Drawing.Point(712, 433);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(135, 28);
            this.btnTest.TabIndex = 5;
            this.btnTest.Text = "Test Connection";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(107, 433);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(100, 28);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAddNew
            // 
            this.btnAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddNew.Location = new System.Drawing.Point(8, 433);
            this.btnAddNew.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(100, 28);
            this.btnAddNew.TabIndex = 1;
            this.btnAddNew.Text = "Add New";
            this.btnAddNew.UseVisualStyleBackColor = true;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // lstConnections
            // 
            this.lstConnections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstConnections.FullRowSelect = true;
            this.lstConnections.HideSelection = false;
            this.lstConnections.Location = new System.Drawing.Point(8, 23);
            this.lstConnections.Margin = new System.Windows.Forms.Padding(4);
            this.lstConnections.Name = "lstConnections";
            this.lstConnections.Size = new System.Drawing.Size(199, 402);
            this.lstConnections.TabIndex = 0;
            this.lstConnections.UseCompatibleStateImageBehavior = false;
            this.lstConnections.View = System.Windows.Forms.View.Details;
            this.lstConnections.SelectedIndexChanged += new System.EventHandler(this.lstConnections_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Connection Name";
            this.columnHeader1.Width = 258;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 131;
            // 
            // gbSQL
            // 
            this.gbSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSQL.Controls.Add(this.chkSQLTrusted);
            this.gbSQL.Controls.Add(this.txtSQLOptions);
            this.gbSQL.Controls.Add(this.label2);
            this.gbSQL.Controls.Add(this.txtSQLPassword);
            this.gbSQL.Controls.Add(this.label4);
            this.gbSQL.Controls.Add(this.txtSQLUserName);
            this.gbSQL.Controls.Add(this.label5);
            this.gbSQL.Controls.Add(this.txtSQLDatabase);
            this.gbSQL.Controls.Add(this.label6);
            this.gbSQL.Controls.Add(this.txtSQLServerName);
            this.gbSQL.Controls.Add(this.label7);
            this.gbSQL.Controls.Add(this.txtSQLConnName);
            this.gbSQL.Controls.Add(this.label8);
            this.gbSQL.Location = new System.Drawing.Point(214, 23);
            this.gbSQL.Name = "gbSQL";
            this.gbSQL.Size = new System.Drawing.Size(633, 403);
            this.gbSQL.TabIndex = 3;
            this.gbSQL.TabStop = false;
            this.gbSQL.Text = "SQL Server Connection";
            // 
            // chkSQLTrusted
            // 
            this.chkSQLTrusted.AutoSize = true;
            this.chkSQLTrusted.Location = new System.Drawing.Point(165, 139);
            this.chkSQLTrusted.Margin = new System.Windows.Forms.Padding(4);
            this.chkSQLTrusted.Name = "chkSQLTrusted";
            this.chkSQLTrusted.Size = new System.Drawing.Size(176, 21);
            this.chkSQLTrusted.TabIndex = 6;
            this.chkSQLTrusted.Text = "Use trusted connection";
            this.chkSQLTrusted.UseVisualStyleBackColor = true;
            this.chkSQLTrusted.CheckedChanged += new System.EventHandler(this.chkTrustedConnection_CheckedChanged);
            this.chkSQLTrusted.TextChanged += new System.EventHandler(this.chkTrustedConnection_CheckedChanged);
            // 
            // txtSQLOptions
            // 
            this.txtSQLOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSQLOptions.Location = new System.Drawing.Point(163, 229);
            this.txtSQLOptions.Margin = new System.Windows.Forms.Padding(4);
            this.txtSQLOptions.Multiline = true;
            this.txtSQLOptions.Name = "txtSQLOptions";
            this.txtSQLOptions.Size = new System.Drawing.Size(463, 160);
            this.txtSQLOptions.TabIndex = 12;
            this.txtSQLOptions.TextChanged += new System.EventHandler(this.txtOptions_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 232);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Options:";
            // 
            // txtSQLPassword
            // 
            this.txtSQLPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSQLPassword.Location = new System.Drawing.Point(165, 199);
            this.txtSQLPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtSQLPassword.Name = "txtSQLPassword";
            this.txtSQLPassword.PasswordChar = '*';
            this.txtSQLPassword.Size = new System.Drawing.Size(463, 22);
            this.txtSQLPassword.TabIndex = 10;
            this.txtSQLPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 202);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Password:";
            // 
            // txtSQLUserName
            // 
            this.txtSQLUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSQLUserName.Location = new System.Drawing.Point(165, 167);
            this.txtSQLUserName.Margin = new System.Windows.Forms.Padding(4);
            this.txtSQLUserName.Name = "txtSQLUserName";
            this.txtSQLUserName.Size = new System.Drawing.Size(463, 22);
            this.txtSQLUserName.TabIndex = 8;
            this.txtSQLUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 170);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "User Name:";
            // 
            // txtSQLDatabase
            // 
            this.txtSQLDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSQLDatabase.Location = new System.Drawing.Point(165, 105);
            this.txtSQLDatabase.Margin = new System.Windows.Forms.Padding(4);
            this.txtSQLDatabase.Name = "txtSQLDatabase";
            this.txtSQLDatabase.Size = new System.Drawing.Size(463, 22);
            this.txtSQLDatabase.TabIndex = 5;
            this.txtSQLDatabase.TextChanged += new System.EventHandler(this.txtDatabase_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 108);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 17);
            this.label6.TabIndex = 4;
            this.label6.Text = "Database:";
            // 
            // txtSQLServerName
            // 
            this.txtSQLServerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSQLServerName.Location = new System.Drawing.Point(165, 73);
            this.txtSQLServerName.Margin = new System.Windows.Forms.Padding(4);
            this.txtSQLServerName.Name = "txtSQLServerName";
            this.txtSQLServerName.Size = new System.Drawing.Size(463, 22);
            this.txtSQLServerName.TabIndex = 3;
            this.txtSQLServerName.TextChanged += new System.EventHandler(this.txtServer_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 76);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 17);
            this.label7.TabIndex = 2;
            this.label7.Text = "Server:";
            // 
            // txtSQLConnName
            // 
            this.txtSQLConnName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSQLConnName.Location = new System.Drawing.Point(165, 41);
            this.txtSQLConnName.Margin = new System.Windows.Forms.Padding(4);
            this.txtSQLConnName.Name = "txtSQLConnName";
            this.txtSQLConnName.Size = new System.Drawing.Size(463, 22);
            this.txtSQLConnName.TabIndex = 1;
            this.txtSQLConnName.TextChanged += new System.EventHandler(this.txtConnName_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 44);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(124, 17);
            this.label8.TabIndex = 0;
            this.label8.Text = "Connection Name:";
            // 
            // gbAPI
            // 
            this.gbAPI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAPI.Controls.Add(this.txtAPIURL);
            this.gbAPI.Controls.Add(this.label20);
            this.gbAPI.Controls.Add(this.txtAPIConnName);
            this.gbAPI.Controls.Add(this.label21);
            this.gbAPI.Location = new System.Drawing.Point(0, 0);
            this.gbAPI.Name = "gbAPI";
            this.gbAPI.Size = new System.Drawing.Size(633, 403);
            this.gbAPI.TabIndex = 2;
            this.gbAPI.TabStop = false;
            this.gbAPI.Text = "API Connection";
            // 
            // txtAPIURL
            // 
            this.txtAPIURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAPIURL.Location = new System.Drawing.Point(165, 72);
            this.txtAPIURL.Margin = new System.Windows.Forms.Padding(4);
            this.txtAPIURL.Name = "txtAPIURL";
            this.txtAPIURL.Size = new System.Drawing.Size(463, 22);
            this.txtAPIURL.TabIndex = 2;
            this.txtAPIURL.TextChanged += new System.EventHandler(this.txtAPIURL_TextChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(7, 75);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(65, 17);
            this.label20.TabIndex = 1;
            this.label20.Text = "API URL:";
            // 
            // txtAPIConnName
            // 
            this.txtAPIConnName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAPIConnName.Location = new System.Drawing.Point(165, 41);
            this.txtAPIConnName.Margin = new System.Windows.Forms.Padding(4);
            this.txtAPIConnName.Name = "txtAPIConnName";
            this.txtAPIConnName.Size = new System.Drawing.Size(463, 22);
            this.txtAPIConnName.TabIndex = 0;
            this.txtAPIConnName.TextChanged += new System.EventHandler(this.txtConnName_TextChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(7, 44);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(124, 17);
            this.label21.TabIndex = 25;
            this.label21.Text = "Connection Name:";
            // 
            // gbMongo
            // 
            this.gbMongo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbMongo.Controls.Add(this.txtMongoOptions);
            this.gbMongo.Controls.Add(this.label13);
            this.gbMongo.Controls.Add(this.txtMongoReplSetName);
            this.gbMongo.Controls.Add(this.gbAPI);
            this.gbMongo.Controls.Add(this.label14);
            this.gbMongo.Controls.Add(this.txtMongoPassword);
            this.gbMongo.Controls.Add(this.label15);
            this.gbMongo.Controls.Add(this.txtMongoUserName);
            this.gbMongo.Controls.Add(this.label16);
            this.gbMongo.Controls.Add(this.txtMongoDatabase);
            this.gbMongo.Controls.Add(this.label17);
            this.gbMongo.Controls.Add(this.txtMongoServerName);
            this.gbMongo.Controls.Add(this.label18);
            this.gbMongo.Controls.Add(this.txtMongoConnName);
            this.gbMongo.Controls.Add(this.label19);
            this.gbMongo.Location = new System.Drawing.Point(214, 23);
            this.gbMongo.Name = "gbMongo";
            this.gbMongo.Size = new System.Drawing.Size(633, 403);
            this.gbMongo.TabIndex = 3;
            this.gbMongo.TabStop = false;
            this.gbMongo.Text = "MongoDB Connection";
            // 
            // txtMongoOptions
            // 
            this.txtMongoOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMongoOptions.Location = new System.Drawing.Point(165, 227);
            this.txtMongoOptions.Margin = new System.Windows.Forms.Padding(4);
            this.txtMongoOptions.Multiline = true;
            this.txtMongoOptions.Name = "txtMongoOptions";
            this.txtMongoOptions.Size = new System.Drawing.Size(463, 162);
            this.txtMongoOptions.TabIndex = 13;
            this.txtMongoOptions.TextChanged += new System.EventHandler(this.txtOptions_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 230);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(61, 17);
            this.label13.TabIndex = 12;
            this.label13.Text = "Options:";
            // 
            // txtMongoReplSetName
            // 
            this.txtMongoReplSetName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMongoReplSetName.Location = new System.Drawing.Point(165, 196);
            this.txtMongoReplSetName.Margin = new System.Windows.Forms.Padding(4);
            this.txtMongoReplSetName.Name = "txtMongoReplSetName";
            this.txtMongoReplSetName.Size = new System.Drawing.Size(463, 22);
            this.txtMongoReplSetName.TabIndex = 11;
            this.txtMongoReplSetName.TextChanged += new System.EventHandler(this.txtReplSetName_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 199);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(148, 17);
            this.label14.TabIndex = 10;
            this.label14.Text = "Replication Set Name:";
            // 
            // txtMongoPassword
            // 
            this.txtMongoPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMongoPassword.Location = new System.Drawing.Point(165, 165);
            this.txtMongoPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtMongoPassword.Name = "txtMongoPassword";
            this.txtMongoPassword.PasswordChar = '*';
            this.txtMongoPassword.Size = new System.Drawing.Size(463, 22);
            this.txtMongoPassword.TabIndex = 9;
            this.txtMongoPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(7, 168);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(73, 17);
            this.label15.TabIndex = 8;
            this.label15.Text = "Password:";
            // 
            // txtMongoUserName
            // 
            this.txtMongoUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMongoUserName.Location = new System.Drawing.Point(165, 134);
            this.txtMongoUserName.Margin = new System.Windows.Forms.Padding(4);
            this.txtMongoUserName.Name = "txtMongoUserName";
            this.txtMongoUserName.Size = new System.Drawing.Size(463, 22);
            this.txtMongoUserName.TabIndex = 7;
            this.txtMongoUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(7, 137);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(83, 17);
            this.label16.TabIndex = 6;
            this.label16.Text = "User Name:";
            // 
            // txtMongoDatabase
            // 
            this.txtMongoDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMongoDatabase.Location = new System.Drawing.Point(165, 103);
            this.txtMongoDatabase.Margin = new System.Windows.Forms.Padding(4);
            this.txtMongoDatabase.Name = "txtMongoDatabase";
            this.txtMongoDatabase.Size = new System.Drawing.Size(463, 22);
            this.txtMongoDatabase.TabIndex = 5;
            this.txtMongoDatabase.TextChanged += new System.EventHandler(this.txtDatabase_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(7, 106);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(73, 17);
            this.label17.TabIndex = 4;
            this.label17.Text = "Database:";
            // 
            // txtMongoServerName
            // 
            this.txtMongoServerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMongoServerName.Location = new System.Drawing.Point(165, 72);
            this.txtMongoServerName.Margin = new System.Windows.Forms.Padding(4);
            this.txtMongoServerName.Name = "txtMongoServerName";
            this.txtMongoServerName.Size = new System.Drawing.Size(463, 22);
            this.txtMongoServerName.TabIndex = 3;
            this.txtMongoServerName.TextChanged += new System.EventHandler(this.txtServer_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(7, 75);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(54, 17);
            this.label18.TabIndex = 2;
            this.label18.Text = "Server:";
            // 
            // txtMongoConnName
            // 
            this.txtMongoConnName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMongoConnName.Location = new System.Drawing.Point(165, 41);
            this.txtMongoConnName.Margin = new System.Windows.Forms.Padding(4);
            this.txtMongoConnName.Name = "txtMongoConnName";
            this.txtMongoConnName.Size = new System.Drawing.Size(463, 22);
            this.txtMongoConnName.TabIndex = 1;
            this.txtMongoConnName.TextChanged += new System.EventHandler(this.txtConnName_TextChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(7, 44);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(124, 17);
            this.label19.TabIndex = 0;
            this.label19.Text = "Connection Name:";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(770, 495);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 28);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(13, 491);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(100, 17);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "Version: x.x.x.x";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 533);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(901, 578);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Phytel Services Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.gbSQL.ResumeLayout(false);
            this.gbSQL.PerformLayout();
            this.gbAPI.ResumeLayout(false);
            this.gbAPI.PerformLayout();
            this.gbMongo.ResumeLayout(false);
            this.gbMongo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.ListView lstConnections;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.GroupBox gbSQL;
        private System.Windows.Forms.CheckBox chkSQLTrusted;
        private System.Windows.Forms.TextBox txtSQLOptions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSQLPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSQLUserName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSQLDatabase;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSQLServerName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSQLConnName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox gbMongo;
        private System.Windows.Forms.TextBox txtMongoOptions;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtMongoReplSetName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtMongoPassword;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtMongoUserName;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtMongoDatabase;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtMongoServerName;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtMongoConnName;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox gbAPI;
        private System.Windows.Forms.TextBox txtAPIURL;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtAPIConnName;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button btnSave;
    }
}

