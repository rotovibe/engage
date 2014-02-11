using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    [Api(Description = "A Request object to get all communication modes from the API.")]
    [Route("/{Context}/{Version}/{ContractNumber}/{Type}", "GET")]
    public class GetLookUpsDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Type", Description = "Type of the lookup being requested.", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string Type { get; set; }

        [ApiMember(Name = "Context", Description = "Context", ParameterType = "body", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the Data Domain API being called", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}
