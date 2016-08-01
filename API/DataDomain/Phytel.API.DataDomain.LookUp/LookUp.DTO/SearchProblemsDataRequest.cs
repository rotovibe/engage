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
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}
