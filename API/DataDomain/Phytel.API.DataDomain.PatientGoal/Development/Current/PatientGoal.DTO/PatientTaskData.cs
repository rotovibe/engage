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
        public int Order { get; set; }
        public DateTime? TargetDate { get; set; }
        public List<AttributeData> Attributes { get; set; }
        public List<string> Barriers { get; set; }
        public string Description { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? StartDate { get; set; }

        public System.DateTime? TTLDate { get; set; }
    }
}
