using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO.Utilization
{
    public class GetPatientUtilizationsResponse : IDomainResponse
    {
        public List<PatientUtilization> Utilizations { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
