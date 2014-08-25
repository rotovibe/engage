using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Api(Description = "A Request object to insert a ToDo.")]
    [Route("/{Version}/{ContractNumber}/Scheduling/ToDo/Insert", "POST")]
    public class PostInsertToDoRequest : IAppDomainRequest
    {
        [ApiMember(Name = "ToDo", Description = "ToDo object to be inserted", ParameterType = "property", DataType = "ToDo", IsRequired = true)]
        public ToDo ToDo { get; set; }
        
        [ApiMember(Name = "UserId", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        public PostInsertToDoRequest() { }
    }
}

     