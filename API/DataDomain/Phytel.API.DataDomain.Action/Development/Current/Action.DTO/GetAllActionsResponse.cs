using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Action.DTO
{
    public class GetAllActionsResponse : IDomainResponse
   {
        public List<Action> Actions { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
