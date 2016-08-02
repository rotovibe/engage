using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllCareManagersResponse : IDomainResponse
    {
        public List<Contact> Contacts { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
