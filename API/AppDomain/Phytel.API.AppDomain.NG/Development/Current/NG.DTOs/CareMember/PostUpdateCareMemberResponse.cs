using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PostUpdateCareMemberResponse : IDomainResponse
    {
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
