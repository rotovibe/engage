using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Contact/{ContactID}", "GET")]
    public class GetContactRequest : IDataDomainRequest
    {
        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "ContactID", Description = "ID of the Contact being requested", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContactID { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Contact", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}
