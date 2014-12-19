using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contract.DTO
{
    public class GetAllContractsDataResponse : IDomainResponse
    {
        public List<ContractData> Contracts { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    } 
}
