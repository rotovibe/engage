using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/ProgramDesign/Program/{ProgramId}/Module/{ModuleId}", "PUT")]
    public class PutModuleInProgramRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ModuleId", Description = "Id for the Module being added to Program", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ModuleId { get; set; }

        [ApiMember(Name = "ProgramId", Description = "Id for the parent Program for this module", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ProgramId { get; set; }

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