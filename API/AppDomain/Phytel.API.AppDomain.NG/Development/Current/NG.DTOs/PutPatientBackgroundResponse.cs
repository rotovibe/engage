using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PutPatientBackgroundResponse : IDomainResponse
    {
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
