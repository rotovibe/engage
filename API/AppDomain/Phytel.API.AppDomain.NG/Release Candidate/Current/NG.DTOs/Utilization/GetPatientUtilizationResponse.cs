using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO.Utilization
{
    public class GetPatientUtilizationResponse : IDomainResponse
    {
        public PatientUtilization Utilization { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
