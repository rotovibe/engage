using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using Phytel.API.Common;
using Phytel.API.DataDomain.ProgramDesign;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    public class GetModuleResponse : IDomainResponse
    {
        public Module Module { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Module
    {
        public string Id { get; set; }
        public double Version { get; set; }
        public string Name { get; set; }
        public List<string> Objectives { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public List<ActionData> Actions { get; set; }
    }
}
