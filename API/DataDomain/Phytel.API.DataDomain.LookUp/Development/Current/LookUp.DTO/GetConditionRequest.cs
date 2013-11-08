using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/condition/{ConditionID}", "GET")]
    public class GetConditionRequest : IDataDomainRequest, IReturn<ConditionResponse>
    {
        public string ConditionID { get; set; }
        public string Context { get; set; }
        public string ContractNumber { get; set; }
        public string Version { get; set; }
    }


}
