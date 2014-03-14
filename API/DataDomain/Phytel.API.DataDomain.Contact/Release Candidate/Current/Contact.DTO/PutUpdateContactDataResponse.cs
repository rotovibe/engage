using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class PutUpdateContactDataResponse : IDomainResponse
    {
        public bool SuccessData { get; set; }
        public List<CleanupIdData> UpdatedPhoneData { get; set; }
        public List<CleanupIdData> UpdatedEmailData { get; set; }
        public List<CleanupIdData> UpdatedAddressData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class CleanupIdData
    {
        public string OldId { get; set; }
        public string NewId { get; set; }
    }
}
