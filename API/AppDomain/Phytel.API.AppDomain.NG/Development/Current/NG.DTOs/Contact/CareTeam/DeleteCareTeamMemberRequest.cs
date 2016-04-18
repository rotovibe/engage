using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/Contacts/{ContactId}/CareTeams/{CareTeamId}/CareTeamMembers/{Id}", "DELETE")]
    [Api(Description = "A Request object to update a care team member.")]
    public class DeleteCareTeamMemberRequest : IAppDomainRequest
    {

        [ApiMember(Name = "ContactId", Description = "Id of the contact whose care team  needs to be updated.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContactId { get; set; }

        [ApiMember(Name = "CareTeamId", Description = "Id of the care team whose  member needs to be updated.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string CareTeamId { get; set; }

        [ApiMember(Name = "Id", Description = "Id of care team Member that is being updated.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Id { get; set; }

        [ApiMember(Name = "UserId", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "query", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string Token { get; set; }
    }
}
