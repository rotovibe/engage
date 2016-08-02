using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/patient/{PatientID}/cohortpatientview", "PUT")]
    public class PutCohortPatientViewDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientID", Description = "ID of the patient being created", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientID { get; set; }

        [ApiMember(Name = "LastName", Description = "Last name of the patient being created", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string LastName { get; set; }

        [ApiMember(Name = "SearchFields", Description = "Search fields of the patient being created", ParameterType = "property", DataType = "SearchFieldData", IsRequired = true)]
        public List<SearchFieldData> SearchFields { get; set; }

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