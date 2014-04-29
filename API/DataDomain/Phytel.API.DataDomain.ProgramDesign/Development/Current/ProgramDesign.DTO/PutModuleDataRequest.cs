using Phytel.API.Interface;
using ServiceStack.ServiceHost;
//using Phytel.API.DataDomain.Contact.DTO;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/ProgramDesign/Module", "PUT")]
    public class PutModuleDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Context", Description = "Product Context requesting the Module", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = false)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Name", Description = "Name for this module", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Name { get; set; }

        [ApiMember(Name = "ProgramId", Description = "Id for the parent Program for this module", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ProgramId { get; set; }
    }
}