using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class NGService : ServiceStack.ServiceInterface.Service
    {

        public PostInsertToDoResponse Post(PostInsertToDoRequest request)
        {
            PostInsertToDoResponse response = new PostInsertToDoResponse();
            SchedulingManager toDoMgr = new SchedulingManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = toDoMgr.InsertToDo(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    toDoMgr.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;
                if (request.ToDo != null)
                {
                    patientIds = new List<string>();
                    patientIds.Add(request.ToDo.PatientId);
                }
                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public PostUpdateToDoResponse Post(PostUpdateToDoRequest request)
        {
            PostUpdateToDoResponse response = new PostUpdateToDoResponse();
            SchedulingManager toDoMgr = new SchedulingManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = toDoMgr.UpdateToDo(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    toDoMgr.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;
                if (request.ToDo != null)
                {
                    patientIds = new List<string>();
                    patientIds.Add(request.ToDo.PatientId);
                }
                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public GetToDosResponse Post(GetToDosRequest request)
        {
            GetToDosResponse response = new GetToDosResponse();
            SchedulingManager toDoMgr = new SchedulingManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = toDoMgr.GetToDos(request);                    
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    toDoMgr.LogException(ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();
                patientIds.Add(request.PatientId);
                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }
    }
}