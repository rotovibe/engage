using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO.Goal
{
    public class Goal : IGoal
    {
        public string Id { get; set; }
        public List<string> FocusAreaIds { get; set; }
        public string Name { get; set; }
        public string SourceId { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int StartDateRange { get; set; }
        public int TargetDateRange { get; set; }
        public string TargetValue { get; set; }
        public DateTime? TargetDate { get; set; }
        public List<CustomAttribute> CustomAttributes { get; set; }

        //public List<PatientBarrierData> BarriersData { get; set; }
        //public List<PatientTaskData> TasksData { get; set; }
        //public List<PatientInterventionData> InterventionsData { get; set; }
    }
}
