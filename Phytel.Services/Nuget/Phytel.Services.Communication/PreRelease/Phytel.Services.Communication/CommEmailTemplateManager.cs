using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
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
            contractID = emailActivityDetail.ContractNumber;
            
            TemplateResults utilityResults = _templateUtilities.BuildHeader(xdoc, emailActivityDetail, missingObjects, _Mode);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            //ConfirmURL
            Dictionary<string, string> queryStrings = new Dictionary<string, string>
            {
                {"SendID", sendID},
                {"ContractID", contractID}
            };

            XmlNode confirmURLNode = xdoc.SelectSingleNode(string.Format(_templateUtilities.GetModeSpecificTag(XMLFields.ModeConfirmationURL, _Mode), "true"));
            confirmURL = BuildURL(confirmURL, queryStrings);
            _templateUtilities.SetCDATAXMlNodeInnerText(confirmURLNode, confirmURL, xdoc);
                  
            //OptOutURL
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

            TemplateResults utilityResults = _templateUtilities.BuildPatient(xdoc, emailActivityDetail, missingObjects, _Mode, new[] { "PatientName" });
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

            if (mediaRows.Count > 0)
            {
                foreach (ActivityMedia media in mediaRows)
                {
                    scheduleNameLF = media.Narrative;
                    if (!string.IsNullOrWhiteSpace(scheduleNameLF))
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
            if (!string.IsNullOrWhiteSpace(facilityState))
            {
                facilityState = facilityState.ToUpper();
            }
            _templateUtilities.SetXMlNodeInnerText(facilityStateNode, facilityState);

            //FacilityZip
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityZip, _Mode);
            XmlNode facilityZipNode = xdoc.SelectSingleNode(xpath);
            _templateUtilities.SetXMlNodeInnerText(facilityZipNode, facilityZip);

            //Facility phonenumber
            if (!string.IsNullOrWhiteSpace(facilityPhoneNumber))
            {
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityPhoneNumber, _Mode);
                XmlNode facilityPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                if (facilityPhoneNumberNode != null)
                    facilityPhoneNumberNode.InnerText = String.Format("{0:(###) ###-####}", Convert.ToInt64(facilityPhoneNumber));
            }
            else
            {
                string missingObjString = "Facility/Schedule ACD Phone number is missing for facility ID:  " + facilityID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }

            //Facility Logo
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityLogo, _Mode);
            XmlNode facilityLogoNode = xdoc.SelectSingleNode(xpath);
            media = activityMediaList.FirstOrDefault(m => m.OwnerID == emailActivityDetail.FacilityID
                                                          && m.CategoryCode == "LOGO"
                                                          && m.OwnerCode == "EMAIL");
            //Facility level
            if (media != null)
            {
                facilityLogo = media.Filename;
                _templateUtilities.SetXMlNodeInnerText(facilityLogoNode, facilityLogo);
            }
            else
            {
                //Contract level
                media = activityMediaList.FirstOrDefault(m => m.OwnerID == -1
                                                              && m.CategoryCode == "LOGO"
                                                              && m.OwnerCode == "EMAIL"
                                                              && !m.Filename.Contains("GLOBAL"));
                if (media != null)
                {
                    facilityLogo = media.Filename;
                    _templateUtilities.SetXMlNodeInnerText(facilityLogoNode, facilityLogo);
                }
                else 
                {
                    //Phytelmaster level
                    media = activityMediaList.FirstOrDefault(m => m.OwnerID == -1
                                                                  && m.CategoryCode == "LOGO"
                                                                  && m.OwnerCode == "EMAIL"
                                                                  && !m.Filename.Contains("GLOBAL"));
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
            media = activityMediaList.FirstOrDefault(m => m.OwnerID == emailActivityDetail.FacilityID
                                                          && m.CategoryCode == "URL"
                                                          && m.OwnerCode == "EMAIL");
            if (media != null)
            {
                facilityURL = media.Narrative;
                _templateUtilities.SetXMlNodeInnerText(facilityURLNode, facilityURL);
            }
            else
            {
                media = activityMediaList.FirstOrDefault(m => m.OwnerID == -1
                                                              && m.CategoryCode == "URL"
                                                              && m.OwnerCode == "EMAIL");
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

        public TemplateResults BuildEmailMessage(XmlDocument xdoc, EmailActivityDetail emailActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects, int campaignId)
        {
            TemplateResults results = new TemplateResults();

            // string messageSubject = string.Empty; Not used.
            string messageType = string.Empty;
            string patientFirstName = string.Empty;
            string patientLastName = string.Empty;
            string facilityName = string.Empty;

            TemplateResults emailResults = BuildEmailBase(xdoc, emailActivityDetail, activityMediaList, missingObjects);
            xdoc = emailResults.PopulatedTemplate;
            missingObjects = emailResults.MissingObjects;

            XmlNode facilityNameNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityName, _Mode));            
            if (facilityNameNode != null)
            {
                facilityName = facilityNameNode.InnerText;
            }

            XmlNode patientFirstNameNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModePatientFirstName, _Mode));
            if (patientFirstNameNode != null)
            {
                patientFirstName = patientFirstNameNode.InnerText;
            }

            XmlNode patientLastNameNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModePatientLastName, _Mode));
            if (patientLastNameNode != null)
            {
                patientLastName = patientLastNameNode.InnerText;
            }

            //MessageType
            XmlNode messageTypeNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModeMessageType, _Mode));
            switch (campaignId) 
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
            XmlNode messageSubjectNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModeMessageSubject, _Mode));
            if (messageSubjectNode != null && !string.IsNullOrEmpty(messageSubjectNode.InnerText))
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
                XmlNode apptDurationNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModeApptDuration, _Mode));
                _templateUtilities.SetXMlNodeInnerText(apptDurationNode, emailActivityDetail.ScheduleDuration.ToString());

                //Appointment date fields
                if (string.IsNullOrEmpty(emailActivityDetail.ScheduleDateTime) || Convert.ToDateTime(emailActivityDetail.ScheduleDateTime) < DateTime.Now)
                {
                    string missingObjString = "Appointment Datetime " + emailActivityDetail.ActivityID;
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

            TemplateResults utilityResults = _templateUtilities.BuildApptDateTime(xdoc, emailActivityDetail, missingObjects, _Mode, false, new[] { "ScheduleDateTime" });
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
            int contactRoleId, List<ContractPermission> contractPermissionRecords)
        {
            TemplateResults results = new TemplateResults();

            int contractLookUp = -1;

            //Appointment specific message
            XmlNode apptSpecificMsgNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModeAppointmentSpecificMessage, _Mode));

            if (IsAppointmentSpecificMsgEnabled(contractPermissionRecords, contactRoleId))
            {
                //First get based on email scheduleid
                List<ActivityMedia> mediaRows = activityMediaList.Where(m => m.OwnerID == emailActivityDetail.PatientID
                                                                             && m.CategoryCode == "TTC"
                                                                             && m.OwnerCode == "EMAIL"
                                                                             && m.LanguagePreferenceCode == "EN"
                                                                             && m.FacilityID == emailActivityDetail.RecipientSchedID).ToList();
              
                //If not available for schedule get based on facilityid
                if (mediaRows.Count == 0)
                {
                    mediaRows = activityMediaList.Where(m => m.OwnerID == emailActivityDetail.PatientID
                                                             && m.CategoryCode == "TTC"
                                                             && m.OwnerCode == "EMAIL"
                                                             && m.LanguagePreferenceCode == "EN"
                                                             && m.FacilityID == emailActivityDetail.FacilityID).ToList();
                  
                    //Finally get based on contract
                    if (mediaRows.Count == 0)
                    {
                        mediaRows = activityMediaList.Where(m => m.OwnerID == emailActivityDetail.PatientID
                                                                 && m.CategoryCode == "TTC"
                                                                 && m.OwnerCode == "EMAIL"
                                                                 && m.LanguagePreferenceCode == "EN"
                                                                 && m.FacilityID == contractLookUp).ToList();

                        //First get based on scheduleid for email template
                        if (mediaRows.Count == 0)
                        {
                            mediaRows = activityMediaList.Where(m => m.OwnerID == emailActivityDetail.PatientID
                                                                     && m.CategoryCode == "TTC"
                                                                     && m.LanguagePreferenceCode == "EN"
                                                                     && m.FacilityID == emailActivityDetail.RecipientSchedID).ToList();

                            //First get based on scheduleid for email template
                            if (mediaRows.Count == 0)
                            {
                                mediaRows = activityMediaList.Where(m => m.OwnerID == emailActivityDetail.PatientID
                                                                         && m.CategoryCode == "TTC"
                                                                         && m.LanguagePreferenceCode == "EN"
                                                                         && m.FacilityID == emailActivityDetail.FacilityID).ToList();

                                //First get based on scheduleid for email template
                                if (mediaRows.Count == 0)
                                {
                                    mediaRows = activityMediaList.Where(m => m.OwnerID == emailActivityDetail.PatientID
                                                                             && m.CategoryCode == "TTC"
                                                                             && m.LanguagePreferenceCode == "EN"
                                                                             && m.FacilityID == contractLookUp).ToList();
                                }
                            }
                        }
                    }
                }
                if (mediaRows.Count > 0)
                {
                    foreach (ActivityMedia media in mediaRows)
                    {
                        string appointmentSpecificMessage = _templateUtilities.ProperCase(media.Narrative);
                        _templateUtilities.SetXMlNodeInnerText(apptSpecificMsgNode, appointmentSpecificMessage);
                    }
                }
                else
                {
                    //Set enabled to false for Day 1
                    if (apptSpecificMsgNode != null)
                    {
                        apptSpecificMsgNode.Attributes[XMLFields.Enable].Value = "false";
                        _templateUtilities.SetXMlNodeInnerText(apptSpecificMsgNode, string.Empty);
                    }
                }
            }
            else
            {
                //Set enabled to false for Day 1
                if (apptSpecificMsgNode != null)
                {
                    apptSpecificMsgNode.Attributes[XMLFields.Enable].Value = "false";
                    _templateUtilities.SetXMlNodeInnerText(apptSpecificMsgNode, string.Empty);
                }
            }

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public string Transform(XmlDocument xml, TemplateDetail templateDetail)
        {
            var body = _templateUtilities.Transform(xml, templateDetail, _Mode);
            return body;
        }

        #endregion

        #region Build Intro Emails

        public TemplateResults BuildIntroPatient(XmlDocument xdoc, EmailActivityDetail emailActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            //PatientID
            XmlNode patientIdNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModePatientID, _Mode));
            _templateUtilities.SetXMlNodeInnerText(patientIdNode, emailActivityDetail.PatientID.ToString());

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

            string messageType = string.Empty;
            string facilityName = string.Empty;
            
            TemplateResults utilityResults = BuildEmailBase(xdoc, emailActivityDetail, activityMediaList, missingObjects);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            XmlNode facilityNameNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityName, _Mode));
            if (facilityNameNode != null)
            {
                facilityName = facilityNameNode.InnerText;
            }

            //MessageType
            XmlNode messageTypeNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModeMessageType, _Mode));
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
            XmlNode messageSubjectNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModeMessageSubject, _Mode));
            if (!string.IsNullOrWhiteSpace(facilityName))
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

            int i = 0;
            foreach (KeyValuePair<string, string> queryString in querystrings)
            {
                string encryptedString = dataProtector.Encrypt(queryString.Value);
                string key = queryString.Key;
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

        public bool IsAppointmentSpecificMsgEnabled(List<ContractPermission> contractPermissions, int contactRoleId)
        {
            bool isEnabled = false;
            try
            {
                if (contractPermissions != null && contractPermissions.Count > 0)
                {
                    List<ContractPermission> permissionRows =
                        contractPermissions.Where(cp => cp.ChildObjectID == (int) Prompts.AppointmentSpecificMessage 
                        && cp.RoleID == contactRoleId).ToList();

                    if (permissionRows.Count > 0)
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

            //FacilityID
            XmlNode facilityIdNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityID, _Mode));
            _templateUtilities.SetXMlNodeInnerText(facilityIdNode, emailActivityDetail.FacilityID.ToString());

            //Facility Name
            XmlNode facilityNameNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityName, _Mode));

            //Check for email override
            ActivityMedia media = activityMediaList.FirstOrDefault(m => m.OwnerID == emailActivityDetail.FacilityID
                                                                        && m.CategoryCode == "SNOVR"
                                                                        && m.OwnerCode == "EMAIL"
                                                                        && m.LanguagePreferenceCode == "EN");
            if (media != null)
            {
                string facilityName = media.Narrative;
                if (!string.IsNullOrWhiteSpace(facilityName))
                {
                    _templateUtilities.SetXMlNodeInnerText(facilityNameNode, facilityName.Trim());
                }
                else
                {
                    string missingObjString = "Facility name " + emailActivityDetail.FacilityID;
                    missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                }
            }
            else
            {
                string missingObjString = "Facility name " + emailActivityDetail.FacilityID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        private TemplateResults BuildEmailBase(XmlDocument xdoc, EmailActivityDetail emailActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            string fromEmailAddress = string.Empty;
            string replyToEmailAddress = string.Empty;
            string facilityName = string.Empty;

            //FacilityName
            XmlNode facilityNameNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityName, _Mode));
            if (facilityNameNode != null)
            {
                facilityName = facilityNameNode.InnerText;
            }

            //ToEmailAddress
            XmlNode toEmailAddressNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModeToEmailAddress, _Mode));
            if (_templateUtilities.IsEmailAddressFormatValid(emailActivityDetail.ToEmailAddress))
            {
                if (toEmailAddressNode != null) toEmailAddressNode.InnerText = emailActivityDetail.ToEmailAddress;
            }
            else
            {
                string missingObjString = "ToEmailAddress" + emailActivityDetail.ActivityID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }

            //FromEmailAddress
            XmlNode fromEmailAddressNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModeFromEmailAddress, _Mode));
            ActivityMedia media = activityMediaList.FirstOrDefault(m => m.OwnerID == emailActivityDetail.FacilityID
                                                                        && m.CategoryCode == "EMAIL"
                                                                        && m.OwnerCode == "EMAIL");
            if (media != null)
            {
                //Set default reply email address
                fromEmailAddress = media.Narrative;
            }

            if (!string.IsNullOrWhiteSpace(fromEmailAddress))
            {
                _templateUtilities.SetXMlNodeInnerText(fromEmailAddressNode, fromEmailAddress.Trim());
            }
            else
            {
                //Get Configured Displayname based on Contract override
                media = activityMediaList.FirstOrDefault(m => m.OwnerID == -1
                                                              && m.CategoryCode == "EMAIL"
                                                              && m.OwnerCode == "EMAIL");
                if (media != null)
                {
                    fromEmailAddress = media.Narrative;
                    if (!string.IsNullOrWhiteSpace(fromEmailAddress))
                    {
                        _templateUtilities.SetXMlNodeInnerText(fromEmailAddressNode, fromEmailAddress.Trim());
                    }
                    else
                    {
                        string missingObjString = "From Email Address not configured at facility" + emailActivityDetail.FacilityID + " and contract level ";
                        missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                    }
                }
                else
                {
                    string missingObjString = "From Email Address not configured at facility" + emailActivityDetail.FacilityID + " and contract level ";
                    missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                }
            }

            //ReplyToEmailAddress
            XmlNode replyToEmailAddressNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModeReplyToEmailAddress, _Mode));

            //Get reply address at facility level
            media = activityMediaList.FirstOrDefault(m => m.OwnerID == emailActivityDetail.FacilityID
                                                          && m.CategoryCode == "REPLY"
                                                          && m.OwnerCode == "EMAIL");
            if (media != null)
            {
                replyToEmailAddress = media.Narrative;
            }

            //Facility level reply address
            if (!string.IsNullOrWhiteSpace(replyToEmailAddress))
            {
                if (replyToEmailAddressNode != null) replyToEmailAddressNode.InnerText = replyToEmailAddress.Trim();
            }

            //Contract level reply address
            else
            {
                //Get Configured Displayname based on Contract override
                media = activityMediaList.FirstOrDefault(m => m.OwnerID == -1
                                                              && m.CategoryCode == "REPLY"
                                                              && m.OwnerCode == "EMAIL");
                if (media != null)
                {
                    replyToEmailAddress = media.Narrative;
                    if (!string.IsNullOrWhiteSpace(replyToEmailAddress))
                    {
                        _templateUtilities.SetXMlNodeInnerText(replyToEmailAddressNode, replyToEmailAddress.Trim());
                    }
                    else
                    {
                        string missingObjString = "Reply Email Address not configured at facility" + emailActivityDetail.FacilityID + " and contract level ";
                        missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                    }
                }
                else
                {
                    string missingObjString = "Reply Email Address not configured at facility" + emailActivityDetail.FacilityID + " and contract level ";
                    missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                }
            }

            //DisplayName
            //Default display name to facility name
            XmlNode displayNameNode = xdoc.SelectSingleNode(_templateUtilities.GetModeSpecificTag(XMLFields.ModeDisplayName, _Mode));
            if (displayNameNode != null)
            {
                displayNameNode.InnerText = facilityName;

                //Get Configured Displayname based on ScheduleID override
                media = activityMediaList.FirstOrDefault(m => m.OwnerID == emailActivityDetail.RecipientSchedID
                                                              && m.CategoryCode == "NAME"
                                                              && m.OwnerCode == "EMAIL");
                if (media != null)
                {
                    _templateUtilities.SetXMlNodeInnerText(displayNameNode, media.Narrative);
                }
                else
                {
                    //Get Configured Displayname based on FacilityID override
                    media = activityMediaList.FirstOrDefault(m => m.OwnerID == emailActivityDetail.FacilityID
                                                                  && m.CategoryCode == "NAME"
                                                                  && m.OwnerCode == "EMAIL");
                    if (media != null)
                    {
                        _templateUtilities.SetXMlNodeInnerText(displayNameNode, media.Narrative);
                    }
                    else
                    {
                        //Get Configured Displayname based on Contract override
                        media = activityMediaList.FirstOrDefault(m => m.OwnerID == -1
                                                                      && m.CategoryCode == "NAME"
                                                                      && m.OwnerCode == "EMAIL");     
                        if (media != null)
                        {
                            _templateUtilities.SetXMlNodeInnerText(displayNameNode, media.Narrative);
                        }
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
