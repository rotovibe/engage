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
using System.Xml.Xsl;
using System.IO;

namespace Phytel.Services.Communication
{
    public class CommEmailTemplateManager : ICommEmailTemplateManager
    {
        private const string _Mode = "Email";
        private ITemplateUtilities _templateUtilities;

        public CommEmailTemplateManager(ITemplateUtilities templateUtilities)
        {
            _templateUtilities = templateUtilities;
        }

        #region Build Emails

        public TemplateResults BuildHeader(XmlDocument xdoc, EmailActivityDetail emailActivityDetail, Hashtable missingObjects, string confirmURL, string optoutURL)
        {
            TemplateResults results = new TemplateResults();

            string sendID = string.Empty;
            string contractID = string.Empty;
            string rescheduleURL = string.Empty;
            string cancelURL = string.Empty;
            string xpath = string.Empty;            

            sendID = emailActivityDetail.SendID.ToString();
            contractID = emailActivityDetail.ContractNumber.ToString();
            
            TemplateResults utilityResults = _templateUtilities.BuildHeader(xdoc, emailActivityDetail, missingObjects, _Mode);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            //ConfirmURL
            Dictionary<string, string> queryStrings = new Dictionary<string, string>();
            queryStrings.Add("SendID", sendID);
            queryStrings.Add("ContractID", contractID);

            XmlNode confirmURLNode = xdoc.SelectSingleNode(string.Format(_templateUtilities.GetModeSpecificTag(XMLFields.ModeConfirmationURL, _Mode), "true"));
            confirmURL = BuildURL(confirmURL, queryStrings);
            _templateUtilities.SetCDATAXMlNodeInnerText(confirmURLNode, confirmURL, xdoc);
                  
            //OptOutURL
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeOptOutURL, _Mode);
            XmlNode optOutURLNode = xdoc.SelectSingleNode(string.Format(_templateUtilities.GetModeSpecificTag(XMLFields.ModeOptOutURL, _Mode), "true"));
            optoutURL = BuildURL(optoutURL, queryStrings);
            _templateUtilities.SetCDATAXMlNodeInnerText(optOutURLNode, optoutURL, xdoc);

            //Reschedule optional
            XmlNode rescheduleURLNode = xdoc.SelectSingleNode(string.Format(_templateUtilities.GetModeSpecificTag(XMLFields.ModeRescheduleURL, _Mode), "true"));
            rescheduleURL = BuildURL(rescheduleURL, queryStrings);
            _templateUtilities.SetCDATAXMlNodeInnerText(rescheduleURLNode, rescheduleURL, xdoc);

            //Cancel optional
            XmlNode cancelURLNode = xdoc.SelectSingleNode(string.Format(_templateUtilities.GetModeSpecificTag(XMLFields.ModeCancelURL, _Mode), "true"));
            cancelURL = BuildURL(cancelURL, queryStrings);
            _templateUtilities.SetCDATAXMlNodeInnerText(cancelURLNode, cancelURL, xdoc);

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildPatient(XmlDocument xdoc, EmailActivityDetail emailActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = _templateUtilities.BuildPatient(xdoc, emailActivityDetail, missingObjects, _Mode, new string[] { "PatientName" });
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildSchedule(XmlDocument xdoc, EmailActivityDetail emailActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            string scheduleNameLF = string.Empty;
            string xpath = string.Empty;
            List<ActivityMedia> mediaRows = null;

            TemplateResults utilityResults = _templateUtilities.BuildSchedule(xdoc, emailActivityDetail, missingObjects, _Mode);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeScheduleFullName, _Mode);
            XmlNode ScheduleNameLFNode = xdoc.SelectSingleNode(xpath);
            mediaRows = (from m in activityMediaList
                         where m.OwnerID == emailActivityDetail.RecipientSchedID
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
                        _templateUtilities.SetXMlNodeInnerText(ScheduleNameLFNode, scheduleNameLF);
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

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildFacility(XmlDocument xdoc, EmailActivityDetail emailActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects, string taskTypeCategory)
        {
            TemplateResults results = new TemplateResults();

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

            facilityID = emailActivityDetail.FacilityID.ToString();
            facilityAddr1 = emailActivityDetail.FacilityAddrLine1;
            facilityAddr2 = emailActivityDetail.FacilityAddrLine2;
            facilityCity = emailActivityDetail.FacilityCity;
            facilityState = emailActivityDetail.FacilityState;
            facilityZip = emailActivityDetail.FacilityZipCode;
            facilityPhoneNumber = emailActivityDetail.ProviderACDNumber;

            TemplateResults utilityResults = BuildEmailFacility(xdoc, emailActivityDetail, activityMediaList, missingObjects);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            //FacilityAddr1
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityAddr1, _Mode);
            XmlNode facilityAddr1Node = xdoc.SelectSingleNode(xpath);
            facilityAddr1 = _templateUtilities.ProperCase(facilityAddr1);
            _templateUtilities.SetXMlNodeInnerText(facilityAddr1Node, facilityAddr1);

            //FacilityAddr2
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityAddr2, _Mode);
            XmlNode facilityAddr2Node = xdoc.SelectSingleNode(xpath);
            facilityAddr2 = _templateUtilities.ProperCase(facilityAddr2);
            _templateUtilities.SetXMlNodeInnerText(facilityAddr2Node, facilityAddr2);

            //FacilityCity
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityCity, _Mode);
            XmlNode facilityCityNode = xdoc.SelectSingleNode(xpath);
            facilityCity = _templateUtilities.ProperCase(facilityCity);
            _templateUtilities.SetXMlNodeInnerText(facilityCityNode, facilityCity);

            //FacilityState
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityState, _Mode);
            XmlNode facilityStateNode = xdoc.SelectSingleNode(xpath);
            if (!String.IsNullOrEmpty(facilityState) && !String.IsNullOrEmpty(facilityState.Trim()))
            {
                facilityState = facilityState.ToUpper();
            }
            _templateUtilities.SetXMlNodeInnerText(facilityStateNode, facilityState);

            //FacilityZip
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityZip, _Mode);
            XmlNode facilityZipNode = xdoc.SelectSingleNode(xpath);
            _templateUtilities.SetXMlNodeInnerText(facilityZipNode, facilityZip);

            //Facility phonenumber
            if (!String.IsNullOrEmpty(facilityPhoneNumber) && !String.IsNullOrEmpty(facilityPhoneNumber.Trim()))
            {
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityPhoneNumber, _Mode);
                XmlNode facilityPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                _templateUtilities.SetXMlNodeInnerText(facilityPhoneNumberNode, String.Format("{0:(###) ###-####}", Convert.ToInt64(facilityPhoneNumber)));
            }
            else
            {
                string missingObjString = "Facility/Schedule ACD Phone number is missing for facility ID:  " + facilityID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }

            //Facility Logo
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityLogo, _Mode);
            XmlNode facilityLogoNode = xdoc.SelectSingleNode(xpath);
            media = (from m in activityMediaList
                     where m.OwnerID == emailActivityDetail.FacilityID
                        && m.CategoryCode == "LOGO"
                        && m.OwnerCode == "EMAIL"
                        select m).FirstOrDefault();
            //Facility level
            if (media != null)
            {
                facilityLogo = media.Filename;
                _templateUtilities.SetXMlNodeInnerText(facilityLogoNode, facilityLogo);
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
                    _templateUtilities.SetXMlNodeInnerText(facilityLogoNode, facilityLogo);
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
                        _templateUtilities.SetXMlNodeInnerText(facilityLogoNode, facilityLogo);
                    }
                    else
                    {
                        //Set enabled to false for Day 1
                        facilityLogoNode.Attributes[XMLFields.Enable].Value = "false";
                        _templateUtilities.SetXMlNodeInnerText(facilityLogoNode, string.Empty);
                    }
                }
            }

            // Facility URL
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityURL, _Mode);
            XmlNode facilityURLNode = xdoc.SelectSingleNode(xpath);
            media = (from m in activityMediaList
                     where m.OwnerID == emailActivityDetail.FacilityID
                        && m.CategoryCode == "URL"
                        && m.OwnerCode == "EMAIL"
                        select m).FirstOrDefault();
            if (media != null)
            {
                facilityURL = media.Narrative;
                _templateUtilities.SetXMlNodeInnerText(facilityURLNode, facilityURL);
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
                    _templateUtilities.SetXMlNodeInnerText(facilityURLNode, facilityURL);
                }
                else
                {
                    //Set enabled to false for Day 1
                    facilityURLNode.Attributes[XMLFields.Enable].Value = "false";
                    _templateUtilities.SetXMlNodeInnerText(facilityURLNode, string.Empty);
                }
            }

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildEmailMessage(XmlDocument xdoc, EmailActivityDetail emailActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects, int campaignID)
        {
            TemplateResults results = new TemplateResults();

            string appointmentDateTime = string.Empty;
            string appointmentDuration = string.Empty;
            string messageSubject = string.Empty;
            string activityID = string.Empty;
            string messageType = string.Empty;
            string patientFirstName = string.Empty;
            string patientLastName = string.Empty;
            string facilityName = string.Empty;
            
            string xpath = string.Empty;

            TemplateResults emailResults = BuildEmailBase(xdoc, emailActivityDetail, activityMediaList, missingObjects);
            xdoc = emailResults.PopulatedTemplate;
            missingObjects = emailResults.MissingObjects;
            
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityName, _Mode);
            XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
            if (facilityNameNode != null)
            {
                facilityName = facilityNameNode.InnerText.ToString();
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

            activityID = emailActivityDetail.ActivityID.ToString();
            appointmentDateTime = emailActivityDetail.ScheduleDateTime;
            appointmentDuration = emailActivityDetail.ScheduleDuration.ToString();

            //MessageType
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeMessageType, _Mode);
            XmlNode messageTypeNode = xdoc.SelectSingleNode(xpath);
            switch (campaignID) 
            {
                case (int)CampaignTypes.ACDefault:
                    messageType = MessageTypes.AppointmentReminder;
                    _templateUtilities.SetXMlNodeInnerText(messageTypeNode, messageType);
                    break;

                case (int)CampaignTypes.OutreachDefault:
                    messageType = MessageTypes.OutreachRecall;
                    _templateUtilities.SetXMlNodeInnerText(messageTypeNode, messageType);
                    break;
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
                _templateUtilities.SetXMlNodeInnerText(messageSubjectNode, messageType);
            }

            if (messageType != MessageTypes.OutreachRecall)
            {
                //Set Appointment Duration
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeApptDuration, _Mode);
                XmlNode apptDurationNode = xdoc.SelectSingleNode(xpath);
                _templateUtilities.SetXMlNodeInnerText(apptDurationNode, appointmentDuration);

                //Appointment date fields
                if (String.IsNullOrEmpty(appointmentDateTime) || Convert.ToDateTime(appointmentDateTime) < System.DateTime.Now)
                {
                    string missingObjString = "Appointment Datetime " + activityID;
                    missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                }
                else
                {
                    TemplateResults apptDateTimeResults = BuildApptDateTime(xdoc, emailActivityDetail, missingObjects);
                    xdoc = apptDateTimeResults.PopulatedTemplate;
                    missingObjects = apptDateTimeResults.MissingObjects;
                }                
            }

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildApptDateTime(XmlDocument xdoc, EmailActivityDetail emailActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();
            string appointmentDuration = string.Empty;
            string xpath = string.Empty;
            appointmentDuration = emailActivityDetail.ScheduleDuration.ToString();

            TemplateResults utilityResults = _templateUtilities.BuildApptDateTime(xdoc, emailActivityDetail, missingObjects, _Mode, false, new string[] { "ScheduleDateTime" });
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            //Set Appointment Duration
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeApptDuration, _Mode);
            XmlNode apptDurationNode = xdoc.SelectSingleNode(xpath);
            _templateUtilities.SetXMlNodeInnerText(apptDurationNode, appointmentDuration);

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildAppointmentSpecificMessage(XmlDocument xdoc, EmailActivityDetail emailActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects, 
            int contactRoleID, List<ContractPermission> contractPermissionRecords)
        {
            TemplateResults results = new TemplateResults();

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

            activityID = emailActivityDetail.ActivityID.ToString();
            facilityID = emailActivityDetail.FacilityID.ToString();
            scheduleID = emailActivityDetail.RecipientSchedID.ToString();
            patientId = emailActivityDetail.PatientID.ToString();

            if (IsAppointmentSpecificMsgEnabled(contractPermissionRecords, contactRoleID))
            {
                //First get based on email scheduleid
                mediaRows = (from m in activityMediaList
                            where m.OwnerID == emailActivityDetail.PatientID
                            && m.CategoryCode == "TTC"
                            && m.OwnerCode == "EMAIL"
                            && m.LanguagePreferenceCode == "EN"
                            && m.FacilityID == emailActivityDetail.RecipientSchedID
                            select m).ToList();
              
                //If not available for schedule get based on facilityid
                if (mediaRows == null || mediaRows.Count == 0)
                {
                    mediaRows = (from m in activityMediaList
                                    where m.OwnerID == emailActivityDetail.PatientID
                                    && m.CategoryCode == "TTC"
                                    && m.OwnerCode == "EMAIL"
                                    && m.LanguagePreferenceCode == "EN"
                                    && m.FacilityID == emailActivityDetail.FacilityID
                                    select m).ToList();
                  
                    //Finally get based on contract
                    if (mediaRows == null || mediaRows.Count == 0)
                    {
                        mediaRows = (from m in activityMediaList
                                        where m.OwnerID == emailActivityDetail.PatientID
                                        && m.CategoryCode == "TTC"
                                        && m.OwnerCode == "EMAIL"
                                        && m.LanguagePreferenceCode == "EN"
                                        && m.FacilityID == contractLookUp
                                        select m).ToList();

                        //First get based on scheduleid for email template
                        if (mediaRows == null || mediaRows.Count == 0)
                        {
                            mediaRows = (from m in activityMediaList
                                            where m.OwnerID == emailActivityDetail.PatientID
                                            && m.CategoryCode == "TTC"
                                            && m.LanguagePreferenceCode == "EN"
                                            && m.FacilityID == emailActivityDetail.RecipientSchedID
                                            select m).ToList();

                            //First get based on scheduleid for email template
                            if (mediaRows == null || mediaRows.Count == 0)
                            {
                                mediaRows = (from m in activityMediaList
                                                where m.OwnerID == emailActivityDetail.PatientID
                                                && m.CategoryCode == "TTC"
                                                && m.LanguagePreferenceCode == "EN"
                                                && m.FacilityID == emailActivityDetail.FacilityID
                                                select m).ToList();

                                //First get based on scheduleid for email template
                                if (mediaRows == null || mediaRows.Count == 0)
                                {
                                    mediaRows = (from m in activityMediaList
                                                    where m.OwnerID == emailActivityDetail.PatientID
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
                        _templateUtilities.SetXMlNodeInnerText(apptSpecificMsgNode, appointmentSpecificMessage);
                    }
                }
                else
                {
                    //Set enabled to false for Day 1
                    apptSpecificMsgNode.Attributes[XMLFields.Enable].Value = "false";
                    _templateUtilities.SetXMlNodeInnerText(apptSpecificMsgNode, string.Empty);
                }
            }
            else
            {
                //Set enabled to false for Day 1
                apptSpecificMsgNode.Attributes[XMLFields.Enable].Value = "false";
                _templateUtilities.SetXMlNodeInnerText(apptSpecificMsgNode, string.Empty);
            }

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public string Transform(XmlDocument xml, TemplateDetail templateDetail)
        {
            string body = string.Empty;

            body = _templateUtilities.Transform(xml, templateDetail, _Mode);

            return body;
        }

        #endregion

        #region Build Intro Emails

        public TemplateResults BuildIntroPatient(XmlDocument xdoc, EmailActivityDetail emailActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            string xpath = string.Empty;

            //PatientID
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModePatientID, _Mode);
            XmlNode patientIDNode = xdoc.SelectSingleNode(xpath);
            _templateUtilities.SetXMlNodeInnerText(patientIDNode, emailActivityDetail.PatientID.ToString());

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildIntroFacility(XmlDocument xdoc, EmailActivityDetail emailActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = BuildEmailFacility(xdoc, emailActivityDetail, activityMediaList, missingObjects);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildIntroEmailMessage(XmlDocument xdoc, EmailActivityDetail emailActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects, int campaignID)
        {
            TemplateResults results = new TemplateResults();

            string messageSubject = string.Empty;
            string messageType = string.Empty;
            string facilityName = string.Empty;
            
            TemplateResults utilityResults = BuildEmailBase(xdoc, emailActivityDetail, activityMediaList, missingObjects);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            string xpath = string.Empty;
            
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityName, _Mode);
            XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
            if (facilityNameNode != null)
            {
                facilityName = facilityNameNode.InnerText.ToString();
            }
                        
            //MessageType
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeMessageType, _Mode);
            XmlNode messageTypeNode = xdoc.SelectSingleNode(xpath);
            switch (campaignID)
            {
                case (int)CampaignTypes.ACDefault:
                case (int)CampaignTypes.OutreachDefault:
                    messageType = MessageTypes.IntroductoryEmail;
                    _templateUtilities.SetXMlNodeInnerText(messageTypeNode, messageType);
                    break;
            }
             
            //Subject 
            //Don't set inner text via SetXMlNodeInnerText as facilityname XML characters will be decoded again
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeMessageSubject, _Mode);
            XmlNode messageSubjectNode = xdoc.SelectSingleNode(xpath);
            if (!String.IsNullOrEmpty(facilityName)
                    && !String.IsNullOrEmpty(facilityName.Trim()))
            {
                _templateUtilities.SetXMlNodeInnerText(messageSubjectNode, "A message from " + facilityName);
            }
            else
            {
                _templateUtilities.SetXMlNodeInnerText(messageSubjectNode, messageType);
            }

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        #endregion

        #region Helpers

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

        private TemplateResults BuildEmailFacility(XmlDocument xdoc, EmailActivityDetail emailActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            string facilityID = string.Empty;
            string facilityName = string.Empty;
            ActivityMedia media = null;
            string xpath = string.Empty;

            facilityID = emailActivityDetail.FacilityID.ToString();

            //FacilityID
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityID, _Mode);
            XmlNode facilityIDNode = xdoc.SelectSingleNode(xpath);
            _templateUtilities.SetXMlNodeInnerText(facilityIDNode, facilityID);

            //Facility Name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityName, _Mode);
            XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
            //check for email override
            media = (from m in activityMediaList
                     where m.OwnerID == emailActivityDetail.FacilityID
                     && m.CategoryCode == "SNOVR"
                     && m.OwnerCode == "EMAIL"
                     && m.LanguagePreferenceCode == "EN"
                     select m).FirstOrDefault();

            if (media != null)
            {
                facilityName = media.Narrative;
                if (!String.IsNullOrEmpty(facilityName) && !String.IsNullOrEmpty(facilityName.Trim()))
                {
                    _templateUtilities.SetXMlNodeInnerText(facilityNameNode, facilityName.Trim());
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

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        private TemplateResults BuildEmailBase(XmlDocument xdoc, EmailActivityDetail emailActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            string toEmailAddress = string.Empty;
            string fromEmailAddress = string.Empty;
            string replyToEmailAddress = string.Empty;
            string activityID = string.Empty;
            string facilityID = string.Empty;
            string facilityName = string.Empty;

            ActivityMedia media = null;
            string xpath = string.Empty;

            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityName, _Mode);
            XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
            if (facilityNameNode != null)
            {
                facilityName = facilityNameNode.InnerText.ToString();
            }

            activityID = emailActivityDetail.ActivityID.ToString();
            toEmailAddress = emailActivityDetail.ToEmailAddress;
            facilityID = emailActivityDetail.FacilityID.ToString();

            //ToEmailAddress
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeToEmailAddress, _Mode);
            XmlNode toEmailAddressNode = xdoc.SelectSingleNode(xpath);
            if (_templateUtilities.IsEmailAddressFormatValid(toEmailAddress))
            {
                _templateUtilities.SetXMlNodeInnerText(toEmailAddressNode, toEmailAddress);
            }
            else
            {
                string missingObjString = "ToEmailAddress" + activityID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }

            //FromEmailAddress
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFromEmailAddress, _Mode);
            XmlNode fromEmailAddressNode = xdoc.SelectSingleNode(xpath);
            media = (from m in activityMediaList
                     where m.OwnerID == emailActivityDetail.FacilityID
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
                _templateUtilities.SetXMlNodeInnerText(fromEmailAddressNode, fromEmailAddress.Trim());
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
                        _templateUtilities.SetXMlNodeInnerText(fromEmailAddressNode, fromEmailAddress.Trim());
                    }
                    else
                    {
                        string missingObjString = "From Email Address not configured at facility" + facilityID + " and contract level ";
                        missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                    }
                }
                else
                {
                    string missingObjString = "From Email Address not configured at facility" + facilityID + " and contract level ";
                    missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                }
            }

            //ReplyToEmailAddress
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeReplyToEmailAddress, _Mode);
            XmlNode replyToEmailAddressNode = xdoc.SelectSingleNode(xpath);

            //Get reply address at facility level
            media = (from m in activityMediaList
                     where m.OwnerID == emailActivityDetail.FacilityID
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
                _templateUtilities.SetXMlNodeInnerText(replyToEmailAddressNode, replyToEmailAddress.Trim());
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
                        _templateUtilities.SetXMlNodeInnerText(replyToEmailAddressNode, replyToEmailAddress.Trim());
                    }
                    else
                    {
                        string missingObjString = "Reply Email Address not configured at facility" + facilityID + " and contract level ";
                        missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                    }
                }
                else
                {
                    string missingObjString = "Reply Email Address not configured at facility" + facilityID + " and contract level ";
                    missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                }
            }

            //DisplayName
            //Default display name to facility name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeDisplayName, _Mode);
            XmlNode displayNameNode = xdoc.SelectSingleNode(xpath);
            _templateUtilities.SetXMlNodeInnerText(displayNameNode, facilityName);

            //Get Configured Displayname based on ScheduleID override
            media = (from m in activityMediaList
                     where m.OwnerID == emailActivityDetail.RecipientSchedID
                        && m.CategoryCode == "NAME"
                        && m.OwnerCode == "EMAIL"
                     select m).FirstOrDefault();
            if (media != null)
            {
                _templateUtilities.SetXMlNodeInnerText(displayNameNode, media.Narrative);
            }
            else
            {
                //Get Configured Displayname based on FacilityID override
                media = (from m in activityMediaList
                         where m.OwnerID == emailActivityDetail.FacilityID
                            && m.CategoryCode == "NAME"
                            && m.OwnerCode == "EMAIL"
                         select m).FirstOrDefault();
                if (media != null)
                {
                    _templateUtilities.SetXMlNodeInnerText(displayNameNode, media.Narrative);
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
                        _templateUtilities.SetXMlNodeInnerText(displayNameNode, media.Narrative);
                    }
                }
            }

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        #endregion
    }
}
