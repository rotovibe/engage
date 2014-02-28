using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    public class GetAdditionalObservationsResponse : IDomainResponse
    {
        public List<PatientObservationData> StandardObservations { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
