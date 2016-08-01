using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    public class DomainResponse : IDomainResponse
    {
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
