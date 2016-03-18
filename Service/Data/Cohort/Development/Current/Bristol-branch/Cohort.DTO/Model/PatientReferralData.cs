using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Cohort.DTO.Model
{
    public class PatientReferralData
    {
        public string Id { get; set; }
        public string ReferralId { get; set; }
        public string PatientId { get; set; }
        public string CreatedBy { get; set; }
    }
}
