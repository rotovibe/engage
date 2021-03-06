using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class PutRecentPatientResponse : IDomainResponse
    {
        public bool SuccessData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
