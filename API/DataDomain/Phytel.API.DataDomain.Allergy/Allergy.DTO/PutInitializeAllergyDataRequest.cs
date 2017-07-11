using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Allergy/Initialize", "PUT")]
    public class PutInitializeAllergyDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "AllergyName", Description = "Name of the allergy which is getting initialized", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string AllergyName { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientGoal", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }
    }
}