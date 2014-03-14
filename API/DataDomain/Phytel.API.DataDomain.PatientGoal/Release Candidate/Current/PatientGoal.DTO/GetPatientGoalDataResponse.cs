using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class GetPatientGoalDataResponse : IDomainResponse
    {
        public PatientGoalData GoalData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
