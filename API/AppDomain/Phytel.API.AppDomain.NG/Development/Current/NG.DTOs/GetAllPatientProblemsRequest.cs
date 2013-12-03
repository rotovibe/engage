using ServiceStack;
using Phytel.API.Interface;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/patientproblems/{PatientID}", "GET")]
    public class GetAllPatientProblemsRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientID", Description = "Patient ID", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string PatientID { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract parameter will be defined in the route.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Token parameter in Header", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the Request", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Version { get; set; }
    }
}
