using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientSystem.DTO
{
    public class InsertPatientSystemDataResponse : IDomainResponse
    {
        public PatientSystemData PatientSystemData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
