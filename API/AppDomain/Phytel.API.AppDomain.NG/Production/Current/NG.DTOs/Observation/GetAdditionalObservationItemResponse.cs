using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO.Observation;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAdditionalObservationItemResponse : IDomainResponse
    {
        public PatientObservation Observation { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
}
