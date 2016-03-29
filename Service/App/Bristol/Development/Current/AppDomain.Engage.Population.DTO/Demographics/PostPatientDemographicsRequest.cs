using System;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace AppDomain.Engage.Population.DTO.Demographics
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patients/Demographics", "POST")]
    public class PostPatientDemographicsRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientList", Description = "Patients list", ParameterType = "Body", DataType = "List<Patient>", IsRequired = true)]
        public List<Patient> PatientList { get; set; }

        // IAppDomainRequest implementation
        [ApiMember(Name = "Context", Description = "Product Context", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

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