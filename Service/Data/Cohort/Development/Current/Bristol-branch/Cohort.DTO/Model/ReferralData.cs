using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phytel.API.DataDomain.Cohort.DTO.Model
{
    /// <summary>
    /// POCO for referral data domain. This object will be the model used form transfer to appdomains.
    /// </summary>
    public class ReferralData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}