using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class NGService : ServiceStack.ServiceInterface.Service
    {
        public GetInitializeGoalResponse Get(GetInitializeGoalRequest request)
        {
            GetInitializeGoalResponse response = new GetInitializeGoalResponse();
            GoalsManager gm = new GoalsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = gm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.GetInitialGoalRequest(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    gm.LogException(ex);
            }
            finally
            {
               List<string> patientIds = null;

                if (response.Goal != null)
                {
                    patientIds = new List<string>();
                    patientIds.Add(response.Goal.PatientId);
                }

                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetInitializeBarrierResponse Get(GetInitializeBarrierRequest request)
        {
            GetInitializeBarrierResponse response = new GetInitializeBarrierResponse();
            GoalsManager gm = new GoalsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = gm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.GetInitialBarrierRequest(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    gm.LogException(ex);
            }
            finally
            {
                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response; 
        }
        
        public GetInitializeTaskResponse Get(GetInitializeTaskRequest request)
        {
            GetInitializeTaskResponse response = null;
            GoalsManager gm = new GoalsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = gm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.GetInitialTask(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    gm.LogException(ex);
            }
            finally
            {
                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetInitializeInterventionResponse Get(GetInitializeInterventionRequest request)
        {
            GetInitializeInterventionResponse response = null;
            GoalsManager gm = new GoalsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = gm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.GetInitialIntervention(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    gm.LogException(ex);
            }
            finally
            {
                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response; 
        }

        public GetPatientGoalResponse Get(GetPatientGoalRequest request)
        {
            GetPatientGoalResponse response = new GetPatientGoalResponse();
            GoalsManager gm = new GoalsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = gm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.GetPatientGoal(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    gm.LogException(ex);
            }
            finally
            {
                 List<string> patientIds = null;

                if (response.Goal != null)
                {
                    patientIds = new List<string>();
                    patientIds.Add(response.Goal.PatientId);
                }

                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response; 
        }

        public GetAllPatientGoalsResponse Get(GetAllPatientGoalsRequest request)
        {
            GetAllPatientGoalsResponse response = new GetAllPatientGoalsResponse();
            GoalsManager gm = new GoalsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = gm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.GetAllPatientGoals(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    gm.LogException(ex);
            }
            finally
            {
                 List<string> patientIds = null;

                if (response.Goals != null)
                {
                    patientIds = response.Goals.Select(x => x.PatientId).ToList();                   
                }

                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response; 
        }

        public PostPatientGoalResponse Post(PostPatientGoalRequest request)
        {
            PostPatientGoalResponse response = new PostPatientGoalResponse();
            GoalsManager gm = new GoalsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = gm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.SavePatientGoal(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    gm.LogException(ex);
            }
            finally
            {
                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response; 
        }

        public PostDeletePatientGoalResponse Post(PostDeletePatientGoalRequest request)
        {
            PostDeletePatientGoalResponse response = null;
            GoalsManager gm = new GoalsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = gm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.DeletePatientGoal(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    gm.LogException(ex);
            }
            finally
            {
                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response; 
        }
    }
}