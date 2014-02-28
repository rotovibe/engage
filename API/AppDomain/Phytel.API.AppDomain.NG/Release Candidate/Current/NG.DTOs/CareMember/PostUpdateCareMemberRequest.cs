using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Api(Description = "A Request object to update a CareMember.")]
    [Route("/{Version}/{ContractNumber}/Patient/{PatientId}/CareMember/Update", "POST")]
    public class PostUpdateCareMemberRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "Id of the patient", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "CareMember", Description = "CareMember being updated", ParameterType = "body", DataType = "CareMember", IsRequired = true)]
        public CareMember CareMember { get; set; }
        
        [ApiMember(Name = "UserId", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Version { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        public PostUpdateCareMemberRequest() { }
    }
}

     