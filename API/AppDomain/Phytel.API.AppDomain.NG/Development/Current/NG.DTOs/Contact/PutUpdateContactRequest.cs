using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Api(Description = "A Request object to update the contact card details.")]
    [Route("/{Version}/{ContractNumber}/Contact", "POST")]
    [Route("/{Version}/{ContractNumber}/Patient/Contact", "POST")]
    public class PutUpdateContactRequest : IAppDomainRequest
    {

        [ApiMember(Name = "Contact", Description = "Contact being updated", ParameterType = "property", DataType = "Contact", IsRequired = true)]
        public Contact Contact { get; set; }
        
        [ApiMember(Name = "UserId", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Version { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        public PutUpdateContactRequest() { }
    }
}

     