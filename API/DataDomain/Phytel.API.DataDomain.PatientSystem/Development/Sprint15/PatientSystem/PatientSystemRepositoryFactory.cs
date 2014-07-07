using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.DataDomain.PatientSystem
{
    public class PatientSystemRepositoryFactory : IPatientSystemRepositoryFactory
    {
        public IPatientSystemRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            IPatientSystemRepository repo = null;

            switch (type)
            {
                case RepositoryType.PatientSystem:
                    {
                        repo = new MongoPatientSystemRepository(request.ContractNumber) as IPatientSystemRepository;
                        break;
                    }
            }

            repo.UserId = request.UserId;
            return repo;
        }

        //public static IPatientSystemRepository<T> GetPatientSystemRepository(string dbName, string productName, string userId)
        //{
        //    IPatientSystemRepository<T> repo = null;

        //    //We only have 1 repository at this time, just return it
        //    repo = new MongoPatientSystemRepository<T>(dbName) as IPatientSystemRepository<T>;
        //    repo.UserId = userId;
        //    return repo;
        //}
    }
}
