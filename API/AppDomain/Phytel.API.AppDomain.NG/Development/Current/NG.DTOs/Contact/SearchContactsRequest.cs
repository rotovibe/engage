using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/SearchContacts", "POST")]
    public class SearchContactsRequest : IAppDomainRequest
    {

         public List<string> ContactTypeIds { get; set; }

        //[ApiMember(Name = "ContactTypes", Description = "List of ContactTypes to Search for ", ParameterType = "query", DataType = "ContactType", IsRequired = false)]
        //public List<ContactType> ContactTypes { get; set; }

        [ApiMember(Name = "ContactStatuses", Description = "List of ContactStatuses to Search for ", ParameterType = "query", DataType = "ContactStatus", IsRequired = false)]
        public List<ContactStatus> ContactStatuses { get; set; }

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


        #region IAppDomainRequest Members

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        #endregion
    }

    public enum ContactType
    {
        Person = 1,
       // Organization = 2
    }

    public enum ContactStatus
    {
        Active = 1,
        Inactive = 2,
        Archived = 3
    }
}
