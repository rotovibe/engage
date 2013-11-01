using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.Security.DTO
{
    public class ValidateTokenResponse
    {
        public bool IsValid { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
