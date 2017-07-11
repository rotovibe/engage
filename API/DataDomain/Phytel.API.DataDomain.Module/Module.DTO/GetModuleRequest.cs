using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Module.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Module/{ModuleID}", "GET")]
    public class GetModuleRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ModuleID", Description = "ID of the Module being requested", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ModuleID { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Module", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}