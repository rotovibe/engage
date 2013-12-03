using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/problems", "GET")]
    public class GetAllProblemsDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Context", Description = "Context", ParameterType = "body", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the Data Domain API being called", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Version { get; set; }
    }
}
