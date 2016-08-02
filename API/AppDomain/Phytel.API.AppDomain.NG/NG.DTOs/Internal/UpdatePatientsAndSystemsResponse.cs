using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO.Internal
{
    public class UpdatePatientsAndSystemsResponse : IDomainResponse
    {
        public string Message { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
}
