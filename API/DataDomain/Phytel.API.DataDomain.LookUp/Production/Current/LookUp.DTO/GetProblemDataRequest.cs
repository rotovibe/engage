using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    [Api(Description = "A Request object to get a problem by it's ID from the API.")]
    [Route("/{Context}/{Version}/{ContractNumber}/problem/{ProblemID}", "GET")]
    public class GetProblemDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ProblemID", Description = "ID of the problem being requested", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ProblemID { get; set; }

        [ApiMember(Name = "Context", Description = "Context", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the Data Domain API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }


}
