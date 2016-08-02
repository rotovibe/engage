using System;
using System.Collections.Generic;
using System.Data;
using C3.Data;
using Phytel.Framework.SQL.Data;
using C3.Business.Interfaces;

namespace C3.Business
{
    public class ContractService : ServiceBase, IContractService
    {
        private static volatile ContractService _instance;
        private static object _syncRoot = new Object();

        private string _dbConnName = "Phytel";

        public ContractService()
        {
            _dbConnName = System.Configuration.ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
        }

        public static ContractService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if ( _instance == null)
                            _instance = new ContractService();
                    }
                }
                return _instance;
            }
        }

        public List<Contract> GetAllContracts()
        {
            return CachedQueryAll<Contract>(null, _dbConnName, StoredProcedure.GetAllContracts, Contract.Build, new CacheAccessor("C3Cache", "Contracts"));
        }

        public DataTable GetUsersToImpersonateByContract(int contractId)
        {
            return Query(null, _dbConnName, StoredProcedure.GetUsersByContractId, contractId);
        }

        public DataTable GetAllContractsDT()
        {
            return Query(null, _dbConnName, StoredProcedure.GetAllContracts);
        }
    
        public Contract GetContractById(int contractId)
        {
            return Query<Contract>(null, _dbConnName, StoredProcedure.GetContractById, Contract.Build, new object[] { contractId });            
        }

        public List<Contract> GetByUser(Guid UserId)
        {
            List<Contract> contractList = QueryAll<Contract>(null, _dbConnName, StoredProcedure.GetUserContracts, Contract.Build, UserId);

            if (contractList.Count == 0)
            {
                Contract contract = new Contract();

                contract.ContractId = 0;
                contract.Name = "No Contract";
                contract.DefaultContract = false;
                contractList.Add(contract);
            }
            return contractList;
        }

        public DataTable GetByUserDT(Guid UserId)
        {
            return Query(null, _dbConnName, StoredProcedure.GetUserContracts, UserId);
        }


        public void InsertUserContract(string[] contractIds, Guid userId)
        {
            foreach (string contractId in contractIds)
            {
                new SqlDataExecutor().Execute(_dbConnName, StoredProcedure.InsertUserContract, new object[] { contractId, userId });
            }
        }

        public void DeleteUserContract(List<Contract> contracts, Guid userId)
        {
            foreach (Contract contract in contracts)
            {
                new SqlDataExecutor().Execute(_dbConnName, StoredProcedure.DeleteUserFromContract, new object[] { contract.ContractId, userId });
            }
        }

		public Dictionary<string, string> GetProperties(Contract contract)
		{
			Dictionary<string, string> properties = new Dictionary<string, string>();
			DataTable queryResults = Query(contract.ConnectionString, contract.Database, StoredProcedure.GetContractProperty);
			foreach (DataRow current in queryResults.Rows)
			{
				properties.Add(current["Key"].ToString(), current["Value"].ToString());
			}

			return properties;
		}

    }
}
