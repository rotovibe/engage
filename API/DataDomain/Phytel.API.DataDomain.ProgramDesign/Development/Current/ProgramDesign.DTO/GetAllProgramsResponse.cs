using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    public class GetAllProgramsResponse : IDomainResponse
   {
        public List<Program> Programs { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
