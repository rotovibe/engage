using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Cohort.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Cohort/{CohortID}", "GET")]
    public class GetCohortDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "CohortID", Description = "ID of the Cohort being requested", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string CohortID { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Cohort", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}
