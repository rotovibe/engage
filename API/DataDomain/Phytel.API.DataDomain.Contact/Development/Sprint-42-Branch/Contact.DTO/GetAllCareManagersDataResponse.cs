using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class GetAllCareManagersDataResponse : IDomainResponse
    {
        public List<ContactData> Contacts { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
