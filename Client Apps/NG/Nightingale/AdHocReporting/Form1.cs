using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.Program.DTO;
using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.DataDomain.CareMember.DTO;
using System.Collections.Generic;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.LookUp.DTO;
using System.Configuration;
using System.Xml;
using System.Reflection;

namespace AdHocReporting
{
    public partial class Form1 : Form
    {
        Reporter _reporter = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _reporter = new Reporter();

            _reporter.DataDomainURL = ConfigurationManager.AppSettings.Get("URL").ToString();
            _reporter.ContractNumber = ConfigurationManager.AppSettings.Get("Contract").ToString();
            _reporter.UserId = ConfigurationManager.AppSettings.Get("UserId").ToString();
            _reporter.ConfigFile = ConfigurationManager.AppSettings.Get("ConfigFile").ToString();
            _reporter.OutputPath = ConfigurationManager.AppSettings.Get("OutputPath").ToString();
            if (_reporter.OutputPath.EndsWith(@"\") == false)
                _reporter.OutputPath += @"\";

            _reporter.LoadLookUps();

            _reporter.StatusUpdate += _reporter_StatusUpdate;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                _reporter.BuildProgramReport();
            }
            catch(Exception ex)
            {
                UpdateStatus(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                _reporter.BuildActionReport();
            }
            catch(Exception ex)
            {
                UpdateStatus(ex.Message);
            }
        }

        void _reporter_StatusUpdate(string updateMessage)
        {
            UpdateStatus(updateMessage);
        }

        private void UpdateStatus(string updateMessage)
        {
            this.lblStatus.Text = updateMessage;
            this.lblStatus.Refresh();
            Application.DoEvents();
        }

    }
}
