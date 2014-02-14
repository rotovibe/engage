using System;
using System.Collections.Generic;
using Phytel.API.Common.CustomObjects;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PatientGoal
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public List<string> FocusAreaIds { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public List<string> Programs { get; set; }
        public string Type { get; set; }
        public int Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TargetValue { get; set; }
        public DateTime? TargetDate { get; set; }
        public List<Attribute> Attributes { get; set; }

        public List<PatientBarrier> Barriers { get; set; }
        public List<PatientTask>  Tasks{ get; set; }
        public List<PatientIntervention> Interventions { get; set; }
    }
}
