using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Template.DTO
{
    public class GetAllTemplatesResponse : IDomainResponse
   {
        public List<Template> Templates { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
