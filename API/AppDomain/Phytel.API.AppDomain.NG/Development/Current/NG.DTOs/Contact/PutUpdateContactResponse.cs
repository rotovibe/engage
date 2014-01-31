using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PutUpdateContactResponse : IDomainResponse
    {
        public bool Success { get; set; }
        public List<CleanupId> UpdatedPhone { get; set; }
        public List<CleanupId> UpdatedEmail { get; set; }
        public List<CleanupId> UpdatedAddress { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class CleanupId
    {
        public string OldId { get; set; }
        public string NewId { get; set; }
    }
}
