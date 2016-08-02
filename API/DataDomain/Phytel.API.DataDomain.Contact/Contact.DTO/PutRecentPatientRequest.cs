using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO
{
    [Api(Description = "A Request object to update the contact card details.")]
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/Contact/Recent", "PUT")]
    public class PutRecentPatientRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ContactId", Description = "ID of the Contact being updated", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContactId { get; set; }

        [ApiMember(Name = "PatientId", Description = "ID of the patient being put on the stack", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Contact", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }
    }
}

