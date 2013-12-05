using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Action.DTO
{
    [Api(Description = "A Request object to get all Actions from the API.")]
    [Route("/{Context}/{Version}/{ContractNumber}/Actions", "GET")]
    public class GetAllActionsDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Context", Description = "Product Context requesting the Action", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Version { get; set; }
    }
}
