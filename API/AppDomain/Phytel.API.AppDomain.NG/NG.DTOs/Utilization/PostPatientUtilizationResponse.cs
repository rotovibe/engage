using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO.Utilization
{
    public class PostPatientUtilizationResponse : IDomainResponse
    {
        public PatientUtilization Utilization { get; set; }
        public double Version { get; set; }
        public bool Result { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
