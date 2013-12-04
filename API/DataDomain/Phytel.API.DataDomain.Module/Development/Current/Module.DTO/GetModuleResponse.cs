using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Module.DTO
{
    public class GetModuleResponse : IDomainResponse
    {
        public Module Module { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Module
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public string Objective { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
