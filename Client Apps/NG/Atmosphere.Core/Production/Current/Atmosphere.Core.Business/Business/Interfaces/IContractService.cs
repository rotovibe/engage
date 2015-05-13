using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using C3.Data;
using Phytel.Framework.SQL.Data;
using System.Configuration;


namespace C3.Business.Interfaces
{
    public interface IContractService
    {
        List<Contract> GetAllContracts();
        DataTable GetUsersToImpersonateByContract(int contractId);
        DataTable GetAllContractsDT();
        Contract GetContractById(int contractId);
        List<Contract> GetByUser(Guid UserId);
        DataTable GetByUserDT(Guid UserId);
        void InsertUserContract(string[] contractIds, Guid userId);
        void DeleteUserContract(List<Contract> contracts, Guid userId);
    }
}
