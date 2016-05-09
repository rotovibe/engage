using System.Collections.Generic;
using ServiceStack.ServiceHost;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Cohort.DTO.Referrals
{
    [Route("/api/{Context}/{Version}/{ContractNumber}/Patient/Referrals/Insert/All", "POST")]
    public class PostPatientsListReferralDefinitionRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientsReferralsList", Description = "Inbound Patients Referrals List", ParameterType = "body", DataType = "", IsRequired = true)]
        public List <PatientReferralsListEntityData> PatientsReferralsList { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Cohort", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "query", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Reason", Description = "Explains why this patient is being referred to a specialist", ParameterType = "body", DataType = "string" , IsRequired = true)]
        public string Reason { get; set; }

        [ApiMember(Name = "Name", Description = "Referred patient's name", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string Name { get; set; }

        [ApiMember(Name = "Description", Description = "Any commentary or other remarks can be stored here", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string Description { get; set; }

    }       // end class defintion
}           // end namespace definition
