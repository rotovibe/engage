using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;

namespace Phytel.API.AppDomain.NG.DTO.Search
{
    [Route("/{Version}/{ContractNumber}/Search/Meds/Fields", "GET")]
    public class GetMedFieldsRequest : IAppDomainRequest
    {
        [ApiMember(Name = "ProprietaryName", Description = "proprietary Name to search data for.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string NameParam { get; set; }

        [ApiMember(Name = "Type", Description = "Type of type if applicable.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string TypeParam { get; set; }

        [ApiMember(Name = "Route", Description = "Route if applicable.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string RouteParam { get; set; }

        [ApiMember(Name = "Dosage", Description = "Dose if applicable.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string DoseParam { get; set; }

        [ApiMember(Name = "Strength", Description = "Strength of dose if applicable.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string StrengthParam { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Program", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }
    }
}
