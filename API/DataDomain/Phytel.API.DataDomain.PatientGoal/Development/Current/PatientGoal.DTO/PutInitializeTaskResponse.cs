using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class PutInitializeTaskResponse : IDomainResponse
    {
        public PatientTaskData Task { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
