using Phytel.API.DataDomain.Template.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Template;
using System;

namespace Phytel.API.DataDomain.Template
{
    public static class TemplateDataManager
    {
        public static GetTemplateResponse GetTemplateByID(GetTemplateRequest request)
        {
            try
            {
                GetTemplateResponse result = new GetTemplateResponse();

                ITemplateRepository<GetTemplateResponse> repo = TemplateRepositoryFactory<GetTemplateResponse>.GetTemplateRepository(request.ContractNumber, request.Context);
                result = repo.FindByID(request.TemplateID) as GetTemplateResponse;

                return (result != null ? result : new GetTemplateResponse());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static GetAllTemplatesResponse GetTemplateList(GetAllTemplatesRequest request)
        {
            try
            {
                GetAllTemplatesResponse result = new GetAllTemplatesResponse();

                ITemplateRepository<GetAllTemplatesResponse> repo = TemplateRepositoryFactory<GetAllTemplatesResponse>.GetTemplateRepository(request.ContractNumber, request.Context);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
