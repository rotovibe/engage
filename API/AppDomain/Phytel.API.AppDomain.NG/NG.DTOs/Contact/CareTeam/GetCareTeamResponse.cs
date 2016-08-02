using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetCareTeamResponse : IDomainResponse
    {
        public CareTeam CareTeam { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
  
}
