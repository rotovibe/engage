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
        protected static readonly int Limit = Convert.ToInt32(ConfigurationManager.AppSettings["RecentLimit"]);

        public IContractRepositoryFactory Factory { get; set; }

        public ContractData GetContracts(GetContractsDataRequest request)
        {
            ContractData result = null;
            try
            {
                IContractRepository repo = Factory.GetRepository(request, RepositoryType.Contract);

                //result = repo.FindContracts(request) as ContractData;

                //Hardcoding results
                result = new ContractData();
                result.ContractId = "452";
                result.ContractNumber = "ATMOSPHEREDEMO001";

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

    }
}   
