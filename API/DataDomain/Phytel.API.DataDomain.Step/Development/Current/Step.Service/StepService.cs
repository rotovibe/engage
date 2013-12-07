using System;
using System.Net;
using Phytel.API.DataDomain.Step;
using Phytel.API.DataDomain.Step.DTO;

namespace Phytel.API.DataDomain.Step.Service
{
    public class StepService : ServiceStack.ServiceInterface.Service
    {
        //public GetStepResponse Post(GetStepRequest request)
        //{
        //    GetStepResponse response = new GetStepResponse();
        //    try
        //    {
        //        response = StepDataManager.GetStepByID(request);
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

        //public GetStepResponse Get(GetStepRequest request)
        //{
        //    GetStepResponse response = new GetStepResponse();
        //    try
        //    {
        //     response = StepDataManager.GetStepByID(request);
        //    response.Version = request.Version;
        //                }
        //    catch (Exception ex)
        //    {
        //        //TODO: Log this to C3 database via ASE
        //        base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
        //    }
        //    return response;
        //}

        //public GetAllStepsResponse Post(GetAllStepsRequest request)
        //{
        //    GetAllStepsResponse response = new GetAllStepsResponse();
        //    try
        //    {
        //        response = StepDataManager.GetStepList(request);
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