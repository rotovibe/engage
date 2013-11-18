using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/problem/{ProblemID}", "GET")]
    public class GetProblemRequest : IDataDomainRequest
    {
        public string ProblemID { get; set; }
        public string Context { get; set; }
        public string ContractNumber { get; set; }
        public string Version { get; set; }
    }


}
