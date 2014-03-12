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
        private const string _phytelUserIDToken = "x-Phytel-UserID";

        public GetYesNoStepDataResponse Get(GetYesNoStepDataRequest request)
        {
            GetYesNoStepDataResponse response = new GetYesNoStepDataResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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

        public GetTextStepDataResponse Get(GetTextStepDataRequest request)
        {
            GetTextStepDataResponse response = new GetTextStepDataResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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