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

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class NGService : ServiceStack.ServiceInterface.Service
    {
        public PostInitializeGoalResponse Post(PostInitializeGoalRequest request)
        {
            PostInitializeGoalResponse response = null;
            try
            {
                GoalsManager gm = new GoalsManager();
                ValidateTokenResponse result = gm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Id = gm.PostInitialGoalRequest(request);
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
    }
}