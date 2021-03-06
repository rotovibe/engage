﻿using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO
{
    [Api(Description = "A Request object to Update Contact Info .")]
    [Route("/{Context}/{Version}/{ContractNumber}/Contacts/{ContactId}/Sync", "PUT")]
    public class SyncContactInfoDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ContactInfo", Description = "ContactData to be inserted.", ParameterType = "body", DataType = "SyncContactInfoData", IsRequired = true)]
        public SyncContactInfoData ContactInfo { get; set; }

        [ApiMember(Name = "ContactId", Description = "ContactId to Update", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContactId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Contact", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }
    }
}
