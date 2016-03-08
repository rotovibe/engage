using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Cohort.DTO.Model
{
    public class PatientReferralData
    {
        public string CohortId { get; set; }
        public string PatientId { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Flag { get; set; }
        public DateTime TTLDate { get; set; }
    }
}
