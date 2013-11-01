using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.Security.DTO
{
    public class AuthenticateResponse
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SessionTimeout { get; set; }
        public string APIToken { get; set; }
        public List<ContractInfo> Contracts { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class ContractInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
