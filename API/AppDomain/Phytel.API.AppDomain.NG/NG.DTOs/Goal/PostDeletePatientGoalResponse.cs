using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PostDeletePatientGoalResponse : IDomainResponse
    {
        public bool Result { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
