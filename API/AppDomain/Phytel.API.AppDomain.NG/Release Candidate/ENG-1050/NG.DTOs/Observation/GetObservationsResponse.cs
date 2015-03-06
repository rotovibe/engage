using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetObservationsResponse : IDomainResponse
    {
        public List<Observation> Observations { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }

    public class Observation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TypeId { get; set; }
        public bool Standard { get; set; }
    }
}
