using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PostDeletePatientResponse : IDomainResponse
    {
        public string DeletedId { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
