using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO.Utilization
{
    [Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Notes/Utilizations/", "GET")]
    public class GetPatientUtilizationsRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "Id of the patient", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "Count", Description = "Count of Utilizations to be returned.", ParameterType = "QueryString", DataType = "int", IsRequired = true)]
        public int Count { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientNote", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        //[ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "Header", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        public GetPatientUtilizationsRequest() { }
    }
}