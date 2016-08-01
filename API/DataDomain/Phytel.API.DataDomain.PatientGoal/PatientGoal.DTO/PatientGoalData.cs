using System;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class PatientGoalData
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public List<string> FocusAreaIds { get; set; }
        public string Name { get; set; }
        public string SourceId { get; set; }
        public List<string> ProgramIds { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public string TemplateId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TargetValue { get; set; }
        public DateTime? TargetDate { get; set; }
        public List<CustomAttributeData> CustomAttributes { get; set; }

        public List<PatientBarrierData> BarriersData { get; set; }
        public List<PatientTaskData> TasksData { get; set; }
        public List<PatientInterventionData> InterventionsData { get; set; }
        public string Details { get; set; }
    }
}
