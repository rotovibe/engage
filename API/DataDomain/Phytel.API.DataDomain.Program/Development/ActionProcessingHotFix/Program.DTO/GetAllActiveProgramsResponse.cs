using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class GetAllActiveProgramsResponse : IDomainResponse
   {
        public List<ProgramInfo> Programs { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
   }
}
