using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PatientTask
    {
        public string Id { get; set; }
        public string PatientGoalId { get; set; }
        public string GoalName { get; set; }
        public string CreatedById { get; set; }
        public string TemplateId { get; set; }
        public string TargetValue { get; set; }
        public int StatusId { get; set; }
        public DateTime? TargetDate { get; set; }
        public List<CustomAttribute> CustomAttributes { get; set; }
        public List<string> BarrierIds { get; set; }
        public string Description { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public bool DeleteFlag { get; set; }
        public string Details { get; set; }
    }
}
