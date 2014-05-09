using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;

namespace Phytel.API.AppDomain.Security.DTO
{
    public class ValidateTokenResponse : IDomainResponse
    {
        public string TokenId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string SQLUserId { get; set; }
        public int SessionLengthInMinutes { get; set; }
        public DateTime SessionTimeOut { get; set; }
        
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
}
