using System;
using System.Net;
using Phytel.API.DataDomain.Module;
using Phytel.API.DataDomain.Module.DTO;

namespace Phytel.API.DataDomain.Module.Service
{
    public class ModuleService : ServiceStack.ServiceInterface.Service
    {
        //public GetModuleResponse Post(GetModuleRequest request)
        //{
        //    GetModuleResponse response = new GetModuleResponse();
        //    try
        //    {
        //        response = ModuleDataManager.GetModuleByID(request);
        //        response.Version = request.Version;
        //    }
        //    catch (Exception ex)
        //    {
        //        //TODO: Log this to C3 database via ASE
        //        base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
        //    }
        //    return response;
        //}

        public GetModuleResponse Get(GetModuleRequest request)
        {
            GetModuleResponse response = new GetModuleResponse();
            try
            {
                response = ModuleDataManager.GetModuleByID(request);
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

        //public GetAllModulesResponse Post(GetAllModulesRequest request)
        //{
        //    GetAllModulesResponse response = new GetAllModulesResponse();
        //    try
        //    {
        //        response = ModuleDataManager.GetModuleList(request);
        //        response.Version = request.Version;
        //    }
        //    catch (Exception ex)
        //    {
        //        //TODO: Log this to C3 database via ASE
        //        base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
        //    }
        //    return response;
        //}
    }
}