using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.CareMember.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/CareMember/{CareMemberID}", "GET")]
    public class GetCareMemberRequest : IDataDomainRequest
    {
        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "CareMemberID", Description = "ID of the CareMember being requested", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string CareMemberID { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the CareMember", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}
