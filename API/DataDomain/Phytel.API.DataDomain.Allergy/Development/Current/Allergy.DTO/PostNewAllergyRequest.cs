using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Allergy", "PUT")]
    public class PostNewAllergyRequest : IDataDomainRequest
    {
        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Description", Description = "Description of new allergy registration", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Description { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the DdAllergy", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }
    }
}
