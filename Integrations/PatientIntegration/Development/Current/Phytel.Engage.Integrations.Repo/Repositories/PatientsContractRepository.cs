using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.Repo.Bridge;
using Phytel.Engage.Integrations.Repo.Connections;
using Phytel.Engage.Integrations.Repo.DTOs;

namespace Phytel.Engage.Integrations.Repo.Repositories
{
    public class PatientsContractRepository : IRepository
    {
        private string _contract;
        public ISQLConnectionProvider ConnStr { get; set; }
        public IQueryImplementation Implementor { get; set; }

        public PatientsContractRepository(string contract, ISQLConnectionProvider conProvider, IQueryImplementation implementor )
        {
            _contract = contract;
            ConnStr = conProvider;
            Implementor = implementor;
        }

        public object SelectAll()
        {
            try
            {
                Dictionary<int, PatientInfo> ptInfo = null;
                using (var ct = new ContractEntities(ConnStr.GetConnectionStringEF(_contract)))
                {
                    var query = Implementor.GetPatientInfoQuery(ct);
                    ptInfo = query.ToDictionary(r => Convert.ToInt32(r.PatientId));
                }

                return ptInfo;
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("PatientsContractRepository:SelectAll(): " + ex.Message, false) );
                throw;
            }
        }



        public object Insert(object list)
        {
            return null;
        }


        public object SelectAllGeneral()
        {
            throw new NotImplementedException();
        }
    }
}
