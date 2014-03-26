using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    public class GetPatientProblemsDataResponse : IDomainResponse
    {
        public PatientProblemData PatientProblem { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
   
}
