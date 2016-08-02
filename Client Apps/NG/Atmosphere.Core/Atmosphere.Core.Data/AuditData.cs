using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;

using C3.Data;

namespace C3.Data
{
    [Serializable]
    public class AuditData
    {
        public AuditData() { }


        #region Required Properties
        public string Type { get; set; }
        public Guid UserId { get; set; }
        public Guid ImpersonatingUserId { get; set; }
        public DateTime EventDateTime { get; set; }
        public string SourcePage { get; set; }
        public string SourceIP { get; set; }
        public string Browser { get; set; }
        public string SessionId { get; set; }
        public int ContractID { get; set; }
        #endregion

        #region Optional Properties
        // patient list
        [XmlArray("PatientIDList")]
        [XmlArrayItem("PatientID")]
        public List<int> Patients { get; set; }
        public Guid EditedUserId { get; set; }
        public string EnteredUserName { get; set; }
        public string SearchText { get; set; }
        // additional audit information
        public object AdditionalAuditData { get; set; }
        public string LandingPage { get; set; }
        public Message Message { get; set; }
        public string TOSVersion { get; set; }
        public string NotificationTotal { get; set; }
        public string DownloadedReport { get; set; }
        #endregion
    }

    [Serializable]
    public class Message
    {
        public Message()
        {
        }

        public string Id { get; set; }
        public string Text { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
    }
}
