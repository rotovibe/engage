using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.CareMember.DTO
{
    public class GetPrimaryCareManagerDataResponse : IDomainResponse
    {
        public CareMemberData CareMember { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
