using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/Observation/Type/{TypeId}/MatchLibrary", "GET")]
    [Route("/{Version}/{ContractNumber}/Observation/Type/{TypeId}/MatchLibrary/{Standard}", "GET")]
    public class GetAdditionalObservationLibraryRequest : IAppDomainRequest
    {
        [ApiMember(Name = "TypeId", Description = "Id of the patient for whom a goal is being created.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string TypeId { get; set; }

        [ApiMember(Name = "Standard", Description = "Determines if observation is standard or not.", ParameterType = "path", DataType = "boolean", IsRequired = false)]
        public bool? Standard { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Program", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        public GetAdditionalObservationLibraryRequest() { }
    }
}
