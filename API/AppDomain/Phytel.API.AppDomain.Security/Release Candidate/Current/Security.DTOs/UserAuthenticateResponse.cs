using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.Security.DTO
{
    public class UserAuthenticateResponse : IDomainResponse
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public int SessionTimeout { get; set; }
        public string APIToken { get; set; }
        public List<ContractInfo> Contracts { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
}
