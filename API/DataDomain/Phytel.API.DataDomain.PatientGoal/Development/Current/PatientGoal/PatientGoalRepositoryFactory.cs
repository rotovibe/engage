using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal
{
    public class PatientGoalRepositoryFactory : IPatientGoalRepositoryFactory
    {
        public IPatientGoalRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            try
            {
                IPatientGoalRepository repo = null;

                switch (type)
                {
                    case RepositoryType.PatientGoal:
                    {
                        repo = new MongoPatientGoalRepository(request.ContractNumber);
                        break;
                    }
                    case RepositoryType.PatientBarrier:
                    {
                        repo = new MongoPatientBarrierRepository(request.ContractNumber);
                        break;
                    }
                    case RepositoryType.PatientTask:
                    {
                        repo = new MongoPatientTaskRepository(request.ContractNumber);
                        break;
                    }
                    case RepositoryType.PatientIntervention:
                    {
                        repo = new MongoPatientInterventionRepository(request.ContractNumber);
                        break;
                    }
                }

                repo.UserId = request.UserId;
                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        //public static IPatientGoalRepository<T> GetPatientGoalRepository(string dbName, string productName, string userId)
        //{
        //    try
        //    {
        //        IPatientGoalRepository<T> repo = null;

        //        //We only have 1 repository at this time, just return it
        //        repo = new MongoPatientGoalRepository<T>(dbName) as IPatientGoalRepository<T>;
        //        repo.UserId = userId;
        //        return repo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static IPatientGoalRepository<T> GetPatientBarrierRepository(string dbName, string productName, string userId)
        //{
        //    try
        //    {
        //        IPatientGoalRepository<T> repo = null;

        //        //We only have 1 repository at this time, just return it
        //        repo = new MongoPatientBarrierRepository<T>(dbName) as IPatientGoalRepository<T>;
        //        repo.UserId = userId;
        //        return repo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static IPatientGoalRepository<T> GetPatientTaskRepository(string dbName, string productName, string userId)
        //{
        //    try
        //    {
        //        IPatientGoalRepository<T> repo = null;

        //        //We only have 1 repository at this time, just return it
        //        repo = new MongoPatientTaskRepository<T>(dbName) as IPatientGoalRepository<T>;
        //        repo.UserId = userId;
        //        return repo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static IPatientGoalRepository<T> GetPatientInterventionRepository(string dbName, string productName, string userId)
        //{
        //    try
        //    {
        //        IPatientGoalRepository<T> repo = null;

        //        //We only have 1 repository at this time, just return it
        //        repo = new MongoPatientInterventionRepository<T>(dbName) as IPatientGoalRepository<T>;
        //        repo.UserId = userId;
        //        return repo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static IAttributeRepository<T> GetAttributeLibraryRepository(string dbName, string productName, string userId)
        //{
        //    try
        //    {
        //        IAttributeRepository<T> repo = null;

        //        //We only have 1 repository at this time, just return it
        //        repo = new MongoAttributeLibraryRepository<T>(dbName) as IAttributeRepository<T>;
        //        repo.UserId = userId;
        //        return repo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
