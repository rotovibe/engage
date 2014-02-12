using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class GetPatientGoalDataResponse : IDomainResponse
    {
        public PatientGoalData PatientGoal { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class PatientGoalData
    {
        public string Id { get; set; }
        public List<string> FocusAreas { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public List<string> Programs { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TargetValue { get; set; }
        public DateTime? TargetDate { get; set; }
    }
}
