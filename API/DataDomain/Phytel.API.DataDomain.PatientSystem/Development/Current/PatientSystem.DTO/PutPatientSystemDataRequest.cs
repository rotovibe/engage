using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientSystem.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/PatientSystem", "PUT")]
    public class PutPatientSystemDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "SystemID", Description = "System ID of the PatientSystem being requested", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string SystemID { get; set; }

        [ApiMember(Name = "PatientID", Description = "Patient ID of the PatientSystem being requested", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PatientID { get; set; }

        [ApiMember(Name = "DisplayLabel", Description = "Label of the PatientSystem source being requested", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string DisplayLabel { get; set; }

        [ApiMember(Name = "SystemName", Description = "System name of the PatientSystem being requested", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string SystemName { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientSystem", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}
