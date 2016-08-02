using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientNote.Repo;
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


        public List<PatientUtilizationData> GetPatientUtilizations(string patientId)
        {
            try
            {
                var repository = Factory.GetRepository(RepositoryType.Utilization);
                var utilList = repository.FindByPatientId(patientId);
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

        public DeleteUtilizationByPatientIdDataResponse DeletePatientUtilizationsByPatientId(DeleteUtilizationsByPatientIdDataRequest request)
        {
            DeleteUtilizationByPatientIdDataResponse response = null;
            try
            {
                response = new DeleteUtilizationByPatientIdDataResponse();

                var repo = Factory.GetRepository(RepositoryType.Utilization);
                List<PatientUtilizationData> patientUtils = repo.FindByPatientId(request.PatientId) as List<PatientUtilizationData>;
                List<string> deletedIds = null;
                if (patientUtils != null)
                {
                    deletedIds = new List<string>();
                    patientUtils.ForEach(u =>
                    {
                        repo.Delete(u.Id);
                        deletedIds.Add(u.Id);
                    });
                    response.DeletedIds = deletedIds;
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public UndoDeletePatientUtilizationsDataResponse UndoDeletePatientPatientUtilizations(UndoDeletePatientUtilizationsDataRequest request)
        {
            UndoDeletePatientUtilizationsDataResponse response = null;
            try
            {
                response = new UndoDeletePatientUtilizationsDataResponse();

                var repo = Factory.GetRepository(RepositoryType.Utilization);
                if (request.Ids != null && request.Ids.Count > 0)
                {
                    request.Ids.ForEach(u =>
                    {
                        repo.UndoDelete(u);
                    });
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}   
