using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using Phytel.API.DataDomain.ASE.Common.Enums;
using Phytel.API.DataDomain.ASE.DTO.Message;
using Phytel.ASE.Core;
using Phytel.Services;

namespace Phytel.ASEProcessor
{
    public class ProcessAuditLog : QueueProcessBase
    {
        private string _connectionString = string.Empty;
        private string _dbProvider = string.Empty;
        private XmlDocument _bodyDom = null;

        /* Audit Attributes */
        private string _type = string.Empty;
        List<string> _dbAuditTypes = new List<string>();
        private string _validatedAuditType = string.Empty;
        private Guid _userId = Guid.Empty;
        private Guid _impersonatingUserId = Guid.Empty;
        private DateTime _eventDateTime = DateTime.Now;
        private string _sourcePage = string.Empty;
        private string _sourceIP = string.Empty;
        private string _browser = string.Empty;
        private string _sessionId = string.Empty;
        private int _contractID = 0;

        private Guid _editedUserId = Guid.Empty;
        private string _enteredUserName = string.Empty;
        private string _searchText = string.Empty;
        private List<string> _patients = new List<string>();
        private XmlNodeList _patientIDNodeList = null;
        private string _landingPage = string.Empty;
        private XmlNode _message = null;
        private string _tOSVersion = string.Empty;
        private string _notificationTotal = string.Empty;
        private string _patientSearchData = string.Empty;
        private int _patientSummaryPatientId = 0;
        private string _downloadedReport = string.Empty;

        private string _DBConnName = "Phytel";

        /*
        *  <Phytel.ASE.Process>
        *      <ProcessConfiguration>
        *          <PhytelServicesConnName>Phytel</PhytelServicesConnName>
        *      </ProcessConfiguration>
        *  </Phytel.ASE.Process>
        */

        public override void Execute(QueueMessage queueMessage)
        {
            _bodyDom = new XmlDocument();
            _bodyDom.LoadXml(queueMessage.Body);

            if(base.Configuration.SelectSingleNode("//Phytel.ASE.Process/ProcessConfiguration/PhytelServicesConnName") != null)
                _DBConnName = base.Configuration.SelectSingleNode("//Phytel.ASE.Process/ProcessConfiguration/PhytelServicesConnName").InnerText;

            SetupBaseProperties();
            InitializeAuditTypeList();
            WriteAuditLog();
        }

        private void InitializeAuditTypeList()
        {
            // get audit type from db and assign it
            DataSet ds = SQLDataService.Instance.GetDataSet(_DBConnName, false, "spPhy_GetAuditTypes", new ParameterCollection());
         
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _dbAuditTypes.Add(dr["Name"].ToString());
            }
        }

        public void WriteAuditLog()
        {
            CheckAuditTypeValidity();

            if (_validatedAuditType == "PageView")
            {
                //Call stored procedure for AuditView
                Phytel.Services.ParameterCollection phyParamsAudView = new Phytel.Services.ParameterCollection();
                AddEssentialParams(ref phyParamsAudView);
                Phytel.Services.SQLDataService.Instance.ExecuteProcedure(_DBConnName, false, "spPhy_LogAuditView", phyParamsAudView);

            }
            else if (_validatedAuditType == "ErrorMessage" || _validatedAuditType == "SystemError")
            {
                string _errorid = (_message != null ? _message.SelectSingleNode("Id").InnerText : "0");
                string _errorText = (_message != null ? _message.SelectSingleNode("Text").InnerText : "Unknown");
                string _source = (_message != null ? _message.SelectSingleNode("Source").InnerText : "Unknown");
                string _stackT = (_message != null ? _message.SelectSingleNode("StackTrace").InnerText : "Unknown");

                Phytel.Services.ParameterCollection phyParamsError = new Phytel.Services.ParameterCollection();
                AddEssentialParams(ref phyParamsError);

                phyParamsError.Add(new Parameter("@AuditType", _type, SqlDbType.VarChar, ParameterDirection.Input, _type.Length));
                phyParamsError.Add(new Parameter("@ErrorId", _errorid, SqlDbType.VarChar, ParameterDirection.Input, 20));
                phyParamsError.Add(new Parameter("@ErrorText", _errorText, SqlDbType.VarChar, ParameterDirection.Input, 20));
                phyParamsError.Add(new Parameter("@Source", _source, SqlDbType.VarChar, ParameterDirection.Input, _source.Length));
                phyParamsError.Add(new Parameter("@StackTrace", _stackT, SqlDbType.VarChar, ParameterDirection.Input, _stackT.Length));

                //Call stored procedure for AuditError
                Phytel.Services.SQLDataService.Instance.ExecuteProcedure(_DBConnName, false, "spPhy_LogAuditError", phyParamsError);

                if (_validatedAuditType == "SystemError")
                {
                    base.LogError(_errorText,LogErrorCode.Error, LogErrorSeverity.High, _stackT);
                }
            }
            else
            {
                //Call stored procedure for AuditAction
                int tosVersion = 0;
                int notificationTotal = 0;
                bool isNumeric = false;

                string patients = string.Empty;

                if (_patientIDNodeList != null && _patientIDNodeList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (XmlNode n in _patientIDNodeList)
                    {
                        sb.Append(n.InnerText + "|");
                    }
                    patients = sb.ToString();
                }
                else if (_patientSummaryPatientId != 0)
                    patients = _patientSummaryPatientId.ToString();


                if (!string.IsNullOrEmpty(_tOSVersion))
                    isNumeric = int.TryParse(_tOSVersion, out tosVersion);

                if (!string.IsNullOrEmpty(_notificationTotal))
                    isNumeric = int.TryParse(_notificationTotal, out notificationTotal);

                Phytel.Services.ParameterCollection phyParams = new Phytel.Services.ParameterCollection();
                AddEssentialParams(ref phyParams);

                phyParams.Add(new Parameter("@AuditType", _type, SqlDbType.VarChar, ParameterDirection.Input, 50));
                phyParams.Add(new Parameter("@EditedUserId", _editedUserId, SqlDbType.UniqueIdentifier, ParameterDirection.Input, 100));
                phyParams.Add(new Parameter("@EnteredUserName", _enteredUserName, SqlDbType.VarChar, ParameterDirection.Input, 50));
                phyParams.Add(new Parameter("@SearchText", _searchText, SqlDbType.VarChar, ParameterDirection.Input, -1));
                phyParams.Add(new Parameter("@LandingPage", _landingPage, SqlDbType.VarChar, ParameterDirection.Input, 50));
                phyParams.Add(new Parameter("@TOSVersion", tosVersion, SqlDbType.Int, ParameterDirection.Input, 4));
                phyParams.Add(new Parameter("@NotificationTotal", notificationTotal, SqlDbType.Int, ParameterDirection.Input, 4));
                phyParams.Add(new Parameter("@DownloadedReport", _downloadedReport, SqlDbType.VarChar, ParameterDirection.Input, 100));
                phyParams.Add(new Parameter("@Patients", patients, SqlDbType.VarChar, ParameterDirection.Input, -1));

                Phytel.Services.SQLDataService.Instance.ExecuteProcedure(_DBConnName, false, "spPhy_LogAuditAction", phyParams);
            }
        }

        private void AddEssentialParams(ref ParameterCollection phyParams)
        {
            phyParams.Add(new Parameter("@UserId", _userId, SqlDbType.UniqueIdentifier, ParameterDirection.Input, 100));
            phyParams.Add(new Parameter("@ImpersonatingUserId", _impersonatingUserId, SqlDbType.UniqueIdentifier, ParameterDirection.Input, 100));
            phyParams.Add(new Parameter("@DateTimeStamp", _eventDateTime, SqlDbType.DateTime, ParameterDirection.Input, 10));
            phyParams.Add(new Parameter("@SourcePage", _sourcePage, SqlDbType.VarChar, ParameterDirection.Input, 50));
            phyParams.Add(new Parameter("@SourceIp", _sourceIP, SqlDbType.VarChar, ParameterDirection.Input, 15));
            phyParams.Add(new Parameter("@Browser", _browser, SqlDbType.VarChar, ParameterDirection.Input, 50));
            phyParams.Add(new Parameter("@SessionId", _sessionId, SqlDbType.VarChar, ParameterDirection.Input, 100));
            phyParams.Add(new Parameter("@ContractId", _contractID, SqlDbType.Int, ParameterDirection.Input, 4));
        }

        private void CheckAuditTypeValidity()
        {
            if (_dbAuditTypes.Contains(_type))
            {
                _validatedAuditType = _type;
            }
            else
            {
                //Write the _type to the audittype database via a stored procedure. then add to _dbAuditTypes
                AddAuditType(_type);              

                _validatedAuditType = _type;
            }
        }

        private void AddAuditType(string type)
        {
            Phytel.Services.ParameterCollection phyParams = new Phytel.Services.ParameterCollection();
            
            phyParams.Add(new Parameter("@TypeName", type, SqlDbType.VarChar, ParameterDirection.Input, 50));
            Phytel.Services.SQLDataService.Instance.ExecuteProcedure(_DBConnName, false, "spPhy_InsertAuditType", phyParams);
            
            _dbAuditTypes.Add(_type);
        }

        private void SetupBaseProperties()
        {
            string xPath = "//AuditData/{0}";

            /* These should ALWAYS be present */
            _type = GetDomValue("Type");
            _userId = new Guid(GetDomValue("UserId"));
            _impersonatingUserId = new Guid(GetDomValue("ImpersonatingUserId"));
            _eventDateTime = DateTime.Parse(GetDomValue("EventDateTime"));
            _sourcePage = GetDomValue("SourcePage");
            _sourceIP = GetDomValue("SourceIP");
            _browser = GetDomValue("Browser");
            _sessionId = GetDomValue("SessionId");
            _contractID = int.Parse(GetDomValue("ContractID"));

            /* These are optional */
            _editedUserId = new Guid(GetDomValue("EditedUserId"));
            _enteredUserName = GetDomValue("EnteredUserName");
            _searchText = GetDomValue("SearchText");
            _landingPage = GetDomValue("LandingPage");
            _tOSVersion = GetDomValue("TOSVersion");
            _notificationTotal = GetDomValue("NotificationTotal");
            _downloadedReport = GetDomValue("DownloadedReport");

            _patientIDNodeList = _bodyDom.SelectNodes(string.Format(xPath, "PatientIDList/PatientID"));
            _message = _bodyDom.SelectSingleNode(string.Format(xPath, "Message"));
            //_patientSearchData = GetDomValue("PatientSearchData");
            //_patientSummaryPatientId = int.Parse(GetDomValue("PatientSummaryPatientId"));
        }

        private string GetDomValue(string fieldName)
        {
            string xPath = "//AuditData/{0}";

            XmlNode xnode = _bodyDom.SelectSingleNode(string.Format(xPath, fieldName));

            if (xnode != null)
                return xnode.InnerText;
            else
                return string.Empty;
        }
    }
}
