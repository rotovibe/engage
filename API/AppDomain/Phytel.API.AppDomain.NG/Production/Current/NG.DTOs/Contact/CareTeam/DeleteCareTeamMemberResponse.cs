using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO
{
   public class DeleteCareTeamMemberResponse: IDomainResponse
    {
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
