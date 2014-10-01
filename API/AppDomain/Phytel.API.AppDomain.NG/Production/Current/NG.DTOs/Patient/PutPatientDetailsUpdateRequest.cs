using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/patient/Update/", "POST")]
    public class PutPatientDetailsUpdateRequest : IAppDomainRequest
    {
        [ApiMember(Name = "Patient", Description = "Patient details that need to be upserted.", ParameterType = "property", DataType = "Patient", IsRequired = true)]
        public Patient Patient { get; set; }

        [ApiMember(Name = "Insert", Description = "Indicates if the action is to create or update a patient", ParameterType = "path", DataType = "boolean", IsRequired = false)]
        public bool Insert { get; set; }

        [ApiMember(Name = "InsertDuplicate", Description = "Indicates if a duplicate record of the patient should be inserted.", ParameterType = "path", DataType = "boolean", IsRequired = false)]
        public bool InsertDuplicate { get; set; }

        [ApiMember(Name = "Token", Description = "Token parameter", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        public PutPatientDetailsUpdateRequest() { }
    }
}