using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Cohort.DTO.Referrals
{
    [Route("/api/{Context}/{Version}/{ContractNumber}/Referrals", "POST")]
    public class PostReferralDefinitionRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Referral", Description = "Json should look like { 'Referral' : { put body here }}", ParameterType = "body", DataType = "ReferralData", IsRequired = true)]
        public ReferralData Referral { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Cohort", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }
    }
}
