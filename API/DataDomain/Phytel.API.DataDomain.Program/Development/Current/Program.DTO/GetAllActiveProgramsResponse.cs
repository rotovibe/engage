using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class GetAllActiveProgramsResponse : IDomainResponse
   {
        public List<ProgramInfo> Programs { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

    public class ProgramInfo
    {
        public string ProgramID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Status { get; set; }
        public string ProgramStatus { get; set; }
    }
}
