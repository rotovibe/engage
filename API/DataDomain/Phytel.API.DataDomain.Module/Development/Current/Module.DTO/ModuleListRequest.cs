using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Module.DTO
{
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/Modulelist", "POST")]
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/Modulelist/Module/{ModuleNounID}", "GET")]
    public class ModuleListRequest : IDataDomainRequest
    {
        /*
         * Put custom fields here
        */

        [ApiMember(Name = "Context", Description = "Product Context requesting the Module", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}
