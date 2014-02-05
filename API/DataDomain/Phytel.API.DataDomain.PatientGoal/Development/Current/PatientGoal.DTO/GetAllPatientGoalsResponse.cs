using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class GetAllPatientGoalsResponse : IDomainResponse
   {
        public List<PatientGoal> PatientGoals { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
