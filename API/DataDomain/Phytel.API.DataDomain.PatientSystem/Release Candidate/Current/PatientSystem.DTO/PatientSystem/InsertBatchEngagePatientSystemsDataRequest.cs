using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientSystem.DTO
{
    [Api("Inserts Engage Patient System Ids in bulk.")]
    [Route("/{Context}/{Version}/{ContractNumber}/Batch/Engage/PatientSystems", "POST")]
    public class InsertBatchEngagePatientSystemsDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientIds", Description = "List of Patient Ids for which Engage Patient system need to be inserted.", ParameterType = "property", DataType = "List<string>", IsRequired = true)]
        public List<string> PatientIds { get; set; }

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
