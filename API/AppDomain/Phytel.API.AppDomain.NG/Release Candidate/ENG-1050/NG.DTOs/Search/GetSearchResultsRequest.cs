using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;

namespace Phytel.API.AppDomain.NG.DTO.Search
{
    [Route("/{Version}/{ContractNumber}/Search/{SearchDomain}", "GET")]
    public class GetSearchResultsRequest : IAppDomainRequest
    {
        [ApiMember(Name = "SearchTerm", Description = "Term to search for.", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string SearchTerm { get; set; }

        [ApiMember(Name = "Take", Description = "Number of results to return.", ParameterType = "property", DataType = "int", IsRequired = true)]
        public int Take { get; set; }

        [ApiMember(Name = "SearchDomain", Description = "Domain to search data from.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string SearchDomain { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Program", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }
    }
}
