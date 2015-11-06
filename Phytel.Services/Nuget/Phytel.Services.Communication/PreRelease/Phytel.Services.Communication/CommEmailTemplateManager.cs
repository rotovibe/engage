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
        private const string _Mode = "Email";
        private TemplateUtilities _templateUtilities;

        public CommEmailTemplateManager(TemplateUtilities templateUtilities)
        {
            TemplateUtilities _templateUtilities = templateUtilities;
        }

        public void BuildHeader(ref XmlDocument xdoc, EmailActivityDetail activityEmailDetail, ref Hashtable missingObjects, string confirmURL, string optoutURL)
        {
            string sendID = string.Empty;
            string activityID = string.Empty;
            string contractID = string.Empty;
            string rescheduleURL = string.Empty;
            string cancelURL = string.Empty;
            string xpath = string.Empty;

            sendID = activityEmailDetail.SendID.ToString();
            contractID = activityEmailDetail.ContractNumber.ToString();
            activityID = activityEmailDetail.ActivityID.ToString();
                 
            //SendID
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeSendID, _Mode);
            XmlNode sendIDNode = xdoc.SelectSingleNode(xpath);
            sendIDNode.InnerText = sendID;

            //ActivityID
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeActivityID, _Mode);
            XmlNode activityIDNode = xdoc.SelectSingleNode(xpath);
            activityIDNode.InnerText = activityID;

            //ContractID
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeContractID, _Mode);
            XmlNode contractIDNode = xdoc.SelectSingleNode(xpath);
            contractIDNode.InnerText = contractID;

            //ConfirmURL
            Dictionary<string, string> queryStrings = new Dictionary<string, string>();
            queryStrings.Add("SendID", sendID);
            queryStrings.Add("ContractID", contractID);

            XmlNode confirmURLNode = xdoc.SelectSingleNode(string.Format(_templateUtilities.GetModeSpecificTag(XMLFields.ModeConfirmationURL, _Mode), "true"));
            confirmURL = BuildURL(confirmURL, queryStrings);
            xdoc = _templateUtilities.SetCDATAXMlNodeInnerText(confirmURLNode, confirmURL, xdoc);
                  
            //OptOutURL
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeOptOutURL, _Mode);
            XmlNode optOutURLNode = xdoc.SelectSingleNode(string.Format(_templateUtilities.GetModeSpecificTag(XMLFields.ModeOptOutURL, _Mode), "true"));
            optoutURL = BuildURL(optoutURL, queryStrings);
            xdoc = _templateUtilities.SetCDATAXMlNodeInnerText(optOutURLNode, optoutURL, xdoc);

            //Reschedule optional
            XmlNode rescheduleURLNode = xdoc.SelectSingleNode(string.Format(_templateUtilities.GetModeSpecificTag(XMLFields.ModeRescheduleURL, _Mode), "true"));
            rescheduleURL = BuildURL(rescheduleURL, queryStrings);
            xdoc = _templateUtilities.SetCDATAXMlNodeInnerText(rescheduleURLNode, rescheduleURL, xdoc);

            //Cancel optional
            XmlNode cancelURLNode = xdoc.SelectSingleNode(string.Format(_templateUtilities.GetModeSpecificTag(XMLFields.ModeCancelURL, _Mode), "true"));
            cancelURL = BuildURL(cancelURL, queryStrings);
            xdoc = _templateUtilities.SetCDATAXMlNodeInnerText(cancelURLNode, cancelURL, xdoc);
        }

        public void BuildPatient(ref XmlDocument xdoc, EmailActivityDetail activityEmailDetail, ref Hashtable missingObjects)
        {
            string patientID = string.Empty;
            string patientFirstName = string.Empty;
            string patientLastName = string.Empty;
            string patientNameLF = string.Empty;
            string xpath = string.Empty;

            patientID = activityEmailDetail.PatientID.ToString();
            patientFirstName = activityEmailDetail.PatientFirstName;
            patientLastName = activityEmailDetail.PatientLastName;
            patientNameLF = activityEmailDetail.PatientNameLF;
                                   
            if ((String.IsNullOrEmpty(patientFirstName) && String.IsNullOrEmpty(patientFirstName.Trim()))
                || (String.IsNullOrEmpty(patientLastName) && String.IsNullOrEmpty(patientLastName.Trim())))
            {
                string missingObjString = "Patient name " + patientID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }
            else
            {
                //PatientID
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModePatientID, _Mode);
                XmlNode patientIDNode = xdoc.SelectSingleNode(xpath);
                patientIDNode.InnerText = patientID;

                //PatientFirstName
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModePatientFirstName, _Mode);
                XmlNode patientFirstNameNode = xdoc.SelectSingleNode(xpath);
                patientFirstName = _templateUtilities.ProperCase(patientFirstName);
                patientFirstNameNode.InnerText = patientFirstName;

                //PatientLastName
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModePatientLastName, _Mode);
                XmlNode patientLastNameNode = xdoc.SelectSingleNode(xpath);
                patientLastName = _templateUtilities.ProperCase(patientLastName);
                patientLastNameNode.InnerText = patientLastName;

                //PatientFullName
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModePatientFullName, _Mode);
                XmlNode patientNameLFNode = xdoc.SelectSingleNode(xpath);
                patientNameLF = _templateUtilities.ProperCase(patientNameLF);
                patientNameLFNode.InnerText = patientNameLF;
            }
        }

        public void BuildIntroPatient(ref XmlDocument xdoc, EmailActivityDetail activityEmailDetail, ref Hashtable missingObjects)
        {
            string xpath = string.Empty;

            //PatientID
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModePatientID, _Mode);
            XmlNode patientIDNode = xdoc.SelectSingleNode(xpath);
            patientIDNode.InnerText = activityEmailDetail.PatientID.ToString();          
        }

        public void BuildSchedule(ref XmlDocument xdoc, EmailActivityDetail activityEmailDetail, List<ActivityMedia> activityMediaList, ref Hashtable missingObjects)
        {
            string scheduleNameLF = string.Empty;
            string xpath = string.Empty;
            string scheduleID = string.Empty;
            List<ActivityMedia> mediaRows = null;
            
            scheduleID = activityEmailDetail.RecipientSchedID.ToString();

            //ScheduleID
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeScheduleID, _Mode);
            XmlNode scheduleIDNode = xdoc.SelectSingleNode(xpath);
            scheduleIDNode.InnerText = scheduleID;

            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeScheduleFullName, _Mode);
            XmlNode ScheduleNameLFNode = xdoc.SelectSingleNode(xpath);
            mediaRows = (from m in activityMediaList
                         where m.OwnerID == activityEmailDetail.RecipientSchedID
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
                        scheduleNameLF = _templateUtilities.ProperCase(scheduleNameLF.Trim());
                        ScheduleNameLFNode.InnerText = scheduleNameLF;
                    }
                    else
                    {
                        string missingObjString = "Schedule name " + scheduleNameLF;
                        missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                    }
                }
            }
            else
            {
                string missingObjString = "Schedule name " + scheduleNameLF;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }
        }

        public void BuildFacility(ref XmlDocument xdoc, EmailActivityDetail activityEmailDetail, List<ActivityMedia> activityMediaList, ref Hashtable missingObjects, string taskTypeCategory)
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


            facilityID = activityEmailDetail.FacilityID.ToString();
            facilityAddr1 = activityEmailDetail.FacilityAddrLine1;
            facilityAddr2 = activityEmailDetail.FacilityAddrLine2;
            facilityCity = activityEmailDetail.FacilityCity;
            facilityState = activityEmailDetail.FacilityState;
            facilityZip = activityEmailDetail.FacilityZipCode;
            facilityPhoneNumber = activityEmailDetail.ProviderACDNumber;
             

            //FacilityID
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityID, _Mode);
            XmlNode facilityIDNode = xdoc.SelectSingleNode(xpath);
            facilityIDNode.InnerText = facilityID;

            //FacilityAddr1
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityAddr1, _Mode);
            XmlNode facilityAddr1Node = xdoc.SelectSingleNode(xpath);
            facilityAddr1 = _templateUtilities.ProperCase(facilityAddr1);
            facilityAddr1Node.InnerText = facilityAddr1;

            //FacilityAddr2
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityAddr2, _Mode);
            XmlNode facilityAddr2Node = xdoc.SelectSingleNode(xpath);
            facilityAddr2 = _templateUtilities.ProperCase(facilityAddr2);
            facilityAddr2Node.InnerText = facilityAddr2;

            //FacilityCity
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityCity, _Mode);
            XmlNode facilityCityNode = xdoc.SelectSingleNode(xpath);
            facilityCity = _templateUtilities.ProperCase(facilityCity);
            facilityCityNode.InnerText = facilityCity;


            //FacilityState
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityState, _Mode);
            XmlNode facilityStateNode = xdoc.SelectSingleNode(xpath);
            if (!String.IsNullOrEmpty(facilityState) && !String.IsNullOrEmpty(facilityState.Trim()))
            {
                facilityState = facilityState.ToUpper();
            }
            facilityStateNode.InnerText = facilityState;

            //FacilityZip
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityZip, _Mode);
            XmlNode facilityZipNode = xdoc.SelectSingleNode(xpath);
            facilityZipNode.InnerText = facilityZip;


            //Facility phonenumber
            if (!String.IsNullOrEmpty(facilityPhoneNumber) && !String.IsNullOrEmpty(facilityPhoneNumber.Trim()))
            {
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityPhoneNumber, _Mode);
                XmlNode facilityPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                facilityPhoneNumberNode.InnerText = String.Format("{0:(###) ###-####}", Convert.ToInt64(facilityPhoneNumber));
            }


            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityLogo, _Mode);
            XmlNode facilityLogoNode = xdoc.SelectSingleNode(xpath);
            media = (from m in activityMediaList
                     where m.OwnerID == activityEmailDetail.FacilityID
                        && m.CategoryCode == "LOGO"
                        && m.OwnerCode == "EMAIL"
                        select m).FirstOrDefault();
            //Facility level
            if (media != null)
            {
                facilityLogo = media.Filename;
                facilityLogoNode.InnerText = facilityLogo;
            }
            else
            {
                //Contract level
                media = (from m in activityMediaList
                            where m.OwnerID == -1
                            && m.CategoryCode == "LOGO"
                            && m.OwnerCode == "EMAIL"
                            && !m.Filename.Contains("GLOBAL")
                            select m).FirstOrDefault();
                if (media != null)
                {
                    facilityLogo = media.Filename;
                    facilityLogoNode.InnerText = facilityLogo;
                }
                else 
                {
                    //Phytelmaster level
                    media = (from m in activityMediaList
                                where m.OwnerID == -1
                                && m.CategoryCode == "LOGO"
                                && m.OwnerCode == "EMAIL"
                                && !m.Filename.Contains("GLOBAL")
                                select m).FirstOrDefault();
                    if (media != null && taskTypeCategory != TaskTypeCategory.OutreachRecall)
                    {
                        facilityLogo = media.Filename;
                        facilityLogoNode.InnerText = facilityLogo;
                    }
                    else
                    {
                        //Set enabled to false for Day 1
                        facilityLogoNode.Attributes[XMLFields.Enable].Value = "false";
                        facilityLogoNode.InnerText = string.Empty;
                    }
                }
            }

            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityURL, _Mode);
            XmlNode facilityURLNode = xdoc.SelectSingleNode(xpath);
            media = (from m in activityMediaList
                     where m.OwnerID == activityEmailDetail.FacilityID
                        && m.CategoryCode == "URL"
                        && m.OwnerCode == "EMAIL"
                        select m).FirstOrDefault();
            if (media != null)
            {
                facilityURL = media.Narrative;
                facilityURLNode.InnerText = facilityURL;
            }
            else
            {
                media = (from m in activityMediaList
                            where m.OwnerID == -1
                            && m.CategoryCode == "URL"
                            && m.OwnerCode == "EMAIL"
                            select m).FirstOrDefault();
                if (media != null)
                {
                    facilityURL = media.Narrative;
                    facilityURLNode.InnerText = facilityURL;
                }
                else
                {
                    //Set enabled to false for Day 1
                    facilityURLNode.Attributes[XMLFields.Enable].Value = "false";
                    facilityURLNode.InnerText = string.Empty;
                }
            }

            //Facility Name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityName, _Mode);
            XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
            //check for email override
            media = (from m in activityMediaList
                     where m.OwnerID == activityEmailDetail.FacilityID
                        && m.CategoryCode == "SNOVR"
                        && m.OwnerCode == "EMAIL"
                        && m.LanguagePreferenceCode == "EN"
                        select m).FirstOrDefault();
            if (media != null)
            {
                facilityName = media.Narrative;
                if (!String.IsNullOrEmpty(facilityName) && !String.IsNullOrEmpty(facilityName.Trim()))
                {
                    facilityNameNode.InnerText = facilityName.Trim();
                }
                else
                {
                    string missingObjString = "Facility name " + facilityID;
                    missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                }
            }
            else
            {
                string missingObjString = "Facility name " + facilityID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }
        }

        public void BuildEmailMessage(ref XmlDocument xdoc, EmailActivityDetail activityEmailDetail, List<ActivityMedia> activityMediaList, ref Hashtable missingObjects, int campaignID, int contactRoleID,
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
            
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityName, _Mode);
            XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
            if (facilityNameNode != null)
            {
                facilityName = facilityNameNode.InnerText.ToString();
            }

            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeScheduleFullName, _Mode);
            XmlNode scheudleNameLFNode = xdoc.SelectSingleNode(xpath);
            if (scheudleNameLFNode != null)
            {
                scheduleName = scheudleNameLFNode.InnerText.ToString();
            }

            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModePatientFirstName, _Mode);
            XmlNode patientFirstNameNode = xdoc.SelectSingleNode(xpath);
            if (patientFirstNameNode != null)
            {
                patientFirstName = patientFirstNameNode.InnerText.ToString();
            }

            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModePatientLastName, _Mode);
            XmlNode patientLastNameNode = xdoc.SelectSingleNode(xpath);
            if (patientLastNameNode != null)
            {
                patientLastName = patientLastNameNode.InnerText.ToString();
            }

            activityID = activityEmailDetail.ActivityID.ToString();
            appointmentDateTime = activityEmailDetail.ScheduleDateTime;
            toEmailAddress = activityEmailDetail.ToEmailAddress;
            facilityID = activityEmailDetail.FacilityID.ToString();
            scheduleID = activityEmailDetail.RecipientSchedID.ToString();
            patientId = activityEmailDetail.PatientID.ToString();
            appointmentDuration = activityEmailDetail.ScheduleDuration.ToString();

            //ToEmailAddress
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeToEmailAddress, _Mode);
            XmlNode toEmailAddressNode = xdoc.SelectSingleNode(xpath);
            if (String.IsNullOrEmpty(toEmailAddress)
                || !Regex.IsMatch(toEmailAddress.ToString().Trim(), RegExPatterns.EmailAddressPatern))
            {
                string missingObjString = "ToEmailAddress" + activityID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }
            else
            {
                toEmailAddressNode.InnerText = toEmailAddress; 
               
            }

            //FromEmailAddress
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFromEmailAddress, _Mode);
            XmlNode fromEmailAddressNode = xdoc.SelectSingleNode(xpath);
            media = (from m in activityMediaList
                     where m.OwnerID == activityEmailDetail.FacilityID
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
                fromEmailAddressNode.InnerText = fromEmailAddress.Trim();
            }
            else
            {
                //Get Configured Displayname based on Contract override
                media = (from m in activityMediaList
                            where m.OwnerID == -1
                            && m.CategoryCode == "EMAIL"
                            && m.OwnerCode == "EMAIL"
                            select m).FirstOrDefault();
                if (media != null)
                {
                    fromEmailAddress = media.Narrative;
                    if (!String.IsNullOrEmpty(fromEmailAddress) && !String.IsNullOrEmpty(fromEmailAddress.Trim()))
                    {
                        fromEmailAddressNode.InnerText = fromEmailAddress.Trim();
                    }
                    else
                    {
                        string missingObjString = "From Email Address not configured at facility" + facilityID + " and contract level "  ;
                        missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                    }
                }
                else
                {
                    string missingObjString = "From Email Address not configured at facility" + facilityID + " and contract level "  ;
                    missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                }                    
            }
                
            //ReplyToEmailAddress
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeReplyToEmailAddress, _Mode);
            XmlNode replyToEmailAddressNode = xdoc.SelectSingleNode(xpath);
                
            //Get reply address at facility level
            media = (from m in activityMediaList
                     where m.OwnerID == activityEmailDetail.FacilityID
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
                replyToEmailAddressNode.InnerText = replyToEmailAddress.Trim();
            }
            //Contract level reply address
            else
            {
                //Get Configured Displayname based on Contract override
                media = (from m in activityMediaList
                            where m.OwnerID == -1
                            && m.CategoryCode == "REPLY"
                            && m.OwnerCode == "EMAIL"
                            select m).FirstOrDefault();
                if (media != null)
                {
                    replyToEmailAddress = media.Narrative;
                    if (!String.IsNullOrEmpty(replyToEmailAddress) && !String.IsNullOrEmpty(replyToEmailAddress.Trim()))
                    {
                        replyToEmailAddressNode.InnerText = replyToEmailAddress.Trim();
                    }
                    else
                    {
                        string missingObjString = "Reply Email Address not configured at facility" + facilityID + " and contract level "  ;
                        missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                    }
                }
                else
                {
                    string missingObjString = "Reply Email Address not configured at facility" + facilityID + " and contract level ";
                    missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                }                    
            }
 
            //MessageType
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeMessageType, _Mode);
            XmlNode messageTypeNode = xdoc.SelectSingleNode(xpath);
            switch (campaignID) 
            {
                case (int)CampaignTypes.ACDefault:
                    messageType = MessageTypes.AppointmentReminder;
                    messageTypeNode.InnerText = messageType;
                    break;

                case (int)CampaignTypes.OutreachDefault:
                    messageType = MessageTypes.OutreachRecall;
                    messageTypeNode.InnerText = messageType;
                    break;
            }

            //DisplayName
            //Default display name to facility name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeDisplayName, _Mode);
            XmlNode displayNameNode = xdoc.SelectSingleNode(xpath);
            displayNameNode.InnerText = facilityName;

            //Get Configured Displayname based on ScheduleID override
            media = (from m in activityMediaList
                     where m.OwnerID == activityEmailDetail.RecipientSchedID
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
                media = (from m in activityMediaList
                         where m.OwnerID == activityEmailDetail.FacilityID
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
                    media = (from m in activityMediaList
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
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeMessageSubject, _Mode);
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

        public void BuildApptDateTime(ref XmlDocument xdoc, EmailActivityDetail activityEmailDetail, ref Hashtable missingObjects)
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

            activityID = activityEmailDetail.ActivityID.ToString();
            appointmentDateTime = activityEmailDetail.ScheduleDateTime;
            appointmentDuration = activityEmailDetail.ScheduleDuration.ToString();

            //Set Appointment Duration
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeApptDuration, _Mode);
            XmlNode apptDurationNode = xdoc.SelectSingleNode(xpath);
            apptDurationNode.InnerText = appointmentDuration;

            //Appointment date fields
            if (String.IsNullOrEmpty(appointmentDateTime) || Convert.ToDateTime(appointmentDateTime) < System.DateTime.Now)
            {
                string missingObjString = "Appointment Datetime " + activityID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }
            else
            {
                DateTime apptDateTime = Convert.ToDateTime(appointmentDateTime);
                    
                //Appointment DayOfWeek
                appointmentDayOfWeek = apptDateTime.DayOfWeek.ToString();
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeApptDayOfWeek, _Mode);
                XmlNode apptDayOfWeekNode = xdoc.SelectSingleNode(xpath);
                apptDayOfWeekNode.InnerText = appointmentDayOfWeek;

                //Appointment Month
                appointmentMonth = apptDateTime.ToString("MMMM");
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeApptMonth, _Mode);
                XmlNode appointmentMonthNode = xdoc.SelectSingleNode(xpath);
                appointmentMonthNode.InnerText = appointmentMonth;

                //Appointment Date
                appointmentDate = apptDateTime.Day.ToString();
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeApptDate, _Mode);
                XmlNode appointmentDateNode = xdoc.SelectSingleNode(xpath);
                appointmentDateNode.InnerText = appointmentDate;

                //Appointment Year
                appointmentYear = apptDateTime.Year.ToString();
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeApptYear, _Mode);
                XmlNode appointmentYearNode = xdoc.SelectSingleNode(xpath);
                appointmentYearNode.InnerText = appointmentYear;

                //Appointment Time
                appointmentTime = apptDateTime.ToString("h:mm tt");
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeApptTime, _Mode);
                XmlNode appointmentTimeNode = xdoc.SelectSingleNode(xpath);
                appointmentTimeNode.InnerText = appointmentTime;
            }
        }

        public void BuildAppointmentSpecificMessage(ref XmlDocument xdoc, EmailActivityDetail activityEmailDetail, List<ActivityMedia> activityMediaList, ref Hashtable missingObjects, int contactRoleID,
            int templateID, List<ContractPermission> contractPermissionRecords)
        {
            int contractLookUp = -1;
            List<ActivityMedia> mediaRows = null;
            string xpath = string.Empty;
            string appointmentSpecificMessage = string.Empty;

            //Appointment specific message
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeAppointmentSpecificMessage, _Mode);
            XmlNode apptSpecificMsgNode = xdoc.SelectSingleNode(xpath);
            string activityID = string.Empty;
            string strMediaFilter = string.Empty;

            string facilityID = string.Empty;
            string scheduleID = string.Empty;
            string patientId = string.Empty;

            activityID = activityEmailDetail.ActivityID.ToString();
            facilityID = activityEmailDetail.FacilityID.ToString();
            scheduleID = activityEmailDetail.RecipientSchedID.ToString();
            patientId = activityEmailDetail.PatientID.ToString();

            if (IsAppointmentSpecificMsgEnabled(contractPermissionRecords, contactRoleID))
            {
                //First get based on email scheduleid
                mediaRows = (from m in activityMediaList
                            where m.OwnerID == activityEmailDetail.PatientID
                            && m.CategoryCode == "TTC"
                            && m.OwnerCode == "EMAIL"
                            && m.LanguagePreferenceCode == "EN"
                            && m.FacilityID == activityEmailDetail.RecipientSchedID
                            select m).ToList();
              
                //If not available for schedule get based on facilityid
                if (mediaRows == null || mediaRows.Count == 0)
                {
                    mediaRows = (from m in activityMediaList
                                    where m.OwnerID == activityEmailDetail.PatientID
                                    && m.CategoryCode == "TTC"
                                    && m.OwnerCode == "EMAIL"
                                    && m.LanguagePreferenceCode == "EN"
                                    && m.FacilityID == activityEmailDetail.FacilityID
                                    select m).ToList();
                  
                    //Finally get based on contract
                    if (mediaRows == null || mediaRows.Count == 0)
                    {
                        mediaRows = (from m in activityMediaList
                                        where m.OwnerID == activityEmailDetail.PatientID
                                        && m.CategoryCode == "TTC"
                                        && m.OwnerCode == "EMAIL"
                                        && m.LanguagePreferenceCode == "EN"
                                        && m.FacilityID == contractLookUp
                                        select m).ToList();

                        //First get based on scheduleid for email template
                        if (mediaRows == null || mediaRows.Count == 0)
                        {
                            mediaRows = (from m in activityMediaList
                                            where m.OwnerID == activityEmailDetail.PatientID
                                            && m.CategoryCode == "TTC"
                                            && m.LanguagePreferenceCode == "EN"
                                            && m.FacilityID == activityEmailDetail.RecipientSchedID
                                            select m).ToList();

                            //First get based on scheduleid for email template
                            if (mediaRows == null || mediaRows.Count == 0)
                            {
                                mediaRows = (from m in activityMediaList
                                                where m.OwnerID == activityEmailDetail.PatientID
                                                && m.CategoryCode == "TTC"
                                                && m.LanguagePreferenceCode == "EN"
                                                && m.FacilityID == activityEmailDetail.FacilityID
                                                select m).ToList();

                                //First get based on scheduleid for email template
                                if (mediaRows == null || mediaRows.Count == 0)
                                {
                                    mediaRows = (from m in activityMediaList
                                                    where m.OwnerID == activityEmailDetail.PatientID
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
                        appointmentSpecificMessage = _templateUtilities.ProperCase(media.Narrative);
                        apptSpecificMsgNode.InnerText = appointmentSpecificMessage;
                    }
                }
                else
                {
                    //Set enabled to false for Day 1
                    apptSpecificMsgNode.Attributes[XMLFields.Enable].Value = "false";
                    apptSpecificMsgNode.InnerText = string.Empty;
                }
            }
            else
            {
                //Set enabled to false for Day 1
                apptSpecificMsgNode.Attributes[XMLFields.Enable].Value = "false";
                apptSpecificMsgNode.InnerText = string.Empty;
            }
        }

        public void BuildIntroFacility(ref XmlDocument xdoc, EmailActivityDetail activityEmailDetail, List<ActivityMedia> activityMediaList, ref Hashtable missingObjects)
        {
            string facilityID = string.Empty;
            string facilityName = string.Empty;
            ActivityMedia media = null;
            string xpath = string.Empty;
            
            facilityID = activityEmailDetail.FacilityID.ToString();

            //FacilityID
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityID, _Mode);
            XmlNode facilityIDNode = xdoc.SelectSingleNode(xpath);
            facilityIDNode.InnerText = facilityID;

            //Facility Name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityName, _Mode);
            XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
            //check for email override
            media = (from m in activityMediaList
                        where m.OwnerID == activityEmailDetail.FacilityID
                        && m.CategoryCode == "SNOVR"
                        && m.OwnerCode == "EMAIL"
                        && m.LanguagePreferenceCode == "EN"
                        select m).FirstOrDefault();

            if (media != null)
            {
                facilityName = media.Narrative;
                if (!String.IsNullOrEmpty(facilityName) && !String.IsNullOrEmpty(facilityName.Trim()))
                {
                    facilityNameNode.InnerText = facilityName.Trim();
                }
                else
                {
                    string missingObjString = "Facility name " + facilityID;
                    missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                }
            }
            else
            {
                string missingObjString = "Facility name " + facilityID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }
        }

        public void BuildIntroEmailMessage(ref XmlDocument xdoc, EmailActivityDetail activityEmailDetail, List<ActivityMedia> activityMediaList, ref Hashtable missingObjects, int campaignID, int contactRoleID, int templateID)
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
            
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityName, _Mode);
            XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
            if (facilityNameNode != null)
            {
                facilityName = facilityNameNode.InnerText.ToString();
            }
                        
            activityID = activityEmailDetail.ActivityID.ToString();
            toEmailAddress = activityEmailDetail.ToEmailAddress;
            facilityID = activityEmailDetail.FacilityID.ToString();
            scheduleID = activityEmailDetail.RecipientSchedID.ToString();


            //ToEmailAddress
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeToEmailAddress, _Mode);
            XmlNode toEmailAddressNode = xdoc.SelectSingleNode(xpath);
            if (String.IsNullOrEmpty(toEmailAddress)
                || !Regex.IsMatch(toEmailAddress.Trim(), RegExPatterns.EmailAddressPatern))
            {
                string missingObjString = "ToEmailAddress" + activityID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }
            else
            {
                toEmailAddressNode.InnerText = toEmailAddress;
            }

            //FromEmailAddress
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFromEmailAddress, _Mode);
            XmlNode fromEmailAddressNode = xdoc.SelectSingleNode(xpath);
            media = (from m in activityMediaList
                        where m.OwnerID == activityEmailDetail.FacilityID
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
                fromEmailAddressNode.InnerText = fromEmailAddress.Trim();
            }
            else
            {
                //Get Configured Displayname based on Contract override
                media = (from m in activityMediaList
                            where m.OwnerID == -1
                            && m.CategoryCode == "EMAIL"
                            && m.OwnerCode == "EMAIL"
                            select m).FirstOrDefault();

                if (media != null)
                {
                    fromEmailAddress = media.Narrative;
                    if (!String.IsNullOrEmpty(fromEmailAddress) && !String.IsNullOrEmpty(fromEmailAddress.Trim()))
                    {
                        fromEmailAddressNode.InnerText = fromEmailAddress.Trim();
                    }
                    else
                    {
                        string missingObjString = "From Email Address not configured at facility" + facilityID + " and contract level "  ;
                        missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                    }
                }
                else
                {
                    string missingObjString = "From Email Address not configured at facility" + facilityID + " and contract level "  ;
                    missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                }                    
            }
                
            //ReplyToEmailAddress
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeReplyToEmailAddress, _Mode);
            XmlNode replyToEmailAddressNode = xdoc.SelectSingleNode(xpath);
                
            //Get reply address at facility level
            media = (from m in activityMediaList
                        where m.OwnerID == activityEmailDetail.FacilityID
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
                replyToEmailAddressNode.InnerText = replyToEmailAddress.Trim();
            }
            //Contract level reply address
            else
            {
                //Get Configured Displayname based on Contract override
                media = (from m in activityMediaList
                            where m.OwnerID == -1
                            && m.CategoryCode == "REPLY"
                            && m.OwnerCode == "EMAIL"
                            select m).FirstOrDefault();

                if (media != null)
                {
                    replyToEmailAddress = media.Narrative;
                    if (!String.IsNullOrEmpty(replyToEmailAddress) && !String.IsNullOrEmpty(replyToEmailAddress.Trim()))
                    {
                        replyToEmailAddressNode.InnerText = replyToEmailAddress.Trim();
                    }
                    else
                    {
                        string missingObjString = "Reply Email Address not configured at facility" + facilityID + " and contract level "  ;
                        missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                    }
                }
                else
                {
                    string missingObjString = "Reply Email Address not configured at facility" + facilityID + " and contract level ";
                    missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                }                    
            }

            //MessageType
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeMessageType, _Mode);
            XmlNode messageTypeNode = xdoc.SelectSingleNode(xpath);
            if (campaignID == (int)CampaignTypes.ACDefault)
            {
                messageType = MessageTypes.IntroductoryEmail;
                messageTypeNode.InnerText =  messageType;
            }

            //DisplayName
            //Defult display name to facility name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeDisplayName, _Mode);
            XmlNode displayNameNode = xdoc.SelectSingleNode(xpath);
            displayNameNode.InnerText = facilityName;

            //Get Configured Displayname based on ScheduleID override
            media = (from m in activityMediaList
                        where m.OwnerID == activityEmailDetail.RecipientSchedID
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
                media = (from m in activityMediaList
                            where m.OwnerID == activityEmailDetail.FacilityID
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
                    media = (from m in activityMediaList
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
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeMessageSubject, _Mode);
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

        public string BuildURL(string url, Dictionary<string, string> querystrings)
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
            return url;
        }

        public bool IsAppointmentSpecificMsgEnabled(List<ContractPermission> contractPermissions, int contactRoleID)
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
