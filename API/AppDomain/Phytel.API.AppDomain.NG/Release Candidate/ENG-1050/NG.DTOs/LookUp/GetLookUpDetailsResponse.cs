using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetLookUpDetailsResponse : IDomainResponse
    {
        public List<LookUpDetails> LookUpDetails { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
}
