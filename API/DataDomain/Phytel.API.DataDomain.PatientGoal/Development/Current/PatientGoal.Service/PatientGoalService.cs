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
        private const string _phytelUserIDToken = "x-Phytel-UserID";

        public PutInitializeGoalDataResponse Put(PutInitializeGoalDataRequest request)
        {
            PutInitializeGoalDataResponse response = new PutInitializeGoalDataResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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

        public DeleteTaskResponse Delete(DeleteTaskRequest request)
        {
            DeleteTaskResponse response = new DeleteTaskResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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

        public DeleteInterventionResponse Delete(DeleteInterventionRequest request)
        {
            DeleteInterventionResponse response = new DeleteInterventionResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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

        public DeleteBarrierResponse Delete(DeleteBarrierRequest request)
        {
            DeleteBarrierResponse response = new DeleteBarrierResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
    }
}