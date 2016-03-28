using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO.ContactTypeLookUp
{
    [Api(Description = "A Request object to save Contact Type Lookup.")]
    [Route("/{Context}/{Version}/{ContractNumber}/ContactTypeLookUps", "PUT")]
    public class PutContactTypeLookUpDataRequest: IDataDomainRequest
    {
        [ApiMember(Name = "ContactTypeLookUpData", Description = "Data to add", ParameterType = "body", DataType = "ContactLookUpData", IsRequired = true)]
        public ContactTypeLookUpData ContactTypeLookUpData { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Contact", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "query", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}
