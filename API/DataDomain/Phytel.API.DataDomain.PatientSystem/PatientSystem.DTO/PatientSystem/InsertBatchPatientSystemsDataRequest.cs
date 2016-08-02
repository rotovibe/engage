using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientSystem.DTO
{
    [Api("Inserts Patient System Data in bulk")]
    [Route("/{Context}/{Version}/{ContractNumber}/Batch/PatientSystems", "POST")]
    [Route("/PatientSystems", "POST")]
    public class InsertBatchPatientSystemsDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientSystemsData", Description = "List of PatientSystems that need to be inserted.", ParameterType = "property", DataType = "List<PatientSystemData>", IsRequired = true)]
        public List<PatientSystemData> PatientSystemsData { get; set; }

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
