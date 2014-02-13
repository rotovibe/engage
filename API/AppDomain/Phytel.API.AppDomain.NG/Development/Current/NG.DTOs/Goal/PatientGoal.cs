using System;
using System.Collections.Generic;
using Phytel.API.Common.CustomObjects;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PatientGoal
    {
        public string Id { get; set; }
        public List<string> FocusAreas { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public List<IdNamePair> Programs { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
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
