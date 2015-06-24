using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PostPatientToProgramsResponse : IDomainResponse
    {
        public ProgramInfo Program { get; set; }
        public Outcome Result { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Outcome
    {
        public int Result { get; set; }
        public string Reason { get; set; }
    }
}
