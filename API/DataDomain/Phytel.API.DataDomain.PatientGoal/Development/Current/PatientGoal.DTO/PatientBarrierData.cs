using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class PatientBarrierData
    {
        public string Id { get; set; }
        public string PatientGoalId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime? StatusDate { get; set; }
    }
}
