using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contract.DTO
{
    [Api(Description = "A Request object to get all contract details for an individual from the API.")]
    [Route("/{Context}/{Version}/{ContractNumber}/Contract", "GET")]
    public class GetContractsDataRequest : IDataDomainRequest
    {

        [ApiMember(Name = "Context", Description = "Product Context requesting the Contract", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; } 

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}
