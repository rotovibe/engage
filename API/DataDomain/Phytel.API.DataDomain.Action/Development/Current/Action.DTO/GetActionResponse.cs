using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Action.DTO
{
    public class GetActionResponse : IDomainResponse
    {
        public Action Action { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Action
    {
        public string ActionID { get; set; }
        public string Version { get; set; }
    }
}
