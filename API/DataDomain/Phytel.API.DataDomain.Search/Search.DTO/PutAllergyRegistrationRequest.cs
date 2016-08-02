using Phytel.API.Common.CustomObject;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Search.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Search/AllergyIndex/Insert", "PUT")]
    public class PutAllergyRegistrationRequest : IDataDomainRequest
    {
        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "IdNamePair", Description = "New Allergy document to register in the search index.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public IdNamePair AllergyDocument { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Search", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }
    }
}
