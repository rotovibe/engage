using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    public class PutRegisterPatientObservationResponse : IDomainResponse
    {
        public PatientObservationData PatientObservation { get; set; }
        public double Version { get; set; }
        public Outcome Outcome { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
