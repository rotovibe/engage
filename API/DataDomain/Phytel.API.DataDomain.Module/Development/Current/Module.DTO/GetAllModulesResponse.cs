using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Module.DTO
{
    public class GetAllModulesResponse : IDomainResponse
   {
        public List<Module> Modules { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
