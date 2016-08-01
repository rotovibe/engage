using System;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class GoalData
    {
        public string Id { get; set; }
        public List<string> FocusAreaIds { get; set; }
        public string Name { get; set; }
        public string SourceId { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public DateTime? StartDate { get; set; }
        public int StartDateRange { get; set; }
        public int TargetDateRange { get; set; }
        public DateTime? EndDate { get; set; }
        public string TargetValue { get; set; }
        public DateTime? TargetDate { get; set; }
        public List<CustomAttributeData> CustomAttributes { get; set; }

        //public List<PatientBarrierData> BarriersData { get; set; }
        //public List<PatientTaskData> TasksData { get; set; }
        //public List<PatientInterventionData> InterventionsData { get; set; }
    }
}
