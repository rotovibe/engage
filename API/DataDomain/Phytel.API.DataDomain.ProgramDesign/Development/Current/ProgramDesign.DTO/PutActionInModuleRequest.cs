using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/ProgramDesign/Module/{ModuleId}/Action/{ActionId}", "PUT")]
    public class PutActionInModuleRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ModuleId", Description = "Id for the parent Module for this Action", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ModuleId { get; set; }

        [ApiMember(Name = "ActionId", Description = "Id for the Action being added to Module", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ActionId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Module", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = false)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}