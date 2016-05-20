using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO.CareTeam
{
    [Route("/{Context}/{Version}/{ContractNumber}/Contacts/{ContactId}/CareTeams/{Id}", "PUT")]
    [Api(Description = "A Request object to delete a care team.")]
    public class UndoDeleteCareTeamDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ContactId", Description = "Id of the contact whose care team needs to be undeleted.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContactId { get; set; }

        [ApiMember(Name = "Id", Description = "Id of the care team that needs to be undeleted.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Id { get; set; }

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
