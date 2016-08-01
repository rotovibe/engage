using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class GetProgramDetailsSummaryResponse : IDomainResponse
    {
        public ProgramDetail Program { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
