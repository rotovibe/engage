using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace Phytel.Services.Communication
{
    public class TemplateUtilities : ITemplateUtilities
    {
        public TemplateUtilities(){ }
        
        public string GetModeSpecificTag(string originalTag, string mode)
        {
            string modeSpecificTag = string.Empty;

            modeSpecificTag = originalTag.Replace(XMLFields.ModePlaceHolder, mode);

            return modeSpecificTag;
        }

        public string ProperCase(string input)
        {
            string pattern = @"\w+|\W+";
            string result = "";
            Boolean capitalizeNext = true;

            foreach (Match m in Regex.Matches(input, pattern))
            {
                // get the matched string
                string x = m.ToString().ToLower();

                // if the first char is lower case
                if (char.IsLower(x[0]) && capitalizeNext)
                {
                    // capitalize it
                    x = char.ToUpper(x[0]) + x.Substring(1, x.Length - 1);
                }

                // Check if the word starts with Mc
                //if (x[0] == 'M' && x[1] == 'c' && !String.IsNullOrEmpty(x[2].ToString()))
                if (x[0] == 'M' && x.Length > 1 && x[1] == 'c' && !String.IsNullOrEmpty(x[2].ToString()))
                {
                    // Capitalize the letter after Mc
                    x = "Mc" + char.ToUpper(x[2]) + x.Substring(3, x.Length - 3);
                }

                if (capitalizeNext == false)
                    capitalizeNext = true;

                // if the apostrophe is at the end i.e. Andrew's
                // then do not capitalize the next letter
                if (x[0].ToString() == "'" && m.NextMatch().ToString().Length == 1)
                {
                    capitalizeNext = false;
                }

                // collect all text
                result += x;
            }
            return result;
        }

        public Hashtable AddMissingObjects(Hashtable missingObjects, string missingObjString)
        {
            if (!missingObjects.ContainsValue(missingObjString))
            {
                missingObjects.Add(missingObjects.Count + 1, missingObjString);
            }
            return missingObjects;
        }

        public void SetXMlNodeInnerText(XmlNode originalNode, string innerText)
        {
            if (originalNode == null) return;
            var result = HandleXMlSpecialCharacters(innerText);
            originalNode.InnerText = result;
        }

        public void SetCDATAXMlNodeInnerText(XmlNode node, string innerText, XmlDocument xmlDoc)
        {
            if (node != null)
            {
                XmlAttribute isCDATAAttr = node.Attributes["IsCDATA"];
                if (isCDATAAttr != null && isCDATAAttr.Value.ToString().ToLower() == "true")
                {
                    XmlCDataSection cdata = xmlDoc.CreateCDataSection(innerText);
                    node.AppendChild(cdata);
                }
                else
                {
                    HandleXMlSpecialCharacters(innerText);
                    node.InnerText = innerText;
                }
            }
        }

        public string HandleXMlSpecialCharacters(string innerText)
        {
            string result = string.Empty;
            if (!String.IsNullOrEmpty(innerText) && !String.IsNullOrEmpty(innerText.Trim()))
            {
                //1.& - &amp; 2.< - &lt; 3.> - &gt; 4." - &quot; 5.' - &apos;
                result = innerText.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
                ////2.< - &lt; 
                //innerText = innerText.Replace("<", "&lt;");
                ////3.> - &gt; 
                //innerText = innerText.Replace(">", "&gt;");
                ////4." - &quot; 
                //innerText = innerText.Replace("\"", "&quot;");
                ////5.' - &apos;
                //innerText = innerText.Replace("'", "&apos;");
            }
            return result;
        }

        private string GetShortMonth(string month)
        {
            string shortMonth = string.Empty;
            
            switch (month)
            {
                case "January":
                    shortMonth = "Jan.";
                    break;
                case "February":
                    shortMonth = "Feb.";
                    break;
                case "March":
                    shortMonth = "Mar.";
                    break;
                case "April":
                    shortMonth = "Apr.";
                    break;
                case "May":
                    shortMonth = "May";
                    break;
                case "July":
                    shortMonth = "Jul.";
                    break;
                case "June":
                    shortMonth = "Jun.";
                    break;
                case "August":
                    shortMonth = "Aug.";
                    break;
                case "September":
                    shortMonth = "Sept.";
                    break;
                case "October":
                    shortMonth = "Oct.";
                    break;
                case "November":
                    shortMonth = "Nov.";
                    break;
                case "December":
                    shortMonth = "Dec.";
                    break;
            }
            return shortMonth;
        }

        public TemplateResults BuildPatient(XmlDocument xdoc, ActivityDetail activityDetail, Hashtable missingObjects, string mode, string[] requiredObjects = null)
        {
            TemplateResults results = new TemplateResults();

            string patientID = string.Empty;
            string patientFirstName = string.Empty;
            string patientLastName = string.Empty;
            string patientNameLF = string.Empty;
            string xpath = string.Empty;

            patientID = activityDetail.PatientID.ToString();
            patientFirstName = activityDetail.PatientFirstName.ToString();
            patientLastName = activityDetail.PatientLastName.ToString();
            patientNameLF = activityDetail.PatientNameLF.ToString();

            if (((String.IsNullOrEmpty(patientFirstName) && String.IsNullOrEmpty(patientFirstName.Trim()))
                || (String.IsNullOrEmpty(patientLastName) && String.IsNullOrEmpty(patientLastName.Trim())))
                && (requiredObjects != null && requiredObjects.Contains("PatientName")))
            {
                string missingObjString = "Patient name " + patientID;
                missingObjects = AddMissingObjects(missingObjects, missingObjString);
            }
            else
            {
                //PatientID
                xpath = GetModeSpecificTag(XMLFields.ModePatientID, mode);
                XmlNode patientIDNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(patientIDNode, patientID);

                //PatientFirstName
                xpath = GetModeSpecificTag(XMLFields.ModePatientFirstName, mode);
                XmlNode patientFirstNameNode = xdoc.SelectSingleNode(xpath);
                patientFirstName = ProperCase(patientFirstName);
                SetXMlNodeInnerText(patientFirstNameNode, patientFirstName);

                //PatientLastName
                xpath = GetModeSpecificTag(XMLFields.ModePatientLastName, mode);
                XmlNode patientLastNameNode = xdoc.SelectSingleNode(xpath);
                patientLastName = ProperCase(patientLastName);
                SetXMlNodeInnerText(patientLastNameNode, patientLastName);

                //PatientFullName
                xpath = GetModeSpecificTag(XMLFields.ModePatientFullName, mode);
                XmlNode patientNameLFNode = xdoc.SelectSingleNode(xpath);
                patientNameLF = ProperCase(patientNameLF);
                SetXMlNodeInnerText(patientNameLFNode, patientNameLF);
            }

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildHeader(XmlDocument xdoc, ActivityDetail activityDetail, Hashtable missingObjects, string mode, string[] requiredObjects = null)
        {
            TemplateResults results = new TemplateResults();

            string sendID = string.Empty;
            string activityID = string.Empty;
            string contractID = string.Empty;
            string xpath = string.Empty;

            sendID = activityDetail.SendID.ToString();
            contractID = activityDetail.ContractNumber.ToString();
            activityID = activityDetail.ActivityID.ToString();

            //SendID
            xpath = GetModeSpecificTag(XMLFields.ModeSendID, mode);
            XmlNode sendIDNode = xdoc.SelectSingleNode(xpath);
            SetXMlNodeInnerText(sendIDNode, sendID);

            //ActivityID
            xpath = GetModeSpecificTag(XMLFields.ModeActivityID, mode);
            XmlNode activityIDNode = xdoc.SelectSingleNode(xpath);
            SetXMlNodeInnerText(activityIDNode, activityID);

            //ContractID
            xpath = GetModeSpecificTag(XMLFields.ModeContractID, mode);
            XmlNode contractIDNode = xdoc.SelectSingleNode(xpath);
            SetXMlNodeInnerText(contractIDNode, contractID);

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildSchedule(XmlDocument xdoc, ActivityDetail activityDetail, Hashtable missingObjects, string mode, string[] requiredObjects = null)
        {
            TemplateResults results = new TemplateResults();

            string xpath = string.Empty;
            string scheduleID = string.Empty;

            scheduleID = activityDetail.RecipientSchedID.ToString();

            //ScheduleID
            xpath = GetModeSpecificTag(XMLFields.ModeScheduleID, mode);
            XmlNode scheduleIDNode = xdoc.SelectSingleNode(xpath);
            SetXMlNodeInnerText(scheduleIDNode, scheduleID);

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildApptDateTime(XmlDocument xdoc, ActivityDetail activityDetail, Hashtable missingObjects, string mode, bool shortMonth = false, string[] requiredObjects = null)
        {
            TemplateResults results = new TemplateResults();

            string appointmentDateTime = string.Empty;
            string activityID = string.Empty; 
            string appointmentDayOfWeek = string.Empty;
            string appointmentMonth = string.Empty;
            string appointmentDate = string.Empty;
            string appointmentYear = string.Empty;
            string appointmentTime = string.Empty;
            string xpath = string.Empty;

            activityID = activityDetail.ActivityID.ToString();
            appointmentDateTime = activityDetail.ScheduleDateTime;
            
            //Appointment date fields
            if ((string.IsNullOrEmpty(appointmentDateTime) || Convert.ToDateTime(appointmentDateTime) < DateTime.Now) 
                && (requiredObjects != null && requiredObjects.Contains("ScheduleDateTime")))
            {
                string missingObjString = "Appointment Datetime " + activityID;
                missingObjects = AddMissingObjects(missingObjects, missingObjString);
            }
            else
            {
                DateTime apptDateTime = Convert.ToDateTime(appointmentDateTime);

                //Appointment DayOfWeek
                appointmentDayOfWeek = apptDateTime.DayOfWeek.ToString();
                xpath = GetModeSpecificTag(XMLFields.ModeApptDayOfWeek, mode);
                XmlNode apptDayOfWeekNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(apptDayOfWeekNode, appointmentDayOfWeek);

                //Appointment Month
                appointmentMonth = shortMonth ? apptDateTime.ToString("MMM.") : apptDateTime.ToString("MMMM");
                xpath = GetModeSpecificTag(XMLFields.ModeApptMonth, mode);
                XmlNode appointmentMonthNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(appointmentMonthNode, appointmentMonth);

                //Appointment Date
                appointmentDate = apptDateTime.Day.ToString();
                xpath = GetModeSpecificTag(XMLFields.ModeApptDate, mode);
                XmlNode appointmentDateNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(appointmentDateNode, appointmentDate);

                //Appointment Year
                appointmentYear = apptDateTime.Year.ToString();
                xpath = GetModeSpecificTag(XMLFields.ModeApptYear, mode);
                XmlNode appointmentYearNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(appointmentYearNode, appointmentYear);

                //Appointment Time
                appointmentTime = apptDateTime.ToString("h:mm tt");
                xpath = GetModeSpecificTag(XMLFields.ModeApptTime, mode);
                XmlNode appointmentTimeNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(appointmentTimeNode, appointmentTime);
            }
            
            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public bool IsEmailAddressFormatValid(string emailAddress)
        {
            return (!string.IsNullOrEmpty(emailAddress) && Regex.IsMatch(emailAddress.Trim(), RegExPatterns.EmailAddressPatern));
        }

        public string[] GetCommRequestStatuses()
        {
            return Enum.GetNames(typeof(CommunicationRequestStatuses));
        }

        public string Transform(XmlDocument xml, TemplateDetail templateDetail, string mode)
        {
            string body = string.Empty;
            string xpath = string.Empty;

            body = TransformTemplate(xml, templateDetail, mode);
            xpath = GetModeSpecificTag(XMLFields.ModeMessageBody, mode);
            XmlNode bodyNode = xml.SelectSingleNode(xpath);
            SetXMlNodeInnerText(bodyNode, body);
            body = HandleXMlSpecialCharacters(body);

            return body;
        }
        
        private string TransformTemplate(XmlDocument xml, TemplateDetail templateDetail, string mode)
        {
            string transformedString = string.Empty;
            int facilityID = 0;
            if (xml != null)
            {
                string xpath = GetModeSpecificTag(XMLFields.ModeFacilityID, mode);
                XmlNode facilityIDNode = xml.SelectSingleNode(xpath);
                if (facilityIDNode != null)
                    facilityID = Convert.ToInt32(facilityIDNode.InnerText.ToString());
            }
            if (templateDetail != null)
            {
                string xslBody = templateDetail.TemplateXSLBody.ToString().Trim();
                XmlDocument xslDoc = new XmlDocument();
                xslDoc.LoadXml(xslBody);
                XslCompiledTransform xslTransform = new XslCompiledTransform();
                xslTransform.Load(xslDoc);
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlTextWriter xw = new XmlTextWriter(sw))
                    {
                        xslTransform.Transform(xml, null, xw);
                    }
                    transformedString = sw.ToString();
                }
            }
            return transformedString;
        }
    }
}
