using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.Security.DTO
{
    public class ValidateTokenResponse : IDomainResponse
    {
        public bool IsValid { get; set; }
        public ResponseStatus Status { get; set; }
        public string Version { get; set; }
    }
}
