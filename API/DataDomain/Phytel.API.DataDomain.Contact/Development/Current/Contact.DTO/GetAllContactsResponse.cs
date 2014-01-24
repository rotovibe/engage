using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class GetAllContactsResponse : IDomainResponse
   {
        public List<Contact> Contacts { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
