using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class SaveCareTeamResponse : IDomainResponse
    {
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
        public string Id { get; set; }
    }
}
