using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetCareMemberResponse : IDomainResponse
    {
        public CareMember CareMember { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
