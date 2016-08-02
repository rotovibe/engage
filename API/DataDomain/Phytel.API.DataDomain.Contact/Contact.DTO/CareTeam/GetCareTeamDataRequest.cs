using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO.CareTeam
{
    [Api(Description = "A Request object to get the CareTeam for a contact")]
    [Route("/{Context}/{Version}/{ContractNumber}/Contacts/{ContactId}/CareTeams", "GET")]
    public class GetCareTeamDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ContactId", Description = "Id of the contact whose care team needs to be fetched.", ParameterType = "path", DataType = "string", IsRequired = true)]
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
