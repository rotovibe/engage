using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Security.DTO
{
    public class ValidateTokenResponse
    {
        public string Validated { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
