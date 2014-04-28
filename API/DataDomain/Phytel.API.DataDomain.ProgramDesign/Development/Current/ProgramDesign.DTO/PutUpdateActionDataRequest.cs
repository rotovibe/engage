using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/ProgramDesign/Action/{ActionId}/Update", "PUT")]
    public class PutUpdateActionDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "ActionId", Description = "Id of the Action being updated", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ActionId { get; set; }

        [ApiMember(Name = "Name", Description = "Name of the Action being updated", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Name { get; set; }

        [ApiMember(Name = "Description", Description = "Description of the Action being updated", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Description { get; set; }

        [ApiMember(Name = "CompletedBy", Description = "Name of who completed the Action being updated", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string CompletedBy { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Action", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }
    }
}
