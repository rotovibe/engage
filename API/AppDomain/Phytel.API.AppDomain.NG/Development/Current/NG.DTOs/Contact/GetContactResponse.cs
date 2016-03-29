using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetContactResponse : IDomainResponse
    {
        public Contact Contact { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
