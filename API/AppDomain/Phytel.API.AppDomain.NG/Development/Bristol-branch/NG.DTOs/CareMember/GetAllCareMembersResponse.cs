using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllCareMembersResponse : IDomainResponse
    {
        public List<CareMember> CareMembers { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
