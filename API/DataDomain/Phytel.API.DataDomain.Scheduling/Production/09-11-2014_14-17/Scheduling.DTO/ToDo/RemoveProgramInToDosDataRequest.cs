using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Scheduling.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Scheduling/ToDo/RemoveProgram/{ProgramId}/Update", "PUT")]
    public class RemoveProgramInToDosDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ToDoId", Description = "ToDoId", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ToDoId { get; set; }

        [ApiMember(Name = "ProgramId", Description = "ProgramId used in ToDos.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ProgramId { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientNote", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }
    }
}
