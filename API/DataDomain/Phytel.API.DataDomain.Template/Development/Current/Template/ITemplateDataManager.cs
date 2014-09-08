using System.Collections.Generic;
using Phytel.API.DataDomain.Template.DTO;

namespace Phytel.API.DataDomain.Template
{
    public interface ITemplateDataManager
    {
        DTO.Template GetTemplateByID(GetTemplateRequest request);
        List<DTO.Template> GetTemplateList(GetAllTemplatesRequest request);
    }
}