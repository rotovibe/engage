using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Medication.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/{PatientId}", "POST")]
    public class GetPatientMedSuppsDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "ID of the patient who's MedSupps are being requested", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PatientId { get; set; }

        [ApiMember(Name = "StatusIds", Description = "List of Status ids.", ParameterType = "property", DataType = "List<int>", IsRequired = false)]
        public List<int> StatusIds { get; set; }

        [ApiMember(Name = "CategoryIds", Description = "List of Category Ids.", ParameterType = "property", DataType = "List<int>", IsRequired = false)]
        public List<int> CategoryIds { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }
    }
}
