using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Search.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Search/{Type}/", "GET")]
    public class GetSearchRequest : IDataDomainRequest
    {
        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Type", Description = "Type to search for: Med or Allergy", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Type { get; set; }

        [ApiMember(Name = "Term", Description = "Term to search for", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Term { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Search", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }
    }
}
