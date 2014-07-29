using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using Phytel.API.DataDomain.PatientObservation.DTO;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    public class GetInitializeProblemDataResponse : IDomainResponse
    {
        public PatientObservationData PatientObservation { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
