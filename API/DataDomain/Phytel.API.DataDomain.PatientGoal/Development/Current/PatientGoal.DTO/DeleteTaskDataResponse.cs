using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class DeleteTaskDataResponse : IDomainResponse
    {
        public bool Deleted { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
