using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/CohortPatients/{CohortID}", "GET")]
    public class GetCohortPatientsDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "CohortID", Description = "CohortID", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string CohortID { get; set; }

        [ApiMember(Name = "SearchFilter", Description = "Filter Text to limit results", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string SearchFilter { get; set; }

        [ApiMember(Name = "Skip", Description = "number of records to skip.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public int Skip { get; set; }

        [ApiMember(Name = "Take", Description = "number of records to retrieve.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public int Take { get; set; }

        [ApiMember(Name = "Context", Description = "Context", ParameterType = "body", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the Data Domain API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}
