using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
//using Phytel.Framework.ASE.Data.Common;
//using Phytel.Framework.ASE.Data;
//using Phytel.Framework.ASE.Process;
//using PhytelDLLQueueText.Data;
using System.IO;
using System.Collections;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Web;

namespace Phytel.Services.Communication
{
    internal class CommTextTemplateManager
    {
        //#region Internal Methods

        ///// <summary>
        ///// Method to build the email template xml for given activity and 
        ///// save the details with correct inprocess flag to database
        ///// </summary>
        ///// <param name="activityInfoRecords"></param>
        ///// <param name="activityID"></param>
        ///// <param name="contractNumber"></param>
        //internal void SaveSendQueueInputXML(DataSet activityInfoRecords, int activityID, string contractNumber,
        //    int sendID, XmlNode baseConfiguration, int baseProcessId)
        //{
        //    try
        //    {
        //        int templateID = 0;
        //        int campaignID = 0;
        //        int contactRoleID = 0;
        //        int facilityID = 0;
        //        DataRow activityRow = null;
        //        DataManager dataManager = new DataManager();
        //        string templateBody = string.Empty;
        //        string templateXML = string.Empty;
        //        bool isSuccess = false;
        //        DataSet textXMLTemplate = null;
        //        string tasktypeCategory = string.Empty;
        //        XmlDocument xDoc = new XmlDocument();
        //        Hashtable missingObjects = new Hashtable();
        //        //Get activity row
        //        activityRow = activityInfoRecords.Tables[DatabaseFileds.ActivityTable].Rows[activityInfoRecords.Tables[DatabaseFileds.ActivityTable].Rows.Count - 1];
        //        tasktypeCategory = activityRow[DatabaseFileds.TaskTypeCategory].ToString();
        //        //Validate TemplateID , CampaignID and ContractRoleID
        //        ValidateActivityConfigurations(activityID, contractNumber, ref templateID, ref campaignID, ref contactRoleID, activityRow, tasktypeCategory);

        //        Log.LogDebug(baseProcessId, "TaskTypeCategory : " + tasktypeCategory + " for activity " + activityID);

        //        if (activityRow != null)
        //        {
        //            if (activityRow["FacilityID"] != null)
        //            {
        //                if (!string.IsNullOrEmpty(activityRow["FacilityID"].ToString()))
        //                {
        //                    facilityID = Convert.ToInt32(activityRow["FacilityID"].ToString());

        //                    Log.LogDebug(baseProcessId, "Get Activity FacilityID : " + facilityID + " for activity " + activityID);
        //                }
        //            }
        //        }
        //        //Get communication template to build call script
        //        textXMLTemplate = dataManager.GetTextTemplateDetails(contractNumber, facilityID, templateID, campaignID, tasktypeCategory);


        //        if (textXMLTemplate != null && textXMLTemplate.Tables.Count > 0)
        //        {
        //            foreach (DataRow dr in textXMLTemplate.Tables[0].Rows)
        //            {
        //                templateBody = dr[DatabaseFileds.TemplateXMLBody].ToString().Trim();

        //                Log.LogDebug(baseProcessId, "Intro Text XML Template body : " + templateBody);
        //            }
        //            xDoc.LoadXml(templateBody);
        //            //Build call script details
        //            isSuccess = BuildTemplate(xDoc, activityInfoRecords, templateID, campaignID, sendID, contactRoleID, ref missingObjects, baseProcessId, baseConfiguration, tasktypeCategory);
        //            Log.LogDebug(baseProcessId, "Build Template Successfull : " + isSuccess + " for activity " + activityID);

        //            using (StringWriter sw = new StringWriter())
        //            {

        //                using (XmlTextWriter xw = new XmlTextWriter(sw))
        //                {
        //                    xDoc.WriteTo(xw);
        //                    templateXML = sw.ToString().Replace("utf-8", "utf-16");

        //                    Log.LogDebug(baseProcessId, "templateXML");

        //                }

        //            }

        //            Log.LogDebug(baseProcessId, "Update SendQueue START");

        //            dataManager.UpdateSendQueue(templateXML, activityID, contractNumber, SendQueueInprocessStatus.BuildXML, sendID, null, null);

        //            Log.LogDebug(baseProcessId, "Updated SendQueue Successfully for SendID: " + sendID);
        //            Log.LogDebug(baseProcessId, "Update SendQueue END");

        //            if (!isSuccess)
        //            {
        //                Log.LogDebug(baseProcessId, "SendQueue update failed START");

        //                SendEmailAlert(missingObjects, baseConfiguration, sendID, contractNumber, baseProcessId);
        //                dataManager.UpdateTextExceptions(activityID, contractNumber, SendQueueInprocessStatus.SentForCommError,
        //                                                ActivityNotifySender.TextFailed, SendQueueErrorMessage.InvalidActivityConfiguration, SendQueueStatus.Failed, sendID);

        //                Log.LogDebug(baseProcessId, "SendQueue update failed for SendID: " + sendID);

        //                Log.LogDebug(baseProcessId, "SendQueue update failed END");

        //            }
        //            else
        //            {
        //                Log.LogDebug(baseProcessId, "SendQueue update " + isSuccess + " for SendID: " + sendID);
        //            }

        //        }
        //        else
        //        {
        //            Log.LogDebug(baseProcessId, "Unable to retrieve text XML template for activity : " + activityID + " for contract : " + contractNumber);

        //            throw new Exception("Unable to retrieve text XML template for activity : " + activityID + " for contract : " + contractNumber);
        //        }
        //    }
        //    catch (XmlException xmlEx)
        //    {

        //        throw xmlEx;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }

        //}


        //#endregion

        private string ProperCase(string input)
        {

            string pattern = @"\w+|\W+";
            string result = "";
            Boolean capitalizeNext = true;

            foreach (System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(input, pattern))
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

        //private static void ValidateActivityConfigurations(int activityID, string contractNumber, ref int templateID,
        //                                                 ref int campaignID, ref int contactRoleID, DataRow activityRow ,string tasktypeCategory)
        //{
        //    switch (tasktypeCategory)
        //    {
        //        case TasktypeCategory.AppointmentReminder:
        //            if (!String.IsNullOrEmpty(activityRow[DatabaseFields.TemplateID].ToString()) &&
        //              !String.IsNullOrEmpty(activityRow[DatabaseFileds.TemplateID].ToString().Trim()) &&
        //              !String.IsNullOrEmpty(activityRow[DatabaseFileds.CampaignID].ToString()) &&
        //              !String.IsNullOrEmpty(activityRow[DatabaseFileds.CampaignID].ToString().Trim()) &&
        //              !String.IsNullOrEmpty(activityRow[DatabaseFileds.ContactRoleID].ToString()) &&
        //              !String.IsNullOrEmpty(activityRow[DatabaseFileds.ContactRoleID].ToString().Trim()))
        //                        {
        //                            templateID = Convert.ToInt32(activityRow[DatabaseFileds.TemplateID].ToString().Trim());
        //                            campaignID = Convert.ToInt32(activityRow[DatabaseFileds.CampaignID].ToString().Trim());
        //                            contactRoleID = Convert.ToInt32(activityRow[DatabaseFileds.ContactRoleID].ToString().Trim());
        //                        }
        //            else
        //            {
        //                throw new Exception("Invalid TemplateID or CampaignID or ContactRoleID values for activity : " + activityID + " for contract : " + contractNumber);
        //            }
        //            break;


        //        case TasktypeCategory.AppointmentReminderIntroText:
        //            if (!String.IsNullOrEmpty(activityRow[DatabaseFileds.TemplateID].ToString()) &&
        //              !String.IsNullOrEmpty(activityRow[DatabaseFileds.TemplateID].ToString().Trim()) &&
        //              !String.IsNullOrEmpty(activityRow[DatabaseFileds.CampaignID].ToString()) &&
        //              !String.IsNullOrEmpty(activityRow[DatabaseFileds.CampaignID].ToString().Trim()))
        //            {
        //                templateID = Convert.ToInt32(activityRow[DatabaseFileds.TemplateID].ToString().Trim());
        //                campaignID = Convert.ToInt32(activityRow[DatabaseFileds.CampaignID].ToString().Trim());
        //    }
        //            else
        //            {
        //                throw new Exception("Invalid Intro Text TemplateID or CampaignID values for activity : " + activityID + " for contract : " + contractNumber);
        //            }
        //            break;
        //    }

          
        //}

     

        //private bool BuildTemplate(XmlDocument xdoc, DataSet inputXMlDataset, int templateID, int campaignID, int SendID, int contactRoleID,
        //                        ref Hashtable missingObjects, int processId, XmlNode baseConfiguration, string tasktypeCategory)
        //{
        //   bool isSuccess = true;
        //    try
        //    {
        //        switch (tasktypeCategory)
        //        {
        //            case TaskTypeCategory.AppointmentReminder:
        //                BuildHeader(ref xdoc, inputXMlDataset, SendID, ref missingObjects);
        //                BuildPatient(ref xdoc, inputXMlDataset, ref missingObjects);
        //                BuildSchedule(ref xdoc, inputXMlDataset, ref missingObjects);
        //                BuildFacility(ref xdoc, inputXMlDataset, ref missingObjects);
        //                BuildTextMessage(ref xdoc, inputXMlDataset, ref missingObjects, campaignID, contactRoleID, templateID);
        //                break;

        //            case TaskTypeCategory.IntroductoryText:
        //                //Log.LogDebug(processId, "Build Template AppointmentReminderIntroText : " + TasktypeCategory.AppointmentReminderIntroText);
        //                BuildIntroHeader(ref xdoc, inputXMlDataset, ref missingObjects, processId);
        //                BuildIntroTextPatient(ref xdoc, inputXMlDataset, ref missingObjects, processId);
        //                BuildIntroTextSchedule(ref xdoc, inputXMlDataset, ref missingObjects, processId);
        //                BuildIntroTextFacility(ref xdoc, inputXMlDataset, ref missingObjects, processId);
        //                BuildIntroTextMessage(ref xdoc, inputXMlDataset, ref missingObjects, campaignID, templateID, processId);
        //                break;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    return isSuccess;    
        //}


        #region BuildTemplate

        private void BuildPatient(ref XmlDocument xdoc, DataSet inputXMlDataset, ref Hashtable missingObjects)
       {
           string patientID = string.Empty;
           string patientFirstName = string.Empty;
           string patientLastName = string.Empty;
           string patientNameLF = string.Empty;
           string xpath = string.Empty;
           string strMediaFilter = string.Empty;
           string strMediaSort = string.Empty;          

           DataTable activityTable = inputXMlDataset.Tables[DatabaseFields.ActivityTable];
           DataTable activityMediaTable = inputXMlDataset.Tables[DatabaseFileds.ActivityMediaTable];
           if (activityTable != null)
           {
               if (activityTable.Rows != null && activityTable.Rows.Count > 0)
               {
                   patientID = activityTable.Rows[0][DatabaseFileds.PatientID].ToString();
                   patientFirstName = activityTable.Rows[0][DatabaseFileds.PatientFirstName].ToString();
                   patientLastName = activityTable.Rows[0][DatabaseFileds.PatientLastName].ToString();
                   patientNameLF = activityTable.Rows[0][DatabaseFileds.PatientNameLF].ToString();
                   
               }

                //PatientID
                xpath = XMLFields.PatientID;
                XmlNode patientIDNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(patientIDNode, patientID);

                //PatientFirstName
                xpath = XMLFields.PatientFirstName;
                XmlNode patientFirstNameNode = xdoc.SelectSingleNode(xpath);
                patientFirstName = ProperCase(patientFirstName);
                SetXMlNodeInnerText(patientFirstNameNode, patientFirstName);

                //PatientLastName
                xpath = XMLFields.PatientLastName;
                XmlNode patientLastNameNode = xdoc.SelectSingleNode(xpath);
                patientLastName = ProperCase(patientLastName);
                SetXMlNodeInnerText(patientLastNameNode, patientLastName);

                //PatientFullName
                xpath = XMLFields.PatientFullName;
                XmlNode patientNameLFNode = xdoc.SelectSingleNode(xpath);
                patientNameLF = ProperCase(patientNameLF);
                SetXMlNodeInnerText(patientNameLFNode, patientNameLF);

           }
       }

        private void BuildHeader(ref XmlDocument xdoc, DataSet inputXMlDataset, int SendID, ref Hashtable missingObjects)
        {
            string sendID = string.Empty;
            string activityID = string.Empty;
            string contractID = string.Empty;
            string xpath = string.Empty;
            DataTable activityTable = inputXMlDataset.Tables[DatabaseFileds.ActivityTable];
            DataTable activityMediaTable = inputXMlDataset.Tables[DatabaseFileds.ActivityMediaTable];


            if (activityTable != null)
            {
                if (activityTable.Rows != null && activityTable.Rows.Count > 0)
                {
                    sendID = SendID.ToString();
                    contractID = activityTable.Rows[0][DatabaseFileds.ContractNumber].ToString();
                    activityID = activityTable.Rows[0][DatabaseFileds.Activity].ToString();

                    //SendID
                    xpath = XMLFields.SendID;
                    XmlNode sendIDNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(sendIDNode, sendID);

                    //ActivityID
                    xpath = XMLFields.ActivityID;
                    XmlNode activityIDNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(activityIDNode, activityID);

                    //ContractID
                    xpath = XMLFields.TextContractID;
                    XmlNode contractIDNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(contractIDNode, contractID);

                }

            }
        }

        private void BuildSchedule(ref XmlDocument xdoc, DataSet inputXMlDataset, ref Hashtable missingObjects)
       {
           string scheduleNameLF = string.Empty;
           string scheduleDisplayName = string.Empty;
           string xpath = string.Empty;
           string strMediaFilter = string.Empty;
           string strMediaSort = string.Empty;
           string scheduleID = string.Empty;
           

           DataRow[] mediaRows = null;

           DataTable activityTable = inputXMlDataset.Tables[DatabaseFileds.ActivityTable];
           DataTable activityMediaTable = inputXMlDataset.Tables[DatabaseFileds.ActivityMediaTable];
           if (activityTable != null)
           {
               if (activityTable.Rows != null && activityTable.Rows.Count > 0)
               {
                   scheduleID = activityTable.Rows[0][DatabaseFileds.ScheduleID].ToString();
                   scheduleNameLF = activityTable.Rows[0][DatabaseFileds.CommScheduleName].ToString();
               }

               //ScheduleID
               xpath = XMLFields.TextScheduleID;
               XmlNode scheduleIDNode = xdoc.SelectSingleNode(xpath);
               SetXMlNodeInnerText(scheduleIDNode, scheduleID);

               //Contact Entities Schedule Name
               xpath = XMLFields.ScheduleFullName;
               XmlNode scheduleFullname = xdoc.SelectSingleNode(xpath);
               SetXMlNodeInnerText(scheduleFullname, scheduleNameLF);

               //Schedule Display name
               xpath = XMLFields.ScheduleDisplayName;
               XmlNode scheduleDisplayNameNode = xdoc.SelectSingleNode(xpath);
               strMediaFilter = "OwnerID ='" + scheduleID + "' AND CategoryCode = 'SCOVR' AND OwnerCode = 'TEXT' AND LanguagePreferenceCode ='EN'";
                mediaRows = activityMediaTable.Select(strMediaFilter);

               //Check to see if there is any records
                if (mediaRows != null && mediaRows.Length > 0)
                {
                    foreach (DataRow dr in mediaRows)
                    {
                        scheduleDisplayName = dr[DatabaseFileds.Narrative].ToString();
                        if (!String.IsNullOrEmpty(scheduleDisplayName) && !String.IsNullOrEmpty(scheduleDisplayName.Trim()))
                        {
                            scheduleDisplayName = ProperCase(scheduleDisplayName.Trim());
                            SetXMlNodeInnerText(scheduleDisplayNameNode, scheduleDisplayName);
                        }
                        else
                        {
                            //if there is no configured schedule name then default to your provider
                            scheduleDisplayName = "your provider";
                            SetXMlNodeInnerText(scheduleDisplayNameNode, scheduleDisplayName);
                        }
                    }
                }
                    else
                {
                    //if there is no configured schedule name then default to your provider
                    scheduleDisplayName = "your provider";
                    SetXMlNodeInnerText(scheduleDisplayNameNode, scheduleDisplayName);
                }
           }
       }

        private void BuildFacility(ref XmlDocument xdoc, DataSet inputXMlDataset, ref Hashtable missingObjects)
       {
        
           string facilityID = string.Empty;
           string facilityPhoneNumber = string.Empty;
           string facilityFullname = string.Empty;
           string facilityDisplayName = string.Empty;

           string strMediaFilter = string.Empty;
           string strMediaSort = string.Empty;

           DataRow[] mediaRows = null;
           string xpath = string.Empty;

           DataTable activityTable = inputXMlDataset.Tables[DatabaseFileds.ActivityTable];
           DataTable activityMediaTable = inputXMlDataset.Tables[DatabaseFileds.ActivityMediaTable];

           if (activityTable != null)
           {
               if (activityTable.Rows != null && activityTable.Rows.Count > 0)
               {
                   facilityID = activityTable.Rows[0][DatabaseFileds.FacilityID].ToString();
                   facilityPhoneNumber = activityTable.Rows[0][DatabaseFileds.ProviderACDNumber].ToString();
                   facilityFullname = activityTable.Rows[0][DatabaseFileds.FacilityFullName].ToString();
               }

                //FacilityID
               xpath = XMLFields.FacilityID;
                XmlNode facilityIDNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(facilityIDNode, facilityID);

                //Facility ContactEntities Name
                xpath = XMLFields.FacilityName;
                XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(facilityNameNode, facilityFullname);

                //Facility phonenumber
                if (!String.IsNullOrEmpty(facilityPhoneNumber) && !String.IsNullOrEmpty(facilityPhoneNumber.Trim()))
                {
                    xpath = XMLFields.FacilityPhoneNumber;
                    XmlNode facilityPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(facilityPhoneNumberNode, String.Format("{0:(###)###-####}", Convert.ToInt64(facilityPhoneNumber)));
                }
                else 
                {
                    string missingObjString = "Facility/Schedule ACD Phone number is missing for facility ID:  " + facilityID;
                    missingObjects = AddMissingObjects(missingObjects, missingObjString);
                }

                //Facility Display name
                xpath = XMLFields.FacilityDisplayName;
                XmlNode facilityDisplayNameNode = xdoc.SelectSingleNode(xpath);
                strMediaFilter = "OwnerID ='" + facilityID + "' AND CategoryCode = 'SNOVR' AND OwnerCode = 'TEXT' AND LanguagePreferenceCode ='EN'";
                mediaRows = activityMediaTable.Select(strMediaFilter);

                //Check to see if there is any records
                if (mediaRows != null && mediaRows.Length > 0)
                {
                    foreach (DataRow dr in mediaRows)
                    {
                        facilityDisplayName = dr[DatabaseFileds.Narrative].ToString();
                        if (!String.IsNullOrEmpty(facilityDisplayName) && !String.IsNullOrEmpty(facilityDisplayName.Trim()))
                        {
                            facilityDisplayName = ProperCase(facilityDisplayName.Trim());
                            SetXMlNodeInnerText(facilityDisplayNameNode, facilityDisplayName);
                        }
                    }
                }
             }
          }

        private void BuildTextMessage(ref XmlDocument xdoc, DataSet inputXMlDataset, ref Hashtable missingObjects, int campaignID, int contactRoleID, int templateID)
       {
            //TO DO 
           string appointmentDateTime = string.Empty;
            string activityID = string.Empty;
           string facilityID = string.Empty;
           string scheduleID = string.Empty;
           string patientId = string.Empty;
           string toTextNumber = string.Empty;
           string fromTextNumber = string.Empty;
           string helpTextNumber = string.Empty;
           string xpath = string.Empty;
           DataTable activityTable = inputXMlDataset.Tables[DatabaseFileds.ActivityTable];
           DataTable activityMediaTable = inputXMlDataset.Tables[DatabaseFileds.ActivityMediaTable];

           if (activityTable != null)
           {
               if (activityTable.Rows != null && activityTable.Rows.Count > 0)
               {
                   activityID = activityTable.Rows[0][DatabaseFileds.Activity].ToString();
                   appointmentDateTime = activityTable.Rows[0][DatabaseFileds.AppointmentDateTime].ToString();
                   facilityID = activityTable.Rows[0][DatabaseFileds.FacilityID].ToString();
                   scheduleID = activityTable.Rows[0][DatabaseFileds.ScheduleID].ToString();
                   patientId = activityTable.Rows[0][DatabaseFileds.PatientID].ToString();
                   fromTextNumber = activityTable.Rows[0][DatabaseFileds.TextFromNumber].ToString(); //this is the short code or long code
                   toTextNumber = activityTable.Rows[0][DatabaseFileds.PhoneNumber].ToString(); //this is the patient mobile number
                   helpTextNumber = activityTable.Rows[0][DatabaseFileds.ProviderACDNumber].ToString(); // this is the number that will be used for help and in the message
                   
               }

                //Appointment date fields
                if (String.IsNullOrEmpty(appointmentDateTime) || Convert.ToDateTime(appointmentDateTime) < System.DateTime.Now)
                {
                    string missingObjString = "Appointment Datetime " + activityID;
                    missingObjects = AddMissingObjects(missingObjects, missingObjString);
                }
                else
                {
                    BuildApptDateTime(Convert.ToDateTime(appointmentDateTime), ref xdoc);                       
                } 
                
                //Patient To phone number
                if (!String.IsNullOrEmpty(toTextNumber) && !String.IsNullOrEmpty(toTextNumber.Trim()))
                {
                    xpath = XMLFields.ToPhoneNumber;
                    XmlNode toPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(toPhoneNumberNode, toTextNumber);
                }
                else
                {
                    string missingObjString = "Patient Phone number is missing for patient ID " + patientId;
                    missingObjects = AddMissingObjects(missingObjects, missingObjString);
                }

                //Text From number
                if (!String.IsNullOrEmpty(fromTextNumber) && !String.IsNullOrEmpty(fromTextNumber.Trim()))
                {
                    xpath = XMLFields.FromPhoneNumber;
                    XmlNode fromPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(fromPhoneNumberNode, fromTextNumber);
                }
                else
                {
                    string missingObjString = "From Phone number(Short Code) is missing for Activity ID " + activityID;
                    missingObjects = AddMissingObjects(missingObjects, missingObjString);
                }

                //Help number
                if (!String.IsNullOrEmpty(helpTextNumber) && !String.IsNullOrEmpty(helpTextNumber.Trim()))
                {
                    xpath = XMLFields.HelpPhoneNumber;
                    XmlNode helpPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(helpPhoneNumberNode, helpTextNumber);
                }
                else
                {
                    string missingObjString = "Help Phone Number is missing for Activity ID " + activityID;
                    missingObjects = AddMissingObjects(missingObjects, missingObjString);
                }
            }         
       }

        private void BuildApptDateTime(DateTime apptDateTime, ref XmlDocument xdoc)
       {

            string appointmentDayOfWeek = string.Empty;
            string appointmentMonth = string.Empty;
            string appointmentDate = string.Empty;
            string appointmentYear = string.Empty;
            string appointmentTime = string.Empty;
            string xpath = string.Empty;


            //Appointment DayOfWeek
            appointmentDayOfWeek = apptDateTime.DayOfWeek.ToString();
            xpath = XMLFields.ApptDayOfWeek;
            XmlNode apptDayOfWeekNode = xdoc.SelectSingleNode(xpath);
            SetXMlNodeInnerText(apptDayOfWeekNode, appointmentDayOfWeek);

            //Appointment Month
            appointmentMonth = apptDateTime.ToString("MMMM");
            xpath = XMLFields.ApptMonth;
            XmlNode appointmentMonthNode = xdoc.SelectSingleNode(xpath);

            switch (appointmentMonth)
            {
                case "January":
                    appointmentMonth = "Jan.";
                    break;
                case "February":
                    appointmentMonth = "Feb.";
                    break;
                case "March":
                    appointmentMonth = "Mar.";
                    break;
                case "April":
                    appointmentMonth = "Apr.";
                    break;
                case "May":
                    appointmentMonth = "May";
                    break;
                case "July":
                    appointmentMonth = "Jul.";
                    break;
                case "June":
                    appointmentMonth = "Jun.";
                    break;
                case "August":
                    appointmentMonth = "Aug.";
                    break;
                case "September":
                    appointmentMonth = "Sept.";
                    break;
                case "October":
                    appointmentMonth = "Oct.";
                    break;
                case "November":
                    appointmentMonth = "Nov.";
                    break;
                case "December":
                    appointmentMonth = "Dec.";
                    break;
            }

            SetXMlNodeInnerText(appointmentMonthNode, appointmentMonth);

            //Appointment Date
            appointmentDate = apptDateTime.Day.ToString();
            xpath = XMLFields.ApptDate;
            XmlNode appointmentDateNode = xdoc.SelectSingleNode(xpath);
            SetXMlNodeInnerText(appointmentDateNode, appointmentDate);

            //Appointment Year
            appointmentYear = apptDateTime.Year.ToString();
            xpath = XMLFields.ApptYear;
            XmlNode appointmentYearNode = xdoc.SelectSingleNode(xpath);
            SetXMlNodeInnerText(appointmentYearNode, appointmentYear);

            //Appointment Time
            appointmentTime = apptDateTime.ToString("h:mm tt");
            xpath = XMLFields.ApptTime;
            XmlNode appointmentTimeNode = xdoc.SelectSingleNode(xpath);
            SetXMlNodeInnerText(appointmentTimeNode, appointmentTime);
        }

        #endregion        

        #region Intro Text Build Template

        private void BuildIntroHeader(ref XmlDocument xdoc, DataSet inputXMlDataset, ref Hashtable missingObjects, int processId)
        {
            string sendID = string.Empty;
            string activityID = string.Empty;
            string contractID = string.Empty;
            string xpath = string.Empty;
            DataTable activityTable = inputXMlDataset.Tables[DatabaseFileds.ActivityTable];
            DataTable activityMediaTable = inputXMlDataset.Tables[DatabaseFileds.ActivityMediaTable];


            if (activityTable != null)
            {
                if (activityTable.Rows != null && activityTable.Rows.Count > 0)
                {
                    sendID = activityTable.Rows[0][DatabaseFileds.SendID].ToString();
                    contractID = activityTable.Rows[0][DatabaseFileds.ContractNumber].ToString();
                    activityID = activityTable.Rows[0][DatabaseFileds.Activity].ToString();

                    //Log.LogDebug(processId, "Build BuildIntroHeader START.");

                    //SendID
                    xpath = XMLFields.SendID;
                    XmlNode sendIDNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(sendIDNode, sendID);

                    //ActivityID
                    xpath = XMLFields.ActivityID;
                    XmlNode activityIDNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(activityIDNode, activityID);

                    //ContractID
                    xpath = XMLFields.TextContractID;
                    XmlNode contractIDNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(contractIDNode, contractID);

                    //Log.LogDebug(processId, "Build Intro Text HeaderTemplate for Send ID : " + sendID + ",Activity ID: " + activityID + ",Contract ID : " + contractID);


                    //Log.LogDebug(processId, "Build BuildIntroHeader END.");
                }

            }
        }

        private void BuildIntroTextPatient(ref XmlDocument xdoc, DataSet inputXMlDataset, ref Hashtable missingObjects, int processId)
        {
            string patientID = string.Empty;
            string patientFirstName = string.Empty;
            string patientLastName = string.Empty;
            string patientNameLF = string.Empty;
            string xpath = string.Empty;
            string strMediaFilter = string.Empty;
            string strMediaSort = string.Empty;

            DataTable activityTable = inputXMlDataset.Tables[DatabaseFileds.ActivityTable];
            DataTable activityMediaTable = inputXMlDataset.Tables[DatabaseFileds.ActivityMediaTable];
            if (activityTable != null)
            {
                if (activityTable.Rows != null && activityTable.Rows.Count > 0)
                {
                    patientID = activityTable.Rows[0][DatabaseFileds.PatientID].ToString();
                    patientFirstName = activityTable.Rows[0][DatabaseFileds.PatientFirstName].ToString();
                    patientLastName = activityTable.Rows[0][DatabaseFileds.PatientLastName].ToString();
                    patientNameLF = activityTable.Rows[0][DatabaseFileds.PatientNameLF].ToString();

                }

                //Log.LogDebug(processId, "Build BuildIntroTextPatient START.");

                //PatientID
                xpath = XMLFields.PatientID;
                XmlNode patientIDNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(patientIDNode, patientID);


                //PatientLastName
                xpath = XMLFields.PatientLastName;
                XmlNode patientLastNameNode = xdoc.SelectSingleNode(xpath);
                patientLastName = ProperCase(patientLastName);
                SetXMlNodeInnerText(patientLastNameNode, patientLastName);

                //PatientFullName
                xpath = XMLFields.PatientFullName;
                XmlNode patientNameLFNode = xdoc.SelectSingleNode(xpath);
                patientNameLF = ProperCase(patientNameLF);
                SetXMlNodeInnerText(patientNameLFNode, patientNameLF);

                if (!String.IsNullOrEmpty(patientFirstName) && !String.IsNullOrEmpty(patientFirstName.Trim()))
                {
                    //PatientFirstName
                    xpath = XMLFields.PatientFirstName;
                    XmlNode patientFirstNameNode = xdoc.SelectSingleNode(xpath);
                    patientFirstName = ProperCase(patientFirstName);
                    SetXMlNodeInnerText(patientFirstNameNode, patientFirstName);
                    //Log.LogDebug(processId, "SetXMlNodeInnerText Patient Name :" + patientFirstName);
                }
                else
                {
                    string missingObjString = "Patient FirstName is missing for Patient ID:  " + patientID;
                    AddMissingObjects(ref missingObjects, missingObjString);

                    //Log.LogDebug(processId, "Patient Name is missing for Patient ID:  " + patientID);
                }

                //Log.LogDebug(processId, "Build BuildIntroTextPatient END.");
            }
        }

        private void BuildIntroTextSchedule(ref XmlDocument xdoc, DataSet inputXMlDataset, ref Hashtable missingObjects, int processId)
        {
            string scheduleNameLF = string.Empty;
            string scheduleDisplayName = string.Empty;
            string xpath = string.Empty;
            string strMediaFilter = string.Empty;
            string strMediaSort = string.Empty;
            string scheduleID = string.Empty;


            string facilityID = string.Empty;
            string facilityPhoneNumber = string.Empty;
            string facilityFullname = string.Empty;
            string facilityDisplayName = string.Empty;

            DataRow[] mediaRows = null;

            DataTable activityTable = inputXMlDataset.Tables[DatabaseFileds.ActivityTable];
            DataTable activityMediaTable = inputXMlDataset.Tables[DatabaseFileds.ActivityMediaTable];
            if (activityTable != null)
            {
                if (activityTable.Rows != null && activityTable.Rows.Count > 0)
                {
                    scheduleID = activityTable.Rows[0][DatabaseFileds.ScheduleID].ToString();
                    scheduleNameLF = activityTable.Rows[0][DatabaseFileds.CommScheduleName].ToString();                 
                }


                Log.LogDebug(processId, "Build BuildIntroTextSchedule START.");

                //ScheduleID
                xpath = XMLFields.TextScheduleID;
                XmlNode scheduleIDNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(scheduleIDNode, scheduleID);
                
                //Contact Entities Schedule Name
                xpath = XMLFields.ScheduleFullName;
                XmlNode scheduleFullname = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(scheduleFullname, scheduleNameLF);

                //Schedule Display name
                xpath = XMLFields.ScheduleDisplayName;
                XmlNode scheduleDisplayNameNode = xdoc.SelectSingleNode(xpath);
                strMediaFilter = "OwnerID ='" + scheduleID + "' AND CategoryCode = 'SCOVR' AND OwnerCode = 'TEXT' AND LanguagePreferenceCode ='EN'";
                mediaRows = activityMediaTable.Select(strMediaFilter);

                //Check to see if there is any records
                if (mediaRows != null && mediaRows.Length > 0)
                {
                    foreach (DataRow dr in mediaRows)
                    {
                        scheduleDisplayName = dr[DatabaseFileds.Narrative].ToString();
                        if (!String.IsNullOrEmpty(scheduleDisplayName) && !String.IsNullOrEmpty(scheduleDisplayName.Trim()))
                        {
                            scheduleDisplayName = ProperCase(scheduleDisplayName.Trim());
                            SetXMlNodeInnerText(scheduleDisplayNameNode, scheduleDisplayName);
                            Log.LogDebug(processId, "SetXMlNodeInnerText Schedule DisplayName");
                        }
                        else
                        {
                            string missingObjString = "Schedule Name is missing for Schedule ID:  " + scheduleID;
                            AddMissingObjects(ref missingObjects, missingObjString);
                            Log.LogDebug(processId, "Schedule Name is missing for Schedule ID:  " + scheduleID);
                        }
                    }
                }
                else
                {
                    string missingObjString = "Schedule Name is missing for Schedule ID:  " + scheduleID;
                    AddMissingObjects(ref missingObjects, missingObjString);
                    Log.LogDebug(processId, "No Media Rows for Schedule ID:  " + scheduleID);
                }

                Log.LogDebug(processId, "Build BuildIntroTextSchedule END.");
            }
        }

        private void BuildIntroTextFacility(ref XmlDocument xdoc, DataSet inputXMlDataset, ref Hashtable missingObjects, int processId)
        {

            string facilityID = string.Empty;
            string facilityPhoneNumber = string.Empty;
            string facilityFullname = string.Empty;
            string facilityDisplayName = string.Empty;

            string strMediaFilter = string.Empty;
            string strMediaSort = string.Empty;

            DataRow[] mediaRows = null;
            string xpath = string.Empty;

            DataTable activityTable = inputXMlDataset.Tables[DatabaseFileds.ActivityTable];
            DataTable activityMediaTable = inputXMlDataset.Tables[DatabaseFileds.ActivityMediaTable];

            if (activityTable != null)
            {
                if (activityTable.Rows != null && activityTable.Rows.Count > 0)
                {
                    facilityID = activityTable.Rows[0][DatabaseFileds.FacilityID].ToString();
                    facilityPhoneNumber = activityTable.Rows[0][DatabaseFileds.ProviderACDNumber].ToString();
                    facilityFullname = activityTable.Rows[0][DatabaseFileds.FacilityFullName].ToString();
                }

                Log.LogDebug(processId, "Build BuildIntroTextFacility START.");

                //FacilityID
                xpath = XMLFields.FacilityID;
                XmlNode facilityIDNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(facilityIDNode, facilityID);

                //Facility ContactEntities Name
                xpath = XMLFields.FacilityName;
                XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(facilityNameNode, facilityFullname);

                //Facility phonenumber
                if (!String.IsNullOrEmpty(facilityPhoneNumber) && !String.IsNullOrEmpty(facilityPhoneNumber.Trim()))
                {
                    xpath = XMLFields.FacilityPhoneNumber;
                    XmlNode facilityPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(facilityPhoneNumberNode, String.Format("{0:(###)###-####}", Convert.ToInt64(facilityPhoneNumber)));

                    Log.LogDebug(processId, "SetXMlNodeInnerText Facility PhoneNumber  " + facilityPhoneNumber + " for facilityID " + facilityID);  
                }

                //Facility Display name
                xpath = XMLFields.FacilityDisplayName;
                XmlNode facilityDisplayNameNode = xdoc.SelectSingleNode(xpath);
                strMediaFilter = "OwnerID ='" + facilityID + "' AND CategoryCode = 'SNOVR' AND OwnerCode = 'TEXT' AND LanguagePreferenceCode ='EN'";
                mediaRows = activityMediaTable.Select(strMediaFilter);

                //Check to see if there is any records
                if (mediaRows != null && mediaRows.Length > 0)
                {
                    foreach (DataRow dr in mediaRows)
                    {
                        facilityDisplayName = dr[DatabaseFileds.Narrative].ToString();
                        if (!String.IsNullOrEmpty(facilityDisplayName) && !String.IsNullOrEmpty(facilityDisplayName.Trim()))
                        {
                            facilityDisplayName = ProperCase(facilityDisplayName.Trim());
                            SetXMlNodeInnerText(facilityDisplayNameNode, facilityDisplayName);
                            Log.LogDebug(processId, "SetXMlNodeInnerText Facility DisplayName  " + facilityDisplayName + " for facilityID " + facilityID );                            
                        }
                    }
                }

                Log.LogDebug(processId, "Build BuildIntroTextFacility END.");

            }
        }

        private void BuildIntroTextMessage(ref XmlDocument xdoc, DataSet inputXMlDataset, ref Hashtable missingObjects, int campaignID, int templateID, int processId)
        {
            string appointmentDateTime = string.Empty;
            string activityID = string.Empty;
            string facilityID = string.Empty;
            string scheduleID = string.Empty;
            string patientId = string.Empty;
            string toTextNumber = string.Empty;
            string fromTextNumber = string.Empty;
            string helpTextNumber = string.Empty;
            string xpath = string.Empty;
            DataTable activityTable = inputXMlDataset.Tables[DatabaseFileds.ActivityTable];
            DataTable activityMediaTable = inputXMlDataset.Tables[DatabaseFileds.ActivityMediaTable];

            if (activityTable != null)
            {
                if (activityTable.Rows != null && activityTable.Rows.Count > 0)
                {
                    activityID = activityTable.Rows[0][DatabaseFileds.Activity].ToString();
                    appointmentDateTime = activityTable.Rows[0][DatabaseFileds.AppointmentDateTime].ToString();
                    facilityID = activityTable.Rows[0][DatabaseFileds.FacilityID].ToString();
                    scheduleID = activityTable.Rows[0][DatabaseFileds.ScheduleID].ToString();
                    patientId = activityTable.Rows[0][DatabaseFileds.PatientID].ToString();
                    fromTextNumber = activityTable.Rows[0][DatabaseFileds.TextFromNumber].ToString(); //this is the short code or long code
                    toTextNumber = activityTable.Rows[0][DatabaseFileds.PhoneNumber].ToString(); //this is the patient mobile number
                    helpTextNumber = activityTable.Rows[0][DatabaseFileds.ProviderACDNumber].ToString(); // this is the number that will be used for help and in the message

                }

                Log.LogDebug(processId, "Build BuildIntroTextMessage START.");

                ////Appointment date fields
                //if (String.IsNullOrEmpty(appointmentDateTime) || Convert.ToDateTime(appointmentDateTime) < System.DateTime.Now)
                //{
                //    string missingObjString = "Appointment Datetime " + activityID;
                //    AddMissingObjects(ref missingObjects, missingObjString);

                //    Log.LogDebug(processId, "Appointment Datetime is missing");
                //}
                //else
                //{
                //    BuildIntroApptDateTime(Convert.ToDateTime(appointmentDateTime), ref xdoc, processId);
                //}

                //Patient To phone number
                if (!String.IsNullOrEmpty(toTextNumber) && !String.IsNullOrEmpty(toTextNumber.Trim()))
                {
                    xpath = XMLFields.ToPhoneNumber;
                    XmlNode toPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(toPhoneNumberNode, toTextNumber);
                    Log.LogDebug(processId, "SetXMlNodeInnerText Patient Phone number: " + toTextNumber);
                }
                else
                {
                    string missingObjString = "Patient Phone number is missing for patient ID " + patientId;
                    AddMissingObjects(ref missingObjects, missingObjString);

                    Log.LogDebug(processId, "Patient Phone number is missing for patient ID " + patientId);
                }

                //Text From number
                if (!String.IsNullOrEmpty(fromTextNumber) && !String.IsNullOrEmpty(fromTextNumber.Trim()))
                {
                    xpath = XMLFields.FromPhoneNumber;
                    XmlNode fromPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(fromPhoneNumberNode, fromTextNumber);

                    Log.LogDebug(processId, "SetXMlNodeInnerText From Phone number(Short Code) " + fromTextNumber);
                }
                else
                {
                    string missingObjString = "From Phone number(Short Code) is missing for Activity ID " + activityID;
                    AddMissingObjects(ref missingObjects, missingObjString);

                    Log.LogDebug(processId, "From Phone number(Short Code) is missing for Activity ID " + activityID);
                }

                //Help number
                if (!String.IsNullOrEmpty(helpTextNumber) && !String.IsNullOrEmpty(helpTextNumber.Trim()))
                {
                    xpath = XMLFields.HelpPhoneNumber;
                    XmlNode helpPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(helpPhoneNumberNode, helpTextNumber);

                    Log.LogDebug(processId, "SetXMlNodeInnerText Help Phone Number :" + helpTextNumber);
                }
                else
                {
                    string missingObjString = "Help Phone Number is missing for Activity ID " + activityID;
                    AddMissingObjects(ref missingObjects, missingObjString);
                    Log.LogDebug(processId, "Help Phone Number is missing for Activity ID " + activityID);
                }

                Log.LogDebug(processId, "Build BuildIntroTextMessage END.");
            }
        }

        private void BuildIntroApptDateTime(DateTime apptDateTime, ref XmlDocument xdoc, int processId)
        {

           string appointmentDayOfWeek = string.Empty;
           string appointmentMonth = string.Empty;
           string appointmentDate = string.Empty;
           string appointmentYear = string.Empty;
           string appointmentTime = string.Empty;
           string xpath = string.Empty;


           //Appointment DayOfWeek
           appointmentDayOfWeek = apptDateTime.DayOfWeek.ToString();
           xpath = XMLFields.ApptDayOfWeek;
           XmlNode apptDayOfWeekNode = xdoc.SelectSingleNode(xpath);
           SetXMlNodeInnerText(apptDayOfWeekNode, appointmentDayOfWeek);

           //Appointment Month
           appointmentMonth = apptDateTime.ToString("MMMM");
           xpath = XMLFields.ApptMonth;
           XmlNode appointmentMonthNode = xdoc.SelectSingleNode(xpath);

           switch (appointmentMonth)
           {
               case "January":
                   appointmentMonth = "Jan.";
                   break;
               case "February":
                   appointmentMonth = "Feb.";
                   break;
               case "March":
                   appointmentMonth = "Mar.";
                   break;
               case "April":
                   appointmentMonth = "Apr.";
                   break;
               case "May":
                   appointmentMonth = "May";
                   break;
               case "July":
                   appointmentMonth = "Jul.";
                   break;
               case "June":
                   appointmentMonth = "Jun.";
                   break;
               case "August":
                   appointmentMonth = "Aug.";
                   break;
               case "September":
                   appointmentMonth = "Sept.";
                   break;
               case "October":
                   appointmentMonth = "Oct.";
                   break;
               case "November":
                   appointmentMonth = "Nov.";
                   break;
               case "December":
                   appointmentMonth = "Dec.";
                   break;
           }

           Log.LogDebug(processId, "Build IntroApptDateTime START.");

           SetXMlNodeInnerText(appointmentMonthNode, appointmentMonth);

           //Appointment Date
           appointmentDate = apptDateTime.Day.ToString();
           xpath = XMLFields.ApptDate;
           XmlNode appointmentDateNode = xdoc.SelectSingleNode(xpath);
           SetXMlNodeInnerText(appointmentDateNode, appointmentDate);

           //Appointment Year
           appointmentYear = apptDateTime.Year.ToString();
           xpath = XMLFields.ApptYear;
           XmlNode appointmentYearNode = xdoc.SelectSingleNode(xpath);
           SetXMlNodeInnerText(appointmentYearNode, appointmentYear);

           //Appointment Time
           appointmentTime = apptDateTime.ToString("h:mm tt");
           xpath = XMLFields.ApptTime;
           XmlNode appointmentTimeNode = xdoc.SelectSingleNode(xpath);
           SetXMlNodeInnerText(appointmentTimeNode, appointmentTime);

            Log.LogDebug(processId, "Build IntroApptDateTime END.");
       }
        
        #endregion

     
    }
}
