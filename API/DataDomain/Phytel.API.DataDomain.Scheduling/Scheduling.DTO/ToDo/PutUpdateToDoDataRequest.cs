using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Scheduling.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Scheduling/ToDo/Update", "PUT")]
    public class PutUpdateToDoDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ToDoData", Description = "ToDoData object to be updated", ParameterType = "property", DataType = "ToDoData", IsRequired = true)]
        public ToDoData ToDoData { get; set; }

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
