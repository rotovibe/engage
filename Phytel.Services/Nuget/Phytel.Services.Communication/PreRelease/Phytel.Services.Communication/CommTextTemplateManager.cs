using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

using System.IO;

namespace Phytel.Services.Communication
{
    public class CommTextTemplateManager
    {
        private const string _Mode = "Text";
        private TemplateUtilities _templateUtilities;

        public CommTextTemplateManager(TemplateUtilities templateUtilities)
        {
            _templateUtilities = templateUtilities;
        }

        #region BuildTemplate

        public TemplateResults BuildHeader(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = _templateUtilities.BuildHeader(xdoc, textActivityDetail, missingObjects, _Mode);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildPatient(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = _templateUtilities.BuildPatient(xdoc, textActivityDetail, missingObjects, _Mode, new string[] { "PatientName" });
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildSchedule(XmlDocument xdoc, TextActivityDetail textActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults(); 
            
            string scheduleNameLF = string.Empty;
            string scheduleDisplayName = string.Empty;
            string xpath = string.Empty;
            List<ActivityMedia> mediaRows = null;

            scheduleNameLF = textActivityDetail.ScheduleName.ToString();

            TemplateResults utilityResults = _templateUtilities.BuildSchedule(xdoc, textActivityDetail, missingObjects, _Mode);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            //Contact Entities Schedule Name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeScheduleFullName, _Mode);
            XmlNode scheduleFullname = xdoc.SelectSingleNode(xpath);
            scheduleFullname.InnerText = scheduleNameLF;

            //Schedule Display name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeScheduleDisplayName, _Mode);
            XmlNode scheduleDisplayNameNode = xdoc.SelectSingleNode(xpath);
            mediaRows = (from m in activityMediaList
                         where m.OwnerID == textActivityDetail.RecipientSchedID
                            && m.CategoryCode == "SCOVR"
                            && m.OwnerCode == "TEXT"
                            && m.LanguagePreferenceCode == "EN"
                         select m).ToList();

            //Check to see if there is any records
            if (mediaRows != null && mediaRows.Count > 0)
            {
                foreach (ActivityMedia media in mediaRows)
                {
                    scheduleDisplayName = media.Narrative.ToString();
                    if (!String.IsNullOrEmpty(scheduleDisplayName) && !String.IsNullOrEmpty(scheduleDisplayName.Trim()))
                    {
                        scheduleDisplayName = _templateUtilities.ProperCase(scheduleDisplayName.Trim());
                        scheduleDisplayNameNode.InnerText = scheduleDisplayName;
                    }
                    else
                    {
                        //if there is no configured schedule name then default to your provider
                        scheduleDisplayName = "your provider";
                        scheduleDisplayNameNode.InnerText = scheduleDisplayName;
                    }
                }
            }
            else
            {
                //if there is no configured schedule name then default to your provider
                scheduleDisplayName = "your provider";
                scheduleDisplayNameNode.InnerText = scheduleDisplayName;
            }

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildFacility(XmlDocument xdoc, TextActivityDetail textActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            string facilityID = string.Empty;
            string facilityPhoneNumber = string.Empty;
            string facilityFullname = string.Empty;
            string facilityDisplayName = string.Empty;

            ActivityMedia media = null;
            string xpath = string.Empty;
            
            facilityID = textActivityDetail.FacilityID.ToString();
            facilityPhoneNumber = textActivityDetail.ProviderACDNumber.ToString();
            facilityFullname = textActivityDetail.FacilityName.ToString();
            
            //FacilityID
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityID, _Mode);
            XmlNode facilityIDNode = xdoc.SelectSingleNode(xpath);
            facilityIDNode.InnerText = facilityID;

            //Facility ContactEntities Name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityName, _Mode);
            XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
            facilityNameNode.InnerText = facilityFullname;

            //Facility phonenumber
            if (!String.IsNullOrEmpty(facilityPhoneNumber) && !String.IsNullOrEmpty(facilityPhoneNumber.Trim()))
            {
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityPhoneNumber, _Mode);
                XmlNode facilityPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                facilityPhoneNumberNode.InnerText = String.Format("{0:(###)###-####}", Convert.ToInt64(facilityPhoneNumber));
            }
            else
            {
                string missingObjString = "Facility/Schedule ACD Phone number is missing for facility ID:  " + facilityID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }

            //Facility Display name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityDisplayName, _Mode);
            XmlNode facilityDisplayNameNode = xdoc.SelectSingleNode(xpath);
            //check for email override
            media = (from m in activityMediaList
                     where m.OwnerID == textActivityDetail.FacilityID
                        && m.CategoryCode == "SNOVR"
                        && m.OwnerCode == "TEXT"
                        && m.LanguagePreferenceCode == "EN"
                     select m).FirstOrDefault();
            if (media != null)
            {
                facilityDisplayName = media.Narrative;
                if (!String.IsNullOrEmpty(facilityDisplayName) && !String.IsNullOrEmpty(facilityDisplayName.Trim()))
                {
                    facilityDisplayNameNode.InnerText = facilityDisplayName.Trim();
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

        private TemplateResults BuildTextMessage(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults(); 
            
            string appointmentDateTime = string.Empty;
            string activityID = string.Empty;
            string facilityID = string.Empty;
            string scheduleID = string.Empty;
            string patientId = string.Empty;
            string toTextNumber = string.Empty;
            string fromTextNumber = string.Empty;
            string helpTextNumber = string.Empty;
            string xpath = string.Empty;

            activityID = textActivityDetail.ActivityID.ToString();
            appointmentDateTime = textActivityDetail.ScheduleDateTime.ToString();
            facilityID = textActivityDetail.FacilityID.ToString();
            scheduleID = textActivityDetail.RecipientSchedID.ToString();
            patientId = textActivityDetail.PatientID.ToString();
            fromTextNumber = textActivityDetail.TextFromNumber.ToString(); //this is the short code or long code
            toTextNumber = textActivityDetail.PhoneNumber.ToString(); //this is the patient mobile number
            helpTextNumber = textActivityDetail.ProviderACDNumber.ToString(); // this is the number that will be used for help and in the message

            //Patient To phone number
            if (!String.IsNullOrEmpty(toTextNumber) && !String.IsNullOrEmpty(toTextNumber.Trim()))
            {
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeToPhoneNumber, _Mode);
                XmlNode toPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                toPhoneNumberNode.InnerText = toTextNumber;
            }
            else
            {
                string missingObjString = "Patient Phone number is missing for patient ID " + patientId;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }

            //Text From number
            if (!String.IsNullOrEmpty(fromTextNumber) && !String.IsNullOrEmpty(fromTextNumber.Trim()))
            {
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFromPhoneNumber, _Mode);
                XmlNode fromPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                fromPhoneNumberNode.InnerText = fromTextNumber;
            }
            else
            {
                string missingObjString = "From Phone number(Short Code) is missing for Activity ID " + activityID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }

            //Help number
            if (!String.IsNullOrEmpty(helpTextNumber) && !String.IsNullOrEmpty(helpTextNumber.Trim()))
            {
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeHelpPhoneNumber, _Mode);
                XmlNode helpPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                helpPhoneNumberNode.InnerText = helpTextNumber;
            }
            else
            {
                string missingObjString = "Help Phone Number is missing for Activity ID " + activityID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        private TemplateResults BuildApptDateTime(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = _templateUtilities.BuildApptDateTime(xdoc, textActivityDetail, missingObjects, _Mode, new string[] { "ScheduleDateTime" });
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        #endregion

        #region Intro Text Build Template

        private TemplateResults BuildIntroHeader(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = BuildHeader(xdoc, textActivityDetail, missingObjects);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        private TemplateResults BuildIntroTextPatient(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = BuildPatient(xdoc, textActivityDetail, missingObjects);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        private TemplateResults BuildIntroTextSchedule(XmlDocument xdoc, TextActivityDetail textActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults(); 
            
            string scheduleID = string.Empty; 
            string scheduleNameLF = string.Empty;
            string scheduleDisplayName = string.Empty;
            string xpath = string.Empty;
            List<ActivityMedia> mediaRows = null;

            scheduleID = textActivityDetail.RecipientSchedID.ToString();
            scheduleNameLF = textActivityDetail.ScheduleName.ToString();

            TemplateResults utilityResults = _templateUtilities.BuildSchedule(xdoc, textActivityDetail, missingObjects, _Mode);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            //Contact Entities Schedule Name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeScheduleFullName, _Mode);
            XmlNode scheduleFullname = xdoc.SelectSingleNode(xpath);
            scheduleFullname.InnerText = scheduleNameLF;

            //Schedule Display name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeScheduleDisplayName, _Mode);
            XmlNode scheduleDisplayNameNode = xdoc.SelectSingleNode(xpath);
            mediaRows = (from m in activityMediaList
                         where m.OwnerID == textActivityDetail.RecipientSchedID
                            && m.CategoryCode == "SCOVR"
                            && m.OwnerCode == "TEXT"
                            && m.LanguagePreferenceCode == "EN"
                         select m).ToList();

            //Check to see if there is any records
            if (mediaRows != null && mediaRows.Count > 0)
            {
                foreach (ActivityMedia media in mediaRows)
                {
                    scheduleDisplayName = media.Narrative;
                    if (!String.IsNullOrEmpty(scheduleDisplayName) && !String.IsNullOrEmpty(scheduleDisplayName.Trim()))
                    {
                        scheduleDisplayName = _templateUtilities.ProperCase(scheduleDisplayName.Trim());
                        scheduleDisplayNameNode.InnerText = scheduleDisplayName;
                    }
                    else
                    {
                        string missingObjString = "Schedule Name is missing for Schedule ID:  " + scheduleID;
                        missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
                    }
                }
            }
            else
            {
                string missingObjString = "Schedule Name is missing for Schedule ID:  " + scheduleID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        private TemplateResults BuildIntroTextFacility(XmlDocument xdoc, TextActivityDetail textActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = BuildFacility(xdoc, textActivityDetail, activityMediaList, missingObjects);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            int key = missingObjects.Count-1;                        
            string value = (string)missingObjects[key];

            if(value.Contains("Facility name"))
                missingObjects.Remove(key);

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        private TemplateResults BuildIntroTextMessage(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = BuildTextMessage(xdoc, textActivityDetail, missingObjects);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        private TemplateResults BuildIntroApptDateTime(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = _templateUtilities.BuildApptDateTime(xdoc, textActivityDetail, missingObjects, _Mode, new string[] { "" });
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        #endregion

     
    }
}
