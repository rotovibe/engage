using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/PatientAllergy/UndoDelete", "PUT")]
    public class UndoDeletePatientAllergiesDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Ids", Description = "PatientAllergy Ids that need to be un-deleted.", ParameterType = "property", DataType = "List<string>", IsRequired = true)]
        public List<string> Ids { get; set; }

        [ApiMember(Name = "PatientAllergyId", Description = "PatientAllergy Id", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PatientAllergyId { get; set; }

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
