using System;
using System.Net;
using Phytel.API.DataDomain.Action;
using Phytel.API.DataDomain.Action.DTO;
using ServiceStack.ServiceClient.Web;
using Phytel.API.Common.Format;
using System.Configuration;
using System.Web;

namespace Phytel.API.DataDomain.Action.Service
{
    public class ActionService : ServiceStack.ServiceInterface.Service
    {
        public GetActionDataResponse Get(GetActionDataRequest request)
        {
            GetActionDataResponse response = new GetActionDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ActionDD:Get():Unauthorized Access");

                response = ActionDataManager.GetActionByID(request);
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