using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientSystem.DTO
{
    public class GetPatientSystemDataResponse : IDomainResponse
    {
        public PatientSystemData PatientSystem { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
