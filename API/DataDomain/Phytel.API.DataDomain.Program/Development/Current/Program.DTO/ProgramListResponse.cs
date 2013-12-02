using System.Collections.Generic;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class ProgramListResponse
   {
        public List<ProgramResponse> Programs { get; set; }
        public string Version { get; set; }
    }

}
