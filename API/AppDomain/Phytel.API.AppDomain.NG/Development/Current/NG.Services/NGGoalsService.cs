using System;
using System.Net;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.ServiceHost;
using Phytel.API.Common.Format;
using Phytel.API.AppDomain.Security.DTO;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceInterface.Cors;
using System.Text;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class NGService : ServiceStack.ServiceInterface.Service
    {
        public GetInitializeGoalResponse Get(GetInitializeGoalRequest request)
        {
            GetInitializeGoalResponse response = new GetInitializeGoalResponse();
            try
            {
                GoalsManager gm = new GoalsManager();
                ValidateTokenResponse result = gm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.GetInitialGoalRequest(request);
                }
                else
                    throw new UnauthorizedAccessException();

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }

        public GetInitializeBarrierResponse Get(GetInitializeBarrierRequest request)
        {
            GetInitializeBarrierResponse response = new GetInitializeBarrierResponse();
            try
            {
                GoalsManager gm = new GoalsManager();
                ValidateTokenResponse result = gm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.GetInitialBarrierRequest(request);
                }
                else
                    throw new UnauthorizedAccessException();

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }
        
        public GetInitializeTaskResponse Get(GetInitializeTaskRequest request)
        {
            GetInitializeTaskResponse response = null;
            try
            {
                GoalsManager gm = new GoalsManager();
                ValidateTokenResponse result = gm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.GetInitialTask(request);
                }
                else
                    throw new UnauthorizedAccessException();

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }

        public GetInitializeInterventionResponse Get(GetInitializeInterventionRequest request)
        {
            GetInitializeInterventionResponse response = null;
            try
            {
                GoalsManager gm = new GoalsManager();
                ValidateTokenResponse result = gm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.GetInitialIntervention(request);
                }
                else
                    throw new UnauthorizedAccessException();

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }

        public GetPatientGoalResponse Get(GetPatientGoalRequest request)
        {
            GetPatientGoalResponse response = new GetPatientGoalResponse();
            try
            {
                GoalsManager gm = new GoalsManager();
                ValidateTokenResponse result = gm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.GetPatientGoal(request);
                }
                else
                    throw new UnauthorizedAccessException();

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }

        public GetAllPatientGoalsResponse Get(GetAllPatientGoalsRequest request)
        {
            GetAllPatientGoalsResponse response = new GetAllPatientGoalsResponse();
            try
            {
                GoalsManager gm = new GoalsManager();
                ValidateTokenResponse result = gm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.GetAllPatientGoals(request);
                }
                else
                    throw new UnauthorizedAccessException();

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }

        public PostPatientGoalResponse Post(PostPatientGoalRequest request)
        {
            PostPatientGoalResponse response = new PostPatientGoalResponse();

            try
            {
                GoalsManager gm = new GoalsManager();
                ValidateTokenResponse result = gm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.SavePatientGoal(request);
                }
                else
                    throw new UnauthorizedAccessException();

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }

        public PostDeletePatientGoalResponse Post(PostDeletePatientGoalRequest request)
        {
            PostDeletePatientGoalResponse response = null;
            try
            {
                GoalsManager gm = new GoalsManager();
                ValidateTokenResponse result = gm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = gm.DeletePatientGoal(request);
                }
                else
                    throw new UnauthorizedAccessException();

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }
    }
}