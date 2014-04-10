using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    public class GetPatientProblemsSummaryResponse : IDomainResponse
   {
        public List<PatientObservationData> PatientObservations { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
