using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    public class PatientProblem
    {
        public string PatientID { get; set; }
        public string ProblemID { get; set; }
        public bool Active { get; set; }
        public bool Featured { get; set; }
        public int Level { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Version { get; set; }
        public string UpdatedBy { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime? TTLDate { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

    }
}
