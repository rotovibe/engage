using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PostCareMemberResponse : IDomainResponse
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
