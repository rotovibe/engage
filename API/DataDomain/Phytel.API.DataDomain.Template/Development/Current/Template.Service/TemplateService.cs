using System;
using System.Net;
using DataDomain.Template.Repo;
using Phytel.API.Common;
using Phytel.API.DataDomain.Template;
using Phytel.API.DataDomain.Template.DTO;
using Phytel.API.Common.Format;
using System.Configuration;

namespace Phytel.API.DataDomain.Template.Service
{
    public class TemplateService : ServiceStack.ServiceInterface.Service
    {
        public ITemplateDataManager Manager { get; set; }
        public ICommonFormatterUtil FormatUtil { get; set; }
        public IHelpers Helpers { get; set; }

        public GetTemplateResponse Post(GetTemplateRequest request)
        {
            GetTemplateResponse response = new GetTemplateResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Put()::Unauthorized Access");

                response = Manager.GetTemplateByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                FormatUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetTemplateResponse Get(GetTemplateRequest request)
        {
            GetTemplateResponse response = new GetTemplateResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Put()::Unauthorized Access");

                response = Manager.GetTemplateByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                FormatUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAllTemplatesResponse Post(GetAllTemplatesRequest request)
        {
            GetAllTemplatesResponse response = new GetAllTemplatesResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("ProgramDD:Put()::Unauthorized Access");

                response = Manager.GetTemplateList(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                FormatUtil.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helpers.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
    }
}