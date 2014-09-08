using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Template.DTO
{
    public class GetAllTemplatesResponse : DomainResponse
   {
        public List<Template> Templates { get; set; }
   }

}
