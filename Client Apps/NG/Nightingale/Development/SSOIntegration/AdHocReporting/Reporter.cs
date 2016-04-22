using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.DataDomain.Program.DTO;

namespace AdHocReporting
{
    public class Reporter
    {
        public delegate void StatusUpdateHandler(string updateMessage);

        private List<IdNamePair> modesLookUp = new List<IdNamePair>();
        private List<CommTypeData> typesLookUp = new List<CommTypeData>();
        private List<StateData> statesLookUp = new List<StateData>();
        private List<TimeZoneData> zonesLookUp = new List<TimeZoneData>();
        private TimeZoneData zoneDefault = new TimeZoneData();
        private List<IdNamePair> careMemberLookUp = new List<IdNamePair>();

        public string DataDomainURL { get; set; }
        public string ContractNumber { get; set; }
        public string UserId { get; set; }
        public string ConfigFile { get; set; }
        public string OutputPath { get; set; }

        public event StatusUpdateHandler StatusUpdate;
        protected virtual void OnStatusUpdate(string updateMessage)
        {
            if(StatusUpdate != null)
                StatusUpdate(updateMessage);
        }

        public void BuildProgramReport()
        {
            try
            {
                OnStatusUpdate("Retrieving Individuals, please wait...");
                //Go get all patients first
                GetCohortPatientsDataResponse patients = GetPatients("53237514072ef709d84efe9d");
                GetAllCareManagersDataResponse careManagers = GetAllCareManagers();

                string fileData = BuildHeader("ProgramReport");

                foreach (PatientData patient in patients.CohortPatients)
                {
                    OnStatusUpdate(string.Format("Building report for '{0} {1}'", patient.FirstName, patient.LastName));

                    GetPatientDataResponse patientDetail = GetPatientDetail(patient.ID);
                    GetContactByPatientIdDataResponse patientContactCard = GetPatientContactCardDetail(patient.ID);

                    GetPatientSystemDataResponse patientSystem = null;
                    if (string.IsNullOrEmpty(patientDetail.Patient.DisplayPatientSystemId) == false)
                        patientSystem = GetPatientSystem(patientDetail.Patient.DisplayPatientSystemId);

                    GetAllCareMembersDataResponse careMember = GetCareManager(patient.ID);

                    ContactData careManagerContactCard = null;
                    if (careMember.CareMembers != null)
                    {
                        CareMemberData careManager = careMember.CareMembers.Where(x => x.Primary == true).FirstOrDefault();
                        if (careManager != null)
                            careManagerContactCard = careManagers.Contacts.Where(x => x.ContactId == careManager.ContactId).FirstOrDefault();
                    }

                    GetPatientProgramsResponse patPrograms = GetPatientPrograms(patient.ID);

                    if (patPrograms.programs != null && patPrograms.programs.Count > 0)
                    {
                        foreach (ProgramInfo p in patPrograms.programs)
                        {
                            OnStatusUpdate(string.Format("Writing program report for '{0} {1}' - '{2}'", patient.FirstName, patient.LastName, p.Name));

                            GetProgramDetailsSummaryResponse response = GetProgramDetailsSummary(patient.ID, p.Id);
                            if (response.Program != null)
                                fileData += WriteData("ProgramReport", patientDetail.Patient, patientContactCard.Contact, patientSystem.PatientSystem, careManagerContactCard, response.Program);
                        }
                    }
                    else
                        fileData += WriteData("ProgramReport", patientDetail.Patient, patientContactCard.Contact, patientSystem.PatientSystem, careManagerContactCard, null);   
                }
                OnStatusUpdate("Writing report file, please wait...");
                string fileName = string.Format("{0}ProgramReport_{1}.csv", OutputPath, DateTime.Now.ToShortDateString()).Replace("/", "_");
                try
                {
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                }
                catch { }

                File.WriteAllText(fileName, fileData);
                OnStatusUpdate("File written successfully, report complete.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void BuildActionReport()
        {
            try
            {
                OnStatusUpdate("Retrieving Individuals, please wait...");

                //Go get all patients first
                GetCohortPatientsDataResponse patients = GetPatients("53237514072ef709d84efe9d");
                GetAllCareManagersDataResponse careManagers = GetAllCareManagers();

                string fileData = BuildHeader("ActionCounts");

                foreach (PatientData patient in patients.CohortPatients)
                {
                    OnStatusUpdate(string.Format("Building report for '{0} {1}'", patient.FirstName, patient.LastName));

                    GetPatientDataResponse patientDetail = GetPatientDetail(patient.ID);

                    GetAllCareMembersDataResponse careMember = GetCareManager(patient.ID);

                    ContactData careManagerContactCard = null;
                    string careManagerName = string.Empty;
                    if (careMember.CareMembers != null)
                    {
                        CareMemberData careManager = careMember.CareMembers.Where(x => x.Primary == true).FirstOrDefault();
                        if (careManager != null)
                            careManagerContactCard = careManagers.Contacts.Where(x => x.ContactId == careManager.ContactId).FirstOrDefault();
                        careManagerName = careManagerContactCard.PreferredName;
                    }

                    GetPatientProgramsResponse patPrograms = GetPatientPrograms(patient.ID);

                    if (patPrograms.programs != null && patPrograms.programs.Count > 0)
                    {
                        foreach (ProgramInfo p in patPrograms.programs)
                        {
                            OnStatusUpdate(string.Format("Writing action report for '{0} {1}' - '{2}'", patient.FirstName, patient.LastName, p.Name));

                            GetProgramDetailsSummaryResponse response = GetProgramDetailsSummary(patient.ID, p.Id);
                            if (response.Program != null)
                            {

                                string patientName = string.Format("{0} {1}", patient.FirstName, patient.LastName);

                                foreach (ModuleDetail module in response.Program.Modules)
                                {
                                    fileData += careManagerName + ",";
                                    fileData += patientName + ",";
                                    fileData += module.Name + ",";
                                    fileData += module.Actions.Count.ToString() + ",";
                                    fileData += GetSavedActionCount(module, true) + ",";
                                    fileData += GetSavedActionCount(module, false) + Environment.NewLine;
                                }
                            }
                        }
                    }
                }
                OnStatusUpdate("Writing report file, please wait...");
                string fileName = string.Format("{0}ActionCounts_{1}.csv", OutputPath, DateTime.Now.ToShortDateString()).Replace("/", "_");
                try
                {
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                }
                catch { }

                File.WriteAllText(fileName, fileData);
                OnStatusUpdate("File written successfully, report complete.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string BuildHeader(string reportName)
        {
            string returnHeader = string.Empty;

            XmlDocument doc = new XmlDocument();
            doc.Load(ConfigFile);
            XmlNode reportNode = doc.SelectSingleNode(string.Format("//Reports/Report[@name='{0}']/Header", reportName));
            if (reportNode != null)
            {
                XmlNodeList fields = reportNode.SelectNodes("Field");
                foreach (XmlNode field in fields)
                {
                    if (returnHeader != string.Empty)
                        returnHeader += ",";

                    returnHeader += field.InnerText;
                }
            }
            else
            {
                throw new Exception(string.Format("Can't find report in configuration file (ReportConfig.xml): {0}", reportName));
            }
            return returnHeader + Environment.NewLine;
        }

        private XmlNodeList GetBodyFields(string reportName)
        {
            XmlNodeList fields = null;

            XmlDocument doc = new XmlDocument();
            doc.Load(ConfigFile);
            XmlNode reportNode = doc.SelectSingleNode(string.Format("//Reports/Report[@name='{0}']/Body", reportName));
            if (reportNode != null)
            {
                fields = reportNode.SelectNodes("Field");
            }
            else
            {
                throw new Exception(string.Format("Can't find report in configuration file (ReportConfig.xml): {0}", reportName));
            }
            return fields;
        }

        private string GetSimpleField(XmlNode bodyField, PatientData patient, ContactData contact, PatientSystemData patientSystem, ContactData careManager, ProgramDetail program)
        {
            string returnVal = string.Empty;

            object obj = null;

            switch (bodyField.Attributes.GetNamedItem("objectname").Value.ToUpper().Trim())
            {
                case "PATIENT":
                    obj = patient;
                    break;
                case "CONTACT":
                    obj = contact;
                    break;
                case "PATIENTSYSTEM":
                    obj = patientSystem;
                    break;
                case "CAREMANAGER":
                    obj = careManager;
                    break;
                case "PROGRAM":
                    obj = program;
                    break;
                default:
                    throw new Exception("Invalid ObjectName in Configuration: " + bodyField.Attributes.GetNamedItem("objectname").Value);
            }
            if (obj != null)
            {
                PropertyInfo propInfo = obj.GetType().GetProperty(bodyField.Attributes.GetNamedItem("fieldname").Value);
                returnVal = (propInfo.GetValue(obj) != null ? propInfo.GetValue(obj).ToString() : string.Empty);
            }
            return returnVal;
        }

        private string GetLookupField(XmlNode bodyField, PatientData patient, ContactData contact, PatientSystemData patientSystem, ContactData careManager, ProgramDetail program)
        {
            string returnVal = string.Empty;

            string simpleFieldValue = GetSimpleField(bodyField, patient, contact, patientSystem, careManager, program);

            if (simpleFieldValue != string.Empty)
            {
                switch (bodyField.Attributes.GetNamedItem("lookup").Value.ToUpper().Trim())
                {
                    case "TIMEZONEDATA":
                        TimeZoneData timeZone = zonesLookUp.Where(x => x.Id == simpleFieldValue).FirstOrDefault();
                        if (timeZone != null)
                        {
                            PropertyInfo propInfo = timeZone.GetType().GetProperty(bodyField.Attributes.GetNamedItem("lookupfield").Value);
                            returnVal = (propInfo.GetValue(timeZone) != null ? propInfo.GetValue(timeZone).ToString() : string.Empty);
                        }
                        else if (zoneDefault != null)
                        {
                            PropertyInfo propInfo = zoneDefault.GetType().GetProperty(bodyField.Attributes.GetNamedItem("lookupfield").Value);
                            returnVal = (propInfo.GetValue(zoneDefault) != null ? propInfo.GetValue(zoneDefault).ToString() : string.Empty);
                        }
                        break;
                    case "COMMTYPEDATA":
                        CommTypeData commType = typesLookUp.Where(x => x.Id == simpleFieldValue).FirstOrDefault();
                        if (commType != null)
                        {
                            PropertyInfo propInfo = commType.GetType().GetProperty(bodyField.Attributes.GetNamedItem("lookupfield").Value);
                            returnVal = (propInfo.GetValue(commType) != null ? propInfo.GetValue(commType).ToString() : string.Empty);
                        }
                        break;
                    case "STATEDATA":
                        StateData stateData = statesLookUp.Where(x => x.Id == simpleFieldValue).FirstOrDefault();
                        if (stateData != null)
                        {
                            PropertyInfo propInfo = stateData.GetType().GetProperty(bodyField.Attributes.GetNamedItem("lookupfield").Value);
                            returnVal = (propInfo.GetValue(stateData) != null ? propInfo.GetValue(stateData).ToString() : string.Empty);
                        }
                        break;
                }
            }
            return returnVal;
        }

        private string GetLookupField(XmlNode bodyField, string simpleFieldValue)
        {
            string returnVal = string.Empty;

            if (simpleFieldValue != string.Empty)
            {
                switch (bodyField.Attributes.GetNamedItem("lookup").Value.ToUpper().Trim())
                {
                    case "TIMEZONEDATA":
                        TimeZoneData timeZone = zonesLookUp.Where(x => x.Id == simpleFieldValue).FirstOrDefault();
                        if (timeZone != null)
                        {
                            PropertyInfo propInfo = timeZone.GetType().GetProperty(bodyField.Attributes.GetNamedItem("lookupfield").Value);
                            returnVal = (propInfo.GetValue(timeZone) != null ? propInfo.GetValue(timeZone).ToString() : string.Empty);
                        }
                        else if (zoneDefault != null)
                        {
                            PropertyInfo propInfo = zoneDefault.GetType().GetProperty(bodyField.Attributes.GetNamedItem("lookupfield").Value);
                            returnVal = (propInfo.GetValue(zoneDefault) != null ? propInfo.GetValue(zoneDefault).ToString() : string.Empty);
                        }
                        break;
                    case "COMMTYPEDATA":
                        CommTypeData commType = typesLookUp.Where(x => x.Id == simpleFieldValue).FirstOrDefault();
                        if (commType != null)
                        {
                            PropertyInfo propInfo = commType.GetType().GetProperty(bodyField.Attributes.GetNamedItem("lookupfield").Value);
                            returnVal = (propInfo.GetValue(commType) != null ? propInfo.GetValue(commType).ToString() : string.Empty);
                        }
                        break;
                    case "STATEDATA":
                        StateData stateData = statesLookUp.Where(x => x.Id == simpleFieldValue).FirstOrDefault();
                        if (stateData != null)
                        {
                            PropertyInfo propInfo = stateData.GetType().GetProperty(bodyField.Attributes.GetNamedItem("lookupfield").Value);
                            returnVal = (propInfo.GetValue(stateData) != null ? propInfo.GetValue(stateData).ToString() : string.Empty);
                        }
                        break;
                }
            }
            return returnVal;
        }

        private string GetArrayField(XmlNode bodyField, PatientData patient, ContactData contact, PatientSystemData patientSystem, ContactData careManager, ProgramDetail program)
        {
            string returnVal = string.Empty;

            object obj = null;

            switch (bodyField.Attributes.GetNamedItem("objectname").Value.ToUpper().Trim())
            {
                case "PATIENT":
                    obj = patient;
                    break;
                case "CONTACT":
                    obj = contact;
                    break;
                case "PATIENTSYSTEM":
                    obj = patientSystem;
                    break;
                case "CAREMANAGER":
                    obj = careManager;
                    break;
                case "PROGRAM":
                    obj = program;
                    break;
                default:
                    throw new Exception("Invalid ObjectName in Configuration: " + bodyField.Attributes.GetNamedItem("objectname").Value);
            }
            if (obj != null)
            {
                int arrayItem = int.Parse(bodyField.Attributes.GetNamedItem("arrayitem").Value);
                string arrayType = bodyField.Attributes.GetNamedItem("arraytype").Value;
                string arrayField = bodyField.Attributes.GetNamedItem("arrayfield").Value;

                PropertyInfo propInfo = obj.GetType().GetProperty(bodyField.Attributes.GetNamedItem("fieldname").Value);
                object array = propInfo.GetValue(obj);
                if (array != null)
                    returnVal = GetArrayObjectFieldValue(array, arrayField, arrayItem, arrayType);
            }
            return returnVal;
        }

        private string GetArrayObjectFieldValue(object arrayObject, string arrayField, int arrayItem, string arrayType)
        {
            string returnVal = string.Empty;

            switch (arrayType.ToUpper().Trim())
            {
                case "PHONEDATA":
                    List<PhoneData> pd = arrayObject as List<PhoneData>;
                    if (pd.Count > arrayItem)
                    {
                        PhoneData phoneD = pd[arrayItem];
                        returnVal = (phoneD.GetType().GetProperty(arrayField).GetValue(phoneD) != null ? phoneD.GetType().GetProperty(arrayField).GetValue(phoneD).ToString() : string.Empty);
                    }
                    break;
                case "EMAILDATA":
                    List<EmailData> ed = arrayObject as List<EmailData>;
                    if (ed.Count > arrayItem)
                    {
                        EmailData emailD = ed[arrayItem];
                        returnVal = (emailD.GetType().GetProperty(arrayField).GetValue(emailD) != null ? emailD.GetType().GetProperty(arrayField).GetValue(emailD).ToString() : string.Empty);
                    }
                    break;
                case "ADDRESSDATA":
                    List<AddressData> ad = arrayObject as List<AddressData>;
                    if (ad.Count > arrayItem)
                    {
                        AddressData addressD = ad[arrayItem];
                        returnVal = (addressD.GetType().GetProperty(arrayField).GetValue(addressD) != null ? addressD.GetType().GetProperty(arrayField).GetValue(addressD).ToString() : string.Empty);
                    }
                    break;
            }
            return returnVal;
        }

        private string GetArrayLookupField(XmlNode bodyField, PatientData patient, ContactData contact, PatientSystemData patientSystem, ContactData careManager, ProgramDetail program)
        {
            string returnVal = string.Empty;

            object obj = null;

            switch (bodyField.Attributes.GetNamedItem("objectname").Value.ToUpper().Trim())
            {
                case "PATIENT":
                    obj = patient;
                    break;
                case "CONTACT":
                    obj = contact;
                    break;
                case "PATIENTSYSTEM":
                    obj = patientSystem;
                    break;
                case "CAREMANAGER":
                    obj = careManager;
                    break;
                case "PROGRAM":
                    obj = program;
                    break;
                default:
                    throw new Exception("Invalid ObjectName in Configuration: " + bodyField.Attributes.GetNamedItem("objectname").Value);
            }
            if (obj != null)
            {
                int arrayItem = int.Parse(bodyField.Attributes.GetNamedItem("arrayitem").Value);
                string arrayType = bodyField.Attributes.GetNamedItem("arraytype").Value;
                string arrayField = bodyField.Attributes.GetNamedItem("arrayfield").Value;

                PropertyInfo propInfo = obj.GetType().GetProperty(bodyField.Attributes.GetNamedItem("fieldname").Value);
                object array = propInfo.GetValue(obj);
                if (array != null)
                {
                    string arrayValue = GetArrayObjectFieldValue(array, arrayField, arrayItem, arrayType);
                    returnVal = GetLookupField(bodyField, arrayValue);
                }
            }
            return returnVal;
        }

        private string GetResponseField(XmlNode bodyField, PatientData patient, ContactData contact, PatientSystemData patientSystem, ContactData careManager, ProgramDetail program)
        {
            string moduleId = bodyField.Attributes.GetNamedItem("moduleid").Value.Trim();
            string actionId = bodyField.Attributes.GetNamedItem("actionid").Value.Trim();
            string stepId = bodyField.Attributes.GetNamedItem("stepid").Value.Trim();
            string responseId = bodyField.Attributes.GetNamedItem("responseid").Value.Trim();
            // Response Text was added because we cannot do a lookup on responseId, as patientresponses collection doesnot contain a reference to Source Id ("srcid") from Responses collection.
            string responseText = bodyField.Attributes.GetNamedItem("responsetxt").Value.Trim();
            bool isDate = bool.Parse(bodyField.Attributes.GetNamedItem("isdate").Value.Trim());

            return GetValue(program, moduleId, actionId, stepId, responseId,responseText, isDate);
        }

        private string WriteData(string reportName, PatientData patient, ContactData contact, PatientSystemData patientSystem, ContactData careManager, ProgramDetail program)
        {
            string outputLine = string.Empty;

            XmlNodeList bodyFields = GetBodyFields(reportName);
            foreach (XmlNode bodyField in bodyFields)
            {
                if (outputLine != string.Empty)
                    outputLine += ",";

                switch (bodyField.Attributes.GetNamedItem("type").Value.ToUpper().Trim())
                {
                    case "SIMPLE":
                        outputLine += "\"" + GetSimpleField(bodyField, patient, contact, patientSystem, careManager, program) + "\"";
                        break;
                    case "LOOKUP":
                        outputLine += "\"" + GetLookupField(bodyField, patient, contact, patientSystem, careManager, program) + "\"";
                        break;
                    case "ARRAY":
                        outputLine += "\"" + GetArrayField(bodyField, patient, contact, patientSystem, careManager, program) + "\"";
                        break;
                    case "ARRAYLOOKUP":
                        outputLine += "\"" + GetArrayLookupField(bodyField, patient, contact, patientSystem, careManager, program) + "\"";
                        break;
                    case "RESPONSE":
                        outputLine += "\"" + GetResponseField(bodyField, patient, contact, patientSystem, careManager, program) + "\"";
                        break;
                }
            }
            outputLine += Environment.NewLine;

            return outputLine;
        }

        private string GetValue(ProgramDetail program, string moduleId, string actionId, string stepId, string responseId, string responseText, bool isDate)
        {
            string returnValue = string.Empty;
            if (program != null)
            {
                foreach (ModuleDetail module in program.Modules)
                {
                    if ((module.SourceId.Equals(moduleId)) || (moduleId == string.Empty))
                    {
                        foreach (ActionsDetail a in module.Actions)
                        {
                            if ((a.SourceId.Equals(actionId)) || (actionId == string.Empty))
                            {
                                if (a.ElementState == (int)ElementState.Completed || a.ElementState == (int)ElementState.InProgress) 
                                {
                                    foreach (StepsDetail step in a.Steps)
                                    {
                                        if (step.SourceId.Equals(stepId))
                                        {
                                            //If only 1 possible response (Date, text, etc...):  step.Responses[0].Value
                                            switch (step.StepTypeId)
                                            {
                                                case 1: //YesNo = 1,
                                                    foreach (ResponseDetail r in step.Responses)
                                                    {
                                                        if (r.Id == step.SelectedResponseId)
                                                        {
                                                            returnValue = (string.IsNullOrEmpty(r.Text) ? r.Value : r.Text);
                                                            break;
                                                        }
                                                    }
                                                    break;
                                                case 2: //Text = 2,
                                                    returnValue = (string.IsNullOrEmpty(step.Responses[0].Text) ? step.Responses[0].Value : step.Responses[0].Text);
                                                    break;
                                                case 3: //Input = 3,
                                                    if (responseId != string.Empty)
                                                    {
                                                        foreach (ResponseDetail rd in step.Responses)
                                                        {
                                                            if (rd.Id == responseId)
                                                                returnValue = (string.IsNullOrEmpty(rd.Text) ? rd.Value : rd.Text);
                                                        }
                                                    }
                                                    else
                                                        returnValue = (string.IsNullOrEmpty(step.Responses[0].Text) ? step.Responses[0].Value : step.Responses[0].Text);
                                                    break;
                                                case 4: //Single = 4,
                                                    foreach (ResponseDetail r in step.Responses)
                                                    {
                                                        if (r.Id == step.SelectedResponseId)
                                                        {
                                                            returnValue = (string.IsNullOrEmpty(r.Text) ? r.Value : r.Text);
                                                            break;
                                                        }
                                                    }
                                                    break;
                                                case 5: //Multi = 5,
                                                    foreach (ResponseDetail r in step.Responses)
                                                    {
                                                        if (!string.IsNullOrEmpty(r.Value) && bool.Parse(r.Value))
                                                        {
                                                            if (r.Text == responseText)
                                                            {
                                                                returnValue = r.Value.ToString();
                                                            }
                                                        }
                                                    }
                                                    break;
                                                case 6: //Date = 6,
                                                    string datetime = (string.IsNullOrEmpty(step.Responses[0].Text) ? step.Responses[0].Value : step.Responses[0].Text);
                                                    if (!string.IsNullOrEmpty(datetime))
                                                    { 
                                                        DateTime result;
                                                        if (DateTime.TryParse(datetime, out result))
                                                        {
                                                            TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                                                            returnValue = TimeZoneInfo.ConvertTimeFromUtc(result.ToUniversalTime(), estZone).ToString();
                                                        }
                                                    }
                                                    break;
                                                case 7: //Complete = 7,
                                                    break;
                                                case 8: //DateTime = 8,
                                                    returnValue = (string.IsNullOrEmpty(step.Responses[0].Text) ? step.Responses[0].Value : step.Responses[0].Text);
                                                    break;
                                                case 9: //Time = 9,
                                                    returnValue = (string.IsNullOrEmpty(step.Responses[0].Text) ? step.Responses[0].Value : step.Responses[0].Text);
                                                    break;
                                                case 10: //InputMultiline = 10
                                                    returnValue = (string.IsNullOrEmpty(step.Responses[0].Text) ? step.Responses[0].Value : step.Responses[0].Text);
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (isDate && returnValue.Trim() != string.Empty)
                return DateTime.Parse(returnValue).ToString();
            else
                return returnValue;
        }

        private GetAllCareMembersDataResponse GetCareManager(string patientId)
        {
            Uri getUri = new Uri(string.Format("{0}/CareMember/NG/1.0/{1}/Patient/{2}/CareMembers?UserId={3}&format=json",
                                                DataDomainURL, ContractNumber, patientId, UserId));

            HttpClient getContactClient = new HttpClient();

            GetAllCareMembersDataRequest modesRequest = new GetAllCareMembersDataRequest();

            var modesResponse = getContactClient.GetStringAsync(getUri);
            var modesResponseContent = modesResponse.Result;

            string modesResponseString = modesResponseContent;
            GetAllCareMembersDataResponse responseModes = null;

            responseModes = (GetAllCareMembersDataResponse)ServiceStack.Text.JsonSerializer.DeserializeFromString(modesResponseString, typeof(GetAllCareMembersDataResponse));

            return responseModes;
        }

        private GetPatientSystemDataResponse GetPatientSystem(string patientSystemId)
        {
            Uri getUri = new Uri(string.Format("{0}/PatientSystem/NG/1.0/{1}/PatientSystem/{2}?UserId={3}&format=json",
                                                DataDomainURL, ContractNumber, patientSystemId, UserId));

            HttpClient getContactClient = new HttpClient();

            GetPatientSystemDataRequest modesRequest = new GetPatientSystemDataRequest();

            var modesResponse = getContactClient.GetStringAsync(getUri);
            var modesResponseContent = modesResponse.Result;

            string modesResponseString = modesResponseContent;
            GetPatientSystemDataResponse responseModes = null;

            responseModes = (GetPatientSystemDataResponse)ServiceStack.Text.JsonSerializer.DeserializeFromString(modesResponseString, typeof(GetPatientSystemDataResponse));

            return responseModes;
        }

        private GetAllCareManagersDataResponse GetAllCareManagers()
        {
            Uri getUri = new Uri(string.Format("{0}/Contact/NG/1.0/{1}/Contact/CareManagers?UserId={2}&format=json",
                                                DataDomainURL, ContractNumber, UserId));

            HttpClient getContactClient = new HttpClient();

            GetAllCareManagersDataRequest modesRequest = new GetAllCareManagersDataRequest();

            var modesResponse = getContactClient.GetStringAsync(getUri);
            var modesResponseContent = modesResponse.Result;

            string modesResponseString = modesResponseContent;
            GetAllCareManagersDataResponse responseModes = null;

            responseModes = (GetAllCareManagersDataResponse)ServiceStack.Text.JsonSerializer.DeserializeFromString(modesResponseString, typeof(GetAllCareManagersDataResponse));

            return responseModes;
        }

        private GetContactByPatientIdDataResponse GetPatientContactCardDetail(string patientId)
        {
            Uri getUri = new Uri(string.Format("{0}/Contact/NG/1.0/{1}/Patient/{2}/Contact?UserId={3}&format=json",
                                                DataDomainURL, ContractNumber, patientId, UserId));

            HttpClient getContactClient = new HttpClient();

            GetContactByPatientIdDataRequest modesRequest = new GetContactByPatientIdDataRequest();

            var modesResponse = getContactClient.GetStringAsync(getUri);
            var modesResponseContent = modesResponse.Result;

            string modesResponseString = modesResponseContent;
            GetContactByPatientIdDataResponse responseModes = null;

            responseModes = (GetContactByPatientIdDataResponse)ServiceStack.Text.JsonSerializer.DeserializeFromString(modesResponseString, typeof(GetContactByPatientIdDataResponse));

            return responseModes;
        }

        private GetPatientDataResponse GetPatientDetail(string patientId)
        {
            Uri getUri = new Uri(string.Format("{0}/Patient/NG/1.0/{1}/Patient/{2}?UserId={3}&format=json",
                                                DataDomainURL, ContractNumber, patientId, UserId));

            HttpClient getContactClient = new HttpClient();

            GetPatientDataRequest modesRequest = new GetPatientDataRequest();

            var modesResponse = getContactClient.GetStringAsync(getUri);
            var modesResponseContent = modesResponse.Result;

            string modesResponseString = modesResponseContent;
            GetPatientDataResponse responseModes = null;

            responseModes = (GetPatientDataResponse)ServiceStack.Text.JsonSerializer.DeserializeFromString(modesResponseString, typeof(GetPatientDataResponse));

            return responseModes;
        }

        private GetCohortPatientsDataResponse GetPatients(string cohortId)
        {
            Uri getUri = new Uri(string.Format("{0}/Patient/NG/1.0/{1}/CohortPatients/{2}?UserId={3}&format=json",
                                                DataDomainURL, ContractNumber, cohortId, UserId));

            HttpClient getContactClient = new HttpClient();

            GetCohortPatientsDataRequest modesRequest = new GetCohortPatientsDataRequest();

            var modesResponse = getContactClient.GetStringAsync(getUri);
            var modesResponseContent = modesResponse.Result;

            string modesResponseString = modesResponseContent;
            GetCohortPatientsDataResponse responseModes = null;

            responseModes = (GetCohortPatientsDataResponse)ServiceStack.Text.JsonSerializer.DeserializeFromString(modesResponseString, typeof(GetCohortPatientsDataResponse));

            return responseModes;
        }

        private GetPatientProgramsResponse GetPatientPrograms(string patientId)
        {
            Uri getUri = new Uri(string.Format("{0}/Program/NG/1.0/{1}/Patient/{2}/Programs?UserId={3}&format=json",
                                                DataDomainURL, ContractNumber, patientId, UserId));

            HttpClient getContactClient = new HttpClient();

            GetPatientProgramsRequest modesRequest = new GetPatientProgramsRequest();

            var modesResponse = getContactClient.GetStringAsync(getUri);
            var modesResponseContent = modesResponse.Result;

            string modesResponseString = modesResponseContent;
            GetPatientProgramsResponse responseModes = null;

            responseModes = (GetPatientProgramsResponse)ServiceStack.Text.JsonSerializer.DeserializeFromString(modesResponseString, typeof(GetPatientProgramsResponse));

            return responseModes;
        }

        private GetProgramDetailsSummaryResponse GetProgramDetailsSummary(string patientId, string programId)
        {
            Uri getUri = new Uri(string.Format("{0}/Program/NG/1.0/{1}/Patient/{2}/Program/{3}/Details?UserId={4}&format=json",
                                                DataDomainURL, ContractNumber, patientId, programId, UserId));

            HttpClient getContactClient = new HttpClient();

            GetProgramDetailsSummaryRequest modesRequest = new GetProgramDetailsSummaryRequest();

            var modesResponse = getContactClient.GetStringAsync(getUri);
            var modesResponseContent = modesResponse.Result;

            string modesResponseString = modesResponseContent;
            GetProgramDetailsSummaryResponse responseModes = null;

            responseModes = (GetProgramDetailsSummaryResponse)ServiceStack.Text.JsonSerializer.DeserializeFromString(modesResponseString, typeof(GetProgramDetailsSummaryResponse));

            return responseModes;
        }

        public void LoadLookUps()
        {
            //modes
            Uri modesUri = new Uri(string.Format("{0}/LookUp/NG/1.0/InHealth001/commmodes?UserId={1}&format=json",
                                                    DataDomainURL,
                                                    UserId));
            HttpClient modesClient = new HttpClient();

            GetAllCommModesDataRequest modesRequest = new GetAllCommModesDataRequest();

            DataContractJsonSerializer modesJsonSer = new DataContractJsonSerializer(typeof(GetAllCommModesDataRequest));
            MemoryStream modesMs = new MemoryStream();
            modesJsonSer.WriteObject(modesMs, modesRequest);
            modesMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader modesSr = new StreamReader(modesMs);
            StringContent modesContent = new StringContent(modesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var modesResponse = modesClient.GetStringAsync(modesUri);
            var modesResponseContent = modesResponse.Result;

            string modesResponseString = modesResponseContent;
            GetAllCommModesDataResponse responseModes = null;

            using (var modesMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(modesResponseString)))
            {
                var modesSerializer = new DataContractJsonSerializer(typeof(GetAllCommModesDataResponse));
                responseModes = (GetAllCommModesDataResponse)modesSerializer.ReadObject(modesMsResponse);
            }
            modesLookUp = responseModes.CommModes;

            //types
            Uri typesUri = new Uri(string.Format("{0}/LookUp/NG/1.0/InHealth001/commtypes?UserId={1}&format=json",
                                                    DataDomainURL,
                                                    UserId));
            HttpClient typesClient = new HttpClient();

            GetAllCommTypesDataRequest typesRequest = new GetAllCommTypesDataRequest();

            DataContractJsonSerializer typesJsonSer = new DataContractJsonSerializer(typeof(GetAllCommTypesDataRequest));
            MemoryStream typesMs = new MemoryStream();
            typesJsonSer.WriteObject(typesMs, typesRequest);
            typesMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader typesSr = new StreamReader(typesMs);
            StringContent typesContent = new StringContent(typesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var typesResponse = typesClient.GetStringAsync(typesUri);
            var typesResponseContent = typesResponse.Result;

            string typesResponseString = typesResponseContent;
            GetAllCommTypesDataResponse responseTypes = null;

            using (var typesMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(typesResponseString)))
            {
                var typesSerializer = new DataContractJsonSerializer(typeof(GetAllCommTypesDataResponse));
                responseTypes = (GetAllCommTypesDataResponse)typesSerializer.ReadObject(typesMsResponse);
            }
            typesLookUp = responseTypes.CommTypes;

            //states
            Uri statesUri = new Uri(string.Format("{0}/LookUp/NG/1.0/InHealth001/states?UserId={1}&format=json",
                                                    DataDomainURL,
                                                    UserId));
            HttpClient statesClient = new HttpClient();

            GetAllStatesDataRequest statesRequest = new GetAllStatesDataRequest();

            DataContractJsonSerializer statesJsonSer = new DataContractJsonSerializer(typeof(GetAllStatesDataRequest));
            MemoryStream statesMs = new MemoryStream();
            statesJsonSer.WriteObject(statesMs, statesRequest);
            statesMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader statesSr = new StreamReader(statesMs);
            StringContent statesContent = new StringContent(statesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var statesResponse = statesClient.GetStringAsync(statesUri);
            var statesResponseContent = statesResponse.Result;

            string statesResponseString = statesResponseContent;
            GetAllStatesDataResponse responseStates = null;

            using (var statesMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(statesResponseString)))
            {
                var statesSerializer = new DataContractJsonSerializer(typeof(GetAllStatesDataResponse));
                responseStates = (GetAllStatesDataResponse)statesSerializer.ReadObject(statesMsResponse);
            }
            statesLookUp = responseStates.States;

            //timezones
            Uri zonesUri = new Uri(string.Format("{0}/LookUp/NG/1.0/InHealth001/timeZones?UserId={1}&format=json",
                                                    DataDomainURL,
                                                    UserId));
            HttpClient zonesClient = new HttpClient();

            GetAllTimeZonesDataRequest zonesRequest = new GetAllTimeZonesDataRequest();

            DataContractJsonSerializer zonesJsonSer = new DataContractJsonSerializer(typeof(GetAllTimeZonesDataRequest));
            MemoryStream zonesMs = new MemoryStream();
            zonesJsonSer.WriteObject(zonesMs, zonesRequest);
            zonesMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader zonesSr = new StreamReader(zonesMs);
            StringContent zonesContent = new StringContent(zonesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var zonesResponse = zonesClient.GetStringAsync(zonesUri);
            var zonesResponseContent = zonesResponse.Result;

            string zonesResponseString = zonesResponseContent;
            GetAllTimeZonesDataResponse responseZones = null;

            using (var zonesMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(zonesResponseString)))
            {
                var zonesSerializer = new DataContractJsonSerializer(typeof(GetAllTimeZonesDataResponse));
                responseZones = (GetAllTimeZonesDataResponse)zonesSerializer.ReadObject(zonesMsResponse);
            }
            zonesLookUp = responseZones.TimeZones;

            //default timezone
            Uri zoneUri = new Uri(string.Format("{0}/LookUp/NG/1.0/InHealth001/TimeZone/Default?UserId={1}&format=json",
                                                    DataDomainURL,
                                                    UserId));
            HttpClient zoneClient = new HttpClient();

            GetTimeZoneDataRequest zoneRequest = new GetTimeZoneDataRequest();

            DataContractJsonSerializer zoneJsonSer = new DataContractJsonSerializer(typeof(GetTimeZoneDataRequest));
            MemoryStream zoneMs = new MemoryStream();
            zoneJsonSer.WriteObject(zoneMs, zoneRequest);
            zoneMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader zoneSr = new StreamReader(zoneMs);
            StringContent zoneContent = new StringContent(zoneSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var zoneResponse = zoneClient.GetStringAsync(zoneUri);
            var zoneResponseContent = zoneResponse.Result;

            string zoneResponseString = zoneResponseContent;
            GetTimeZoneDataResponse responseZone = null;

            using (var zoneMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(zoneResponseString)))
            {
                var zoneSerializer = new DataContractJsonSerializer(typeof(GetTimeZoneDataResponse));
                responseZone = (GetTimeZoneDataResponse)zoneSerializer.ReadObject(zoneMsResponse);
            }
            zoneDefault = responseZone.TimeZone;

            //Care Member
            ///{Context}/{Version}/{txtContract.Text}/Type/{Name}
            Uri careMemberUri = new Uri(string.Format("{0}/LookUp/NG/1.0/InHealth001/Type/CareMemberType?UserId={1}&format=json",
                                                    DataDomainURL,
                                                    UserId));
            HttpClient careMemberClient = new HttpClient();

            GetLookUpsDataRequest careMemberRequest = new GetLookUpsDataRequest();

            DataContractJsonSerializer careMemberJsonSer = new DataContractJsonSerializer(typeof(GetLookUpsDataRequest));
            MemoryStream careMemberMs = new MemoryStream();
            careMemberJsonSer.WriteObject(careMemberMs, careMemberRequest);
            careMemberMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader careMemberSr = new StreamReader(careMemberMs);
            StringContent careMemberContent = new StringContent(careMemberSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var careMemberResponse = careMemberClient.GetStringAsync(careMemberUri);
            var careMemberResponseContent = careMemberResponse.Result;

            string careMemberResponseString = careMemberResponseContent;
            GetLookUpsDataResponse responsecareMember = null;

            using (var careMemberMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(careMemberResponseString)))
            {
                var careMemberSerializer = new DataContractJsonSerializer(typeof(GetLookUpsDataResponse));
                responsecareMember = (GetLookUpsDataResponse)careMemberSerializer.ReadObject(careMemberMsResponse);
            }
            careMemberLookUp = responsecareMember.LookUpsData;
        }

        private int GetSavedActionCount(ModuleDetail module, bool complete)
        {
            int counter = 0;

            foreach (ActionsDetail action in module.Actions)
            {
                if (complete)
                    counter = counter + ((action.ElementState == (int)ElementState.Completed) ? 1 : 0);
                else
                    counter = counter + ((action.ElementState == (int)ElementState.Started || action.ElementState == (int)ElementState.InProgress) ? 1 : 0);
            }
            return counter;
        }
    }
}
