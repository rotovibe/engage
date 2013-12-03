using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Template.DTO
{
    public class GetTemplateResponse : IDomainResponse
    {
        public Template Template { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Template
    {
        public string TemplateID { get; set; }
        public string Version { get; set; }
    }
}
