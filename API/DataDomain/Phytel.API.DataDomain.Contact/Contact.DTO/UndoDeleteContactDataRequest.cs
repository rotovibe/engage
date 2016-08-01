using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Contact/UndoDelete", "PUT")]
    public class UndoDeleteContactDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Id", Description = "Contact Id to be un-deleted", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Id { get; set; }

        [ApiMember(Name = "List<ContactWithUpdatedRecentList>", Description = "Contact Ids having the deleted contact in their recentList.", ParameterType = "property", DataType = "List<ContactWithUpdatedRecentList>", IsRequired = false)]
        public List<ContactWithUpdatedRecentList> ContactWithUpdatedRecentLists { get; set; }
        
        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientNote", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }
    }
}
