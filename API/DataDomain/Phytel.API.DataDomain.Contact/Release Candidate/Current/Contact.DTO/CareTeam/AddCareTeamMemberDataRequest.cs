using Phytel.API.DataDomain.Contact.DTO.CareTeam;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Contacts/{ContactId}/CareTeams/{CareTeamId}/CareTeamMembers", "POST")]
    public class AddCareTeamMemberDataRequest: IDataDomainRequest
    {
        [ApiMember(Name = "ContactId", Description = "Id of the contact whose care team needs to be updated.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContactId { get; set; }

        [ApiMember(Name = "CareTeamId", Description = "Id of the care team whose member needs to be updated.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string CareTeamId { get; set; }

        [ApiMember(Name = "CareTeamMemberData", Description = "care team member object to be updated", ParameterType = "property", DataType = "CareTeamMember", IsRequired = true)]
        public CareTeamMemberData CareTeamMemberData { get; set; }

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
