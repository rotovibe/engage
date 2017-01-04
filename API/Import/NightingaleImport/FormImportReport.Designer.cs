namespace NightingaleImport
{
    partial class FormImportReport
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
            this.components = new System.ComponentModel.Container();
            this.dataSetImportReport = new NightingaleImport.DataSetImportReport();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.importResultBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.firstNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dOBDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.failedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.failedMessageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetImportReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.importResultBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataSetImportReport
            // 
            this.dataSetImportReport.DataSetName = "DataSetImportReport";
            this.dataSetImportReport.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.firstNameDataGridViewTextBoxColumn,
            this.lastNameDataGridViewTextBoxColumn,
            this.dOBDataGridViewTextBoxColumn,
            this.failedDataGridViewCheckBoxColumn,
            this.failedMessageDataGridViewTextBoxColumn,
            this.pidDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.importResultBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(991, 504);
            this.dataGridView1.TabIndex = 1;
            // 
            // importResultBindingSource
            // 
            this.importResultBindingSource.DataMember = "ImportResult";
            this.importResultBindingSource.DataSource = this.dataSetImportReport;
            // 
            // firstNameDataGridViewTextBoxColumn
            // 
            this.firstNameDataGridViewTextBoxColumn.DataPropertyName = "FirstName";
            this.firstNameDataGridViewTextBoxColumn.HeaderText = "FirstName";
            this.firstNameDataGridViewTextBoxColumn.Name = "firstNameDataGridViewTextBoxColumn";
            this.firstNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.firstNameDataGridViewTextBoxColumn.Width = 190;
            // 
            // lastNameDataGridViewTextBoxColumn
            // 
            this.lastNameDataGridViewTextBoxColumn.DataPropertyName = "LastName";
            this.lastNameDataGridViewTextBoxColumn.HeaderText = "LastName";
            this.lastNameDataGridViewTextBoxColumn.Name = "lastNameDataGridViewTextBoxColumn";
            this.lastNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastNameDataGridViewTextBoxColumn.Width = 189;
            // 
            // dOBDataGridViewTextBoxColumn
            // 
            this.dOBDataGridViewTextBoxColumn.DataPropertyName = "DOB";
            this.dOBDataGridViewTextBoxColumn.HeaderText = "DOB";
            this.dOBDataGridViewTextBoxColumn.Name = "dOBDataGridViewTextBoxColumn";
            this.dOBDataGridViewTextBoxColumn.ReadOnly = true;
            this.dOBDataGridViewTextBoxColumn.Width = 190;
            // 
            // failedDataGridViewCheckBoxColumn
            // 
            this.failedDataGridViewCheckBoxColumn.DataPropertyName = "Failed";
            this.failedDataGridViewCheckBoxColumn.HeaderText = "Failed";
            this.failedDataGridViewCheckBoxColumn.Name = "failedDataGridViewCheckBoxColumn";
            this.failedDataGridViewCheckBoxColumn.ReadOnly = true;
            this.failedDataGridViewCheckBoxColumn.Width = 189;
            // 
            // failedMessageDataGridViewTextBoxColumn
            // 
            this.failedMessageDataGridViewTextBoxColumn.DataPropertyName = "FailedMessage";
            this.failedMessageDataGridViewTextBoxColumn.HeaderText = "FailedMessage";
            this.failedMessageDataGridViewTextBoxColumn.Name = "failedMessageDataGridViewTextBoxColumn";
            this.failedMessageDataGridViewTextBoxColumn.ReadOnly = true;
            this.failedMessageDataGridViewTextBoxColumn.Width = 500;
            // 
            // pidDataGridViewTextBoxColumn
            // 
            this.pidDataGridViewTextBoxColumn.DataPropertyName = "pid";
            this.pidDataGridViewTextBoxColumn.HeaderText = "pid";
            this.pidDataGridViewTextBoxColumn.Name = "pidDataGridViewTextBoxColumn";
            this.pidDataGridViewTextBoxColumn.ReadOnly = true;
            this.pidDataGridViewTextBoxColumn.Visible = false;
            // 
            // FormImportReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 504);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormImportReport";
            this.Text = "Import Report";
            this.Load += new System.EventHandler(this.FormImportReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataSetImportReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.importResultBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DataSetImportReport dataSetImportReport;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource importResultBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn firstNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dOBDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn failedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn failedMessageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pidDataGridViewTextBoxColumn;
    }
}