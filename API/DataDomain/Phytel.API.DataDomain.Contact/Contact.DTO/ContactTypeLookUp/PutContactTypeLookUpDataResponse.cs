using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Contact.DTO.ContactTypeLookUp
{
    public class PutContactTypeLookUpDataResponse : IDomainResponse
    {
        public string Id { get; set; }

        public double Version { get; set; }

        public ResponseStatus Status { get; set; }
    }
}
