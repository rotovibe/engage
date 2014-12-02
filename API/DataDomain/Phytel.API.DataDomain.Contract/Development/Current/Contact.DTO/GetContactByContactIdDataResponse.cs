using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class GetContactByContactIdDataResponse : IDomainResponse
    {
        public ContactData Contact { get; set; }
        public int Limit { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
