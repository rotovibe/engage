using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/ToDo/AssignedTo/{AssignedToId}/Patient/{PatientId}/Status/{StatusIds}", "GET")]
    public class GetToDosRequest : IAppDomainRequest
    {
        [ApiMember(Name = "AssignedToId", Description = "AssignedToId is the Id to which ToDo is assigned to.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string AssignedToId { get; set; }

        [ApiMember(Name = "PatientId", Description = "PatientId is the Id of the patient for whom a todo is associated.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PatientId { get; set; }

        [ApiMember(Name = "StatusIds", Description = "Comma separated Status ids of the ToDos", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string StatusIds { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        public GetToDosRequest() { }
    }
}
