using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Phytel.API.AppDomain.NG.DTO
{    
    [Route("/{Version}/{ContractNumber}/ContactTypeLookups", "GET")]
    public class GetContactTypeLookupRequest : IAppDomainRequest
    {        
        [ApiMember(Name = "GroupType", Description = "the group type to return per request.", ParameterType = "query", DataType = "string", IsRequired = false)]
        public int GroupType { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract parameter will be defined in the route.", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "header", DataType = "string", IsRequired = false)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the Request", ParameterType = "path", DataType = "double", IsRequired = false)]
        public double Version { get; set; }
        
        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        public GetContactTypeLookupRequest() { }
    }
}
