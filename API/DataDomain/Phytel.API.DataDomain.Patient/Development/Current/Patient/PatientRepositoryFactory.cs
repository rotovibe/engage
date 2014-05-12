using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Patient
{
    public class PatientRepositoryFactory : IPatientRepositoryFactory 
    {
        public IPatientRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            IPatientRepository repo = null;

            switch (type)
            {
                case RepositoryType.Patient:
                    {
                        repo = new MongoPatientRepository(request.ContractNumber) as IPatientRepository;
                        break;
                    }
                case RepositoryType.CohortPatientView:
                    {
                        repo = new MongoCohortPatientViewRepository(request.ContractNumber) as IPatientRepository;
                        break;
                    }
            }

            repo.UserId = request.UserId;
            return repo;
        }

        //public static IPatientRepository<T> GetPatientRepository(string dbName, string productName, string userId)
        //{
        //    IPatientRepository<T> repo = null;
        //    repo = new MongoPatientRepository<T>(dbName) as IPatientRepository<T>;
        //    repo.UserId = userId;
        //    return repo;
        //}

        //public static IPatientRepository<T> GetCohortPatientViewRepository(string dbName, string productName, string userId)
        //{
        //    IPatientRepository<T> repo = null;
        //    repo = new MongoCohortPatientViewRepository<T>(dbName) as IPatientRepository<T>;
        //    repo.UserId = userId;
        //    return repo;
        //}
    }
}
