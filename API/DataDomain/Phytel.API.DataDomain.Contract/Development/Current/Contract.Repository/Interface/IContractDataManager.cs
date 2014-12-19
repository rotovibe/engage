using Phytel.API.DataDomain.Contract.DTO;
using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Contract.Repository
{
    public interface IContractDataManager
    {
        List<ContractData> GetAllContracts(GetAllContractsDataRequest request);
    }
}
