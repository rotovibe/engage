using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/ProgramDesign/Program/{Id}/Update", "PUT")]
    public class PutUpdateProgramDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Name", Description = "Name of the Program being updated", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Name { get; set; }

        [ApiMember(Name = "Id", Description = "Id of the Program being updated", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Id { get; set; }

        [ApiMember(Name = "Description", Description = "Description of the Program being updated", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Description { get; set; }

        [ApiMember(Name = "ShortName", Description = "Short name of the Program being updated", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ShortName { get; set; }

        [ApiMember(Name = "AssignedBy", Description = "Name of who assigned the Program being updated", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string AssignedBy { get; set; }

        [ApiMember(Name = "AssignedOn", Description = "Date assigned to the Program being updated", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string AssignedOn { get; set; }

        [ApiMember(Name = "Client", Description = "Client of the Program being updated", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Client { get; set; }

        [ApiMember(Name = "Modules", Description = "Modules of the Program being updated", ParameterType = "property", DataType = "Module", IsRequired = false)]
        public List<Module> Modules { get; set; }

        [ApiMember(Name = "Order", Description = "Order of the Program being updated", ParameterType = "property", DataType = "int", IsRequired = false)]
        public int Order { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Program", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }
    }
}