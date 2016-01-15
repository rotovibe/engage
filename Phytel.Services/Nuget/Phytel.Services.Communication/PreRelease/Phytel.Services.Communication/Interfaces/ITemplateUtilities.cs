using System;
using System.Collections.Generic;
using System.Xml;
namespace Phytel.Services.Communication
{
    public interface ITemplateUtilities
    {
        System.Collections.Hashtable AddMissingObjects(System.Collections.Hashtable missingObjects, string missingObjString);
        TemplateResults BuildApptDateTime(System.Xml.XmlDocument xdoc, ActivityDetail activityDetail, System.Collections.Hashtable missingObjects, string mode, bool shortMonth, string[] requiredObjects = null);
        TemplateResults BuildHeader(System.Xml.XmlDocument xdoc, ActivityDetail activityDetail, System.Collections.Hashtable missingObjects, string mode, string[] requiredObjects = null);
        TemplateResults BuildPatient(System.Xml.XmlDocument xdoc, ActivityDetail activityDetail, System.Collections.Hashtable missingObjects, string mode, string[] requiredObjects = null);
        TemplateResults BuildSchedule(System.Xml.XmlDocument xdoc, ActivityDetail activityDetail, System.Collections.Hashtable missingObjects, string mode, string[] requiredObjects = null);
        string GetModeSpecificTag(string originalTag, string mode);
        string HandleXMlSpecialCharacters(string innerText);
        string ProperCase(string input);
        void SetCDATAXMlNodeInnerText(System.Xml.XmlNode node, string innerText, System.Xml.XmlDocument xmlDoc);
        void SetXMlNodeInnerText(System.Xml.XmlNode originalNode, string innerText);
        bool IsEmailAddressFormatValid(string emailAddress);
        string[] GetCommRequestStatuses();
        string Transform(XmlDocument xml, TemplateDetail templateDetail, string mode);
        CommTextResult BuildCommTextResult(XmlNode call);
    }
}
