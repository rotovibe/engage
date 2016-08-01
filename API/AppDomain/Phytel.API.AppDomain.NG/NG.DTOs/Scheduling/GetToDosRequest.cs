using System;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/Scheduling/ToDos", "POST")]
    public class GetToDosRequest : IAppDomainRequest, ISortableRequest
    {
        [ApiMember(Name = "AssignedToId", Description = "AssignedToId is the Id to which ToDo is assigned to.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string AssignedToId { get; set; }

        [ApiMember(Name = "NotAssignedToId", Description = "NotAssignedToId is the Id to which ToDo is NOT assigned to.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string NotAssignedToId { get; set; }

        [ApiMember(Name = "CreatedById", Description = "CreatedById is the Id of the user who created the ToDo.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string CreatedById { get; set; }

        [ApiMember(Name = "PatientId", Description = "PatientId is the Id of the patient for whom a todo is associated.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PatientId { get; set; }

        [ApiMember(Name = "FromDate", Description = "Get the Todos having ClosedDate greater than or equal to FromDate.", ParameterType = "property", DataType = "DateTime", IsRequired = false)]
        public DateTime? FromDate { get; set; }

        [ApiMember(Name = "Skip", Description = "Skip - records to skip. used with Take", ParameterType = "property", DataType = "int", IsRequired = false)]
        public int Skip { get; set; }

        [ApiMember(Name = "Take", Description = "Take - number of records. used with Skip.", ParameterType = "property", DataType = "int", IsRequired = false)]
        public int Take { get; set; }

        [ApiMember(Name = "Sort", Description = "Sort by mongo entity property name. to define sort direction prepend +/- (asc/desc) ", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Sort { get; set; }

        [ApiMember(Name = "StatusIds", Description = "List of ToDo Status ids.", ParameterType = "property", DataType = "List<int>", IsRequired = false)]
        public List<int> StatusIds { get; set; }
        
        [ApiMember(Name = "PriorityIds", Description = "List of ToDo Priority ids.", ParameterType = "property", DataType = "List<int>", IsRequired = false)]
        public List<int> PriorityIds { get; set; }

        [ApiMember(Name = "CategoryIds", Description = "List of ToDo Category ids.", ParameterType = "property", DataType = "List<int>", IsRequired = false)]
        public List<string> CategoryIds { get; set; }
        
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
