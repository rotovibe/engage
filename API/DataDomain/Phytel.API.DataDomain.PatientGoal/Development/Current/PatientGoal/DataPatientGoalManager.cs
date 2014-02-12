using Phytel.API.DataDomain.PatientGoal.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientGoal;
using System;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.PatientGoal
{
    public static class PatientGoalDataManager
    {
        public static GetPatientGoalDataResponse GetPatientGoalByID(GetPatientGoalDataRequest request)
        {
            try
            {
                GetPatientGoalDataResponse result = new GetPatientGoalDataResponse();

                IPatientGoalRepository<GetPatientGoalDataResponse> repo = PatientGoalRepositoryFactory<GetPatientGoalDataResponse>.GetPatientGoalRepository(request.ContractNumber, request.Context);
                result = repo.FindByID(request.PatientGoalId) as GetPatientGoalDataResponse;

                // if cross-domain service call has error
                //if (result.Status != null)
                //{
                //    throw new ArgumentException(result.Status.Message, new Exception() { Source = result.Status.StackTrace });
                //}

                return (result != null ? result : new GetPatientGoalDataResponse());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static GetAllPatientGoalsDataResponse GetPatientGoalList(GetAllPatientGoalsDataRequest request)
        {
            try
            {
                GetAllPatientGoalsDataResponse result = new GetAllPatientGoalsDataResponse();

                IPatientGoalRepository<GetAllPatientGoalsDataResponse> repo = PatientGoalRepositoryFactory<GetAllPatientGoalsDataResponse>.GetPatientGoalRepository(request.ContractNumber, request.Context);
               

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region // TASKS
        public static PutInitializeTaskResponse InsertNewPatientTask(PutInitializeTaskRequest request)
        {
            try
            {
                PutInitializeTaskResponse result = new PutInitializeTaskResponse();

                IPatientGoalRepository<GetAllPatientGoalsDataResponse> repo = PatientGoalRepositoryFactory<GetAllPatientGoalsDataResponse>.GetPatientTaskRepository(request.ContractNumber, request.Context);

                PatientTask mePTask = new PatientTask
                {
                    TTLDate = System.DateTime.UtcNow.AddDays(1)
                };

                PatientTask rpt = (PatientTask)repo.Insert(mePTask);
                result.Id = rpt.Id;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}   
