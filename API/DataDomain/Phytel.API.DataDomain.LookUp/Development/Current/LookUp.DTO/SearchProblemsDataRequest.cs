using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/problems", "POST")]
    public class SearchProblemsDataRequest : IDataDomainRequest
    {
        public bool Active { get; set; }
        public string Type { get; set; }
        public string Context { get; set; }
        public string ContractNumber { get; set; }
        public string Version { get; set; }
    }
}
