using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/patientproblems", "POST")]
    public class PatientProblemRequest: IDataDomainRequest
    {

        [ApiMember(Name = "PatientID", Description = "ID of the patient being requested", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string PatientID { get; set; }

        [ApiMember(Name = "Status", Description = "Status of the problem being requested", ParameterType = "body", DataType = "string", IsRequired = false)]
        public string Status { get; set; }

        [ApiMember(Name = "Category", Description = "Category of the problem being requested", ParameterType = "body", DataType = "string", IsRequired = false)]
        public string Category { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the patient", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the Data Domain API being called", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Version { get; set; }

    }
}
