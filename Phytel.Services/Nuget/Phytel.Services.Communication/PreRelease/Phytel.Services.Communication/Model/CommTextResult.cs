using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Phytel.Services.Communication
{
    public class CommTextResult
    {
        public int SendID { get; set; }
        public string ContractNumber { get; set; }
        public int FacilityID { get; set; }
        public int ActivityID { get; set; }
        public int ScheduleID { get; set; }
        public string CallID { get; set; }
        public string FacilityName { get; set; }
        public string FromNumber { get; set; }
        public string ToNumber { get; set; }
        public DateTime CallDistributorTimeOfCall { get; set; }
        public long CallDuration { get; set; }
        public string ResultCode { get; set; }
        public string ResultStatus { get; set; }
        public string ActivityStatus { get; set; }
        public int NotifySender { get; set; }
        public int TaskComplete { get; set; }
        public int Inprocess { get; set; }
        public int Retries { get; set; }
        public int RetryCount { get; set; }
        public int RetryInterval { get; set; }
        public int ActivityRetryCount { get; set; }
        public int UnsuccessfulCount { get; set; }
        public int UnsuccessfulLimit { get; set; }
        public int UnsuccessfulRetryInterval { get; set; }
        public string TaskTypeCategory { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ActivityStatusDate { get; set; }
        public bool RetryFlag { get; set; }
        public string CommInputXML { get; set; }
        public string CommApplicationURL { get; set; }
        public int ContractID { get; set; }
        public DateTime CommStartDateTime { get; set; }
        public DateTime CommStopDateTime { get; set; }
        public String ErrorCode { get; set; }
    }
}
