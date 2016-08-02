using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.Repo.Bridge;
using Phytel.Engage.Integrations.Repo.Connections;
using Phytel.Engage.Integrations.Repo.DTOs;


namespace Phytel.Engage.Integrations.Repo.Repositories
{
    public class PCPPhoneRepository : IRepository
    {
        private string _contract;
        public ISQLConnectionProvider ConnStr { get; set; }
        public IQueryImplementation Implementor { get; set; }

        public PCPPhoneRepository(string contract, ISQLConnectionProvider conProvider, IQueryImplementation implementor)
        {
            _contract = contract;
            ConnStr = conProvider;
            Implementor = implementor;
        }

        public object SelectAll()
        {
            try
            {
                List<PCPPhone> ptInfo = null;
                using (var ct = new ContractEntities(ConnStr.GetConnectionStringEF(_contract)))
                {
                    var query = Implementor.GetPCPPhoneQuery(ct);
                    ptInfo = query.ToList();
                }

                return ptInfo;
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("PCPPhoneRepository:SelectAll(): " + ex.Message, false));
                throw;
            }
        }

        public object SelectAllGeneral()
        {
            try
            {
                List<PCPPhone> ptInfo = null;
                using (var ct = new ContractEntities(ConnStr.GetConnectionStringEF(_contract)))
                {
                    var query = Implementor.GetPCPPhoneQueryGeneral(ct);
                    ptInfo = query.ToList();
                }

                return ptInfo;
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("PCPPhoneRepository:SelectAll(): " + ex.Message, false));
                throw;
            }
        }


        public object Insert(object list)
        {
            return null;
        }
    }
}
