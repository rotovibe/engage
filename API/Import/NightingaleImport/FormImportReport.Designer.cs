﻿using System.Reflection.Emit;

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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.importResultBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetImportReport = new NightingaleImport.DataSetImportReport();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.lblInsertPassedValue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblInsertFailedValue = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblUpdatePassedValue = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblUpdatedFailedValue = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblUpdatedSkippedValue = new System.Windows.Forms.Label();
            this.lblUpdatedSkipped = new System.Windows.Forms.Label();
            this.firstNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dOBDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.failedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.OperationType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.failedMessageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.importResultBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetImportReport)).BeginInit();
            this.SuspendLayout();
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
            this.dataGridViewCheckBoxColumn1,
            this.OperationType,
            this.failedMessageDataGridViewTextBoxColumn,
            this.pidDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.importResultBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 54);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1321, 566);
            this.dataGridView1.TabIndex = 1;
            // 
            // importResultBindingSource
            // 
            this.importResultBindingSource.DataMember = "ImportResult";
            this.importResultBindingSource.DataSource = this.dataSetImportReport;
            // 
            // dataSetImportReport
            // 
            this.dataSetImportReport.DataSetName = "DataSetImportReport";
            this.dataSetImportReport.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Total:";
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Location = new System.Drawing.Point(77, 14);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(16, 17);
            this.lblTotalValue.TabIndex = 3;
            this.lblTotalValue.Text = "0";
            // 
            // lblInsertPassedValue
            // 
            this.lblInsertPassedValue.AutoSize = true;
            this.lblInsertPassedValue.Location = new System.Drawing.Point(225, 14);
            this.lblInsertPassedValue.Name = "lblInsertPassedValue";
            this.lblInsertPassedValue.Size = new System.Drawing.Size(16, 17);
            this.lblInsertPassedValue.TabIndex = 5;
            this.lblInsertPassedValue.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(125, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Insert Passed:";
            // 
            // lblInsertFailedValue
            // 
            this.lblInsertFailedValue.AutoSize = true;
            this.lblInsertFailedValue.Location = new System.Drawing.Point(376, 14);
            this.lblInsertFailedValue.Name = "lblInsertFailedValue";
            this.lblInsertFailedValue.Size = new System.Drawing.Size(16, 17);
            this.lblInsertFailedValue.TabIndex = 7;
            this.lblInsertFailedValue.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(281, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Insert Failed:";
            // 
            // lblUpdatePassedValue
            // 
            this.lblUpdatePassedValue.AutoSize = true;
            this.lblUpdatePassedValue.Location = new System.Drawing.Point(559, 14);
            this.lblUpdatePassedValue.Name = "lblUpdatePassedValue";
            this.lblUpdatePassedValue.Size = new System.Drawing.Size(16, 17);
            this.lblUpdatePassedValue.TabIndex = 9;
            this.lblUpdatePassedValue.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(444, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 17);
            this.label7.TabIndex = 8;
            this.label7.Text = "Update Passed:";
            // 
            // lblUpdatedFailedValue
            // 
            this.lblUpdatedFailedValue.AutoSize = true;
            this.lblUpdatedFailedValue.Location = new System.Drawing.Point(707, 14);
            this.lblUpdatedFailedValue.Name = "lblUpdatedFailedValue";
            this.lblUpdatedFailedValue.Size = new System.Drawing.Size(16, 17);
            this.lblUpdatedFailedValue.TabIndex = 11;
            this.lblUpdatedFailedValue.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(601, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 17);
            this.label9.TabIndex = 10;
            this.label9.Text = "Update Failed:";
            // 
            // lblUpdatedSkippedValue
            // 
            this.lblUpdatedSkippedValue.AutoSize = true;
            this.lblUpdatedSkippedValue.Location = new System.Drawing.Point(839, 14);
            this.lblUpdatedSkippedValue.Name = "lblUpdatedSkippedValue";
            this.lblUpdatedSkippedValue.Size = new System.Drawing.Size(16, 17);
            this.lblUpdatedSkippedValue.TabIndex = 13;
            this.lblUpdatedSkippedValue.Text = "0";
            // 
            // lblUpdatedSkipped
            // 
            this.lblUpdatedSkipped.AutoSize = true;
            this.lblUpdatedSkipped.Location = new System.Drawing.Point(747, 14);
            this.lblUpdatedSkipped.Name = "lblUpdatedSkipped";
            this.lblUpdatedSkipped.Size = new System.Drawing.Size(63, 17);
            this.lblUpdatedSkipped.TabIndex = 12;
            this.lblUpdatedSkipped.Text = "Skipped:";
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
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "Skipped";
            this.dataGridViewCheckBoxColumn1.HeaderText = "Skipped";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            // 
            // OperationType
            // 
            this.OperationType.DataPropertyName = "OperationType";
            this.OperationType.HeaderText = "OperationType";
            this.OperationType.Name = "OperationType";
            this.OperationType.ReadOnly = true;
            this.OperationType.Width = 150;
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1321, 620);
            this.Controls.Add(this.lblUpdatedSkippedValue);
            this.Controls.Add(this.lblUpdatedSkipped);
            this.Controls.Add(this.lblUpdatedFailedValue);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblUpdatePassedValue);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblInsertFailedValue);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblInsertPassedValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTotalValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormImportReport";
            this.Text = "Import Report";
            this.Load += new System.EventHandler(this.FormImportReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.importResultBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetImportReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DataSetImportReport dataSetImportReport;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource importResultBindingSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Label lblInsertPassedValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblInsertFailedValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblUpdatePassedValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblUpdatedFailedValue;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblUpdatedSkippedValue;
        private System.Windows.Forms.Label lblUpdatedSkipped;
        private System.Windows.Forms.DataGridViewTextBoxColumn firstNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dOBDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn failedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn OperationType;
        private System.Windows.Forms.DataGridViewTextBoxColumn failedMessageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pidDataGridViewTextBoxColumn;
    }
}