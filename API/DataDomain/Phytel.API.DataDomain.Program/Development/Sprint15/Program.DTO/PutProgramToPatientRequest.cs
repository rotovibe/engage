using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Program.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Programs/", "PUT")]
    public class PutProgramToPatientRequest : IDataDomainRequest
    {
        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "CareManagerId", Description = "ContactId of primary care manager.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string CareManagerId { get; set; }

        [ApiMember(Name = "PatientId", Description = "PatientId", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PatientId { get; set; }

        [ApiMember(Name = "ContractProgramId", Description = "Id of the contractprogram to assign to patient", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractProgramId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Program", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }
    }
}
