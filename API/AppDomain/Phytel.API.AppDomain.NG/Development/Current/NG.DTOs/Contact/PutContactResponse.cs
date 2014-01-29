using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PutContactResponse : IDomainResponse
    {
        public bool Success { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
