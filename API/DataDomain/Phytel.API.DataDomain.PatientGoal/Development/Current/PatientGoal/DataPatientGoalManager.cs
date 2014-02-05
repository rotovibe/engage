using Phytel.API.DataDomain.PatientGoal.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientGoal;
using System;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.PatientGoal
{
    public static class PatientGoalDataManager
    {
        public static GetPatientGoalResponse GetPatientGoalByID(GetPatientGoalRequest request)
        {
            try
            {
                GetPatientGoalResponse result = new GetPatientGoalResponse();

                IPatientGoalRepository<GetPatientGoalResponse> repo = PatientGoalRepositoryFactory<GetPatientGoalResponse>.GetPatientGoalRepository(request.ContractNumber, request.Context);
                result = repo.FindByID(request.PatientGoalID) as GetPatientGoalResponse;

                // if cross-domain service call has error
                //if (result.Status != null)
                //{
                //    throw new ArgumentException(result.Status.Message, new Exception() { Source = result.Status.StackTrace });
                //}

                return (result != null ? result : new GetPatientGoalResponse());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static GetAllPatientGoalsResponse GetPatientGoalList(GetAllPatientGoalsRequest request)
        {
            try
            {
                GetAllPatientGoalsResponse result = new GetAllPatientGoalsResponse();

                IPatientGoalRepository<GetAllPatientGoalsResponse> repo = PatientGoalRepositoryFactory<GetAllPatientGoalsResponse>.GetPatientGoalRepository(request.ContractNumber, request.Context);
               

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
