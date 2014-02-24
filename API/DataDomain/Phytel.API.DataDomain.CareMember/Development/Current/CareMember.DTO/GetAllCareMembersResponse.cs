using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.CareMember.DTO
{
    public class GetAllCareMembersResponse : IDomainResponse
   {
        public List<CareMember> CareMembers { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
