using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class GetProgramResponse : IDomainResponse
    {
        public Program Program { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Program
    {
        public string ProgramID { get; set; }
        public string Version { get; set; }
    }
}
