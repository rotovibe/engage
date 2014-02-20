using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    public class PutNewPatientProblemResponse : IDomainResponse
    {
        public PatientProblem PatientProblem { get; set; }
        public string Version { get; set; }
        public Outcome Outcome { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
