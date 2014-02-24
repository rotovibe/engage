using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.CareMember.DTO
{
    public class GetCareMemberResponse : IDomainResponse
    {
        public CareMember CareMember { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class CareMember
    {
        public string CareMemberID { get; set; }
        public string Version { get; set; }
    }
}
