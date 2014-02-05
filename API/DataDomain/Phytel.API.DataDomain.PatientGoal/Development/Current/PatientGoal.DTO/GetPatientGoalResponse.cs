using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class GetPatientGoalResponse : IDomainResponse
    {
        public PatientGoal PatientGoal { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class PatientGoal
    {
        public string PatientGoalID { get; set; }
        public string Version { get; set; }
    }
}
