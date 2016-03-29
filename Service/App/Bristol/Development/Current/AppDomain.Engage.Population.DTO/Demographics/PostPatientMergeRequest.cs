using System;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace AppDomain.Engage.Population.DTO.Demographics
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patients/Patient-Merge", "POST")]
    public class PostPatientMergeRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientSystemIds", Description = "Patients EMR source id list", ParameterType = "Body", DataType = "List<string>", IsRequired = true)]
        public List<string> PatientSystemIds { get; set; }

        [ApiMember(Name = "SIDs", Description = "SIDs list", ParameterType = "Body", DataType = "List<string>", IsRequired = true)]
        public List<string> SIDs { get; set; }

        [ApiMember(Name = "Event", Description = "SplitFragment = 1,Expand = 2, Merge = 3", ParameterType = "Body", DataType = "MatchEvent", IsRequired = true)]
        public int Event { get; set; }

        // IAppDomainRequest implementation
        [ApiMember(Name = "Context", Description = "Product Context", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "Version", Description = "API Version", ParameterType = "path", DataType = "string", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract name", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Security Token", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string Token { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId assigned", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}