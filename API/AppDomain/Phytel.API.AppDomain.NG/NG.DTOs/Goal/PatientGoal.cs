using System;
using System.Collections.Generic;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PatientGoal
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public List<string> FocusAreaIds { get; set; }
        public string Name { get; set; }
        public string SourceId { get; set; }
        public string TemplateId { get; set; }
        public List<string> ProgramIds { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TargetValue { get; set; }
        public DateTime? TargetDate { get; set; }
        public List<CustomAttribute> CustomAttributes { get; set; }

        public List<PatientBarrier> Barriers { get; set; }
        public List<PatientTask>  Tasks{ get; set; }
        public List<PatientIntervention> Interventions { get; set; }
        public string Details { get; set; }
    }
}
