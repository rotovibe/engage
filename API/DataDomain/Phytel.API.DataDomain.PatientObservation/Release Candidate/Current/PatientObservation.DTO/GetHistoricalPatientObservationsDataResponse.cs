using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    public class GetHistoricalPatientObservationsDataResponse : IDomainResponse
    {
        public List<PatientObservationData> PatientObservationsData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
