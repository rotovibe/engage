using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class PatientInterventionData
    {
        public string Id { get; set; }
        public string PatientGoalId { get; set; }
        public int CategoryId { get; set; }
        public string AssignedTo { get; set; }
        public int Order { get; set; }
        public List<string> Barriers { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? StartDate { get; set; }
        public List<AttributeData> Attributes { get; set; }

        public System.DateTime? TTLDate { get; set; }
    }
}
