using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Medication.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/MedicationMap/{Name}", "GET")]
    public class GetMedicationMapDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Name", Description = "Full Name of the medication map that is being requested", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Name { get; set; }

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
