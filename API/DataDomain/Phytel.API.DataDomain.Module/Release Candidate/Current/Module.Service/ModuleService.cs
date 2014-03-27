using System;
using System.Net;
using Phytel.API.DataDomain.Module;
using Phytel.API.DataDomain.Module.DTO;
using Phytel.API.Common.Format;
using System.Configuration;
using System.Web;

namespace Phytel.API.DataDomain.Module.Service
{
    public class ModuleService : ServiceStack.ServiceInterface.Service
    {
        public GetModuleResponse Get(GetModuleRequest request)
        {
            GetModuleResponse response = new GetModuleResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ModuleDD:Get()::Unauthorized Access");

                response = ModuleDataManager.GetModuleByID(request);
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