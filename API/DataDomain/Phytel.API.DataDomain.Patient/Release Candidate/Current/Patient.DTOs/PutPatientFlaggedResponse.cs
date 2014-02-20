using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class PutPatientFlaggedResponse : IDomainResponse
    {
        public string Version { get; set; }
        public bool flagged { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
