using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.DataDomain.Contract.DTO;
using Phytel.API.Interface;
using MongoDB.Bson;
using System.Configuration;

namespace Phytel.API.DataDomain.Contract.Repository
{
    public class ContractDataManager : IContractDataManager
    {
        public IContractRepositoryFactory Factory { get; set; }

        public List<ContractData> GetAllContracts(GetAllContractsDataRequest request)
        {
            List<ContractData> result = null;
            try
            {
                IContractRepository repo = Factory.GetRepository(request, RepositoryType.Contract);
                result = repo.GetAllContracts(request) as List<ContractData>;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

    }
}   
