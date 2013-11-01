using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Security.DTO
{
    public class ValidateResponse
    {
        public bool Validated { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
