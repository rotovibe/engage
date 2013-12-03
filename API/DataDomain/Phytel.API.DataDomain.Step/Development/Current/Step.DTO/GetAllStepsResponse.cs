using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Step.DTO
{
    public class GetAllStepsResponse : IDomainResponse
   {
        public List<Step> Steps { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
