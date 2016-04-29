using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDomain.Engage.Population.DTO.Referrals
{
    public class ReferralDefinitionData
    {

        public string ReferralName { get; set; }
        public string ExternalId { get; set; }
        //commenting this out as we dont find this saved in db
        //public string ReferralDefinition { get; set; }
        public string ReferralReason { get; set; }
        public string DataSource { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }

    }
}