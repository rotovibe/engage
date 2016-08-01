using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Contact.DTO.ContactTypeLookUp
{
    public class GetContactTypeLookUpDataResponse : IDomainResponse
    {
        public GetContactTypeLookUpDataResponse()
        {
            ContactTypeLookUps = new List<ContactTypeLookUpData>();
        }

        public List<ContactTypeLookUpData> ContactTypeLookUps { get; set; }

        public double Version { get; set; }

        public ResponseStatus Status { get; set; }
    }
}
