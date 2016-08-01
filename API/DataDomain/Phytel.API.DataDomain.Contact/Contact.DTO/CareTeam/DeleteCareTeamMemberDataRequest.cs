using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO.CareTeam
{
    [Route("/{Context}/{Version}/{ContractNumber}/Contacts/{ContactId}/CareTeams/{CareTeamId}/CareTeamMembers/{MemberId}", "DELETE")]
    [Api(Description = "A Request object to delete a care team member.")]
    public class DeleteCareTeamMemberDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ContactId", Description = "Id of the contact whose care team needs to be updated.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContactId { get; set; }

        [ApiMember(Name = "CareTeamId", Description = "Id of the care team whose member needs to be updated.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string CareTeamId { get; set; }

        [ApiMember(Name = "MemberId", Description = "Id of care team Member that is being updated.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string MemberId { get; set; }
       
        [ApiMember(Name = "Context", Description = "Product Context requesting the Contact", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }
    }
}
