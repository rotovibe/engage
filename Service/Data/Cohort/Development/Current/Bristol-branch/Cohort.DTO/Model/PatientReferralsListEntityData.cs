using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Cohort.DTO.Model
{
   public class PatientReferralsListEntityData : PatientReferralData
    {
        public string DataSource { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }

    }       // end class definition
}           // end namespace definition
