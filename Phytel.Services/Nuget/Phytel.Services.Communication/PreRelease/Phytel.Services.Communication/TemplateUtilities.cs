using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

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

        public XmlNode SetXMlNodeInnerText(XmlNode originalNode, string innerText)
        {
            try
            {
                if (originalNode != null)
                {
                    //HandleXMlSpecialCharacters(ref innerText);
                    originalNode.InnerText = innerText;
                }
            }
            catch (XmlException xmlEx)
            {
                throw xmlEx;
            }
            catch (Exception e)
            {
                throw e;
            }
            return originalNode;
        }

        public XmlDocument SetCDATAXMlNodeInnerText(XmlNode node, string innerText, XmlDocument xmlDoc)
        {
            try
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
            catch (XmlException xmlEx)
            {
                throw xmlEx;
            }
            catch (Exception e)
            {
                throw e;
            }
            return xmlDoc;
        }

        public string HandleXMlSpecialCharacters(string innerText)
        {
            if (!String.IsNullOrEmpty(innerText) && !String.IsNullOrEmpty(innerText.Trim()))
            {
                //1.& - &amp; 
                innerText = innerText.Replace("&", "&amp;");
                //2.< - &lt; 
                innerText = innerText.Replace("<", "&lt;");
                //3.> - &gt; 
                innerText = innerText.Replace(">", "&gt;");
                //4." - &quot; 
                innerText = innerText.Replace("\"", "&quot;");
                //5.' - &apos;
                innerText = innerText.Replace("'", "&apos;");
            }
            return innerText;
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
                patientIDNode.InnerText = patientID;

                //PatientFirstName
                xpath = GetModeSpecificTag(XMLFields.ModePatientFirstName, mode);
                XmlNode patientFirstNameNode = xdoc.SelectSingleNode(xpath);
                patientFirstName = ProperCase(patientFirstName);
                patientFirstNameNode.InnerText = patientFirstName;

                //PatientLastName
                xpath = GetModeSpecificTag(XMLFields.ModePatientLastName, mode);
                XmlNode patientLastNameNode = xdoc.SelectSingleNode(xpath);
                patientLastName = ProperCase(patientLastName);
                patientLastNameNode.InnerText = patientLastName;

                //PatientFullName
                xpath = GetModeSpecificTag(XMLFields.ModePatientFullName, mode);
                XmlNode patientNameLFNode = xdoc.SelectSingleNode(xpath);
                patientNameLF = ProperCase(patientNameLF);
                patientNameLFNode.InnerText = patientNameLF;
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
            sendIDNode.InnerText = sendID;

            //ActivityID
            xpath = GetModeSpecificTag(XMLFields.ModeActivityID, mode);
            XmlNode activityIDNode = xdoc.SelectSingleNode(xpath);
            activityIDNode.InnerText = activityID;

            //ContractID
            xpath = GetModeSpecificTag(XMLFields.ModeContractID, mode);
            XmlNode contractIDNode = xdoc.SelectSingleNode(xpath);
            contractIDNode.InnerText = contractID;

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
            scheduleIDNode.InnerText = scheduleID;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildApptDateTime(XmlDocument xdoc, ActivityDetail activityDetail, Hashtable missingObjects, string mode, string[] requiredObjects = null)
        {
            TemplateResults results = new TemplateResults();

            string appointmentDateTime = string.Empty;
            string appointmentDuration = string.Empty;
            string activityID = string.Empty; 
            string appointmentDayOfWeek = string.Empty;
            string appointmentMonth = string.Empty;
            string appointmentDate = string.Empty;
            string appointmentYear = string.Empty;
            string appointmentTime = string.Empty;
            string xpath = string.Empty;

            activityID = activityDetail.ActivityID.ToString();
            appointmentDateTime = activityDetail.ScheduleDateTime;
            appointmentDuration = activityDetail.ScheduleDuration.ToString();

            //Set Appointment Duration
            xpath = GetModeSpecificTag(XMLFields.ModeApptDuration, mode);
            XmlNode apptDurationNode = xdoc.SelectSingleNode(xpath);
            apptDurationNode.InnerText = appointmentDuration;

            //Appointment date fields
            if ((String.IsNullOrEmpty(appointmentDateTime) || Convert.ToDateTime(appointmentDateTime) < System.DateTime.Now) 
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
                apptDayOfWeekNode.InnerText = appointmentDayOfWeek;

                //Appointment Month
                appointmentMonth = apptDateTime.ToString("MMMM");
                xpath = GetModeSpecificTag(XMLFields.ModeApptMonth, mode);
                XmlNode appointmentMonthNode = xdoc.SelectSingleNode(xpath);
                appointmentMonthNode.InnerText = (mode.ToLower() == "text" ? GetShortMonth(appointmentMonth) : appointmentMonth);
                //appointmentMonthNode.InnerText = appointmentMonth;

                //Appointment Date
                appointmentDate = apptDateTime.Day.ToString();
                xpath = GetModeSpecificTag(XMLFields.ModeApptDate, mode);
                XmlNode appointmentDateNode = xdoc.SelectSingleNode(xpath);
                appointmentDateNode.InnerText = appointmentDate;

                //Appointment Year
                appointmentYear = apptDateTime.Year.ToString();
                xpath = GetModeSpecificTag(XMLFields.ModeApptYear, mode);
                XmlNode appointmentYearNode = xdoc.SelectSingleNode(xpath);
                appointmentYearNode.InnerText = appointmentYear;

                //Appointment Time
                appointmentTime = apptDateTime.ToString("h:mm tt");
                xpath = GetModeSpecificTag(XMLFields.ModeApptTime, mode);
                XmlNode appointmentTimeNode = xdoc.SelectSingleNode(xpath);
                appointmentTimeNode.InnerText = appointmentTime;
            }
            
            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public bool IsEmailAddressFormatValid(string emailAddress)
        {
            bool result = false;

            result = (!string.IsNullOrEmpty(emailAddress) && Regex.IsMatch(emailAddress.Trim(), RegExPatterns.EmailAddressPatern));

            return result;
        }

        public string[] GetCommRequestStatuses()
        {
            return Enum.GetNames(typeof(CommunicationRequestStatuses));
        }
    }
}
