using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Program.DTO
{
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/Program", "POST")]
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/Program/{ProgramID}", "GET")]
    public class ProgramRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ProgramID", Description = "ID of the Program being requested", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ProgramID { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Program", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}
