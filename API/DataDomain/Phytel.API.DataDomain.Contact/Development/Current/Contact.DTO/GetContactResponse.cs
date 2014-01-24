using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class GetContactResponse : IDomainResponse
    {
        public Contact Contact { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Contact
    {
        public string ContactID { get; set; }
        public string Version { get; set; }
    }
}
