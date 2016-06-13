﻿using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/Contacts/{ContactId}", "GET")]
    public class GetContactByContactIdRequest : IAppDomainRequest
    {
        [ApiMember(Name = "ContactId", Description = "ContactId to fetch", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContactId { get; set; }

        [ApiMember(Name = "UserId", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "query", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string Token { get; set; }
    }
}
