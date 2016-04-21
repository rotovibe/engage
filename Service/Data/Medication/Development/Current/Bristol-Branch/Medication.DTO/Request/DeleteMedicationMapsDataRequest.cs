using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Medication.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/MedicationMap/{Ids}", "DELETE")]
    public class DeleteMedicationMapsDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Ids", Description = "Comma limited MedicationMap Ids that need to be deleted.", ParameterType = "Path", DataType = "string", IsRequired = true)]
        public string Ids { get; set; }

        [ApiMember(Name = "Id", Description = "MedicationMap Id.", ParameterType = "Body", DataType = "string", IsRequired = false)]
        public string Id { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientSystem", ParameterType = "Path", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "Path", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "Path", DataType = "double", IsRequired = false)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "Query", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}
