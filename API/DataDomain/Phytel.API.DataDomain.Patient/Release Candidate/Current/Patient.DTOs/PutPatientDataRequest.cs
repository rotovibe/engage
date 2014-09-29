using Phytel.API.Interface;
using ServiceStack.ServiceHost;
//using Phytel.API.DataDomain.Contact.DTO;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/Insert", "PUT")]
    public class PutPatientDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "FirstName", Description = "First Name of the patient being created", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string FirstName { get; set; }

        [ApiMember(Name = "LastName", Description = "Last name of the patient being created", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string LastName { get; set; }

        [ApiMember(Name = "MiddleName", Description = "Middle name of the patient being created", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string MiddleName { get; set; }

        [ApiMember(Name = "Suffix", Description = "Suffix of the patient being created", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Suffix { get; set; }

        [ApiMember(Name = "PreferredName", Description = "Preferred name of the patient being created", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PreferredName { get; set; }

        [ApiMember(Name = "Gender", Description = "Gender of the patient being created", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Gender { get; set; }

        [ApiMember(Name = "DOB", Description = "Date of birth of the patient being created", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string DOB { get; set; }

        [ApiMember(Name = "Background", Description = "Background of the patient being created", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Background { get; set; }

        [ApiMember(Name = "FullSSN", Description = "SSN value of the patient being created", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string FullSSN { get; set; }

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