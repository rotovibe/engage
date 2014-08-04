using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class PutProgramActionProcessingResponse : IDomainResponse
    {
        public ProgramDetail program { get; set; }
        public Outcome Outcome { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
