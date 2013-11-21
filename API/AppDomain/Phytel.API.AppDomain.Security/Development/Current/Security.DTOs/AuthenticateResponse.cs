using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.Security.DTO
{
    public class AuthenticateResponse: IDomainResponse
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SessionTimeout { get; set; }
        public string APIToken { get; set; }
        public List<ContractInfo> Contracts { get; set; }
        public ResponseStatus Status { get; set; }
        public string Version { get; set; }
    }
}
