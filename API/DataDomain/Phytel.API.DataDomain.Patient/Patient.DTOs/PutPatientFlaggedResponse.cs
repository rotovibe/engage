using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class PutPatientFlaggedResponse : IDomainResponse
    {
        public double Version { get; set; }
        public bool Success { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
