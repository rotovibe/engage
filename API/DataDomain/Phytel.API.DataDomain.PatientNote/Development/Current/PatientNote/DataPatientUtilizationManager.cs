using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientNote.Repo;
using ServiceStack.Common;
using System.Linq;
using DataDomain.PatientNote.Repo;

namespace Phytel.API.DataDomain.PatientNote
{
    public class DataPatientUtilizationManager : IDataPatientUtilizationManager
    {
        IPatientNoteRepositoryFactory Factory { get; set; }


        public DataPatientUtilizationManager(IPatientNoteRepositoryFactory repo)
        {
            Factory = repo;
        }

        public PatientUtilizationData InsertPatientUtilization(PatientUtilizationData data)
        {
            try
            {
                var repository = Factory.GetRepository(RepositoryType.Utilization);
                var pUtil = repository.Insert(data) as PatientUtilizationData;
                return pUtil;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PatientUtilizationData UpdatePatientUtilization(PatientUtilizationData data)
        {
            try
            {
                var repository = Factory.GetRepository(RepositoryType.Utilization);
                var result = repository.Update(data);
                return result as PatientUtilizationData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<PatientUtilizationData> GetPatientUtilizations(string userId)
        {
            try
            {
                var repository = Factory.GetRepository(RepositoryType.Utilization);
                var utilList = repository.FindByPatientId(userId);
                List<PatientUtilizationData> listU = null;
                if (utilList != null)
                    listU = utilList.Select(e => (PatientUtilizationData) e).ToList();
                return listU;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public PatientUtilizationData GetPatientUtilization(string utilId)
        {
            try
            {
                var repository = Factory.GetRepository(RepositoryType.Utilization);
                var utilList = repository.FindByID(utilId);
                return utilList as PatientUtilizationData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeletePatientUtilization(string utilId)
        {
            try
            {
                var repository = Factory.GetRepository(RepositoryType.Utilization);
                repository.Delete(utilId);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
