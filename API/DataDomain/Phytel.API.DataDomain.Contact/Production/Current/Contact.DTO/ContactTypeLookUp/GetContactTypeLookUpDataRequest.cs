using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO
{
    [Api(Description = "A Request object to get Contact lookUp Types")]
    [Route("/{Context}/{Version}/{ContractNumber}/ContactTypeLookUps", "GET")]
    public class GetContactTypeLookUpDataRequest : IDataDomainRequest
    {

        [ApiMember(Name = "ContactLookUpGroupType", Description = "Group Type to filter lookups", ParameterType = "query", DataType = "ContactLookUpGroupType", IsRequired = true)]
        public ContactLookUpGroupType GroupType { get; set; }

        [ApiMember(Name = "FlattenData", Description = "flatten", ParameterType = "query", DataType = "bool", IsRequired = false)]
        public bool FlattenData { get; set; }

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
