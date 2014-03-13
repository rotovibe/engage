using System;
using System.Net;
using Phytel.API.DataDomain.Template;
using Phytel.API.DataDomain.Template.DTO;
using Phytel.API.Common.Format;
using System.Configuration;

namespace Phytel.API.DataDomain.Template.Service
{
    public class TemplateService : ServiceStack.ServiceInterface.Service
    {
        public GetTemplateResponse Post(GetTemplateRequest request)
        {
            GetTemplateResponse response = new GetTemplateResponse();
            try
            {
                response = TemplateDataManager.GetTemplateByID(request);
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

        public GetTemplateResponse Get(GetTemplateRequest request)
        {
            GetTemplateResponse response = new GetTemplateResponse();
            try
            {
                response = TemplateDataManager.GetTemplateByID(request);
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

        public GetAllTemplatesResponse Post(GetAllTemplatesRequest request)
        {
            GetAllTemplatesResponse response = new GetAllTemplatesResponse();
            try
            {
                response = TemplateDataManager.GetTemplateList(request);
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