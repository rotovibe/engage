using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.LookUp.DTO
{
   public class GetProblemDataResponse : IDomainResponse
   {
        public ProblemData Problem { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
