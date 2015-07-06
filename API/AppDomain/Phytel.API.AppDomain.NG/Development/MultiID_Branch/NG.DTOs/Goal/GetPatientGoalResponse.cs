using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetPatientGoalResponse : IDomainResponse
    {
        public PatientGoal Goal { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
