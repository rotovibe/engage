using Phytel.API.DataDomain.Contract.DTO;
using System;

namespace Phytel.API.DataDomain.Contract.Repository
{
    public interface IContractDataManager
    {
        ContractData GetContracts(GetContractsDataRequest request);
    }
}
