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
        //private DataSetImportReport dsImport = new DataSetImportReport();
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
                    dr.pid = key;
                    dr.DOB = currentData[key].patientData.DOB;
                    dr.FirstName = currentData[key].patientData.FirstName;
                    dr.LastName = currentData[key].patientData.LastName;
                    dr.Failed  = currentData[key].failed;
                    dr.FailedMessage = currentData[key].failedMessage;
                    this.dataSetImportReport.ImportResult.Rows.Add(dr);
                }
            }
            
        }

        private void FormImportReport_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = this.dataSetImportReport.ImportResult;            
        }
    }
}
