using System;
using System.Net;
using Phytel.API.DataDomain.Step;
using Phytel.API.DataDomain.Step.DTO;
using System.Configuration;
using Phytel.API.Common.Format;
using System.Web;

namespace Phytel.API.DataDomain.Step.Service
{
    public class StepService : ServiceStack.ServiceInterface.Service
    {
        public GetYesNoStepDataResponse Get(GetYesNoStepDataRequest request)
        {
            GetYesNoStepDataResponse response = new GetYesNoStepDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("StepDD:Get()::Unauthorized Access");

                response = StepDataManager.GetYesNoStepByID(request);
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

        public GetAllYesNoStepDataResponse Get(GetAllYesNoStepDataRequest request)
        {
            GetAllYesNoStepDataResponse response = new GetAllYesNoStepDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("StepDD:Get()::Unauthorized Access");

                response = StepDataManager.GetAllYesNoSteps(request);
                //response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetTextStepDataResponse Get(GetTextStepDataRequest request)
        {
            GetTextStepDataResponse response = new GetTextStepDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("StepDD:Get()::Unauthorized Access");

                response = StepDataManager.GetTextStepByID(request);
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