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
        public IPatientGoalDataManager Manager { get; set; }

        #region Initialize
        public PutInitializeGoalDataResponse Put(PutInitializeGoalDataRequest request)
        {
            PutInitializeGoalDataResponse response = new PutInitializeGoalDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Put()::Unauthorized Access");

                response = Manager.InitializeGoal(request);
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

                response = Manager.InitializeBarrier(request);
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

                response = Manager.InsertNewPatientTask(request);
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

                response = Manager.InsertNewPatientIntervention(request);
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
        #endregion

        #region Gets
        public GetPatientGoalDataResponse Get(GetPatientGoalDataRequest request)
        {
            GetPatientGoalDataResponse response = new GetPatientGoalDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = Manager.GetPatientGoal(request);
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

        public GetPatientGoalByTemplateIdResponse Get(GetPatientGoalByTemplateIdRequest request)
        {
            GetPatientGoalByTemplateIdResponse response = new GetPatientGoalByTemplateIdResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = Manager.GetPatientByTemplateIdGoal(request);
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

        public GetTaskDataResponse Get(GetTaskDataRequest request)
        {
            GetTaskDataResponse response = new GetTaskDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = Manager.GetTask(request);
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

        public GetInterventionDataResponse Get(GetInterventionDataRequest request)
        {
            GetInterventionDataResponse response = new GetInterventionDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = Manager.GetIntervention(request);
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

        public GetGoalDataResponse Get(GetGoalDataRequest request)
        {
            GetGoalDataResponse response = new GetGoalDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = Manager.GetGoal(request);
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

                response = Manager.GetPatientGoalList(request);
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

        public GetPatientInterventionsDataResponse Post(GetPatientInterventionsDataRequest request)
        {
            GetPatientInterventionsDataResponse response = new GetPatientInterventionsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Post()::Unauthorized Access");

                response = Manager.GetPatientInterventions(request);
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

        public GetPatientTaskByTemplateIdResponse Get(GetPatientTaskByTemplateIdRequest request)
        {
            GetPatientTaskByTemplateIdResponse response = new GetPatientTaskByTemplateIdResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = Manager.GetPatientTaskByTemplateId(request);
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

        public GetPatientInterventionByTemplateIdResponse Get(GetPatientInterventionByTemplateIdRequest request)
        {
            GetPatientInterventionByTemplateIdResponse response = new GetPatientInterventionByTemplateIdResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = Manager.GetPatientInterventionByTemplateId(request);
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

        public GetPatientTasksDataResponse Post(GetPatientTasksDataRequest request)
        {
            GetPatientTasksDataResponse response = new GetPatientTasksDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Post()::Unauthorized Access");

                response = Manager.GetTasks(request);
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
        #endregion

        #region Update
        public PutUpdateGoalDataResponse Put(PutUpdateGoalDataRequest request)
        {
            PutUpdateGoalDataResponse response = new PutUpdateGoalDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Put()::Unauthorized Access");

                response = Manager.PutPatientGoal(request);
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

                response = Manager.UpdatePatientTask(request);
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

                response = Manager.UpdatePatientIntervention(request);
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

                response = Manager.UpdatePatientBarrier(request);
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
        #endregion

        #region Delete & UndoDelete
        public DeletePatientGoalDataResponse Delete(DeletePatientGoalDataRequest request)
        {
            DeletePatientGoalDataResponse response = new DeletePatientGoalDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = Manager.DeletePatientGoal(request);
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

                response = Manager.DeleteTask(request);
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

                response = Manager.DeleteIntervention(request);
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

                response = Manager.DeleteBarrier(request);
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

                response = Manager.DeletePatientGoalByPatientId(request);
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

                response = Manager.UndoDeletePatientGoals(request);
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
        #endregion

        #region CustomAttribute
        public GetCustomAttributesDataResponse Get(GetCustomAttributesDataRequest request)
        {
            GetCustomAttributesDataResponse response = new GetCustomAttributesDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:Get()::Unauthorized Access");

                response = Manager.GetCustomAttributesByType(request);
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
        #endregion

        #region RemoveProgram
        public RemoveProgramInPatientGoalsDataResponse Put(RemoveProgramInPatientGoalsDataRequest request)
        {
            RemoveProgramInPatientGoalsDataResponse response = new RemoveProgramInPatientGoalsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientGoalDD:RemoveProgramInPatientGoals()::Unauthorized Access");

                response = Manager.RemoveProgramInPatientGoals(request);
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
        #endregion
    }
}