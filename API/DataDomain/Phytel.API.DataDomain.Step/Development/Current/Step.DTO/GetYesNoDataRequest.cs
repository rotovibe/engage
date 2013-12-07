using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Step.DTO
{
    [Api(Description = "A Request object to get an YesNos by it's ID from the API.")]
    [Route("/{Context}/{Version}/{ContractNumber}/YesNo/{YesNoID}", "GET")]
    public class GetYesNoDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "YesNoID", Description = "ID of the YesNo being requested", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string YesNoID { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the YesNo", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Version { get; set; }
    }
}
