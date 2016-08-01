using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class GetInterventionDataResponse : IDomainResponse
   {
        public InterventionData InterventionsData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
   }
}
