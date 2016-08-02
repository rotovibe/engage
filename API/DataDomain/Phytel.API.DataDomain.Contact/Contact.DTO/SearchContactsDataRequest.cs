using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO
{
    [Api(Description = "A Request object to search details for list of contacts from the API.")]
    [Route("/{Context}/{Version}/{ContractNumber}/SearchContacts", "POST")]
    public class SearchContactsDataRequest : IDataDomainRequest
    {
        public List<string> ContactTypeIds { get; set; }

        [ApiMember(Name = "ContactStatuses", Description = "List of Contact Statuses to Search for ", ParameterType = "query", DataType = "ContactType", IsRequired = false)]
        public List<Status> ContactStatuses { get; set; }

        [ApiMember(Name = "ContactSubTypeIds", Description = "List of ContactSubTypeIds to Search for ", ParameterType = "body", DataType = "list of strings", IsRequired = false)]
        public List<string> ContactSubTypeIds { get; set; }

        [ApiMember(Name = "FirstName", Description = "FirstName", ParameterType = "query", DataType = "string", IsRequired = false)]
        public string FirstName { get; set; }

        [ApiMember(Name = "LastName", Description = "LastName", ParameterType = "query", DataType = "string", IsRequired = false)]
        public string LastName { get; set; }

        [ApiMember(Name = "Take", Description = "Number of contacts to return", ParameterType = "query", DataType = "int", IsRequired = false)]
        public int? Take { get; set; }

        [ApiMember(Name = "Skip", Description = "Number of contacts to skip", ParameterType = "query", DataType = "int", IsRequired = false)]
        public int Skip { get; set; }

        [ApiMember(Name = "FilterType", Description = "Indicates the filter for search (StartsWith or ExactMatch)", ParameterType = "query", DataType = "FilterType", IsRequired = false)]
        [ApiAllowableValues("FilterType", typeof(DTO.FilterType))]
        public FilterType FilterType { get; set; } 

        #region IDataDomainRequest Members

        [ApiMember(Name = "Context", Description = "Product Context requesting the Contact", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        #endregion
    }
}
