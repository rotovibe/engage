using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Contact.DTO.CareTeam
{
    public class SaveCareTeamDataResponse : IDomainResponse
    {
        public double Version { get; set; }

        public ResponseStatus Status { get; set; }

        public string Id { get; set; }
    }
}
