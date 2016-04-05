using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Cohort.DTO.Model
{
    /// <summary>
    /// POCO for referral data domain. This object will be the model used form transfer to appdomains.
    /// </summary>
    public class ReferralData 
    {
        [ApiMember(Name = "CohortId", Description = "CohortId is a Mongo ObjectId. eg:528aa055d4332317acc50978", ParameterType = "body", DataType = "string")]
        public string CohortId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
        public string DataSource { get; set; }
        public string CreatedBy { get; set; }
    }
}