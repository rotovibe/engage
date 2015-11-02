using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

using Phytel.Services;
using Phytel.Services.Security;

namespace Phytel.Services.Communication
{
    public class CommEmailTemplateManager
    {
        public CommEmailTemplateManager()
        {            
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

        public void BuildHeader(ref XmlDocument xdoc, ActivityEmailDetail activityEmailDetail, ref Hashtable missingObjects, string confirmURL, string optoutURL)
        {
            string sendID = string.Empty;
            string activityID = string.Empty;
            string contractID = string.Empty;
            string rescheduleURL = string.Empty;
            string cancelURL = string.Empty;
            string xpath = string.Empty;
            List<ActivityDetail> activityTable = activityEmailDetail.ActivityDetails;
            List<ActivityMedia> activityMediaTable = activityEmailDetail.ActivityMedias;         

            if (activityTable != null)
            {
                if (activityTable.Count > 0)
                {
                    sendID = activityTable[0].SendID.ToString();
                    contractID = activityTable[0].ContractNumber.ToString();
                    activityID = activityTable[0].ActivityID.ToString();
                 
                    //SendID
                    xpath = XMLFields.SendID;
                    XmlNode sendIDNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(sendIDNode, sendID);

                    //ActivityID
                    xpath = XMLFields.ActivityID;
                    XmlNode activityIDNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(activityIDNode, activityID);

                    //ContractID
                    xpath = XMLFields.EmailContractID;
                    XmlNode contractIDNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(contractIDNode, contractID);

                    //ConfirmURL
                    Dictionary<string, string> queryStrings = new Dictionary<string, string>();
                    queryStrings.Add("SendID", sendID);
                    queryStrings.Add("ContractID", contractID);

                    XmlNode confirmURLNode = xdoc.SelectSingleNode(string.Format(XMLFields.ConfirmationURL, "true"));
                    BuildURL(ref confirmURL, queryStrings);
                    SetCDATAXMlNodeInnerText(confirmURLNode, confirmURL,ref  xdoc);
                  
                    //OptOutURL
                    xpath = XMLFields.OptOutURL;
                    XmlNode optOutURLNode = xdoc.SelectSingleNode(string.Format(XMLFields.OptOutURL, "true"));
                    BuildURL(ref optoutURL, queryStrings);
                    SetCDATAXMlNodeInnerText(optOutURLNode, optoutURL, ref xdoc);

                    //Reschedule optional
                    XmlNode rescheduleURLNode = xdoc.SelectSingleNode(string.Format(XMLFields.RescheduleURL, "true"));
                    BuildURL(ref rescheduleURL, queryStrings);
                    SetCDATAXMlNodeInnerText(rescheduleURLNode, rescheduleURL,ref xdoc);

                    //Cancel optional
                    XmlNode cancelURLNode = xdoc.SelectSingleNode(string.Format(XMLFields.CancelURL, "true"));
                    BuildURL(ref cancelURL, queryStrings);
                    SetCDATAXMlNodeInnerText(cancelURLNode, cancelURL, ref xdoc);
                }
            }
        }

        public void BuildPatient(ref XmlDocument xdoc, ActivityEmailDetail activityEmailDetail, ref Hashtable missingObjects)
        {
            string patientID = string.Empty;
            string patientFirstName = string.Empty;
            string patientLastName = string.Empty;
            string patientNameLF = string.Empty;
            string xpath = string.Empty;

            List<ActivityDetail> activityTable = activityEmailDetail.ActivityDetails;
            List<ActivityMedia> activityMediaTable = activityEmailDetail.ActivityMedias;        
            if (activityTable != null)
            {
                if (activityTable.Count > 0)
                {
                    patientID = activityTable[0].PatientID.ToString();
                    patientFirstName = activityTable[0].PatientFirstName;
                    patientLastName = activityTable[0].PatientLastName;
                    patientNameLF = activityTable[0].PatientNameLF;
                   
                }
                
                if ((String.IsNullOrEmpty(patientFirstName) && String.IsNullOrEmpty(patientFirstName.Trim()))
                    || (String.IsNullOrEmpty(patientLastName) && String.IsNullOrEmpty(patientLastName.Trim())))
                {
                    string missingObjString = "Patient name " + patientID;
                    AddMissingObjects(ref missingObjects, missingObjString);
                }

                else
                {
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
        }

        public void BuildIntroPatient(ref XmlDocument xdoc, ActivityEmailDetail activityEmailDetail, ref Hashtable missingObjects)
        {
            string patientID = string.Empty;
            string xpath = string.Empty;

            List<ActivityDetail> activityTable = activityEmailDetail.ActivityDetails;
            List<ActivityMedia> activityMediaTable = activityEmailDetail.ActivityMedias; 
            if (activityTable != null)
            {
                if (activityTable.Count > 0)
                {
                    patientID = activityTable[0].PatientID.ToString();
                }

                //PatientID
                xpath = XMLFields.PatientID;
                XmlNode patientIDNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(patientIDNode, patientID);
            }            
        }

        public void BuildSchedule(ref XmlDocument xdoc, ActivityEmailDetail activityEmailDetail, ref Hashtable missingObjects)
        {
            string scheduleNameLF = string.Empty;
            string xpath = string.Empty;
            string scheduleID = string.Empty;
            List<ActivityMedia> mediaRows = null;

            List<ActivityDetail> activityTable = activityEmailDetail.ActivityDetails;
            List<ActivityMedia> activityMediaTable = activityEmailDetail.ActivityMedias; 
            if (activityTable != null)
            {
                if (activityTable.Count > 0)
                {
                    scheduleID = activityTable[0].RecipientSchedID.ToString();
                }

                //ScheduleID
                xpath = XMLFields.EmailScheduleID;
                XmlNode scheduleIDNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(scheduleIDNode, scheduleID);

                xpath = XMLFields.ScheduleFullName;
                XmlNode ScheudleNameLFNode = xdoc.SelectSingleNode(xpath);
                mediaRows = (from m in activityMediaTable
                             where m.OwnerID == activityTable[0].RecipientSchedID
                             && m.CategoryCode == "SCOVR"
                             && m.OwnerCode == "EMAIL"
                             && m.LanguagePreferenceCode == "EN"
                             select m).ToList();

                if (mediaRows != null && mediaRows.Count > 0)
                {
                    foreach (ActivityMedia media in mediaRows)
                    {
                        scheduleNameLF = media.Narrative;
                        if (!String.IsNullOrEmpty(scheduleNameLF) && !String.IsNullOrEmpty(scheduleNameLF.Trim()))
                        {
                            scheduleNameLF = ProperCase(scheduleNameLF.Trim());
                            SetXMlNodeInnerText(ScheudleNameLFNode, scheduleNameLF);
                        }
                        else
                        {
                            string missingObjString = "Schedule name " + scheduleNameLF;
                            AddMissingObjects(ref missingObjects, missingObjString);
                        }
                    }
                }
                else
                {
                    string missingObjString = "Schedule name " + scheduleNameLF;
                    AddMissingObjects(ref missingObjects, missingObjString);
                }
            }
        }

        public void BuildFacility(ref XmlDocument xdoc, ActivityEmailDetail activityEmailDetail, ref Hashtable missingObjects, string taskTypeCategory)
        {

            string facilityID = string.Empty;
            string facilityLogo = string.Empty;
            string facilityURL = string.Empty;
            string facilityName = string.Empty;
            string facilityAddr1 = string.Empty;
            string facilityAddr2 = string.Empty;
            string facilityCity = string.Empty;
            string facilityState = string.Empty;
            string facilityZip = string.Empty;
            string facilityPhoneNumber = string.Empty;

            ActivityMedia media = null;
            string xpath = string.Empty;

            List<ActivityDetail> activityTable = activityEmailDetail.ActivityDetails;
            List<ActivityMedia> activityMediaTable = activityEmailDetail.ActivityMedias;

            if (activityTable != null)
            {
                if (activityTable.Count > 0)
                {
                    facilityID = activityTable[0].FacilityID.ToString();
                    facilityAddr1 = activityTable[0].FacilityAddrLine1;
                    facilityAddr2 = activityTable[0].FacilityAddrLine2;
                    facilityCity = activityTable[0].FacilityCity;
                    facilityState = activityTable[0].FacilityState;
                    facilityZip = activityTable[0].FacilityZipCode;
                    facilityPhoneNumber = activityTable[0].ProviderACDNumber;
                }

                //FacilityID
                xpath = XMLFields.FacilityID;
                XmlNode facilityIDNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(facilityIDNode, facilityID);

                //FacilityAddr1
                xpath = XMLFields.FacilityAddr1;
                XmlNode facilityAddr1Node = xdoc.SelectSingleNode(xpath);
                facilityAddr1 = ProperCase(facilityAddr1);
                SetXMlNodeInnerText(facilityAddr1Node, facilityAddr1);

                //FacilityAddr2
                xpath = XMLFields.FacilityAddr2;
                XmlNode facilityAddr2Node = xdoc.SelectSingleNode(xpath);
                facilityAddr2 = ProperCase(facilityAddr2);
                SetXMlNodeInnerText(facilityAddr2Node, facilityAddr2);

                //FacilityCity
                xpath = XMLFields.FacilityCity;
                XmlNode facilityCityNode = xdoc.SelectSingleNode(xpath);
                facilityCity = ProperCase(facilityCity);
                SetXMlNodeInnerText(facilityCityNode, facilityCity);


                //FacilityState
                xpath = XMLFields.FacilityState;
                XmlNode facilityStateNode = xdoc.SelectSingleNode(xpath);
                if (!String.IsNullOrEmpty(facilityState) && !String.IsNullOrEmpty(facilityState.Trim()))
                {
                    facilityState = facilityState.ToUpper();
                }
                SetXMlNodeInnerText(facilityStateNode,facilityState);

                //FacilityZip
                xpath = XMLFields.FacilityZip;
                XmlNode facilityZipNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(facilityZipNode, facilityZip);


                //Facility phonenumber
                if (!String.IsNullOrEmpty(facilityPhoneNumber) && !String.IsNullOrEmpty(facilityPhoneNumber.Trim()))
                {
                    xpath = XMLFields.FacilityPhoneNumber;
                    XmlNode facilityPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(facilityPhoneNumberNode, String.Format("{0:(###) ###-####}", Convert.ToInt64(facilityPhoneNumber)));
                }


                xpath = XMLFields.FacilityLogo;
                XmlNode facilityLogoNode = xdoc.SelectSingleNode(xpath);
                media = (from m in activityMediaTable
                         where m.OwnerID == activityTable[0].FacilityID
                         && m.CategoryCode == "LOGO"
                         && m.OwnerCode == "EMAIL"
                         select m).FirstOrDefault();
                //Facility level
                if (media != null)
                {
                    facilityLogo = media.Filename;
                    SetXMlNodeInnerText(facilityLogoNode, facilityLogo);
                }
                else
                {
                    //Contract level
                    media = (from m in activityMediaTable
                             where m.OwnerID == -1
                             && m.CategoryCode == "LOGO"
                             && m.OwnerCode == "EMAIL"
                             && !m.Filename.Contains("GLOBAL")
                             select m).FirstOrDefault();
                    if (media != null)
                    {
                        facilityLogo = media.Filename;
                        SetXMlNodeInnerText(facilityLogoNode, facilityLogo);
                    }
                    else 
                    {
                        //Phytelmaster level
                        media = (from m in activityMediaTable
                                 where m.OwnerID == -1
                                 && m.CategoryCode == "LOGO"
                                 && m.OwnerCode == "EMAIL"
                                 && !m.Filename.Contains("GLOBAL")
                                 select m).FirstOrDefault();
                        if (media != null && taskTypeCategory != TaskTypeCategory.OutreachRecall)
                        {
                            facilityLogo = media.Filename;
                            SetXMlNodeInnerText(facilityLogoNode, facilityLogo);
                        }
                        else
                        {
                            //Set enabled to false for Day 1
                            facilityLogoNode.Attributes[XMLFields.Enable].Value = "false";
                            SetXMlNodeInnerText(facilityLogoNode, string.Empty);
                        }
                    }
                }

                xpath = XMLFields.FacilityURL;
                XmlNode facilityURLNode = xdoc.SelectSingleNode(xpath);
                media = (from m in activityMediaTable
                         where m.OwnerID == activityTable[0].FacilityID
                         && m.CategoryCode == "URL"
                         && m.OwnerCode == "EMAIL"
                         select m).FirstOrDefault();
                if (media != null)
                {
                    facilityURL = media.Narrative;
                    SetXMlNodeInnerText(facilityURLNode, facilityURL);
                }
                else
                {
                    media = (from m in activityMediaTable
                             where m.OwnerID == -1
                             && m.CategoryCode == "URL"
                             && m.OwnerCode == "EMAIL"
                             select m).FirstOrDefault();
                    if (media != null)
                    {
                        facilityURL = media.Narrative;
                        SetXMlNodeInnerText(facilityURLNode, facilityURL);
                    }
                    else
                    {
                        //Set enabled to false for Day 1
                        facilityURLNode.Attributes[XMLFields.Enable].Value = "false";
                        SetXMlNodeInnerText(facilityURLNode, string.Empty);
                    }
                }

                //Facility Name
                xpath = XMLFields.FacilityName;
                XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
                //check for email override
                media = (from m in activityMediaTable
                         where m.OwnerID == activityTable[0].FacilityID
                         && m.CategoryCode == "SNOVR"
                         && m.OwnerCode == "EMAIL"
                         && m.LanguagePreferenceCode == "EN"
                         select m).FirstOrDefault();
                if (media != null)
                {
                    facilityName = media.Narrative;
                    if (!String.IsNullOrEmpty(facilityName) && !String.IsNullOrEmpty(facilityName.Trim()))
                    {
                        SetXMlNodeInnerText(facilityNameNode, facilityName.Trim());
                    }
                    else
                    {
                        string missingObjString = "Facility name " + facilityID;
                        AddMissingObjects(ref missingObjects, missingObjString);
                    }
                }
                else
                {
                    string missingObjString = "Facility name " + facilityID;
                    AddMissingObjects(ref missingObjects, missingObjString);
                }
            }
        }

        public void BuildEmailMessage(ref XmlDocument xdoc, ActivityEmailDetail activityEmailDetail, ref Hashtable missingObjects, int campaignID, int contactRoleID,
            int templateID)
        {
            string appointmentDateTime = string.Empty;
            string appointmentDuration = string.Empty;
            string appointmentSpecificMsg = string.Empty;
            string messageSubject = string.Empty;
            string toEmailAddress = string.Empty;
            string fromEmailAddress = string.Empty;
            string replyToEmailAddress = string.Empty;
            string activityID = string.Empty;
            string messageType = string.Empty;
            string facilityID = string.Empty;
            string scheduleID = string.Empty;
            string patientId = string.Empty;
            string patientFirstName = string.Empty;
            string patientLastName = string.Empty;
            string facilityName = string.Empty;
            string scheduleName = string.Empty;

            ActivityMedia media = null;
            string xpath = string.Empty;

            List<ActivityDetail> activityTable = activityEmailDetail.ActivityDetails;
            List<ActivityMedia> activityMediaTable = activityEmailDetail.ActivityMedias;

            xpath = XMLFields.FacilityName;
            XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
            if (facilityNameNode != null)
            {
                facilityName = facilityNameNode.InnerText.ToString();
            }

            xpath = XMLFields.ScheduleFullName;
            XmlNode scheudleNameLFNode = xdoc.SelectSingleNode(xpath);
            if (scheudleNameLFNode != null)
            {
                scheduleName = scheudleNameLFNode.InnerText.ToString();
            }

            xpath = XMLFields.PatientFirstName;
            XmlNode patientFirstNameNode = xdoc.SelectSingleNode(xpath);
            if (patientFirstNameNode != null)
            {
                patientFirstName = patientFirstNameNode.InnerText.ToString();
            }

            xpath = XMLFields.PatientLastName;
            XmlNode patientLastNameNode = xdoc.SelectSingleNode(xpath);
            if (patientLastNameNode != null)
            {
                patientLastName = patientLastNameNode.InnerText.ToString();
            }

            if (activityTable != null)
            {
                if (activityTable.Count > 0)
                {
                    activityID = activityTable[0].ActivityID.ToString();
                    appointmentDateTime = activityTable[0].ScheduleDateTime;
                    toEmailAddress = activityTable[0].ToEmailAddress;
                    facilityID = activityTable[0].FacilityID.ToString();
                    scheduleID = activityTable[0].RecipientSchedID.ToString();
                    patientId = activityTable[0].PatientID.ToString();
                    appointmentDuration = activityTable[0].ScheduleDuration.ToString();                   
                }

                //ToEmailAddress
                xpath = XMLFields.ToEmailAddress;
                XmlNode toEmailAddressNode = xdoc.SelectSingleNode(xpath);
                if (String.IsNullOrEmpty(toEmailAddress)
                    || !Regex.IsMatch(toEmailAddress.ToString().Trim(), RegExPatterns.EmailAddressPatern))
                {
                    string missingObjString = "ToEmailAddress" + activityID;
                    AddMissingObjects(ref missingObjects, missingObjString);
                }
                else
                {
                    SetXMlNodeInnerText(toEmailAddressNode, toEmailAddress); 
               
                }

                //FromEmailAddress
                xpath = XMLFields.FromEmailAddress;
                XmlNode fromEmailAddressNode = xdoc.SelectSingleNode(xpath);
                media = (from m in activityMediaTable
                         where m.OwnerID == activityTable[0].FacilityID
                         && m.CategoryCode == "EMAIL"
                         && m.OwnerCode == "EMAIL"
                         select m).FirstOrDefault();
                if (media != null)
                {
                    //Set default reply email address
                    fromEmailAddress = media.Narrative;
                }

                if (!String.IsNullOrEmpty(fromEmailAddress) && !String.IsNullOrEmpty(fromEmailAddress.Trim()))
                {
                    SetXMlNodeInnerText(fromEmailAddressNode, fromEmailAddress.Trim());
                }
                else
                {
                    //Get Configured Displayname based on Contract override
                    media = (from m in activityMediaTable
                                where m.OwnerID == -1
                                && m.CategoryCode == "EMAIL"
                                && m.OwnerCode == "EMAIL"
                                select m).FirstOrDefault();
                    if (media != null)
                    {
                        fromEmailAddress = media.Narrative;
                        if (!String.IsNullOrEmpty(fromEmailAddress) && !String.IsNullOrEmpty(fromEmailAddress.Trim()))
                        {
                            SetXMlNodeInnerText(fromEmailAddressNode, fromEmailAddress.Trim());
                        }
                        else
                        {
                            string missingObjString = "From Email Address not configured at facility" + facilityID + " and contract level "  ;
                            AddMissingObjects(ref missingObjects, missingObjString);
                        }
                    }
                    else
                    {
                        string missingObjString = "From Email Address not configured at facility" + facilityID + " and contract level "  ;
                        AddMissingObjects(ref missingObjects, missingObjString);
                    }                    
                }
                
                //ReplyToEmailAddress
                xpath = XMLFields.ReplyToEmailAddress;
                XmlNode replyToEmailAddressNode = xdoc.SelectSingleNode(xpath);
                
                //Get reply address at facility level
                media = (from m in activityMediaTable
                         where m.OwnerID == activityTable[0].FacilityID
                         && m.CategoryCode == "REPLY"
                         && m.OwnerCode == "EMAIL"
                         select m).FirstOrDefault();
                
                if (media != null)
                {
                    replyToEmailAddress = media.Narrative;
                }

                //Facility level reply address
                if (!String.IsNullOrEmpty(replyToEmailAddress) && !String.IsNullOrEmpty(replyToEmailAddress.Trim()))
                {
                    SetXMlNodeInnerText(replyToEmailAddressNode, replyToEmailAddress.Trim());
                }
                //Contract level reply address
                else
                {
                    //Get Configured Displayname based on Contract override
                    media = (from m in activityMediaTable
                                where m.OwnerID == -1
                                && m.CategoryCode == "REPLY"
                                && m.OwnerCode == "EMAIL"
                                select m).FirstOrDefault();
                    if (media != null)
                    {
                        replyToEmailAddress = media.Narrative;
                        if (!String.IsNullOrEmpty(replyToEmailAddress) && !String.IsNullOrEmpty(replyToEmailAddress.Trim()))
                        {
                            SetXMlNodeInnerText(replyToEmailAddressNode, replyToEmailAddress.Trim());
                        }
                        else
                        {
                            string missingObjString = "Reply Email Address not configured at facility" + facilityID + " and contract level "  ;
                            AddMissingObjects(ref missingObjects, missingObjString);
                        }
                    }
                    else
                    {
                        string missingObjString = "Reply Email Address not configured at facility" + facilityID + " and contract level ";
                        AddMissingObjects(ref missingObjects, missingObjString);
                    }                    
                }
 
                //MessageType
                xpath = XMLFields.MessageType;
                XmlNode messageTypeNode = xdoc.SelectSingleNode(xpath);
                switch (campaignID) 
                {
                    case (int)CampaignTypes.ACDefault:
                        messageType = MessageTypes.AppointmentReminder;
                        SetXMlNodeInnerText(messageTypeNode, messageType);
                        break;

                    case (int)CampaignTypes.OutreachDefault:
                        messageType = MessageTypes.OutreachRecall;
                        SetXMlNodeInnerText(messageTypeNode, messageType);
                        break;
                }

                //DisplayName
                //Default display name to facility name
                xpath = XMLFields.DisplayName;
                XmlNode displayNameNode = xdoc.SelectSingleNode(xpath);
                displayNameNode.InnerText = facilityName;

                //Get Configured Displayname based on ScheduleID override
                media = (from m in activityMediaTable
                         where m.OwnerID == activityTable[0].RecipientSchedID
                         && m.CategoryCode == "NAME"
                         && m.OwnerCode == "EMAIL"
                         select m).FirstOrDefault();
                if (media != null)
                {
                    displayNameNode.InnerText = media.Narrative;
                }
                else
                {
                    //Get Configured Displayname based on FacilityID override
                    media = (from m in activityMediaTable
                             where m.OwnerID == activityTable[0].FacilityID
                             && m.CategoryCode == "NAME"
                             && m.OwnerCode == "EMAIL"
                             select m).FirstOrDefault();
                    if (media != null)
                    {
                        displayNameNode.InnerText = media.Narrative;
                    }
                    else
                    {
                        //Get Configured Displayname based on Contract override
                        media = (from m in activityMediaTable
                                 where m.OwnerID == -1
                                 && m.CategoryCode == "NAME"
                                 && m.OwnerCode == "EMAIL"
                                 select m).FirstOrDefault();
                        if (media != null)
                        {
                            displayNameNode.InnerText = media.Narrative;
                        }
                    }
                }
                                
                //Subject 
                //Don't set inner text via SetXMlNodeInnerText as facilityname XML characters will be decoded again
                xpath = XMLFields.MessageSubject;
                XmlNode messageSubjectNode = xdoc.SelectSingleNode(xpath);
                if (!String.IsNullOrEmpty(messageSubjectNode.InnerText))
                {
                    messageSubjectNode.InnerText = messageSubjectNode.InnerText
                        .Replace("FACILITY_NAME", facilityName)
                        .Replace("PATIENT_FIRST_NAME", patientFirstName)
                        .Replace("PATIENT_LAST_NAME", patientLastName);
                }
                else
                {
                    messageSubjectNode.InnerText = messageType;
                }

                //if (messageType != MessageTypes.OutreachRecall)
                //{
                //    //Set Appointment Duration
                //    xpath = XMLFields.ApptDuration;
                //    XmlNode apptDurationNode = xdoc.SelectSingleNode(xpath);
                //    SetXMlNodeInnerText(apptDurationNode, appointmentDuration);

                //    //Appointment date fields
                //    if (String.IsNullOrEmpty(appointmentDateTime) || Convert.ToDateTime(appointmentDateTime) < System.DateTime.Now)
                //    {
                //        string missingObjString = "Appointment Datetime " + activityID;
                //        AddMissingObjects(ref missingObjects, missingObjString);
                //    }
                //    else
                //    {
                //        BuildApptDateTime(Convert.ToDateTime(appointmentDateTime), ref xdoc);
                //    }
                
                    //Appointment specific message
                    //BuildAppointmentSpecificMessage(ref  xdoc, activityEmailDetail, ref  missingObjects, contactRoleID, templateID, contractPermissionRecords);
                //}
            }
        }

        public void BuildApptDateTime(ref XmlDocument xdoc, ActivityEmailDetail activityEmailDetail, ref Hashtable missingObjects)
        {
            string appointmentDateTime = string.Empty;
            string appointmentDuration = string.Empty;
            string activityID = string.Empty;            
            string appointmentDayOfWeek = string.Empty;
            string appointmentMonth = string.Empty;
            string appointmentDate = string.Empty;
            string appointmentYear = string.Empty;
            string appointmentTime = string.Empty;
            string xpath = string.Empty;
            
            List<ActivityDetail> activityTable = activityEmailDetail.ActivityDetails;

            if (activityTable != null)
            {
                if (activityTable.Count > 0)
                {
                    activityID = activityTable[0].ActivityID.ToString();
                    appointmentDateTime = activityTable[0].ScheduleDateTime;                    
                    appointmentDuration = activityTable[0].ScheduleDuration.ToString();
                }

                //Set Appointment Duration
                xpath = XMLFields.ApptDuration;
                XmlNode apptDurationNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(apptDurationNode, appointmentDuration);

                //Appointment date fields
                if (String.IsNullOrEmpty(appointmentDateTime) || Convert.ToDateTime(appointmentDateTime) < System.DateTime.Now)
                {
                    string missingObjString = "Appointment Datetime " + activityID;
                    AddMissingObjects(ref missingObjects, missingObjString);
                }
                else
                {
                    DateTime apptDateTime = Convert.ToDateTime(appointmentDateTime);
                    
                    //Appointment DayOfWeek
                    appointmentDayOfWeek = apptDateTime.DayOfWeek.ToString();
                    xpath = XMLFields.ApptDayOfWeek;
                    XmlNode apptDayOfWeekNode = xdoc.SelectSingleNode(xpath);
                    SetXMlNodeInnerText(apptDayOfWeekNode, appointmentDayOfWeek);

                    //Appointment Month
                    appointmentMonth = apptDateTime.ToString("MMMM");
                    xpath = XMLFields.ApptMonth;
                    XmlNode appointmentMonthNode = xdoc.SelectSingleNode(xpath);
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
            }
        }

        public void BuildAppointmentSpecificMessage(ref XmlDocument xdoc, ActivityEmailDetail activityEmailDetail, ref Hashtable missingObjects, int contactRoleID,
            int templateID, List<ContractPermission> contractPermissionRecords)
        {
            int contractLookUp = -1;
            List<ActivityMedia> mediaRows = null;
            string xpath = string.Empty;
            string appointmentSpecificMessage = string.Empty;

            //Appointment specific message
            xpath = XMLFields.AppointmentSpecificMessage;
            XmlNode apptSpecificMsgNode = xdoc.SelectSingleNode(xpath);
            string activityID = string.Empty;
            string strMediaFilter = string.Empty;

            string facilityID = string.Empty;
            string scheduleID = string.Empty;
            string patientId = string.Empty;

            List<ActivityDetail> activityTable = activityEmailDetail.ActivityDetails;
            List<ActivityMedia> activityMediaTable = activityEmailDetail.ActivityMedias;

            if (activityTable != null)
            {
                if (activityTable.Count > 0)
                {
                    activityID = activityTable[0].ActivityID.ToString();
                    facilityID = activityTable[0].FacilityID.ToString();
                    scheduleID = activityTable[0].RecipientSchedID.ToString();
                    patientId = activityTable[0].PatientID.ToString();

                }
            }
            if (IsAppointmentSpecificMsgEnabled(contractPermissionRecords, contactRoleID))
            {
                //First get based on email scheduleid
                mediaRows = (from m in activityMediaTable
                            where m.OwnerID == activityTable[0].PatientID
                            && m.CategoryCode == "TTC"
                            && m.OwnerCode == "EMAIL"
                            && m.LanguagePreferenceCode == "EN"
                            && m.FacilityID == activityTable[0].RecipientSchedID
                            select m).ToList();
              
                //If not available for schedule get based on facilityid
                if (mediaRows == null || mediaRows.Count == 0)
                {
                    mediaRows = (from m in activityMediaTable
                                    where m.OwnerID == activityTable[0].PatientID
                                    && m.CategoryCode == "TTC"
                                    && m.OwnerCode == "EMAIL"
                                    && m.LanguagePreferenceCode == "EN"
                                    && m.FacilityID == activityTable[0].FacilityID
                                    select m).ToList();
                  
                    //Finally get based on contract
                    if (mediaRows == null || mediaRows.Count == 0)
                    {
                        mediaRows = (from m in activityMediaTable
                                        where m.OwnerID == activityTable[0].PatientID
                                        && m.CategoryCode == "TTC"
                                        && m.OwnerCode == "EMAIL"
                                        && m.LanguagePreferenceCode == "EN"
                                        && m.FacilityID == contractLookUp
                                        select m).ToList();

                        //First get based on scheduleid for email template
                        if (mediaRows == null || mediaRows.Count == 0)
                        {
                            mediaRows = (from m in activityMediaTable
                                            where m.OwnerID == activityTable[0].PatientID
                                            && m.CategoryCode == "TTC"
                                            && m.LanguagePreferenceCode == "EN"
                                            && m.FacilityID == activityTable[0].RecipientSchedID
                                            select m).ToList();

                            //First get based on scheduleid for email template
                            if (mediaRows == null || mediaRows.Count == 0)
                            {
                                mediaRows = (from m in activityMediaTable
                                                where m.OwnerID == activityTable[0].PatientID
                                                && m.CategoryCode == "TTC"
                                                && m.LanguagePreferenceCode == "EN"
                                                && m.FacilityID == activityTable[0].FacilityID
                                                select m).ToList();

                                //First get based on scheduleid for email template
                                if (mediaRows == null || mediaRows.Count == 0)
                                {
                                    mediaRows = (from m in activityMediaTable
                                                    where m.OwnerID == activityTable[0].PatientID
                                                    && m.CategoryCode == "TTC"
                                                    && m.LanguagePreferenceCode == "EN"
                                                    && m.FacilityID == contractLookUp
                                                    select m).ToList();                                   
                                }
                            }
                        }
                    }
                }
                if (mediaRows != null && mediaRows.Count > 0)
                {
                    foreach (ActivityMedia media in mediaRows)
                    {
                        appointmentSpecificMessage = ProperCase(media.Narrative);
                        SetXMlNodeInnerText(apptSpecificMsgNode, appointmentSpecificMessage);
                    }
                }
                else
                {
                    //Set enabled to false for Day 1
                    apptSpecificMsgNode.Attributes[XMLFields.Enable].Value = "false";
                    SetXMlNodeInnerText(apptSpecificMsgNode, string.Empty);
                }
            }
            else
            {
                //Set enabled to false for Day 1
                apptSpecificMsgNode.Attributes[XMLFields.Enable].Value = "false";
                SetXMlNodeInnerText(apptSpecificMsgNode, string.Empty);
            }
        }

        public void BuildIntroFacility(ref XmlDocument xdoc, ActivityEmailDetail activityEmailDetail, ref Hashtable missingObjects)
        {
            string facilityID = string.Empty;
            string facilityName = string.Empty;
            ActivityMedia media = null;
            string xpath = string.Empty;

            List<ActivityDetail> activityTable = activityEmailDetail.ActivityDetails;
            List<ActivityMedia> activityMediaTable = activityEmailDetail.ActivityMedias;

            if (activityTable != null)
            {
                if (activityTable.Count > 0)
                {
                    facilityID = activityTable[0].FacilityID.ToString();
                }

                //FacilityID
                xpath = XMLFields.FacilityID;
                XmlNode facilityIDNode = xdoc.SelectSingleNode(xpath);
                SetXMlNodeInnerText(facilityIDNode, facilityID);

                //Facility Name
                xpath = XMLFields.FacilityName;
                XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
                //check for email override
                media = (from m in activityMediaTable
                         where m.OwnerID == activityTable[0].FacilityID
                         && m.CategoryCode == "SNOVR"
                         && m.OwnerCode == "EMAIL"
                         && m.LanguagePreferenceCode == "EN"
                         select m).FirstOrDefault();

                if (media != null)
                {
                    facilityName = media.Narrative;
                    if (!String.IsNullOrEmpty(facilityName) && !String.IsNullOrEmpty(facilityName.Trim()))
                    {
                        SetXMlNodeInnerText(facilityNameNode, facilityName.Trim());
                    }
                    else
                    {
                        string missingObjString = "Facility name " + facilityID;
                        AddMissingObjects(ref missingObjects, missingObjString);
                    }
                }
                else
                {
                    string missingObjString = "Facility name " + facilityID;
                    AddMissingObjects(ref missingObjects, missingObjString);
                }
            }
        }

        public void BuildIntroEmailMessage(ref XmlDocument xdoc, ActivityEmailDetail activityEmailDetail, ref Hashtable missingObjects, int campaignID, int contactRoleID, int templateID)
        {
            string messageSubject = string.Empty;
            string toEmailAddress = string.Empty;
            string fromEmailAddress = string.Empty;
            string replyToEmailAddress = string.Empty;
            string activityID = string.Empty;
            string messageType = string.Empty;
            string facilityID = string.Empty;
            string facilityName = string.Empty;
            string scheduleID = string.Empty;            

            ActivityMedia media = null;

            string xpath = string.Empty;
            List<ActivityDetail> activityTable = activityEmailDetail.ActivityDetails;
            List<ActivityMedia> activityMediaTable = activityEmailDetail.ActivityMedias;

            xpath = XMLFields.FacilityName;
            XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
            if (facilityNameNode != null)
            {
                facilityName = facilityNameNode.InnerText.ToString();
            }

            if (activityTable != null)
            {
                if (activityTable.Count > 0)
                {
                    activityID = activityTable[0].ActivityID.ToString();
                    toEmailAddress = activityTable[0].ToEmailAddress;
                    facilityID = activityTable[0].FacilityID.ToString();
                    scheduleID = activityTable[0].RecipientSchedID.ToString();
                }

                //ToEmailAddress
                xpath = XMLFields.ToEmailAddress;
                XmlNode toEmailAddressNode = xdoc.SelectSingleNode(xpath);
                if (String.IsNullOrEmpty(toEmailAddress)
                   || !Regex.IsMatch(toEmailAddress.Trim(), RegExPatterns.EmailAddressPatern))
                {
                    string missingObjString = "ToEmailAddress" + activityID;
                    AddMissingObjects(ref missingObjects, missingObjString);
                }
                else
                {
                    SetXMlNodeInnerText(toEmailAddressNode, toEmailAddress);
                }

                //FromEmailAddress
                xpath = XMLFields.FromEmailAddress;
                XmlNode fromEmailAddressNode = xdoc.SelectSingleNode(xpath);
                media = (from m in activityMediaTable
                         where m.OwnerID == activityTable[0].FacilityID
                         && m.CategoryCode == "EMAIL"
                         && m.OwnerCode == "EMAIL"
                         select m).FirstOrDefault();

                if (media != null)
                {
                    //Set default reply email address
                    fromEmailAddress = media.Narrative;
                }

                if (!String.IsNullOrEmpty(fromEmailAddress) && !String.IsNullOrEmpty(fromEmailAddress.Trim()))
                {
                    SetXMlNodeInnerText(fromEmailAddressNode, fromEmailAddress.Trim());
                }
                else
                {
                    //Get Configured Displayname based on Contract override
                    media = (from m in activityMediaTable
                                where m.OwnerID == -1
                                && m.CategoryCode == "EMAIL"
                                && m.OwnerCode == "EMAIL"
                                select m).FirstOrDefault();

                    if (media != null)
                    {
                        fromEmailAddress = media.Narrative;
                        if (!String.IsNullOrEmpty(fromEmailAddress) && !String.IsNullOrEmpty(fromEmailAddress.Trim()))
                        {
                            SetXMlNodeInnerText(fromEmailAddressNode, fromEmailAddress.Trim());
                        }
                        else
                        {
                            string missingObjString = "From Email Address not configured at facility" + facilityID + " and contract level "  ;
                            AddMissingObjects(ref missingObjects, missingObjString);
                        }
                    }
                    else
                    {
                        string missingObjString = "From Email Address not configured at facility" + facilityID + " and contract level "  ;
                        AddMissingObjects(ref missingObjects, missingObjString);
                    }                    
                }
                
                //ReplyToEmailAddress
                xpath = XMLFields.ReplyToEmailAddress;
                XmlNode replyToEmailAddressNode = xdoc.SelectSingleNode(xpath);
                
                //Get reply address at facility level
                media = (from m in activityMediaTable
                         where m.OwnerID == activityTable[0].FacilityID
                         && m.CategoryCode == "REPLY"
                         && m.OwnerCode == "EMAIL"
                         select m).FirstOrDefault();
                
                if (media != null)
                {
                    replyToEmailAddress = media.Narrative;
                }

                //Facility level reply address
                if (!String.IsNullOrEmpty(replyToEmailAddress) && !String.IsNullOrEmpty(replyToEmailAddress.Trim()))
                {
                    SetXMlNodeInnerText(replyToEmailAddressNode, replyToEmailAddress.Trim());
                }
                //Contract level reply address
                else
                {
                    //Get Configured Displayname based on Contract override
                    media = (from m in activityMediaTable
                             where m.OwnerID == -1
                             && m.CategoryCode == "REPLY"
                             && m.OwnerCode == "EMAIL"
                             select m).FirstOrDefault();

                    if (media != null)
                    {
                        replyToEmailAddress = media.Narrative;
                        if (!String.IsNullOrEmpty(replyToEmailAddress) && !String.IsNullOrEmpty(replyToEmailAddress.Trim()))
                        {
                            SetXMlNodeInnerText(replyToEmailAddressNode, replyToEmailAddress.Trim());
                        }
                        else
                        {
                            string missingObjString = "Reply Email Address not configured at facility" + facilityID + " and contract level "  ;
                            AddMissingObjects(ref missingObjects, missingObjString);
                        }
                    }
                    else
                    {
                        string missingObjString = "Reply Email Address not configured at facility" + facilityID + " and contract level ";
                        AddMissingObjects(ref missingObjects, missingObjString);
                    }                    
                }

                //MessageType
                xpath = XMLFields.MessageType;
                XmlNode messageTypeNode = xdoc.SelectSingleNode(xpath);
                if (campaignID == (int)CampaignTypes.ACDefault)
                {
                    messageType = MessageTypes.IntroductoryEmail;
                    SetXMlNodeInnerText(messageTypeNode, messageType);
                }

                //DisplayName
                //Defult display name to facility name
                xpath = XMLFields.DisplayName;
                XmlNode displayNameNode = xdoc.SelectSingleNode(xpath);
                displayNameNode.InnerText = facilityName;

                //Get Configured Displayname based on ScheduleID override
                media = (from m in activityMediaTable
                         where m.OwnerID == activityTable[0].RecipientSchedID
                         && m.CategoryCode == "NAME"
                         && m.OwnerCode == "EMAIL"
                         select m).FirstOrDefault();

                if (media != null)
                {
                    displayNameNode.InnerText = media.Narrative;
                }
                else
                {
                    //Get Configured Displayname based on FacilityID override
                    media = (from m in activityMediaTable
                             where m.OwnerID == activityTable[0].FacilityID
                             && m.CategoryCode == "NAME"
                             && m.OwnerCode == "EMAIL"
                             select m).FirstOrDefault();

                    if (media != null)
                    {
                        displayNameNode.InnerText = media.Narrative;
                    }
                    else
                    {
                        //Get Configured Displayname based on Contract override
                        media = (from m in activityMediaTable
                                 where m.OwnerID == -1
                                 && m.CategoryCode == "NAME"
                                 && m.OwnerCode == "EMAIL"
                                 select m).FirstOrDefault();

                        if (media != null)
                        {
                            displayNameNode.InnerText = media.Narrative;
                        }
                    }
                }               
              
                //Subject 
                //Don't set inner text via SetXMlNodeInnerText as facilityname XML characters will be decoded again
                xpath = XMLFields.MessageSubject;
                XmlNode messageSubjectNode = xdoc.SelectSingleNode(xpath);
                if (!String.IsNullOrEmpty(facilityName)
                     && !String.IsNullOrEmpty(facilityName.Trim()))
                {
                    messageSubjectNode.InnerText = "A message from " + facilityName;
                }
                else
                {
                    messageSubjectNode.InnerText = messageType;
                }
            }
        }

        public void AddMissingObjects(ref Hashtable missingObjects, string missingObjString)
        {
            if (!missingObjects.ContainsValue(missingObjString))
            {
                missingObjects.Add(missingObjects.Count + 1, missingObjString);
            }
        }

        public void BuildURL(ref string url, Dictionary<string, string> querystrings)
        {
            DataProtector dataProtector = new DataProtector(DataProtector.Store.USE_SIMPLE_STORE);

            string stringToEncrypt = string.Empty;
            string encryptedString = string.Empty;
            string key = string.Empty;
            int i = 0;
            foreach (KeyValuePair<string, string> queryString in querystrings)
            {
                encryptedString = dataProtector.Encrypt(queryString.Value.ToString());
                key = queryString.Key.ToString();
                if (i == 0)
                {
                    url = url + "?" + key + "=" + HttpUtility.UrlEncode(encryptedString);
                }
                else
                {
                    url = url + "&" + key + "=" + HttpUtility.UrlEncode(encryptedString);
                }
                i++;
            }
        }

        public  void SetXMlNodeInnerText(XmlNode node, string innerText)
        {
            try
            {
                if (node != null)
                {
                    //HandleXMlSpecialCharacters(ref innerText);
                    node.InnerText = innerText;
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
        }

        public void SetCDATAXMlNodeInnerText(XmlNode node, string innerText, ref XmlDocument xmlDoc)
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
                        HandleXMlSpecialCharacters(ref innerText);
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
        }

        public void HandleXMlSpecialCharacters(ref string innerText)
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
        }

        public bool IsAppointmentSpecificMsgEnabled(List<ContractPermission> contractPermissions,  int contactRoleID)
        {
            List<ContractPermission> permissionRows = null;
            bool isEnabled = false;
            try
            {
                if (contractPermissions != null && contractPermissions.Count > 0)
                {
                    permissionRows = (from cp in contractPermissions
                                        where cp.ChildObjectID == (int)Prompts.AppointmentSpecificMessage
                                        && cp.RoleID == contactRoleID
                                        select cp).ToList();
                    if (permissionRows != null && permissionRows.Count > 0)
                    {
                        isEnabled = true;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return isEnabled;
        }
    }
}
