using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Phytel.Services.Communication
{
    public class CommTextTemplateManager : ICommTextTemplateManager
    {
        private const string Mode = "Text";
        private readonly ITemplateUtilities _templateUtilities;

        public CommTextTemplateManager(ITemplateUtilities templateUtilities)
        {
            _templateUtilities = templateUtilities;
        }

        #region BuildTemplate

        public TemplateResults BuildHeader(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = _templateUtilities.BuildHeader(xdoc, textActivityDetail, missingObjects, Mode);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildPatient(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = _templateUtilities.BuildPatient(xdoc, textActivityDetail, missingObjects, Mode, new[] { "PatientName" });
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

            scheduleNameLF = textActivityDetail.ScheduleName;

            TemplateResults utilityResults = _templateUtilities.BuildSchedule(xdoc, textActivityDetail, missingObjects, Mode);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            //Contact Entities Schedule Name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeScheduleFullName, Mode);
            XmlNode scheduleFullname = xdoc.SelectSingleNode(xpath);
            _templateUtilities.SetXMlNodeInnerText(scheduleFullname, scheduleNameLF);

            //Schedule Display name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeScheduleDisplayName, Mode);
            XmlNode scheduleDisplayNameNode = xdoc.SelectSingleNode(xpath);
            List<ActivityMedia> mediaRows =
                activityMediaList.Where(m => m.OwnerID == textActivityDetail.RecipientSchedID
                                             && m.CategoryCode == "SCOVR"
                                             && m.OwnerCode == "TEXT"
                                             && m.LanguagePreferenceCode == "EN").ToList();
            //Check to see if there is any records
            if (mediaRows.Count > 0)
            {
                foreach (var media in mediaRows)
                {
                    scheduleDisplayName = media.Narrative;
                    if (!string.IsNullOrEmpty(scheduleDisplayName) && !string.IsNullOrEmpty(scheduleDisplayName.Trim()))
                    {
                        scheduleDisplayName = _templateUtilities.ProperCase(scheduleDisplayName.Trim());
                        _templateUtilities.SetXMlNodeInnerText(scheduleDisplayNameNode, scheduleDisplayName);
                    }
                    else
                    {
                        //if there is no configured schedule name then default to your provider
                        scheduleDisplayName = "your provider";
                        _templateUtilities.SetXMlNodeInnerText(scheduleDisplayNameNode, scheduleDisplayName);
                    }
                }
            }
            else
            {
                //if there is no configured schedule name then default to your provider
                scheduleDisplayName = "your provider";
                _templateUtilities.SetXMlNodeInnerText(scheduleDisplayNameNode, scheduleDisplayName);
            }

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildFacility(XmlDocument xdoc, TextActivityDetail textActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            string facilityID = textActivityDetail.FacilityID.ToString();
            string facilityPhoneNumber = textActivityDetail.ProviderACDNumber;
            string facilityFullname = textActivityDetail.FacilityName;
            
            //FacilityID
            string xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityID, Mode);
            XmlNode facilityIDNode = xdoc.SelectSingleNode(xpath);
            _templateUtilities.SetXMlNodeInnerText(facilityIDNode, facilityID);

            //Facility ContactEntities Name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityName, Mode);
            XmlNode facilityNameNode = xdoc.SelectSingleNode(xpath);
            _templateUtilities.SetXMlNodeInnerText(facilityNameNode, facilityFullname);

            //Facility phonenumber
            if (!string.IsNullOrEmpty(facilityPhoneNumber) && !string.IsNullOrEmpty(facilityPhoneNumber.Trim()))
            {
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityPhoneNumber, Mode);
                XmlNode facilityPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                _templateUtilities.SetXMlNodeInnerText(facilityPhoneNumberNode, string.Format("{0:(###)###-####}", Convert.ToInt64(facilityPhoneNumber)));
            }
            else
            {
                string missingObjString = "Facility/Schedule ACD Phone number is missing for facility ID:  " + facilityID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }

            //Facility Display name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFacilityDisplayName, Mode);
            XmlNode facilityDisplayNameNode = xdoc.SelectSingleNode(xpath);

            //check for email override
            ActivityMedia media = activityMediaList.FirstOrDefault(m => m.OwnerID == textActivityDetail.FacilityID
                                                                        && m.CategoryCode == "SNOVR"
                                                                        && m.OwnerCode == "TEXT"
                                                                        && m.LanguagePreferenceCode == "EN");
            if (media != null)
            {
                string facilityDisplayName = media.Narrative;
                if (!string.IsNullOrEmpty(facilityDisplayName) && !string.IsNullOrEmpty(facilityDisplayName.Trim()))
                {
                    _templateUtilities.SetXMlNodeInnerText(facilityDisplayNameNode, facilityDisplayName.Trim());
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

        public TemplateResults BuildTextMessage(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            string xpath;

            string activityID = textActivityDetail.ActivityID.ToString();
            string patientId = textActivityDetail.PatientID.ToString();
            string fromTextNumber = textActivityDetail.TextFromNumber;
            string toTextNumber = textActivityDetail.PhoneNumber;
            string helpTextNumber = textActivityDetail.ProviderACDNumber;

            //Patient To phone number
            if (!string.IsNullOrEmpty(toTextNumber) && !string.IsNullOrEmpty(toTextNumber.Trim()))
            {
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeToPhoneNumber, Mode);
                XmlNode toPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                _templateUtilities.SetXMlNodeInnerText(toPhoneNumberNode, toTextNumber);
            }
            else
            {
                string missingObjString = "Patient Phone number is missing for patient ID " + patientId;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }

            //Text From number
            if (!string.IsNullOrEmpty(fromTextNumber) && !string.IsNullOrEmpty(fromTextNumber.Trim()))
            {
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeFromPhoneNumber, Mode);
                XmlNode fromPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                _templateUtilities.SetXMlNodeInnerText(fromPhoneNumberNode, fromTextNumber);
            }
            else
            {
                string missingObjString = "From Phone number(Short Code) is missing for Activity ID " + activityID;
                missingObjects = _templateUtilities.AddMissingObjects(missingObjects, missingObjString);
            }

            //Help number
            if (!string.IsNullOrEmpty(helpTextNumber) && !string.IsNullOrEmpty(helpTextNumber.Trim()))
            {
                xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeHelpPhoneNumber, Mode);
                XmlNode helpPhoneNumberNode = xdoc.SelectSingleNode(xpath);
                _templateUtilities.SetXMlNodeInnerText(helpPhoneNumberNode, helpTextNumber);
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

        public TemplateResults BuildApptDateTime(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = _templateUtilities.BuildApptDateTime(xdoc, textActivityDetail, missingObjects, Mode, true, new string[] { "ScheduleDateTime" });
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public string Transform(XmlDocument xml, TemplateDetail templateDetail)
        {
            var body = _templateUtilities.Transform(xml, templateDetail, Mode);
            return body;
        }

        #endregion

        #region Intro Text Build Template

        public TemplateResults BuildIntroHeader(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = BuildHeader(xdoc, textActivityDetail, missingObjects);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildIntroPatient(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = BuildPatient(xdoc, textActivityDetail, missingObjects);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildIntroSchedule(XmlDocument xdoc, TextActivityDetail textActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults(); 
            
            string scheduleID = string.Empty; 
            string scheduleNameLF = string.Empty;
            string scheduleDisplayName = string.Empty;
            string xpath = string.Empty;

            scheduleID = textActivityDetail.RecipientSchedID.ToString();
            scheduleNameLF = textActivityDetail.ScheduleName;

            TemplateResults utilityResults = _templateUtilities.BuildSchedule(xdoc, textActivityDetail, missingObjects, Mode);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            //Contact Entities Schedule Name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeScheduleFullName, Mode);
            XmlNode scheduleFullname = xdoc.SelectSingleNode(xpath);
            _templateUtilities.SetXMlNodeInnerText(scheduleFullname, scheduleNameLF);

            //Schedule Display name
            xpath = _templateUtilities.GetModeSpecificTag(XMLFields.ModeScheduleDisplayName, Mode);
            XmlNode scheduleDisplayNameNode = xdoc.SelectSingleNode(xpath);
            List<ActivityMedia> mediaRows =
                activityMediaList.Where(m => m.OwnerID == textActivityDetail.RecipientSchedID
                                             && m.CategoryCode == "SCOVR"
                                             && m.OwnerCode == "TEXT"
                                             && m.LanguagePreferenceCode == "EN").ToList();
            //Check to see if there is any records
            if (mediaRows.Count > 0)
            {
                foreach (var media in mediaRows)
                {
                    scheduleDisplayName = media.Narrative;
                    if (!string.IsNullOrEmpty(scheduleDisplayName) && !string.IsNullOrEmpty(scheduleDisplayName.Trim()))
                    {
                        scheduleDisplayName = _templateUtilities.ProperCase(scheduleDisplayName.Trim());
                        _templateUtilities.SetXMlNodeInnerText(scheduleDisplayNameNode, scheduleDisplayName);
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

        public TemplateResults BuildIntroFacility(XmlDocument xdoc, TextActivityDetail textActivityDetail, List<ActivityMedia> activityMediaList, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = BuildFacility(xdoc, textActivityDetail, activityMediaList, missingObjects);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            int key = missingObjects.Count-1;
            if (key > -1)
            {
                string value = (string)missingObjects[key];

                if (value.Contains("Facility name"))
                    missingObjects.Remove(key);
            }
            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildIntroTextMessage(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = BuildTextMessage(xdoc, textActivityDetail, missingObjects);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        public TemplateResults BuildIntroApptDateTime(XmlDocument xdoc, TextActivityDetail textActivityDetail, Hashtable missingObjects)
        {
            TemplateResults results = new TemplateResults();

            TemplateResults utilityResults = _templateUtilities.BuildApptDateTime(xdoc, textActivityDetail, missingObjects, Mode, true);
            xdoc = utilityResults.PopulatedTemplate;
            missingObjects = utilityResults.MissingObjects;

            results.PopulatedTemplate = xdoc;
            results.MissingObjects = missingObjects;
            return results;
        }

        #endregion

    }
}
