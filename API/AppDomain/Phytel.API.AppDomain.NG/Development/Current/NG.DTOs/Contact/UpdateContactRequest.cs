using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Api(Description = "A Request object to update a contact.")]
    [Route("/{Version}/{ContractNumber}/Contacts/{Id}", "PUT")]
    public class UpdateContactRequest : IAppDomainRequest
    {

        [ApiMember(Name = "Id", Description = "Id of contact that is being updated.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Id { get; set; }

        [ApiMember(Name = "Contact", Description = "Contact object to be updated", ParameterType = "property", DataType = "Contact", IsRequired = true)]
        public Contact Contact { get; set; }

        [ApiMember(Name = "UserId", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        public UpdateContactRequest() { }
    }
}
