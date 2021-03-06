using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetPatientProgramsResponse : IDomainResponse
    {
        public List<ProgramInfo> Programs { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
}
