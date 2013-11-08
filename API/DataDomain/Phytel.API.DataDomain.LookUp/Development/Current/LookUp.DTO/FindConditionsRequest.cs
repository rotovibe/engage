using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/conditions", "GET")]
    public class FindConditionsRequest : IDataDomainRequest
    {
        public string Context { get; set; }
        public string ContractNumber { get; set; }
        public string Version { get; set; }
    }
}
