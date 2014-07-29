using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using Phytel.API.AppDomain.NG.DTO.Observation;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetPatientProblemsResponse : IDomainResponse
   {
        public List<PatientObservation> Problems { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
