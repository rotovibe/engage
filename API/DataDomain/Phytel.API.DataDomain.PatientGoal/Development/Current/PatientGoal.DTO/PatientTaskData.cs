using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class PatientTaskData
    {
        public string Id { get; set; }
        public string TargetValue { get; set; }
        public int Status { get; set; }
        public DateTime? TargetDate { get; set; }
        public List<TaskAttribute> Attributes { get; set; }
        public List<string> Barriers { get; set; }
        public string Description { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? StartDate { get; set; }

        public Dictionary<string, object> ExtraElements { get; set; }
        public string Version { get; set; }
        public string UpdatedBy { get; set; }
        public bool DeleteFlag { get; set; }
        public System.DateTime? TTLDate { get; set; }
        public System.DateTime? LastUpdatedOn { get; set; }
    }
}
