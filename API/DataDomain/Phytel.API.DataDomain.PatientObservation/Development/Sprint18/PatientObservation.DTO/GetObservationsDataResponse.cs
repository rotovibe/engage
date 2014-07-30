using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    public class GetObservationsDataResponse : IDomainResponse
    {
        public List<ObservationData> ObservationsData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
