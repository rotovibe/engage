using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.CareMember.DTO
{
    public class GetAllCareMembersDataResponse : IDomainResponse
   {
        public List<CareMemberData> CareMembers { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
   }
}
