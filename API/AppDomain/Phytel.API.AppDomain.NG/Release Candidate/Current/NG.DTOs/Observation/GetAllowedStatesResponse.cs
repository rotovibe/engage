using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO.Observation;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllowedStatesResponse : IDomainResponse
    {
        public List<IdNamePair> States { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
}
