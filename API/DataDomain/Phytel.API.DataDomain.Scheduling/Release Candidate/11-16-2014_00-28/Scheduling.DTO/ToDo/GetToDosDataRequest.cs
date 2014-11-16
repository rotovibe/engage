using System;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Scheduling.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Scheduling/ToDos", "POST")]
    public class GetToDosDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "AssignedToId", Description = "AssignedToId is the Id to which ToDo is assigned to.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string AssignedToId { get; set; }

        [ApiMember(Name = "CreatedById", Description = "CreatedById is the Id of the user who created the ToDo.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string CreatedById { get; set; }

        [ApiMember(Name = "PatientId", Description = "PatientId is the Id of the patient for whom a todo is associated.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PatientId { get; set; }

        [ApiMember(Name = "FromDate", Description = "Get the Todos having ClosedDate greater than or equal to FromDate.", ParameterType = "property", DataType = "DateTime", IsRequired = false)]
        public DateTime? FromDate { get; set; }

        [ApiMember(Name = "StatusIds", Description = "List of ToDo Status ids.", ParameterType = "property", DataType = "List<int>", IsRequired = false)]
        public List<int> StatusIds { get; set; }
        
        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the ToDo", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }
    }
}
