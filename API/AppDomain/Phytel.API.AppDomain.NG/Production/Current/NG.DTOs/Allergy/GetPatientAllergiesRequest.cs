using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/PatientAllergy/{PatientId}", "POST")]
    public class GetPatientAllergiesRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "Id of the patient for whom the allergies need to be retrieved.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "StatusIds", Description = "List of patient allergy Status ids.", ParameterType = "property", DataType = "List<int>", IsRequired = false)]
        public List<int> StatusIds { get; set; }

        [ApiMember(Name = "TypeIds", Description = "List of allergy Type ids.", ParameterType = "property", DataType = "List<string>", IsRequired = false)]
        public List<string> TypeIds { get; set; }
        
        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }
    }
}
