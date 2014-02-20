using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientID}/Problems", "GET")]
    public class GetAllPatientProblemsDataRequest: IDataDomainRequest
    {
        public string PatientID { get; set; }
        public string Context { get; set; }
        public string ContractNumber { get; set; }
        public string Version { get; set; }
        public string UserId { get; set; }
    }
}
