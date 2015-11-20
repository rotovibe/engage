using System;
namespace Phytel.Services.Communication
{
    interface ICommTextTemplateManager
    {
        TemplateResults BuildApptDateTime(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildFacility(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Generic.List<ActivityMedia> activityMediaList, System.Collections.Hashtable missingObjects);
        TemplateResults BuildHeader(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildIntroApptDateTime(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildIntroHeader(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildIntroTextFacility(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Generic.List<ActivityMedia> activityMediaList, System.Collections.Hashtable missingObjects);
        TemplateResults BuildIntroTextMessage(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildIntroTextPatient(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildIntroTextSchedule(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Generic.List<ActivityMedia> activityMediaList, System.Collections.Hashtable missingObjects);
        TemplateResults BuildPatient(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildSchedule(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Generic.List<ActivityMedia> activityMediaList, System.Collections.Hashtable missingObjects);
        TemplateResults BuildTextMessage(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
    }
}
