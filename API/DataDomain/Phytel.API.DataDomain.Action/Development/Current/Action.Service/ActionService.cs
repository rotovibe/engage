using System;
using System.Net;
using Phytel.API.DataDomain.Action;
using Phytel.API.DataDomain.Action.DTO;

namespace Phytel.API.DataDomain.Action.Service
{
    public class ActionService : ServiceStack.ServiceInterface.Service
    {
        public GetActionResponse Post(GetActionRequest request)
        {
            GetActionResponse response = new GetActionResponse();
            try
            {
                response = ActionDataManager.GetActionByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public GetActionResponse Get(GetActionRequest request)
        {
            GetActionResponse response = new GetActionResponse();
            try
            {
             response = ActionDataManager.GetActionByID(request);
            response.Version = request.Version;
                        }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public GetAllActionsResponse Post(GetAllActionsRequest request)
        {
            GetAllActionsResponse response = new GetAllActionsResponse();
            try
            {
                response = ActionDataManager.GetActionList(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }
    }
}