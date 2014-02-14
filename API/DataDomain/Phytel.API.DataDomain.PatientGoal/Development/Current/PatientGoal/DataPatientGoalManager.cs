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

                IPatientGoalRepository<PutInitializeTaskResponse> repo = PatientGoalRepositoryFactory<PutInitializeTaskResponse>.GetPatientTaskRepository(request.ContractNumber, request.Context);

                result.Id = repo.Initialize(request);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PutUpdateTaskResponse UpdatePatientTask(PutUpdateTaskRequest request)
        {
            try
            {
                PutUpdateTaskResponse result = new PutUpdateTaskResponse();

                IPatientGoalRepository<PutUpdateTaskResponse> repo = PatientGoalRepositoryFactory<PutUpdateTaskResponse>.GetPatientTaskRepository(request.ContractNumber, request.Context);
                bool status = (bool)repo.Update(request.Task);

                result.Updated = status;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public static PutInitializeInterventionResponse InsertNewPatientIntervention(PutInitializeInterventionRequest request)
        {
            try
            {
                PutInitializeInterventionResponse result = new PutInitializeInterventionResponse();

                IPatientGoalRepository<PutInitializeInterventionResponse> repo = PatientGoalRepositoryFactory<PutInitializeInterventionResponse>.GetPatientInterventionRepository(request.ContractNumber, request.Context);

                result.Id = repo.Initialize(request);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PutUpdateInterventionResponse UpdatePatientIntervention(PutUpdateInterventionRequest request)
        {
            try
            {
                PutUpdateInterventionResponse result = new PutUpdateInterventionResponse();

                IPatientGoalRepository<PutUpdateInterventionResponse> repo = PatientGoalRepositoryFactory<PutUpdateInterventionResponse>.GetPatientInterventionRepository(request.ContractNumber, request.Context);
                bool status = (bool)repo.Update(request.Intervention);

                result.Updated = status;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PutPatientGoalDataResponse PutPatientGoal(PutPatientGoalDataRequest request)
        {
            try
            {
                PutPatientGoalDataResponse result = new PutPatientGoalDataResponse();

                IPatientGoalRepository<PutPatientGoalDataResponse> repo = PatientGoalRepositoryFactory<PutPatientGoalDataResponse>.GetPatientGoalRepository(request.ContractNumber, request.Context);
                bool status = (bool)repo.Update(request.GoalData);

                result.Updated = status;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
