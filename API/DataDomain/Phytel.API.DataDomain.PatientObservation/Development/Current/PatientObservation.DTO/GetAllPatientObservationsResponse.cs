using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    public class GetAllPatientObservationsResponse : IDomainResponse
   {
        public List<PatientObservation> PatientObservations { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
