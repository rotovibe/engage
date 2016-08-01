using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Api(Description = "A Request object to Update Patient Info .")]
    [Route("/{Context}/{Version}/{ContractNumber}/Patients/{PatientId}/Sync", "PUT")]
    public class SyncPatientInfoDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientInfo", Description = "PatientInfo to be inserted.", ParameterType = "body", DataType = "PatientInfoData", IsRequired = true)]
        public SyncPatientInfoData PatientInfo { get; set; }

        [ApiMember(Name = "PatientId", Description = "Patient to Update", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Contact", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }
    }
}
