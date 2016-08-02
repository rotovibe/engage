using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;

namespace Phytel.API.AppDomain.NG.Observation
{
    public interface IObservationEndpointUtil
    {
        List<PatientObservationData> GetHistoricalPatientObservations(IPatientObservationsRequest request);
    }
}