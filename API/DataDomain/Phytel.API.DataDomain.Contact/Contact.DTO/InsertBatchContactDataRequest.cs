using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Contact.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Batch/Contacts", "POST")]
    public class InsertBatchContactDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ContactsData", Description = "List of Contacts to be inserted, if they exist, update them.", ParameterType = "property", DataType = "List<ContactData>", IsRequired = true)]
        public List<ContactData> ContactsData { get; set; }

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
