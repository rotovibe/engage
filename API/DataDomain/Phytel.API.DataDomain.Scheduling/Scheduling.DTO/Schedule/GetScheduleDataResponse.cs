using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Scheduling.DTO
{
    public class GetScheduleDataResponse : IDomainResponse
   {
        public ScheduleData Schedule { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
   }
}
