using Phytel.API.DataDomain.PatientGoal.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientGoal;
using System;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.PatientGoal
{
    public static class PatientGoalDataManager
    {
        public static PutInitializeGoalDataResponse InitializeGoal(PutInitializeGoalDataRequest request)
        {
            PutInitializeGoalDataResponse response = null;
            try
            {
                response = new PutInitializeGoalDataResponse();
                IPatientGoalRepository<PutInitializeGoalDataResponse> repo = PatientGoalRepositoryFactory<PutInitializeGoalDataResponse>.GetPatientGoalRepository(request.ContractNumber, request.Context);
                response.Id = repo.Initialize(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public static GetPatientGoalDataResponse GetPatientGoal(GetPatientGoalDataRequest request)
        {
            GetPatientGoalDataResponse result = null;
            try
            {
                result = new GetPatientGoalDataResponse();

                IPatientGoalRepository<GetPatientGoalDataResponse> repo = PatientGoalRepositoryFactory<GetPatientGoalDataResponse>.GetPatientGoalRepository(request.ContractNumber, request.Context);
                result.GoalData = repo.FindByID(request.Id) as PatientGoalData;
                result.Version = request.Version;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static GetAllPatientGoalsDataResponse GetPatientGoalList(GetAllPatientGoalsDataRequest request)
        //{
        //    try
        //    {
        //        GetAllPatientGoalsDataResponse result = new GetAllPatientGoalsDataResponse();

        //        IPatientGoalRepository<GetAllPatientGoalsDataResponse> repo = PatientGoalRepositoryFactory<GetAllPatientGoalsDataResponse>.GetPatientGoalRepository(request.ContractNumber, request.Context);
               

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #region // TASKS
        public static PutInitializeTaskResponse InsertNewPatientTask(PutInitializeTaskRequest request)
        {
            try
            {
                PutInitializeTaskResponse result = new PutInitializeTaskResponse();

                IPatientGoalRepository<GetAllPatientGoalsDataResponse> repo = PatientGoalRepositoryFactory<GetAllPatientGoalsDataResponse>.GetPatientTaskRepository(request.ContractNumber, request.Context);

                PatientTaskData mePTask = new PatientTaskData
                {
                    TTLDate = System.DateTime.UtcNow.AddDays(1)
                };

                PatientTaskData rpt = (PatientTaskData)repo.Insert(mePTask);
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
