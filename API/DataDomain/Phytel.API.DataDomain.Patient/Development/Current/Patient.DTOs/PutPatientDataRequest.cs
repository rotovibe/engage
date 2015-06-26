using Phytel.API.Interface;
using ServiceStack.ServiceHost;
//using Phytel.API.DataDomain.Contact.DTO;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/Insert", "PUT")]
    public class PutPatientDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Patient", Description = "Patient to insert", ParameterType = "property", DataType = "string", IsRequired = true)]
        public PatientData Patient { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context creating the patient", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}