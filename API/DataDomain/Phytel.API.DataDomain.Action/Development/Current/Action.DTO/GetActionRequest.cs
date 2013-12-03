using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Action.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Action/{ActionID}", "GET")]
    public class GetActionRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ActionID", Description = "ID of the Action being requested", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ActionID { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Action", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}
