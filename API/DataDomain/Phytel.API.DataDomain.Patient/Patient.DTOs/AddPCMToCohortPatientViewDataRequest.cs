using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Api]
    [Route("/{Context}/{Version}/{ContractNumber}/CohortPatientView/Patients/{Id}/PCM", "PUT")]
    public class AddPCMToCohortPatientViewDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Id", Description = "Id of the patient to be deleted", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Id { get; set; }

        [ApiMember(Name = "ContactId", Description = "ContactId", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContactIdToAdd { get; set; }

        [ApiMember(Name = "ActiveCorePcmIsUser", Description = "ActiveCorePcmIsUser", ParameterType = "property", DataType = "bool", IsRequired = true)]
        public bool ActiveCorePcmIsUser { get; set; }

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
