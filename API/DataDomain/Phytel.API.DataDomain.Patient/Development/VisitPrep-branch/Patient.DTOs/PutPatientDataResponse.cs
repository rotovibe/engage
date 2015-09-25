using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class PutPatientDataResponse : IDomainResponse
    {
        public string Id { get; set; }
        public string EngagePatientSystemId { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
