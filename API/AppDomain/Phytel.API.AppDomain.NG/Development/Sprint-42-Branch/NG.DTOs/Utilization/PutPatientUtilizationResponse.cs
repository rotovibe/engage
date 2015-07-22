using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO.Utilization
{
    public class PutPatientUtilizationResponse : IDomainResponse
    {
        public PatientUtilization Utilization { get; set; }
        public double Version { get; set; }
        public bool Result { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
