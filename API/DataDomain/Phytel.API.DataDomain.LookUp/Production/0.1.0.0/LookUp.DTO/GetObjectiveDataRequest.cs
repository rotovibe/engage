using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    [Api(Description = "A Request object to get an objectives by it's ID from the API.")]
    [Route("/{Context}/{Version}/{ContractNumber}/Objective/{ObjectiveID}", "GET")]
    public class GetObjectiveDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ObjectiveID", Description = "ID of the Objective being requested", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ObjectiveID { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Objective", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}
