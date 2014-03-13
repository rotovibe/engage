using Phytel.API.DataDomain.Template.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Template;
using System;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.Template
{
    public static class TemplateDataManager
    {
        public static GetTemplateResponse GetTemplateByID(GetTemplateRequest request)
        {
            try
            {
                GetTemplateResponse result = new GetTemplateResponse();

                ITemplateRepository<GetTemplateResponse> repo = TemplateRepositoryFactory<GetTemplateResponse>.GetTemplateRepository(request.ContractNumber, request.Context, request.UserId);
                repo.UserId = request.UserId;
                result = repo.FindByID(request.TemplateID) as GetTemplateResponse;

                return (result != null ? result : new GetTemplateResponse());
            }
            catch (Exception ex)
            { 
                throw new Exception("TemplateDD:GetTemplateByID()::" + ex.Message, ex.InnerException); 
            }
        }

        public static GetAllTemplatesResponse GetTemplateList(GetAllTemplatesRequest request)
        {
            try
            {
                GetAllTemplatesResponse result = new GetAllTemplatesResponse();

                ITemplateRepository<GetAllTemplatesResponse> repo = TemplateRepositoryFactory<GetAllTemplatesResponse>.GetTemplateRepository(request.ContractNumber, request.Context, request.UserId);
                repo.UserId = request.UserId;
                result = repo.SelectAll() as GetAllTemplatesResponse;

                return (result != null ? result : new GetAllTemplatesResponse());
            }
            catch (Exception ex)
            {
                throw new Exception("TemplateDD:GetTemplateList()::" + ex.Message, ex.InnerException);
            }
        }
    }
}   
