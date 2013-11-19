using ServiceStack.ServiceHost;
using Phytel.API.Interface;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/patientproblems/{PatientID}", "GET")]
    [Route("/{Context}/{Version}/{ContractNumber}/patientproblems", "POST")]
    public class GetAllPatientProblemRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientID", Description = "ID parameter", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientID { get; set; }

        [ApiMember(Name = "Status", Description = "Status of the problem being requested", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Status { get; set; }

        [ApiMember(Name = "Category", Description = "Category of the problem being requested", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Category { get; set; }

        [ApiMember(Name = "Context", Description = "Product parameter will be defined in the route.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract parameter will be defined in the route.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Token parameter in Header", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the Request", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Version { get; set; }
    }
}
