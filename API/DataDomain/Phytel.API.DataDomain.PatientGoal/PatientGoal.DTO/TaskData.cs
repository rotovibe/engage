using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class TaskData
    {
        public string Id { get; set; }
        public string CreatedById { get; set; }
        public string TargetValue { get; set; }
        public string TemplateGoalId { get; set; }
        public string GoalName { get; set; }
        public int StatusId { get; set; }
        public DateTime? TargetDate { get; set; }
        public List<string> BarrierIds { get; set; }
        public string Description { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? StartDate { get; set; }
        public int StartDateRange { get; set; }
        public int TargetDateRange { get; set; }
        public List<CustomAttributeData> CustomAttributes { get; set; }
        public bool DeleteFlag { get; set; }
    }
}
