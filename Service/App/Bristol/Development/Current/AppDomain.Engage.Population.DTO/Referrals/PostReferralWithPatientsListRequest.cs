using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using AppDomain.Engage.Population.DTO.Demographics;
using System.Collections.Generic;


namespace AppDomain.Engage.Population.DTO.Referrals
{
    [Route("/api/{Context}/{Version}/{ContractNumber}/ReferralWithPatientsList", "POST")]
    public class PostReferralWithPatientsListRequest : IAppDomainRequest
    {
        [ApiMember(Name = "ReferralDefinition", Description = "Referral entity", ParameterType = "property", DataType = "ReferralData", IsRequired = true)]
        public ReferralDefinitionData ReferralDefinitionData { get; set; }

        [ApiMember(Name = "PatientsData", Description = "List of patients to be inserted", ParameterType = "property", DataType = "List<Patient>", IsRequired = true)]
        public List<Patient> PatientsData { get; set; }

        //create patient referral

        // IAppDomainRequest implementation
        [ApiMember(Name = "Context", Description = "Product Context requesting the patient list", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "Token", Description = "Token", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

    }
}
