using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Template.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Template/{TemplateID}", "GET")]
    public class GetTemplateRequest : IDataDomainRequest
    {
        [ApiMember(Name = "TemplateID", Description = "ID of the Template being requested", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string TemplateID { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Template", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}
