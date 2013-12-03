using Phytel.API.Interface;
using ServiceStack;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/patientproblems/{PatientID}", "GET")]
    public class GetAllPatientProblemsDataRequest: IDataDomainRequest
    {
        public string PatientID { get; set; }
        public string Context { get; set; }
        public string ContractNumber { get; set; }
        public string Version { get; set; }
    }
}
