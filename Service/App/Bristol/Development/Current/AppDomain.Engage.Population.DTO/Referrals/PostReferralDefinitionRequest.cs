using System;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace AppDomain.Engage.Population.DTO.Referrals
{
    public class PostReferralDefinitionRequest : IAppDomainRequest
    {
        [ApiMember(Name = "", Description = "", ParameterType = "", DataType = "", IsRequired = true)]
        public string PatientListName { get; set; }

        [ApiMember(Name = "", Description = "", ParameterType = "", DataType = "", IsRequired = true)]
        public string PatientListKey { get; set; }

        [ApiMember(Name = "", Description = "", ParameterType = "", DataType = "", IsRequired = true)]
        public string PatientListDescription { get; set; }

        [ApiMember(Name = "", Description = "", ParameterType = "", DataType = "", IsRequired = true)]
        public string ReferralReason { get; set; }

        [ApiMember(Name = "", Description = "", ParameterType = "", DataType = "", IsRequired = true)]
        public DateTime CreatedOn { get; set; }

        [ApiMember(Name = "", Description = "", ParameterType = "", DataType = "", IsRequired = true)]
        public string CreatedBy { get; set; }

        [ApiMember(Name = "", Description = "", ParameterType = "", DataType = "", IsRequired = true)]
        public DateTime UpdatedOn { get; set; }

        [ApiMember(Name = "", Description = "", ParameterType = "", DataType = "", IsRequired = true)]
        public string UpdatedBy { get; set; }

        [ApiMember(Name = "", Description = "", ParameterType = "", DataType = "", IsRequired = true)]
        public string AssociatedContract { get; set; }

        [ApiMember(Name = "", Description = "", ParameterType = "", DataType = "", IsRequired = true)]
        public string ApplicationId { get; set; }

        [ApiMember(Name = "Version", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "UserId", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }
    }
}
