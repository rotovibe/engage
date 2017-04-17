using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using Microsoft.ReportingServices.Diagnostics.Internal;
using NGDataImport;
using Phytel.API.DataDomain.Patient.DTO;
using static NightingaleImport.DataSetImportReport;

namespace NightingaleImport
{
    public partial class FormImportReport : Form
    {
        //private Dictionary<string, ImportData> currentData;
        private List<ImportData> currentData;
        private int insertPassed = 0;
        private int insertFailed = 0;
        private int updatePassed = 0;
        private int updateFailed = 0;
        private int skippedRow = 0;
        private int total = 0;
        private int flag = 0;

        public int Total { get { return total; } set { total = value; } }
        public int UpdateFailed { get { return updateFailed; } set { total = updateFailed; } }
        public int InsertFailed { get { return insertFailed; } set { total = insertFailed; } }
        public int UpdatePassed { get { return updatePassed; } set { updatePassed = value; } }
        public int InsertPassed { get { return insertPassed; } set { insertPassed = value; } }

        //public FormImportReport(Dictionary<string,ImportData> data )
        //{
        //    InitializeComponent();
        //    currentData = data;
        //    SetDataset();
        //}

        public FormImportReport(List<ImportData> data)
        {
            InitializeComponent();
            currentData = data;
            SetDataset();
        }
        public FormImportReport()
        {
        }
        private void SetDataset()
        {
            foreach (var key in currentData)
            {
                ImportResultRow dr = this.dataSetImportReport.ImportResult.NewImportResultRow();
                var currentValue = key;
                dr.pid = currentValue.id.ToString();
                if (currentValue.patientData != null)
                {
                    dr.DOB = currentValue.patientData.DOB;
                    dr.FirstName = currentValue.patientData.FirstName;
                    dr.LastName = currentValue.patientData.LastName;
                }
                dr.Failed = currentValue.failed;
                dr.FailedMessage = currentValue.failedMessage;
                dr.Skipped = currentValue.skipped;
                dr.OperationType = currentValue.importOperation.ToString();
                if ((currentValue.skipped) || (currentValue.failed))
                {
                    this.dataSetImportReport.ImportResult.Rows.Add(dr);
                }
                setCounts(dr);
            }
        }

        private void setCounts(ImportResultRow dr)
        {
            total++;
            switch (dr.OperationType)
            {
                case "INSERT":
                    {
                        if (dr.Failed)
                            insertFailed++;
                        else
                            insertPassed++;
                        break;
                    }
                case "UPDATE":
                    {
                        if (dr.Failed)
                            updateFailed++;
                        else
                            updatePassed++;
                        break;
                    }
                case "SKIPPED":
                    {
                        if (dr.Skipped)
                            skippedRow++;
                        break;
                    }
                case "LOOKUP_USER_CONTACT":
                    {
                        if (dr.Skipped)
                            skippedRow++;
                        break;
                    }
                case "LOOKUP_PATIENT":
                    {
                        if (dr.Skipped)
                            skippedRow++;
                        break;
                    }

                default:
                    break;
            }
        }

        private void FormImportReport_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = this.dataSetImportReport.ImportResult;
            lblTotalValue.Text = Total.ToString();
            lblInsertPassedValue.Text = InsertPassed.ToString();
            lblInsertFailedValue.Text = InsertFailed.ToString();
            lblUpdatePassedValue.Text = UpdatePassed.ToString();
            lblUpdatedFailedValue.Text = UpdateFailed.ToString();
            lblUpdatedSkippedValue.Text = skippedRow.ToString();
        }
    }
}
