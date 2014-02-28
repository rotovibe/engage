using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO.Observation;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAdditionalObservationItemsResponse : IDomainResponse
    {
        public List<PatientObservation> Observations { get; set; }
        public ResponseStatus Status { get; set; }
        public string Version { get; set; }
    }
}
