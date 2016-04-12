using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO.CareTeam
{
    [Api(Description = "A Request object to insert a CareTeam for a contact")]
    [Route("/{Context}/{Version}/{ContractNumber}/Contacts/{ContactId}/CareTeams", "POST")]
    public class SaveCareTeamDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ContactData", Description = "ContactData to be inserted.", ParameterType = "body", DataType = "string", IsRequired = true)]
        public CareTeamData CareTeamData { get; set; }

        [ApiMember(Name = "ContactId", Description = "ContactId", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContactId { get; set; }

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
