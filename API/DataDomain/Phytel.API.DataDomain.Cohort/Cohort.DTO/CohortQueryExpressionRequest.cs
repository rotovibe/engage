using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Cohort.DTO
{
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/Cohort/{CohortId}/QueryExpression", "GET")]
    public class CohortQueryExpressionRequest : IDataDomainRequest
    {
        [ApiMember(Name = "CohortId", Description = "Cohort ID", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string CohortId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Cohort", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}
