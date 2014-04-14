using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    public class GetAllProgramDesignsResponse : IDomainResponse
   {
        public List<ProgramDesign> ProgramDesigns { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
