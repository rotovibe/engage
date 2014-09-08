using System;
using Phytel.API.DataDomain.Template.DTO;

namespace Phytel.API.DataDomain.Template.Service
{
    public class TemplateService : ServiceBase
    {
        protected readonly ITemplateDataManager Manager;

        public TemplateService(ITemplateDataManager mgr)
        {
            Manager = mgr;
        }

        public GetTemplateResponse Post(GetTemplateRequest request)
        {
            var response = new GetTemplateResponse{ Version = request.Version};
            try
            {
                RequireUserId(request);
                response.Template = Manager.GetTemplateByID(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public GetTemplateResponse Get(GetTemplateRequest request)
        {
            var response = new GetTemplateResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.Template = Manager.GetTemplateByID(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public GetAllTemplatesResponse Post(GetAllTemplatesRequest request)
        {
            var response = new GetAllTemplatesResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                response.Templates = Manager.GetTemplateList(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
    }
}