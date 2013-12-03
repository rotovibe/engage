using Phytel.API.DataDomain.Template.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Template;

namespace Phytel.API.DataDomain.Template
{
    public static class TemplateDataManager
    {
        public static GetTemplateResponse GetTemplateByID(GetTemplateRequest request)
        {
            GetTemplateResponse result = new GetTemplateResponse();

            ITemplateRepository<GetTemplateResponse> repo = TemplateRepositoryFactory<GetTemplateResponse>.GetTemplateRepository(request.ContractNumber, request.Context);
            result = repo.FindByID(request.TemplateID) as GetTemplateResponse;
            
            return (result != null ? result : new GetTemplateResponse());
        }

        public static GetAllTemplatesResponse GetTemplateList(GetAllTemplatesRequest request)
        {
            GetAllTemplatesResponse result = new GetAllTemplatesResponse();

            ITemplateRepository<GetAllTemplatesResponse> repo = TemplateRepositoryFactory<GetAllTemplatesResponse>.GetTemplateRepository(request.ContractNumber, request.Context);

            return result;
        }
    }
}   
