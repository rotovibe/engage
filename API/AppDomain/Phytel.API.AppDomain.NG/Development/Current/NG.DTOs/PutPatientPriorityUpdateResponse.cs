﻿using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PutPatientPriorityUpdateResponse : IDomainResponse
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
