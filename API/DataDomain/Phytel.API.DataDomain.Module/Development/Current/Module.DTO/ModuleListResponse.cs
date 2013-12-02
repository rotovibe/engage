using System.Collections.Generic;

namespace Phytel.API.DataDomain.Module.DTO
{
    public class ModuleListResponse
   {
        public List<ModuleResponse> Modules { get; set; }
        public string Version { get; set; }
    }

}
