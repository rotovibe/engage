using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using Microsoft.ReportingServices.Diagnostics.Internal;
using NGDataImport;
using static NightingaleImport.DataSetImportReport;

namespace NightingaleImport
{
    public partial class FormImportReport : Form
    {
        private Dictionary<string, ImportData> currentData;
        private int insertPassed = 0;
        private int insertFailed = 0;
        private int updatePassed = 0;
        private int updateFailed = 0;
        private int total = 0;

        public FormImportReport(Dictionary<string,ImportData> data )
        {
            InitializeComponent();
            currentData = data;
            SetDataset();

        }

        private void SetDataset()
        {                      
            foreach (var key in currentData.Keys)
            {
                if (currentData[key].patientData!=null)
                {                                    
                    ImportResultRow dr = this.dataSetImportReport.ImportResult.NewImportResultRow();
                    var currentValue = currentData[key];
                    dr.pid = key;
                    dr.DOB = currentValue.patientData.DOB;
                    dr.FirstName = currentValue.patientData.FirstName;
                    dr.LastName = currentValue.patientData.LastName;
                    dr.Failed  = currentValue.failed;
                    dr.FailedMessage = currentValue.failedMessage;
                    dr.OperationType = currentValue.importOperation.ToString();
                    this.dataSetImportReport.ImportResult.Rows.Add(dr);
                    setCounts(dr);
                }
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
                default:
                    break;
            }
        }

        private void FormImportReport_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = this.dataSetImportReport.ImportResult;
            //lblTotalValue.Text = total.ToString();
            lblInsertPassedValue.Text = insertPassed.ToString();
            lblInsertFailedValue.Text = insertFailed.ToString();
            lblUpdatePassedValue.Text = updatePassed.ToString();
            lblUpdatedFailedValue.Text = updateFailed.ToString();
        }
    }
}
