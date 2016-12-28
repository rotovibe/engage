using Phytel.API.Interface;
using ServiceStack.ServiceHost;
//using Phytel.API.DataDomain.Contact.DTO;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/PatientData", "GET")]
    public class GetPatientDataByNameDOBRequest : IDataDomainRequest
    {
        [ApiMember(Name = "FirstName", Description = "Patient First Name", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string FirstName { get; set; }

        [ApiMember(Name = "LastName", Description = "Patient Last Name", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string LastName { get; set; }

        [ApiMember(Name = "DateOfBirth", Description = "Patient Date of Birth", ParameterType = "property", DataType = "DateTime", IsRequired = true)]
        public string DOB { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context creating the patient", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}