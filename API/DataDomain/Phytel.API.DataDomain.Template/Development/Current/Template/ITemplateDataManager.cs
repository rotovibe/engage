using Phytel.API.DataDomain.Template.DTO;

namespace Phytel.API.DataDomain.Template
{
    public interface ITemplateDataManager
    {
        GetTemplateResponse GetTemplateByID(GetTemplateRequest request);
        GetAllTemplatesResponse GetTemplateList(GetAllTemplatesRequest request);
    }
}