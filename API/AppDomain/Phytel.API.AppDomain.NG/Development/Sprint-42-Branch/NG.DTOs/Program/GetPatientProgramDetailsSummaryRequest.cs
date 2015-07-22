using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Program/{PatientProgramId}/", "GET")]
    public class GetPatientProgramDetailsSummaryRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientProgramId", Description = "programId of program to get details.", ParameterType = "Path", DataType = "string", IsRequired = true)]
        public string PatientProgramId { get; set; }

        [ApiMember(Name = "PatientId", Description = "PatientId", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Program", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }
    }
}
