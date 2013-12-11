using System;
using System.Net;
using Phytel.API.DataDomain.Template;
using Phytel.API.DataDomain.Template.DTO;
using Phytel.API.Common.Format;

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
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            }
            return response;
        }
    }
}