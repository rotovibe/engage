using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Cohort.DTO
{
    [Route("/api/{Context}/{Version}/{ContractNumber}/Referral/{ReferralID}", "GET")]
    public class GetReferralDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ReferralID", Description = "ReferralID is a Mongo ObjectId. eg:528aa055d4332317acc50978", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ReferralID { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Referral", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }
    }
}
