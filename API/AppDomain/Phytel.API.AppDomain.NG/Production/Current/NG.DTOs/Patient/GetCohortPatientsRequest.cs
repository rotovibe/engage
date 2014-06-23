using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/cohortpatients/{CohortId}", "GET")]
    public class GetCohortPatientsRequest : IAppDomainRequest
    {
        [ApiMember(Name = "ContractNumber", Description = "Contract parameter will be defined in the route.", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "SearchFilter", Description = "Filter Text to limit results", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string SearchFilter { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "header", DataType = "string", IsRequired = true)]        
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the Request", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "CohortID", Description = "Cohort Identifier", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string CohortID { get; set; }

        [ApiMember(Name = "Skip", Description = "Position to skip to define the start of a set of records.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Skip { get; set; }

        [ApiMember(Name = "Take", Description = "Number of records to return per request.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Take { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        public GetCohortPatientsRequest() { }
    }
}
