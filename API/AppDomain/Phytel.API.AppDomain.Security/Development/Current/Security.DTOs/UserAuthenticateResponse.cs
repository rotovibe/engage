using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.Security.DTO
{
    public class UserAuthenticateResponse
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public int SessionTimeout { get; set; }
        public string APIToken { get; set; }
        public List<ContractInfo> Contracts { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
