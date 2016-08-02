using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO.Utilization
{
    [Api(Description = "A Request object to insert a patient note.")]
    [Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Notes/Utilizations", "POST")]
    public class PostPatientUtilizationRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "Id of the patient", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "Utilization", Description = "PatientUtilization being created", ParameterType = "body", DataType = "PatientNote", IsRequired = true)]
        public PatientUtilization Utilization { get; set; }
        
        public string UserId { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "Header", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        public PostPatientUtilizationRequest() { }
    }
}

     