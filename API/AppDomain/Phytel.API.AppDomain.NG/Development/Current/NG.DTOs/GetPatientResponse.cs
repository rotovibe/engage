using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetPatientResponse : IDomainResponse
    {
        public Patient Patient { get; set; }
        public ResponseStatus Status { get; set; }
        public string Version { get; set; }
    }
}
