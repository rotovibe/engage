using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class GetAllProgramsResponse : IDomainResponse
   {
        public List<Program> Programs { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
