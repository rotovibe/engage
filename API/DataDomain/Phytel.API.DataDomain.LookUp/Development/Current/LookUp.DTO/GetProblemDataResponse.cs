using DataDomain.LookUp.DTO;
using Phytel.API.Interface;
using ServiceStack;

namespace Phytel.API.DataDomain.LookUp.DTO
{
   public class GetProblemDataResponse : IDomainResponse
   {
        public ProblemData Problem { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
