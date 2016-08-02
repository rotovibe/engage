using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Api(Description = "A Request object to delete a patient utilization.")]
    [Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Notes/Utilizations/{Id}", "DELETE")]
    public class DeletePatientUtilizationRequest : IAppDomainRequest
    {
        [ApiMember(Name = "Id", Description = "Id of the utilization that needs to be deleted.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Id { get; set; }

        [ApiMember(Name = "PatientId", Description = "Id of the patient", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        public string UserId { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "header", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        public DeletePatientUtilizationRequest() { }
    }
}

     