using System.Collections.Generic;

namespace Phytel.API.DataDomain.Action.DTO
{
    public class ActionListResponse
   {
        public List<ActionResponse> Actions { get; set; }
        public string Version { get; set; }
    }

}
