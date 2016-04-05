using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Cohort.DTO.Model
{
    public class PatientReferralData
    {
        [ApiMember(Name = "PatientId", Description = "PatientId is a Mongo ObjectId. eg:528aa055d4332317acc50978", ParameterType = "body", DataType = "string")]
        public string PatientId { get; set; }
        [ApiMember(Name = "ReferralId", Description = "ReferralId is a Mongo ObjectId. eg:528aa055d4332317acc50978", ParameterType = "body", DataType = "string")]
        public string ReferralId { get; set; }
        [ApiMember(Name = "ReferralDate", Description = "ReferralDate is a Mongo Date. eg:Date(62135596800000-0000)", ParameterType = "body", DataType = "string")]
        public System.DateTime ReferralDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
