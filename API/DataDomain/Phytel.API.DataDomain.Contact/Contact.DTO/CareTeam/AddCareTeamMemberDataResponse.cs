using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class AddCareTeamMemberDataResponse : IDomainResponse
    {
        public string Id { get; set; }

        public double Version { get; set; }

        public ResponseStatus Status { get; set; }
    }
}
