using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientNote.DTO.Request.Utilization
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Notes/Utilizations/", "POST")]
    public class PostPatientUtilizationDataRequest : IDataDomainRequest
    {

        [ApiMember(Name = "PatientId", Description = "Id of the patient", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "PatientUtilization", Description = "PatientUtilization object to be inserted", ParameterType = "property", DataType = "PatientNoteData", IsRequired = false)]
        public PatientUtilizationData PatientUtilization { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientNote", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }
    }
}
