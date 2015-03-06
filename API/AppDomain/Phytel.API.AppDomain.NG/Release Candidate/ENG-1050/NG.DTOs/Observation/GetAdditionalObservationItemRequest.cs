using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Api(Description = "A Request object to initialize an additional observation for the patient specified by the observationid. This additional observation can be non/standard.")]
    [Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Observation/{ObservationId}", "GET")]
    public class GetAdditionalObservationItemRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "Id of the patient.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        //[ApiMember(Name = "TypeId", Description = "Id of the observation type.", ParameterType = "path", DataType = "string", IsRequired = true)]
        //public string TypeId { get; set; }

        [ApiMember(Name = "ObservationId", Description = "Id of the observation type to initialize.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ObservationId { get; set; }

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

        public GetAdditionalObservationItemRequest() { }
    }
}
