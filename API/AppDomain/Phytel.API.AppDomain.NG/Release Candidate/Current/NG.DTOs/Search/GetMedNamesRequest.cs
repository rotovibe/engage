using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;

namespace Phytel.API.AppDomain.NG.DTO.Search
{
    [Route("/{Version}/{ContractNumber}/Search/Meds/ProprietaryNames", "GET")]
    public class GetMedNamesRequest : IAppDomainRequest
    {
        [ApiMember(Name = "Term", Description = "Term to search Name data for.", ParameterType = "Query", DataType = "string", IsRequired = true)]
        public string Term { get; set; }

        [ApiMember(Name = "Take", Description = "Number of results to return.", ParameterType = "Query", DataType = "int", IsRequired = true)]
        public int Take { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "Query", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "Path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "Header", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "Path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }
    }
}
