using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestCommService
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            localhost.CallResultsService callService = new TestCommService.localhost.CallResultsService();
            
            localhost.reqObject_Array requestArray = new TestCommService.localhost.reqObject_Array();
            requestArray.gfRequest = new localhost.reqObject[1];

            
                localhost.reqObject  request1 = new localhost.reqObject();
                request1.ActivityID = 5740947;
                request1.CallDistributorTimeOfCall = Convert.ToDateTime("1/27/2011 1:45:04 PM");
                request1.CallDuration = 117094;
                request1.CallID = "129615738450884438ELMC0012194363";
                request1.CommEventID = 99099;
                request1.ContractID = "ELMC001";
                request1.FacilityID = 5939;
                request1.CommDialerIP  = "10.10.246.94";
                request1.CommRecordingServerIP = "10.10.246.94";
                request1.CommApplicationURL = "http://10.10.246.94:18080/Phytel-Voice/outbound.jsp?callAlertId=129615738450884438ELMC001";
                request1.CommMediaServerIP = "10.10.246.94";
                //request1.FileLocation = "\\AMG001\\7_6_2010\\CallHour_15\\";
                //request1.FileName = "Recall-1237047-110917-100126-936122";
                //request1.HangupLocale = "";
                //request.ProviderID = i;
                request1.Recording = false;
                request1.ResultCode = "1";
                request1.ResultStatus = "UNCNF";
                request1.ScheduleID = 97206;
                request1.SendID = 84438;
                //request1.LanguageResultsCode = "EN";
                requestArray.gfRequest[0] = request1;

                
            

            localhost.respObject_Array results;
            try
            {
                results = callService.sendOCQEResults(requestArray);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
