using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient", "PUT")]
    public class PutUpdatePatientDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientData", Description = "Patient details who is being inserted or updated.", ParameterType = "property", DataType = "PatientData", IsRequired = true)]
        public PatientData PatientData { get; set; }

        [ApiMember(Name = "Insert", Description = "Indicates if the action is to create or update a patient", ParameterType = "path", DataType = "boolean", IsRequired = false)]
        public bool Insert { get; set; }

        [ApiMember(Name = "InsertDuplicate", Description = "Indicates if a duplicate record of the patient should be inserted.", ParameterType = "path", DataType = "boolean", IsRequired = false)]
        public bool InsertDuplicate { get; set; }

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