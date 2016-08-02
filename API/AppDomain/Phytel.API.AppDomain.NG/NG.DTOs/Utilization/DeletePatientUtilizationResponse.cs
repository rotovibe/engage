using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class DeletePatientUtilizationResponse : IDomainResponse
    {
        public double Version { get; set; }
        public bool Success { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
