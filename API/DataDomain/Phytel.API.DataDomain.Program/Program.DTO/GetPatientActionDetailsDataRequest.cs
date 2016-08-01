using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Program.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Program/{PatientProgramId}/Module/{PatientModuleId}/Action/{PatientActionId}", "GET")]
    public class GetPatientActionDetailsDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "PatientId of the contexted user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "PatientProgramId", Description = "ID of the program to patient registration", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientProgramId { get; set; }

        [ApiMember(Name = "PatientModuleId", Description = "ID of the module in patientprogram", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientModuleId { get; set; }

        [ApiMember(Name = "PatientActionId", Description = "ID of the action in patientprogram", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientActionId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Program", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the contexted user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }
    }
}
