using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Cohort.DTO.Model
{
    public class PatientReferralData
    {
        public string PatientId { get; set; }
        public string ReferralId { get; set; }
        public System.DateTime ReferralDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
