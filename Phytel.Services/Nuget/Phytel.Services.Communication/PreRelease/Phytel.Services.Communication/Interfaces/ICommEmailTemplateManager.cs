﻿using System;
using System.Collections.Generic;
using System.Xml;
namespace Phytel.Services.Communication
{
    public interface ICommEmailTemplateManager
    {
        TemplateResults BuildAppointmentSpecificMessage(System.Xml.XmlDocument xdoc, EmailActivityDetail emailActivityDetail, System.Collections.Generic.List<ActivityMedia> activityMediaList, System.Collections.Hashtable missingObjects, int contactRoleId, System.Collections.Generic.List<ContractPermission> contractPermissionRecords);
        TemplateResults BuildApptDateTime(System.Xml.XmlDocument xdoc, EmailActivityDetail emailActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildEmailMessage(System.Xml.XmlDocument xdoc, EmailActivityDetail emailActivityDetail, System.Collections.Generic.List<ActivityMedia> activityMediaList, System.Collections.Hashtable missingObjects, int campaignId);
        TemplateResults BuildFacility(System.Xml.XmlDocument xdoc, EmailActivityDetail emailActivityDetail, System.Collections.Generic.List<ActivityMedia> activityMediaList, System.Collections.Hashtable missingObjects, string taskTypeCategory);
        TemplateResults BuildHeader(System.Xml.XmlDocument xdoc, EmailActivityDetail emailActivityDetail, System.Collections.Hashtable missingObjects, string confirmURL, string optoutURL);
        TemplateResults BuildIntroEmailMessage(System.Xml.XmlDocument xdoc, EmailActivityDetail emailActivityDetail, System.Collections.Generic.List<ActivityMedia> activityMediaList, System.Collections.Hashtable missingObjects, int campaignID);
        TemplateResults BuildIntroFacility(System.Xml.XmlDocument xdoc, EmailActivityDetail emailActivityDetail, System.Collections.Generic.List<ActivityMedia> activityMediaList, System.Collections.Hashtable missingObjects);
        TemplateResults BuildIntroPatient(System.Xml.XmlDocument xdoc, EmailActivityDetail emailActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildPatient(System.Xml.XmlDocument xdoc, EmailActivityDetail emailActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildSchedule(System.Xml.XmlDocument xdoc, EmailActivityDetail emailActivityDetail, System.Collections.Generic.List<ActivityMedia> activityMediaList, System.Collections.Hashtable missingObjects);
        string BuildURL(string url, System.Collections.Generic.Dictionary<string, string> querystrings);
        bool IsAppointmentSpecificMsgEnabled(System.Collections.Generic.List<ContractPermission> contractPermissions, int contactRoleId);
        string Transform(XmlDocument xml, TemplateDetail templateDetails);
    }
}
