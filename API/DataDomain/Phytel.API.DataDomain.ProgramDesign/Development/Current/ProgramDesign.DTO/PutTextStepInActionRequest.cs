using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/ProgramDesign/Action/{ActionId}/Text/{StepId}", "PUT")]
    public class PutTextStepInActionRequest : IDataDomainRequest
    {
        [ApiMember(Name = "StepId", Description = "Id for the Step being added to Action", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string StepId { get; set; }

        [ApiMember(Name = "ActionId", Description = "Id for the parent Action for this Step", ParameterType = "property", DataType = "string", IsRequired = true)]
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