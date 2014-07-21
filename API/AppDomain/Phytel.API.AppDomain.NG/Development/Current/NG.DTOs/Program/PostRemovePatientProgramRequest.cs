using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Api(Description = "A Request object to delete a patient.")]
    [Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Program/{Id}/Remove", "POST")]
    public class PostRemovePatientProgramRequest : IAppDomainRequest
    {
        [ApiMember(Name = "Id", Description = "Id of the program that needs to be removed.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Id { get; set; }

        [ApiMember(Name = "PatientId", Description = "Id of the patient whose program needs to be removed.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "ProgramName", Description = "Name of the program to be removed.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ProgramName { get; set; }

        [ApiMember(Name = "Reason", Description = "Reason for removing the program.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Reason { get; set; }

        [ApiMember(Name = "UserId", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        public PostRemovePatientProgramRequest() { }
    }
}

     