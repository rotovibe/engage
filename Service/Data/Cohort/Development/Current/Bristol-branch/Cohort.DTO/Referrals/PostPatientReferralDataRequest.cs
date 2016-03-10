using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Cohort.DTO.Referrals
{
    [Route("/{Context}/{Version}/{ContractNumber}/Cohorts/PatientReferrals", "POST")]
    public class PostPatientReferralDefinitionRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientReferral", Description = "PatientReferral entity", ParameterType = "property", DataType = "PatientReferralData", IsRequired = true)]
        public PatientReferralData PatientReferral { get; set; }

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
