using System;
using System.Net;
using Phytel.API.DataDomain.PatientGoal;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Common.Format;
using System.Configuration;
using System.Web;

namespace Phytel.API.DataDomain.PatientGoal.Service
{
    public class PatientGoalService : ServiceStack.ServiceInterface.Service
    {
        public PutInitializeGoalDataResponse Put(PutInitializeGoalDataRequest request)
        {
            PutInitializeGoalDataResponse response = new PutInitializeGoalDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Put()::Unauthorized Access");

                response = PatientGoalDataManager.InitializeGoal(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutInitializeBarrierDataResponse Put(PutInitializeBarrierDataRequest request)
        {
            PutInitializeBarrierDataResponse response = new PutInitializeBarrierDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Put()::Unauthorized Access");

                response = PatientGoalDataManager.InitializeBarrier(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetPatientGoalDataResponse Get(GetPatientGoalDataRequest request)
        {
            GetPatientGoalDataResponse response = new GetPatientGoalDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = PatientGoalDataManager.GetPatientGoal(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAllPatientGoalsDataResponse Get(GetAllPatientGoalsDataRequest request)
        {
            GetAllPatientGoalsDataResponse response = new GetAllPatientGoalsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = PatientGoalDataManager.GetPatientGoalList(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutPatientGoalDataResponse Put(PutPatientGoalDataRequest request)
        {
            PutPatientGoalDataResponse response = new PutPatientGoalDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Put()::Unauthorized Access");

                response = PatientGoalDataManager.PutPatientGoal(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutInitializeTaskResponse Put(PutInitializeTaskRequest request)
        {
            PutInitializeTaskResponse response = new PutInitializeTaskResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Put()::Unauthorized Access");

                response = PatientGoalDataManager.InsertNewPatientTask(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutUpdateTaskResponse Put(PutUpdateTaskRequest request)
        {
            PutUpdateTaskResponse response = new PutUpdateTaskResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Put()::Unauthorized Access");

                response = PatientGoalDataManager.UpdatePatientTask(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutInitializeInterventionResponse Put(PutInitializeInterventionRequest request)
        {
            PutInitializeInterventionResponse response = new PutInitializeInterventionResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Put()::Unauthorized Access");

                response = PatientGoalDataManager.InsertNewPatientIntervention(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutUpdateInterventionResponse Put(PutUpdateInterventionRequest request)
        {
            PutUpdateInterventionResponse response = new PutUpdateInterventionResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Put()::Unauthorized Access");

                response = PatientGoalDataManager.UpdatePatientIntervention(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutUpdateBarrierResponse Put(PutUpdateBarrierRequest request)
        {
            PutUpdateBarrierResponse response = new PutUpdateBarrierResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Put()::Unauthorized Access");

                response = PatientGoalDataManager.UpdatePatientBarrier(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public DeletePatientGoalDataResponse Delete(DeletePatientGoalDataRequest request)
        {
            DeletePatientGoalDataResponse response = new DeletePatientGoalDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = PatientGoalDataManager.DeletePatientGoal(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public DeleteTaskDataResponse Delete(DeleteTaskDataRequest request)
        {
            DeleteTaskDataResponse response = new DeleteTaskDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = PatientGoalDataManager.DeleteTask(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public DeleteInterventionDataResponse Delete(DeleteInterventionDataRequest request)
        {
            DeleteInterventionDataResponse response = new DeleteInterventionDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = PatientGoalDataManager.DeleteIntervention(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public DeleteBarrierDataResponse Delete(DeleteBarrierDataRequest request)
        {
            DeleteBarrierDataResponse response = new DeleteBarrierDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = PatientGoalDataManager.DeleteBarrier(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetCustomAttributesDataResponse Get(GetCustomAttributesDataRequest request)
        {
            GetCustomAttributesDataResponse response = new GetCustomAttributesDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = PatientGoalDataManager.GetCustomAttributesByType(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public DeletePatientGoalByPatientIdDataResponse Delete(DeletePatientGoalByPatientIdDataRequest request)
        {
            DeletePatientGoalByPatientIdDataResponse response = new DeletePatientGoalByPatientIdDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:PatientGoalDelete()::Unauthorized Access");

                response = PatientGoalDataManager.DeletePatientGoalByPatientId(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public UndoDeletePatientGoalDataResponse Put(UndoDeletePatientGoalDataRequest request)
        {
            UndoDeletePatientGoalDataResponse response = new UndoDeletePatientGoalDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:PatientGoalUndoDelete()::Unauthorized Access");

                response = PatientGoalDataManager.UndoDeletePatientGoals(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public RemoveProgramInPatientGoalsDataResponse Put(RemoveProgramInPatientGoalsDataRequest request)
        {
            RemoveProgramInPatientGoalsDataResponse response = new RemoveProgramInPatientGoalsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:RemoveProgramInPatientGoals()::Unauthorized Access");

                response = PatientGoalDataManager.RemoveProgramInPatientGoals(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetInterventionsDataResponse Post(GetInterventionsDataRequest request)
        {
            GetInterventionsDataResponse response = new GetInterventionsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Post()::Unauthorized Access");

                response = PatientGoalDataManager.GetInterventions(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetTasksDataResponse Post(GetTasksDataRequest request)
        {
            GetTasksDataResponse response = new GetTasksDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Post()::Unauthorized Access");

                response = PatientGoalDataManager.GetTasks(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
    }
}