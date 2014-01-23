using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class GetAllTimeZonesDataResponse : IDomainResponse
   {
        public List<LookUpData> TimeZones { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
