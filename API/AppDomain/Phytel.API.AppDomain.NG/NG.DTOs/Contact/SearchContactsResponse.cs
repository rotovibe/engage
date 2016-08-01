using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class SearchContactsResponse: IDomainResponse
    {
        public SearchContactsResponse()
        {
            Contacts = new List<Contact>();
        }

        public List<Contact> Contacts { get; set; }

        public long TotalCount { get; set; }

        public double Version { get; set; }

        public ResponseStatus Status { get; set; }
    }
}
