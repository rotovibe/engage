using System;
using System.Collections.Generic;
using System.Xml;
namespace Phytel.Services.Communication
{
    public interface ICommTextTemplateManager
    {
        TemplateResults BuildApptDateTime(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildFacility(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Generic.List<ActivityMedia> activityMediaList, System.Collections.Hashtable missingObjects);
        TemplateResults BuildHeader(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildIntroApptDateTime(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildIntroHeader(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildIntroFacility(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Generic.List<ActivityMedia> activityMediaList, System.Collections.Hashtable missingObjects);
        TemplateResults BuildIntroTextMessage(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildIntroPatient(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildIntroSchedule(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Generic.List<ActivityMedia> activityMediaList, System.Collections.Hashtable missingObjects);
        TemplateResults BuildPatient(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
        TemplateResults BuildSchedule(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Generic.List<ActivityMedia> activityMediaList, System.Collections.Hashtable missingObjects);
        TemplateResults BuildTextMessage(System.Xml.XmlDocument xdoc, TextActivityDetail textActivityDetail, System.Collections.Hashtable missingObjects);
        string Transform(XmlDocument xml, TemplateDetail templateDetail);
    }
}
