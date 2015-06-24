using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PostPatientInterventionResponse : IDomainResponse
    {
        public PatientIntervention Intervention { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
