using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Problem/", "GET")]
    public class GetPatientProblemsDataRequest : IDataDomainRequest
    {
        public string PatientId { get; set; }
        public string ProblemId { get; set; }
        public string Context { get; set; }
        public string ContractNumber { get; set; }
        public string Version { get; set; }
        public string UserId { get; set; }
    }
}
